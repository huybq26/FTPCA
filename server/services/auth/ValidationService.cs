// using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");
// connection.Open();

// using var command = new MySqlCommand("SELECT field FROM table;", connection);
// using var reader = command.ExecuteReader();
// while (reader.Read())
//     Console.WriteLine(reader.GetString(0));

namespace ValidationService
{
    public class ValidationService
    {


        public static Boolean checkValidRegistrationInput(string username)
        {
            // simply check if the username has existed or not from the database
            return true;
        }
        public static Boolean checkValidLoginInput(string username, string password)
        {
            // check if the password matches with the username's password from the database
            return false;
        }

    }
}
