using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Mapping
{
    public class ProjetoClienteMap : EntityTypeConfiguration<ProjetoCliente>
    {
        public ProjetoClienteMap()
        {

            this.HasKey(t => t.Oid);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProjetoCliente");

            this.Property(t => t.IdProjeto).HasColumnName("Projeto");
            this.Property(t => t.IdCliente).HasColumnName("Cliente");

            // Relationships
            this.HasOptional(t => t.EmpresaInstituicao)
                .WithMany(t => t.ProjetoClientes)
                .HasForeignKey(d => d.IdCliente);

            this.HasOptional(t => t.Projeto)
                .WithMany(t => t.ProjetoClientes)
                .HasForeignKey(d => d.IdProjeto);

        }
    }
}
