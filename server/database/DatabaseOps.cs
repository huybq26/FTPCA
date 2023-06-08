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
            // using var command = new MySqlCommand("CREATE TABLE IF NOT EXISTS FriendRequest (senderid int NOT NULL, receiverid int NOT NULL)", connection);
            // await command.ExecuteNonQueryAsync();
            // using var command2 = new MySqlCommand("CREATE TABLE IF NOT EXISTS Friendship (userid1 int NOT NULL, userid2 int NOT NULL)", connection);
            // await command2.ExecuteNonQueryAsync();
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

            // using var selectCommand = new MySqlCommand("SELECT * FROM User", connection);
            // using var reader = await selectCommand.ExecuteReaderAsync();
            // while (await reader.ReadAsync())
            // {
            //     var value = reader.GetValue(0);
            //     // do something with 'value'
            //     Console.WriteLine(value);
            // }

        }

        public static async Task<int> CountOccurrence(string field, string value)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var selectCommand = new MySqlCommand("SELECT COUNT(*) FROM User WHERE " + field + " = @value", Database.connection);
                selectCommand.Parameters.AddWithValue("@value", value);

                int rowCount = Convert.ToInt32(await selectCommand.ExecuteScalarAsync());
                Console.WriteLine($"Number of rows: {rowCount}");
                return rowCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task<List<string>> GetDetailsFromUsername(string username)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                List<string> list = new List<string>();

                using var selectCommand = new MySqlCommand("SELECT userid, username, phonenumber, email, name, hashedpassword FROM User WHERE username = @value", Database.connection);
                selectCommand.Parameters.AddWithValue("@value", username);

                using var reader = await selectCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(reader.GetInt32(0).ToString());
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        list.Add(reader.GetString(i));
                    }
                }

                // Console.WriteLine(string.Join(", ", list));

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task<string> getUsernameFromId(int id)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var selectCommand = new MySqlCommand("SELECT username FROM User WHERE userid = @value", Database.connection);
                selectCommand.Parameters.AddWithValue("@value", id);

                using var reader = await selectCommand.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.GetString(0);
                }
                else
                {
                    // Handle the case when no rows are returned
                    // For example, return null or throw an exception
                    // Likely where the user account is deleted
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task<List<string>> GetUserDetailsFromId(int id)
        {
            List<string> results = new List<string>();

            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var selectCommand = new MySqlCommand("SELECT userid, username, phonenumber, name, email FROM User WHERE userid = @value", Database.connection);
                selectCommand.Parameters.AddWithValue("@value", id);

                using var reader = await selectCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    List<string> temp = new List<string>();
                    temp.Add(reader.GetInt32(0).ToString());
                    temp.Add(reader.GetString(1));
                    temp.Add(reader.GetString(2));
                    temp.Add(reader.GetString(3));
                    temp.Add(reader.GetString(4));
                    results.Add(string.Join(",", temp));
                }

                Console.WriteLine(string.Join(";", results));

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }




        public static async Task registerUser(string username, string phoneNumber, string email, string name, string hashedPassword)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var insertCommand = new MySqlCommand("INSERT INTO User (username, phonenumber, email, name, hashedpassword) VALUES (@username, @phonenumber, @email, @name, @hashedpassword)", Database.connection);

                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@phonenumber", phoneNumber);
                insertCommand.Parameters.AddWithValue("@email", email);
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@hashedpassword", hashedPassword);

                await insertCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task changePassword(string username, string newHashedPassword)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var insertCommand = new MySqlCommand("UPDATE User SET hashedpassword=@hashedpassword WHERE username=@username", Database.connection);

                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@hashedpassword", newHashedPassword);

                await insertCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task updateDetails(string username, string phoneNumber, string email, string name)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var insertCommand = new MySqlCommand("UPDATE User SET phoneNumber=@phonenumber, email=@email, name=@name WHERE username=@username", Database.connection);

                insertCommand.Parameters.AddWithValue("@username", username);
                insertCommand.Parameters.AddWithValue("@phonenumber", phoneNumber);
                insertCommand.Parameters.AddWithValue("@email", email);
                insertCommand.Parameters.AddWithValue("@name", name);

                await insertCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task<List<string>> SearchFriends(string term)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                List<string> results = new List<string>();
                using var selectCommand = new MySqlCommand("SELECT userid, username, phonenumber, name, email FROM User WHERE username LIKE @value OR phonenumber LIKE @value OR email LIKE @value OR name LIKE @value", Database.connection);

                selectCommand.Parameters.AddWithValue("@value", "%" + term + "%");
                using var reader = await selectCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i += 5)
                    {

                        List<string> temp = new List<string>();
                        temp.Add(reader.GetString(i));
                        temp.Add(reader.GetString(i + 1));
                        temp.Add(reader.GetString(i + 2));
                        temp.Add(reader.GetString(i + 3));
                        temp.Add(reader.GetString(i + 4));
                        results.Add(string.Join(",", temp));
                    }
                }

                Console.WriteLine(string.Join(";", results));

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }

        public static async Task AddFriendRequest(int senderid, int receiverid)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                Console.WriteLine(senderid + " to " + receiverid);
                using var insertCommand = new MySqlCommand("INSERT INTO FriendRequest(senderid, receiverid) VALUES(@senderid, @receiverid)", Database.connection);
                insertCommand.Parameters.AddWithValue("@senderid", senderid);
                insertCommand.Parameters.AddWithValue("@receiverid", receiverid);

                await insertCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }


        public static async Task AcceptFriendRequest(int senderid, int receiverid)
        {
            using var transaction = Database.connection.BeginTransaction();

            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var insertCommand = new MySqlCommand("INSERT INTO Friendship(userid1, userid2) VALUES(@senderid, @receiverid)", transaction.Connection);
                insertCommand.Parameters.AddWithValue("@senderid", senderid);
                insertCommand.Parameters.AddWithValue("@receiverid", receiverid);

                await insertCommand.ExecuteNonQueryAsync();

                using var deleteCommand = new MySqlCommand("DELETE FROM FriendRequest WHERE senderid = @senderid AND receiverid = @receiverid", transaction.Connection);
                deleteCommand.Parameters.AddWithValue("@senderid", senderid);
                deleteCommand.Parameters.AddWithValue("@receiverid", receiverid);

                await deleteCommand.ExecuteNonQueryAsync();

                // Commit the transaction if both insert and delete are successful
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Handle any exceptions and roll back the transaction
                Console.WriteLine("An error occurred: " + ex.Message);
                transaction.Rollback();
            }
        }

        public static async Task RejectFriendRequest(int senderid, int receiverid)
        {
            using var transaction = Database.connection.BeginTransaction();
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var deleteCommand = new MySqlCommand("DELETE FROM FriendRequest WHERE senderid = @senderid AND receiverid = @receiverid", transaction.Connection);
                deleteCommand.Parameters.AddWithValue("@senderid", senderid);
                deleteCommand.Parameters.AddWithValue("@receiverid", receiverid);

                await deleteCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static async Task<List<string>> QueryFriendRequest(int userid)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                List<string> results = new List<string>();
                using var selectCommand = new MySqlCommand("SELECT senderid FROM FriendRequest WHERE receiverid = @receiverid", Database.connection);

                selectCommand.Parameters.AddWithValue("@receiverid", userid);
                using var reader = await selectCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        int senderid = reader.GetInt32(i);
                        string username = await getUsernameFromId(senderid);
                        results.Add(username);
                    }
                }

                Console.WriteLine(string.Join(", ", results));

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }

        public static async Task<List<string>> ListAllFriends(int userid)
        {
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                List<string> friendList = new List<string>();  // Fix: Use List<string> instead of List<int>
                using var selectCommand = new MySqlCommand("SELECT userid1, userid2 FROM Friendship WHERE userid1 = @userid OR userid2 = @userid", Database.connection);

                selectCommand.Parameters.AddWithValue("@userid", userid);
                using var reader = await selectCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i += 2)  // Fix: Increment i by 2 to read two userids at a time
                    {
                        int user1 = reader.GetInt32(i);
                        int user2 = reader.GetInt32(i + 1);

                        if (user1 != userid)
                        {
                            string username = await getUsernameFromId(user1);
                            friendList.Add(username);
                        }
                        else if (user2 != userid)
                        {
                            string username = await getUsernameFromId(user2);
                            friendList.Add(username);
                        }
                    }
                }

                Console.WriteLine(string.Join(", ", friendList));

                return friendList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception or rethrow it
                throw;
            }
        }


        public static async Task DeleteExistingFriend(int userid, int friendid)
        {
            using var transaction = Database.connection.BeginTransaction();
            try
            {
                Database.connection = await DbConnection.GetDbConnection();
                using var deleteCommand = new MySqlCommand("DELETE FROM Friendship WHERE (userid1 = @userid AND userid2 = @friendid) OR (userid2 = @userid AND userid1 = @friendid)", transaction.Connection);
                deleteCommand.Parameters.AddWithValue("@userid", userid);
                deleteCommand.Parameters.AddWithValue("@friendid", friendid);

                await deleteCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }

    public class DbConnection
    {
        private static MySqlConnection connection = null;

        private void DatabaseConnectionCreation() { }

        public static async Task<MySqlConnection> GetDbConnection()
        {
            // if (connection == null)
            // {
            connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");
            await connection.OpenAsync();
            // }
            return connection;
        }


    }
}
