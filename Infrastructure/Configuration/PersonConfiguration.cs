using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class PersonConfiguration:IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.FirstName).HasMaxLength(20);
        builder.Property(q => q.LastName).HasMaxLength(50);
    }
}