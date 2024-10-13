using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendalicce.Domain.Chats;
using Rendalicce.Domain.ServiceTransactions;

namespace Rendalicce.Persistency.EntityTypeConfigurations;

public class ServiceTransactionConfiguration : IEntityTypeConfiguration<ServiceTransaction>
{
    public void Configure(EntityTypeBuilder<ServiceTransaction> builder)
    {
        builder.HasMany(c => c.Participants).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}