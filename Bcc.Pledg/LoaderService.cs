using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bcc.Pledg
{
    public class LoaderService : IHostedService
    {
        private readonly ILogger<LoaderService> _logger;
        public LoaderService(ILogger<LoaderService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var resource in ReferenceContext.Resources)
            {
                tasks.Add(Task.Run(async () =>
                {
                    _logger.LogInformation("Started loading for {0}", resource.Key.ToString());
                    var result = await Loader.Load(resource.Key, resource.Value);
                    ReferenceContext.References.TryAdd(resource.Key, result);
                }));
            }

            return Task.WhenAll(tasks);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
