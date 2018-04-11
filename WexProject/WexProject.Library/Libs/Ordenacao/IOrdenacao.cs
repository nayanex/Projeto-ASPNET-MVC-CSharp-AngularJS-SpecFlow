using System;
using System.Collections.Generic;

namespace WexProject.Library.Libs.Ordenacao
{
    /// <summary>
    /// Interface de ordenação
    /// </summary>
    public interface IOrdenacao
    {
        /// <summary>
        /// Variável que recebe a maior ordem
        /// </summary>
        /// <returns>Maior ordem</returns>
        UInt16 GetMaiorOrdem();
        /// <summary>
        /// Variável que recebe o valor de SalvandoOrdens
        /// </summary>
        /// <returns>Valor reordenado</returns>
        bool GetReOrdenando();
        /// <summary>
        /// Variável que seta o valor de SalvandoOrdens
        /// </summary>
        /// <param name="value">value</param>
        void SetReOrdenando(bool value);
        /// <summary>
        /// Variável que recebe o valor da sequência antiga
        /// </summary>
        /// <returns>valor da sequência antiga</returns>
        UInt16 GetOrdemOld();
        /// <summary>
        /// Variável que atribui o valor de sequência antiga
        /// </summary>
        /// <param name="value">value</param>
        void SetOrdemOld(UInt16 value);
        /// <summary>
        /// Variável que recebe o valor da sequência
        /// </summary>
        /// <returns>Valor da sequência</returns>
        UInt16 GetNbOrdem();
        /// <summary>
        /// Variável que atribui o valor para a sequência
        /// </summary>
        /// <param name="value">value</param>
        void SetNbOrdem(UInt16 value);
        /// <summary>
        /// Variável retorn o Oid do objeto
        /// </summary>
        /// <returns>Oid</returns>
        Guid GetOid();
        /// <summary>
        /// Metodo de salvar
        /// </summary>
        void Save();
        /// <summary>
        /// Metodo para verificar se está deletado
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IsDeleted();
        /// <summary>
        /// Get para retornar todos os valores a serem reeordenados
        /// </summary>
        /// <param name="ordemInicial">odermInicial</param>
        /// <param name="ordemFinal">ordemFinal</param>
        /// <returns>Valores a serem reeordenados</returns>
        List<Object> GetItensPorOrdem(int ordemInicial, int ordemFinal = -1);
    }
}
