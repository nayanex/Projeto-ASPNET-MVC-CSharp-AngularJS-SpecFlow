using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class TarefaEdicaoDto
    {
        #region Atributos

        [DataMember]
        public bool EdicaoStatus
        {
            get;
            set;
        }

        [DataMember]
        public DateTime DtAtualizadoEm
        {
            get;
            set;
        }

        [DataMember]
        public string TxAtualizadoPor
        {
            get;
            set;
        }

        #endregion
    }
}
