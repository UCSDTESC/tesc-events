using Microsoft.EntityFrameworkCore;
using TescEvents.Models;
using TescEvents.Utilities;

namespace TescEvents.Entities; 

public class RepositoryContext : DbContext {
    public RepositoryContext(DbContextOptions options) : base(options) {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        if (!options.IsConfigured)
            options.UseNpgsql(AppSettings.ConnectionString);
    }
    
    public DbSet<Batch>? Batches { get; set; }
    public DbSet<EventCode>? EventCodes { get; set; }
    public DbSet<User>? Users { get; set; }
}