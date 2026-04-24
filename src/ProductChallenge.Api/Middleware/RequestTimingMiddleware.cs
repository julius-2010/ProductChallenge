using System.Diagnostics;
using System.Text;

namespace ProductChallenge.Api.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public RequestTimingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var logDirectory = Path.Combine(_environment.ContentRootPath, "Logs");
            Directory.CreateDirectory(logDirectory);

            var logFilePath = Path.Combine(logDirectory, "request-log.txt");

            var logLine = new StringBuilder()
                .Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | ")
                .Append($"{context.Request.Method} ")
                .Append($"{context.Request.Path}{context.Request.QueryString} | ")
                .Append($"{context.Response.StatusCode} | ")
                .Append($"{stopwatch.ElapsedMilliseconds} ms")
                .ToString();

            await File.AppendAllTextAsync(logFilePath, logLine + Environment.NewLine);
        }
    }
}
