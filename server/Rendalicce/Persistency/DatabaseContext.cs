using Microsoft.EntityFrameworkCore;
using Rendalicce.Domain.ApplicationSettings;
using Rendalicce.Domain.Chats;
using Rendalicce.Domain.Reviews;
using Rendalicce.Domain.ServiceSeekers;
using Rendalicce.Domain.Users;
using Rendalicce.Persistency.EntityTypeConfigurations;
using ServiceProvider = Rendalicce.Domain.ServiceProviders.ServiceProvider;

namespace Rendalicce.Persistency;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    
    public required DbSet<ApplicationSettings> ApplicationSettings { get; init; }
    
    public required DbSet<Chat> Chats { get; init; }
    public required DbSet<Review> Reviews { get; init; }
    public required DbSet<ServiceProvider> ServiceProviders { get; init; }
    public required DbSet<ServiceSeeker> ServiceSeekers { get; init; }
    public required DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}