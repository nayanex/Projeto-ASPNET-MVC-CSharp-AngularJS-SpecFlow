using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.Library.Libs.Xaf;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Classe referente aos Tipos de Afastamento
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Tipo de Afastamento")]
    [RuleIsReferenced("RuleIsReferenced_ColaboradorAfastamentoTipoAfastamento", DefaultContexts.Delete, typeof(ColaboradorAfastamento),
        "TipoAfastamento", InvertResult = true,
        CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = "O Tipo de Afastamento está sendo usado por um Afastamento de Colaborador.")]
    [OptimisticLocking( false )]
    public class TipoAfastamento : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Descrição do Tipo de Afastamento
        /// </summary>
        private string txDescricao;

        /// <summary>
        /// Indica se é para férias realizadas
        /// </summary>
        private bool isParaFeriasRealizadas;

        /// <summary>
        /// Indica se é um Tipo de Afastamento remunerado
        /// </summary>
        private bool isRemunerado;

        /// <summary>
        /// Situação do Tipo de Afastamento
        /// </summary>
        private CsSituacao csSituacao;

        #endregion

        #region Propriedades

        /// <summary>
        /// Descrição do Tipo de Afastamento
        /// </summary>
        [Custom("Caption", "Descrição")]
        [RuleRequiredField("TipoAfastamento_TxDescricao_Required", DefaultContexts.Save,
            Name = "Descrição", CustomMessageTemplate = "Informe a descrição")]
        [RuleUniqueValue("TipoAfastamento_TxDescricao_Unique", DefaultContexts.Save,
            "Já existe um Tipo de Afastamento com essa descrição.")]
        [Size(200)]
        public string TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (txDescricao == value)
                    return;

                SetPropertyValue<string>("TxDescricao", ref txDescricao, value);
            }
        }

        /// <summary>
        /// Indica se é para férias realizadas
        /// </summary>
        [VisibleInListView(false)]
        [Custom("Caption", "Para férias realizadas?")]
        [ImmediatePostData]
        public bool IsParaFeriasRealizadas
        {
            get
            {
                return isParaFeriasRealizadas;
            }
            set
            {
                if (value)
                {
                    IsRemunerado = true;
                }

                if (isParaFeriasRealizadas == value)
                    return;

                SetPropertyValue<bool>("IsParaFeriasRealizadas", ref isParaFeriasRealizadas, value);
            }
        }

        /// <summary>
        /// Indica se é um Tipo de Afastamento remunerado
        /// </summary>
        [Custom("Caption", "Remunerado?")]
        [ImmediatePostData]
        [AppearanceAttribute("TipoAfastamento_IsRemunerado_AppearanceAttribute",
            Criteria = "IsParaFeriasRealizadas = true",
            Enabled = false,
            TargetItems = "IsRemunerado")]
        public bool IsRemunerado
        {
            get
            {
                return isRemunerado;
            }
            set
            {
                if (isRemunerado == value)
                    return;

                SetPropertyValue<bool>("IsRemunerado", ref isRemunerado, value);
            }
        }

        /// <summary>
        /// Situação do Tipo de Afastamento
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

        #region Propriedades Não Persistentes

        /// <summary>
        /// Propriedade textual de "Para férias realizadas?"
        /// </summary>
        [NonPersistent]
        [VisibleInDetailView(false)]
        [Custom("Caption", "Para férias realizadas?")]
        public string _IsParaFeriasRealizadas
        {
            get
            {
                if (IsParaFeriasRealizadas)
                    return "Sim";

                return "Não";
            }
        }

        /// <summary>
        /// Propriedade textual de "Remunerado?"
        /// </summary>
        [NonPersistent]
        [VisibleInDetailView(false)]
        [Custom("Caption", "Remunerado?")]
        public string _IsRemunerado
        {
            get
            {
                if (IsRemunerado)
                    return "Sim";

                return "Não";
            }
        }

        /// <summary>
        /// Tipo de Afastamento para Férias Realizadas
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public TipoAfastamento _TipoFeriasRealizadas
        {
            get;
            set;
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Override do OnSaving
        /// </summary>
        protected override void OnSaving()
        {
            if (_TipoFeriasRealizadas != null && IsParaFeriasRealizadas && CsSituacao == CsSituacao.Ativo)
            {
                _TipoFeriasRealizadas.IsParaFeriasRealizadas = false;
                _TipoFeriasRealizadas.Save();
            }

            base.OnSaving();
        }

        /// <summary>
        /// Verificação (para os tipos de afastamentos de férias realizadas) se já não existe outro
        /// tipo de afastamento de férias realizadas cadastrado (só pode ter um)
        /// </summary>
        /// <returns>Se foi encontrado algum tipo</returns>
        public bool RnVerificarExistenciaOutroTipoAfastamentoParaFeriasRealizadas()
        {
            _TipoFeriasRealizadas = null;

            if (IsParaFeriasRealizadas && CsSituacao == CsSituacao.Ativo)
            {
                using (XPCollection<TipoAfastamento> tipos = new XPCollection<TipoAfastamento>(Session,
                    CriteriaOperator.Parse("Oid != ? AND CsSituacao = ? AND IsParaFeriasRealizadas = true",
                    Oid, CsSituacao.Ativo)))
                {
                    if (tipos.Count > 0)
                    {
                        _TipoFeriasRealizadas = tipos[0];
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Gets

        /// <summary>
        /// Obtenção do Tipo de Afastamento para Férias Realizadas
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <returns>Objeto de TipoAfastamento</returns>
        public static TipoAfastamento GetTipoAfastamentoParaFeriasRealizadas(Session session)
        {
            XPCollection<TipoAfastamento> tipos = new XPCollection<TipoAfastamento>(session,
                    CriteriaOperator.Parse("CsSituacao = ? AND IsParaFeriasRealizadas = true", CsSituacao.Ativo));

            if (tipos.Count > 0)
            {
                return tipos[0];
            }
            else
            {
                // Novo Tipo
                TipoAfastamento novo = new TipoAfastamento(session)
                {
                    TxDescricao = "Férias",
                    IsParaFeriasRealizadas = true,
                    IsRemunerado = true,
                    CsSituacao = CsSituacao.Ativo
                };

                novo.GerarDescricaoTipoAfastamento();
                novo.Save();

                return novo;
            }
        }

        #endregion

        #region Construtores

        /// <summary>
        /// Método que apresenta amigavelmente a informação da classe
        /// </summary>
        /// <returns>Descrição do Afastamento</returns>
        public override string ToString()
        {
            return TxDescricao;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão atual</param>
        public TipoAfastamento(Session session)
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
        /// Depois de construir um objeto (novo)
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        #endregion

        #region Utils

        /// <summary>
        /// Gerador de descrição
        /// </summary>
        private void GerarDescricaoTipoAfastamento()
        {
            int valornum = 2;
            string valorbase = TxDescricao;

            ValidationState state = ValidationUtil.GetRuleState(this,
                "TipoAfastamento_TxDescricao_Unique", DefaultContexts.Save);

            while (state == ValidationState.Invalid)
            {
                TxDescricao = string.Format("{0} {1}", valorbase, valornum);
                valornum++;

                state = ValidationUtil.GetRuleState(this,
                    "TipoAfastamento_TxDescricao_Unique", DefaultContexts.Save);
    }
        }

        #endregion
}

}
