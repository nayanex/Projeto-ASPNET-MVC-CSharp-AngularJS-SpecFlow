using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Exceptions.Analise;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Analise.Custos;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.Library.Libs.Collection;

namespace WexProject.BLL.BOs.Analise
{
    public class AnaliseBo
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static AnaliseBo _instance;

        /// <summary>
        /// Singleton
        /// </summary>
        private AnaliseBo()
        {
        }

        /// <summary>
        /// Singleton
        /// </summary>
        public static AnaliseBo Instance
        {
            get { return _instance ?? (_instance = new AnaliseBo()); }
        }

        /// <summary>
        /// Análise do extrato de um Projeto por ano, com previsões onde não existem valores reais.
        /// </summary>
        /// <param name="rubricaMeses">Detalhes dos meses das Rúbricas do Projeto.</param>
        /// <param name="inicioPlanejado">Início planejado de um projeto.</param>
        /// <param name="terminoPlanejado">Término planejado de um projeto.</param>
        /// <returns>Extrato do Projeto.</returns>
        public ExtratoProjetoDto GerarExtratoProjeto(List<RubricaMesDto> rubricaMeses, DateTime inicioPlanejado,
            DateTime terminoPlanejado)
        {
            ExtratoProjetoDto extratoProjetoDto = new ExtratoProjetoDto
            {
                Total = new OrcamentoDespesas(),
                Anos = new Dictionary<string, List<Object>>()
            };

            foreach (int anoPlanejado in ListarAnosPlanejados(inicioPlanejado, terminoPlanejado))
            {
                GerarCalendarioExtratoProjetoVazio(extratoProjetoDto, anoPlanejado);

                foreach (CsMesDomain mes in ListarMeses())
                {
                    IEnumerable<RubricaMesDto> mesComRubricas = FiltrarMesComRubricas(rubricaMeses, anoPlanejado,
                        (int) mes);

                    if (!VerificarExistenciaCustos(rubricaMeses, anoPlanejado, (int) mes))
                    {
                        continue;
                    }

                    PreencherExtratoProjeto(extratoProjetoDto, mesComRubricas, mes, anoPlanejado);
                }
            }

            return extratoProjetoDto;
        }

        /// <summary>
        /// Preenche as propriedades do objeto Detalhamento
        /// </summary>
        /// <param name="extratoProjetoDto">Objeto extratoProjetoDto</param>
        /// <param name="mesComRubricas">Objetos RubricaMesDto</param>
        /// <param name="mes">mês de que se fala</param>
        /// <param name="anoPlanejado">ano de que se fala</param>
        private void PreencherExtratoProjeto(ExtratoProjetoDto extratoProjetoDto, IEnumerable<RubricaMesDto> mesComRubricas,
            CsMesDomain mes,
            int anoPlanejado)
        {
            Decimal orcamentoDesenvolvimento = CalcularOrcamentoDesenvolvimento(mesComRubricas);
            Custo despesasDesenvolvimento = CalcularDespesasDesenvolvimento(mesComRubricas);

            Decimal orcamentoAdministrativo = CalcularOrcamentoAdministrativo(mesComRubricas);
            Custo despesasAdministrativas = CalcularDespesasAdministrativas(mesComRubricas);

            Custo resultadoMensal = (orcamentoAdministrativo + orcamentoDesenvolvimento) -
                                    despesasAdministrativas - despesasDesenvolvimento;

            extratoProjetoDto.Total.Acumulado += resultadoMensal;

            String anoPlanejadoString = Convert.ToString(anoPlanejado);

            int mesJavascript = SelecionarMesJavascript(mes);

            extratoProjetoDto.Anos[anoPlanejadoString][mesJavascript] = new OrcamentoDespesas
            {
                OrcamentoAprovadoAdministracao = orcamentoAdministrativo,
                OrcamentoAprovadoDesenvolvimento = orcamentoDesenvolvimento,
                DespesasAdministrativas = despesasAdministrativas,
                DespesasDesenvolvimento = despesasDesenvolvimento,
                ResultadoMensal = resultadoMensal,
                Acumulado = extratoProjetoDto.Total.Acumulado
            };

            extratoProjetoDto.Total.OrcamentoAprovadoAdministracao += orcamentoAdministrativo;
            extratoProjetoDto.Total.OrcamentoAprovadoDesenvolvimento += orcamentoDesenvolvimento;
            extratoProjetoDto.Total.DespesasAdministrativas += despesasAdministrativas;
            extratoProjetoDto.Total.DespesasDesenvolvimento += despesasDesenvolvimento;
            extratoProjetoDto.Total.ResultadoMensal = extratoProjetoDto.Total.Acumulado;
        }

        /// <summary>
        /// Filtra as rubricas de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMeses">Objeto RubricaMesDto</param>
        /// <param name="anoPlanejado">Ano de que se fala</param>
        /// <param name="mes">Mês de que se fala</param>
        /// <returns></returns>
        private static IEnumerable<RubricaMesDto> FiltrarMesComRubricas(IEnumerable<RubricaMesDto> rubricaMeses,
            int anoPlanejado, int mes)
        {
            return rubricaMeses.Where(
                rubricaMesDto =>
                    rubricaMesDto.Ano == anoPlanejado &&
                    (int) rubricaMesDto.Mes == mes);
        }

        /// <summary>
        /// Verifica a existência de custos de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMeses"></param>
        /// <param name="anoPlanejado"></param>
        /// <param name="mesPlanejado"></param>
        /// <returns></returns>
        private bool VerificarExistenciaCustos(IEnumerable<RubricaMesDto> rubricaMeses, int anoPlanejado,
            int mesPlanejado)
        {
            return
                rubricaMeses.Any(
                    rubricaMesDto =>
                        rubricaMesDto.Classe != "Aportes" && rubricaMesDto.Ano == anoPlanejado &&
                        (int) rubricaMesDto.Mes == mesPlanejado);
        }

        /// <summary>
        /// Gera um calendario vazio para ser utilizado no detalhamento do Extrato do Projeto
        /// na tela de Análise Crítica por Projeto.
        /// OBS: Este refactor tem como objetivo deixar o código mais legível. Ainda há a
        /// necessidade de discutir melhorias no modelo das entidades e retorno dos dados para WebAPI
        /// </summary>
        /// <param name="extratoProjetoDto">Objeto ExtratoProjetoDto</param>
        /// <param name="anoPlanejado">Ano de interesse</param>
        private void GerarCalendarioExtratoProjetoVazio(ExtratoProjetoDto extratoProjetoDto, int anoPlanejado)
        {
            string anoPlanejadoString = Convert.ToString(anoPlanejado);

            if (!extratoProjetoDto.Anos.ContainsKey(anoPlanejadoString))
            {
                extratoProjetoDto.Anos[anoPlanejadoString] = new List<Object>();
            }

            foreach (int mes in Enum.GetValues(typeof (CsMesDomain)))
            {
                extratoProjetoDto.Anos[anoPlanejadoString].Add(new Object());
            }
        }

        /// <summary>
        ///     Gera um calendario vazio para ser utilizado no detalhamento do Extrato do Projeto
        ///     na tela de Análise Crítica por Projeto.
        ///     OBS: Este refactor tem como objetivo deixar o código mais legível. Ainda há a
        ///     necessidade de discutir melhorias no modelo das entidades e retorno dos dados para WebAPI
        /// </summary>
        /// <param name="extratoFinanceiroDto">Objeto ExtratoFinanceiroDto</param>
        /// <param name="anoPlanejado">Ano de interesse</param>
        private void GerarCalendarioExtratoFinanceiroVazio(ExtratoFinanceiroDto extratoFinanceiroDto, int anoPlanejado)
        {
            string anoPlanejadoString = Convert.ToString(anoPlanejado);

            if (!extratoFinanceiroDto.Anos.ContainsKey(anoPlanejadoString))
            {
                extratoFinanceiroDto.Anos[anoPlanejadoString] = new List<Object>();
            }

            foreach (int mes in Enum.GetValues(typeof (CsMesDomain)))
            {
                extratoFinanceiroDto.Anos[anoPlanejadoString].Add(new Object());
            }
        }

        /// <summary>
        /// Realiza o somatório do Orçamento Aprovado de Desenvolvimento de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMesDtos">Objeto RubricaMesDto</param>
        /// <returns>Soma de valores planejados de meses referente a Rubricas de Desenvolvimento</returns>
        private decimal CalcularOrcamentoDesenvolvimento(IEnumerable<RubricaMesDto> rubricaMesDtos)
        {
            IEnumerable<RubricaMesDto> mesComRubricas =
                rubricaMesDtos.Where(
                    rubricaMesDto =>
                        rubricaMesDto.Classe == "Desenvolvimento");

            return mesComRubricas.Sum(rubricaMesDto => rubricaMesDto.Planejado.GetValueOrDefault());
        }

        /// <summary>
        /// Realiza o somatório do Orçamento Aprovado de Administrativo de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMesDtos"></param>
        /// <returns></returns>
        private decimal CalcularOrcamentoAdministrativo(IEnumerable<RubricaMesDto> rubricaMesDtos)
        {
            IEnumerable<RubricaMesDto> mesComRubricas =
                rubricaMesDtos.Where(
                    rubricaMesDto =>
                        rubricaMesDto.Classe == "Administrativo");

            return mesComRubricas.Sum(rubricaMesDto => rubricaMesDto.Planejado.GetValueOrDefault());
        }

        /// <summary>
        /// Realiza o somatório das despesas de desenvolvimento de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMesDtos"></param>
        /// <returns></returns>
        private Custo CalcularDespesasDesenvolvimento(IEnumerable<RubricaMesDto> rubricaMesDtos)
        {
            IEnumerable<RubricaMesDto> mesComRubricas =
                rubricaMesDtos.Where(
                    rubricaMesDto =>
                        rubricaMesDto.Classe == "Desenvolvimento");
            return mesComRubricas.Aggregate<RubricaMesDto, Custo>(0, (soma, rubricaMesDto) => soma + SelecionarValorDespesaReal(rubricaMesDto));
        }

        /// <summary>
        /// Realiza o somatório das Despesas Administrativas de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMesDtos"></param>
        /// <returns></returns>
        private Custo CalcularDespesasAdministrativas(IEnumerable<RubricaMesDto> rubricaMesDtos)
        {
            IEnumerable<RubricaMesDto> mesComRubricas =
                rubricaMesDtos.Where(
                    rubricaMesDto =>
                        rubricaMesDto.Classe == "Administrativo");

            return mesComRubricas.Aggregate<RubricaMesDto, Custo>(0, (soma, rubricaMesDto) => soma + SelecionarValorDespesaReal(rubricaMesDto));
        }

        /// <summary>
        /// Realiza o somatório dos Aportes Planejados de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMesDtos"></param>
        /// <returns></returns>
        private decimal CalcularAportePlanejado(IEnumerable<RubricaMesDto> rubricaMesDtos)
        {
            IEnumerable<RubricaMesDto> mesComRubricas =
                rubricaMesDtos.Where(
                    rubricaMesDto =>
                        rubricaMesDto.Classe == "Aportes");

            return mesComRubricas.Sum(rubricaMesDto => rubricaMesDto.Planejado.GetValueOrDefault());
        }

        /// <summary>
        /// Realiza o somatório dos Aportes Reais de um determinado ano e mes
        /// </summary>
        /// <param name="rubricaMesDtos"></param>
        /// <returns></returns>
        private Custo CalcularAporteReal(IEnumerable<RubricaMesDto> rubricaMesDtos)
        {
            IEnumerable<RubricaMesDto> mesComRubricas =
                rubricaMesDtos.Where(
                    rubricaMesDto =>
                        rubricaMesDto.Classe == "Aportes");

            return mesComRubricas.Aggregate<RubricaMesDto, Custo>(0, (soma, rubricaMesDto) => soma + SelecionarValorDespesaReal(rubricaMesDto));
        }

        /// <summary>
        /// Análise do fluxo de caixa de um Projeto por ano, com previsões onde não existem valores reais.
        /// </summary>
        /// <param name="rubricaMeses">Detalhes dos meses das Rúbricas do Projeto.</param>
        /// <param name="inicioPlanejado">Início planejado de um projeto.</param>
        /// <param name="terminoPlanejado">Término planejado de um projeto.</param>
        /// <returns>Extrato financeiro do Projeto.</returns>
        public ExtratoFinanceiroDto GerarExtratoFinanceiro(List<RubricaMesDto> rubricaMeses, DateTime inicioPlanejado,
            DateTime terminoPlanejado)
        {
            ExtratoFinanceiroDto extratoFinanceiroDto = new ExtratoFinanceiroDto
            {
                Total = new DespesasAportes(),
                Anos = new Dictionary<string, List<Object>>()
            };

            foreach (int anoPlanejado in ListarAnosPlanejados(inicioPlanejado, terminoPlanejado))
            {
                GerarCalendarioExtratoFinanceiroVazio(extratoFinanceiroDto, anoPlanejado);

                foreach (CsMesDomain mes in ListarMeses())
                {
                    IEnumerable<RubricaMesDto> mesComRubricas = FiltrarMesComRubricas(rubricaMeses, anoPlanejado,
                        (int) mes);

                    if (!VerificarExistenciaCustos(mesComRubricas, anoPlanejado, (int) mes))
                    {
                        continue;
                    }

                    PreencherExtratoFinanceiro(extratoFinanceiroDto, mesComRubricas, mes, anoPlanejado);
                }
            }

            return extratoFinanceiroDto;
        }

        /// <summary>
        /// Preenche as propriedades do objeto ExtratoFinanceiro
        /// </summary>
        /// <param name="extratoFinanceiroDto"></param>
        /// <param name="mesComRubricas"></param>
        /// <param name="mes"></param>
        /// <param name="anoPlanejado"></param>
        private void PreencherExtratoFinanceiro(ExtratoFinanceiroDto extratoFinanceiroDto,
            IEnumerable<RubricaMesDto> mesComRubricas, CsMesDomain mes,
            int anoPlanejado)
        {
            Decimal aportePlanejado = CalcularAportePlanejado(mesComRubricas);

            Custo aporteReal = CalcularAporteReal(mesComRubricas);

            Custo despesasAdministrativas = CalcularDespesasAdministrativas(mesComRubricas);
            Custo despesasDesenvolvimento = CalcularDespesasDesenvolvimento(mesComRubricas);

            Custo despesasReais = despesasAdministrativas + despesasDesenvolvimento;

            extratoFinanceiroDto.Total.Acumulado += aporteReal - despesasReais;

            String anoPlanejadoString = Convert.ToString(anoPlanejado);

            int mesJavascript = SelecionarMesJavascript(mes);

            extratoFinanceiroDto.Anos[anoPlanejadoString][mesJavascript] = new DespesasAportes
            {
                AportePlanejado = aportePlanejado,
                AporteRealizado = aporteReal,
                DespesasReaisAdministrativas = despesasAdministrativas,
                DespesasReaisDesenvolvimento = despesasDesenvolvimento,
                Acumulado = extratoFinanceiroDto.Total.Acumulado
            };

            extratoFinanceiroDto.Total.AportePlanejado += aportePlanejado;
            extratoFinanceiroDto.Total.AporteRealizado += aporteReal;

            extratoFinanceiroDto.Total.DespesasReaisAdministrativas += despesasAdministrativas;
            extratoFinanceiroDto.Total.DespesasReaisDesenvolvimento += despesasDesenvolvimento;

        }

        /// <summary>
        /// Retorna uma lista de projetos para análise de custos
        /// </summary>
        /// <returns>Lista de dtos de projetos com algumas informações de custos</returns>
        public List<AnaliseCriticaSimplesDto> ListarCustosProjetos()
        {
            var filtro = new Filtro<Projeto>
            {
                {"ProjetoMacroOid", null},
                {"CsSituacaoProjeto", CsProjetoSituacaoDomain.EmAndamento}
            };

            List<Projeto> projetosFiltrados = ProjetoDao.Instancia
                .ListarProjetos()
                .Filtra(filtro)
                .ToList();

            return projetosFiltrados.Select(GerarAnaliseCriticaSimples).Where(analiseCritica => analiseCritica != null).ToList();
        }

        /// <summary>
        /// Recupera análise de custos de projeto por id
        /// </summary>
        /// <param name="projetoOid">Id do projeto a ser recuperado</param>
        /// <returns>Dto de custos de projeto com o id passado</returns>
        public AnaliseCriticaDto GerarAnaliseCriticaProjeto(Guid projetoOid)
        {
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorOid(projetoOid);
            List<RubricaMesDto> rubricaMeses = RubricaMesBo.Instance.ListarRubricaMeses(projetoOid);

            if (projeto.DtInicioPlan == null)
            {
                throw new NenhumPlanejamentoException("O projeto não possui data inicial planejada");
            }

            if (projeto.DtTerminoPlan == null)
            {
                throw new NenhumPlanejamentoException("O projeto não possui data término planejada");
            }

            DateTime inicioPlanejado = (DateTime) projeto.DtInicioPlan;
            DateTime terminoPlanejado = (DateTime) projeto.DtTerminoPlan;

            AnaliseCriticaDto projetoDto = new AnaliseCriticaDto
            {
                Nome = projeto.TxNome,
                CustoTotal = projeto.NbValor,
                ExtratoDoProjeto = GerarExtratoProjeto(rubricaMeses, inicioPlanejado, terminoPlanejado),
                ExtratoFinanceiro = GerarExtratoFinanceiro(rubricaMeses, inicioPlanejado, terminoPlanejado)
            };

            return projetoDto;
        }

        /// <summary>
        /// Realiza análise simples do Projeto. Contempla tempo ocorrido, orçamento consumido, fluxo de caixa, projeção e
        /// status.
        /// </summary>
        /// <param name="projeto">Projeto a ser analisado</param>
        /// <returns>Objeto contendo a análise.</returns>
        private AnaliseCriticaSimplesDto GerarAnaliseCriticaSimples(Projeto projeto)
        {
            AnaliseCriticaSimplesDto analiseCriticaSimplesDto = new AnaliseCriticaSimplesDto
            {
                Oid = projeto.Oid,
                Nome = projeto.TxNome,
                Status = Enum.GetName(typeof (CsProjetoSituacaoDomain), projeto.CsSituacaoProjeto)
            };

            List<Aditivo> aditivos = AditivoDao.Instance.ConsultarAditivos(projeto.Oid);

            try
            {
                analiseCriticaSimplesDto.TempoPlanejado = CalcularTempoPlanejado(aditivos);
                analiseCriticaSimplesDto.TempoConsumido = CalcularTempoConsumido(projeto, aditivos);
                analiseCriticaSimplesDto.Porcentagem = CalcularPorcentagem(analiseCriticaSimplesDto.TempoConsumido, analiseCriticaSimplesDto.TempoPlanejado);
            }
            catch (NenhumPlanejamentoException)
            {
                return null;
            }

            List<RubricaMes> rubricaMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(projeto.Oid);

            analiseCriticaSimplesDto.OrcamentoPrevisto = ConsultarOrcamentoPrevisto(projeto);
            analiseCriticaSimplesDto.OrcamentoConsumido = CalcularGastosReais(rubricaMeses);

            analiseCriticaSimplesDto.Projecao = CalcularProjecaoProjeto(rubricaMeses, analiseCriticaSimplesDto.OrcamentoConsumido);
            analiseCriticaSimplesDto.FluxoCaixa = CalcularFluxoCaixa(rubricaMeses);

            analiseCriticaSimplesDto.SituacaoFinanceira = SelecionarSituacaoFinanceira(analiseCriticaSimplesDto.Projecao,
                analiseCriticaSimplesDto.FluxoCaixa);

            return analiseCriticaSimplesDto;
        }

        /// <summary>
        /// </summary>
        /// <param name="aditivos"></param>
        /// <returns></returns>
        public int CalcularTempoPlanejado(List<Aditivo> aditivos)
        {
            if (aditivos == null || !aditivos.Any())
            {
                throw new NenhumPlanejamentoException(
                    "O projeto não possui aditivos para realizar o cálculo do tempo planejado.");
            }
            DateTime dataInicial = aditivos.OrderBy(aditivo => aditivo.DtInicio).First().DtInicio;
            DateTime dataFinal = aditivos.OrderByDescending(aditivo => aditivo.DtTermino).First().DtTermino;

            return (CalcularQuantidadeDeMesesEntreDatas(dataInicial, dataFinal));
        }

        /// <summary>
        /// </summary>
        /// <param name="projeto"></param>
        /// <param name="aditivos"></param>
        /// <returns></returns>
        public int CalcularTempoConsumido(Projeto projeto, IEnumerable<Aditivo> aditivos)
        {
            if (aditivos == null || !aditivos.Any())
            {
                throw new NenhumPlanejamentoException(
                    "O projeto não possui aditivos para realizar o cálculo do tempo planejado.");
            }
            DateTime dataInicial = aditivos.OrderBy(aditivo => aditivo.DtInicio).First().DtInicio;
            DateTime dataAtual = projeto.DtTerminoReal ?? DateTime.Now;
            DateTime dataFinal = aditivos.OrderByDescending(aditivo => aditivo.DtTermino).First().DtTermino;

            if (dataAtual > dataFinal && CalcularQuantidadeDeMesesEntreDatas(dataFinal, dataAtual) > 1)
            {
                return CalcularQuantidadeDeMesesEntreDatas(dataFinal, dataAtual) + CalcularQuantidadeDeMesesEntreDatas(dataInicial, dataFinal) - 1;
            }
            //Condição quando existir datas de aditivos maiores que a data atual
            return CalcularQuantidadeDeMesesEntreDatas(dataInicial, dataAtual);
        }

        /// <summary>
        /// Cálcula diferença de meses entre duas datas
        /// </summary>
        /// <param name="dataInicial">Data inicial</param>
        /// <param name="dataFinal">Data Final</param>
        /// <returns>número de meses entre duas datas</returns>
        public static int CalcularQuantidadeDeMesesEntreDatas(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataFinal >= dataInicial)
            {
                return (dataFinal.Month - dataInicial.Month) + 12 * (dataFinal.Year - dataInicial.Year) + 1;
            }
            else
                return 0;
        }

        
        /// <summary>
        /// Calcula Porcentagem para barra de Análise Crítica de Projetos
        /// </summary>
        /// <param name="tempoConsumido">tempo passado desde início de planejamento de projeto</param>
        /// <param name="tempoPlanejado">tempo planejado para execução de projeto</param>
        /// <returns>porcentagem de valor de tempo consumido em relação a tempo planejado</returns>
        public static double CalcularPorcentagem(int tempoConsumido, int tempoPlanejado)
        {
            return ((double)tempoConsumido /tempoPlanejado) * 100;
        }

        /// <summary>
        /// </summary>
        /// <param name="projeto"></param>
        /// <returns></returns>
        private decimal ConsultarOrcamentoPrevisto(Projeto projeto)
        {
            decimal orcamentoPrevisto = projeto.NbValor;
            return orcamentoPrevisto;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private string SelecionarSituacaoFinanceira(Decimal projecao, Decimal fluxoCaixa)
        {
            if (projecao < 0)
            {
                return "Critico";
            }
            if (fluxoCaixa < 0)
            {
                return "Atencao";
            }
            return "Positivo";
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMeses"></param>
        /// <param name="gastosReais"></param>
        /// <returns></returns>
        private Decimal CalcularProjecaoProjeto(IEnumerable<RubricaMes> rubricaMeses, Decimal gastosReais)
        {
            Decimal projetoProjecao = 0;

            IEnumerable<RubricaMes> rubricaMesesFiltradas = rubricaMeses
                .Where(
                    rubricaMes =>
                        !rubricaMes.Rubrica.TipoRubrica.CsClasse.HasFlag(CsClasseRubrica.Aportes));

            foreach (RubricaMes rubricaMes in rubricaMesesFiltradas)
            {
                projetoProjecao += rubricaMes.NbPlanejado.GetValueOrDefault();

                if (rubricaMes.NbGasto.HasValue) continue;

                if (rubricaMes.NbReplanejado.HasValue)
                {
                    projetoProjecao -= rubricaMes.NbReplanejado.GetValueOrDefault();
                }
                else if (rubricaMes.NbPlanejado.HasValue)
                {
                    projetoProjecao -= rubricaMes.NbPlanejado.GetValueOrDefault();
                }
            }

            projetoProjecao -= gastosReais;

            return projetoProjecao;
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMeses"></param>
        /// <returns></returns>
        private Decimal CalcularGastosReais(IEnumerable<RubricaMes> rubricaMeses)
        {
            Decimal gastosReais =
                rubricaMeses
                    .Where(
                        rubricaMes =>
                            !rubricaMes.Rubrica.TipoRubrica.CsClasse.HasFlag(CsClasseRubrica.Aportes) &&
                            rubricaMes.NbGasto.HasValue)
                    .Sum(rubricaMes => rubricaMes.NbGasto) ?? 0;
            return gastosReais;
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMeses"></param>
        /// <returns></returns>
        private decimal CalcularFluxoCaixa(IEnumerable<RubricaMes> rubricaMeses)
        {
            decimal fluxoCaixa = 0;
            foreach (RubricaMes rubricaMes in rubricaMeses)
            {
                if (rubricaMes.Rubrica.TipoRubrica.CsClasse.HasFlag(CsClasseRubrica.Aportes))
                {
                    fluxoCaixa += SelecionarValorFluxoCaixa(rubricaMes);
                }
                else
                {
                    fluxoCaixa -= SelecionarValorFluxoCaixa(rubricaMes);
                }
            }
            return fluxoCaixa;
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMes"></param>
        /// <returns></returns>
        private decimal SelecionarValorFluxoCaixa(RubricaMes rubricaMes)
        {
            if (rubricaMes.NbGasto.HasValue)
            {
                return rubricaMes.NbGasto.GetValueOrDefault();
            }

            if (rubricaMes.NbReplanejado.HasValue)
            {
                return rubricaMes.NbReplanejado.GetValueOrDefault();
            }

            if (rubricaMes.NbPlanejado.HasValue)
            {
                return rubricaMes.NbPlanejado.GetValueOrDefault();
            }

            return 0;
        }

        /// <summary>
        ///     Retorna valor Gasto OU Replanejado OU Planejado de RubricaMes
        /// </summary>
        /// <param name="rubrica">uma instância de RubricaMesDto</param>
        /// <returns>Valor decimal de acordo com estrtura condicional criada</returns>
        private Custo SelecionarValorDespesaReal(RubricaMesDto rubrica)
        {
            Custo despesaReal = 0;

            if (rubrica.Gasto.HasValue)
            {
                despesaReal = rubrica.Gasto;
                despesaReal.Previsao = false;
            }
            else if (rubrica.Replanejado.HasValue)
            {
                despesaReal = rubrica.Replanejado;
                despesaReal.Previsao = true;
            }
            else if (rubrica.Planejado.HasValue)
            {
                despesaReal = rubrica.Planejado;
                despesaReal.Previsao = true;
            }

            return despesaReal;
        }

        /// <summary>
        /// </summary>
        /// <param name="inicioPlanejado"></param>
        /// <param name="terminoPlanejado"></param>
        /// <returns></returns>
        private IEnumerable<int> ListarAnosPlanejados(DateTime inicioPlanejado, DateTime terminoPlanejado)
        {
            List<int> anos = new List<int>();
            for (int anoPlanejado = inicioPlanejado.Year; anoPlanejado <= terminoPlanejado.Year; anoPlanejado++)
            {
                anos.Add(anoPlanejado);
            }
            return anos;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CsMesDomain> ListarMeses()
        {
            return Enum.GetValues(typeof (CsMesDomain)).Cast<CsMesDomain>().ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        private int SelecionarMesJavascript(CsMesDomain mes)
        {
            return (int) mes - 1;
        }
    }
}