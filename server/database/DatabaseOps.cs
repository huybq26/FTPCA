using MySql.Data.MySqlClient;
using System.Data;
using System;
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
        convid VARCHAR(36) PRIMARY KEY,
        convname varchar(255) NOT NULL,
        creationtime DATETIME NOT NULL,
        lastmessage varchar(1000),
        lastsender INT,
    )", connection);
            await commandConv.ExecuteNonQueryAsync();

            using var commandConvP = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS Conv_Parti (
        convid VARCHAR(36) NOT NULL,
        userid INT NOT NULL,
        FOREIGN KEY (convid) REFERENCES Conversation(convid),
        FOREIGN KEY (userid) REFERENCES User(userid),
        INDEX fk_convid_idx (convid),
        INDEX fk_userid_idx (userid)
    )", connection);
            await commandConvP.ExecuteNonQueryAsync();

            using var commandMess = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS Message (
        messageid VARCHAR(36) NOT NULL PRIMARY KEY,
        convid VARCHAR(36) NOT NULL,
        senderid INT NOT NULL,
        content varchar(1000) NOT NULL,
        timestampt DATETIME NOT NULL,
        fileid VARCHAR(36),
        FOREIGN KEY (convid) REFERENCES Conversation(convid),
        FOREIGN KEY (senderid) REFERENCES User(userid),
        INDEX fk_convid_idx (convid),
        INDEX fk_senderid_idx (senderid)
    )", connection);
            await commandMess.ExecuteNonQueryAsync();

            using var commandFile = new MySqlCommand(@"
    CREATE TABLE IF NOT EXISTS File (
        fileid VARCHAR(36) PRIMARY KEY,
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
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
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

        public static async Task<string> CreateConversation(string convname, List<int> participants)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                using var insertCommand = new MySqlCommand("INSERT INTO Conversation (convid, convname, creationtime, lastmessage) VALUES (@convid, @convname, @creationtime, @lastmessage)", Database.connection);
                Guid uuid = Guid.NewGuid();
                string uuidString = uuid.ToString();

                insertCommand.Parameters.AddWithValue("@convid", uuidString);
                insertCommand.Parameters.AddWithValue("@convname", convname);
                insertCommand.Parameters.AddWithValue("@creationtime", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@lastmessage", DateTime.Now);

                await insertCommand.ExecuteNonQueryAsync();

                await Database.AddParticipants(uuidString, participants);
                return uuidString;
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

        public static async Task AddParticipants(string convid, List<int> participants)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                foreach (int userid in participants)
                {
                    using var insertCommand = new MySqlCommand("INSERT INTO Conv_Parti (convid, userid) VALUES (@convid, @userid)", Database.connection);

                    insertCommand.Parameters.AddWithValue("@convid", convid);
                    insertCommand.Parameters.AddWithValue("@userid", userid);

                    await insertCommand.ExecuteNonQueryAsync();
                }
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

        public static async Task<List<List<string>>> QueryConversation(int userid, DateTime lastMessageTime)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                List<List<string>> Conversations = new List<List<string>>();
                using var selectCommand = new MySqlCommand("SELECT Conversation.convid, Conversation.convname FROM Conversation LEFT JOIN Conv_Parti ON Conv_Parti.convid = Conversation.convid WHERE Conversation.lastmessage < @lastmessage AND userid = @userid ORDER BY Conversation.lastmessage DESC LIMIT 25", Database.connection);

                selectCommand.Parameters.AddWithValue("@lastmessage", lastMessageTime);
                selectCommand.Parameters.AddWithValue("@userid", userid);
                using var reader = await selectCommand.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    List<string> conversation = new List<string>
                    {
                        reader.GetString(0), // Assuming the first column is convid
                        reader.GetString(1)  // Assuming the second column is convname
                    };
                    Conversations.Add(conversation);
                }

                Console.WriteLine(string.Join(", ", Conversations));

                return Conversations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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

        public static async Task<List<List<string>>> GetParticipantsFromConv(string convid)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                List<List<string>> results = new List<List<string>>();
                using var selectCommand = new MySqlCommand("SELECT User.userid, User.username, User.name FROM User LEFT JOIN Conv_Parti ON User.userid = Conv_Parti.userid WHERE Conv_Parti.convid = @convid", Database.connection);

                selectCommand.Parameters.AddWithValue("@convid", convid);
                using var reader = await selectCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.IsDBNull(i))
                        {
                            temp.Add(null); // Handle null values if necessary
                        }
                        else
                        {
                            if (i == 0) // Assuming the first column is the userid
                            {
                                temp.Add(reader.GetInt32(i).ToString()); // Convert the integer value to string
                            }
                            else
                            {
                                temp.Add(reader.GetString(i)); // GetString for string columns
                            }
                        }
                    }
                    results.Add(temp);
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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


        public static async Task RemoveParticipant(string convid, int participantId)
        {
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                using var insertCommand = new MySqlCommand("DELETE FROM Conv_Parti WHERE userid = @userid", Database.connection);

                insertCommand.Parameters.AddWithValue("@userid", participantId);

                await insertCommand.ExecuteNonQueryAsync();

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

        public static async Task SendMessage(int senderid, string convid, string content, string fileId)
        {
            // Check if the sender is a participant of the conversation
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                using var checkParticipantCommand = new MySqlCommand("SELECT COUNT(*) FROM Conv_Parti WHERE convid = @convid AND userid = @userid", Database.connection);
                checkParticipantCommand.Parameters.AddWithValue("@convid", convid);
                checkParticipantCommand.Parameters.AddWithValue("@userid", senderid);
                object participantCountObj = await checkParticipantCommand.ExecuteScalarAsync();

                if (participantCountObj != null)
                {
                    int participantCount = Convert.ToInt32(participantCountObj);

                    if (participantCount == 0)
                    {
                        // Sender is not a participant of the conversation
                        Console.WriteLine("Unauthorized: Sender is not a participant of the conversation.");
                        return;
                    }
                }
                else
                {
                    // Error occurred while retrieving the participant count
                    Console.WriteLine("Unable to determine the participant count.");
                    return;
                }

                // Sender is a participant, proceed with inserting the message
                using var transaction = Database.connection.BeginTransaction();

                try
                {
                    using var insertCommand = new MySqlCommand("INSERT INTO Message (messageid, convid, senderid, content, fileid, timestampt) VALUES (@messageid, @convid, @senderid, @content, @fileid, @timestampt)", transaction.Connection);
                    Guid uuid = Guid.NewGuid();
                    string uuidString = uuid.ToString();
                    insertCommand.Parameters.AddWithValue("@messageid", uuidString);
                    insertCommand.Parameters.AddWithValue("@convid", convid);
                    insertCommand.Parameters.AddWithValue("@senderid", senderid);
                    insertCommand.Parameters.AddWithValue("@content", content);
                    insertCommand.Parameters.AddWithValue("@fileid", fileId);
                    insertCommand.Parameters.AddWithValue("@timestampt", DateTime.Now);
                    await insertCommand.ExecuteNonQueryAsync();

                    using var updateCommand = new MySqlCommand("UPDATE Conversation SET lastmessage = @lastmessage AND lastsender=@lastsender WHERE convid = @convid", transaction.Connection);
                    updateCommand.Parameters.AddWithValue("@lastmessage", DateTime.Now);
                    updateCommand.Parameters.AddWithValue("@lastsender", senderid);
                    updateCommand.Parameters.AddWithValue("@convid", convid);

                    await updateCommand.ExecuteNonQueryAsync();

                    // Commit the transaction if both insert and update are successful
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions and roll back the transaction
                    Console.WriteLine("An error occurred: " + ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    if (Database.connection.State != ConnectionState.Closed)
                    {
                        Database.connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                if (Database.connection.State != ConnectionState.Closed)
                {
                    Database.connection.Close();
                }
            }
        }


        public static async Task<List<List<string>>> QueryMessage(string convid, DateTime lastMessageTime)
        {
            // return the name (not username) of the sender, content and sent time
            Database.connection = await DbConnection.GetDbConnection();
            try
            {
                List<List<string>> Messages = new List<List<string>>();
                using var selectCommand = new MySqlCommand("SELECT Message.messageid, User.name, Message.content, Message.timestampt FROM Message LEFT JOIN User ON Message.senderid = User.userid WHERE Message.convid = @convid AND Message.timestampt < @lastmessage ORDER BY Message.timestampt LIMIT 25", Database.connection);
                selectCommand.Parameters.AddWithValue("@convid", convid);
                selectCommand.Parameters.AddWithValue("@lastmessage", lastMessageTime);
                using var reader = await selectCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    List<string> message = new List<string>
                    {
                        reader.GetString(0), // Assuming the first column is convid
                        reader.GetString(1),  // Assuming the second column is convname
                        reader.GetString(2),
                        reader.GetString(3)
                    };
                    Messages.Add(message);
                }

                Console.WriteLine(string.Join(", ", Messages));

                return Messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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

        public static async Task UploadFile(string fileid, string filename, string filepath, int filesize, string filetype)
        {
            Database.connection = await DbConnection.GetDbConnection();

            try
            {
                using var insertCommand = new MySqlCommand("INSERT INTO File VALUES (@fileid, @filename, @filepath, @filesize, @filetype)", Database.connection);
                insertCommand.Parameters.AddWithValue("@fileid", fileid);
                insertCommand.Parameters.AddWithValue("@filename", filename);
                insertCommand.Parameters.AddWithValue("@filepath", filepath);
                insertCommand.Parameters.AddWithValue("@filesize", filesize);
                insertCommand.Parameters.AddWithValue("@filetype", filetype);
                await insertCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                // Handle any exceptions and roll back the transaction
                Console.WriteLine("An error occurred: " + ex.Message);
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
