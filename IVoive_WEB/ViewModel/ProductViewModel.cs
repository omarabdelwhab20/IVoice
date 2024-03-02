using IVoive_WEB.Attributes;
using Microsoft.AspNetCore.Http;

namespace IVoive_WEB.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        [AllowedExtension(FileSettings.AllowedExtensions),
            MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile ImageFile { get; set; }

    }
}
