namespace TescEvents.Utilities; 

public class DbOptions {
    public const string Db = "Db";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string Database { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public static string ConnectionString(string host, int port, string database, string user, string password) =>
        $"Host={host};Port={port};Database={database};Username={user};Password={password}";
}