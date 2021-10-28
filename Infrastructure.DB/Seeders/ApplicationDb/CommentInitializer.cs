using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class CommentInitializer : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            
            builder
                .HasOne( comment => comment.isPostedOn)
                .WithMany(dossier => dossier.Comments)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasOne( comment => comment.CreatedBy)
                .WithMany(user => user.CommentsCreated)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}