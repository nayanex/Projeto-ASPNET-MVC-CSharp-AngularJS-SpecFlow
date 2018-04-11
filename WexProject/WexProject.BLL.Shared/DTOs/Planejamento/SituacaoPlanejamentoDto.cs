using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using WexProject.BLL.Shared.Domains.Planejamento;
using System.Windows.Forms;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class SituacaoPlanejamentoDTO
    {
        #region Atributos

        /// <summary>
        /// Oid da Situação Planejamento
        /// </summary>
        public Guid Oid
        {
            get;
            set;
        }

        /// <summary>
        /// Nome da Situação de Planejamento
        /// </summary>
        public string TxDescricao
        {
            get;
            set;
        }

        /// <summary>
        /// Tipo da situação de planejamento
        /// </summary>
        public CsTipoSituacaoPlanejamento CsSituacao
        {
            get;
            set;
        }

        /// <summary>
        /// Tipo da situação de planejamento
        /// </summary>
        public CsTipoPlanejamento CsTipo
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CsPadraoSistema CsPadrao
        {
            get;
            set;
        }

        /// <summary>
        /// Atalhos da situação planejamento
        /// </summary>
        public Shortcut KeyPress
        {
            get;
            set;
        }

        /// <summary>
        /// Atalhos possíveis na situação planejamento
        /// </summary>
        public string TxKeys
        {
            get;
            set;
        }

        /// <summary>
        /// Atributo que contém imagem em array de binários da situação planejamento
        /// </summary>
        public byte[] BlImagemSituacaoPlanejamento
        {
            set;
            get;
        }


        #endregion
    }
}
