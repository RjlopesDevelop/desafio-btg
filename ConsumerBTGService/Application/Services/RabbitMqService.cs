using ConsumerBTGService.Application.Interfaces;
using ConsumerBTGService.Domain.DTOs;
using ConsumerBTGService.Domain.Models;
using ConsumerBTGService.Infrastructure;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace ConsumerBTGService.Application.Services
{

    public class RabbitMqService : IRabbitMqService, IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        //  private readonly IOrderService _orderService;
      //  private readonly BtgDbContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqService( IServiceScopeFactory serviceScopeFactory)
        {
            // _orderService = orderService;
          //  _context = context;
            _serviceScopeFactory = serviceScopeFactory;
            // Cria a conexão com o RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = Settings.GetQueueHost(),
                Port = Settings.GetQueuePort(),
                UserName = Settings.GetQueueUser(),
                Password = Settings.GetQueuePassword()
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: Settings.GetQueueName(),
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void ConsumeMessage()
        {

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Message Received: {0}", message);
                // Deserializar a mensagem recebida em um objeto JSON
                //var order = JsonConvert.DeserializeObject<OrderDTO>(message);
                OrderDTO? order = JsonConvert.DeserializeObject<OrderDTO>(message);
                if (order == null)
                {
                    Console.WriteLine("Failed to deserialize the message.");
                    return;
                }
                // Gravar o pedido no banco de dados
                try
                {
                    // Criando um escopo para obter uma nova instância de IOrderService e DbContext
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                        await orderService.RecordOrderAsync(order);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing message: {0}", ex.Message);
                }

            };
            _channel.BasicConsume(queue: Settings.GetQueueName(),
                                 autoAck: true,
                                 consumer: consumer);
        }
        public void PostMessage(OrderDTO orderDTO)
        {
            var messageJson = JsonConvert.SerializeObject(orderDTO);
            var body = Encoding.UTF8.GetBytes(messageJson);

            _channel.BasicPublish(exchange: "",
                                  routingKey: Settings.GetQueueName(),
                                  basicProperties: null,
                                  body: body);

            Console.WriteLine(" [x] Message Sent: {0}", messageJson);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("RabbitMQ Service Started...");
            ConsumeMessage();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("RabbitMQ Service Stopping...");
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }
    }
}



