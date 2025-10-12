using Microsoft.EntityFrameworkCore;
using KwikNesta.Infrastruture.Svc.Domain.Entities;
using TimeZone = KwikNesta.Infrastruture.Svc.Domain.Entities.TimeZone;

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<TimeZone> TimeZones { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("infrastructure-svc");

            base.OnModelCreating(builder);
        }
    }
}