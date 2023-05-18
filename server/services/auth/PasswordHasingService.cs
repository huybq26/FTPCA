using BCr = BCrypt.Net;
namespace PasswordHashingServiceGroup
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
