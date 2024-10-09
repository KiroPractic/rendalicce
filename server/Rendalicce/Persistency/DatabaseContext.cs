using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.ApplicationSettings;
using Rendalicce.Domain.Users;
using Rendalicce.Persistency.EntityTypeConfigurations;

namespace Rendalicce.Persistency;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    
    public required DbSet<ApplicationSettings> ApplicationSettings { get; init; }
    public required DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}