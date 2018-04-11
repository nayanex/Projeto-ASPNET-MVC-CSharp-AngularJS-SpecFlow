using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.BOs.Custos
{
    public class TipoRubricaBo
    {
        /// <summary>
        ///     Singleton
        /// </summary>
        private static TipoRubricaBo _instance;

        /// <summary>
        ///     Singleton
        /// </summary>
        private TipoRubricaBo()
        {
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static TipoRubricaBo Instance
        {
            get { return _instance ?? (_instance = new TipoRubricaBo()); }
        }

        /// <summary>
        ///     Lista todos os tipos de Rúbricas.
        /// </summary>
        /// <returns>Lista de DTOs de tipo de Rúbrica.</returns>
        public List<TipoRubricaDto> ListarTiposRubricas()
        {
            List<TipoRubrica> tiposRubricas = TipoRubricaDao.Instance.ConsultarTiposRubricas();

            List<TipoRubricaDto> tiposRubricasDto = (from tipoRubrica in tiposRubricas
                select new TipoRubricaDto
                {
                    TipoRubricaId = tipoRubrica.TipoRubricaId,
                    Nome = tipoRubrica.TxNome,
                    Classe = tipoRubrica.CsClasse,
                    PaiId = tipoRubrica.TipoPaiId,
                    Pai = tipoRubrica.TipoPai != null ? tipoRubrica.TipoPai.TxNome : ""
                }).ToList();

            return tiposRubricasDto;
        }

        /// <summary>
        ///     Recupera do Banco de Dados os tipos de Rúbrica pertencentes a um tipo de Projeto, transformando em DTO.
        /// </summary>
        /// <param name="tipoProjetoId">Id do tipo de Projeto dos tipos de Rúbricas a recuperar.</param>
        /// <returns>Lista de DTOs de tipos de Rúbricas.</returns>
        public List<TipoRubricaDto> ListarTiposRubricas(int tipoProjetoId)
        {
            List<TipoRubrica> tiposRubricas = TipoRubricaDao.Instance.ConsultarTiposRubricas(tipoProjetoId);

            List<TipoRubricaDto> tiposRubricasDto = (from tipoRubrica in tiposRubricas
                select new TipoRubricaDto
                {
                    TipoRubricaId = tipoRubrica.TipoRubricaId,
                    Nome = tipoRubrica.TxNome,
                    Classe = tipoRubrica.CsClasse,
                    PaiId = tipoRubrica.TipoPaiId,
                    Pai = tipoRubrica.TipoPai != null ? tipoRubrica.TipoPai.TxNome : ""
                }).ToList();

            return tiposRubricasDto;
        }

        /// <summary>
        ///     Recupera detalhes de um mês de todas os Tipos de Rúbricas de uma Classe de Rúbricas.
        /// </summary>
        /// <param name="classeRubrica">Classe das Rúbricas.</param>
        /// <param name="ano">ano.</param>
        /// <param name="mes">mes.</param>
        /// <returns>Lista de DTOs do detalhe do mês.</returns>
        public List<CustoTipoRubricaDto> ListarCustosTiposRubricas(CsClasseRubrica classeRubrica, int ano, int mes)
        {
            List<TipoRubrica> tiposRubricas = TipoRubricaDao.Instance.ConsultarTiposRubricas(classeRubrica);
            List<RubricaMes> rubricasMeses = RubricaMesDao.Instance.ConsultarRubricaMeses(classeRubrica, ano, mes);

            List<CustoTipoRubricaDto> custosTiposRubricas = ListarCustosTipoRubricas(tiposRubricas, rubricasMeses);

            ProcessarCustosTipoRubricas(custosTiposRubricas, ano, mes);

            return custosTiposRubricas;
        }

        /// <summary>
        /// </summary>
        /// <param name="custosTiposRubricas"></param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        private void ProcessarCustosTipoRubricas(List<CustoTipoRubricaDto> custosTiposRubricas, int ano, int mes)
        {
            foreach (CustoTipoRubricaDto custoTipoRubricaDto in custosTiposRubricas)
            {
                List<CustoProjetoDto> projetos =
                    RubricaMesBo.Instance.ListarCustosProjetos(custoTipoRubricaDto.TipoRubricaId, ano, mes);
                custoTipoRubricaDto.OrcamentoAprovado = CalcularOrcamentoAprovadoRubrica(projetos);
                custoTipoRubricaDto.DespesaReal = CalcularDespesaRealRubrica(projetos);
                custoTipoRubricaDto.SaldoDisponivel = CalcularSaldoDisponivelRubrica(projetos);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="tiposRubricas"></param>
        /// <param name="rubricaMeses"></param>
        /// <returns></returns>
        private List<CustoTipoRubricaDto> ListarCustosTipoRubricas(List<TipoRubrica> tiposRubricas,
            List<RubricaMes> rubricaMeses)
        {
            return (from tipoRubrica in tiposRubricas
                join rubricaMes in rubricaMeses on tipoRubrica.TipoRubricaId equals
                    rubricaMes.Rubrica.TipoRubrica.TipoRubricaId into tipoRubricaMes
                from rmJoined in tipoRubricaMes.DefaultIfEmpty()
                group rmJoined by tipoRubrica
                into trGrupo
                select new CustoTipoRubricaDto
                {
                    TipoRubricaId = trGrupo.Key.TipoRubricaId,
                    Nome = trGrupo.Key.TxNome
                }).OrderBy(tipoRubrica => tipoRubrica.Nome).ToList();
        }

        /// <summary>
        ///     Método para calcular orçamento aprovado dos projetos de um TipoRubrica
        /// </summary>
        /// <param name="custoProjetoDtos"></param>
        /// <returns></returns>
        private Decimal? CalcularOrcamentoAprovadoRubrica(List<CustoProjetoDto> custoProjetoDtos)
        {
            return custoProjetoDtos.Sum(p => p.OrcamentoAprovado);
        }

        /// <summary>
        ///     Método para calcular despesa real dos projetos de uma rubrica
        /// </summary>
        /// <param name="custoProjetoDtos"></param>
        /// <returns></returns>
        private Decimal? CalcularDespesaRealRubrica(List<CustoProjetoDto> custoProjetoDtos)
        {
            return custoProjetoDtos.Sum(p => p.DespesaReal);
        }

        /// <summary>
        ///     Método para calcular saldo disponível dos projetos de uma rubrica
        /// </summary>
        /// <param name="custoProjetoDtos"></param>
        /// <returns></returns>
        private Decimal? CalcularSaldoDisponivelRubrica(List<CustoProjetoDto> custoProjetoDtos)
        {
            return custoProjetoDtos.Sum(p => p.SaldoDisponivel);
        }

        /// <summary>
        ///     Método para preencher os custos das rubricas administrativas
        /// </summary>
        /// <param name="classeRubrica">Classe do tipoRubrica</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>CustoRubricasDto</returns>
        public CustosRubricasDto DetalharCustosTipoRubrica(CsClasseRubrica classeRubrica, int ano, int mes)
        {
            List<CustoTipoRubricaDto> rubricaAdmDto = ListarCustosTiposRubricas(classeRubrica, ano, mes);
            var custoRubrica = new CustosRubricasDto
            {
                TiposRubricas = rubricaAdmDto,
                Total = new CustoValoresDto
                {
                    OrcamentoAprovado = rubricaAdmDto.Sum(r => r.OrcamentoAprovado),
                    DespesaReal = rubricaAdmDto.Sum(r => r.DespesaReal),
                    SaldoDisponivel = rubricaAdmDto.Sum(r => r.SaldoDisponivel)
                }
            };

            return custoRubrica;
        }

    }

}