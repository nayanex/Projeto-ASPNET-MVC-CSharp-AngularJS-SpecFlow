using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe referente aos emails de envio com cópia oculta
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Novos negócios > Solicitação de Documentos > Email > Cco")]
    [OptimisticLocking( false )]
    public class ConfiguracaoDocumentoSituacaoEmailCco : ConfiguracaoDocumentoSituacaoEmail
    {
        #region Attributes

        /// <summary>
        /// Atributo de ConfiguracaoDocumentoSituacao
        /// </summary>
        private ConfiguracaoDocumentoSituacao configuracaoDocumentoSituacao;

        /// <summary>
        /// Atributo de SolicitacaoOrcamento
        /// </summary>
        //private SolicitacaoOrcamento solicitacaoOrcamento;

        #endregion

        #region Properties

        /// <summary>
        /// Situação de configuração de orçamento do email atual
        /// </summary>
        [Browsable(false)]
        [Association("ConfiguracaoDocumentoSituacao_ConfiguracaoDocumentoSituacaoEmailCco", 
            typeof(ConfiguracaoDocumentoSituacao))]
        public ConfiguracaoDocumentoSituacao ConfiguracaoDocumentoSituacao
        {
            get
            {
                return configuracaoDocumentoSituacao;
            }
            set
            {
                SetPropertyValue<ConfiguracaoDocumentoSituacao>("ConfiguracaoDocumentoSituacao",
                ref configuracaoDocumentoSituacao, value);
            }
        }

        /// <summary>
        /// Solicitações de Orçamento do email atual
        /// </summary>
        /*[Browsable(false)]
        [Association("SolicitacaoOrcamento_ConfiguracaoDocumentoSituacaoEmailCco", typeof(SolicitacaoOrcamento))]
        public SolicitacaoOrcamento SolicitacaoOrcamento
        {
            get
            {
                return solicitacaoOrcamento;
            }
            set
            {
                SetPropertyValue<SolicitacaoOrcamento>("SolicitacaoOrcamento", ref solicitacaoOrcamento, value);
            }
        }*/

        #endregion

        #region NonPersistent Properties

        #endregion

        #region BusinessRules
        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        #endregion

        #region Utils

        /// <summary>
        /// Clone do objeto
        /// </summary>
        /// <param name="solicitacao">Objeto de SolicitacaoOrcamento</param>
        /// <returns>Objeto de ConfiguracaoDocumentoSituacaoEmailCco</returns>
        /*public ConfiguracaoDocumentoSituacaoEmailCco Clone(SolicitacaoOrcamento solicitacao)
        {
            ConfiguracaoDocumentoSituacaoEmailCco email =
            new ConfiguracaoDocumentoSituacaoEmailCco(Session);

            email.SolicitacaoOrcamento = solicitacao;
            email.TxEmail = TxEmail;

            return email;
        }*/

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public ConfiguracaoDocumentoSituacaoEmailCco(Session session)
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
