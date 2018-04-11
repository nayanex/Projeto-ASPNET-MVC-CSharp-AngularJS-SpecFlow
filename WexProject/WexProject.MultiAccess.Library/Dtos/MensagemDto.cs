using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WexProject.MultiAccess.Library.Domains;

namespace WexProject.MultiAccess.Library.Dtos
{
    public class MensagemDto
    {
        #region Propriedades
        /// <summary>
        /// Armazena as propriedades da mensagem a serem serializadas e enviadas pela mensagem
        /// </summary>
        public Hashtable Propriedades { get; set; }
        /// <summary>
        /// Enum tipo Mensagem
        /// </summary>
        public CsTipoMensagem Tipo { get; set; }
        #endregion

        #region Métodos

        /// <summary>
        /// Efetuar clone da mensagem
        /// </summary>
        /// <returns></returns>
        public MensagemDto Clone()
        {
            MensagemDto mensagem = base.MemberwiseClone() as MensagemDto;
            mensagem.Propriedades = Propriedades.Clone() as Hashtable;
            return mensagem;
        }
        #endregion

        #region Construtores
        /// <summary>
        /// Construtor Simples
        /// </summary>
        public MensagemDto()
        {
            Propriedades = new Hashtable();
        }
        #endregion
    }
}
