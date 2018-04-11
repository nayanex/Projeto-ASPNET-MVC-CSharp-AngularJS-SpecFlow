using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;

using WexProject.BLL.Models.Planejamento;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.Library.Libs.Email;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe para cadastro de EmpresaInstituicao .
    /// </summary>
    [DefaultClassOptions]
    [RuleIsReferenced("RuleIsReferenced_EmpresaInstituicaoProjeto", DefaultContexts.Delete, typeof(Projeto), "EmpresaInstituicao", InvertResult = true,
    CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
    MessageTemplateMustBeReferenced = "Empresa / Instituição não pode ser excluído pois está associado a um Projeto.")]
    [Custom("Caption", "Empresa/Instituição")]
    [OptimisticLocking( false )]
    public class EmpresaInstituicao  : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxNome
        /// </summary>
        private String txNome;
        /// <summary>
        /// Atributo de TxSigla
        /// </summary>
        private String txSigla;
        /// <summary>
        /// Atributo de TxEmail
        /// </summary>
        private String txEmail;
        /// <summary>
        /// Atributo de TxFoneFax
        /// </summary>
        private String txFoneFax;
        /// <summary>
        /// Pais da Empresa/Instituição
        /// </summary>
        private Pais pais;
        #endregion

        #region Properties

        /// <summary>
        /// Declaração da variavel TxNome que irá armazenar os nomes dos EmpresaInstituicao .
        /// </summary>
        [Custom("Caption", "Empresa/Instuição")]
        [RuleUniqueValue("EmpresaInstituicao_TxNome_Unique", DefaultContexts.Save, Name = "Empresa/Instuição")]
        [RuleRequiredField("EmpresaInstituicao_TxNome_Required",DefaultContexts.Save,Name = "Empresa/Instuição")]
        [Size(-1)]
        public String TxNome
        {
            get
            {
                return txNome;
            }
            set
            {
                txNome = StrUtil.RetirarEspacoVazio(txNome);

                if (value != null)
                {
                SetPropertyValue<String>("TxNome", ref txNome, value.Trim());
            }
                else
                {
                    SetPropertyValue<String>("TxNome", ref txNome, null);
        }
            }
        }

        /// <summary>
        /// Declaração da variavel TxSigla que irá armazenar as siglas das EmpresaInstituicao .
        /// </summary>
        [Custom("Caption", "Sigla")]
        [RuleUniqueValue("EmpresaInstituicao_TxSigla_Unique", DefaultContexts.Save, Name = "Sigla")]
        [RuleRequiredField("EmpresaInstituicao_TxSigla_Required",DefaultContexts.Save,Name="Sigla")]
        [Size(30)]
        public String TxSigla
        {
            get
            {
                return txSigla;
            }
            set
            {
                if (value != null)
                {
                    txSigla = StrUtil.RetirarEspacoVazio(txSigla);
                    SetPropertyValue<String>("TxSigla", ref txSigla, value.Trim());
                }
            }
        }

        /// <summary>
        /// Declaração da variavel TxEmail que irá armazenar os emails das EmpresaInstituicao .
        /// </summary>
        [Custom("Caption", "E-mail")]
        [RuleUniqueValue("EmpresaInstituicao_TxEmail_Unique",DefaultContexts.Save,Name="Email")]
        [Size(-1)]
        public String TxEmail
        {
            get
            {
                return txEmail;
            }
            set
            {
                try
                {
                    txEmail = StrUtil.RetirarEspacoVazio(txEmail);
                    SetPropertyValue<String>("TxEmail", ref txEmail, value.Trim());
                }
                catch
                {
                    txEmail = StrUtil.RetirarEspacoVazio(txEmail);
                    SetPropertyValue<String>("TxEmail", ref txEmail, value);
                }
            }
        }

        /// <summary>
        /// Declaração da variavel TxFoneFax que irá armazenar os telefones/faxes das EmpresaInstituicao .
        /// </summary>
        [Size(30)]
        [Custom("Caption", "Fone/Fax")]
        [Appearance("EmpresaIntituicao_Disable_TxFoneFax", Criteria = "Pais is null", Enabled = false, TargetItems = "TxFoneFax")]
        public String TxFoneFax
        {
            get
            {
                return txFoneFax;
            }
            set
            {
                if (value != null && value != txFoneFax)
                    value = StrUtil.RetirarEspacoVazio(value.Trim());
                SetPropertyValue<String>("TxFoneFax", ref txFoneFax, value);
            }
        }

        /// <summary>
        /// Pais da Empresa/Instituição
        /// </summary>
        [Custom("Caption", "País")]
        [RuleRequiredField("EmpresaInstituicao_Pais_Required", DefaultContexts.Save, Name="País")]
        [DataSourceProperty("_PaisesAtivos"), ImmediatePostData]
        public Pais Pais
        {
            get
            {
                return pais;
            }
            set
            {
                SetPropertyValue<Pais>("Pais", ref pais, value);

                if (value == null)
                {
                    TxFoneFax = string.Empty;
                }
            }
        }

        #endregion

        #region NonPersistentProperties

        /// <summary>
        /// Data Source da propiedade de País
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public XPCollection<Pais> _PaisesAtivos
        {
            get 
            {
                return Pais.GetPaisesAtivos(Session);
            }
        }

        /// <summary>
        /// Mascara do país selecionado
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public string _MascaraPais
        {
            get 
            {
                return Pais == null ? string.Empty : Pais.TxMascara;
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Validação do email
        /// </summary>
        [RuleFromBoolProperty("EmpresaInstituicao_ValidarEmail", DefaultContexts.Save,
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
        /// Ao transformar o objeto para string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return TxNome;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public EmpresaInstituicao(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// AfetrConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Pais = Pais.GetPaisPadrao(Session);
        }

        #endregion
    }

}