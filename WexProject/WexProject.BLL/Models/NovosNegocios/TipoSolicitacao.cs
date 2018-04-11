using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.NovosNegocios
{
    /// <summary>
    /// Classe de manipulação dos tipos de solicitação
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Novos Negocios > Básicos > Tipo de Solicitação de Orçamento")]
    [OptimisticLocking( false )]
    public class TipoSolicitacao : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private string txDescricao;

        #endregion

        #region Properties

        /// <summary>
        /// Descrição do tipo de solicitação
        /// </summary>
        [Size(100)]
        [Custom("Caption", "Descrição")]
        [RuleUniqueValue("TipoSolicitacao_TxDescricao__Unique", DefaultContexts.Save,
        "Já existe um tipo de solicitação com essa descrição!")]
        [RuleRequiredField("TipoSolicitacao_TxDescricao_Required", DefaultContexts.Save,
        Name = "Descrição")]
        public string TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                txDescricao = StrUtil.RetirarEspacoVazio(txDescricao);
                SetPropertyValue<string>("TxDescricao", ref txDescricao, value);
            }
        }

        #endregion

        #region NonPersistent Properties

        #endregion

        #region BusinessRules

        #endregion

        #region NewInstance

        #endregion

        #region DBQueries (Gets)

        #endregion

        #region Utils

        /// <summary>
        /// Transformando a classe em string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return TxDescricao;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public TipoSolicitacao(Session session)
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
