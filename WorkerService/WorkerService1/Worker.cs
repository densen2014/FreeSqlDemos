using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        IFreeSql _fsql { get; set; }

        public Worker(ILogger<Worker> logger, IFreeSql fsql)
        {
            _logger = logger;
            _fsql = fsql;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var res = _fsql.Select<Item>().ToOne().Text;
                _logger.LogInformation("Worker running at: {time} {}", DateTimeOffset.Now, res);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
