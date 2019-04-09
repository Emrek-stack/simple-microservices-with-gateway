using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Pantokrator.Order.Api.Middleware
{
    public class TimeElapsedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<TimeElapsedMiddleware> _logger;

        public TimeElapsedMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<TimeElapsedMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = new Stopwatch();

            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();

                context.Response.Headers.Add("Elapsed", new[] { stopwatch.ElapsedMilliseconds.ToString() });
                context.Response.Headers.Add("Environment", new[] { _env.EnvironmentName });

                string deployment = Environment.GetEnvironmentVariable("DEPLOYMENT_INFO");
                if (!string.IsNullOrEmpty(deployment))
                    context.Response.Headers.Add("Deployment", new[] { deployment });

                if (context.Request.Headers["TransactionId"].Any())
                    context.Response.Headers.Add("TransactionId", context.Request.Headers["TransactionId"]);

                var httpConnectionFeature = context.Features.Get<IHttpConnectionFeature>();
                if (httpConnectionFeature.LocalIpAddress != null)
                {
                    context.Response.Headers.Add("ServerIpAddress", httpConnectionFeature.LocalIpAddress.ToString());
                    context.Response.Headers.Add("ConnectionId", httpConnectionFeature.ConnectionId);
                    context.Response.Headers.Add("Hostname", Environment.MachineName);
                }



                return Task.CompletedTask;
            });

            stopwatch.Start();
            await _next.Invoke(context);
        }
    }

    public static class TimeElapsedExtensions
    {
        public static IApplicationBuilder UseTimeElapsed(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimeElapsedMiddleware>();
        }
    }
}
