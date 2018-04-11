using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Extensions.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.Externo.Custos;

namespace WexProject.BLL.BOs.Custos
{
    /// <summary>
    ///     BO de Mãos de Obra.
    /// </summary>
    public class MaosDeObraBo
    {
        /// <summary>
        ///     Identificação da rubrica "Mão-de-obra direta"
        /// </summary>
        private const int TipoRubricaId = 15;

        /// <summary>
        ///     Instancia da classe (usado para Singleton)
        /// </summary>
        private static MaosDeObraBo _instance;

        /// <summary>
        ///     Construtor
        /// </summary>
        private MaosDeObraBo()
        {
            MaosDeObraDao = DAOs.Custos.MaosDeObraDao.Instancia;
        }

        /// <summary>
        ///     Instância `singleton` do BO de Mão de Obra.
        /// </summary>
        public static MaosDeObraBo Instance
        {
            get { return _instance ?? (_instance = new MaosDeObraBo()); }
        }

        /// <summary>
        ///     Interface com aplicação externa de dados de Mãos de Obra.
        /// </summary>
        public IMaosDeObraExterno MaosDeObraExterno { get; set; }

        /// <summary>
        ///     Interface utilizada para facilitar a implementaçao com testes unitários (experimental)
        /// </summary>
        public IMaosDeObraDao MaosDeObraDao { get; set; }

        /// <summary>
        ///     Lista Mãos de Obra e Lote mais atualizados de um determinado Mês e Ano de um Projeto.
        ///     Retorna os resultados através de Referências.
        /// </summary>
        /// <param name="centroCustoId">Identificação do centro de custo</param>
        /// <param name="ano">Ano referente aos custos</param>
        /// <param name="mes">Mês referente aos custos</param>
        /// <param name="loteDto">Referência para DTO de Lote de Mão de Obra que será preenchido.</param>
        /// <param name="maosDeObraDto">Referência para DTO de Mão de Obra que será preenchido.</param>
        /// <param name="somaValorTotal">Somatório da propriedade ValorTotal das mãos-de-obra</param>
        public void ListarMaosDeObra(int centroCustoId, int aditivoId, int ano, int mes, out LoteMaoDeObraDto loteDto,
            out List<MaoDeObraDto> maosDeObraDto, out Decimal somaValorTotal, out int quantidadeColaboradores)
        {
            LoteMaoDeObra lote;
            List<MaoDeObra> maosDeObra;
            somaValorTotal = 0;

            RubricaMes rubricaMes = RubricaMesBo.Instance.ResgatarRubricaMes(centroCustoId, TipoRubricaId, aditivoId, ano, mes);

            MaosDeObraDao.ConsultarMaosDeObra(centroCustoId, rubricaMes.RubricaMesId, out lote, out maosDeObra);

            if (lote != null)
            {
                maosDeObraDto = maosDeObra.Select(mo => mo.ToDto()).ToList();
                loteDto = lote.ToDto();
                somaValorTotal = SomarValorTotalMaosDeObra(maosDeObra);
                quantidadeColaboradores = CalcularQuantidadeColaboradores(maosDeObra);
            }
            else
            {
                CentroCusto centroCusto = CentroCustoBo.Instance.ConsultarCentroCusto(centroCustoId);
                throw new EntidadeNaoEncontradaException(
                    String.Format("Nenhum lote encontrado no mês de {0} de {1} no Centro de Custo '{2} - {3}'.",
                        Enum.GetName(typeof(CsMesDomain), mes), ano, centroCusto.Codigo, centroCusto.Nome));
            }

        }

        /// <summary>
        /// Realiza o somatório da propriedade ValorTotal das mãos-de-obra
        /// </summary>
        /// <param name="maosDeObra">Lista de mãos-de-obra</param>
        /// <returns>Somatório realizado</returns>
        private decimal SomarValorTotalMaosDeObra(IEnumerable<MaoDeObra> maosDeObra)
        {
            return maosDeObra.Sum(maoDeObra => maoDeObra != null ? maoDeObra.ValorTotal : 0);
        }

        /// <summary>
        /// Calcula a quantidade colaboradores em MaoDeObra
        /// </summary>
        /// <param name="maosDeObra">Lista de mãos-de-obra</param>
        /// <returns>quantidade de colaboradores</returns>
        private int CalcularQuantidadeColaboradores(IEnumerable<MaoDeObra> maosDeObra)
        {
            return maosDeObra.Count();
        }

        /// <summary>
        ///     Importa Mãos de Obra de um determinado Centro de Custo através da interface com aplicação externa.
        ///     Gera um novo Lote para Mês e Ano determinados.
        /// </summary>
        /// <param name="centroCustoId">Identificação do centro de custo</param>
        /// <param name="ano">Ano referente aos custos</param>
        /// <param name="mes">Mês referente aos custos</param>
        /// <returns>DTO de Lote de Mão de Obra gerado.</returns>
        public LoteMaoDeObraDto Importar(int centroCustoId, int aditivoId, int ano, int mes)
        {
            CentroCusto centroCusto = CentroCustoDao.Instance.ConsultarCentroCusto(centroCustoId);

            RubricaMes rubricaMes = RubricaMesBo.Instance.ResgatarRubricaMes(centroCustoId, TipoRubricaId, aditivoId, ano, mes);

            int codigoImportacao = MaosDeObraExterno.ConsultarCodigoImportacao(centroCusto.Codigo, ano, mes);

            var novoLote = new LoteMaoDeObra
            {
                DataAtualizacao = DateTime.Now,
                CentroCustoImportacao = centroCustoId,
                RubricaMesId = rubricaMes.RubricaMesId,
                CodigoImportacao = codigoImportacao
            };

            MaosDeObraDao.SalvarLote(novoLote);

            List<MaoDeObraDto> maosDeObraDto = MaosDeObraExterno.ConsultarMaosDeObra(centroCusto.Codigo, ano, mes);

            foreach (MaoDeObraDto maoDeObraDto in maosDeObraDto)
            {
                maoDeObraDto.PercentualAlocacao = 100;
                maoDeObraDto.LoteId = novoLote.LoteId;

                MaosDeObraDao.SalvarMaoDeObra(maoDeObraDto.FromDto());
            }

            return novoLote.ToDto();
        }

        /// <summary>
        ///     Compara as versões da importação do Wex com a do Totvs para uma determinada data
        /// </summary>
        /// <param name="centroCustoId">Identificação do centro de custo</param>
        /// <param name="ano">Ano referente aos custos</param>
        /// <param name="mes">Mês referente aos custos</param>
        /// <returns>Verdadeiro quando houver uma nova atualização de importação</returns>
        /// <exception cref="WexProject.BLL.Exceptions.Geral.EntidadeNaoEncontradaException">
        ///     Quando o centro de custo ou lote não
        ///     for encontrado
        /// </exception>
        public Boolean VerificarNovaAtualizacao(int centroCustoId, int ano, int mes)
        {
            CentroCusto centroCusto = CentroCustoDao.Instance.ConsultarCentroCusto(centroCustoId);

            if (centroCusto == null)
            {
                throw new EntidadeNaoEncontradaException("Centro de custo não encontrado.");
            }

            LoteMaoDeObra lote = MaosDeObraDao.ConsultarLote(centroCustoId, ano, mes);

            if (lote == null)
            {
                throw new EntidadeNaoEncontradaException(String.Format("Nenhum lote encontrado no mês de {0} de {1}.",
                    Enum.GetName(typeof (CsMesDomain), mes), ano));
            }

            int codigoImportacaoAtual = MaosDeObraExterno.ConsultarCodigoImportacao(centroCusto.Codigo, ano, mes);

            return (lote.CodigoImportacao != codigoImportacaoAtual);
        }

        /// <summary>
        ///     Realiza o somatório dos custos de mãos-de-obra e salva para o mês/ano da rubrica
        /// </summary>
        /// <param name="centroCustoId">Identificação do centro de custo</param>
        /// <param name="aditivoId">Identificação do aditivo</param>
        /// <param name="ano">Ano referente aos custos</param>
        /// <param name="mes">Mês referente aos custos</param>
        public void AplicarSomatorioCustos(int centroCustoId, int aditivoId, int ano, int mes)
        {
            LoteMaoDeObra lote;
            List<MaoDeObra> maosDeObra;

            RubricaMes rubricaMes = RubricaMesBo.Instance.ResgatarRubricaMes(centroCustoId, TipoRubricaId, aditivoId, ano, mes);

            MaosDeObraDao.ConsultarMaosDeObra(centroCustoId, rubricaMes.RubricaMesId, out lote, out maosDeObra);

            if (lote == null)
            {
                throw new EntidadeNaoEncontradaException(String.Format("Nenhum lote encontrado no mês de {0} de {1}.",
                    Enum.GetName(typeof (CsMesDomain), mes), ano));
            }

            rubricaMes.NbReplanejado = null;
            rubricaMes.NbGasto = SomarValorTotalMaosDeObra(maosDeObra);

            RubricaMesBo.Instance.SalvarRubricaMes(rubricaMes);
        }      
    }
}