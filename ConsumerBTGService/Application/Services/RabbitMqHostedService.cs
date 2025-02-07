using ConsumerBTGService.Application.Interfaces;

namespace ConsumerBTGService.Application.Services
{
    public class RabbitMqHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
                rabbitMqService.ConsumeMessage();
            }

            await Task.CompletedTask;
        }
    }
}
