using UserGroup;
using ValidationServiceGroup;
using PasswordHashingServiceGroup;
using DatabaseGroup;

namespace AccountServiceGroup
{
    public class AccountService
    {
        private User user;

        public async Task<Boolean> registerUser(string username, string phoneNumber, string email, string name, string password)
        {
            if (!await ValidationService.checkValidRegistrationInput(username))
            {
                return false;
            }
            else
            {
                await Database.registerUser(username, phoneNumber, email, name, PasswordHashingService.Encrypt(password));
                return true;
            }
        }

        public async Task<Boolean> loginUser(string username, string password)
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

        public async void changePassword(string username, string newPassword)
        {
            await Database.changePassword(username, newPassword);
        }

        public async void updateDetails(string username, string phoneNumber, string email, string name)
        {
            await Database.updateDetails(username, phoneNumber, email, name);
        }
    }
}