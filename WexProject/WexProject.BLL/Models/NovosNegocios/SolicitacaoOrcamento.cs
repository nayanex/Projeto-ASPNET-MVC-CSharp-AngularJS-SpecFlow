using System;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.Shared.Domains.NovosNegocios;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using WexProject.BLL.Models.Rh;
using System.Net.Mail;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.Xpo.DB;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.Email;
using WexProject.Library.Libs.Enumerator;
using WexProject.Library.Libs.Str;
using WexProject.BLL.DAOs;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.BLL.Models.NovosNegocios
{
    /// <summary>
    /// Classe para cadastro de Solicitação de Orçamento
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Novos Negocios > Solicitação de Orçamento")]
    [OptimisticLocking( false )]
    public class SolicitacaoOrcamento : BaseObject
    {
        #region Constants

        /// <summary>
        /// Constante que diz o número mínimo de caracteres do número da solicitação
        /// </summary>
        private const int TamanhoMinimoNumeroSolicitacao = 2;

        #endregion

        #region Attributes

        /// <summary>
        /// Atributo de TxCodigo
        /// </summary>
        private string txCodigo;

        /// <summary>
        /// Atributo de DtEmissao
        /// </summary>
        private DateTime dtEmissao;

        /// <summary>
        /// Atributo de DtPrazo
        /// </summary>
        private DateTime dtPrazo;

        /// <summary>
        /// Atributo de NbValor
        /// </summary>
        private float nbValor;

        /// <summary>
        /// Atributo de DtEntrega
        /// </summary>
        private DateTime dtEntrega;

        /// <summary>
        /// Atributo de TipoSolicitacao
        /// </summary>
        private TipoSolicitacao tipoSolicitacao;

        /// <summary>
        /// Atributo de CsPrioridade
        /// </summary>
        private CsPrioridade csPrioridade;

        /// <summary>
        /// Atributo de Solicitante
        /// </summary>
        private User solicitante;

        /// <summary>
        /// Atributo de Responsavel
        /// </summary>
        private Colaborador responsavel;

        /// <summary>
        /// Atributo de Situacao
        /// </summary>
        private ConfiguracaoDocumentoSituacao situacao;

        /// <summary>
        /// Atributo de DtConclusao
        /// </summary>
        private DateTime dtConclusao;

        /// <summary>
        /// Atributo de TxTitulo
        /// </summary>
        private string txTitulo;

        /// <summary>
        /// Atributo de TxRepositorio
        /// </summary>
        private string txRepositorio;

        /// <summary>
        /// Atributo de Cliente
        /// </summary>
        private EmpresaInstituicao cliente;

        /// <summary>
        /// Atributo de TxContatoCliente
        /// </summary>
        private string txContatoCliente;

        /// <summary>
        /// Atributo de TxEmailContatoCliente
        /// </summary>
        private string txEmailContatoCliente;

        /// <summary>
        /// Atributo de TxFone
        /// </summary>
        private string txFone;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private string txDescricao;

        /// <summary>
        /// Atributo de TxObservacao
        /// </summary>
        private string txObservacao;

        /// <summary>
        /// Atributo de TxObservacao
        /// </summary>
        private string textoEmail;

        /// <summary>
        /// Situação old
        /// </summary>
        private ConfiguracaoDocumentoSituacao oldSituacao;

        /// <summary>
        /// Variavel que guarda o valor do ultimo cliente  
        /// </summary>
        private EmpresaInstituicao oldCliente;
        
        /// <summary>
        /// Texto de emails para serem enviados com cópia
        /// </summary>
        private string txCc;

        /// <summary>
        /// Texto de emails para serem enviados com cópia oculta
        /// </summary>
        private string txCco;

        /// <summary>
        /// Guarda o último comentário da SEOT
        /// </summary>
        private string txUltimoComentario;

        #endregion

        #region Auxiliares

        /// <summary>
        /// Dicionário auxiliar que guarda os emails a serem enviados com cópia
        /// </summary>
        private Dictionary<string, string> _dicEmailsCc;

        /// <summary>
        /// Dicionário auxiliar que guarda os emails a serem enviados com cópia oculta
        /// </summary>
        private Dictionary<string, string> _dicEmailsCco;

        #endregion

        #region Properties

        #region Dados Gerais

        /// <summary>
        /// Solicitante do Orçamento
        /// </summary>
        [Browsable(false)]
        [RuleRequiredField("SolicitacaoOrcamento_Solicitante_Required", DefaultContexts.Save,
        Name = "Solicitante")]
        public User Solicitante
        {
            get
            {
                return solicitante;
            }
            set
            {
                SetPropertyValue<User>("Solicitante", ref solicitante, value);
            }
        }

        /// <summary>
        /// Responsável pelo Orçamento
        /// </summary>
        [Indexed]
        [Custom("Caption", "Responsável")]
        [RuleRequiredField("SolicitacaoOrcamento_Responsavel_Required", DefaultContexts.Save,
            Name = "Responsável")]
        public Colaborador Responsavel
        {
            get
            {
                return responsavel;
            }
            set
            {
                SetPropertyValue<Colaborador>("Responsavel", ref responsavel, value);
            }
        }

        /// <summary>
        /// Situação do Orçamento
        /// </summary>
        [Indexed]
        [Appearance("Rule_ConfiguracaoDocumentoSituacao_TxDescricao")]
        [Custom("Caption", "Situação")]
        [ImmediatePostData]
        [RuleRequiredField("SolicitacaoOrcamento_Situacao_Required", DefaultContexts.Save,
            Name = "Situação")]
        public ConfiguracaoDocumentoSituacao Situacao
        {
            get
            {
                return situacao;
            }
            set
            {
                SetPropertyValue<ConfiguracaoDocumentoSituacao>("Situacao", ref situacao, value);

                if (!IsLoading)
                {
                    InserirEmailsCcCcoTexto();
                }
            }
        }

        /// <summary>
        /// Tipo da solicitação
        /// </summary>
        [Custom("Caption", "Tipo")]
        public TipoSolicitacao TipoSolicitacao
        {
            get
            {
                return tipoSolicitacao;
            }
            set
            {
                SetPropertyValue<TipoSolicitacao>("TipoSolicitacao", ref tipoSolicitacao, value);
            }
        }

        /// <summary>
        /// Prioridade da solicitação
        /// </summary>
        [Custom("Caption", "Prioridade")]
        [RuleRequiredField("SolicitacaoOrcamento_CsPrioridade_Required", DefaultContexts.Save,
        Name = "Prioridade")]
        public CsPrioridade CsPrioridade
        {
            get
            {
                return csPrioridade;
            }
            set
            {
                SetPropertyValue<CsPrioridade>("CsPrioridade", ref csPrioridade, value);
            }
        }

        /// <summary>
        /// Metodo que cria um coleção de historico de orçamento
        /// </summary>
        [Custom("Caption", "Histórico")]
        [Association("SolicitacaoOrcamento_SolicitacaoOrcamentoHistoricos", typeof(SolicitacaoOrcamentoHistorico)), Aggregated]
        public XPCollection<SolicitacaoOrcamentoHistorico> SolicitacaoOrcamentoHistoricos
        {
            get
            {
                return GetCollection<SolicitacaoOrcamentoHistorico>("SolicitacaoOrcamentoHistoricos");
            }
        }

        #endregion

        #region Dados da Solicitação de Orçamento

        /// <summary>
        /// Código
        /// </summary>
        [Size(100)]
        [Custom("Caption", "Código")]
        [AppearanceAttribute("SolicitacaoOrcamento_TxCodigo_Appearance",
            Enabled = false,
            TargetItems = "TxCodigo")]
        public string TxCodigo
        {
            get
            {
                if (string.IsNullOrEmpty(txCodigo))
                {
                    return "Será criado automaticamente";
                }

                return txCodigo;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxCodigo", ref txCodigo, value);
            }
        }

        /// <summary>
        /// Título
        /// </summary>
        [Size(255)]
        [Custom("Caption", "Título")]
        [RuleUniqueValue("SolicitacaoOrcamento_TxTitulo_Unique", DefaultContexts.Save,
        "Já existe uma solicitação com esse título!")]
        [RuleRequiredField("SolicitacaoOrcamento_TxTitulo_Required", DefaultContexts.Save,
        Name = "Título")]
        public string TxTitulo
        {
            get
            {
                return txTitulo;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxTitulo", ref txTitulo, value);
            }
        }

        /// <summary>
        /// Data prazo
        /// </summary>
        [Custom("Caption", "Prazo")]
        [RuleRequiredField("SolicitacaoOrcamento_DtPrazo_Required", DefaultContexts.Save,
        Name = "Prazo")]
        public DateTime DtPrazo
        {
            get
            {

                return dtPrazo;
            }
            set
            {
                SetPropertyValue<DateTime>("DtPrazo", ref dtPrazo, value);
            }
        }

        /// <summary>
        /// Valor da SEOT
        /// </summary>
        [Custom("Caption", "Valor")]
        public float NbValor
        {
            get
            {

                return nbValor;
            }
            set
            {
                SetPropertyValue<float>("NbValor", ref nbValor, value);
            }
        }

        /// <summary>
        /// Data de Entrega
        /// </summary>
        [Custom("Caption", "Data de Entrega")]
        public DateTime DtEntrega
        {
            get
            {

                return dtEntrega;
            }
            set
            {
                SetPropertyValue<DateTime>("DtEntrega", ref dtEntrega, value);
            }
        }

        /// <summary>
        /// Repositório
        /// </summary>
        [Size(255)]
        [Custom("Caption", "Repositório")]
        public string TxRepositorio
        {
            get
            {
                return txRepositorio;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxRepositorio", ref txRepositorio, value.Trim());
            }
        }

        #endregion

        #region Dados do Contato

        /// <summary>
        /// Objeto de EmpresaInstituicao
        /// </summary>
        [Custom("Caption", "Cliente")]
        [ImmediatePostData]
        [RuleRequiredField("SolicitacaoOrcamento_Cliente_Required", DefaultContexts.Save,
        Name = "Cliente")]
        public EmpresaInstituicao Cliente
        {
            get
            {
                return cliente;
            }
            set
            {
                if (value != null && value != cliente && !IsLoading && !IsSaving)
                {
                    value.TxNome = value.TxNome.Trim();
                    TxContatoCliente = value.TxNome;
                    TxEmailContatoCliente = value.TxEmail;
                    TxFone = value.TxFoneFax;
                }

                SetPropertyValue<EmpresaInstituicao>("Cliente", ref cliente, value);
            }
        }

        /// <summary>
        /// Contato do cliente
        /// </summary>
        [Size(255)]
        [Custom("Caption", "Empresa")]
        [VisibleInListView(false)]
        [RuleRequiredField("SolicitacaoOrcamento_TxContatoCliente_Required", DefaultContexts.Save,
        Name = "Nome")]
        public string TxContatoCliente
        {
            get
            {
                return txContatoCliente;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxNome", ref txContatoCliente, value.Trim());
            }
        }

        /// <summary>
        /// Email do cliente
        /// </summary>
        [Size(100)]
        [Custom("Caption", "E-mail")]
        [VisibleInListView(false)]
        [RuleRequiredField("SolicitacaoOrcamento_TxEmailContatoCliente_Required", DefaultContexts.Save,
        Name = "E-mail")]
        public string TxEmailContatoCliente
        {
            get
            {
                return txEmailContatoCliente;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxEmailContatoCliente", ref txEmailContatoCliente, value.Trim());
            }
        }

        /// <summary>
        /// Telefone do cliente
        /// </summary>
        [Size(30)]
        [Custom("Caption", "Fone/Fax")]
        [VisibleInListView(false)]
        public string TxFone
        {
            get
            {
                return txFone;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxFone", ref txFone, value.Trim());
            }
        }

        /// <summary>
        /// Descrição da solicitação
        /// </summary>
        [Custom("Caption", "Descrição")]
        [Size(SizeAttribute.Unlimited)]
        public string TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxDescricao", ref txDescricao, value.Trim());
            }
        }

        /// <summary>
        /// Observação da solicitação
        /// </summary>
        [Custom("Caption", "Observação")]
        [Size(SizeAttribute.Unlimited)]
        public string TxObservacao
        {
            get
            {
                return txObservacao;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxObservacao", ref txObservacao, value.Trim());
            }
        }

        #endregion

        /// <summary>
        /// Data de emissão da solicitação
        /// </summary>
        [Custom("Caption", "Emissão")]
        [VisibleInDetailView(false)]
        public DateTime DtEmissao
        {
            get
            {
                return dtEmissao;
            }
            set
            {
                SetPropertyValue<DateTime>("DtEmissao", ref dtEmissao, value);
            }
        }

        /// <summary>
        /// Data de conclusão da solicitação
        /// </summary>
        [Custom("Caption", "Conclusão")]
        [VisibleInDetailView(false)]
        [ImmediatePostData]
        public DateTime DtConclusao
        {
            get
            {
                return dtConclusao;
            }
            set
            {
                SetPropertyValue<DateTime>("DtConclusao", ref dtConclusao, value);
            }
        }
        
        /// <summary>
        /// Texto de emails para serem enviados com cópia
        /// </summary>
        [Custom("Caption", "Cc")]
        [VisibleInListView(false)]
        public string TxCc
        {
            get
            {
                return txCc;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                SetPropertyValue<string>("TxCc", ref txCc, value);
            }
        }

        /// <summary>
        /// Texto de emails para serem enviados com cópia oculta
        /// </summary>
        [Custom("Caption", "Cco")]
        [VisibleInListView(false)]
        public string TxCco
        {
            get
            {
                return txCco;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                SetPropertyValue<string>("TxCco", ref txCco, value);
            }
        }
        
        /// <summary>
        /// Guarda o último histórico da SEOT
        /// </summary>
        [Custom("Caption", "Comentário")]
        [Size(SizeAttribute.Unlimited)]
        public string TxUltimoComentario
        {
            get
            {
                return txUltimoComentario;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                SetPropertyValue<string>("TxUltimoComentario", ref txUltimoComentario, value);
            }
        }

        #endregion

        #region NonPersistent Properties

        /// <summary>
        /// Total de dias gastos
        /// </summary>
        [NonPersistent]
        [Custom("Caption", "Dias Gastos")]
        [VisibleInDetailView(false)]
        public int _DiasGastos
        {
            get
            {
                return DtPrazo.Date.Subtract(DtEmissao.Date).Days;
            }
        }

        /// <summary>
        /// Solicitante do Orçamento
        /// </summary>
        [Custom("Caption", "Solicitante")]
        [VisibleInListView(false)]
        [AppearanceAttribute("SolicitacaoOrcamento_Solicitante_Appearance",
        Enabled = false,
        TargetItems = "Solicitante")]
        public string _Solicitante
        {
            get
            {
                string _solicitante = string.Empty;

                if (Solicitante != null)
                {
                    if (!string.IsNullOrEmpty(Solicitante.FullName))
                        _solicitante = Solicitante.FullName;
                    else
                        _solicitante = Solicitante.UserName;
                }
                return _solicitante;
            }
        }

        /// <summary>
        /// Indica se o email foi enviado com sucesso ou não
        /// </summary>
        [NonPersistent, Browsable(false)]
        public bool _EmailEnviado
        {
            get;
            set;
        }
        /// <summary>
        /// propriedade que retorna o texto do corpo do e-mail
        /// </summary>
        [NonPersistent, Browsable(false)]
        public string _TextoEmail
        {
            get
            {
                return textoEmail;
            }
        }
        /// <summary>
        /// Usado para não exibir o código da solicitação quando for um
        /// novo objetos
        /// </summary>
        /// <param name="active">Ativo</param>
        /// <returns>Se é pra esconder ou não</returns>
        [EditorStateRule("SolicitacaoOrcamento_TxCodigo_EditorStateRule",
            "TxCodigo", ViewType.DetailView)]
        public EditorState HiddenTxCodigo(out bool active)
        {
            active = Oid.Equals(new Guid());
            return EditorState.Hidden;
        }

        /// <summary>
        /// Mostra a situação na tela web sem hiperlink
        /// </summary>
        [Custom("Caption", "Situação")]
        [NonPersistent]
        public string _Situacao
        {
            get
            {
                if (Situacao != null)
                {
                    return Situacao.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Mostra o responsavel na tela web sem hiperlink
        /// </summary>
        [Custom("Caption", "Responsavel")]
        [NonPersistent]
        public string _Responsavel
        {
            get
            {
                return Responsavel != null ? Responsavel.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Mostra o tipo de solicitação na tela web sem hiperlink
        /// </summary>
        [Custom("Caption", "Tipo de Solicitação")]
        [NonPersistent]
        public string _TipoSolicitacao
        {
            get
            {
                if (tipoSolicitacao != null)
                    return tipoSolicitacao.ToString();
                return "";
            }
        }

        /// <summary>
        /// Grupo de clientes
        /// </summary>
        [Custom("Caption", "Cliente")]
        [NonPersistent]
        [VisibleInDetailView(false)]
        public String _ClienteGroup
        {
            get
            {
                return Cliente != null ? Cliente.TxNome : string.Empty;
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Indica se o objeto está sendo alterado
        /// </summary>
        /// <returns>Alterado?</returns>
        public bool RnVerificarAlteracao()
        {
            return !Oid.Equals(new Guid());
        }

        /// <summary>
        /// Cria o número referente à nova solicitação (para o código)
        /// </summary>
        /// <returns>String</returns>
        private string RnCriarNumeroSolicitacao()
        {
            // Indicando o novo número da Solicitação
            string numero = (GetQuantidadeSolicitacoesCliente(Cliente) + 1).ToString();

            // Acrescentando os 0 à esquerda da string
            numero = numero.PadLeft(TamanhoMinimoNumeroSolicitacao, '0');

            return numero;
        }

        /// <summary>
        /// Método para salvar
        /// </summary>
        protected override void OnSaved()
        {
            if (Situacao != oldSituacao || !string.IsNullOrEmpty(TxUltimoComentario))
            {
                _EmailEnviado = EnviarEmails();
            }
            else
            {
                _EmailEnviado = false;
            }

            base.OnSaved();
        }

        /// <summary>
        /// Método chamado ao salvar
        /// </summary>
        protected override void OnSaving()
        {
            if (Oid.Equals(new Guid()))
            {
                TxUltimoComentario = "Criação do Documento";
            }

            // Código calculado
            if (oldCliente != Cliente || string.IsNullOrEmpty(txCodigo))
            {
                TxCodigo = String.Format("{0}.{1}/{2}", Cliente.TxSigla, RnCriarNumeroSolicitacao(), DateUtil.ConsultarDataHoraAtual().Year);
                SalvarSolicitacaoOrcamentoHistorico();
            }
            else
            {
                if (Situacao != oldSituacao || Oid.Equals(new Guid()) || !string.IsNullOrEmpty(TxUltimoComentario))
                {
                    SalvarSolicitacaoOrcamentoHistorico();
                }
            }
    
            //verificando dados da instituição
            VerificarAlteracaoDadosEmpresaInstituicao();

            Colaborador colaborador = Colaborador.GetColaboradorCurrent(Session, UsuarioDAO.GetUsuarioLogado(Session));

            if (colaborador != null)
            {
                colaborador.ColaboradorUltimoFiltro.LastTipoSolicitacaoSEOT = TipoSolicitacao;
                colaborador.ColaboradorUltimoFiltro.LastEmpresaInstituicaoSEOT = Cliente;
                colaborador.Save();
            }

            base.OnSaving();
        }

        /// <summary>
        /// adiciona dados a um historico de solicitação de orçamento
        /// </summary>
        private void SalvarSolicitacaoOrcamentoHistorico()
        {
            SolicitacaoOrcamentoHistorico soh = new SolicitacaoOrcamentoHistorico(Session)
            {
                SolicitacaoOrcamento = this,
                ResponsavelHistorico = Responsavel,
                AtualizadoPor = Colaborador.GetColaboradorCurrent(Session),
                Situacoes = Situacao,
                Comentario = TxUltimoComentario,
                DataHora = DateUtil.ConsultarDataHoraAtual()
            };

            soh.Save();
        }

        /// <summary>
        /// Verifica se houve alguma alteração dos dados do cliente na Solicitação, pois se houver,
        /// altera também os dados na classe EmpresaInstituicao
        /// </summary>
        private void VerificarAlteracaoDadosEmpresaInstituicao()
        {
            if (Cliente == null)
                return;

            bool alteracao = false;

            if (TxContatoCliente != Cliente.TxNome)
            {
                Cliente.TxNome = TxContatoCliente;
                alteracao = true;
            }

            if (TxEmailContatoCliente != Cliente.TxEmail)
            {
                Cliente.TxEmail = TxEmailContatoCliente;
                alteracao = true;
            }

            if (TxFone != Cliente.TxFoneFax)
            {
                Cliente.TxFoneFax = TxFone;
                alteracao = true;
            }

            if (alteracao)
                Cliente.Save();
        }

        /// <summary>
        /// Verificação da lista de emails a serem enviados com cópia
        /// </summary>
        [RuleFromBoolProperty("RnValidarEmailsCc", DefaultContexts.Save,
            "Existe(m) email(s) a ser(em) enviado(s) com cópia que possui(em) erros.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarEmailsCc
        {
            get
            {
                TxCc = StrUtil.RetirarConteudoDesnecessarioStringComSeparador(TxCc, ';');

                if (!string.IsNullOrEmpty(TxCc))
                {
                    foreach (string cc in TxCc.Split(';'))
                    {
                        if (!EmailUtil.ValidarEmail(cc))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Verificação da lista de emails a serem enviados com cópia oculta
        /// </summary>
        [RuleFromBoolProperty("RnValidarEmailsCco", DefaultContexts.Save,
            "Existe(m) email(s) a ser(em) enviado(s) com cópia oculta que possui(em) erros.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarEmailsCco
        {
            get
            {
                TxCco = StrUtil.RetirarConteudoDesnecessarioStringComSeparador(TxCco, ';');

                if (!string.IsNullOrEmpty(TxCco))
                {
                    foreach (string cco in TxCco.Split(';'))
                    {
                        if (!EmailUtil.ValidarEmail(cco))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Validação da dataHora prazo => a dataHora prazo precisa ser maior ou igual a dataHora atual
        /// </summary>
        [RuleFromBoolProperty("ValidarDtPrazo", DefaultContexts.Save,
            "O Prazo precisa ser maior ou igual a data e hora atual.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarDtPrazo
        {
            get
            {
                if (!Oid.Equals(new Guid()))
                    return true;
                else
                    return DtPrazo.Date >= DateTime.Now.Date;
            }
        }

        /// <summary>
        /// Validação do email do contato
        /// </summary>
        [RuleFromBoolProperty("ValidarEmailContatoCliente", DefaultContexts.Save,
        "Digite um Email para o Contato válido.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarEmail
        {
            get
            {
                return EmailUtil.ValidarEmail(TxEmailContatoCliente);
            }
        }

        /// <summary>
        /// Regra de negócio que calcula a prioridade das estórias que são substituidas nos itens do ciclo 
        /// </summary>
        [RuleFromBoolProperty("ValidarSalvamento", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Você não pode salvar uma solicitação se não comentar")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarSalvamento
        {
            get
            {
                if (!Oid.Equals(new Guid()) && (TxUltimoComentario == null || TxUltimoComentario.Trim().Equals(string.Empty)))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Metodo que verifica se o responsavel tem seot para ser adicionado ao filtro de responsavel
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="usuario">usuario de quem se verificar se o mesmo tem seot associada a ele</param>
        /// <returns>retorna true quando o usuario tem seot e false quando não tem</returns>
        public static bool RnSeotsPorResponsavel(Session session, User usuario)
        {
            XPCollection<SolicitacaoOrcamento> seot = new XPCollection<SolicitacaoOrcamento>(session);

            var list = from item in seot
                        where usuario.Oid == item.Responsavel.Usuario.Oid
                        select item;

            if (list.ToList().Count > 0)
            {
                if (!usuario.FullName.Equals("Administrador"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo que verifica se a situacao tem seot para ser adicionada ao filtro de situacao
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="situacao">Situacao para verificar se tem seot associada a ela</param>
        /// <returns>retorna true quando a situacao tem seot e false quando não tem</returns>
        public static bool RnSeotsPorSituacao(Session session, ConfiguracaoDocumentoSituacao situacao)
        {
            XPCollection<SolicitacaoOrcamento> seot = new XPCollection<SolicitacaoOrcamento>(session);

            var list = from item in seot
                       where situacao.Oid == item.Situacao.Oid
                       select item;

            if (list.ToList().Count > 0)
            {
               return true; 
            }
            return false;
        }


        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// Obtém a quantidade de solicitações por Cliente
        /// </summary>
        /// <param name="cliente">Objeto de EmpresaInstituicao</param>
        /// <returns>A quantidade de solicitações</returns>
        private int GetQuantidadeSolicitacoesCliente(EmpresaInstituicao cliente)
        {
            return new XPCollection<SolicitacaoOrcamento>(Session,
            CriteriaOperator.Parse("Cliente = ?", cliente)).Count;
        }

        #endregion

        #region Utils

        /// <summary>
        /// Chamado ao referenciar o objeto como string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return TxCodigo;
        }

        /// <summary>
        /// Ajusta os textos de emails para envio com cópia e com cópia oculta
        /// </summary>
        private void InserirEmailsCcCcoTexto()
        {
            /*string emailsCc = string.Empty, emailsCco = string.Empty;

            if (Situacao != null)
            {
                string key = Situacao.Oid.ToString();

                if (_dicEmailsCc.ContainsKey(key))
                {
                    emailsCc = this._dicEmailsCc[key];
                }
                else
                {
                    foreach (ConfiguracaoDocumentoSituacaoEmailCc cc in Situacao.ComCopia)
                    {
                        emailsCc += String.Format("{0};", cc.TxEmail);
                    }

                    if (emailsCc.Length > 0)
                    {
                        emailsCc = emailsCc.Substring(0, emailsCc.Length - 1);
                    }

                    this._dicEmailsCc.Add(key, emailsCc);
                }

                if (_dicEmailsCco.ContainsKey(key))
                {
                    emailsCco = this._dicEmailsCco[key];
                }
                else
                {
                    foreach (ConfiguracaoDocumentoSituacaoEmailCco cco in Situacao.ComCopiaOculta)
                    {
                        emailsCco += String.Format("{0};", cco.TxEmail);
                    }

                    if (emailsCco.Length > 0)
                    {
                        emailsCco = emailsCco.Substring(0, emailsCco.Length - 1);
                    }

                    this._dicEmailsCco.Add(key, emailsCco);
                }
            }

            TxCc = emailsCc;
            TxCco = emailsCco;*/

            if (Situacao != null)
            {
                TxCc = Situacao.TxCc;
                TxCco = Situacao.TxCco;
            }
        }

        /// <summary>
        /// Gerar Assunto do Email
        /// </summary>
        /// <returns>Assunto do Email</returns>
        private string GerarAssuntoEmailSeot()
        {
            string txComentario = StrUtil.LimitarTamanhoColuna(50, TxUltimoComentario);

            return string.Format("[Solicitação de Orçamento] {0} ({1}) - {2}", TxCodigo, Situacao.TxDescricao, txComentario);
        }

        /// <summary>
        /// Envio de email
        /// </summary>
        /// <returns>True se o email foi enviado e False se não foi</returns>
        private bool EnviarEmails()
        {
            try
            {
                // Dados não obrigatórios para inserção no email
                string tipoSolic;
                string reposit;
                string desc;
                string obs;

                if (TipoSolicitacao != null)
                {
                    tipoSolic = TipoSolicitacao.TxDescricao;
                }
                else
                {
                    tipoSolic = "<i>Não informado</i>";
                }

                if (!string.IsNullOrEmpty(TxRepositorio))
                    reposit = TxRepositorio;
                else
                    reposit = "<i>Não informado</i>";

                if (!string.IsNullOrEmpty(TxDescricao))
                    desc = TxDescricao;
                else
                    desc = "<i>Não informada</i>";

                if (!string.IsNullOrEmpty(TxObservacao))
                    obs = TxObservacao;
                else
                    obs = "<i>Não informada</i>";

                MailMessage mail = new MailMessage() { From = new MailAddress(Solicitante.Email, "WexProject") };
                mail.To.Add(new MailAddress(Responsavel.Usuario.Email, Responsavel.Usuario.FullName));

                // Emails com cópia
                foreach (string cc in TxCc.Split(';'))
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        mail.CC.Add(cc);
                    }
                }

                // Emails com cópia oculta
                foreach (string cco in TxCco.Split(';'))
                {
                    if (!string.IsNullOrEmpty(cco))
                    {
                        mail.Bcc.Add(cco);
                    }
                }

                mail.Subject = GerarAssuntoEmailSeot();
                mail.IsBodyHtml = true;

                // Corpo do email
                string body;
                const string Separador = "<br style='clear: both;'>";
                string styleFonte = "font-family: Arial; font-size: 12px;";
                string styleDivTitle = "style='background-color: #006600; color: #FFFFFF; text-align: left;";
                styleDivTitle += "font-weight: bold; width: 100%; margin: 0px; padding: 3px;'";

                body = String.Format("<div style='width: 100%; {0}'>", styleFonte);

                //Informação Mais Recente

                var ultimo_historico = SolicitacaoOrcamentoHistorico.GetUltimoHistorico(this);

                body += String.Format("<div {0}>Informação Mais Recente</div>", styleDivTitle);
                body += String.Format("{0}<b>Situação:</b> {1}", Separador, Situacao.TxDescricao);
                body += String.Format("{0}<b>Comentário:</b> {1}", Separador, ultimo_historico.Comentario);
                body += String.Format("{0}<b>Data/Hora:</b> {1}", Separador, ultimo_historico.DataHora.ToString("dd/MM/yyyy hh:mm"));
                body += String.Format("{0}<b>Alterado Por:</b> {1} {2}", Separador, ultimo_historico.AtualizadoPor.Usuario.FullName, Separador);
                // Dados Gerais                
                body += String.Format("{0}<div {1}>Dados Gerais</div>", Separador, styleDivTitle);
                body += String.Format("{0}<b>Solicitante:</b> {1}", Separador, _Solicitante);
                body += String.Format("{0}<b>Responsável:</b> {1} ({2})", Separador, Responsavel.Usuario.FullName, Responsavel.Usuario.UserName);                
                body += String.Format("{0}<b>Tipo:</b> {1}", Separador, tipoSolic);
                body += String.Format("{0}<b>Prioridade:</b> {1} {2}", Separador, EnumUtil.DescricaoEnum(CsPrioridade), Separador);
                // Dados da Solicitação de Orçamento
                body += String.Format("{0}<div {1}>Dados da Solicitação de Orçamento</div>", Separador, styleDivTitle);
                body += String.Format("{0}<b>Código:</b> {1}", Separador, TxCodigo);
                body += String.Format("{0}<b>Título:</b> {1}", Separador, TxTitulo);
                body += String.Format("{0}<b>Prazo:</b> {1}", Separador, DtPrazo.ToString("dd/MM/yyyy"));
                body += String.Format("{0}<b>Repositório:</b> {1} {2}", Separador, reposit, Separador);
                // Dados do Cliente
                body += String.Format("{0}<div {1}>Dados do Cliente</div>", Separador, styleDivTitle);
                body += String.Format("{0}<b>Cliente:</b> {1}", Separador, Cliente.TxNome);
                body += String.Format("{0}<b>Nome:</b> {1}", Separador, TxContatoCliente);
                body += String.Format("{0}<b>E-mail:</b> {1}", Separador, TxEmailContatoCliente);
                body += String.Format("{0}<b>Fone/Fax:</b> {1} {2}", Separador, TxFone, Separador);

                // Outros
                body += String.Format("{0}<div {1}>Outros</div>", Separador, styleDivTitle);
                body += String.Format("{0}<b>Descrição:</b> {1}", Separador, desc);
                body += String.Format("{0}<b>Observação:</b> {1} {2}", Separador, obs, Separador);

                // Tabela Histórico
                string stylePositionTable = "position: relative;left: -1.7px;top: -1px;right: -50px;";
                body += String.Format("{0}<div {1}>Histórico</div>", Separador, styleDivTitle);                
                body += String.Format("<table style='width: 100%;{0};'><tbody><tr {1}>", stylePositionTable, styleDivTitle);
                body += String.Format("<td><div style='{0}'>Data/Hora</div></td>", styleFonte + "color: #FFFFFF;");
                body += String.Format("<td><div style='{0}'>Situação</div></td>", styleFonte + "color: #FFFFFF;");
                body += String.Format("<td><div style='{0}'>Responsavél</div></td>", styleFonte + "color: #FFFFFF;");
                body += String.Format("<td><div style='{0}'>Comentário</div></td>", styleFonte + "color: #FFFFFF;");
                body += String.Format("<td><div style='{0}'>Alterado Por</div></td></tr>", styleFonte + "color: #FFFFFF;");

                /*body += String.Format("{0}<table style='width: 100%;{1}'><tbody><tr {2}>", Separador, styleFonte, styleDivTitle);
                body += "<td>Data/Hora</td>";
                body += "<td>Situação</td>";
                body += "<td>Responsavél</td>";
                body += "<td>Comentário</td>";
                body += "<td>Alterado Por</td></tr>";*/

                foreach (SolicitacaoOrcamentoHistorico soh in SolicitacaoOrcamentoHistoricos)
                {
                    body += "<tr>";
                    body += String.Format("<td>{0}</td>", soh.DataHora.ToString("dd/MM/yyyy HH:mm"));
                    body += String.Format("<td>{0}</td>", soh.Situacoes);
                    body += String.Format("<td>{0}</td>", soh.ResponsavelHistorico);
                    body += String.Format("<td>{0}</td>", soh.Comentario);

                    if (soh.AtualizadoPor != null)
                    {
                        body += String.Format("<td>{0}</td>", soh.AtualizadoPor.ToString());
                    }
                    else
                    {
                        body += String.Format("<td>{0}</td>", string.Empty);
                    }

                    body += "</tr>";

                }

                body += "</tbody></table></div>";

                mail.Body = body;
                textoEmail = body;

                SmtpClient smtp = new SmtpClient("fpfmail.dom.fpf.br", 25) { EnableSsl = false };
                smtp.Send(mail);
                
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        /// <summary>
        /// Obter apenas colaboradores que são superiores
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <returns>Lista de colaboradores que são superiores</returns>
        public List<Colaborador> GetAllSuperioresImediatos
        {
            get
            {
                return Colaborador.GetAllSuperioresImediatos(Session);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public SolicitacaoOrcamento(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Depois de construir o objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            TxCc = TxCco = TxUltimoComentario = string.Empty;

            _dicEmailsCc = new Dictionary<string, string>();
            _dicEmailsCco = new Dictionary<string, string>();

            // Usuário logado é o solicitante
            Solicitante = UsuarioDAO.GetUsuarioLogado(Session);

            // Data atual
            DtConclusao = DtEmissao = DateUtil.ConsultarDataHoraAtual();

            // Prazo para depois de 10 dias
            DtPrazo = Calendario.AcrescimoDiasUteisData(Session, DateUtil.ConsultarDataHoraAtual(), 10);

            // Situação inicial
            Situacao = ConfiguracaoDocumentoSituacao.GetSituacaoInicial(
                ConfiguracaoDocumento.GetConfiguracaoPorTipo(Session, CsTipoDocumento.SolicitacaoOrcamento));

            Colaborador colaborador = Colaborador.GetColaboradorCurrent(Session, UsuarioDAO.GetUsuarioLogado(Session));
            if (colaborador != null)
            {
                TipoSolicitacao = colaborador.ColaboradorUltimoFiltro.LastTipoSolicitacaoSEOT;
                Cliente = colaborador.ColaboradorUltimoFiltro.LastEmpresaInstituicaoSEOT;
            }
        }

        /// <summary>
        /// Ao dar load no objeto
        /// </summary>
        protected override void OnLoaded()
        {
            if (oldSituacao == null)
            {
                oldSituacao = Situacao;
            }

            if (oldCliente == null)
            {
                oldCliente = Cliente;
            }

            if (_dicEmailsCc == null)
            {
                _dicEmailsCc = new Dictionary<string, string>();
            }

            if (_dicEmailsCco == null)
            {
                _dicEmailsCco = new Dictionary<string, string>();
            }

            if (SolicitacaoOrcamentoHistoricos.Sorting.Count == 0)
            {
                SolicitacaoOrcamentoHistoricos.Sorting.Add(new SortProperty("DataHora", SortingDirection.Descending));
            }

            base.OnLoaded();
        }

        #endregion
    }
}
