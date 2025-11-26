using Application.Ports.DrivenPorts.Negocio;
using Application.Ports.DriverPorts.Perfil;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using Application.Ports.DrivenPorts.Ingrediente;
using Application.Ports.DriverPorts.Ingrediente;
using Application.UseCases.Negocio;
using Application.UseCases.Rol;
using Application.UseCases.Ingrediente;
using Infrastructure.Data;
using Infrastructure.DrivenAdapters.Negocio;
using Infrastructure.DrivenAdapters.Rol;
using Infrastructure.DriverAdapters.Rol;
using Infrastructure.DrivenAdapters.Ingrediente;
using Infrastructure.DriverAdapters.Ingrediente;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
//// DbContext (activar conexión real antes de ejecutar migraciones)
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Cross-cutting opcional
//builder.Services.AddHttpClient<IDrivenNegocioRepository, DrivenAdapterNegocio>();
//builder.Services.AddHealthChecks()
//    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

// Driver Ports (Use Cases) - Transient
builder.Services.AddTransient<PortDriverRolConsultar, ConsultarRolUseCase>();
builder.Services.AddTransient<PortDriverIngredienteConsultar, ConsultarIngredienteUseCase>();
builder.Services.AddTransient<PortDriverIngredienteCrear, CrearIngredienteUseCase>();
builder.Services.AddTransient<PortDriverIngredienteEliminar, EliminarIngredienteUseCase>();
builder.Services.AddTransient<PortDriverIngredienteModificar, ModificarIngredienteUseCase>();
builder.Services.AddTransient<IDriverPerfilPort, ConsultarNegociosDisponiblesUseCase>();

// Driven Ports (Repositories) - Scoped
builder.Services.AddScoped<PortDrivenRolConsultar, DrivenAdapterRolConsultar>();
builder.Services.AddScoped<IDrivenNegocioRepository, DrivenAdapterNegocio>();
builder.Services.AddScoped<PortDrivenIngredienteConsultar, DrivenAdapterIngredienteConsultar>();
builder.Services.AddScoped<PortDrivenIngredienteCrear, DrivenAdapterIngredienteCrear>();
builder.Services.AddScoped<PortDrivenIngredienteEliminar, DrivenAdapterIngredienteEliminar>();
builder.Services.AddScoped<PortDrivenIngredienteModificar, DrivenAdapterIngredienteModificar>();

var app = builder.Build();

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

app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

