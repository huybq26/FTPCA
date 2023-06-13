using MySql.Data.MySqlClient;
using System.Data;
namespace DatabaseGroup
{
    public class Database
    {
        private static MySqlConnection connection;
        public static async Task Init(MySqlConnection connection)
        {
            Database.connection = await DbConnection.GetDbConnection();
            // using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");

            // // NOTE: Run the two below commands only one at the start, then comment them again.
            // using var command = new MySqlCommand("CREATE TABLE IF NOT EXISTS User (userid int NOT NULL AUTO_INCREMENT, username varchar(255) NOT NULL, phonenumber varchar(255) NOT NULL, email varchar(255) NOT NULL, name varchar(255) NOT NULL, hashedpassword varchar(255) NOT NULL, PRIMARY KEY (userid))", connection);
            // await command.ExecuteNonQueryAsync();


            // using var command1 = new MySqlCommand("CREATE TABLE IF NOT EXISTS FriendRequest (senderid int NOT NULL, receiverid int NOT NULL)", connection);
            // await command1.ExecuteNonQueryAsync();
            // using (var command2 = new MySqlCommand("ALTER TABLE FriendRequest ADD CONSTRAINT uc_unique_friend_pair UNIQUE (LEAST(senderid, receiverid), GREATEST(senderid, receiverid)), CHECK (senderid <> receiverid)", connection))
            // {
            //     await command2.ExecuteNonQueryAsync();
            // }
            // using var command3 = new MySqlCommand("CREATE TABLE IF NOT EXISTS Friendship (userid1 int NOT NULL, userid2 int NOT NULL)", connection);
            // await command3.ExecuteNonQueryAsync();
            // using (var command4 = new MySqlCommand("ALTER TABLE Friendship ADD CONSTRAINT uc_unique_friend_pair UNIQUE (LEAST(userid1, userid2), GREATEST(userid1, userid2)), CHECK (userid1 <> userid2)", connection))
            // {
            //     await command4.ExecuteNonQueryAsync();
            // }
            // using var insertCommand = new MySqlCommand("INSERT INTO User (username, phonenumber, email, name, hashedpassword) VALUES (@username, @phonenumber, @email, @name, @hashedpassword)", connection);

            // Set parameter values for initial user
            // insertCommand.Parameters.AddWithValue("@username", "admin2");
            // insertCommand.Parameters.AddWithValue("@phonenumber", "12345678");
            // insertCommand.Parameters.AddWithValue("@email", "admin@gmail.com");
            // insertCommand.Parameters.AddWithValue("@name", "Admin2");
            // insertCommand.Parameters.AddWithValue("@hashedpassword", "admin");

            // // Execute the insert command
            // await insertCommand.ExecuteNonQueryAsync();

            using var commandConv = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS Conversation (
        convid BINARY(16) PRIMARY KEY,
        convname varchar(255) NOT NULL,
        creationtime TIME NOT NULL,
        lastmessage TIME NOT NULL
    )", connection);
            await commandConv.ExecuteNonQueryAsync();

            using var commandConvP = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS Conv_Parti (
        convid BINARY(16) NOT NULL,
        userid INT NOT NULL,
        FOREIGN KEY (convid) REFERENCES Conversation(convid),
        FOREIGN KEY (userid) REFERENCES User(userid),
        INDEX fk_convid_idx (convid),
        INDEX fk_userid_idx (userid)
    )", connection);
            await commandConvP.ExecuteNonQueryAsync();

            using var commandMess = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS Message (
        messageid BINARY(16) NOT NULL PRIMARY KEY,
        convid BINARY(16) NOT NULL,
        senderid INT NOT NULL,
        content varchar(1000) NOT NULL,
        timestampt TIME NOT NULL,
        fileid BINARY(16),
        FOREIGN KEY (convid) REFERENCES Conversation(convid),
        FOREIGN KEY (senderid) REFERENCES User(userid),
        INDEX fk_convid_idx (convid),
        INDEX fk_senderid_idx (senderid)
    )", connection);
            await commandMess.ExecuteNonQueryAsync();

            using var commandFile = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS File (
        fileid BINARY(16) PRIMARY KEY,
        filename VARCHAR(255) NOT NULL,
        filepath VARCHAR(255) NOT NULL,
        filesize INT NOT NULL,
        filetype VARCHAR(50) NOT NULL
    )", connection);
            await commandFile.ExecuteNonQueryAsync();

            if (Database.connection.State != ConnectionState.Closed)
            {
                Database.connection.Close();
            }

        }


        public static async Task<int> CountOccurrence(string field, string value)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task<List<string>> GetDetailsFromUsername(string username)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task<string> getUsernameFromId(int id)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task<List<string>> GetUserDetailsFromId(int id)
        {
            List<string> results = new List<string>();
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }




        public static async Task registerUser(string username, string phoneNumber, string email, string name, string hashedPassword)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task changePassword(string username, string newHashedPassword)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task updateDetails(string username, string phoneNumber, string email, string name)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task<List<string>> SearchFriends(string term)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }


        public static async Task AcceptFriendRequest(int senderid, int receiverid)
        {
            Database.connection = await DbConnection.GetDbConnection();
            using var transaction = Database.connection.BeginTransaction();

            try
            {

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
            Database.connection = await DbConnection.GetDbConnection();
            using var transaction = Database.connection.BeginTransaction();
            try
            {

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
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task<List<string>> ListAllFriends(int userid)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {

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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }


        public static async Task DeleteExistingFriend(int userid, int friendid)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                using var deleteCommand = new MySqlCommand("DELETE FROM Friendship WHERE (userid1 = @userid AND userid2 = @friendid) OR (userid2 = @userid AND userid1 = @friendid)", Database.connection);
                deleteCommand.Parameters.AddWithValue("@userid", userid);
                deleteCommand.Parameters.AddWithValue("@friendid", friendid);

                await deleteCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }

        public static async Task<string> CheckRelationshipStatus(int userid1, int userid2)
        {
            try
            {
                using (var connection = await DbConnection.GetDbConnection())
                {
                    using (var selectCommand = new MySqlCommand("SELECT userid1, userid2 FROM Friendship WHERE (userid1 = @userid1 AND userid2 = @userid2) OR (userid1 = @userid2 AND userid2=@userid1)", connection))
                    {
                        selectCommand.Parameters.AddWithValue("@userid1", userid1);
                        selectCommand.Parameters.AddWithValue("@userid2", userid2);

                        using (var reader = await selectCommand.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Friend");
                                return "Friend";
                            }
                        }
                    }

                    using (var selectCommand = new MySqlCommand("SELECT senderid, receiverid FROM FriendRequest WHERE senderid = @userid1 AND receiverid = @userid2", connection))
                    {
                        selectCommand.Parameters.AddWithValue("@userid1", userid1);
                        selectCommand.Parameters.AddWithValue("@userid2", userid2);

                        using (var reader = await selectCommand.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Cancel Friend Request");
                                return "Cancel Friend Request";
                            }
                        }
                    }

                    using (var selectCommand = new MySqlCommand("SELECT senderid, receiverid FROM FriendRequest WHERE senderid = @userid1 AND receiverid = @userid2", connection))
                    {
                        selectCommand.Parameters.AddWithValue("@userid1", userid2);
                        selectCommand.Parameters.AddWithValue("@userid2", userid1);

                        using (var reader = await selectCommand.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Accept Friend Request");
                                return "Accept Friend Request";
                            }
                        }
                    }
                }
                Console.WriteLine("Add Friend");
                return "Add Friend";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
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
