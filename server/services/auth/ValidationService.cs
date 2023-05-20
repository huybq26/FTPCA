// using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");
// connection.Open();

// using var command = new MySqlCommand("SELECT field FROM table;", connection);
// using var reader = command.ExecuteReader();
// while (reader.Read())
//     Console.WriteLine(reader.GetString(0));
using DatabaseGroup;
using PasswordHashingServiceGroup;

namespace ValidationServiceGroup
{
    public class ValidationService
    {


        public static async Task<Boolean> checkValidRegistrationInput(string username)
        {
            // simply check if the username has existed or not from the database

            int count = await Database.CountOccurrence("username", username);
            return count == 0;
        }
        public static async Task<Boolean> checkValidLoginInput(string username, string password)
        {
            // check if the password matches with the username's password from the database
            // Console.WriteLine("Hashed Password from Database is: " + Database.GetDetailsFromUsername.get(0).get(5));
            // Console.WriteLine("Provided hashed password is: " + PasswordHashingService.Encrypt(password));
            List<string> databasePassword = await Database.GetDetailsFromUsername(username);
            Console.WriteLine(string.Join(", ", databasePassword));
            Console.WriteLine(PasswordHashingService.Encrypt(password));
            return PasswordHashingService.Validate(password, databasePassword[5]);
            // return databasePassword[5] == PasswordHashingService.Encrypt(password);
        }

    }
}
