using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.Models.Escopo;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp;
using DevExpress.Xpo.DB;
using System.Collections;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.BLL.Shared.Domains.Qualidade;
using WexProject.BLL.DAOs;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.BLL.Models.Qualidade
{
    /// <summary>
    /// Classe para Casos de Teste
    /// </summary>
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("CasoTeste_Projeto_TxID_Unique", DefaultContexts.Save, "TxID, Requisito.Modulo.Projeto", Name = "JaExisteIDRequisito",
    CustomMessageTemplate = "Já existe ID nesse Projeto!")]
    [RuleCombinationOfPropertiesIsUnique("CasoTeste_Requisito_TxSumario_Unique", DefaultContexts.Save, "TxSumario, Requisito", Name = "JaExisteSumarioRequisito",
    CustomMessageTemplate = "Já existe um Caso de Teste com esse sumário no Projeto")]
    [DeferredDeletion(true)]
    [OptimisticLocking( false )]
    public class CasoTeste : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxID
        /// </summary>
        private String txID;

        /// <summary>
        /// Atributo de Requisito
        /// </summary>
        private Requisito requisito;

        /// <summary>
        /// Atributo de TxSumario
        /// </summary>
        private String txSumario;

        /// <summary>
        /// Atributo de CsCasTeste
        /// </summary>
        private CsCasoTesteDomain csCasoTeste;

        /// <summary>
        /// Atributo de NbMaiorPrecondicao
        /// </summary>
        private UInt16 nbMaiorPrecondicao;

        /// <summary>
        /// Atributo de NbMaiorPasso
        /// </summary>
        private UInt16 nbMaiorPasso;

        /// <summary>
        /// Atributo de Usuario
        /// </summary>
        private User usuario;

        /// <summary>
        /// Atributo de DtHoraEData
        /// </summary>
        private DateTime dtHoraEData;

        /// <summary>
        /// Atributo de Projeto
        /// </summary>
        private Projeto projetoSelecionado;

        /// <summary>
        /// Variável que verifica se o requisito está sendo setado através da tela de requisito
        /// </summary>
        [NonPersistent, Browsable(false)]
        private bool csVerificandoNestedObject;

        /// <summary>
        /// Variável que recebe o valor do ID
        /// </summary>
        private String id;
        #endregion

        #region Properties
        /// <summary>
        /// propriedade de csVerificandoNestedObject, não pode ser excluida pois é usado em outra parte do sistema 
        /// </summary>
        [Browsable(false)]
        public bool CsVerificandoNestedObject
        {
            get
            {
                return csVerificandoNestedObject;
            }
            set
            {
                SetPropertyValue<bool>("CsVerificandoNestedObject", ref csVerificandoNestedObject, value);
            }
        }
        /// <summary>
        /// Variável que guarda o ID do Caso de Teste
        /// </summary>
        [Size(100)]
        public String TxID
        {
            get
            {
                return txID;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxID", ref txID, value.Trim());
            }
        }

        /// <summary>
        /// Import do Requisito
        /// </summary>
        //Visibility = ViewItemVisibility.Hide,
        [Indexed]
        [AppearanceAttribute("CasoTeste_Requisito_Appearance",
            Enabled = false,
            Criteria = "csVerificandoNestedObject = True",
            TargetItems = "Requisito"
            )]
        [Association("RequisitoCasosTeste", typeof(Requisito))]
        public Requisito Requisito
        {
            get
            {
                return requisito;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<Requisito>("Requisito", ref requisito, value);
            }
        }
        /// <summary>
        /// Variável do Sumário
        /// </summary>
        [RuleRequiredField("CasoTeste_TxSumario_Required", DefaultContexts.Save, "Digite uma descrição para o sumário")]
        [Size(255)]
        public String TxSumario
        {
            get
            {
                return txSumario;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxSumario", ref txSumario, value.Trim());
            }
        }

        /// <summary>
        /// Import CsCasoTeste
        /// </summary>
        public CsCasoTesteDomain CsCasoTeste
        {
            get
            {
                return csCasoTeste;
            }
            set
            {
                SetPropertyValue<CsCasoTesteDomain>("CsCasoTeste", ref csCasoTeste, value);
            }
        }

        /// <summary>
        /// Associação com EstoriasCasoTeste
        /// </summary>
        [Association("EstoriasCasoTeste", typeof(EstoriaCasoTeste)), Aggregated]
        public XPCollection EstoriasCasoTeste
        {
            get
            {
                return GetCollection("EstoriasCasoTeste");
            }
        }
        /// <summary>
        /// Associação com ResultadoCasoEsperado
        /// </summary>
        [Association("CasoTestePasso", typeof(CasoTestePasso)), Aggregated]
        public XPCollection<CasoTestePasso> Passos
        {
            get
            {
                return GetCollection<CasoTestePasso>("Passos");
            }
        }

        /// <summary>
        /// Associação com PreCondicoes
        /// </summary>
        [Association("CasoTestePreCondicoes", typeof(CasoTestePreCondicao))]
        [Aggregated]
        public XPCollection PreCondicoes
        {
            get
            {
                return GetCollection("PreCondicoes");
            }
        }

        /// <summary>
        /// Variável que guarda o valor da maior pré-condição
        /// </summary>
        [Browsable(false)]
        public UInt16 NbMaiorPrecondicao
        {
            get
            {
                return nbMaiorPrecondicao;
            }
            set
            {
                SetPropertyValue<UInt16>("NbMaiorPrecondicao", ref nbMaiorPrecondicao, value);
            }
        }

        /// <summary>
        /// Variável que guarda o maior valor de um passo
        /// </summary>
        [Browsable(false)]
        public UInt16 NbMaiorPasso
        {
            get
            {
                return nbMaiorPasso;
            }
            set
            {
                SetPropertyValue<UInt16>("NbMaiorPasso", ref nbMaiorPasso, value);
            }
        }

        /// <summary>
        /// Variavel que guarda o usuario
        /// </summary>
        [Browsable(false)]
        public User Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                SetPropertyValue<User>("User", ref usuario, value);

            }
        }

        /// <summary>
        /// Variável que guarda o valor da dataHora e da hora
        /// </summary>
        [Browsable(false)]
        public DateTime DtHoraEData
        {
            get
            {
                return dtHoraEData;
            }
            set
            {
                SetPropertyValue<DateTime>("DtHoraEData", ref dtHoraEData, value);
            }
        }
        #endregion

        #region NonPersistent
        /// <summary>
        /// Classe que retorna o caso de teste ordenado com o sumários
        /// </summary>
        [NonPersistent]
        public String _TxSumario
        {
            get
            {
                return String.Format("{0} ({1})", TxSumario, CsCasoTeste);
            }
        }

        /// <summary>
        /// Variável que guarda o id antigo do requisito
        /// </summary>
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public Requisito _RequisitoOld
        {
            get;
            set;
        }

        /// <summary>
        /// Variável não persistente que retorna o nome do Módulo
        /// </summary>
        [NonPersistent]
        public String _TxRequisitoModulo
        {
            get
            {
                String nomeDoModulo;

                if (Requisito != null)
                    return nomeDoModulo = Requisito.Modulo.TxNome;
                else
                    return "";
            }
        }
        #endregion

        #region BusinessRules

        /// <summary>
        /// Método que executa a função RnCriarCasoTesteID ao clicar no botão salvar
        /// </summary>
        protected override void OnSaving()
        {

            if (Oid.Equals(new Guid()) || _RequisitoOld != Requisito) /* && id == "1"*/
                RnCriarCasoTesteID();

            DtHoraEData = DateTime.Now;
            base.OnSaving();
        }

        /// <summary>
        /// Função que cria os ID's para os casos de teste
        /// </summary>
        private void RnCriarCasoTesteID()
        {
            if (IsDeleted)
                return;

            int crt, cont = 0;
            crt = Requisito.RequisitoCasosTeste.Count;
            crt -= 1;

            do
            {
                crt += 1;

                if (crt < 10)
                    id = String.Format("{0}.{1}{2}", Requisito.TxID, cont, crt).Replace("RF_", "CT_");
                else
                    id = String.Format("{0}.{1}", Requisito.TxID, crt).Replace("RF_", "CT_");

            }
            while (GetCasoTestePorID(Session, Requisito, id).Count != 0);

            TxID = id;

        }

        /// <summary>
        /// metodo que não deixa excluir um caso de teste se o mesmo estiver na estoria.
        /// </summary>
        [RuleFromBoolProperty("EstoriaCasoTeste2Association", DefaultContexts.Delete, InvertResult = true,
        CustomMessageTemplate = "O Caso de Teste está associado a uma Estória e não pode ser excluído.")]
        [NonPersistent, VisibleInDetailView(false), VisibleInListView(false)]
        public bool RnValidarAssociacaoCasoTesteEstoria
        {
            get
            {
                if (EstoriasCasoTeste.Count > 0 )
                {
                    EstoriasCasoTeste.Filter = "";
                    return true;
                }
                else
                {
                    EstoriasCasoTeste.Filter = "";
                    return false;
                }
            }
        }

        /// <summary>
        /// Regra de negócio que retorna o ultimo requisito selecionado pelo usuário
        /// </summary>
        public void RnSalvarUltimoRequisitoSelecionado()
        {
            CasoTeste casoteste1 = GetTest(Session, Usuario);

            if (casoteste1 != null)
                Requisito = casoteste1.Requisito;
        }

        /// <summary>
        /// Regra de negócio que valida se um caso de teste existe
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTeste", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Selecione um requisito")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarRequisito
        {
            get
            {
                return Requisito != null;
            }
        }

        /// <summary>
        /// Regra de negócio que recebe o projeto selecionado e executa a RN do ultimo requisitos
        /// </summary>
        /// <param name="projeto">projeto</param>
        public void RnSelecionarProjeto(Projeto projeto)
        {
            projetoSelecionado = projeto;
            if (projeto != null)
                RnSalvarUltimoRequisitoSelecionado();
        }

        /// <summary>
        /// Regra de negócio que valida se um um requisito é novo
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoNovo", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Salve primeiro o requisito")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarRequisitoNovo
        {
            get
            {
                if (Requisito != null)
                    return !Requisito.Oid.Equals(new Guid());
                else
                    return false;
            }
        }
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        /// <summary>
        /// Busca no BD os casos de teste por requisito
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="requisito">requisito</param>
        /// <returns>return</returns>
        private static XPCollection GetCasoTestePorRequisito(Session session, Requisito requisito)
        {

            if (requisito != null)
            {
                return new XPCollection(session, typeof(CasoTeste),
                CriteriaOperator.Parse(String.Format("Requisito.Oid = '{0}'", requisito.Oid)));
            }
            else
            {
                return new XPCollection();
            }


        }

        /// <summary>
        /// Busca no BD os casos de teste por ID
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="requisito">Requisito</param>
        /// <param name="txID">txID</param>
        /// <returns>returns</returns>
        private static ICollection GetCasoTestePorID(Session session, Requisito requisito, String txID)
        {

            if (requisito != null)
            {
                return requisito.Session.GetObjects(requisito.Session.GetClassInfo<CasoTeste>(),
                CriteriaOperator.Parse(String.Format("Requisito.Oid = '{0}' AND TxID = '{1}'", requisito.Oid, txID)), null, 0, false, true);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Get que busca qual foi o ultimo requisito selecionado pelo usuario
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="usuario">Usuario</param>
        /// <returns>Último caso de teste cadastrado</returns>
        private static CasoTeste GetTest(Session session, User usuario)
        {
            ICollection collection;
            CasoTeste result = null;
            SortingCollection sortCollection = new SortingCollection();
            sortCollection.Add(new SortProperty("DtHoraEData", SortingDirection.Descending));

            collection = session.GetObjects(session.GetClassInfo<CasoTeste>(),
            CriteriaOperator.Parse("Usuario.Oid = ? And Requisito.Modulo.Projeto.Oid = ?", usuario.Oid, Projeto.SelectedProject), sortCollection, 1, false, true);
            foreach (CasoTeste ct in collection)
            {
                result = ct;
                break;
            }

            return result;
        }
        #endregion

        #region Utils
        /// <summary>
        /// Concatena o TxID com o sumário do caso de teste
        /// </summary>
        /// <returns>TxID - Sumário</returns>
        public override string ToString()
        {
            return String.Format("{0}-{1}", TxID, TxSumario);
        }
        #endregion

        #region UserInterface
        /// <summary>
        /// Interface que verifica se o caso de teste é novo ou está sendo editado
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>Se for um caso de teste novo, ele esconde o campo de ID</returns>
        [EditorStateRule("HiddenCasoTesteTxID", "TxID", ViewType.DetailView)]
        public EditorState HiddenTxID(out bool active)
        {
            active = Oid.Equals(new Guid());
            return EditorState.Hidden;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Método que substutui o valor antido do requisito pelo valor novo
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            _RequisitoOld = Requisito;
        }
        /// <summary>
        /// Construtores da Classe
        /// </summary>
        /// <param name="session"> CasoTeste</param>
        public CasoTeste(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Atribui valores de texto a varivável sumário e resultadoesperado
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TxSumario = "Testar <ação do usuário> <submetido a um determinado cenário de Sucesso ou Falha>";
            id = "1";
            NbMaiorPrecondicao = 0;
            NbMaiorPasso = 0;
            Usuario = UsuarioDAO.GetUsuarioLogado(Session);
            if (Projeto.SelectedProject != new Guid())
                RnSalvarUltimoRequisitoSelecionado();

        }
        #endregion
    }

}
