using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Afastamento do colaborador
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Afastamento")]
    [AppearanceAttribute("Afastamento_Appearance",
        Criteria = "IsCriadoAutomaticamente = true",
        Enabled = false,
        TargetItems = "DtInicio, DtTermino"
        )]
    [OptimisticLocking( false )]
    public class ColaboradorAfastamento : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Associa��o com colaborador
        /// </summary>
        private Colaborador colaborador;

        /// <summary>
        /// Data de in�cio
        /// </summary>
        private DateTime dtInicio;

        /// <summary>
        /// Data de t�rmino
        /// </summary>
        private DateTime dtTermino;

        /// <summary>
        /// Tipo do afastamento
        /// </summary>
        private TipoAfastamento tipoAfastamento;

        /// <summary>
        /// Observa��o
        /// </summary>
        private string txObservacao;

        /// <summary>
        /// Objeto antigo
        /// </summary>
        private ColaboradorAfastamento afastamentoOld;

        /// <summary>
        /// Criado automaticamenre
        /// </summary>
        private bool isCriadoAutomaticamente;

        #endregion

        #region Propriedades

        /// <summary>
        /// Associa��o com colaborador
        /// </summary>
        [Browsable(false)]
        [Association("Colaborador_ColaboradorAfastamento", typeof(Colaborador))]
        [RuleRequiredField("ColaboradorAfastamento_Colaborador_Required", DefaultContexts.Save)]
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
        /// Data de in�cio
        /// </summary>
        [Custom("Caption", "In�cio")]
        [RuleRequiredField("ColaboradorAfastamento_DtInicio_Required", DefaultContexts.Save,
            Name = "In�cio", CustomMessageTemplate = "Informe o In�cio")]
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
        /// Data de t�rmino
        /// </summary>
        [Custom("Caption", "T�rmino")]
        [RuleRequiredField("ColaboradorAfastamento_DtTermino_Required", DefaultContexts.Save,
            Name = "T�rmino", CustomMessageTemplate = "Informe o T�rmino")]
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
        /// Tipo do afastamento
        /// </summary>
        [Custom("Caption", "Tipo")]
        [RuleRequiredField("ColaboradorAfastamento_TipoAfastamento_Required", DefaultContexts.Save,
            Name = "Tipo", CustomMessageTemplate = "Informe a Tipo")]
        public TipoAfastamento TipoAfastamento
        {
            get
            {
                return tipoAfastamento;
            }
            set
            {
                if (tipoAfastamento == value)
                    return;

                SetPropertyValue<TipoAfastamento>("TipoAfastamento", ref tipoAfastamento, value);
            }
        }

        /// <summary>
        /// Observa��o
        /// </summary>
        [Custom("Caption", "Observa��o")]
        [Size(1000)]
        public string TxObservacao
        {
            get
            {
                return txObservacao;
            }
            set
            {
                if (txObservacao == value)
                    return;

                SetPropertyValue<string>("TxObservacao", ref txObservacao, value);
            }
        }

        /// <summary>
        /// Criado automaticamenre
        /// </summary>
        [Custom("Caption", "Criado automaticamente")]
        [Browsable(false)]
        public bool IsCriadoAutomaticamente
        {
            get
            {
                return isCriadoAutomaticamente;
            }
            set
            {
                if (isCriadoAutomaticamente == value)
                    return;

                SetPropertyValue<bool>("IsCriadoAutomaticamente", ref isCriadoAutomaticamente, value);
            }
        }


        #endregion

        #region BusinessRules

        /// <summary>
        /// Regra de neg�cio que valida as datas
        /// </summary>
        [RuleFromBoolProperty("NaoSalvarDataTerminoMenorInicio", DefaultContexts.Save, InvertResult = false,
            CustomMessageTemplate = "N�o � permitido salvar uma data de t�rmino menor ou igual a data de in�cio.")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarDatas
        {
            get
            {
                if (DtTermino != DateTime.MinValue && DtTermino.CompareTo(DtInicio) <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Quando tiver salvando o objeto
        /// </summary>
        protected override void OnSaving()
        {
            if (IsAjustarPeriodosAquisitivosColaborador())
            {
                Colaborador.CriarPeriodosAquisitivos();
                Colaborador.Save();
            }

            base.OnSaving();
        }

        /// <summary>
        /// Quando o objeto for apagado
        /// </summary>
        protected override void OnDeleted()
        {
            base.OnDeleted();

            if (afastamentoOld != null && afastamentoOld.TipoAfastamento != null &&
                !afastamentoOld.TipoAfastamento.IsRemunerado)
            {
                afastamentoOld.Colaborador.CriarPeriodosAquisitivos();
                afastamentoOld.Colaborador.Save();
            }
        }

        /// <summary>
        /// Diz se � para ajustar os per�odos aquisitivos do Colaborador
        /// </summary>
        /// <returns>True, se for para ajustar e false se n�o for</returns>
        private bool IsAjustarPeriodosAquisitivosColaborador()
        {
            return (Oid.Equals(new Guid()) && TipoAfastamento != null && !TipoAfastamento.IsRemunerado) ||
                (afastamentoOld != null && afastamentoOld.TipoAfastamento != null && TipoAfastamento != null &&
                afastamentoOld.TipoAfastamento.IsRemunerado != TipoAfastamento.IsRemunerado);
        }

        /// <summary>
        /// Valida��o do Per�odo do Afastamento
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("ColaboradorAfastamento_ValidacaoPeriodo", DefaultContexts.Save,
            CustomMessageTemplate = @"O per�odo do Afastamento n�o pode conflitar com nenhum outro j� existente para o mesmo Colaborador.")]
        public bool _ValidacaoPeriodo
        {
            get
            {
                int qtde;
                
                Colaborador.Afastamentos.Filter = CriteriaOperator.Parse("Oid != ? AND ((DtInicio >= ? AND DtTermino >= ? AND DtInicio <= ?) " +
                    "OR (DtInicio <= ? AND DtTermino <= ? AND DtTermino >= ?) OR (DtInicio <= ? AND DtTermino >= ?))",
                    Oid, DtInicio.Date, DtTermino.Date, DtTermino.Date, DtInicio.Date, DtTermino.Date, DtInicio.Date, DtInicio.Date, DtTermino.Date);

                qtde = Colaborador.Afastamentos.Count;

                Colaborador.Afastamentos.Filter = null;

                if (qtde > 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Valida��o do Per�odo do Afastamento com os Planejamentos de F�rias do Colaborador
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("ColaboradorAfastamento_ValidacaoPeriodoPlanejamento", DefaultContexts.Save,
            CustomMessageTemplate = @"O per�odo do Afastamento n�o pode conflitar com nenhum Planejamento de F�rias do Colaborador.")]
        public bool _ValidacaoPeriodoPlanejamento
        {
            get
            {
                if(TipoAfastamento != null && TipoAfastamento.IsParaFeriasRealizadas)
                {
                    return true;
                }

                CriteriaOperator criteria = CriteriaOperator.Parse("(DtInicio >= ? AND _DtRetorno >= ? AND DtInicio <= ?) " +
                    "OR (DtInicio <= ? AND _DtRetorno <= ? AND _DtRetorno >= ?) OR (DtInicio <= ? AND _DtRetorno >= ?)",
                    DtInicio.Date, DtTermino.Date, DtTermino.Date, DtInicio.Date, DtTermino.Date, DtInicio.Date, DtInicio.Date, DtTermino.Date);

                foreach(ColaboradorPeriodoAquisitivo periodo in Colaborador.PeriodosAquisitivos)
                {
                    int qtde = 0;

                    periodo.Planejamentos.Filter = criteria;
                    qtde = periodo.Planejamentos.Count;
                    periodo.Planejamentos.Filter = null;

                    if (qtde > 0)
                    {
                        return false;
                    }
                }
                
                return true;
            }
        }

        /// <summary>
        /// Set da vari�vel afastamentoOld para null
        /// </summary>
        public void SetAfastamentoOldToNull()
        {
            afastamentoOld = null;
        }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sess�o</param>
        public ColaboradorAfastamento(Session session)
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

            IsCriadoAutomaticamente = false;
        }

        /// <summary>
        /// Quando o objeto estiver carregado
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (afastamentoOld == null)
            {
                afastamentoOld = MemberwiseClone() as ColaboradorAfastamento;
            }
        }

        #endregion
    }
}