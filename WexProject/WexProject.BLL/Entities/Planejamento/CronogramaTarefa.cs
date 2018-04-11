using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class CronogramaTarefa
    {
        #region Construtor

        public CronogramaTarefa()
        {
            Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public Guid Oid { get; set; }

        public short NbID { get; set; }

        public bool CsExcluido { get; set; }

        #endregion

        #region Chaves Estrangeiras

        [ForeignKey( "Tarefa" )]
        public Guid OidTarefa { get; set; }

        [Column( "Cronograma" ), ForeignKey( "Cronograma" )]
        public Guid OidCronograma { get; set; }

        #endregion

        #region Não Mapeado

        [NotMapped]
        public short NbIdAntigo { get; set; }

        #endregion

        #region Propriedades Navegacionais

        public Cronograma Cronograma { get; set; }

        public Tarefa Tarefa { get; set; }
        #endregion

        /// <summary>
        /// Retorna um clone da instancia atual
        /// </summary>
        /// <returns></returns>
        public CronogramaTarefa Clone()
        {
            return MemberwiseClone() as CronogramaTarefa;
        }
    }
}
