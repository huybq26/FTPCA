using MySql.Data.MySqlClient;
namespace DatabaseInitGroup
{
    public class DatabaseInit
    {
        public static async Task Init(MySqlConnection connection)
        {
            // using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");


            // using var command = new MySqlCommand("CREATE TABLE IF NOT EXISTS User (username varchar(255) PRIMARY KEY, phonenumber varchar(255), email varchar(255), name varchar(255), hashedpassword varchar(255))", connection);
            // await command.ExecuteNonQueryAsync();

            using var insertCommand = new MySqlCommand("INSERT INTO User (username, phonenumber, email, name, hashedpassword) VALUES (@username, @phonenumber, @email, @name, @hashedpassword)", connection);

            // Set parameter values for initial user
            insertCommand.Parameters.AddWithValue("@username", "admin2");
            insertCommand.Parameters.AddWithValue("@phonenumber", "12345678");
            insertCommand.Parameters.AddWithValue("@email", "admin@gmail.com");
            insertCommand.Parameters.AddWithValue("@name", "Admin2");
            insertCommand.Parameters.AddWithValue("@hashedpassword", "admin");

            // Execute the insert command
            await insertCommand.ExecuteNonQueryAsync();

            using var selectCommand = new MySqlCommand("SELECT * FROM User", connection);
            using var reader = await selectCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var value = reader.GetValue(0);
                // do something with 'value'
                Console.WriteLine(value);
            }

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
