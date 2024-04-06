using Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastrucure.DAL.Configurations;

public class AdsConfiguration:IEntityTypeConfiguration<Ads>
{
    public void Configure(EntityTypeBuilder<Ads> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Text)
            .IsRequired().HasMaxLength(100);
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.ExpirationDate).IsRequired();


    }
}