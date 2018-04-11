using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.Entities.Planejamento
{
    [Table("CronogramaUltimaSelecao")]
    public class CronogramaUltimaSelecao
    {
        #region Construtores

        public CronogramaUltimaSelecao()
        {
            Oid = Guid.NewGuid();
        } 

        #endregion

        [Key]
        public Guid Oid { get; set; }

        [Column("DtAcesso")]
        public DateTime? DataAcesso { get; set; }

        #region Chaves Estrangeiras
        [Column( "UltimoCronograma" ), ForeignKey( "Cronograma" )]
        public Guid OidUltimoCronograma { get; set; }

        [Column( "Usuario" ), ForeignKey( "Usuario" )]
        public Guid OidUsuario { get; set; } 
        #endregion

        #region Propriedades Navegacionais

        public Cronograma Cronograma { get; set; }

        public User Usuario { get; set; } 

        #endregion
    }
}
