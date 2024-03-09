namespace IVoice.Attributes
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string _extension;

        public AllowedExtensionAttribute(string allowedExtension)
        {
            _extension = allowedExtension;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            
            if(file is not null)
            {
                var extension = Path.GetExtension(file.FileName);

                var isAllowed = _extension.Split(",").Contains(extension , StringComparer.OrdinalIgnoreCase);

                if (!isAllowed)
                {
                    return new ValidationResult($"only {_extension} are allowed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
