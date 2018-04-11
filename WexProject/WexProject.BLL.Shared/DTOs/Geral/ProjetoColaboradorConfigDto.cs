using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Geral
{
    [DataContract]
    public class ProjetoColaboradorConfigDto
    {

        #region Atributos

        [DataMember]
        public int CorColaborador { get; set; }

        [DataMember]
        public Guid OidColaborador { get; set; }

        [DataMember]
        public Guid OidUsuario { get; set; }

        [DataMember]
        public string NomeCompletoColaborador { get; set; }

        [DataMember]
        public string Login { get; set; }

        #endregion

    }
}
