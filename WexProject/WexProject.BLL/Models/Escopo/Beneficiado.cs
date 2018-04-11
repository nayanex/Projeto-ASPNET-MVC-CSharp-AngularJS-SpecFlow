using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Str;


namespace WexProject.BLL.Models.Escopo
{
    /// <summary>
    /// Classe Beneficiado
    /// </summary>
    [Custom("Caption", "Projetos > Escopo > Básicos > Beneficiados")]
    [DefaultClassOptions]
    [OptimisticLocking(false)]
    public class Beneficiado : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txDescricao;
        #endregion

        #region Properties

        /// <summary>
        /// Variavel que guarda o nome do papel
        /// </summary>
        [RuleUniqueValue("Beneficiado_TxDescricao_Unique", DefaultContexts.Save, "Já Existe Beneficiado com esse nome!")]
        [RuleRequiredField("Beneficiado_TxDescricao_Required", DefaultContexts.Save, "Campo Nome Obrigatório!")]
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
                    txDescricao = StrUtil.RetirarEspacoVazio(txDescricao);
                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value.Trim());
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
        public Beneficiado(Session session)
            : base(session)
        {

        }
        #endregion
    }

}
