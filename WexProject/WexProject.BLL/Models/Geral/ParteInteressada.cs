using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp;
using WexProject.BLL.Models.Rh;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe ParteInteressada
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [Custom("Caption", "Projetos > Planejamento > Comunicação > Parte Interessada")]
    [OptimisticLocking( false )]
    public class ParteInteressada : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de Cargo
        /// </summary>
        private Cargo cargo;

        /// <summary>
        /// Atributo de TxParteInteressada
        /// </summary>
        private String txParteInteressadaNome;

        /// <summary>
        /// Atributo de TxTelephnFixo
        /// </summary>
        private String txTelefoneFixo;

        /// <summary>
        /// Atributo de TxCelular
        /// </summary>
        private String txCelular;

        /// <summary>
        /// Atributo de TxEmail
        /// </summary>
        private String txEmail;

        /// <summary>
        /// Atributo de EmpresaInstituicao
        /// </summary>
        private EmpresaInstituicao empresaInstituicao;

        /// <summary>
        /// Atributo de Projeto
        /// </summary>
        private Projeto projeto;

        /// <summary>
        /// Atributo para dizer se é ou não um colaborador
        /// </summary>
        private CsSimNao isColaborador;

        /// <summary>
        /// Atributo que guarda o colaborador
        /// </summary>
        private Colaborador colaborador;


        #endregion

        #region Properties
        /// <summary>
        /// Import de Projeto
        /// </summary>
        [Association("ParteInteressada", typeof(Projeto)), Browsable(false)]
        public Projeto Projeto
        {
            get
            {
                return projeto;
            }
            set
            {
                SetPropertyValue<Projeto>("Projeto", ref projeto, value);
            }
        }
        /// <summary>
        /// Variável que guarda o Cargo da parte interessada
        /// </summary>
        [RuleRequiredField("ProjetoParteInteressada_Cargo_Required", DefaultContexts.Save, "Campo cargo obrigatório.")]
        [AppearanceAttribute("ParteInteressada_Cargo_Appearance",
        Criteria = "IsColaborador == 'SIM'",
        Enabled = false,
        TargetItems = "Cargo"
        )]
        public Cargo Cargo
        {
            get
            {
                if(IsColaborador == CsSimNao.Sim)
                {
                    if (Colaborador != null)
                    {
                        cargo = this.Colaborador.Cargo;
                    }
                }
                return cargo;
            }
            set
            {
                SetPropertyValue<Cargo>("Cargo", ref cargo, value);
            }
        }
        /// <summary>
        /// Variável que guarda parte interessada
        /// </summary>
        [RuleUniqueValue("ParteInteressada_ParteInteressadaNome_Unique", DefaultContexts.Save, "Já Existe Parte Interessada com esse nome!")]
        public String TxParteInteressadaNome
        {
            get
            {
                if(IsColaborador == CsSimNao.Sim)
                {
                    if (Colaborador != null)
                    {
                        return Colaborador.Usuario.FullName;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return txParteInteressadaNome;
                }
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxParteInteressadaNome", ref txParteInteressadaNome, value.Trim());
            }
        }
        /// <summary>
        /// Variável que guarda Telefone fixo
        /// </summary>
        [AppearanceAttribute("ParteInteressada_TxTelefoneFixo_Appearance",
        Criteria = "IsColaborador == 'SIM'",
        Enabled = false,
        TargetItems = "TxTelefoneFixo"
        )]
        public String TxTelefoneFixo
        {
            get
            {
                String txTelefone = null;
                if(IsColaborador == CsSimNao.Sim)
                {
                    if (Colaborador != null)
                    {

                        Party pn = Colaborador.Usuario;
                        foreach (PhoneNumber phn in pn.PhoneNumbers)
                            if (phn.PhoneType.Equals("Residencial") && phn.Number != null)
                            {
                                txTelefone = phn.Number;
                                break;
                            }

                        if (!IsSaving)
                        {
                            Party pnnn = Colaborador.Usuario;
                            while (pn.IsLoading)
                            {
                            }
                            foreach (PhoneNumber phn in pnnn.PhoneNumbers)
                                if (phn.PhoneType.Equals("Residencial") && phn.Number != null)
                                {
                                    txTelefone = phn.Number;
                                    break;
                                }
                        }

                    }
                }
                else
                    txTelefone = txTelefoneFixo;

                return txTelefone;
            }
            set
            {
                if (value != null)
                {
                    SetPropertyValue<String>("TxTelefoneFixo", ref txTelefoneFixo, value.Trim());
                }
            }
        }
        /// <summary>
        /// Variável que guarda Telefone celular
        /// </summary>
        [AppearanceAttribute("ParteInteressada_TxCelular_Appearance",
        Criteria = "IsColaborador == 'SIM'",
        Enabled = false,
        TargetItems = "TxCelular"
        )]
        public String TxCelular
        {
            get
            {
                String celular = null;
                if(IsColaborador == CsSimNao.Sim)
                {
                    if (Colaborador != null)
                    {
                        if (!IsSaving)
                        {
                            Party pn = Colaborador.Usuario;
                            foreach (PhoneNumber phn in pn.PhoneNumbers)
                                if (phn.PhoneType.Equals("Celular") && phn.Number != null)
                                {
                                    celular = phn.Number;
                                    break;
                                }
                        }


                    }
                }
                else
                    celular = txCelular;
                return celular;
            }
            set
            {
                if (value != null)
                {
                    SetPropertyValue<String>("TxCelular", ref txCelular, value.Trim());
                }
            }
        }
        /// <summary>
        /// Variável que guarda email
        /// </summary>
        [AppearanceAttribute("ParteInteressada_TxEmail_Appearance",
        Criteria = "IsColaborador == 'SIM'",
        Enabled = false,
        TargetItems = "TxEmail"
        )]
        public String TxEmail
        {
            get
            {
                String email = "";
                if(IsColaborador == CsSimNao.Sim)
                {
                    if (Colaborador != null)
                    {
                        email = ((Person)Colaborador.Usuario).Email;
                    }
                }
                else
                {
                    email = txEmail;
                }
                return email;
            }
            set
            {
                if (value != null)
                {
                    SetPropertyValue<String>("TxEmail", ref txEmail, value.Trim());
                }
            }
        }
        /// <summary>
        /// Variável que guarda a Empresa/Instituicao da parte interessada
        /// </summary>
        public EmpresaInstituicao EmpresaInstituicao
        {
            get
            {
                return empresaInstituicao;
            }
            set
            {
                SetPropertyValue<EmpresaInstituicao>("EmpresaInstituicao", ref empresaInstituicao, value);
            }
        }
        /// <summary>
        /// Associação com a classe ProjetoParteInteressada
        /// </summary>
        [Association("ParteInteressadaProjetoParteInteressada", typeof(ProjetoParteInteressada)), Aggregated]
        public XPCollection<ProjetoParteInteressada> ProjetoParteInteressadas
        {
            get
            {
                return GetCollection<ProjetoParteInteressada>("ProjetoParteInteressadas");
            }
        }
        /// <summary>
        /// Variavel para guardar se é ou não colaborador
        /// </summary>
        [ImmediatePostData]
        [Custom("Caption", "É colaborador")]
        public CsSimNao IsColaborador
        {
            get
            {
                return isColaborador;
            }
            set
            {
                SetPropertyValue<CsSimNao>( "IsColaborador", ref isColaborador, value );
            }
        }
        /// <summary>
        /// Propiedade que guarda o colaborador da Parte Interessada
        /// </summary>
        [ImmediatePostData]
        public Colaborador Colaborador
        {
            get
            {
                return colaborador;
            }
            set
            {
                SetPropertyValue<Colaborador>("Colaborador", ref colaborador, value);
            }
        }
        #endregion

        #region NonPersistentProperties

        #endregion

        #region BusinessRules
        /// <summary>
        /// Regra de negócio que valida um e-mail
        /// </summary>
        [RuleFromBoolProperty("NaoSalvarEmailFalso", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Digite um e-mail válido.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarEmail
        {
            get
            {
                if (TxEmail != null)
                    return Regex.IsMatch(TxEmail, "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$");
                else
                    return false;
            }
        }

        /// <summary>
        /// Regra de negócio que se algum colaborador foi escolhido
        /// </summary>
        [RuleFromBoolProperty("NaoEsquecerDeEsquecerColaborador", DefaultContexts.Save, CustomMessageTemplate = "Escolha um colaborador.")]
        [NonPersistent, Browsable(false)]
        public bool RnEscolherColaborador
        {
            get
            {
                bool validar = true;
                if (IsColaborador.Equals(CsSimNao.Sim))
                {
                    if (Colaborador == null)
                    {
                        validar = false;
                    }
                }
                return validar;
            }
        }
        /// <summary>
        /// Regra de negócio que verifica se o nome da parte interessada é diferente de vazio
        /// </summary>
        [RuleFromBoolProperty("NaoNomeParteInteressadaNulo", DefaultContexts.Save, CustomMessageTemplate = "Digite o nome da parte interessada.")]
        [NonPersistent, Browsable(false)]
        public bool RnSemNomeParteInteressada
        {
            get
            {
                bool validar = true;
                if(IsColaborador.Equals( CsSimNao.Não ))
                {
                    if (this.TxParteInteressadaNome == null)
                    {
                        validar = false;
                    }
                }
                return validar;
            }
        }
        /// <summary>
        /// Regra de negócio que verifica se há uma instituação selecionada
        /// </summary>
        [RuleFromBoolProperty("VerificarInstituacaoSelecionada", DefaultContexts.Save, CustomMessageTemplate = "Escolha uma empresa/instituição.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarInstituicaoSelecionada
        {
            get
            {
                bool validar = true;
                if(IsColaborador.Equals( CsSimNao.Não ))
                {
                    if (this.EmpresaInstituicao == null)
                    {
                        validar = false;
                    }
                }
                return validar;
            }
        }
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        #endregion

        #region Utils
        /// <summary>
        /// Overrride de TxParteInteressada
        /// </summary>
        /// <returns>returns</returns>
        public override string ToString()
        {
            if(IsColaborador.Equals( CsSimNao.Sim ))
            {
                string retorno = "";
                if (Colaborador != null)
                {
                    retorno = Colaborador.Usuario.FullName;
                }
                return retorno;
            }
            else
            {
                return TxParteInteressadaNome;
            }
        }

        /// <summary>
        /// Metodo para pegar os dados do colaborador e 
        /// seta-los nos edits de celular email telefone
        /// </summary>
        /// <param name="colaborador">colaborador</param>
        private void GetDadosColaborador(Colaborador colaborador)
        {
            if(colaborador != null && !colaborador.Oid.Equals( new Guid() ) && IsColaborador.Equals( CsSimNao.Sim ))
            {
                this.Cargo = colaborador.Cargo;
                Person p = colaborador.Usuario;
                this.TxEmail = p.Email;
                Party pn = colaborador.Usuario;

                while (pn.IsLoading)
                {
                }
                foreach (PhoneNumber phn in pn.PhoneNumbers)
                {
                    if (phn.PhoneType.Equals("Celular") && phn.Number != null)
                    {
                        this.TxCelular = phn.Number;
                    }
                    else
                    {
                        this.TxTelefoneFixo = phn.Number;
                    }
                }
            }
        }
        #endregion

        #region UserInterface

        /// <summary>
        /// Metodo que verifica se é ou nao um colaborador
        /// caso não seja esconde o looukup de empresa/instituição
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>EditorState.Hidden</returns>
        [EditorStateRule("HiddenParteInteressadaEmpresaInstituicao", "EmpresaInstituicao", ViewType.DetailView)]
        public EditorState HiddenEmpresaInstituicao(out bool active)
        {
            active = IsColaborador.Equals( CsSimNao.Sim );
            return EditorState.Hidden;
        }

        /// <summary>
        /// Metodo que verifica se é ou nao um colaborador
        /// caso não seja esconde o looukup de colaborador
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>return EditorState.Hidden</returns>
        [EditorStateRule("HiddenParteInteressadaColaborador", "Colaborador", ViewType.DetailView)]
        public EditorState HiddenColaborador(out bool active)
        {
            active = IsColaborador.Equals( CsSimNao.Não );
            return EditorState.Hidden;
        }
        /// <summary>
        /// Metodo que verifica se é ou nao um colaborador
        /// caso não seja esconde o txNome
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>EditorState.Hidden</returns>
        [EditorStateRule("HiddenTxParteInteressadaNome", "TxParteInteressadaNome", ViewType.DetailView)]
        public EditorState HiddenTxParteInteressadaNome(out bool active)
        {
            active = IsColaborador.Equals( CsSimNao.Sim );
            return EditorState.Hidden;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public ParteInteressada(Session session)
            : base(session)
        {

        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }



        #endregion

    }
}
