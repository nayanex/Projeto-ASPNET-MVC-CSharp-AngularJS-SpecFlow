using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.DAOs.TotvsWex;
using WexProject.BLL.Extensions.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.BOs.Custos
{
    public class RubricaMesBo
    {
        /// <summary>
        ///     Singleton
        /// </summary>
        private static RubricaMesBo _instance;

        /// <summary>
        ///     Singleton
        /// </summary>
        private RubricaMesBo()
        {
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static RubricaMesBo Instance
        {
            get { return _instance ?? (_instance = new RubricaMesBo()); }
        }

        /// <summary>
        /// </summary>
        /// <param name="despesaRealDto"></param>
        public void SalvarDespesaReal(DespesaRealDto despesaRealDto)
        {
            ProjetoDto projeto = ProjetoBo.Instancia.ConsultarProjeto(despesaRealDto.ProjetoOid);

            RubricaMes rubricaMes = Instance.SelecionarRubricaMes(projeto.Oid, despesaRealDto.TipoRubricaId,
                despesaRealDto.Ano, (int) despesaRealDto.Mes);

            rubricaMes.NbGasto = despesaRealDto.DespesaReal;

            SalvarRubricaMes(rubricaMes);
        }

        /// <summary>
        ///     Método para selecionar uma rubricaMes
        /// </summary>
        /// <param name="projetoId">Id do projeto</param>
        /// <param name="tipoRubricaId">Id do tipoRubrica</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>Rubrica</returns>
        public RubricaMes SelecionarRubricaMes(Guid projetoId, int tipoRubricaId, int ano, int mes)
        {
            List<RubricaMes> rubricaMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(projetoId, tipoRubricaId, ano,
                mes);

            RubricaMes rubricaMes = rubricaMeses.FirstOrDefault();

            if (rubricaMes != null) return rubricaMes;

            Rubrica rubrica = RubricaBo.Instance.PesquisarRubrica(projetoId, tipoRubricaId, ano, mes);

            rubricaMes = new RubricaMes
            {
                RubricaId = rubrica.RubricaId,
                NbAno = ano,
                CsMes = (CsMesDomain) mes
            };

            SalvarRubricaMes(rubricaMes);

            return rubricaMes;
        }

        /// <summary>
        /// </summary>
        /// <param name="centroCustoId"></param>
        /// <param name="tipoRubricaId"></param>
        /// <param name="aditivoId"></param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public RubricaMes ResgatarRubricaMes(int centroCustoId, int tipoRubricaId, int aditivoId, int ano, int mes)
        {
            List<RubricaMes> rubricaMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(centroCustoId, tipoRubricaId, aditivoId,
                ano, mes);

            RubricaMes rubricaMes = rubricaMeses.FirstOrDefault(r => r.Rubrica.AditivoId == aditivoId);

            if (rubricaMes != null) return rubricaMes;

            Rubrica rubrica = RubricaBo.Instance.PesquisarRubrica(centroCustoId, tipoRubricaId, aditivoId);

            rubricaMes = new RubricaMes
            {
                RubricaId = rubrica.RubricaId,
                NbAno = ano,
                CsMes = (CsMesDomain) mes
            };

            SalvarRubricaMes(rubricaMes);

            return rubricaMes;
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMes"></param>
        /// <returns></returns>
        public int SalvarRubricaMes(RubricaMes rubricaMes)
        {
            return RubricaMesDao.Instance.SalvarRubricaMes(rubricaMes);
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMesDto"></param>
        /// <returns></returns>
        public int SalvarRubricaMes(RubricaMesDto rubricaMesDto)
        {
            return SalvarRubricaMes(rubricaMesDto.FromDto());
        }


        /// <summary>
        ///     Recupera detalhes dos meses de todas a Rúbricas de um projeto.
        /// </summary>
        /// <param name="projetoOid">Id do Projeto.</param>
        /// <returns>Lista de DTOs do detalhe do mês.</returns>
        public List<RubricaMesDto> ListarRubricaMeses(Guid projetoOid)
        {
            List<RubricaMesDto> rubricasMesesDto = RubricaMesDao.Instance.ConsultarRubricaMeses(projetoOid)
                .Select(rm => new RubricaMesDto
                {
                    RubricaMesId = rm.RubricaMesId,
                    RubricaId = rm.RubricaId,
                    Classe = Enum.GetName(typeof (CsClasseRubrica), rm.Rubrica.TipoRubrica.CsClasse),
                    Mes = rm.CsMes,
                    Ano = rm.NbAno,
                    PossuiGastosRelacionados = rm.PossuiGastosRelacionados,
                    Planejado = rm.NbPlanejado,
                    Replanejado = rm.NbReplanejado,
                    Gasto = rm.NbGasto
                }).ToList();

            return rubricasMesesDto;
        }

        /// <summary>
        ///     Retorna uma lista dos meses de uma rubrica em um ano
        /// </summary>
        /// <param name="rubrica">Rubrica a ter os meses recuperados</param>
        /// <param name="ano">Ano a ser recuperado</param>
        /// <returns>Lista de descrições dos meses de uma rubrica</returns>
        public List<RubricaMesDto> ListarRubricaMeses(Rubrica rubrica, int ano)
        {
            var rubricaMesDtos = new List<RubricaMesDto>();

            List<RubricaMes> rubricaMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(rubrica);

            for (int mes = 1; mes <= 12; mes++)
            {
                RubricaMesDto rubricaMesDto;

                //RubricaMes rubricaMes = RubricaMesDao.Instance.ConsultarRubricaMes(rubrica, mes, ano);
                RubricaMes rubricaMes = rubricaMeses.FirstOrDefault(rm => rm.NbAno == ano && (int) rm.CsMes == mes);

                if (rubricaMes != null)
                {
                    rubricaMesDto = rubricaMes.ToDto();
                }
                else
                {
                    rubricaMesDto = new RubricaMesDto
                    {
                        RubricaId = rubrica.RubricaId,
                        Mes = (CsMesDomain) mes,
                        Ano = ano
                    };
                }

                rubricaMesDtos.Add(rubricaMesDto);
            }
            return rubricaMesDtos;
        }

        /// <summary>
        ///     Retorna uma lista dos meses de uma rubrica em um ano
        /// </summary>
        /// <param name="rubrica"></param>
        /// <param name="rubricasMeses">Rubrica a ter os meses recuperados</param>
        /// <param name="ano">Ano a ser recuperado</param>
        /// <returns>Lista de descrições dos meses de uma rubrica</returns>
        public List<RubricaMesDto> GerarRubricasMeses(Rubrica rubrica, List<RubricaMes> rubricasMeses, int ano)
        {
            var rubricaMesDtos = new List<RubricaMesDto>();

            for (var mes = 1; mes <= 12; mes++)
            {
                var mesDomain = (CsMesDomain) mes;
                var rubricaMes = rubricasMeses.FirstOrDefault(rm => rm.NbAno == ano && rm.CsMes == mesDomain);
                RubricaMesDto rubricaMesDto;
                if (rubricaMes == null)
                {
                    rubricaMesDto = new RubricaMesDto
                    {
                        RubricaId = rubrica.RubricaId,
                        Ano = ano,
                        Mes = mesDomain
                    }; 
                }
                else
                {
                    rubricaMesDto = rubricaMes.ToDto();
                }
                rubricaMesDtos.Add(rubricaMesDto);
            }
            return rubricaMesDtos;
        }

        /// <summary>
        ///     Calcular o valor do orçamento aprovado de um projeto
        /// </summary>
        /// <param name="rubricaMeses">Lista de RubricaMes</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>Valor do OrcamentoAprovado</returns>
        public Decimal CalcularOrcamentoAprovado(List<RubricaMes> rubricaMeses, int ano, int mes)
        {
            return
                rubricaMeses.Where(rubricaMes => rubricaMes.NbAno == ano && (int) rubricaMes.CsMes == mes)
                    .Sum(r => r.NbPlanejado.GetValueOrDefault());
        }

        /// <summary>
        ///     Método para calcular a despesa real de um projeto
        /// </summary>
        /// <param name="rubricaMeses">Lista de RubricaMes</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>Valor da DespesaReal</returns>
        public Decimal? CalcularDespesaReal(List<RubricaMes> rubricaMeses, int ano, int mes)
        {
            IQueryable<RubricaMes> despesaRealPorRubricaMeses = rubricaMeses.AsQueryable()
                .Where(rubricaMes => rubricaMes.NbAno == ano && (int) rubricaMes.CsMes == mes);

            return
                despesaRealPorRubricaMeses.All(dr => dr.NbGasto == null)
                    ? null
                    : despesaRealPorRubricaMeses.Sum(r => r.NbGasto);
        }

        /// <summary>
        ///     Método para calcular Saldo Disponível de um projeto de rubrica administrativa
        /// </summary>
        /// <param name="rubricaMeses">Lista de RubricaMes</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mês</param>
        /// <returns>Valor do Saldo Disponível</returns>
        public Decimal CalcularSaldoDisponivel(List<RubricaMes> rubricaMeses, int ano, int mes)
        {
            decimal saldoDisponivel = rubricaMeses
                .Where(r => r.NbAno < ano || (r.NbAno == ano && (int) r.CsMes < mes))
                .Sum(r => (r.NbPlanejado.GetValueOrDefault() - r.NbGasto.GetValueOrDefault()));

            saldoDisponivel += CalcularOrcamentoAprovado(rubricaMeses, ano, mes);

            return saldoDisponivel;
        }

        /// <summary>
        ///     Método para preencher o DTO de projeto
        /// </summary>
        /// <param name="tipoRubricaId">Id TipoRubrica</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <param name="projetosCustosDto">ProjetoCustosDto</param>
        public void CalcularCustosProjetos(int tipoRubricaId, int ano, int mes, List<CustoProjetoDto> projetosCustosDto)
        {
            foreach (CustoProjetoDto custoProjetoDto in projetosCustosDto)
            {
                List<RubricaMes> rubricaMeses =
                    RubricaMesDao.Instance.ConsultarRubricaMeses(tipoRubricaId,
                        custoProjetoDto.IdProjeto);

                custoProjetoDto.OrcamentoAprovado = CalcularOrcamentoAprovado(rubricaMeses, ano, mes);
                custoProjetoDto.DespesaReal = CalcularDespesaReal(rubricaMeses, ano, mes);
                custoProjetoDto.SaldoDisponivel = CalcularSaldoDisponivel(rubricaMeses, ano, mes);
            }
        }

        /// <summary>
        ///     Consulta valores de um certo mês de todos projetos que possuem Rúbrica do tipo informado.
        /// </summary>
        /// <param name="tipoRubricaId">Tipo da Rúbrica.</param>
        /// <param name="ano">Ano a recuperar.</param>
        /// <param name="mes">Mês a recuperar.</param>
        /// <returns>Lista de detalhes do mês de cada Projeto.</returns>
        public List<CustoProjetoDto> ListarCustosProjetos(int tipoRubricaId, int ano, int mes)
        {
            List<CustoProjetoDto> projetosCustosDto =
                ProjetoDao.Instancia.ConsultarProjetosPorTipoRubrica(tipoRubricaId, ano, mes)
                    .Select(projeto => new CustoProjetoDto
                    {
                        IdProjeto = projeto.Oid,
                        NomeProjeto = projeto.TxNome
                    })
                    .OrderBy(projeto => projeto.NomeProjeto)
                    .ToList();

            CalcularCustosProjetos(tipoRubricaId, ano, mes, projetosCustosDto);

            return projetosCustosDto;
        }

        /// <summary>
        ///     Método para atualizar os gastos relacionados
        /// </summary>
        /// <param name="rubricaId">Id da rubrica</param>
        public void AtualizarDespesaReal(int rubricaId)
        {

            if (rubricaId == 0) return;

            Rubrica rubrica = RubricaDao.Instance.ConsultarRubrica(rubricaId);
            List<RubricaAnoDto> anosRubrica = RubricaBo.Instance.CriarListaAnosRubrica(rubrica);

            foreach (RubricaAnoDto anoRubrica in anosRubrica)
            {
                foreach (RubricaMesDto rubricaMesDto in anoRubrica.Meses)
                {
                    List<NotaFiscal> notasFiscais = NotasFiscaisDao.ConsultarNotasFiscais(rubrica, rubricaMesDto.Ano, (int) rubricaMesDto.Mes).ToList();
                    DefinirDespesaReal(notasFiscais, rubricaMesDto);
                    SalvarRubricaMes(rubricaMesDto);
                }
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="notasFiscais"></param>
        /// <param name="rubricaMesDto"></param>
        private void DefinirDespesaReal(List<NotaFiscal> notasFiscais, RubricaMesDto rubricaMesDto)
        {
            if (notasFiscais.Any())
            {
                rubricaMesDto.Replanejado = null;
                rubricaMesDto.Gasto = notasFiscais.Sum(g => g.Valor);
                rubricaMesDto.PossuiGastosRelacionados = true;
            }
            else if (rubricaMesDto.PossuiGastosRelacionados)
            {
                rubricaMesDto.Gasto = null;
                rubricaMesDto.PossuiGastosRelacionados = false;
            }
        }
    }
}