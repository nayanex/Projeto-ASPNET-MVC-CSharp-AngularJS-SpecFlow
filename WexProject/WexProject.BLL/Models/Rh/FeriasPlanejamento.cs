using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.ComponentModel;
using WexProject.BLL.Shared.Domains.Rh;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.DAOs;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Planejamento de férias
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Planejamento de Férias")]
    [Appearance("Situacao_ColorRed_ListView", AppearanceItemType = "ViewItem", TargetItems = "CsSituacao",
       Criteria = "DtInicio < LocalDateTimeToday()", Context = "ListView", BackColor = "Red", FontColor = "Black", Priority = 1)]
    [Appearance("Situacao_ColorBlue_ListView", AppearanceItemType = "ViewItem", TargetItems = "CsSituacao",
       Criteria = "Realizadas = True", Context = "ListView", BackColor = "LightCyan", FontColor = "Black", Priority = 1)]
    [Appearance("Situacao_ColorGreen_ListView", AppearanceItemType = "ViewItem", TargetItems = "CsSituacao",
       Criteria = "Vender = ##Enum#WexProject.BLL.Shared.Domains.Geral.CsSimNao,Sim#", Context = "ListView", BackColor = "LimeGreen", FontColor = "Black", Priority = 2 )]
    [Appearance("Situacao_ColorYellow_ListView", AppearanceItemType = "ViewItem", TargetItems = "CsSituacao",
       Criteria = "DtInicio >= LocalDateTimeToday()", Context = "ListView", BackColor = "Yellow", FontColor = "Black", Priority = 1)]
    [OptimisticLocking( false )]
    public class FeriasPlanejamento : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Associação com período aquisitivo
        /// </summary>
        private ColaboradorPeriodoAquisitivo periodo;

        /// <summary>
        /// Atributo de Modalidade
        /// </summary>
        private ModalidadeFerias modalidade;

        /// <summary>
        /// Atributo de DtInicio
        /// </summary>
        private DateTime dtInicio;

        /// <summary>
        /// Atributo de Realizadas
        /// </summary>
        private bool realizadas;

        /// <summary>
        /// Atributo de Vender
        /// </summary>
        private CsSimNao vender;

        /// <summary>
        /// Quem planejou (as férias) e quando
        /// </summary>
        private String txPlanejado;

        /// <summary>
        /// Quem atualizou (as férias) e quando
        /// </summary>
        private String txAtualizado;

        /// <summary>
        /// Ligação com o Afastamento (caso seja criado algum para o Planejamento)
        /// </summary>
        private ColaboradorAfastamento afastamento;

        /// <summary>
        /// Situação do Planejamento de Férias
        /// </summary>
        private CsSituacaoFeriasPlanejamento csSituacaoFerias;

        /// <summary>
        /// Objeto de Planejamento de Férias antigo
        /// </summary>
        private FeriasPlanejamento planejamentoOld;

        /// <summary>
        /// Armazena a data calculada de retorno das ferias.
        /// </summary>
        private DateTime dtRetorno;

        #endregion

        #region Properties

        /// <summary>
        /// Associação com período aquisitivo
        /// </summary>
        [Indexed]
        [VisibleInDetailView(false)]
        [Association("ColaboradorPeriodoAquisitivo_FeriasPlanejamento", typeof(ColaboradorPeriodoAquisitivo))]
        [RuleRequiredField("FeriasPlanejamento_Periodo_Required", DefaultContexts.Save)]
        public ColaboradorPeriodoAquisitivo Periodo
        {
            get
            {
                return periodo;
            }
            set
            {
                SetPropertyValue<ColaboradorPeriodoAquisitivo>("Periodo", ref periodo, value);
            }
        }

        /// <summary>
        /// Modalidade do Planejamento
        /// </summary>
        [Custom("Caption", "Modalidade")]
        [ImmediatePostData]
        [RuleRequiredField("FeriasPlanejamento_Modalidade_Required", DefaultContexts.Save,
        Name = "Modalidade")]
        public ModalidadeFerias Modalidade
        {
            get
            {
                return modalidade;
            }
            set
            {
                SetPropertyValue<ModalidadeFerias>("Modalidade", ref modalidade, value);

                if (Modalidade != null && !Modalidade.PodeVender)
                    Vender = CsSimNao.Não;
            }
        }

        /// <summary>
        /// Situação do Planejamento de Férias
        /// </summary>
        [Custom("Caption", "Situação"), VisibleInListView(false), ImmediatePostData]
        public CsSituacaoFeriasPlanejamento CsSituacaoFerias
        {
            get
            {
                return csSituacaoFerias;
            }
            set
            {
                SetPropertyValue<CsSituacaoFeriasPlanejamento>("CsSituacaoFerias", ref csSituacaoFerias, value);

                if (value == CsSituacaoFeriasPlanejamento.Realizado)
                {
                    Realizadas = true;
                }
                else
                {
                    Realizadas = false;
                }
            }
        }

        /// <summary>
        /// Situação do Planejamento de Férias (Calculada)
        /// </summary>
        [Indexed]
        [NonPersistent, Custom("Caption", "Situação"), VisibleInDetailView(false)]
        public CsSituacaoFerias CsSituacao
        {
            get
            {
                if (Vender == CsSimNao.Sim)
                {
                    return WexProject.BLL.Shared.Domains.Rh.CsSituacaoFerias.Vendida;
                }

                if (Realizadas)
                {
                    return WexProject.BLL.Shared.Domains.Rh.CsSituacaoFerias.Realizado;
                }

                if (DtInicio.Date < DateUtil.ConsultarDataHoraAtual().Date)
                {
                    return WexProject.BLL.Shared.Domains.Rh.CsSituacaoFerias.EmAtraso;
                }

                return WexProject.BLL.Shared.Domains.Rh.CsSituacaoFerias.Planejado;
            }
        }

        /// <summary>
        /// Data de início das férias
        /// </summary>
        [Custom("Caption", "Início")]
        [ImmediatePostData]
        [RuleRequiredField("FeriasPlanejamento_DtInicio_Required", DefaultContexts.Save,
        Name = "Início")]
        public DateTime DtInicio
        {
            get
            {
                return dtInicio;
            }
            set
            {
                SetPropertyValue<DateTime>("DtInicio", ref dtInicio, value);
            }
        }

        /// <summary>
        /// Se as férias serão vendidas
        /// </summary>
        [Custom("Caption", "Vender")]
        public CsSimNao Vender
        {
            get
            {
                return vender;
            }
            set
            {
                SetPropertyValue<CsSimNao>("Vender", ref vender, value);
            }
        }

        /// <summary>
        /// Se as férias já foram realizadas
        /// </summary>
        [Custom("Caption", "Realizadas")]
        public bool Realizadas
        {
            get
            {
                return realizadas;
            }
            set
            {
                SetPropertyValue<bool>("Realizadas", ref realizadas, value);
            }
        }

        /// <summary>
        /// Quem planejou (as férias) e quando
        /// </summary>
        [Custom("Caption", "Planejado por")]
        [Size(100)]
        [AppearanceAttribute("FeriasPlanejamento_TxPlanejado_Appearance",
            Enabled = false,
            TargetItems = "TxPlanejado")]
        public string TxPlanejado
        {
            get
            {
                return txPlanejado;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxPlanejado", ref txPlanejado, value.Trim());
            }
        }

        /// <summary>
        /// Quem fez a atualização (de férias) e quando
        /// </summary>
        [Custom("Caption", "Atualizado por")]
        [Size(100)]
        [AppearanceAttribute("FeriasPlanejamento_TxAtualizado_Appearance",
            Enabled = false,
            TargetItems = "TxAtualizado")]
        public string TxAtualizado
        {
            get
            {
                return txAtualizado;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxPlanejado", ref txAtualizado, value.Trim());
            }
        }

        /// <summary>
        /// Ligação com o Afastamento (caso seja criado algum para o Planejamento)
        /// </summary>
        [Browsable(false)]
        [Aggregated]
        public ColaboradorAfastamento Afastamento
        {
            get
            {
                return afastamento;
            }
            set
            {
                SetPropertyValue<ColaboradorAfastamento>("Afastamento", ref afastamento, value);
            }
        }

        /// <summary>
        /// Armazena a data de retorno calculada da ferias.
        /// </summary>
        public DateTime DtRetorno
        {
            get
            {
                return dtRetorno;
            }

            set
            {
                SetPropertyValue<DateTime>("DtRetorno", ref dtRetorno, value);
            }
        }


        #endregion

        #region NonPersistent Properties

        /// <summary>
        /// Data de retorno do colaborador das férias
        /// </summary>
        [NonPersistent]
        [Custom("Caption", "Retorno")]
        [AppearanceAttribute("FeriasPlanejamento_DtRetorno_Appearance",
        Enabled = false,
        TargetItems = "Retorno")]
        public DateTime _DtRetorno
        {
            get
            {
                if (DtInicio != DateTime.MinValue && Modalidade != null)
                        return DtInicio.AddDays(Modalidade.NbDias - 1).Date;

                    return DateTime.MinValue;
                }
        }
        /// <summary>
        /// Nome do Colaborador para este planjamento
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public string Colaborador_Name
                {
            get
            {
                return Periodo.Colaborador._NomeCompleto;
                }
            }

        /// <summary>
        /// Duração das férias para este planejamento
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public TimeSpan _DuracaoFerias 
        {
            get 
            {
                return _DtRetorno - DtInicio;
        }
        }

        /// <summary>
        /// Descrição das férias para o time line view
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public string _DescriptionFerias
        {
            get 
            {

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append(Periodo.Colaborador.Oid).Append(",");
                builder.Append(Periodo.Oid).Append(",");
                builder.Append(Periodo.NbFeriasPlanejadas).Append(",");
                builder.Append(Modalidade.Oid).Append(",");
                builder.Append(TxPlanejado).Append(",");
                builder.Append(TxAtualizado).Append(",");
                builder.Append(CsSituacao).Append(",");
                builder.Append(Vender).Append(",");
                builder.Append(Afastamento!=null ? Afastamento.TipoAfastamento.IsRemunerado.ToString() : "Não").Append(",");
                builder.Append(Oid).Append(",");
                builder.Append(Periodo.Colaborador.Usuario.FullName).Append(",");

                return builder.ToString();
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Quando estiver salvando o objeto
        /// </summary>
        protected override void OnSaving()
        {
            RnSomarFeriasPlanejadasPeriodo();
            RnPreencherAtualizadoPorPlanejadoPor();
            RnManipularAfastamentoFeriasRealizadas();
            DtRetorno = _DtRetorno;

            base.OnSaving();
        }     

        /// <summary>
        /// Soma das Férias Planejadas para o Período Aquisitivo
        /// </summary>
        private void RnSomarFeriasPlanejadasPeriodo()
        {
            uint qtdePlanejada = 0;

            if (Periodo == null)
                return;

            // Soma das quantidades
            foreach (FeriasPlanejamento ferias in Periodo.Planejamentos)
            {
                if (ferias.Modalidade != null)
                {
                qtdePlanejada += ferias.Modalidade.NbDias;
            }
            }

            Periodo.NbFeriasPlanejadas = qtdePlanejada;
            Periodo.Save();
        }

        /// <summary>
        /// Preenchimento do "Atualizado Por" e "Atualizado Por"
        /// </summary>
        private void RnPreencherAtualizadoPorPlanejadoPor()
        {
            if (IsLoading)
                return;

            if (Oid.Equals(new Guid()))
            {
                TxAtualizado = string.Empty;
                TxPlanejado = string.Format("{0} - {1:dd/MM/yyyy HH:mm}", UsuarioDAO.GetUsuarioLogado(Session).UserName,
                    DateUtil.ConsultarDataHoraAtual());
            }
            else
            {
                TxAtualizado = string.Format("{0} - {1:dd/MM/yyyy HH:mm}", UsuarioDAO.GetUsuarioLogado(Session).UserName,
                    DateUtil.ConsultarDataHoraAtual());
            }
        }

        /// <summary>
        /// Verifica se já existe afastamento criado
        /// </summary>
        /// <param name="dtInicio">Data de Início</param>
        /// <param name="dtTermino">Data de Término</param>
        /// <returns>Se já existe afastamento criado</returns>
        private bool NotExistAfastamento(DateTime dtInicio, DateTime dtTermino)
        {
            int qtde = 0;

            Periodo.Colaborador.Afastamentos.Filter = CriteriaOperator.Parse("DtInicio = ? AND DtTermino = ?", dtInicio, dtTermino);
            qtde = Periodo.Colaborador.Afastamentos.Count;
            Periodo.Colaborador.Afastamentos.Filter = null;

            return qtde == 0;
        }

        /// <summary>
        /// Manipulação de Afastamento para Férias Realizadas
        /// </summary>
        public void RnManipularAfastamentoFeriasRealizadas()
        {
            if ((Realizadas && Oid.Equals(new Guid())) || (planejamentoOld != null && planejamentoOld.Realizadas != Realizadas && Realizadas))
            {
                if (NotExistAfastamento(DtInicio, _DtRetorno))
                {
                    Afastamento = new ColaboradorAfastamento(Session)
                    {
                        Colaborador = Periodo.Colaborador,
                        DtInicio = DtInicio,
                        DtTermino = _DtRetorno,
                        IsCriadoAutomaticamente = true,
                        TipoAfastamento = TipoAfastamento.GetTipoAfastamentoParaFeriasRealizadas(Session),
                        TxObservacao = string.Format("Confirmação de férias planejadas para o período aquisitivo de {0:dd/MM/yyyy} a {1:dd/MM/yyyy}.",
                            DtInicio, _DtRetorno)
                    };

                    // persistencia
                    Afastamento.Save();
                }
            }
            else if(Afastamento != null)
            {
                ColaboradorAfastamento afast = Afastamento;
                Afastamento = null;
                afast.Delete();
            }
        }

        /// <summary>
        /// Validação das datas do Período de Férias
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("FeriasPlanejamento_ValidacaoPeriodoFerias", DefaultContexts.Save,
            CustomMessageTemplate = @"O Planejamento não pode ultrapassar o limite máximo para tirar Férias definido na Configuração e nem pode ser menor que a Data de Início do Período Aquisitivo.")]
        public bool _ValidacaoPeriodoFerias
        {
            get
            {
                uint mesesMaxFerias = Configuracao.GetInstancia(Session).NbDtMaxTirarFerias;

                if (_DtRetorno.Date > Periodo.DtTermino.Date.AddMonths((int)mesesMaxFerias) ||
                    DtInicio < Periodo.DtInicio)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Validação do Período de Venda de Férias
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("FeriasPlanejamento_ValidacaoVendaFerias", DefaultContexts.Save,
            CustomMessageTemplate = @"A quantidade de dias a serem Vendidos não pode ser maior que o definido na Configuração.")]
        public bool _ValidacaoVendaFerias
        {
            get
            {
                if (Modalidade != null)
                {
                    return Modalidade.NbDias <= Configuracao.GetInstancia(Session).NbQtdeMaxVenda || Vender == CsSimNao.Não;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Validação de períodos conflitantes
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("FeriasPlanejamento_ValidacaoPeriodoFeriasConflitantes", DefaultContexts.Save,
            CustomMessageTemplate = @"O período do Planejamento de Férias não pode conflitar com nenhum outro já existente para o Período Aquisitivo.")]
        public bool _ValidacaoPeriodoFeriasConflitantes
        {
            get
            {
                int qtde;

                Periodo.Planejamentos.Filter = CriteriaOperator.Parse("Oid != ? AND ((DtInicio >= ? AND _DtRetorno >= ? AND DtInicio <= ?) " +
                    "OR (DtInicio <= ? AND _DtRetorno <= ? AND _DtRetorno >= ?) OR (DtInicio <= ? AND _DtRetorno >= ?))",
                    Oid, DtInicio.Date, _DtRetorno.Date, _DtRetorno.Date, DtInicio.Date, _DtRetorno.Date, DtInicio.Date, DtInicio.Date, _DtRetorno.Date);

                qtde = Periodo.Planejamentos.Count;

                Periodo.Planejamentos.Filter = null;

                if (qtde > 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Validação da Data de Início
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("FeriasPlanejamento_ValidacaoDtInicio", DefaultContexts.Save,
            CustomMessageTemplate = @"A Data de Início do Planejamento não pode ser menor que a Data de Admissão do Colaborador.")]
        public bool _ValidacaoDtInicio
        {
            get
            {
                if (DtInicio.Date < Periodo.Colaborador.DtAdmissao.Date)
                {
                    return false;
                }

                return true;
            }
        }

        public static List<Colaborador> RNGetColaboradoresVisiveis(XPCollection<FeriasPlanejamento> planejamentos, List<Projeto> projetosOcultos)
        {
            List<Colaborador> colaboradores = new List<Colaborador>();
            List<FeriasPlanejamento> aux = RNGetPlanejamentosVisiveis(planejamentos, projetosOcultos);
            foreach (FeriasPlanejamento item in aux)
            {
                colaboradores.Add(item.Periodo.Colaborador);
            }
            return colaboradores;
        }

        public static List<FeriasPlanejamento> RNGetPlanejamentosVisiveis(XPCollection<FeriasPlanejamento> planejamentos, List<Projeto> projetosOcultos)
        {
            List<FeriasPlanejamento> ferias = new List<FeriasPlanejamento>();
            foreach (FeriasPlanejamento plan in planejamentos)
            {
                bool add = true;
                foreach (Projeto proj in projetosOcultos)
                {
                    foreach (ParteInteressada p in proj.ParteInteressada)
                    {
                        if (p.Colaborador.Equals(plan.Periodo.Colaborador))
                        {
                            add = false;
                            break;
                        }
                    }
                    if (!add)
                        break;
                }

                if (add)
                {
                    ferias.Add(plan);
                }

            }
            return ferias;
        }

        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// Resgatar o planejamento de férias que está nas buscas
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="superiores_imdediatos">Lista de superiores imaediatos</param>
        /// <param name="situacao_ferias">Lista de situação de Férias</param>
        /// <param name="startOn">datetime</param>
        /// <param name="endOn">datetime</param>
        /// <returns>FeriasPlanejamento</returns>
        public static XPCollection<FeriasPlanejamento> GetPlanejamentoFerias(Session session, string[] superiores_imediatos, string[] situacao_ferias, DateTime dtInicio, DateTime dtRetorno)
        {
            List<CriteriaOperator> criterias = new List<CriteriaOperator>();
            List<CriteriaOperator> periodos = new List<CriteriaOperator>();
            List<CriteriaOperator> superiores = new List<CriteriaOperator>();
            List<CriteriaOperator> situacoes = new List<CriteriaOperator>();

            periodos.Add(CriteriaOperator.Or(CriteriaOperator.Parse("?>=DtInicio and ?<=DtRetorno", dtInicio, dtInicio)));
            periodos.Add(CriteriaOperator.Or(CriteriaOperator.Parse("?>=DtInicio and ?<=DtRetorno", dtRetorno, dtRetorno)));
            periodos.Add(CriteriaOperator.Or(CriteriaOperator.Parse("?<DtInicio and ?>DtRetorno", dtRetorno, dtRetorno)));
            criterias.Add(CriteriaOperator.Or(periodos));

            if (superiores_imediatos != null)
            {
                foreach (string superior in superiores_imediatos)
                    superiores.Add(CriteriaOperator.Or(CriteriaOperator.Parse("Periodo.Colaborador.Coordenador=?", new Guid(superior))));

                criterias.Add(CriteriaOperator.Or(superiores));
            }
            else
            criterias.Add(CriteriaOperator.Parse("Periodo.Colaborador.Coordenador is not null"));

            if (situacao_ferias != null)
            {
            for (int i = 0; i < situacao_ferias.Length; i++)
            {
                    situacoes.Add(CriteriaOperator.Or(CriteriaOperator.Parse(situacao_ferias[i])));
            }
                criterias.Add(CriteriaOperator.Or(situacoes));
            }            

            return new XPCollection<FeriasPlanejamento>(session, CriteriaOperator.And(criterias));
        }



        #endregion

        #region UserInterface

        /// <summary>
        /// Hidden do campo de Venda
        /// </summary>
        /// <param name="active">Ativo?</param>
        /// <returns>EditorState.Hidden</returns>
        [EditorStateRule("HiddenVender", "Vender", ViewType.DetailView)]
        public EditorState HiddenVender(out bool active)
        {
            active = Modalidade != null && !Modalidade.PodeVender;
            return EditorState.Hidden;
        }

        #endregion

        #region Utils

        /// <summary>
        /// Ao dar load no objeto
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (planejamentoOld == null)
            {
                planejamentoOld = MemberwiseClone() as FeriasPlanejamento;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public FeriasPlanejamento(Session session)
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