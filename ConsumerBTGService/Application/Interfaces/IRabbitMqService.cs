using ConsumerBTGService.Domain.DTOs;
using ConsumerBTGService.Domain.Models;

namespace ConsumerBTGService.Application.Interfaces
{
    public interface IRabbitMqService
    {
        void ConsumeMessage();
        void PostMessage(OrderDTO order);
    }
}