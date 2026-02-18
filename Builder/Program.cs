using Application.Ports.DrivenPorts.Ingrediente;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DrivenPorts.Producto;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Ingrediente;
using Application.Ports.DriverPorts.Perfil;
using Application.Ports.DriverPorts.Producto;
using Application.Ports.DriverPorts.Rol;
using Application.UseCases.Ingrediente;
using Application.UseCases.Perfil;
using Application.UseCases.Producto;
using Application.UseCases.Rol;
using Infrastructure.Data;
using Infrastructure.DrivenAdapters.Ingrediente;
using Infrastructure.DrivenAdapters.Perfil;
using Infrastructure.DrivenAdapters.Producto;
using Infrastructure.DrivenAdapters.Rol;
using Infrastructure.DriverAdapters.Ingrediente;
using Infrastructure.DriverAdapters.Rol;
using Infrastructure.Tarjetas;
using Infrastructure.Tarjetas.TarjetaAdapter;
using Infrastructure.Tarjetas.TarjetaPort;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dealgestor API", Version = "v1" });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Ejemplo: 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
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
        }
    });
});

// 1. Conexión Hexagonal: Inyectamos la implementación de la Tarjeta
builder.Services.Configure<ConfiguracionDeTarjeta>(
builder.Configuration.GetSection(ConfiguracionDeTarjeta.NombreSeccion));
builder.Services.AddSingleton<PortTarjetaGenerador, EmisorDeTarjetas>();
builder.Services.AddSingleton<Application.Ports.DrivenPorts.Tarjeta.IGeneradorDeTokens, EmisorDeTarjetas>();

// 2. Configuración del JWT Bearer para la validación
var configTarjeta = builder.Configuration
    .GetSection(ConfiguracionDeTarjeta.NombreSeccion)
    .Get<ConfiguracionDeTarjeta>();

if (configTarjeta == null)
{
    throw new InvalidOperationException($"La sección de configuración '{ConfiguracionDeTarjeta.NombreSeccion}' no está presente en appsettings.json");
}

if (string.IsNullOrWhiteSpace(configTarjeta.Secret))
{
    throw new InvalidOperationException("El Secret de JWT no está configurado en appsettings.json");
}

if (string.IsNullOrWhiteSpace(configTarjeta.Issuer))
{
    throw new InvalidOperationException("El Issuer de JWT no está configurado en appsettings.json");
}

if (string.IsNullOrWhiteSpace(configTarjeta.Audience))
{
    throw new InvalidOperationException("El Audience de JWT no está configurado en appsettings.json");
}

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

// Rate limiting básico
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 20;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// DbContext - Conexión a PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Cross-cutting opcional
//// builder.Services.AddHttpClient<IDrivenNegocioRepository, DrivenAdapterNegocio>();
//builder.Services.AddHealthChecks()
//    .AddDbContextCheck<ApplicationDbContext>("db");

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
builder.Services.AddTransient<PortDriverPerfilAutenticar, AutenticarPerfilUseCase>();
builder.Services.AddTransient<PortDriverProductoConsultar, ConsultarProductoUseCase>();
builder.Services.AddTransient<PortDriverProductoCrear, CrearProductoUseCase>();
builder.Services.AddTransient<PortDriverProductoEliminar, EliminarProductoUseCase>();
builder.Services.AddTransient<PortDriverProductoModificar, ModificarProductoUseCase>();

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
builder.Services.AddScoped<PortDrivenProductoConsultar, DrivenAdapterProductoConsultar>();
builder.Services.AddScoped<PortDrivenProductoCrear, DrivenAdapterProductoCrear>();
builder.Services.AddScoped<PortDrivenProductoEliminar, DrivenAdapterProductoEliminar>();
builder.Services.AddScoped<PortDrivenProductoModificar, DrivenAdapterProductoModificar>();

builder.Services.AddAuthorization();

var app = builder.Build();

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dealgestor API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Orden correcto del middleware pipeline
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication(); // Identifica al usuario (Lee la tarjeta)
app.UseAuthorization();  // Autoriza al usuario (Revisa los permisos/roles)

//app.MapHealthChecks("/health");
app.MapControllers();
app.Run();

