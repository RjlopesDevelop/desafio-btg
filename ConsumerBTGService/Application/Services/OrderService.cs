
using ConsumerBTGService.Application.Interfaces;
using ConsumerBTGService.Domain.Models;
using ConsumerBTGService.Infrastructure;
using ConsumerBTGService.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ConsumerBTGService.Application.Services
{
    public class OrderService(BtgDbContext context) : IOrderService
    {
        private BtgDbContext _context = context;

        public async Task RecordOrderAsync(OrderDTO order)
        {
            if (order != null)
            {
                //Verificar se o pedido já existe

                if (await GetOrderByIdAsync(order.OrderId) != null)
                {
                    Console.WriteLine($"Order {order.OrderId} already exists");
                    return;
                }
                else
                {
                    await CreateAsync(order);
                    Console.WriteLine($"Order {order.OrderId} created successfully");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(order));
            }

        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
        }
        public OrdersConstumerDTO GetOrdersByCustomerId(int customerId)
        {
            var ordersConstumerDTO = new OrdersConstumerDTO();

            var ordersModel = _context.Orders.Include(x => x.OrderDetails).Where(x => x.CustomerId == customerId).ToList();

            if (ordersModel.Count >= 0)
            {

                ordersConstumerDTO.CustomerId = customerId;
                ordersConstumerDTO.Quantity = ordersModel.Count;
                ordersConstumerDTO.TotalAmount = ordersModel.Sum(x => x.TotalAmount);
                ordersConstumerDTO.Orders = [];


                foreach (var order in ordersModel)
                {
                    var orderDTO = new OrderDTO
                    {
                        OrderId = order.OrderId,
                        OrderDate = order.OrderDate,
                        CustomerId = order.CustomerId,
                        TotalAmount = order.OrderDetails?.Sum(x => x.UnitPrice * x.Quantity) ?? 0,
                        Itens = []
                    };

                    foreach (var item in order.OrderDetails!.ToList())
                    {
                        orderDTO.Itens.Add(new OrderItemDTO
                        {
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        });
                    }

                    ordersConstumerDTO.Orders.Add(orderDTO);
                }
            }


            return ordersConstumerDTO;
        }

        #region Métodos privados
        private async Task CreateAsync(OrderDTO order)
        {
            try
            {
                var orderModel = new Order
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.Itens?.Sum(x => x.UnitPrice * x.Quantity) ?? 0
                };
                _context.Orders.Add(orderModel);

                foreach (var item in order.Itens!)
                {
                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

    }


}