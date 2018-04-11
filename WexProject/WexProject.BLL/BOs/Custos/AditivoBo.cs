using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.TotvsWex;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.TotvsWex;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Projeto;

namespace WexProject.BLL.BOs.Custos
{
    public class AditivoBo
    {

        /// <summary>
        /// Singleton
        /// </summary>
        private static AditivoBo _instance;

        /// <summary>
        /// Singleton
        /// </summary>
        private AditivoBo()
        {
        }

        /// <summary>
        /// Singleton
        /// </summary>
		public static AditivoBo Instance
		{
			get { return _instance ?? (_instance = new AditivoBo()); }
		}

        /// <summary>
        ///     Verifica se Aditivo está vazio, ou seja, não possui valores preenchidos.
        /// </summary>
        /// <param name="aditivo">Aditivo a ser verificado.</param>
        /// <returns>Boolean indicando se está vazio.</returns>
        public Boolean VerificarAditivoVazio(Aditivo aditivo)
        {
            if (aditivo.NbOrcamento > 0)
            {
                return false;
            }

            List<Rubrica> rubricas = RubricaDao.Instance.ConsultarRubricas(aditivo.AditivoId);

            return rubricas.All(RubricaBo.Instance.VerificarRubricaVazia);
        }

        /// <summary>
        ///     Remove um Aditivo se estiver vazio vazi ou se force for igual a true.
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo a ser removido.</param>
        /// <param name="force">Indica se o Aditivo deve ser excluído quando não estiver vazio.</param>
        /// <returns>Id do Aditivo excluído.</returns>
        /// <exception cref="AditivoNaoVazioException">Caso Aditivo não esteja vazio e force for igual a false.</exception>
        public int RemoverAditivo(int aditivoId, bool force)
        {
            List<int> filhos;
            Aditivo aditivo = AditivoDao.Instance.ConsultarAditivo(aditivoId);

            if (force || VerificarAditivoVazio(aditivo))
            {

                List<RubricaDto> rubricas = RubricaBo.Instance.ListarRubricas(aditivoId);
                List<AditivoPatrocinador> aditivoPatrocinador = AditivoDao.Instance.ConsultarAditivosPatrocinadores(aditivoId);
                List<CentroCustoDto> centrosCusto = ListarCentrosCustos(aditivoId);

                foreach (var cc in centrosCusto)
                {
                    DesassociarCentroCusto(aditivoId, cc.IdCentroCusto);
                }

                foreach (var adPatrocinador in aditivoPatrocinador)
                {
                    DesassociarPatrocinador(aditivoId, adPatrocinador.PatrocinadorOid);
                }


                foreach (var rubrica in rubricas)
                {
                    RubricaBo.Instance.RemoverRubrica(rubrica.RubricaId, force, out filhos);
                }


                AditivoDao.Instance.RemoverAditivo(aditivoId);
                ProjetoBo.Instancia.RecalcularDadosProjeto(aditivo.ProjetoOid);
                return aditivoId;
            }

            throw new AditivoNaoVazioException(String.Format("Aditivo {0} não está vazio. Use force para remover.",
                aditivoId));
        }

        /// <summary>
        ///     Lista todos os aditivos de um projeto
        /// </summary>
        /// <param name="projetoOid">Id do projeto</param>
        /// <returns>Lista de AditivoDto</returns>
        public List<AditivoDto> ListarAditivos(Guid projetoOid)
        {
            List<Aditivo> aditivos = AditivoDao.Instance.ConsultarAditivos(projetoOid);

            List<AditivoDto> aditivosDto = (from a in aditivos
                select new AditivoDto
                {
                    AditivoId = a.AditivoId,
                    Nome = a.TxNome,
                    DataInicio = a.DtInicio,
                    DataTermino = a.DtTermino,
                    Duracao = a.NbDuracao,
                    Orcamento = a.NbOrcamento,
					OrcamentoRestante = CalcularOrcamentoRestante(a.AditivoId),
                    Projeto = a.ProjetoOid
                }).ToList();

            return aditivosDto;
        }

        /// <summary>
        ///     Método para consultar um aditivo pelo Id
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <returns>AditivoDto</returns>
        public AditivoDto PesquisarAditivo(int aditivoId)
        {
            Aditivo aditivo = AditivoDao.Instance.ConsultarAditivo(aditivoId);

            var aditivoDto = new AditivoDto
            {
                AditivoId = aditivo.AditivoId,
                Nome = aditivo.TxNome,
                DataInicio = aditivo.DtInicio,
                DataTermino = aditivo.DtTermino,
                Duracao = aditivo.NbDuracao,
                Orcamento = aditivo.NbOrcamento,
				OrcamentoRestante = CalcularOrcamentoRestante(aditivo.AditivoId),
                Projeto = aditivo.ProjetoOid
            };

            return aditivoDto;
        }

        /// <summary>
        ///     Adiciona ou atualiza aditivo no banco
        /// </summary>
        /// <param name="aditivoDto">Objeto aditivoDto a ser adicionado ou atualizado</param>
        /// <returns>Id do aditivo no banco</returns>
        public int SalvarAditivo(AditivoDto aditivoDto)
        {
            var aditivo = new Aditivo
            {
                AditivoId = aditivoDto.AditivoId,
                TxNome = aditivoDto.Nome,
                DtInicio = aditivoDto.DataInicio,
                DtTermino = aditivoDto.DataTermino,
                NbDuracao = aditivoDto.Duracao,
                NbOrcamento = aditivoDto.Orcamento,
                ProjetoOid = aditivoDto.Projeto
            };

            int aditivoId = AditivoDao.Instance.SalvarAditivo(aditivo);

            ProjetoBo.Instancia.RecalcularDadosProjeto(aditivo.ProjetoOid);

            return aditivoId;
        }

        /// <summary>
        ///     Método para listar os patrocinadores de um aditivo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <returns>Lista de EmpresaInstituicaoDto</returns>
        public List<EmpresaInstituicaoDto> ListarPatrocinadores(int aditivoId)
        {
            List<EmpresaInstituicao> patrocinadores = AditivoDao.Instance.ConsultarPatrocinadores(aditivoId);

            List<EmpresaInstituicaoDto> patrocinadoresDto = (from p in patrocinadores
                select new EmpresaInstituicaoDto
                {
                    Oid = p.Oid,
                    Nome = p.TxNome,
                    Sigla = p.TxSigla,
                    Email = p.TxEmail,
                    FoneFax = p.TxFoneFax
                }).ToList();


            return patrocinadoresDto;
        }

        /// <summary>
        ///     Método para adicionar um patrocinador
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <param name="patrocinadorOid">Id do patrocinador</param>
        /// <returns></returns>
        public int AssociarPatrocinador(int aditivoId, Guid patrocinadorOid)
        {
            int aditivoPatrocinadorId = AditivoDao.Instance.AssociarPatrocinador(aditivoId, patrocinadorOid);
            return aditivoPatrocinadorId;
        }

        /// <summary>
        ///     Método para remover um patrocinador
        /// </summary>
        /// <returns></returns>
        public int DesassociarPatrocinador(int aditivoId, Guid patrocinadorOid)
        {
            int aditivoPatrocinadorId = AditivoDao.Instance.DesassociarPatrocinador(aditivoId, patrocinadorOid);
            return aditivoPatrocinadorId;
        }

        /// <summary>
        ///     Método para consultar centros de custo por id
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <returns>Lista de centros de custo</returns>
        public List<CentroCustoDto> ListarCentrosCustos(int aditivoId)
        {
            List<CentroCusto> centrosCusto = AditivoDao.Instance.ConsultarCentrosCustos(aditivoId);

            List<CentroCustoDto> centrosCustoDto = (from c in centrosCusto
                select new CentroCustoDto
                {
                    IdCentroCusto = c.CentroCustoId,
                    Nome = c.Nome
                }).ToList();

            return centrosCustoDto;
        }

        /// <summary>
        ///     Método para adicionar um centro de custo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <param name="centroCustoId">Id do centro de custo</param>
        /// <returns>Id do centro de custo</returns>
        public int AssociarCentroCusto(int aditivoId, int centroCustoId)
        {
            int aditivoCentroCustoId = AditivoDao.Instance.AssociarCentroCusto(aditivoId, centroCustoId);
            return aditivoCentroCustoId;
        }

        /// <summary>
        ///     Método para remover um centro de custo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <param name="centroCustoId">Id do centro de custo</param>
        /// <returns>Id do centro de custo removido</returns>
        public int DesassociarCentroCusto(int aditivoId, int centroCustoId)
        {
            int aditivoCentroCustoId = AditivoDao.Instance.DesassociarCentroCusto(aditivoId, centroCustoId);
            return aditivoCentroCustoId;
        }

		/// <summary>
		///	Método para realizar o cálculo do orçamento restante do aditivo
		/// </summary>
		/// <param name="aditivoId">Id do aditivo</param>
		/// <returns>Orcamento restante</returns>
		public Decimal CalcularOrcamentoRestante(int aditivoId)
		{
			var rubricas = RubricaDao.Instance.ConsultarRubricas(aditivoId);

			rubricas.RemoveAll(r => r.TipoRubrica.CsClasse == CsClasseRubrica.Aportes);

			var aditivo = AditivoDao.Instance.ConsultarAditivo(aditivoId);

			var valorPlanejadoRubricas = rubricas.Sum(r => r.NbTotalPlanejado);

			return aditivo.NbOrcamento - valorPlanejadoRubricas;
		}
    }
}
