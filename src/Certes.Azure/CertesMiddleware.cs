﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Certes.Azure
{
    public class CertesMiddleware
    {
        private const string WebJobFilePrefix = "Certes.Azure.Resources.WebJob.";
        private int webJobInitialized = 0;

        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly CertesOptions options;

        public CertesMiddleware(RequestDelegate next, IOptions<CertesOptions> optionsAccessor, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<CertesMiddleware>();
            this.options = optionsAccessor.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (0 == Interlocked.CompareExchange(ref webJobInitialized, 1, 0))
            {
                var env = context.RequestServices.GetRequiredService<IHostingEnvironment>();
                var webJobPath = Path.Combine(env.ContentRootPath, "app_data/jobs/triggered/certes");

                var dir = new DirectoryInfo(webJobPath);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                var assembly = typeof(CertesMiddleware).GetTypeInfo().Assembly;
                var webJobFiles = assembly
                    .GetManifestResourceNames()
                    .Where(n => n.StartsWith(WebJobFilePrefix))
                    .ToArray();

                foreach (var file in webJobFiles)
                {
                    var filename = file.Substring(WebJobFilePrefix.Length);
                    var dest = Path.Combine(webJobPath, filename);
                    using (var destStream = File.Create(dest))
                    using (var srcStream = assembly.GetManifestResourceStream(file))
                    {
                        await srcStream.CopyToAsync(destStream);
                    }
                }
            }

            await next.Invoke(context);
        }
    }

    public static class CertesExtensions
    {

        public static IApplicationBuilder UseCertesWebJob(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CertesMiddleware>();
        }
    }
}
