using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityApi.Domain.Entities;

namespace SecurityApi.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("person");
        builder.HasKey(a => a.PersonId);
        builder.Property(a => a.PersonId).ValueGeneratedOnAdd();
        builder.Property(e => e.Nombre);
        builder.Property(e => e.Apellido);

        builder.HasData(
            new Person
            {
                PersonId = 1,
                Nombre = "nombreTest",
                Apellido = "apellidoTest"
            },
            new Person
            {
                PersonId = 2,
                Nombre = "nombreTest2",
                Apellido = "apellidoTest2"
            },
            new Person
            {
                PersonId = 3,
                Nombre = "nombreTest3",
                Apellido = "apellidoTest3"
            }
            );
    }
}
