using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Planejamento
{
    [Table( "CronogramaColaboradorConfig" )]
    public class CronogramaColaboradorConfig
    {
        #region Construtores
        
        public CronogramaColaboradorConfig()
        {
            Oid = Guid.NewGuid();
        } 

        #endregion

        #region Propriedades

        [Key]
        public Guid Oid { get; set; }

        public int? Cor { get; set; } 

        #endregion

        #region Chaves estrangeiras

        [Column("Cronograma"),ForeignKey("Cronograma")]
        public Guid OidCronograma { get; set; }

        [Column("Colaborador"),ForeignKey("Colaborador")]
        public Guid OidColaborador { get; set; } 

        #endregion

        #region Propriedades Navegacionais

        public virtual Cronograma Cronograma { get; set; }

        public virtual Colaborador Colaborador { get; set; }

        #endregion
    }
}
