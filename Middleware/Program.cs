using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.Data;
using Middleware.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Use(async (context, next) =>
{
    // Log the API call time
    var callTime = System.DateTime.Now;
    app.Logger.LogInformation($"API called at {callTime}");

    // Get the user IP address
    var ipAddress = context.Connection.RemoteIpAddress;
    app.Logger.LogInformation($"IP address of the user: {ipAddress}");

    // Read the request body
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    app.Logger.LogInformation($"Request body: {requestBody}");

    // Log the request details
    var request = context.Request;
    app.Logger.LogInformation($"Request method: {request.Method}");
    app.Logger.LogInformation($"Request path: {request.Path}");
    app.Logger.LogInformation($"Request headers: {string.Join(",", request.Headers.Select(h => $"{h.Key}:{h.Value}"))}");
    app.Logger.LogInformation($"Request query string: {request.QueryString}");

    // Call the next middleware in the pipeline
    await next();

    // Store the request details in the log file
    var logMessage = $"API called at {callTime} by IP address {ipAddress} with request method {request.Method}, path {request.Path}, headers {string.Join(",", request.Headers.Select(h => $"{h.Key}:{h.Value}"))}, query string {request.QueryString} and body {requestBody}";
    await File.AppendAllTextAsync("log.txt", logMessage);
});

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