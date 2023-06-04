using Bridge.Domain.Entities.Dog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bridge.Infrastructure.Persistance.Configuration;

public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.HasIndex(a => a.Name)
            .IsUnique();
        builder.Property(a => a.Name)
         .IsRequired();

        builder.Property(a => a.Color)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.TailLength)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Weight)
            .IsRequired();

        builder.Property(a => a.Id)
            .HasConversion(id => id.Value, value => new DogId(value));
    }
}
