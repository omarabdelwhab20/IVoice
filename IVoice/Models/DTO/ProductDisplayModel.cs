namespace IVoice.Models.DTO
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> products { get; set; }
        public string STerm { get; set; } = "";
    }
}
