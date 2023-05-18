using MySql.Data.MySqlClient;
namespace DatabaseGroup
{
    public class Database
    {
        private static MySqlConnection connection;
        public static async Task Init(MySqlConnection connection)
        {
            // using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");

            // NOTE: Run the two below commands only one at the start, then comment them again.
            // using var command = new MySqlCommand("CREATE TABLE IF NOT EXISTS User (userid int NOT NULL AUTO_INCREMENT, username varchar(255) NOT NULL, phonenumber varchar(255) NOT NULL, email varchar(255) NOT NULL, name varchar(255) NOT NULL, hashedpassword varchar(255) NOT NULL, PRIMARY KEY (userid))", connection);
            // await command.ExecuteNonQueryAsync();


            // using var insertCommand = new MySqlCommand("INSERT INTO User (username, phonenumber, email, name, hashedpassword) VALUES (@username, @phonenumber, @email, @name, @hashedpassword)", connection);

            // // Set parameter values for initial user
            // insertCommand.Parameters.AddWithValue("@username", "admin2");
            // insertCommand.Parameters.AddWithValue("@phonenumber", "12345678");
            // insertCommand.Parameters.AddWithValue("@email", "admin@gmail.com");
            // insertCommand.Parameters.AddWithValue("@name", "Admin2");
            // insertCommand.Parameters.AddWithValue("@hashedpassword", "admin");

            // // Execute the insert command
            // await insertCommand.ExecuteNonQueryAsync();
            Database.connection = connection;

            using var selectCommand = new MySqlCommand("SELECT * FROM User", connection);
            using var reader = await selectCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var value = reader.GetValue(0);
                // do something with 'value'
                Console.WriteLine(value);
            }

        }

        public static async Task<int> CountOccurrence(string field, string value)
        {
            using var selectCommand = new MySqlCommand("SELECT COUNT(*) FROM User WHERE " + field + " = @value", Database.connection);
            selectCommand.Parameters.AddWithValue("@value", value);

            int rowCount = Convert.ToInt32(await selectCommand.ExecuteScalarAsync());
            Console.WriteLine($"Number of rows: {rowCount}");
            return rowCount;
        }

        public static async Task<List<string>> GetDetailsFromUsername(string username)
        {
            List<string> list = new List<string>();

            using var selectCommand = new MySqlCommand("SELECT * FROM User WHERE username = @value", Database.connection);
            selectCommand.Parameters.AddWithValue("@value", username);

            using var reader = await selectCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    list.Add(reader.GetString(i));
                }
            }

            Console.WriteLine(string.Join(", ", list));

            return list;
        }

        public static async Task registerUser(string username, string phoneNumber, string email, string name, string hashedPassword)
        {
            using var insertCommand = new MySqlCommand("INSERT INTO User (username, phonenumber, email, name, hashedpassword) VALUES (@username, @phonenumber, @email, @name, @hashedpassword)", Database.connection);

            insertCommand.Parameters.AddWithValue("@username", username);
            insertCommand.Parameters.AddWithValue("@phonenumber", phoneNumber);
            insertCommand.Parameters.AddWithValue("@email", email);
            insertCommand.Parameters.AddWithValue("@name", name);
            insertCommand.Parameters.AddWithValue("@hashedpassword", hashedPassword);

            await insertCommand.ExecuteNonQueryAsync();
        }

        public static async Task changePassword(string username, string newHashedPassword)
        {
            using var insertCommand = new MySqlCommand("UPDATE User SET hashedpassword=@hashedpassword WHERE username=@username", Database.connection);

            insertCommand.Parameters.AddWithValue("@username", username);
            insertCommand.Parameters.AddWithValue("@hashedpassword", newHashedPassword);

            await insertCommand.ExecuteNonQueryAsync();
        }

        public static async Task updateDetails(string username, string phoneNumber, string email, string name)
        {
            using var insertCommand = new MySqlCommand("UPDATE User SET phoneNumber=@phonenumber, email=@email, name=@name WHERE username=@username", Database.connection);

            insertCommand.Parameters.AddWithValue("@username", username);
            insertCommand.Parameters.AddWithValue("@phonenumber", phoneNumber);
            insertCommand.Parameters.AddWithValue("@email", email);
            insertCommand.Parameters.AddWithValue("@name", name);

            await insertCommand.ExecuteNonQueryAsync();
        }

    }

    public class DbConnection
    {
        private static MySqlConnection connection = null;

        private void DatabaseConnectionCreation() { }

        public static async Task<MySqlConnection> GetDbConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");
                await connection.OpenAsync();
            }
            return connection;
        }


    }
}
