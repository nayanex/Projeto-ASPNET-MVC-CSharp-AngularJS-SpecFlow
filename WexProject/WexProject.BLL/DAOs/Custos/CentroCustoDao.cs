using System.Linq;
using System.Collections.Generic;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;
using System.Data;
using WexProject.BLL.Exceptions.Custos;
using System;

namespace WexProject.BLL.DAOs.Custos
{
	public class CentroCustoDao
	{
        private const int IdCentroCustoNulo = 0; 
        /// <summary>
        /// Singleton
        /// </summary>
		private static CentroCustoDao _instance;

        /// <summary>
        /// Singleton
        /// </summary>
        private CentroCustoDao()
        {
        }

	    /// <summary>
	    /// Singleton
	    /// </summary>
	    public static CentroCustoDao Instance
	    {
	        get { return _instance ?? (_instance = new CentroCustoDao()); }
	    }

	    /// <summary>
		/// Retorna todos os centros de custo cadastrados
		/// </summary>
		/// <returns>Uma lista com todos os centros de custo</returns>
		public List<CentroCusto> ListarCentrosCustos()
		{
			List<CentroCusto> centrosCusto;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				centrosCusto = contexto.CentrosCusto.ToList();
			}

			return centrosCusto;
		}

		/// <summary>
		/// Consulta Centro de Custo por Id no Banco de Dados.
		/// </summary>
		/// <param name="centroCustoId">Id do Centro de Custo a ser recuperado.</param>
		/// <returns>Entidade Centro de Custo.</returns>
		public CentroCusto ConsultarCentroCusto(int centroCustoId)
        {
			CentroCusto centroCusto;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                centroCusto = contexto.CentrosCusto.FirstOrDefault(cc => cc.CentroCustoId == centroCustoId);
			}

			return centroCusto;
        }

        /// <summary>
        /// Consulta Centro de Custo por Código CC no Banco de Dados.
        /// </summary>
        /// <param name="codigo">Código do Centro de Custo a ser recuperado.</param>
        /// <returns>Id do registro.</returns>
        public CentroCusto ConsultarCentroCustoPorCodigo(int codigo)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.CentrosCusto.FirstOrDefault(cc => cc.Codigo == codigo);
            }
        }

        /// <summary>
        /// Adiciona ou atualiza Centro de Custos no banco
        /// </summary>
        /// <param name="centroCusto">Objeto CentroCusto a ser adicionado ou atualizado</param>
        /// <returns>Id do Centro de Custo no banco</returns>
        public int SalvarCentroCusto(CentroCusto centroCusto)
        {            
            using(var contexto = ContextFactoryManager.CriarWexDb())
            {
                if (centroCusto.CentroCustoId == IdCentroCustoNulo)
                {
                    contexto.CentrosCusto.Add(centroCusto);
                }
                else
                {
                    contexto.Entry(centroCusto).State = EntityState.Modified;
                }
              
                contexto.SaveChanges();
            }

            return centroCusto.CentroCustoId;
        }
	}
}
