using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class TarefaLogAlteracao
    {
        #region Construtor

        public TarefaLogAlteracao()
        {
            Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public System.Guid Oid { get; set; }
        public Nullable<System.DateTime> DtDataHoraLog { get; set; }
        
        public string TxAlteracoes { get; set; }

        #endregion

        #region Chaves Estrangeiras

        [Column( "Tarefa" ), ForeignKey( "Tarefa" )]
        public Nullable<Guid> OidTarefa { get; set; }

        [Column( "Colaborador" ), ForeignKey( "Colaborador" )]
        public Nullable<Guid> OidColaborador { get; set; }

        #endregion

        #region Propriedades Navegacionais

        public Colaborador Colaborador { get; set; }
        public Tarefa Tarefa { get; set; }

        #endregion

        /// <summary>
        /// Retornar um clone da instancia atual
        /// </summary>
        /// <returns></returns>
        public TarefaLogAlteracao Clone() 
        {
            return MemberwiseClone() as TarefaLogAlteracao;
        }
    }
}
