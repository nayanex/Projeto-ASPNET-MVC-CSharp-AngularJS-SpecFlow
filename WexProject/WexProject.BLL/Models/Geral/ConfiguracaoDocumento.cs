using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.Data.Filtering;
using WexProject.Library.Libs.Enumerator;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe referente às configurações de documento
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Novos Negocios > Basicos > Configuração de Documento")]
    [OptimisticLocking( false )]
    public class ConfiguracaoDocumento : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Atributo de CsTipoDocumento
        /// </summary>
        private CsTipoDocumento csTipoDocumento;

        #endregion

        #region Properties

        /// <summary>
        /// Tipo do documento
        /// </summary>
        [Custom("Caption", "Documento")]
        [RuleUniqueValue("ConfiguracaoDocumento_CsTipoDocumento_Unique", DefaultContexts.Save,
        "Já existe uma configuração com esse documento!")]
        [RuleRequiredField("ConfiguracaoDocumento_CsTipoDocumento_Required", DefaultContexts.Save,
        Name = "Documento")]
        public CsTipoDocumento CsTipoDocumento
        {
            get
            {
                return csTipoDocumento;
            }
            set
            {
                SetPropertyValue<CsTipoDocumento>("CsTipoDocumento", ref csTipoDocumento, value);
            }
        }

        /// <summary>
        /// Situações ligadas à configuração de orçamento
        /// </summary>
        [Custom("Caption", "Situações")]
        [Association("ConfiguracaoDocumento_ConfiguracaoDocumentoSituacao",
        typeof(ConfiguracaoDocumentoSituacao)), Aggregated]
        public XPCollection<ConfiguracaoDocumentoSituacao> Situacoes
        {
            get
            {
                return GetCollection<ConfiguracaoDocumentoSituacao>("Situacoes");
            }
        }

        #endregion

        #region NonPersistent Properties

        #endregion

        #region BusinessRules

        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        public static ConfiguracaoDocumento GetConfiguracaoPorTipo(Session session, CsTipoDocumento tipo)
        {
            XPCollection<ConfiguracaoDocumento> configs =
                new XPCollection<ConfiguracaoDocumento>(session, CriteriaOperator.Parse("CsTipoDocumento = ?", tipo));

            if (configs.Count > 0)
            {
                return configs[0];
            }

            return null;
        }

        #endregion

        #region Utils

        /// <summary>
        /// Chamado ao transformar o objeto em string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return EnumUtil.DescricaoEnum(CsTipoDocumento);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public ConfiguracaoDocumento(Session session)
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
