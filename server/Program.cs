using MySql.Data.MySqlClient;
using DatabaseGroup;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
// Route handler
app.MapGet("/initdb", async (context) =>
{
    // Call the Init method
    MySqlConnection connection = await DbConnection.GetDbConnection();
    await Database.Init(connection);

    // Return a response
    await context.Response.WriteAsync("Database initialization completed.");
});


app.Run();


// using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");

// static async Task Init()
// {
//     using var connection = new MySqlConnection("Server=localhost;User ID=huybq;Password=Qwerty26!;Database=FTPCA");
//     await connection.OpenAsync();

//     using var command = new MySqlCommand("CREATE TABLE IF NOT EXISTS user (username varchar(255), phonenumber varchar(255), email varchar(255), name varchar(255), hashedpassword varchar(255))", connection);
//     await command.ExecuteNonQueryAsync();

//     using var insertCommand = new MySqlCommand("INSERT INTO user (username, phonenumber, email, name, hashedpassword) VALUES (@username, @phonenumber, @email, @name, @hashedpassword)", connection);

//     // Set parameter values for initial user
//     insertCommand.Parameters.AddWithValue("@username", "john");
//     insertCommand.Parameters.AddWithValue("@phonenumber", "12345434");
//     insertCommand.Parameters.AddWithValue("@email", "afsdfa@maafs.com");
//     insertCommand.Parameters.AddWithValue("@name", "John Doe");
//     insertCommand.Parameters.AddWithValue("@hashedpassword", "@hashedpassword");

//     // Execute the insert command
//     await insertCommand.ExecuteNonQueryAsync();

//     // using var selectCommand = new MySqlCommand("SELECT * FROM User", connection);
//     // using var reader = await selectCommand.ExecuteReaderAsync();
//     // while (await reader.ReadAsync())
//     // {
//     //     var value = reader.GetValue(0);
//     //     // do something with 'value'
//     //     Console.WriteLine(value);
//     // }
// }


