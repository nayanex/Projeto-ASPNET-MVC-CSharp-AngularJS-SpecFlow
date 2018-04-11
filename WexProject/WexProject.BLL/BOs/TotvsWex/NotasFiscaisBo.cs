using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.TotvsWex;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.BLL.Shared.DTOs.TotvsWex;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.BOs.TotvsWex
{
    /// <summary>
    ///     Regras de Negócio das notas fiscais
    /// </summary>
    public class NotasFiscaisBo
    {
        /// <summary>
        ///     Singleton
        /// </summary>
        private static NotasFiscaisBo _instance;

        /// <summary>
        ///     Singleton
        /// </summary>
        private NotasFiscaisBo()
        {
            DescricoesFiltradas = new List<String>
            {
                "13.SALARIO",
                "ACADEMIA",
                "ASSISTENCIA MEDICA E SOCIAL",
                "ATL",
                "AUXILIO ALIMENTAçãO",
                "AUXILIO COMBUSTIVEL",
                "AUXILIO TRANSPORTES",
                "AUXILIO EDUCAÇÃO",
                "BOC",
                "FGTS",
                "HORAS EXTRAS",
                "INSS",
                "MATERIAIS DE NATUREZA PERMANENTE",
                "MATERIAL DE EXPEDIENTE",
                "PIS S/FOLHA",
                "PROGRAMA DE IDIOMAS",
                "PTS",
                "RESCISOES DE CONTRATO",
                "SALARIOS E ORDENADOS",
                "BOG",
                "FERIAS",
                "TARIFAS BANCARIAS",
                "OUTROS MATERIAIS DE CONSUMO",
                "PQC",
                "SERVIçO DE MANUTENçãO E CONSERVAçãO‏"
            };
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static NotasFiscaisBo Instance
        {
            get { return _instance ?? (_instance = new NotasFiscaisBo()); }
        }

        public List<string> DescricoesFiltradas { get; private set; }

        /// <summary>
        ///     Importar dados do Microsiga
        /// </summary>
        public void Importar()
        {
            ImportarCentrosCustos();
            ImportarNotasFiscais();
        }

        /// <summary>
        ///     Popular centros de custo
        /// </summary>
        /// <returns>Booleano</returns>
        private void ImportarCentrosCustos()
        {
            // resgatando centros de custo do totvs
            List<CentroCusto> centrosCustos = NotasFiscaisDao.ListarCentroDeCustosTotvs().ToList();

            // removendo centros de custo já importados
            centrosCustos.RemoveAll(
                centro => CentroCustoDao.Instance.ConsultarCentroCustoPorCodigo(centro.Codigo) != null);

            foreach (CentroCusto centro in centrosCustos)
            {
                CentroCustoDao.Instance.SalvarCentroCusto(centro);
            }
        }

        /// <summary>
        ///     Método para popular a tabela de notas fiscais do Wex a partir do banco do Microsiga
        ///     descosiderando notas fiscais importadas anteriormente
        /// </summary>
        private void ImportarNotasFiscais()
        {
            List<NotaFiscal> notasFiscaisTotvs = NotasFiscaisDao.ListaRubricasTotvs().ToList();

            // normalizando acentuacao das descricoes
            List<string> descricoes = DescricoesFiltradas.Select(StrUtil.NormalizarAcentuacao).ToList();

            // removendo (filtrando) descricoes da lista do totvs
            notasFiscaisTotvs.RemoveAll(notaFiscal => descricoes.Contains(StrUtil.NormalizarAcentuacao(notaFiscal.Descricao.Trim())));

            // removendo notas fiscais já importados
            notasFiscaisTotvs.RemoveAll(notaFiscal => NotasFiscaisDao.VerificarNotasFiscaisImportadas(notaFiscal.ChaveImportacao));

            // salvando novos notas fiscais no wex
            foreach (NotaFiscal notaFiscal in notasFiscaisTotvs)
            {
                // centro de custo nao veio na listagem dos notas fiscais no totvs
                if (notaFiscal.CentroCusto == null) continue;

                CentroCusto centroCusto = CentroCustoDao.Instance.ConsultarCentroCustoPorCodigo(notaFiscal.CentroCusto.Codigo);
                notaFiscal.CentroCusto = null;
                notaFiscal.CentroDeCustoId = centroCusto.CentroCustoId;

                NotasFiscaisDao.SalvarNotaFiscal(notaFiscal);
            }
        }

        /// <summary>
        ///     Lista as notas fiscais de um determinado Centro de Custo em uma data específica
        /// </summary>
        /// <param name="centroCustoDto">Centro de Custo</param>
        /// <param name="ano">ano selecionado</param>
        /// <param name="mes">mês selecionado</param>
        /// <returns></returns>
        public IEnumerable<NotaFiscalDto> ListarNotasFiscais(CentroCustoDto centroCustoDto, int ano, int mes)
        {
            var centroCusto = new CentroCusto {CentroCustoId = centroCustoDto.IdCentroCusto};
            return NotasFiscaisDao.ConsultarNotasFiscais(centroCusto, ano, mes).Select(TransformarParaDto);
        }

        /// <summary>
        ///     Lista notas fiscais de uma Rubrica específica dada uma determinada data
        /// </summary>
        /// <param name="rubricaDto">Rubrica</param>
        /// <param name="ano">Ano selecionado</param>
        /// <param name="mes">Mês selecionado</param>
        /// <returns></returns>
        public IEnumerable<NotaFiscalDto> ListarNotasFiscais(RubricaDto rubricaDto, int ano, int mes)
        {
            var rubrica = new Rubrica {RubricaId = rubricaDto.RubricaId};
            return NotasFiscaisDao.ConsultarNotasFiscais(rubrica, ano, mes).Select(TransformarParaDto);
        }

        public IEnumerable<NotaFiscalDto> ListarNotasFiscais(RubricaDto rubricaDto)
        {
            var rubrica = new Rubrica { RubricaId = rubricaDto.RubricaId };
            return NotasFiscaisDao.ConsultarNotasFiscais(rubrica).Select(TransformarParaDto);
        }

        /// <summary>
        /// </summary>
        /// <param name="notaFiscalDto"></param>
        public void AtualizarNotaFiscal(NotaFiscalDto notaFiscalDto)
        {
            NotaFiscal notaFiscal = NotasFiscaisDao.ConsultarNotaFiscal(notaFiscalDto.GastoId);

            int? rubricaIdAntiga = notaFiscal.RubricaId;

            notaFiscal.RubricaId = notaFiscalDto.RubricaId;
            notaFiscal.Justificativa = notaFiscalDto.Justificativa;

            NotasFiscaisDao.SalvarNotaFiscal(notaFiscal);

            if (rubricaIdAntiga != notaFiscalDto.RubricaId)
            {
                RubricaMesBo.Instance.AtualizarDespesaReal(rubricaIdAntiga ?? 0);
            }
        }

        /// <summary>
        ///     Associar nota fiscal a uma Rubrica.
        /// </summary>
        /// <param name="notaFiscalDto">Gasto a ser associado.</param>
        /// <param name="rubricaId">Id da Rubrica.</param>
        public void AssociarNotaFiscal(NotaFiscalDto notaFiscalDto, int rubricaId)
        {
            notaFiscalDto.RubricaId = rubricaId;

            AtualizarNotaFiscal(notaFiscalDto);

            RubricaMesBo.Instance.AtualizarDespesaReal(rubricaId);
        }

        /// <summary>
        ///     Desassociar nota fiscal de uma rubrica
        /// </summary>
        /// <param name="rubricaId">Id da rubrica</param>
        /// <param name="notaFiscalId">Nota fiscal a ser desassociada.</param>
        public void DesassociarNotaFiscal(int rubricaId, int notaFiscalId)
        {
            NotaFiscal notaFiscal = NotasFiscaisDao.ConsultarNotaFiscal(notaFiscalId);

            notaFiscal.RubricaId = null;

            NotasFiscaisDao.SalvarNotaFiscal(notaFiscal);

            RubricaMesBo.Instance.AtualizarDespesaReal(rubricaId);
        }

        /// <summary>
        /// </summary>
        /// <param name="notaFiscal"></param>
        /// <returns></returns>
        private NotaFiscalDto TransformarParaDto(NotaFiscal notaFiscal)
        {
            return new NotaFiscalDto
            {
                GastoId = notaFiscal.Id,
                CentroDeCustoId = notaFiscal.CentroDeCustoId,
                RubricaId = notaFiscal.RubricaId,
                Descricao = notaFiscal.Descricao,
                HistoricoLancamento = notaFiscal.HistoricoLancamento,
                Justificativa = notaFiscal.Justificativa,
                Data = notaFiscal.Data,
                Valor = notaFiscal.Valor
            };
        }

    }

}