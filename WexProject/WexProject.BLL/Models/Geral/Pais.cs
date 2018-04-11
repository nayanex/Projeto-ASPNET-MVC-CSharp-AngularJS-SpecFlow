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
    /// Pa�ses com a Identifica��o da M�scara de Telefone
    /// </summary>
    [DefaultClassOptions, DeferredDeletion(false)]
    [Custom("Caption", "Pa�s")]
    [RuleIsReferenced("RuleIsReferenced_EmpresaInstituicaoPais", DefaultContexts.Delete, typeof(EmpresaInstituicao), "Pais", InvertResult = true,
        CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = "N�o � poss�vel excluir o pa�s, pois o mesmo est� sendo utilizado por uma Empresa/Institui��o")]
    [OptimisticLocking( false )]
    public class Pais: BaseObject
    {
        #region Atributos

        /// <summary>
        /// M�scara de Telefone do Pa�s
        /// </summary>
        private string txMascara;

        /// <summary>
        /// Situa��o do Pa�s
        /// </summary>
        private CsSituacaoDomain csSituacao;

        /// <summary>
        /// Indica se o pa�s � padr�o
        /// </summary>
        private bool isPadrao;

        /// <summary>
        /// Teste da M�scara de Telefone do Pa�s
        /// </summary>
        private string _txTesteMascara;

        /// <summary>
        /// Country associado
        /// </summary>
        private Country country;

        #endregion
        
        #region Propriedades

        /// <summary>
        /// M�scara de Telefone do Pa�s
        /// </summary>
        [Custom("Caption", "M�scara de Telefone"), ImmediatePostData, Size(30)]
        [RuleRequiredField("Pais_TxMascara_Required",DefaultContexts.Save,Name = "M�scara de Telefone")]
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
        /// Situa��o do Pa�s
        /// </summary>
        [Custom("Caption", "Situa��o")]
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
        /// Indica se o pa�s � padr�o
        /// </summary>
        [Custom("Caption","Pa�s Padr�o?"), VisibleInListView(false)]
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

        #region Propriedades N�o Persistentes

        /// <summary>
        /// Indica se o pa�s � padr�o (Texto)
        /// </summary>
        [Custom("Caption", "Pa�s Padr�o?"), NonPersistent, VisibleInDetailView(false)]
        public string _TxIsPadrao
        {
            get
            {
                return IsPadrao ? "Sim" : "N�o";
            }
        }

        /// <summary>
        /// Teste da M�scara criada
        /// </summary>
        [Custom("Caption", "Teste da M�scara"), NonPersistent, VisibleInListView(false)]
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
        /// Ao salvar o Pa�s, salvar tamb�m o Country
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
        /// Regra para verificar se existe um pa�s padr�o e caso exista mostrar 
        /// a janela de mudan�a de pa�s padr�o
        /// </summary>
        /// <returns>Se � para exibir ou n�o</returns>
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
        /// Modifica o Pa�s Padr�o para o atual
        /// </summary>
        public void RnMudarPaisPadrao() 
        {
            Pais pais = GetPaisPadrao(Session);

            pais.IsPadrao = false;
            pais.Save();
        }

        [Browsable(false)]
        [RuleFromBoolProperty("Pais_RnHasLetrasmascaraTelefone_Custom",DefaultContexts.Save,CustomMessageTemplate="N�o pode haver letras na m�scara de telefone.")]
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
        /// <returns>Nome do Pa�s</returns>
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
        /// Traz todos os pa�ses ativos
        /// </summary>
        /// <param name="session">sess�o atual</param>
        /// <returns>XPCollection de Pais</returns>
        public static XPCollection<Pais> GetPaisesAtivos(Session session) 
        {
            return new XPCollection<Pais>(session, CriteriaOperator.Parse("CsSituacao = ?", CsSituacaoDomain.Ativo));
        }

        /// <summary>
        /// Traz o pais padr�o
        /// </summary>
        /// <param name="session">Sess�o Atual</param>
        /// <returns>Pa�s padr�o</returns>
        public static Pais GetPaisPadrao(Session session) 
        {
            return session.FindObject<Pais>(CriteriaOperator.Parse("IsPadrao = true and CsSituacao = ?",CsSituacaoDomain.Ativo));
        }

        #endregion

        #region NonPersistent
        
        #endregion

        #region Construtores

        /// <summary>
        /// Construtor da classe Pa�s
        /// </summary>
        /// <param name="session">Sess�o atual</param>
        public Pais(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// M�todo chamado ap�s criar um novo objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Country = new Country(Session);

            // Padr�es
            TxMascara = "55";
            CsSituacao = CsSituacaoDomain.Ativo;
        }

        #endregion
    }
}