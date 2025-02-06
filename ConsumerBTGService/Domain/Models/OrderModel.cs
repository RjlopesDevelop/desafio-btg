namespace ConsumerBTGService.Domain.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }    
        public virtual  ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual  Order? Order { get; set; }    
    }
}