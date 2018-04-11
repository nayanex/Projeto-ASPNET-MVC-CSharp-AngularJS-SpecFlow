using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using WexProject.Library.Libs.Email;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe referente à email
    /// </summary>
    [Custom("Caption", "Novos Negocios > Básicos > Configuracao de documento > Situacões")]
    [DefaultClassOptions]
    [Custom("Caption", "Novos negócios > Configuração > Documento > Situação > Email")]
    [OptimisticLocking( false )]
    public class ConfiguracaoDocumentoSituacaoEmail : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Atributo de TxEmail
        /// </summary>
        private string txEmail;

        #endregion

        #region Properties

        /// <summary>
        /// Email
        /// </summary>
        [Custom("Caption", "Email")]
        [RuleRequiredField("ConfiguracaoDocumentoSituacaoEmail_TxEmail_Required", DefaultContexts.Save,
        Name = "Email")]
        public string TxEmail
        {
            get
            {
                return txEmail;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxEmail", ref txEmail, value.Trim());
            }
        }

        #endregion

        #region NonPersistent Properties

        #endregion

        #region BusinessRules

        /// <summary>
        /// Validação do email
        /// </summary>
        [RuleFromBoolProperty("ValidarEmail", DefaultContexts.Save,
        "Digite um Email para válido.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarEmail
        {
            get
            {
                return EmailUtil.ValidarEmail(TxEmail);
            }
        }

        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        #endregion

        #region Utils

        /// <summary>
        /// Chamado ao transformar o objeto em string
        /// </summary>
        /// <returns>Descrição do email</returns>
        public override string ToString()
        {
            return TxEmail;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public ConfiguracaoDocumentoSituacaoEmail(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }

        #endregion
    }
}
