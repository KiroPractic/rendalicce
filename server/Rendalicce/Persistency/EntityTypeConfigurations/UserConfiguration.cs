using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendalicce.Domain.Users;

namespace Rendalicce.Persistency.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property<string>("PasswordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();
    }
}