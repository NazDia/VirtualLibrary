using System.Text.Json;
using VirtualLibrary.MiddleWare;
using VirtualLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<VirtualLibraryContext>();
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
