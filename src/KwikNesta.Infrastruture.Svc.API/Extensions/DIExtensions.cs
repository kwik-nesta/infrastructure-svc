using CrossQueue.Hub.Models;
using CrossQueue.Hub.Shared.Extensions;
using DiagnosKit.Core.Configurations;
using DiagnosKit.Core.Extensions;
using DRY.MailJetClient.Library.Extensions;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using KwikNesta.Contracts.Settings;
using KwikNesta.Infrastruture.Svc.Application.Common.Interfaces;
using KwikNesta.Infrastruture.Svc.Infrastructure.Persistence;
using KwikNesta.Infrastruture.Svc.Infrastructure.Repositories;
using KwikNesta.Infrastruture.Svc.Worker;
using KwikNesta.Mediatrix.Core.Abstractions;
using KwikNesta.Mediatrix.Core.Extensions;
using KwikNesta.Mediatrix.Core.Implementations.Pipelines;
using KwikNesta.Mediatrix.Hangfire.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KwikNesta.Infrastruture.Svc.API.Extensions
{
    public static class DIExtensions
    {
        public static void RegisterServices(this IServiceCollection services, 
                                            IConfiguration configuration,
                                            string serviceName)
        {
            services.RegisterRepository()
                .ConfigureSwagger()
                .ConfigureApiVersion()
                .ConfigureJwt(configuration)
                .ConfigureKwikMediator()
                .ConfigureHangfire(configuration)
                .RegisterDbContext(configuration)
                .ConfigureRefit(configuration)
                .RegisterWorkers()
                .ConfigureCors(configuration)
                .AddCrossQueueHubRabbitMqBus(opt =>
                {
                    var settings = configuration.GetSection("RabbitMQ")
                        .Get<QueueSettings>() ?? throw new ArgumentNullException("RabbitMQ");
                    opt.RabbitMQ = new RabbitMQOptions
                    {
                        ConnectionString = settings.ConnectionString,
                        ConsumerRetryCount = 5,
                        ConsumerRetryDelayMs = 500,
                        Durable = true,
                        PublishRetryCount = 5,
                        PublishRetryDelayMs = 500,
                        DefaultExchangeType = settings.ExchangeType,
                        DeadLetterExchange = settings.DeadLetterExchange,
                        DefaultExchange = settings.Exchange
                    };
                })
                .ConfigureRefit(configuration)
                .AddDiagnosKitObservability(serviceName: serviceName, serviceVersion: "1.0.0")
                .ConfigureMailJet(configuration)
                .AddLoggerManager();
        }

        public static void ConfigureESSink(this IHostBuilder host, 
                                           IConfiguration configuration)
        {
            host.ConfigureSerilogESSink(opt =>
            {
                var settings = configuration.GetSection("ElasticSearch")
                    .Get<ElasticSettings>() ?? throw new ArgumentNullException("ElasticSearch");

                opt.Url = settings.Url;
                opt.Username = settings.UserName;
                opt.Password = settings.Password;
                opt.IndexPrefix = settings.IndexPrefix;
                opt.IndexFormat = settings.IndexFormat;
            });
        }

        private static IServiceCollection ConfigureKwikMediator(this IServiceCollection services)
        {
            services
                .AddKwikMediators(typeof(Program).Assembly)
                .AddTransient(typeof(IKwikPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddTransient(typeof(IKwikNotificationBehavior<>), typeof(NotificationLoggingBehavior<>))
                .AddKwikBackgroundMediators();
            return services;
        }

        private static IServiceCollection ConfigureCors(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            var gatewayUrl = configuration.GetSection("ServiceUrls")
                .Get<ServiceUrls>()?.GatewayService ??
                throw new ArgumentNullException("ServiceUrls");

            services.AddCors(options =>
            {
                options.AddPolicy("GatewayOnly", policy =>
                {
                    policy.WithOrigins(gatewayUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            return services;
        }

        private static IServiceCollection ConfigureHangfire(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(opt =>
                    {
                        opt.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
                    })
                    .UseConsole()
                    .UseFilter(new AutomaticRetryAttribute()
                    {
                        Attempts = 5,
                        DelayInSecondsByAttemptFunc = _ => 60
                    });
            }).AddHangfireServer(opt =>
            {
                opt.ServerName = "Kwik Nesta Infrastructure Service Hangfire Server";
                opt.Queues = new[] { "recurring", "default" };
                opt.SchedulePollingInterval = TimeSpan.FromMinutes(1);
                opt.WorkerCount = 5;
            });

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services,
                                                            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static IServiceCollection RegisterWorkers(this IServiceCollection services)
        {
            services.AddHostedService<NotificationWorker>()
               .AddHostedService<AuditTrailWorker>()
               .AddHostedService<DataloadWorker>();

            return services;
        }

        private static IServiceCollection ConfigureRefit(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddTransient<ForwardAuthHeaderHandler>();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            // Refit settings using System.Text.Json with the options above
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(options)
            };

            var servers = configuration.GetSection("ServiceUrls")
                .Get<ServiceUrls>() ??
                throw new ArgumentNullException("ServiceUrls");

            services.AddRefitClient<IIdentityServiceClient>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(servers.IdentityService))
                .AddHttpMessageHandler<ForwardAuthHeaderHandler>();

            services.AddRefitClient<ILocationClientService>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(servers.ExternalLocationClient));

            return services;
        }

        private static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            return services;
        }

        private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kwik Nesta Infrastructure Svc",
                    Version = "v1",
                    Description = "Kwik Nesta Infrastructure Service API v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Kwik Nesta Inc.",
                        Email = "info@kwik-nesta.com",
                        Url = new Uri("https://kwik-nesta.com")
                    }
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Kwik Nesta Infrastructure Svc",
                    Version = "v2",
                    Description = "Kwik Nesta Infrastructure Service API v2.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Kwik Nesta Inc.",
                        Email = "info@kwik-nesta.com",
                        Url = new Uri("https://kwik-nesta.com")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Kwik Nesta Infrastructure API"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }});
            });
        }

        private static IServiceCollection ConfigureApiVersion(this IServiceCollection services)
        {
            return services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new UrlSegmentApiVersionReader());
            });
        }

        private static IServiceCollection ConfigureJwt(this IServiceCollection services, 
                                                       IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt")
                .Get<JwtSettings>() ?? throw new ArgumentNullException("JWT Config can not be null");

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateLifetime = true,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        RoleClaimType = jwtSettings.RoleClaim,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            // stop the default behavior (and the WWW-Authenticate header)
                            context.HandleResponse();
                            var statusCode = StatusCodes.Status401Unauthorized;
                            var message = "Unauthorized. Please login";

                            // Check if the failure is due to token expiration
                            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                            {
                                statusCode = StatusCodes.Status403Forbidden;
                                message = "Forbidden. Token has expired!";
                            }

                            context.Response.StatusCode = statusCode;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsJsonAsync(new
                            {
                                Successful = false,
                                Status = statusCode,
                                Message = message
                            });
                        }
                    };
                });

            services.AddAuthorization();
            return services;
        }
    }
}
