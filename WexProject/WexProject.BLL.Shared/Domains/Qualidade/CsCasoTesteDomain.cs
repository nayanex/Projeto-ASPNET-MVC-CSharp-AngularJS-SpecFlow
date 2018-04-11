using System;

namespace WexProject.BLL.Shared.Domains.Qualidade
{
    /// <summary>
    /// Enumerado com os possíveis tipos de caso de teste.
    /// </summary>
    public enum CsCasoTesteDomain
    {
        #region Attributes
        #endregion

        #region Properties
        /// <summary>
        /// Teste em que a operação é executada com sucesso.
        /// </summary>
        Sucesso,
        /// <summary>
        /// Testa se falhou
        /// </summary>
        Falha
    }
    #endregion
}
