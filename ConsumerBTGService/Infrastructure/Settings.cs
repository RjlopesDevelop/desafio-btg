namespace ConsumerBTGService.Infrastructure
{
    public static class Settings
    {
        private static IConfigurationBuilder builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
        public static string GetStringConnection()
        {

            var configuration = builder.Build();
            return configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }
        public static string GetQueueName()
        {
            var configuration = builder.Build();
            return configuration["RabbitMQ:Queue"] ?? string.Empty;
        }
        public static string GetQueueHost()
        {
            var configuration = builder.Build();
            return configuration["RabbitMQ:HostName"] ?? string.Empty;
        }
        public static string GetQueueUser()
        {
            var configuration = builder.Build();
            return configuration["RabbitMQ:UserName"] ?? string.Empty;
        }
        public static string GetQueuePassword()
        {
            var configuration = builder.Build();
            return configuration["RabbitMQ:Password"] ?? string.Empty;
        }
        public static int GetQueuePort()
        {
            var configuration = builder.Build();
            return int.Parse(configuration["RabbitMQ:Port"] ?? "0");
        }
    }
}