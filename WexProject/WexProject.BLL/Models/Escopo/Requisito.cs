using System;
using DevExpress.Xpo;
using System.Collections;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Qualidade;
using DevExpress.ExpressApp.ConditionalEditorState;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Escopo
{
    /// <summary>
    /// Classe dos Requisitos
    /// </summary>
    [DefaultClassOptions]
    [RuleIsReferenced("RuleIsReferenced_RequisitocasoTeste", DefaultContexts.Delete, typeof(CasoTeste), "Requisito", InvertResult = true,
    CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
    MessageTemplateMustBeReferenced = "")]
    [RuleCombinationOfPropertiesIsUnique("Requisito_Projeto_TxNome_Unique", DefaultContexts.Save, "TxNome, Modulo.Projeto", Name = "JaExisteUmaEstoriaNoModulo",
    CustomMessageTemplate = "Já existe Requisito com esse nome no Projeto!")]
    [Custom("Caption", "Projetos > Escopo > Requisitos")]
    [OptimisticLocking( false )]
    public class Requisito : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxID
        /// </summary>
        private String txID;

        /// <summary>
        /// Atributo de TxNome
        /// </summary>
        private String txNome;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txDescricao;

        /// <summary>
        /// Atributo de TxLinkPrototipo
        /// </summary>
        private String txLinkPrototipo;

        /// <summary>
        /// Atributo de Modulo
        /// </summary>
        private Modulo modulo;
        #endregion

        #region Properties

        /// <summary>
        /// Variável que guarda o ID do Requisito
        /// </summary>
        public String TxID
        {
            get
            {
                return txID;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxID", ref txID, value.Trim());
            }
        }

        /// <summary>
        /// Variável que guarda o nome do Requisito
        /// </summary>
        [RuleRequiredField("Requisito_TxNome_Required", DefaultContexts.Save, "Campo Nome Obrigatório")]
        public String TxNome
        {
            get
            {
                return txNome;
            }
            set
            {
                if (value != null)
                {
                    txNome = StrUtil.RetirarEspacoVazio(txNome);
                    SetPropertyValue<string>("TxNome", ref txNome, value.Trim());
                }
            }
        }

        /// <summary>
        /// Variável que guarda a Descrição do Requisito
        /// </summary>
        public String TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value != null)
                {

                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value.Trim());
                }
            }
        }

        /// <summary>
        /// Variável que contem o link do prototipo
        /// </summary>
        public String TxLinkPrototipo
        {
            get
            {
                return txLinkPrototipo;
            }
            set
            {
                if (value != null)
                {

                    SetPropertyValue<String>("TxLinkPrototipo", ref txLinkPrototipo, value.Trim());
                }
            }
        }

        /// <summary>
        /// Import do Modulo
        /// </summary>
        [Indexed]
        [RuleRequiredField("Requisito_Modulo_Required", DefaultContexts.Save, "Selecione um módulo primeiro")]
        public Modulo Modulo
        {
            get
            {
                return modulo;
            }
            set
            {
                bool alterado = modulo != value;
                SetPropertyValue<Modulo>("Modulo", ref modulo, value);
                if (alterado && !IsLoading && !IsDeleted)
                    RnCriarRequisitoID();
            }
        }

        /// <summary>
        /// Associação com caso de teste
        /// </summary>
        [Association("RequisitoCasosTeste", typeof(CasoTeste)), Aggregated]
        public XPCollection RequisitoCasosTeste
        {
            get
            {
                return GetCollection("RequisitoCasosTeste");
            }
        }
        #endregion

        #region NonPersistentProperties
        /// <summary>
        /// Variável que formata o campo requisito que é apresentado no listview do caso de teste
        /// </summary>
        [NonPersistent, Browsable(false)]
        public String _TxRequisito
        {
            get
            {
                return String.Format("{0} - {1}", TxID, TxNome);
            }
        }

        /// <summary>
        /// Campo que concatena o Id do requisito com o nome
        /// </summary>
        [NonPersistent]
        public String _TxRequisitoNome
        {
            get
            {
                return String.Format("{0} - {1}", TxID, TxNome);
            }
        }
        #endregion

        #region BusinessRules
        /// <summary>
        /// Ativa o RnCriarRequisitoID ao clicar no botão salvar
        /// </summary>
        protected override void OnSaving()
        {
            if (Oid.Equals(new Guid()))
            {
                RnCriarRequisitoID();
            }

            base.OnSaving();
        }

        /// <summary>
        /// Cria os IDs do requisito
        /// </summary>
        private void RnCriarRequisitoID()
        {
            if (IsDeleted)
                return;

            int crt, cont = 0;
            String id;

            crt = GetRequisitosPorModulo(Session, Modulo).Count;

            do
            {
                crt += 1;
                if (crt < 10)
                {
                    id = "RF_" + String.Format("{0}.{1}{2}", Modulo.TxID, cont, crt);
                }
                else
                {
                    id = "RF_" + String.Format("{0}.{1}", Modulo.TxID, crt);
                }

            }
            while
            (GetIDRequisitosPorID(Session, Modulo, id).Count != 0);

            TxID = id;

        }
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// Busca os valores de session e ID do requisito
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">modulo</param>
        /// <param name="txID">txID</param>
        /// <returns>TxID</returns>
        private static ICollection GetIDRequisitosPorID(Session session, Modulo modulo, String txID)
        {
            if (modulo != null)
            {
                return modulo.Session.GetObjects(modulo.Session.GetClassInfo<Requisito>(),
                CriteriaOperator.Parse(String.Format("Modulo.Oid = '{0}' AND TxID = '{1}'", modulo.Oid, txID)), null, 0, false, true);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Busca os valores de session e id do modulo no BD
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">modulo</param>
        /// <returns>return</returns>
        private static XPCollection GetRequisitosPorModulo(Session session, Modulo modulo)
        {
            return new XPCollection(session, typeof(Requisito),
            CriteriaOperator.Parse(String.Format("Modulo.Oid = '{0}'", modulo.Oid)));
        }
        #endregion

        #region Utils
        #endregion

        #region UserInterface
        /// <summary>
        /// Interface que esconde o campo TxID quando o usuário está cadastrando um novo Requisito
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>Hidden Requisito</returns>
        [EditorStateRule("HiddenRequisitoTxID", "TxID", ViewType.DetailView)]
        public EditorState HiddenTxID(out bool active)
        {
            active = Oid.Equals(new Guid());
            return EditorState.Hidden;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">Requisito</param>
        public Requisito(Session session)
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
