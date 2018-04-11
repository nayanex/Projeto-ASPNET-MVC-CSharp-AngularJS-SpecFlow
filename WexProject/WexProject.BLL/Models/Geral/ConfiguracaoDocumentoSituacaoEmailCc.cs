using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe referente aos emails de envio com cópia
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Novos negócios > Solicitação de Documentos > Email > Cco")]
    [OptimisticLocking( false )]
    public class ConfiguracaoDocumentoSituacaoEmailCc : ConfiguracaoDocumentoSituacaoEmail
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
        [Association("ConfiguracaoDocumentoSituacao_ConfiguracaoDocumentoSituacaoEmailCc",
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
        [Association("SolicitacaoOrcamento_ConfiguracaoDocumentoSituacaoEmailCc", typeof(SolicitacaoOrcamento))]
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
        /// <returns>Objeto de ConfiguracaoDocumentoSituacaoEmailCc</returns>
        /*public ConfiguracaoDocumentoSituacaoEmailCc Clone(SolicitacaoOrcamento solicitacao)
        {
            ConfiguracaoDocumentoSituacaoEmailCc email =
            new ConfiguracaoDocumentoSituacaoEmailCc(Session);

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
        public ConfiguracaoDocumentoSituacaoEmailCc(Session session)
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
