using Microsoft.EntityFrameworkCore;
using TescEvents.Models;
using TescEvents.Utilities;

namespace TescEvents.Entities; 

public class RepositoryContext : DbContext {
    public RepositoryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseNpgsql(AppSettings.ConnectionString);
    }
    
    public DbSet<Event>? Events { get; set; }
}