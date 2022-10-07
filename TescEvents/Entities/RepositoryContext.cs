using Microsoft.EntityFrameworkCore;
using TescEvents.Models;

namespace TescEvents.Entities; 

public class RepositoryContext : DbContext {
    public RepositoryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("DB_DATABASE");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var pass = Environment.GetEnvironmentVariable("DB_PASS");
        options.UseNpgsql($"Host={host};Port={port};Database={database};Username={user};Password={pass}");
    }
    
    public DbSet<Event>? Events { get; set; }
}