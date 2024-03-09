

namespace IVoice.ViewModel
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? CurrentCover { get; set; }

        [AllowedExtension(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileSizeInBytes * 1024)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
