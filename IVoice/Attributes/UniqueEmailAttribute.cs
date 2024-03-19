
namespace IVoice.Attributes
{
    public class UniqueEmailAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var email = value.ToString();

                // Retrieve ApplicationDbContext from ValidationContext
                var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

                // Check if any user with the same email already exists
                var existingUser = dbContext.Users.FirstOrDefault(u => u.Email == email);
                if (existingUser != null)
                {
                    return new ValidationResult("Email address is already in use.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
