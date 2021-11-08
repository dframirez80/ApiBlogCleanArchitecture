using Domain.Repository.Entities;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Repositories.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.Names).IsRequired().HasMaxLength(Constraints.MaxLengthNames);
            builder.Property(u => u.Surnames).IsRequired().HasMaxLength(Constraints.MaxLengthSurnames);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(Constraints.MaxLengthEmails);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(Constraints.MaxLengthPassword);
        }
    }
}
