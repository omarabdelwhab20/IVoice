namespace IVoice.Models.DTO
{
    public class UsersDisplayModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public int OrdersCount { get; set; }
    }
}
