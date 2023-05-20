using UserGroup;
using ValidationServiceGroup;
using PasswordHashingServiceGroup;
using DatabaseGroup;

namespace AccountServiceGroup
{
    public class AccountService
    {
        private User user;

        public static async Task<Boolean> registerUser(string username, string phoneNumber, string email, string name, string hashedPassword)
        {
            if (!await ValidationService.checkValidRegistrationInput(username))
            {
                return false;
            }
            else
            {
                await Database.registerUser(username, phoneNumber, email, name, hashedPassword);
                return true;
            }
        }

        public static async Task<Boolean> loginUser(string username, string password)
        {
            if (!await ValidationService.checkValidLoginInput(username, password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void logoutUser(string username) { }

        public static async void changePassword(string username, string newPassword)
        {
            await Database.changePassword(username, newPassword);
        }

        public static async void updateDetails(string username, string phoneNumber, string email, string name)
        {
            await Database.updateDetails(username, phoneNumber, email, name);
        }

        public static async Task<User> GetUserFromUsername(string username)
        {
            List<string> list = await Database.GetDetailsFromUsername(username);
            return new User
            {
                Userid = list[0],
                Username = list[1],
                PhoneNumber = list[2],
                Email = list[3],
                Name = list[4],
                HashedPassword = list[5]
            };
        }

    }
}