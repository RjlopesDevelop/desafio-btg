using ConsumerBTGService.Domain.DTOs;
using ConsumerBTGService.Domain.Models;

namespace ConsumerBTGService.Application.Interfaces
{
  public interface IOrderService
  {
    Task RecordOrderAsync(OrderDTO order);
    Task<Order?> GetOrderByIdAsync(int orderId);
    OrdersConstumerDTO GetOrdersByCustomerId(int customerId);

  }
}