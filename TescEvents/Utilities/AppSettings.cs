namespace TescEvents.Utilities; 

public static class AppSettings {
    public static string ConnectionString {
        get {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("DB_DATABASE");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASS");
            return $"Host={host};Port={port};Database={database};Username={user};Password={pass}";
        }
    }

    public static string ResumeBucket => Environment.GetEnvironmentVariable("RESUME_BUCKET");
}