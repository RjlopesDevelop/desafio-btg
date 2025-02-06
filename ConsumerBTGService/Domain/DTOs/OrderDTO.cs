namespace ConsumerBTGService.Domain.DTOs
{
    public class OrderItemDTO
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<OrderItemDTO>? Itens { get; set; }
    }
    public class OrdersConstumerDTO
    {

        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public List<OrderDTO>? Orders { get; set; }
    }
}
