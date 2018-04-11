using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.TotvsWex;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.TotvsWex;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.BOs.Custos
{
    public class RubricaBo
    {

        /// <summary>
        /// Singleton
        /// </summary>
        private static RubricaBo _instance;

        /// <summary>
        /// Singleton
        /// </summary>
        private RubricaBo()
        {
        }

        /// <summary>
        /// Singleton
        /// </summary>
        public static RubricaBo Instance
        {
            get { return _instance ?? (_instance = new RubricaBo()); }
        }

        /// <summary>
        ///     Verifica se Rúbrica está vazia, ou seja, não possui valores preenchidos.
        /// </summary>
        /// <param name="rubrica">A Rúbrica a ser verificada.</param>
        /// <returns>Boolean indicando se a Rúbrica está vazia.</returns>
        public Boolean VerificarRubricaVazia(Rubrica rubrica)
        {
            if (rubrica.Filhos.Count != 0)
            {
                return false;
            }

            if (rubrica.NbTotalPlanejado > 0)
            {
                return false;
            }

            List<RubricaMesDto> rubricaMesesDto = RubricaMesDao.Instance.ConsultarRubricaMeses(rubrica.RubricaId);

            return
                rubricaMesesDto.All(
                    rubricaMesDto =>
                        !(rubricaMesDto.Planejado > 0) && !(rubricaMesDto.Replanejado > 0) && !(rubricaMesDto.Gasto > 0));
        }

        /// <summary>
        ///     Remove Rúbrica se estiver vazia ou se force for igual a true.
        /// </summary>
        /// <param name="rubricaId">Id da Rúbrica a ser excluída.</param>
        /// <param name="force">Indica se Rúbrica deve ser excluída quando não estiver vazia.</param>
        /// <param name="filhos">Lista de Rúbricas filhas da Rúbrica a ser deletada.</param>
        /// <returns>Id da Rúbrica removida.</returns>
        /// <exception cref="RubricaNaoVaziaException">Caso Rúbrica não esteja vazia e force for igual a false.</exception>
        public int RemoverRubrica(int rubricaId, bool force, out List<int> filhos)
        {
            Rubrica rubrica = RubricaDao.Instance.ConsultarRubrica(rubricaId);

            if (force || VerificarRubricaVazia(rubrica))
            {

                var notasFiscais = NotasFiscaisDao.ConsultarNotasFiscais(rubrica);

                foreach (var notaFiscal in notasFiscais)
                {
                    NotasFiscaisBo.Instance.DesassociarNotaFiscal(rubrica.RubricaId, notaFiscal.Id);
                }

                var rubricasMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(rubrica.RubricaId);

                foreach (var rubricaMes in rubricasMeses)
                {
                    RubricaMesDao.Instance.RemoverRubricaMes(rubricaMes.RubricaMesId);
                }

                return RubricaDao.Instance.RemoverRubrica(rubricaId, out filhos);

            }

            throw new RubricaNaoVaziaException(String.Format("Rúbrica {0} não está vazia. Use force para remover.",
                rubricaId));
        }

        /// <summary>
        ///     Consulta Rúbricas de um Aditivo transformando em DTOs.
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo.</param>
        /// <returns>Lista de RubricaDto do Aditivo.</returns>
        public List<RubricaDto> ListarRubricas(int aditivoId)
        {
            List<Rubrica> rubricas = RubricaDao.Instance.ConsultarRubricas(aditivoId);

            List<RubricaDto> rubricasDto = rubricas.Select(r => new RubricaDto
            {
                RubricaId = r.RubricaId,
                Anos = CriarListaAnosRubrica(r)
            }).ToList();

            return rubricasDto;
        }

        /// <summary>
        ///     Recupera Rúbrica por Id
        /// </summary>
        /// <param name="rubricaId">Id da Rúbrica a recuperar</param>
        /// <returns>DTO de Rpubrica</returns>
        public RubricaDto PesquisarRubrica(int rubricaId)
        {
            Rubrica rubrica = RubricaDao.Instance.ConsultarRubrica(rubricaId);

            var rubricaDto = new RubricaDto
            {
                RubricaId = rubrica.RubricaId,
                AditivoId = rubrica.AditivoId,
                Classe = rubrica.TipoRubrica.CsClasse,
                Tipo = rubrica.TipoRubricaId,
                Nome = rubrica.TipoRubrica.TxNome,
                TotalPlanejado = rubrica.NbTotalPlanejado,
                Anos = CriarListaAnosRubrica(rubrica)
            };

            return rubricaDto;
        }

        /// <summary>
        ///     Recupera Rúbricas de um Aditivo, filtrando por classe.
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo a ter Rúbricas recuperadas.</param>
        /// <param name="classe">Classe das Rúbricas a recuperar.</param>
        /// <returns>Uma lista de DTOs de Rúbricas pertencentes ao Aditivo e à classe passada.</returns>
        public List<RubricaDto> PesquisarRubricas(int aditivoId, CsClasseRubrica classe)
        {
            var rubricasDto = new List<RubricaDto>();

            List<Rubrica> rubricas = RubricaDao.Instance.ConsultarRubricas(aditivoId, classe);

			rubricasDto = DefinirRubricaDto(rubricas);

            return rubricasDto;
        }

		public List<RubricaDto> DefinirRubricaDto(List<Rubrica> rubricas)
		{
			var rubricasDto = new List<RubricaDto>();

			foreach (Rubrica rubrica in rubricas.Where(r => r.PaiId == null))
			{
				rubricasDto.Add(new RubricaDto
				{
					RubricaId = rubrica.RubricaId,
					AditivoId = rubrica.AditivoId,
					Classe = rubrica.TipoRubrica.CsClasse,
					Tipo = rubrica.TipoRubricaId,
					Nome = rubrica.TipoRubrica.TxNome,
					TotalPlanejado = rubrica.NbTotalPlanejado,
					Anos = CriarListaAnosRubrica(rubrica)
				});

				rubricasDto.AddRange(rubrica.Filhos.Select(rubricaFilha => new RubricaDto
				{
					RubricaId = rubricaFilha.RubricaId,
					AditivoId = rubricaFilha.AditivoId,
					PaiId = rubrica.RubricaId,
					Classe = rubricaFilha.TipoRubrica.CsClasse,
					Tipo = rubricaFilha.TipoRubricaId,
					Nome = rubricaFilha.TipoRubrica.TxNome,
					TotalPlanejado = rubricaFilha.NbTotalPlanejado,
					Anos = CriarListaAnosRubrica(rubricaFilha)
				}));
			}

			return rubricasDto;
		}

        /// <summary>
        /// Pesquisa as rubricas utilizadas na associação de notas ficais em custos por projeto
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <returns>rubricas filtradas (não contendo rubricas de RH)</returns>
        public List<RubricaDto> PesquisarRubricasNotasFiscais(int aditivoId)
        {
			var rubricasDto = new List<RubricaDto>();

			List<Rubrica> rubricas = RubricaDao.Instance.ConsultarRubricasNotasFiscais(aditivoId, CsClasseRubrica.Desenvolvimento);

			rubricasDto = DefinirRubricaDto(rubricas);

            return rubricasDto;
        }

        
        /// <summary>
        ///     Cria uma lista de anos da Rúbrica.
        /// </summary>
        /// <param name="rubrica">Rubrica a ter anos criados.</param>
        /// <returns>Lista de anos.</returns>
        public List<RubricaAnoDto> CriarListaAnosRubrica(Rubrica rubrica)
        {
            var anos = new List<RubricaAnoDto>();

            var anoFinal = rubrica.Aditivo.DtInicio.ToUniversalTime().AddMonths(rubrica.Aditivo.NbDuracao - 1).Year;
            anoFinal = Math.Max(rubrica.Aditivo.DtTermino.ToUniversalTime().Year, anoFinal);

            if (rubrica.TipoRubrica.CsClasse.HasFlag(CsClasseRubrica.Pai)) return anos;

            var rubricaMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(rubrica);

            for (var ano = rubrica.Aditivo.DtInicio.ToUniversalTime().Year; ano <= anoFinal; ano++)
            {
                anos.Add(new RubricaAnoDto
                {
                    Ano = ano,
                    Meses = RubricaMesBo.Instance.GerarRubricasMeses(rubrica, rubricaMeses, ano)
                });
            }

            return anos;
        }

        /// <summary>
        ///     Adiciona ou atualiza uma rubrica no banco.
        /// </summary>
        /// <param name="rubricaDto">Objeto DTO de rubrica a ser adicionado ou atualizado</param>
        /// <param name="paiId">Id do pai da Rúbrica ou null caso não tenha.</param>
        /// <returns>Id da rubrica no banco</returns>
        public int SalvarRubrica(RubricaDto rubricaDto, out int? paiId)
        {
            var rubrica = new Rubrica
            {
                RubricaId = rubricaDto.RubricaId,
                AditivoId = rubricaDto.AditivoId,
                PaiId = rubricaDto.PaiId,
                TipoRubricaId = rubricaDto.Tipo,
                NbTotalPlanejado = rubricaDto.TotalPlanejado
            };

            ProjetoBo.Instancia.ValidarTipoProjeto(rubrica.AditivoId, rubrica.TipoRubricaId);

            Rubrica rubricaPai = PesquisarRubricaPai(rubrica);

            if (rubricaPai != null)
            {
                if (rubricaPai.RubricaId == 0)
                {
                    RubricaDao.Instance.SalvarRubrica(rubricaPai);
                }

                rubrica.PaiId = rubricaPai.RubricaId;
            }

            RubricaDao.Instance.SalvarRubrica(rubrica);

            paiId = rubrica.PaiId;

            return rubrica.RubricaId;
        }

        /// <summary>
        ///     Recupera Rúbrica pai do Banco de Dados
        /// </summary>
        /// <param name="rubrica">Rúbrica a recuperar o pai</param>
        /// <returns>Rúbrica pai ou nulo caso não exista</returns>
        public Rubrica PesquisarRubricaPai(Rubrica rubrica)
        {
            Rubrica rubricaPai = null;
            TipoRubrica tipoRubrica =
                TipoRubricaDao.Instance.ConsultarTiposRubricas().Single(tr => tr.TipoRubricaId == rubrica.TipoRubricaId);

            if (tipoRubrica.TipoPaiId.HasValue && !rubrica.PaiId.HasValue)
            {
                rubricaPai = RubricaDao.Instance.ConsultarRubrica(rubrica.AditivoId, tipoRubrica.TipoPaiId.Value) ?? new Rubrica
                {
                    AditivoId = rubrica.AditivoId,
                    TipoRubricaId = tipoRubrica.TipoPaiId.Value,
                    NbTotalPlanejado = rubrica.NbTotalPlanejado
                };
            }

            return rubricaPai;
        }

		public Rubrica PesquisarRubrica(Guid projetoId, int tipoRubricaId, int ano, int mes)
        {
			Rubrica rubrica = RubricaDao.Instance.ConsultarRubricas(projetoId, tipoRubricaId, ano, mes).FirstOrDefault();
            if (rubrica == null)
            {
                throw new EntidadeNaoEncontradaException();
            }
            return rubrica;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centroCustoId"></param>
        /// <param name="tipoRubricaId"></param>
        /// <returns></returns>
        public Rubrica PesquisarRubrica(int centroCustoId, int tipoRubricaId)
        {
            Rubrica rubrica = RubricaDao.Instance.ConsultarRubricas(centroCustoId, tipoRubricaId).FirstOrDefault();
            if (rubrica == null)
            {
                throw new EntidadeNaoEncontradaException();
            }
            return rubrica;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centroCustoId"></param>
        /// <param name="tipoRubricaId"></param>
        /// <param name="aditivoId"></param>
        /// <returns></returns>
        public Rubrica PesquisarRubrica(int centroCustoId, int tipoRubricaId, int aditivoId)
        {
            Rubrica rubrica = RubricaDao.Instance.ConsultarRubricas(centroCustoId, tipoRubricaId).FirstOrDefault(r => r.AditivoId == aditivoId);
            if (rubrica == null)
            {
                throw new EntidadeNaoEncontradaException();
            }
            return rubrica;
        }

        
    }
}