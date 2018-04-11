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
    /// Per�odo aquisitivo do colaborador
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Per�odo Aquisitivo")]
    [OptimisticLocking( false )]
    public class ColaboradorPeriodoAquisitivo : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Associa��o com colaborador
        /// </summary>
        private Colaborador colaborador;

        /// <summary>
        /// Data de in�cio do per�odo aquisitivo
        /// </summary>
        private DateTime dtInicio;

        /// <summary>
        /// Data de t�rmino do per�odo aquisitivo
        /// </summary>
        private DateTime dtTermino;

        /// <summary>
        /// Quantidade de dias de f�rias j� planejadas para o per�odo
        /// </summary>
        private uint nbFeriasPlanejadas;

        /// <summary>
        /// Texto de Planejamento de F�rias
        /// </summary>
        private string planejamentoFerias = null;

        /// <summary>
        /// Situa��o geral das f�rias
        /// </summary>
        private CsSituacaoFerias _csSituacaoFerias;

        /// <summary>
        /// Data m�xima que o colaborador pode tirar f�rias para este per�do aquisitivo
        /// </summary>
        private DateTime dtMaxima;

        #endregion

        #region Propriedades

        /// <summary>
        /// Associa��o com colaborador
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
        /// Data de in�cio do per�odo aquisitivo
        /// </summary>
        [Custom("Caption", "In�cio")]
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
        /// Data de t�rmino do per�odo aquisitivo
        /// </summary>
        [Custom("Caption", "T�rmino")]
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
        /// Quantidade de dias de f�rias j� planejadas para o per�odo
        /// </summary>
        [Custom("Caption", "F�rias Planejadas")]
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
        /// Planejamentos de f�rias
        /// </summary>
        [Custom("Caption", "Planejamento de F�rias")]
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
        /// Data m�xima que o colaborador pode tirar f�rias para este per�do aquisitivo
        /// </summary>
        [Browsable(false)]
        public DateTime DtMaxima
        {
            get { return dtMaxima; }
            set { SetPropertyValue<DateTime>("DtMaxima", ref dtMaxima, value); }
        }

        #endregion

        #region Propriedades N�o Persistentes

        /// <summary>
        /// Texto do Planejamento de F�rias
        /// </summary>
        [NonPersistent, Custom("Caption", "Planejamento de F�rias")]
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
        /// Situa��o geral das f�rias
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
        /// Formata��o do Pe�riodo Aquisitivo para o ToString()
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
        /// C�lculo do texto de planejamento de f�rias
        /// </summary>
        /// <param name="planCollection">Cole��o de Planejamentos</param>
        /// <param name="planejamento">Texto do Planejamento de F�rias</param>
        /// <param name="csSituacaoFerias">Situa��o de F�rias</param>
        /// <returns>Texto do Planejamento de F�rias</returns>
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
                            planejamento += String.Format( "{0} dia(s) (Venda das F�rias) - {1:dd/MM/yyyy} - {2:dd/MM/yyyy} ({3})\n",
                                plan.Modalidade.NbDias, plan.DtInicio, plan._DtRetorno, EnumUtil.DescricaoEnum( plan.CsSituacao ) );
                        }
                        else
                        {
                            planejamento += String.Format( "{0} dia(s) - {1:dd/MM/yyyy} - {2:dd/MM/yyyy} ({3})\n",
                                plan.Modalidade.NbDias, plan.DtInicio, plan._DtRetorno, EnumUtil.DescricaoEnum( plan.CsSituacao ) );
                        }

                        // Setando a situa��o das f�rias do colaborador (em geral)
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
        /// Valida��o da quantidade de F�rias Planejadas
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("ColaboradorPeriodoAquisitivo_ValidacaoQuantidadeFerias", DefaultContexts.Save,
            CustomMessageTemplate = @"A quantidade de F�rias Planejadas n�o pode ser maior que o definido na Configura��o.")]
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
        /// <returns>Data de In�cio concatenada com o T�rmino</returns>
        public override string ToString()
        {
            return _PeriodoAquisitivo;
        }

        /// <summary>
        /// Override do salving para quando salvar este per�do aquisitivo
        /// criar a data m�xima em que se pode tirar as f�rias
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
        /// <param name="session">Sess�o atual</param>
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