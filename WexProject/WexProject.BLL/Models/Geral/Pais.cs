using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Países com a Identificação da Máscara de Telefone
    /// </summary>
    [DefaultClassOptions, DeferredDeletion(false)]
    [Custom("Caption", "País")]
    [RuleIsReferenced("RuleIsReferenced_EmpresaInstituicaoPais", DefaultContexts.Delete, typeof(EmpresaInstituicao), "Pais", InvertResult = true,
        CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = "Não é possível excluir o país, pois o mesmo está sendo utilizado por uma Empresa/Instituição")]
    [OptimisticLocking( false )]
    public class Pais: BaseObject
    {
        #region Atributos

        /// <summary>
        /// Máscara de Telefone do País
        /// </summary>
        private string txMascara;

        /// <summary>
        /// Situação do País
        /// </summary>
        private CsSituacaoDomain csSituacao;

        /// <summary>
        /// Indica se o país é padrão
        /// </summary>
        private bool isPadrao;

        /// <summary>
        /// Teste da Máscara de Telefone do País
        /// </summary>
        private string _txTesteMascara;

        /// <summary>
        /// Country associado
        /// </summary>
        private Country country;

        #endregion
        
        #region Propriedades

        /// <summary>
        /// Máscara de Telefone do País
        /// </summary>
        [Custom("Caption", "Máscara de Telefone"), ImmediatePostData, Size(30)]
        [RuleRequiredField("Pais_TxMascara_Required",DefaultContexts.Save,Name = "Máscara de Telefone")]
        public string TxMascara
        {
            get
            {
                return txMascara;
            }
            set
            {
                if (txMascara == value)
                    return;

                _TxTesteMascara = string.Empty;

                if (value != null)
                {
                    SetPropertyValue<string>("TxMascara", ref txMascara, value.Trim());
                }
                else
                {
                    SetPropertyValue<string>("TxMascara", ref txMascara, null);
                }
            }
        }

        /// <summary>
        /// Situação do País
        /// </summary>
        [Custom("Caption", "Situação")]
        public CsSituacaoDomain CsSituacao
        {
            get
            {
                return csSituacao;
            }
            set
            {
                if (csSituacao == value)
                    return;

                SetPropertyValue<CsSituacaoDomain>("CsSituacao", ref csSituacao, value);
            }
        }

        /// <summary>
        /// Indica se o país é padrão
        /// </summary>
        [Custom("Caption","País Padrão?"), VisibleInListView(false)]
        public bool IsPadrao
        {
            get
            {
                return isPadrao;
            }
            set
            {
                SetPropertyValue<bool>("IsPadrao", ref isPadrao, value);
            }
        }

        /// <summary>
        /// Country associado
        /// </summary>
        [VisibleInDetailView(false), VisibleInListView(false)]
        public Country Country
        {
            get
            {
                return country;
            }
            set
            {
                SetPropertyValue<Country>("Country", ref country, value);
            }
        }

        #endregion

        #region Propriedades Não Persistentes

        /// <summary>
        /// Indica se o país é padrão (Texto)
        /// </summary>
        [Custom("Caption", "País Padrão?"), NonPersistent, VisibleInDetailView(false)]
        public string _TxIsPadrao
        {
            get
            {
                return IsPadrao ? "Sim" : "Não";
            }
        }

        /// <summary>
        /// Teste da Máscara criada
        /// </summary>
        [Custom("Caption", "Teste da Máscara"), NonPersistent, VisibleInListView(false)]
        public string _TxTesteMascara
        {
            get
            {
                return _txTesteMascara;
            }
            set
            {
                if (_txTesteMascara == value)
                    return;

                SetPropertyValue<string>("_TxTesteMascara", ref _txTesteMascara, value);
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Ao salvar o País, salvar também o Country
        /// </summary>
        protected override void OnSaving()
        {
            if (Country != null)
            {
                Country.Save();
            }

            base.OnSaving();
        }

        /// <summary>
        /// Regra para verificar se existe um país padrão e caso exista mostrar 
        /// a janela de mudança de país padrão
        /// </summary>
        /// <returns>Se é para exibir ou não</returns>
        public bool RnIsExibirJanelaMudancaPaisPadrao() 
        {
            if (!IsPadrao || CsSituacao != CsSituacaoDomain.Ativo)
            {
                return false;
            }

            Pais paisPadrao = GetPaisPadrao(Session);

            if (paisPadrao != null && paisPadrao != this) 
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Modifica o País Padrão para o atual
        /// </summary>
        public void RnMudarPaisPadrao() 
        {
            Pais pais = GetPaisPadrao(Session);

            pais.IsPadrao = false;
            pais.Save();
        }

        [Browsable(false)]
        [RuleFromBoolProperty("Pais_RnHasLetrasmascaraTelefone_Custom",DefaultContexts.Save,CustomMessageTemplate="Não pode haver letras na máscara de telefone.")]
        public bool RnHasLetrasmascaraTelefone 
        {
            get 
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^\\d\\(\\)\\s\\+\\-]");

                if (regex.IsMatch(TxMascara)) 
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Ao transformar para string
        /// </summary>
        /// <returns>Nome do País</returns>
        public override string ToString()
        {
            if (Country != null)
            {
                return Country.Name;
            }

            return string.Empty;
        }

        #endregion

        #region DBQueries

        /// <summary>
        /// Traz todos os países ativos
        /// </summary>
        /// <param name="session">sessão atual</param>
        /// <returns>XPCollection de Pais</returns>
        public static XPCollection<Pais> GetPaisesAtivos(Session session) 
        {
            return new XPCollection<Pais>(session, CriteriaOperator.Parse("CsSituacao = ?", CsSituacaoDomain.Ativo));
        }

        /// <summary>
        /// Traz o pais padrão
        /// </summary>
        /// <param name="session">Sessão Atual</param>
        /// <returns>País padrão</returns>
        public static Pais GetPaisPadrao(Session session) 
        {
            return session.FindObject<Pais>(CriteriaOperator.Parse("IsPadrao = true and CsSituacao = ?",CsSituacaoDomain.Ativo));
        }

        #endregion

        #region NonPersistent
        
        #endregion

        #region Construtores

        /// <summary>
        /// Construtor da classe País
        /// </summary>
        /// <param name="session">Sessão atual</param>
        public Pais(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Método chamado após criar um novo objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Country = new Country(Session);

            // Padrões
            TxMascara = "55";
            CsSituacao = CsSituacaoDomain.Ativo;
        }

        #endregion
    }
}