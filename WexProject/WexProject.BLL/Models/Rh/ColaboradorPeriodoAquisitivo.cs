using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.BLL.Shared.Domains.Rh;
using System.Collections.Generic;
using WexProject.Library.Libs.Enumerator;
using DevExpress.Xpo.DB;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Período aquisitivo do colaborador
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Período Aquisitivo")]
    [OptimisticLocking( false )]
    public class ColaboradorPeriodoAquisitivo : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Associação com colaborador
        /// </summary>
        private Colaborador colaborador;

        /// <summary>
        /// Data de início do período aquisitivo
        /// </summary>
        private DateTime dtInicio;

        /// <summary>
        /// Data de término do período aquisitivo
        /// </summary>
        private DateTime dtTermino;

        /// <summary>
        /// Quantidade de dias de férias já planejadas para o período
        /// </summary>
        private uint nbFeriasPlanejadas;

        /// <summary>
        /// Texto de Planejamento de Férias
        /// </summary>
        private string planejamentoFerias = null;

        /// <summary>
        /// Situação geral das férias
        /// </summary>
        private CsSituacaoFerias _csSituacaoFerias;

        /// <summary>
        /// Data máxima que o colaborador pode tirar férias para este perído aquisitivo
        /// </summary>
        private DateTime dtMaxima;

        #endregion

        #region Propriedades

        /// <summary>
        /// Associação com colaborador
        /// </summary>
        [VisibleInDetailView(false)]
        [Association("Colaborador_ColaboradorPeriodoAquisitivo", typeof(Colaborador))]
        [RuleRequiredField("ColaboradorPeriodoAquisitivo_Colaborador_Required", DefaultContexts.Save)]
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

        /// <summary>
        /// Data de início do período aquisitivo
        /// </summary>
        [Custom("Caption", "Início")]
        [AppearanceAttribute("ColaboradorPeriodoAquisitivo_DtInicio_Appearance",
            Enabled = false,
            TargetItems = "DtInicio")]
        public DateTime DtInicio
        {
            get
            {
                return dtInicio;
            }
            set
            {
                if (dtInicio == value)
                    return;

                SetPropertyValue<DateTime>("DtInicio", ref dtInicio, value);
            }
        }

        /// <summary>
        /// Data de término do período aquisitivo
        /// </summary>
        [Custom("Caption", "Término")]
        [AppearanceAttribute("ColaboradorPeriodoAquisitivo_DtTermino_Appearance",
            Enabled = false,
            TargetItems = "DtTermino")]
        public DateTime DtTermino
        {
            get
            {
                return dtTermino;
            }
            set
            {
                if (dtTermino == value)
                    return;

                SetPropertyValue<DateTime>("DtTermino", ref dtTermino, value);
            }
        }

        /// <summary>
        /// Quantidade de dias de férias já planejadas para o período
        /// </summary>
        [Custom("Caption", "Férias Planejadas")]
        [AppearanceAttribute("ColaboradorPeriodoAquisitivo_NbFeriasPlanejadas_Appearance",
            Enabled = false,
            TargetItems = "NbFeriasPlanejadas")]
        public uint NbFeriasPlanejadas
        {
            get
            {
                return nbFeriasPlanejadas;
            }
            set
            {
                if (nbFeriasPlanejadas == value)
                    return;

                SetPropertyValue<uint>("NbFeriasPlanejadas", ref nbFeriasPlanejadas, value);
            }
        }

        /// <summary>
        /// Planejamentos de férias
        /// </summary>
        [Custom("Caption", "Planejamento de Férias")]
        [Association("ColaboradorPeriodoAquisitivo_FeriasPlanejamento",
            typeof(FeriasPlanejamento)), Aggregated]
        public XPCollection<FeriasPlanejamento> Planejamentos
        {
            get
            {
                return GetCollection<FeriasPlanejamento>("Planejamentos");
            }
        }

        /// <summary>
        /// Data máxima que o colaborador pode tirar férias para este perído aquisitivo
        /// </summary>
        [Browsable(false)]
        public DateTime DtMaxima
        {
            get { return dtMaxima; }
            set { SetPropertyValue<DateTime>("DtMaxima", ref dtMaxima, value); }
        }

        #endregion

        #region Propriedades Não Persistentes

        /// <summary>
        /// Texto do Planejamento de Férias
        /// </summary>
        [NonPersistent, Custom("Caption", "Planejamento de Férias")]
        [VisibleInDetailView(false)]
        public string _PlanejamentoFerias
        {
            get
            {
                planejamentoFerias = null;
                return CalculoTextoPlanejamentoFerias(Planejamentos, ref planejamentoFerias, ref _csSituacaoFerias);
            }
        }

        /// <summary>
        /// Situação geral das férias
        /// </summary>
        [NonPersistent, Browsable(false)]
        public CsSituacaoFerias _CsSituacaoFerias
        {
            get
            {
                return _csSituacaoFerias;
            }
            set
            {
                SetPropertyValue<CsSituacaoFerias>("_CsSituacaoFerias", ref _csSituacaoFerias, value);
            }
        }
        /// <summary>
        /// Formatação do Pe´riodo Aquisitivo para o ToString()
        /// </summary>
        [NonPersistent, Browsable(false)]
        public string _PeriodoAquisitivo
        {
            get 
            {
                return String.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", DtInicio, DtTermino);
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Cálculo do texto de planejamento de férias
        /// </summary>
        /// <param name="planCollection">Coleção de Planejamentos</param>
        /// <param name="planejamento">Texto do Planejamento de Férias</param>
        /// <param name="csSituacaoFerias">Situação de Férias</param>
        /// <returns>Texto do Planejamento de Férias</returns>
        public static string CalculoTextoPlanejamentoFerias( XPCollection<FeriasPlanejamento> planCollection, ref string planejamento, ref CsSituacaoFerias csSituacaoFerias )
        {
            if(planejamento == null)
            {
                planejamento = string.Empty;
                csSituacaoFerias = CsSituacaoFerias.Planejado;
                planCollection.Sorting.Add( new SortProperty( "DtInicio", SortingDirection.Ascending ) );

                foreach(FeriasPlanejamento plan in planCollection)
                {
                    if(plan.Oid != Guid.Empty)
                    {
                        if(plan.Vender == CsSimNao.Sim)
                        {
                            planejamento += String.Format( "{0} dia(s) (Venda das Férias) - {1:dd/MM/yyyy} - {2:dd/MM/yyyy} ({3})\n",
                                plan.Modalidade.NbDias, plan.DtInicio, plan._DtRetorno, EnumUtil.DescricaoEnum( plan.CsSituacao ) );
                        }
                        else
                        {
                            planejamento += String.Format( "{0} dia(s) - {1:dd/MM/yyyy} - {2:dd/MM/yyyy} ({3})\n",
                                plan.Modalidade.NbDias, plan.DtInicio, plan._DtRetorno, EnumUtil.DescricaoEnum( plan.CsSituacao ) );
                        }

                        // Setando a situação das férias do colaborador (em geral)
                        if(plan.CsSituacao == CsSituacaoFerias.EmAtraso)
                        {
                            csSituacaoFerias = CsSituacaoFerias.EmAtraso;
                        }
                    }
                }
            }

            return planejamento;
        }

        /// <summary>
        /// Validação da quantidade de Férias Planejadas
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("ColaboradorPeriodoAquisitivo_ValidacaoQuantidadeFerias", DefaultContexts.Save,
            CustomMessageTemplate = @"A quantidade de Férias Planejadas não pode ser maior que o definido na Configuração.")]
        public bool _ValidacaoQuantidadeFerias
        {
            get
            {
                return NbFeriasPlanejadas <= Configuracao.GetInstancia(Session).NbQtdeMaxFerias;
            }
        }

        /// <summary>
        /// Ao transformar o objeto para string
        /// </summary>
        /// <returns>Data de Início concatenada com o Término</returns>
        public override string ToString()
        {
            return _PeriodoAquisitivo;
        }

        /// <summary>
        /// Override do salving para quando salvar este perído aquisitivo
        /// criar a data máxima em que se pode tirar as férias
        /// </summary>
        protected override void OnSaving()
        {
            using (XPCollection<Configuracao> configs = new XPCollection<Configuracao>(Session))
            {
                if (configs.Count > 0)
                {
                    Configuracao config = configs[0];
                    DtMaxima = DtTermino.AddMonths((int)config.NbDtMaxTirarFerias);
                }
            }
            
            base.OnSaving();
        }

        #endregion

        #region DBQueries
        
        #endregion

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão atual</param>
        public ColaboradorPeriodoAquisitivo(Session session)
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
        /// Depois de construir o objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        #endregion
    }
}