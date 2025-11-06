using Application.Ports.DrivenPorts.Negocio;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Perfil;
using Application.Ports.DriverPorts.Rol;
using Application.UseCases.Negocio;
using Application.UseCases.Rol;
using Infrastructure.DrivenAdapters.Negocio;
using Infrastructure.DrivenAdapters.Rol;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add your services here
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDrivenRolRepository, DrivenAdapterRoles>();
builder.Services.AddScoped<IDriverRolPort, ConsultarRolUseCase>();
builder.Services.AddScoped<IDrivenNegocioRepository, DrivenAdapterNegocio>();
builder.Services.AddScoped<IDriverPerfilPort, ConsultarNegociosDisponiblesUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
