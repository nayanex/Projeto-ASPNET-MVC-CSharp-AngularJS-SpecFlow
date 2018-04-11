using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities;
namespace WexProject.BLL.Extensions.Entities
{
    /// <summary>
    /// Classe estática para contendo método de extensão para consultas com entity framework, possibilitando multiplos
    /// includes via parametro do método
    /// </summary>
    public static class EntitiesExtension
    {
        /// <summary>
        /// Método genérico para multiplos includes
        /// </summary>
        /// <typeparam name="T">Entidade que será pesquisada</typeparam>
        /// <param name="query">DbSet da entidade que será feita o include</param>
        /// <param name="includes">relações a serem incluidas</param>
        /// <returns>Retorna um IQueryable que inclui os relacionamentos na</returns>
        public static IQueryable<T> MultiploInclude<T>( this IQueryable<T> query, params Expression<Func<T, object>>[] includes ) where T : class
        {
            if(includes != null)
            {
                query = includes.Aggregate( query,
                    ( current, include ) => current.Include( include ) );
            }
            return query;
        }

        /// <summary>
        /// Marca propriedades de uma entidade para que o seu valor não seja alterado no banco
        /// </summary>
        /// <typeparam name="TEntity">tipo da entidade</typeparam>
        /// <typeparam name="TProperty">tipo da propriedade</typeparam>
        /// <param name="entidade">entidade</param>
        /// <param name="propriedades"> propriedades que nao deverao ter seus valores modificados no banco</param>
        public static void PropriedadesNaoModificadas<TEntity>( this DbContext contexto, TEntity entidade, params Expression<Func<TEntity, object>>[] propriedades ) where TEntity : class
        {
            foreach(var item in propriedades)
            {
                contexto.Entry( entidade ).Property( item ).IsModified = false;
            }
        }

        /// <summary>
        /// Método genérico para verificar se determinado valor existe no banco
        /// </summary>
        /// <typeparam name="T">Entidade</typeparam>
        /// <param name="query">Coleção</param>
        /// <param name="condicao"> condições esperadas</param>
        /// <returns></returns>
        public static bool Existe<T>( this IQueryable<T> query, Expression<Func<T, bool>> condicao ) where T : class
        {
            return condicao != null && query.Any( condicao );
        }

        /// <summary>
        /// Método que verifica se existe uma instancia definida no cache do contexto
        /// </summary>
        /// <typeparam name="T">Entidade</typeparam>
        /// <param name="query">Dbset da entidade</param>
        /// <param name="condicao">Condição de verificação</param>
        /// <returns></returns>
        public static bool ExisteLocalmente<T>( this DbSet<T> query, Func<T, bool> condicao ) where T : class
        {
            return condicao != null && query.Local.Any( condicao );
        }
    }
}
