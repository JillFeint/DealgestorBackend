using Application.Ports.DrivenPorts.Ingrediente;
//using Application.Ports.DrivenPorts.Negocio;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Ingrediente;
using Application.Ports.DriverPorts.Perfil;
using Application.Ports.DriverPorts.Rol;
using Application.UseCases.Ingrediente;
//using Application.UseCases.Negocio;
using Application.UseCases.Perfil;
using Application.UseCases.Rol;
using Infrastructure.Data;
using Infrastructure.DrivenAdapters.Ingrediente;
//using Infrastructure.DrivenAdapters.Negocio;
using Infrastructure.DrivenAdapters.Perfil;
using Infrastructure.DrivenAdapters.Rol;
using Infrastructure.DriverAdapters.Ingrediente;
using Infrastructure.DriverAdapters.Rol;
using Infrastructure.Tarjetas;
using Infrastructure.Tarjetas.TarjetaAdapter;
using Infrastructure.Tarjetas.TarjetaPort;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Conexión Hexagonal: Inyectamos la implementación de la Tarjeta
builder.Services.Configure<ConfiguracionDeTarjeta>(
builder.Configuration.GetSection(ConfiguracionDeTarjeta.NombreSeccion));
builder.Services.AddSingleton<PortTarjetaGenerador, EmisorDeTarjetas>();

// 2. Configuración del JWT Bearer para la validación
var configTarjeta = builder.Configuration
    .GetSection(ConfiguracionDeTarjeta.NombreSeccion)
    .Get<ConfiguracionDeTarjeta>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configTarjeta.Issuer,
        ValidAudience = configTarjeta.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configTarjeta.Secret))
    };
});

//// DbContext (activar conexión real antes de ejecutar migraciones)
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Cross-cutting opcional
//builder.Services.AddHttpClient<IDrivenNegocioRepository, DrivenAdapterNegocio>();
//builder.Services.AddHealthChecks()
//    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

// Driver Ports (Use Cases) - Transient
builder.Services.AddTransient<PortDriverRolConsultar, ConsultarRolUseCase>();
builder.Services.AddTransient<PortDriverRolCrear, CrearRolUseCase>();
builder.Services.AddTransient<PortDriverRolModificar, ModificarRolUseCase>();
builder.Services.AddTransient<PortDriverRolEliminar, EliminarRolUseCase>();
builder.Services.AddTransient<PortDriverIngredienteConsultar, ConsultarIngredienteUseCase>();
builder.Services.AddTransient<PortDriverIngredienteCrear, CrearIngredienteUseCase>();
builder.Services.AddTransient<PortDriverIngredienteEliminar, EliminarIngredienteUseCase>();
builder.Services.AddTransient<PortDriverIngredienteModificar, ModificarIngredienteUseCase>();
builder.Services.AddTransient<PortDriverPerfilCrear, CrearPerfilUseCase>();
builder.Services.AddTransient<PortDriverPerfilConsultar, ConsultarPerfilUseCase>();
builder.Services.AddTransient<PortDriverPerfilEliminar, EliminarPerfilUseCase>();
builder.Services.AddTransient<PortDriverPerfilModificar, ModificarPerfilUseCase>();

// Driven Ports (Repositories) - Scoped
builder.Services.AddScoped<PortDrivenRolConsultar, DrivenAdapterRolConsultar>();
builder.Services.AddScoped<PortDrivenRolCrear, DrivenAdapterRolCrear>();
builder.Services.AddScoped<PortDrivenRolModificar, DrivenAdapterRolModificar>();
builder.Services.AddScoped<PortDrivenRolEliminar, DrivenAdapterRolEliminar>();
builder.Services.AddScoped<PortDrivenIngredienteConsultar, DrivenAdapterIngredienteConsultar>();
builder.Services.AddScoped<PortDrivenIngredienteCrear, DrivenAdapterIngredienteCrear>();
builder.Services.AddScoped<PortDrivenIngredienteEliminar, DrivenAdapterIngredienteEliminar>();
builder.Services.AddScoped<PortDrivenIngredienteModificar, DrivenAdapterIngredienteModificar>();
builder.Services.AddScoped<PortDrivenPerfilCrear, DrivenAdapterPerfilCrear>();
builder.Services.AddScoped<PortDrivenPerfilConsultar, DrivenAdapterPerfilConsultar>();
builder.Services.AddScoped<PortDrivenPerfilEliminar, DrivenAdapterPerfilEliminar>();
builder.Services.AddScoped<PortDrivenPerfilModificar, DrivenAdapterPerfilModificar>();

builder.Services.AddAuthorization(); // Permite el uso de [Authorize]

// ... el resto de tus servicios (Controladores, UseCases, Repositorios)
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// Migraciones automáticas (controlar por configuración)
if (app.Configuration.GetValue<bool>("ApplyMigrationsOnStart"))
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
        logger.LogInformation("Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error aplicando migraciones al iniciar.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Estos dos son los que permiten la protección de Endpoints:
app.UseAuthentication(); // <-- Identifica al usuario (Lee la tarjeta)
app.UseAuthorization();  // <-- Autoriza al usuario (Revisa los permisos/roles)

app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

