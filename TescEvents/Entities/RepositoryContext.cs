using Microsoft.EntityFrameworkCore;
using TescEvents.Models;
using TescEvents.Utilities;

namespace TescEvents.Entities; 

public class RepositoryContext : DbContext {
    public RepositoryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        if (!options.IsConfigured) {
            options.UseNpgsql(AppSettings.ConnectionString);
        }
    }

    public virtual DbSet<Event>? Events { get; set; }
    public virtual DbSet<EventRegistration>? EventRegistrations { get; set; }
    public virtual DbSet<Student>? Students { get; set; }
}