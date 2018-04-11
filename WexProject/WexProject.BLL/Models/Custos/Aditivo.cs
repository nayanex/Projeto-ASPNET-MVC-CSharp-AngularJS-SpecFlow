using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Models.Custos
{
	public class Aditivo : IValidatableObject
	{
		public Aditivo()
		{
			this.Rubricas = new List<Rubrica>();
		}

		[Key]
		public int AditivoId { get; set; }

		[Required]
		public String TxNome { get; set; }

		[Required]
		[Display(Name = "Data de Início")]
		public DateTime DtInicio { get; set; }

		[Required]
		[Display(Name = "Data de Termino")]
		public DateTime DtTermino { get; set; }

		public int NbDuracao { get; set; }

		[Required]
		public Decimal NbOrcamento { get; set; }

		[ForeignKey("Projeto")]
		public Guid ProjetoOid { get; set; }

		public virtual Projeto Projeto { get; set; }
		public virtual ICollection<Rubrica> Rubricas { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (DateTime.Compare(DtInicio, DtTermino) >= 0)
			{
				yield return new ValidationResult("Data de Início não pode ser posterior à Data de Termino");
			}
		}
	}
}