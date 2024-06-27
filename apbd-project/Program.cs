using apbd_project.Data;
using apbd_project.Middleware;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ISoftwareProductService, SoftwareProductService>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseSqlServer("Name=ConnectionStrings:Default"));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Register custom exception handler middleware
app.UseMiddleware<CustomExceptionHandler>();

app.Run();