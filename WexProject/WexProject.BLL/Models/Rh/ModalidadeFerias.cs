using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Modalidade de férias
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "RH > Modalidade de Férias")]
    [RuleIsReferenced("RuleIsReferenced_ModalidadeFerias", DefaultContexts.Delete, typeof(FeriasPlanejamento), "Modalidade", InvertResult = true,
        CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = "A Modalidade de Férias está sendo usada por um Planejamento de Férias.")]
    [OptimisticLocking( false )]
    public class ModalidadeFerias : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Atributo de NbDias
        /// </summary>
        private uint nbDias;

        /// <summary>
        /// Atributo de PodeVender
        /// </summary>
        private bool podeVender;

        /// <summary>
        /// Atributo de CsSituacao
        /// </summary>
        private CsSituacao csSituacao;

        #endregion

        #region Properties

        /// <summary>
        /// Número de dias
        /// </summary>
        [Custom("Caption", "Dias")]
        [ImmediatePostData]
        [RuleUniqueValue("ModalidadeFerias_NbDias_Unique", DefaultContexts.Save,
        "Já existe uma modalidade de férias com essa quantidade de dias!")]
        [RuleRequiredField("ModalidadeFerias_NbDias_Required", DefaultContexts.Save,
        Name = "Dias")]
        public uint NbDias
        {
            get
            {
                return nbDias;
            }
            set
            {
                SetPropertyValue<uint>("NbDias", ref nbDias, value);
            }
        }

        /// <summary>
        /// Indica se pode vender
        /// </summary>
        [Custom("Caption", "Pode Vender?")]
        [RuleRequiredField("ModalidadeFerias_PodeVender_Required", DefaultContexts.Save,
        Name = "Pode Vender?")]
        [VisibleInListView(false)]
        public bool PodeVender
        {
            get
            {
                return podeVender;
            }
            set
            {
                SetPropertyValue<bool>("PodeVender", ref podeVender, value);
            }
        }

        /// <summary>
        /// Situação da Modalidade de Férias
        /// </summary>
        [Custom("Caption", "Situação")]
        public CsSituacao CsSituacao
        {
            get
            {
                return csSituacao;
            }
            set
            {
                if (csSituacao == value)
                    return;

                SetPropertyValue<CsSituacao>("CsSituacao", ref csSituacao, value);
            }
        }

        #endregion

        #region NonPersistent Properties

        /// <summary>
        /// Descrição da modalidade
        /// </summary>
        /*[NonPersistent]
        [Custom("Caption", "Descrição")]
        [AppearanceAttribute("ModalidadeFerias_TxDescricao_Appearance",
        Enabled = false,
        TargetItems = "_TxDescricao")]
        public string _TxDescricao
        {
            get
            {
                return String.Format("{0} dia(s)", NbDias);
            }
        }*/

        /// <summary>
        /// Exibição textual do "Pode Vender?"
        /// </summary>
        [NonPersistent]
        [Custom("Caption", "Pode Vender?")]
        [VisibleInDetailView(false)]
        public string _TxPodeVender
        {
            get
            {
                if(PodeVender)
                    return "Sim";

                return "Não";
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Validação da quantidade de Dias (para a quantidade máxima de Férias na Configuração)
        /// </summary>
        [RuleFromBoolProperty("ModalidadeFerias_RnVerificarDiasMaxFerias", DefaultContexts.Save,
            "A quantidade de dias é maior que o máximo de dias definido para as Férias na Configuração.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarDiasMaxFerias
        {
            get
            {
                Configuracao config = Configuracao.GetInstancia(Session);

                if (config == null || config.NbQtdeMaxFerias < NbDias)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Validação da quantidade de Dias (para a quantidade máxima de venda de Férias na Configuração)
        /// </summary>
        [RuleFromBoolProperty("ModalidadeFerias_RnVerificarDiasMaxVendaFerias", DefaultContexts.Save,
            "A quantidade de dias é maior que o máximo de dias definido para a Venda de Férias na Configuração.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarDiasMaxVendaFerias
        {
            get
            {
                if (!PodeVender)
                {
                    return true;
                }

                if (NbDias < 1) 
                {
                    return false;
                }

                Configuracao config = Configuracao.GetInstancia(Session);

                if (config == null || config.NbQtdeMaxVenda < NbDias)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// Obtenção da Modalidade de Férias com maior quantidade de dias
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="onlyPodeVender">Considerar apenas as Modalidades que podem vender dias?</param>
        /// <returns>A Modalidade de Férias com maior quantidade de dias</returns>
        public static ModalidadeFerias GetModalidadeFeriasAtivaMaiorDia(Session session, bool onlyPodeVender = false)
        {
            XPCollection<ModalidadeFerias> modalidades;

            if (onlyPodeVender)
            {
                modalidades = new XPCollection<ModalidadeFerias>(session,
                    CriteriaOperator.Parse("CsSituacao = ? AND PodeVender = ?", CsSituacao.Ativo, true));
            }
            else
            {
                modalidades = new XPCollection<ModalidadeFerias>(session,
                    CriteriaOperator.Parse("CsSituacao = ?", CsSituacao.Ativo));
            }

            if (modalidades.Count == 0)
                return null;

            modalidades.Sorting.Add(new SortProperty("NbDias", SortingDirection.Descending));

            return modalidades[0];
        }

        #endregion

        #region Utils

        /// <summary>
        /// Método chamado ao transformar o objeto em string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return NbDias.ToString();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public ModalidadeFerias(Session session)
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
