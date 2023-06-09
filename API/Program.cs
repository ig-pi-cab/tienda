using API.Extensions;
using AspNetCoreRateLimit;
using Infraestructure.Data;
using Infraestructure.Data.Csvs;
using Infraestructure.ExternalApiService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Infrastructure;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        var _logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        //Limpia proveedores, remueve mensajes 
        //builder.Logging.ClearProviders(); 


        //builder.Logging.AddSerilog(_logger);

        builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

        builder.Services.ConfigureRateLimitation();
        // Add services to the container.

        builder.Services.ConfigureCors();
        builder.Services.AddAplicacionServices();
        builder.Services.ConfigureApiVersioning();
        builder.Services.AddJwt(builder.Configuration);
        builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        builder.Services.AddHttpClient();

        builder.Services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
        }).AddXmlSerializerFormatters(); //PERMITE SOPORTE DE FORMATO XML

        builder.Services.AddDbContext<TiendaContexto>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        var app = builder.Build();


        //Manejo global de excepciones

        app.UseIpRateLimiting();

        // Configure the HTTP request pipeline.



        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>(); //
            try
            {
                var context = services.GetRequiredService<TiendaContexto>(); //
                await context.Database.MigrateAsync(); // Aplicar de forma asincrona cualquier migracion pendiente. Tambien agregar la base de datos si es que no existe.
                await TiendaContextSeed.SeedAsync(context, loggerFactory);
                await TiendaContextSeed.SeedRolesAsync(context, loggerFactory);
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Ocurrio un error durante la migracion");
            }
        }

        //Canalizacion de peticiones

        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller = Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
                name: "dollar",
                pattern: "api/dollar",
                defaults: new { controller = "Dolar", action = "GetDollarExchangeRate" });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}