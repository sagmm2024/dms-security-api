using Microsoft.EntityFrameworkCore;
using SecurityApi.Domain.Entities;
using System.Reflection;

namespace SecurityApi.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        this.ChangeTracker.AutoDetectChangesEnabled = true;
    }
    public DbSet<Person> Person { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
