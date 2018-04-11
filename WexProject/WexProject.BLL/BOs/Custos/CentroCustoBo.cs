using System;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Models.Custos;
using System.Collections.Generic;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Extensions.Custos;

namespace WexProject.BLL.BOs.Custos
{
    public class CentroCustoBo
    {
        /// <summary>
        ///     Singleton
        /// </summary>
        private static CentroCustoBo _instance;

        /// <summary>
        ///     Singleton
        /// </summary>
        private CentroCustoBo()
        {
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static CentroCustoBo Instance
        {
            get { return _instance ?? (_instance = new CentroCustoBo()); }
        }

        /// <summary>
        /// Método para consultar centro de custo por id
        /// </summary>
        /// <param name="centroCustoId">Id do centro custo</param>
        /// <returns>CentroCusto</returns>
        public CentroCusto ConsultarCentroCusto(int centroCustoId)
        {
            var centroCusto = CentroCustoDao.Instance.ConsultarCentroCusto(centroCustoId);

            if (centroCusto != null)
	        {
                return centroCusto;		        
	        }
            else
            {
                throw new EntidadeNaoEncontradaException(String.Format("Centro de Custo com id {0} não encontrado",
                    centroCustoId));
            }
            
        }

        /// <summary>
        /// Método para consultar o centro de custo pelo id
        /// </summary>
        /// <param name="IdCentroCustos">Id centro custo</param>
        /// <returns>CentroCustoDto</returns>
        public CentroCustoDto ConsultarCentroCustosPorId(int IdCentroCustos)
        {
            return ParseDto(CentroCustoDao.Instance.ConsultarCentroCusto(IdCentroCustos));
        }

        /// <summary>
        /// Método para listar todos os centros de custo
        /// </summary>
        /// <returns>Lista de centros custo DTO </returns>      
        public List<CentroCustoDto> ListarCentrosCustos()
        {
            List<CentroCustoDto> centrosCustos = new List<CentroCustoDto>();
            CentroCustoDao.Instance.ListarCentrosCustos().ForEach(delegate(CentroCusto cc)
            {
                centrosCustos.Add(ParseDto(cc));
            });
            return centrosCustos;
        }

        /// <summary>
        /// Método para converter centroCusto para DTO
        /// </summary>
        /// <param name="centroCusto">Objeto centroCusto</param>
        /// <returns>CentroCustoDTO</returns>
        private static CentroCustoDto ParseDto(CentroCusto centroCusto)
        {
            if (centroCusto == null)
            {
                return null;
            }

            return centroCusto.ToDto();
        }

        /// <summary>
        /// Método para salvar um centro de custo
        /// </summary>
        /// <param name="centroCustoDto">Objeto centro custo DTO</param>
        /// <returns>Id do centro custo salvo ou atualizado</returns>
        public int SalvarCentroCusto(CentroCustoDto centroCustoDto)
        {         
            CentroCusto centroCusto = centroCustoDto.FromDto();

            ResgatarIdCentroCusto(centroCusto);

            CentroCustoDao.Instance.SalvarCentroCusto(centroCusto);
        
            return centroCusto.CentroCustoId;
        }

        /// <summary>
        /// Método para recuperar o id do centro custo pelo código
        /// </summary>
        /// <param name="centroCusto">Objeto centro custo</param>
        private void ResgatarIdCentroCusto(CentroCusto centroCusto)
        {
            var cc = CentroCustoDao.Instance.ConsultarCentroCustoPorCodigo(centroCusto.Codigo);
            if (cc != null)
            {
                centroCusto.CentroCustoId = cc.CentroCustoId;
            }
        }
    }
}
