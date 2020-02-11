using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetData;
using PetData.Models;

namespace PetTasks
{
    /* Worker which updates the animals stats every second */
    public class Worker : BackgroundService
    {
        private int CycleIterval = 1000; // ms
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public static void UpdateAnimals(DatabaseContext db)
        {
            Animal.AnimalsLifeCycle(db);
        }
        private void _UpdateAnimals()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                DatabaseContext db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                UpdateAnimals(db);
               }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _UpdateAnimals();
                await Task.Delay(CycleIterval, stoppingToken);
            }
        }
    }
}
