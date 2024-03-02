using System.ComponentModel.DataAnnotations.Schema;

namespace IVoive_WEB.Models
{
    [Table("Product")]

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; }
        public double Price { get; set; }
        [MaxLength(500)]
        public string Cover { get; set; } = string.Empty;
    }
}
