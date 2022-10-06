using Microsoft.EntityFrameworkCore;
using TescEvents.Models;

namespace TescEvents.Entities; 

public class RepositoryContext : DbContext {
    public RepositoryContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Event>? Events { get; set; }
}