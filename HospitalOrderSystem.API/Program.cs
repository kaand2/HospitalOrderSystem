using HospitalOrderSystem.Application;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Persistence.Context;
using HospitalOrderSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Couldn't connect.");
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddApplicationServices();
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
