using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe de Papel
    /// </summary>
    [DefaultClassOptions]
    [RuleIsReferenced("RuleIsReferenced_PapelParteInteressada", DefaultContexts.Delete, typeof(ProjetoParteInteressada), "ParteInteressadaPapel", InvertResult = true,
    CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
    MessageTemplateMustBeReferenced = "O Papel está sendo referenciado por uma Projeto e Parte Interessa!")]
    [OptimisticLocking( false )]
    public class Papel : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxNome
        /// </summary>
        private String txNome;
        #endregion
        #region Properties

        /// <summary>
        /// Variavel que guarda o nome do papel
        /// </summary>
        [RuleUniqueValue("Papel_TxNome_Unique", DefaultContexts.Save, "Já Existe Papel com esse nome!")]
        [RuleRequiredField("Papel_TxNome_Required", DefaultContexts.Save, "Campo Nome Obrigatório!")]
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
                    SetPropertyValue<String>("TxNome", ref txNome, value.Trim());
                }
            }
        }
        #endregion

        #region NonPersistentProperties
        #endregion

        #region BusinessRules
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        #endregion

        #region Utils
        #endregion

        #region Constructors
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public Papel(Session session)
            : base(session)
        {

        }
        #endregion
    }

}
