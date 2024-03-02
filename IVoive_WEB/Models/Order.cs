namespace IVoive_WEB.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }

        public double Price { get; set; }

        public string OrderStatus { get; set; }


    }
}
