using Example.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Database;

public sealed class AppDbContext : DbContext
{
    public DbSet<ExampleModel> Models { get; private set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}