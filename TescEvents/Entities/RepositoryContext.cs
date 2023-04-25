using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TescEvents.Models;
using TescEvents.Utilities;

namespace TescEvents.Entities; 

public class RepositoryContext : DbContext {
    private readonly DbOptions dbOptions;

    public RepositoryContext(IOptions<DbOptions> dbOptions, DbContextOptions options) : base(options) {
        this.dbOptions = dbOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var connectionString = DbOptions.ConnectionString(dbOptions.Host, dbOptions.Port, dbOptions.Database, dbOptions.User, 
                                                      dbOptions.Password);
        options.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Event>()
                    .HasData(
                             new Event {
                                 Id = Guid.NewGuid(),
                                 Title = "A New Event",
                                 Description = "Come out to our New Event!",
                                 Start = new DateTime(2020, 2, 20, 20, 20, 20).ToUniversalTime(),
                                 End = new DateTime(2020, 2, 20, 23, 20, 20).ToUniversalTime(),
                                 RequiresResume = false
                             });
    }
    
    public DbSet<Event>? Events { get; set; }
    public DbSet<Student>? Students { get; set; }
    public DbSet<EventRegistration>? EventRegistrations { get; set; }
}