using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the API call time
        var callTime = System.DateTime.Now;
        _logger.LogInformation($"API called at {callTime}");

        // Get the user IP address
        var ipAddress = context.Connection.RemoteIpAddress;
        _logger.LogInformation($"IP address of the user: {ipAddress}");

        // Log the request method and path
        _logger.LogInformation($"{context.Request.Method} request to {context.Request.Path}");

        // Log the request headers
        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation($"{header.Key}: {header.Value}");
        }

        // Allow the body to be read multiple times
        context.Request.EnableBuffering();

        // Read the request body
        string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        _logger.LogInformation($"Request body: {requestBody}");

        // Reset the request body stream
        context.Request.Body.Position = 0;

        // Call the next middleware in the pipeline
        await _next(context);
    }
}