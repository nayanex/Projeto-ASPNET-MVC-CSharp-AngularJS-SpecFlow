using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Str;
using WexProject.BLL.Models.Escopo;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe feita para o cargo dos solicitantes
    /// </summary>
    [DefaultClassOptions]
    [RuleIsReferenced("RuleIsReferenced_CargoParteInteressada", DefaultContexts.Delete, typeof(ParteInteressada), "Cargo", InvertResult = true,
    CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
    MessageTemplateMustBeReferenced = "O Cargo está sendo referenciado por uma Parte Interessada!.")]
    [OptimisticLocking( false )]
    public class Cargo : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txDescricao;
        #endregion

        #region Properties        
        /// <summary>
        /// Variável que armazena o nome do cargo
        /// </summary>
        [RuleRequiredField("Cargo_TxDescricao_Required", DefaultContexts.Save, "Campo Descrição Obrigatório!")]
        [RuleUniqueValue("Cargo_TxDescricao_Unique", DefaultContexts.Save, "Já existe um Cargo com esse nome!")]
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
        /// <summary>
        /// Propiedade para looukup para retornar a descricao do cargo
        /// </summary>
        /// <returns>Retorna a descrição do cargo</returns>
        public override string ToString()
        {
            return TxDescricao;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construtores da classe
        /// </summary>
        /// <param name="session">Cargo</param>
        public Cargo(Session session)
            : base(session)
        {
        }

        #endregion
    }

}
