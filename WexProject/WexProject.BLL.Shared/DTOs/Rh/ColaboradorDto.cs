using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Rh
{
    public class ColaboradorDto
    {
        #region Atributos

        /// <summary>
        /// Oid do colaborador
        /// </summary>
        [DataMember]
        public Guid OidColaborador
        {
            set;
            get;
        }

        /// <summary>
        /// Oid do usuário do colaborador
        /// </summary>
        [DataMember]
        public Guid OidUsuario
        {
            set;
            get;
        }

        /// <summary>
        /// Matrícula do colaborador
        /// </summary>
        [DataMember]
        public string TxMatriculaColaborador
        {
            set;
            get;
        }

        /// <summary>
        /// Nome completo colaborador
        /// </summary>
        [DataMember]
        public string TxNomeCompletoColaborador
        {
            set;
            get;
        }

        [DataMember]
        public string Login
        {
            set;
            get;
        }

        /// <summary>
        /// Cargo na empresa.
        /// </summary>
        public string TxCargo
        {
            set;
            get;
        }

        /// <summary>
        /// Primeiro nome do colaborador.
        /// </summary>
        public string TxPrimeiroNome
        {
            set;
            get;
        }

        #endregion
    }
}
