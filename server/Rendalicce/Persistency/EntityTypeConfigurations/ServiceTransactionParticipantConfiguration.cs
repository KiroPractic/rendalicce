using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendalicce.Domain.ServiceTransactions;

namespace Rendalicce.Persistency.EntityTypeConfigurations;

public class ServiceTransactionParticipantConfiguration : IEntityTypeConfiguration<ServiceTransactionParticipant>
{
    public void Configure(EntityTypeBuilder<ServiceTransactionParticipant> builder)
    {
        builder.HasOne(stp => stp.User).WithMany().OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne(
            stp => stp.ProvidedService, 
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });
    }
}