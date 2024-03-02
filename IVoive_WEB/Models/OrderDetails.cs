namespace IVoive_WEB.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Count { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}
