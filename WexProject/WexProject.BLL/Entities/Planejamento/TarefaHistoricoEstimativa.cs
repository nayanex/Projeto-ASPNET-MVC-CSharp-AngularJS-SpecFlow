using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Entities.Planejamento
{
    [Table("TarefaHistoricoEstimativa")]
    public class TarefaHistoricoEstimativa
    {
        public TarefaHistoricoEstimativa()
        {
            Oid = Guid.NewGuid();
        }

        #region Propriedades

        [Key]
        public Guid Oid { get; set; }

        public DateTime DtPlanejado { get; set; }

        public double NbHoraRestante { get; set; }

        [NotMapped]
        public TimeSpan HoraRestante { get { return TimeSpan.FromTicks( (long)NbHoraRestante ); } set { NbHoraRestante = value.Ticks; } }

        #endregion

        #region Chaves Estrangeiras

        [Column( "Tarefa" ), ForeignKey( "Tarefa" )]
        public Guid OidTarefa { get; set; } 

        #endregion

        #region Propriedades Navegacionais

        public Tarefa Tarefa { get; set; }

        #endregion
    }
}
