using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            // Primary Key
            this.HasKey(t => t.Oid);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .HasMaxLength(100);

            this.Property(t => t.MiddleName)
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Person");
            this.Property(t => t.Oid).HasColumnName("Oid");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.MiddleName).HasColumnName("MiddleName");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Email).HasColumnName("Email");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithOptional(t => t.Person);

        }
    }
}
