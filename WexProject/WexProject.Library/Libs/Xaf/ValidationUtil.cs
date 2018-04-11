using System;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections;

namespace WexProject.Library.Libs.Xaf
{
    /// <summary>
    /// Classe utilitária com funções para auxiliar a chamada 
    /// de validações do BaseObject.
    /// </summary>
    public class ValidationUtil
    {
        /// <summary>
        /// Retorna verdeiro se a Rule passada como parâmetro 
        /// estiver válida.
        /// </summary>
        /// <param name="entity">Nome da Entidade</param>
        /// <param name="ruleName">Nome da Regra</param>
        /// <param name="contextIDs">Identificação do contexto onde a validação será aplicada</param>
        /// <returns>Verdadeiro se a regra estiver válida</returns>
        public static ValidationState GetRuleState(BaseObject entity, string ruleName, ContextIdentifiers contextIDs)
        {
            RuleSetValidationResult rsvr = (new RuleSet()).ValidateTarget(entity, contextIDs);
            return GetResultValidationItem(rsvr.Results, ruleName).State;
        }

        /// <summary>
        /// Retorna um resultado de uma coleção de validações.
        /// </summary>
        /// <param name="results">Lista de Resultados da Validação</param>
        /// <param name="ruleName">nome da regra que foi validada</param>
        /// <returns>Instância da Rule</returns>
        public static RuleSetValidationResultItem GetResultValidationItem(ICollection results, string ruleName)
        {
            foreach (RuleSetValidationResultItem item in results)
                if (item.RuleName == ruleName)
                    return item;

            return null;
        }

        /// <summary>
        /// Retorna verdeiro se a Rule passada como parâmetro 
        /// estiver válida.
        /// </summary>
        /// <param name="entity">Nome da Entidade</param>
        /// <param name="ruleID">ID da Regra</param>
        /// <param name="contextIDs">Identificação do contexto onde a validação será aplicada</param>
        /// <returns>Verdadeiro se a regra estiver válida</returns>
        public static ValidationState GetRuleStateID(BaseObject entity, string ruleID, ContextIdentifiers contextIDs)
        {
            RuleSetValidationResult rsvr = (new RuleSet()).ValidateTarget(entity, contextIDs);
            return GetResultValidationItemID(rsvr.Results, ruleID).State;
        }

        /// <summary>
        /// Retorna um resultado de uma coleção de validações.
        /// </summary>
        /// <param name="results">Lista de Resultados da Validação</param>
        /// <param name="ruleName">nome da regra que foi validada</param>
        /// <returns>Instância da Rule</returns>
        public static RuleSetValidationResultItem GetResultValidationItemID(ICollection results, string ruleID)
        {
            foreach (RuleSetValidationResultItem item in results)
                if (item.Rule.Id == ruleID)
                    return item;

            return null;
        }

    }
}
