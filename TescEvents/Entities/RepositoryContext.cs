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
    
    public DbSet<Event>? Events { get; set; }
    public DbSet<Student>? Students { get; set; }
}