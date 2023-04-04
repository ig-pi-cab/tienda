using API.Extensions;
using AspNetCoreRateLimit;
using Infraestructure.Data;
using Infraestructure.Data.Csvs;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services.ConfigureRateLimitation();
// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.ConfigureApiVersioning();
builder.Services.AddJwt(builder.Configuration);

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>(); //
	try
	{
		var context = services.GetRequiredService<TiendaContexto>(); //
		await context.Database.MigrateAsync(); // Aplicar de forma asyncrona cualquier migracion pendiente. Tambien agregar la base de datos si es que no existe.
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

app.Run();
