using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.Drawing;
using DevExpress.Data.Filtering;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe referente às situações das configurações de documento
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Novos negócios > Configuração > Documento > Situações")]
    [OptimisticLocking( false )]
    public class ConfiguracaoDocumentoSituacao : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Atributo de ConfiguracaoDocumento
        /// </summary>
        private ConfiguracaoDocumento configuracaoDocumento;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private string txDescricao;

        /// <summary>
        /// Atributo de TxNomeCor
        /// </summary>
        private string txNomeCor;

        /// <summary>
        /// Atributo de TypeColor
        /// </summary>
        private CsColorDomain typeColor;

        /// <summary>
        /// Indica se é de situação inicial.
        /// </summary>
        private bool isSituacaoInicial;

        /// <summary>
        /// Campo texto com todos os emails a serem notificados com
        /// cópia da alteração separados por ponto e vírgula
        /// </summary>
        private string txCc;

        /// <summary>
        /// Campo texto com todos os emails a serem notificados com
        /// cópia oculta da alteração separados por ponto e vírgula
        /// </summary>
        private string txCco;

        #endregion

        #region Properties

        /// <summary>
        /// Objeto de ConfiguracaoDocumento
        /// </summary>
        [Browsable(false)]
        [Association("ConfiguracaoDocumento_ConfiguracaoDocumentoSituacao", typeof(ConfiguracaoDocumento))]
        public ConfiguracaoDocumento ConfiguracaoDocumento
        {
            get
            {
                return configuracaoDocumento;
            }
            set
            {
                SetPropertyValue<ConfiguracaoDocumento>("ConfiguracaoDocumento", ref configuracaoDocumento, value);
            }
        }

        /// <summary>
        /// Descrição da situação
        /// </summary>
        [Size(100)]
        [Custom("Caption", "Descrição")]
        [RuleUniqueValue("ConfiguracaoDocumentoSituacao_TxDescricao_Unique", DefaultContexts.Save,
            "A Descrição já está definida para outra Situação.")]
        [RuleRequiredField("ConfiguracaoDocumentoSituacao_TxDescricao_Required", DefaultContexts.Save,
        Name = "Descrição")]
        public string TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                txDescricao = StrUtil.RetirarEspacoVazio(txDescricao);
                SetPropertyValue<string>("TxDescricao", ref txDescricao, value);
            }
        }

        /// <summary>
        /// Nome da cor escolhida
        /// </summary>
        [Browsable(false)]
        [RuleUniqueValue("ConfiguracaoDocumentoSituacao_TxNomeCor_Unique", DefaultContexts.Save,
            "A Cor já está definida para outra Situação.")]
        [RuleRequiredField("ConfiguracaoDocumentoSituacao_TxNomeCor_Required", DefaultContexts.Save,
        Name = "Nome da cor")]
        public string TxNomeCor
        {
            get
            {
                return txNomeCor;
            }
            set
            {
                // txNomeCor = StrUtil.RetirarEspacoVazio(txNomeCor);
                SetPropertyValue<string>("TxNomeCor", ref txNomeCor, value);
            }
        }

        /// <summary>
        /// Variável que armazena o type da cor escolhida
        /// </summary>
        [Browsable(false)]
        [RuleRequiredField("ConfiguracaoDocumentoSituacao_TypeColor_Required", DefaultContexts.Save,
        Name = "Tipo da cor")]
        public CsColorDomain TypeColor
        {
            get
            {
                return typeColor;
            }
            set
            {
                SetPropertyValue<CsColorDomain>("TypeColor", ref typeColor, value);
            }
        }

        /// <summary>
        /// Indica se é de situação inicial.
        /// </summary>
        [Custom("Caption", "Situação Inicial?")]
        [RuleRequiredField("ConfiguracaoDocumentoSituacao_SituacaoInicial_Required", DefaultContexts.Save,
            Name = "Situação Inicial?")]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsSituacaoInicial
        {
            get
            {
                return isSituacaoInicial;
            }
            set
            {
                SetPropertyValue<bool>("IsSituacaoInicial", ref isSituacaoInicial, value);
            }
        }

        /// <summary>
        /// Lista de emails para envio com cópia
        /// </summary>
        [Custom("Caption", "Cc")]
        [Association("ConfiguracaoDocumentoSituacao_ConfiguracaoDocumentoSituacaoEmailCc",
        typeof(ConfiguracaoDocumentoSituacaoEmailCc)), Aggregated]
        public XPCollection<ConfiguracaoDocumentoSituacaoEmailCc> ComCopia
        {
            get
            {
                return GetCollection<ConfiguracaoDocumentoSituacaoEmailCc>("ComCopia");
            }
        }

        /// <summary>
        /// Lista de emails para envio com cópia oculta
        /// </summary>
        [Custom("Caption", "Cco")]
        [Association("ConfiguracaoDocumentoSituacao_ConfiguracaoDocumentoSituacaoEmailCco",
        typeof(ConfiguracaoDocumentoSituacaoEmailCco)), Aggregated]
        public XPCollection<ConfiguracaoDocumentoSituacaoEmailCco> ComCopiaOculta
        {
            get
            {
                return GetCollection<ConfiguracaoDocumentoSituacaoEmailCco>("ComCopiaOculta");
            }
        }

        /// <summary>
        /// Campo texto com todos os emails a serem notificados com
        /// cópia da alteração separados por ponto e vírgula
        /// </summary>
        //[NonPersistent]
        [Custom("Caption", "Cc")]
        [VisibleInDetailView(false)]
        [AppearanceAttribute("ConfiguracaoDocumentoSituacao_TxCc_Appearance",
            Enabled = false,
            TargetItems = "TxCc")]
        public string TxCc
        {
            get
            {
                return txCc;
            }
            set
            {
                SetPropertyValue<string>("TxCc", ref txCc, value);
            }
        }

        /// <summary>
        /// Campo texto com todos os emails a serem notificados com
        /// cópia oculta da alteração separados por ponto e vírgula
        /// </summary>
        //[NonPersistent]
        [Custom("Caption", "Cco")]
        [VisibleInDetailView(false)]
        [AppearanceAttribute("ConfiguracaoDocumentoSituacao_TxCco_Appearance",
            Enabled = false,
            TargetItems = "TxCco")]
        public string TxCco
        {
            get
            {
                return txCco;
            }
            set
            {
                SetPropertyValue<string>("TxCco", ref txCco, value);
            }
        }

        #endregion

        #region NonPersistent Properties

        /// <summary>
        /// Cor referente à situação
        /// </summary>
        [NonPersistent]
        [Custom("Caption", "Cor")]
        public Color _ClSituacao
        {
            get
            {
                switch (TypeColor)
                {
                    case CsColorDomain.web:
                        return Color.FromName(TxNomeCor);
                    case CsColorDomain.System:
                        return Color.FromArgb(int.Parse(TxNomeCor));
                    default:
                        return Color.FromArgb(int.Parse(TxNomeCor));
                }
            }
            set
            {
                if (value.IsSystemColor)
                {
                    TypeColor = CsColorDomain.System;
                    TxNomeCor = value.ToArgb().ToString();
                }
                else
                    if (value.IsKnownColor && value.IsNamedColor)
                    {
                        TypeColor = CsColorDomain.web;
                        TxNomeCor = value.Name;
                    }
                    else
                    {
                        TypeColor = CsColorDomain.Custom;
                        TxNomeCor = value.ToArgb().ToString();
                    }
            }
        }

        /// <summary>
        /// Valor texto para o campo IsSituacaoInicial
        /// </summary>
        [NonPersistent]
        [Custom("Caption", "Situação Inicial?")]
        [VisibleInDetailView(false)]
        public string _IsSituacaoInicial
        {
            get
            {
                if (IsSituacaoInicial)
                {
                    return "Sim";
                }

                return "Não";
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Chamado quando tiver persistindo um objeto
        /// </summary>
        protected override void OnSaving()
        {
            AjustarEmailsCcTexto();
            AjustarEmailsCcoTexto();

            base.OnSaving();
        }

        /// <summary>
        /// Ajustar o campo texto de emails Cc
        /// </summary>
        private void AjustarEmailsCcTexto()
        {
            string emails = string.Empty;

            if (ComCopia.Count > 0)
            {
                emails = ComCopia[0].TxEmail;

                for (int pos = 1; pos < ComCopia.Count; pos++)
                    emails += String.Format("; {0}", ComCopia[pos].TxEmail);
            }

            TxCc = emails;
        }

        /// <summary>
        /// Ajustar o campo texto de emails Cco
        /// </summary>
        private void AjustarEmailsCcoTexto()
        {
            string emails = string.Empty;

            if (ComCopiaOculta.Count > 0)
            {
                emails = ComCopiaOculta[0].TxEmail;

                for (int pos = 1; pos < ComCopiaOculta.Count; pos++)
                    emails += String.Format("; {0}", ComCopiaOculta[pos].TxEmail);
            }

            TxCco = emails;
        }

        /// <summary>
        /// Realiza a troca de situação inicial quando um novo registro é definido como padrão
        /// </summary>
        public void RnTrocaSituacaoInicial()
        {
            ConfiguracaoDocumentoSituacao cdsOldPadrao = GetSituacaoInicial(ConfiguracaoDocumento, Oid);

            if (cdsOldPadrao != null)
            {
                cdsOldPadrao.IsSituacaoInicial = false;
                cdsOldPadrao.Save();
            }
        }

        /// <summary>
        /// Regra para verificar se existe uma situação inicial e caso exista mostrar 
        /// a janela de mudança de situação inicial
        /// </summary>
        /// <returns>Se é para exibir ou não</returns>
        public bool RnIsExibirJanelaMudancaoSituacaoPlanejamento()
        {
            if (!IsSituacaoInicial)
                return false;

            ConfiguracaoDocumentoSituacao configuracaoDocumentoSituacao = GetSituacaoInicial(ConfiguracaoDocumento, Oid);

            if (configuracaoDocumentoSituacao != null && configuracaoDocumentoSituacao != this)
                return true;

            return false;
        }

        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// Resgata o objeto de Configuração de Documento padrão
        /// </summary>
        /// <param name="session">Session</param>
        /// <returns>Objeto do ConfiguracaoDocumentoSituacao</returns>
        public static ConfiguracaoDocumentoSituacao GetSituacaoInicial(ConfiguracaoDocumento config, Guid oidSituacaoAtual = new Guid())
        {
            if (config == null)
            {
                return null;
            }

            ConfiguracaoDocumentoSituacao result = null;
            config.Situacoes.Filter = CriteriaOperator.Parse("IsSituacaoInicial = true AND Oid != ?", oidSituacaoAtual);
            
            if (config.Situacoes.Count > 0)
            {
                result = config.Situacoes[0];
            }

            config.Situacoes.Filter = null;

            return result;
        }

        #endregion

        #region Utils

        /// <summary>
        /// Chamado ao transformar o objeto em string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return TxDescricao;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public ConfiguracaoDocumentoSituacao(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }

        /// <summary>
        /// Método chamado depois de construir o objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // Cores padrões ao inserir um novo valor
            TxNomeCor = Color.Yellow.ToArgb().ToString();
            TypeColor = CsColorDomain.Custom;
        }

        #endregion
    }
}
