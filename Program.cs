using System.Text.Json;
using Microsoft.EntityFrameworkCore.Internal;
using VirtualLibrary.Interfaces;
using VirtualLibrary.MiddleWare;
using VirtualLibrary.Models;
using VirtualLibrary.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextFactory<VirtualLibraryContext>();
builder.Services.AddDbContext<VirtualLibraryContext>();
builder.Services.AddScoped<DbContextFactory<VirtualLibraryContext>>();
// builder.Services.AddScoped<IRepository<VirtualLibraryContext>, VirtualLibraryRepository>();
builder.Services.AddScoped<IVirtualLibraryInterface, VirtualLibraryRepository>();
builder.Services.Configure<ErrHandlerMiddleWareOptions>(builder.Configuration.GetSection("MiddleWare"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Custom Middleware
app.UseErrHandlerMiddleware();

app.Run();
