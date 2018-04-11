using System;
using System.Collections.Generic;
using System.Linq;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class TarefaLogAlteracaoDto
    {
        #region Propriedades
        
        /// <summary>
        /// Propriedade que armazeno o login do usuário que alterou a tarefa
        /// </summary>
        public string descricaoColaborador { get; set; }

        /// <summary>
        /// Data e Hora de alteração
        /// </summary>
        public DateTime DtDataEHora { get; set; }

        public string Hora
        {
            get
            {
                return DtDataEHora.ToShortTimeString();
            }
        }

        /// <summary>
        /// Descrição textual das alterações
        /// </summary>
        public string descricaoAlteracao { get; set; }

       #endregion
    }
}
