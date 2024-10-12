using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendalicce.Domain.Chats;

namespace Rendalicce.Persistency.EntityTypeConfigurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasMany(c => c.Messages).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Participants).WithMany();
    }
}