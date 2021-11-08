using Domain.Constants;
using Domain.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Repositories.EntityConfigurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder) {
            builder.HasKey(a => a.ArticleId);
            builder.Property(a => a.Content).IsRequired().HasMaxLength(Constraints.MaxLengthContent);
            builder.Property(a => a.Keyword).IsRequired().HasMaxLength(Constraints.MaxLengthKeyword);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(Constraints.MaxLengthTitle);
            
            builder.HasOne(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Comments).WithOne(c => c.Article).HasForeignKey(a => a.ArticleId);
        }
    }
}
