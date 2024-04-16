using Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.DAL.Configurations;
public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired().HasMaxLength(100);
        builder.HasMany(u => u.Ads)
            .WithOne(a => a.User)
            .HasForeignKey(fk => fk.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}