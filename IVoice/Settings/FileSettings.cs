namespace IVoice.Settings
{
    public class FileSettings
    {
        public const string ImagePath = "/images";
        public const string AllowedExtensions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMb = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMb * 1024 * 1024;
    }
}
