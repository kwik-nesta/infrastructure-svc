namespace KwikNesta.Infrastruture.Svc.API.Extensions
{
    public static class WebAppExtensions
    {
        internal static void RegisterMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("GatewayOnly");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
