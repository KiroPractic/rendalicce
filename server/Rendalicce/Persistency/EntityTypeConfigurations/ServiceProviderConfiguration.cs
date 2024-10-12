
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rendalicce.Persistency.EntityTypeConfigurations;

public class ServiceProviderConfiguration : IEntityTypeConfiguration<Domain.ServiceProviders.ServiceProvider>
{
    public void Configure(EntityTypeBuilder<Domain.ServiceProviders.ServiceProvider> builder)
    {
        builder.HasMany(sp => sp.Reviews)
            .WithOne()
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}