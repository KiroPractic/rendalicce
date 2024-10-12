
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendalicce.Domain.Chats;

namespace Rendalicce.Persistency.EntityTypeConfigurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasMany(c => c.SeenByParticipants).WithMany();
    }
}