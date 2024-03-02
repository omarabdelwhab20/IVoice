
namespace IVoive_WEB.Models
{
    public class FeedBack
    {
        public int Id { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser applicationUser { get; set; }
    }
}
