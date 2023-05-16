using BCr = BCrypt.Net;
namespace PasswordHashingService
{
    public static class PasswordHashingService
    {
        public static string Encrypt(string password)
        {
            return BCr.BCrypt.HashPassword(password);
        }

        public static bool Validate(string password, string hashedPassword)
        {
            return BCr.BCrypt.Verify(password, hashedPassword);
        }
    }

}
