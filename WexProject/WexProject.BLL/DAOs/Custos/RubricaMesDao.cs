using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.Contexto;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.DAOs.Custos
{
    public class RubricaMesDao
    {
        /// <summary>
        ///     Singleton
        /// </summary>
        private static RubricaMesDao _instance;

        /// <summary>
        ///     Singleton
        /// </summary>
        private RubricaMesDao()
        {
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static RubricaMesDao Instance
        {
            get { return _instance ?? (_instance = new RubricaMesDao()); }
        }

        /// <summary>
        /// </summary>
        /// <param name="rubricaMesId"></param>
        /// <returns></returns>
        public RubricaMes ConsultarRubricaMes(int rubricaMesId)
        {
            RubricaMes rubricaMes;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricaMes = contexto.RubricaMeses.FirstOrDefault(rm => rm.RubricaMesId == rubricaMesId);
            }
            return rubricaMes;
        }

        /// <summary>
        ///     Adiciona ou atualiza o detalhes de um mês de uma rubrica no banco
        /// </summary>
        /// <param name="rubricaMes">Objeto RubricaMes a ser adicionado ou atualizado</param>
        /// <returns>Id dos detalhes do mês da rubrica no banco</returns>
        public int SalvarRubricaMes(RubricaMes rubricaMes)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if (rubricaMes.RubricaMesId == 0)
                {
                    contexto.RubricaMeses.Add(rubricaMes);
                }
                else
                {
                    contexto.Entry(rubricaMes).State = EntityState.Modified;
                }

                contexto.SaveChanges();
            }

            return rubricaMes.RubricaMesId;
        }

        /// <summary>
        ///     Retorna os detalhes dos meses da rubrica
        /// </summary>
        /// <param name="rubricaId">Id da rubrica</param>
        /// <returns>Uma lista de RubricaMesDto</returns>
        public List<RubricaMesDto> ConsultarRubricaMeses(int rubricaId)
        {
            List<RubricaMesDto> rubricaMeses;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricaMeses = (from rm in contexto.RubricaMeses
                    where rm.RubricaId == rubricaId
                    select new RubricaMesDto
                    {
                        RubricaMesId = rm.RubricaMesId,
                        RubricaId = rm.RubricaId,
                        Mes = rm.CsMes,
                        Ano = rm.NbAno,
                        PossuiGastosRelacionados = rm.PossuiGastosRelacionados,
                        Planejado = rm.NbPlanejado,
                        Replanejado = rm.NbReplanejado,
                        Gasto = rm.NbGasto
                    }).ToList();
            }

            return rubricaMeses;
        }

        /// <summary>
        ///     Retorna os detalhes dos meses da rubrica
        /// </summary>
        /// <param name="rubrica">Rubrica</param>
        /// <returns>Uma lista de RubricaMesDto</returns>
        public List<RubricaMes> ConsultarRubricaMeses(Rubrica rubrica)
        {
            using (var contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.RubricaMeses.Where(rubricaMes => rubricaMes.RubricaId == rubrica.RubricaId).ToList();
            }
        }

        /// <summary>
        ///     Recupera todas as rubricas de projeto por id
        /// </summary>
        /// <param name="projetoOid">Id do projeto a ser recuperado</param>
        /// <returns>Dto de rubricas por mês de um projeto</returns>
        public List<RubricaMes> ConsultarRubricaMeses(Guid projetoOid)
        {
            List<RubricaMes> rubricaMeses;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricaMeses = contexto.RubricaMeses
                    .Include(rm => rm.Rubrica)
                    .Include(rm => rm.Rubrica.TipoRubrica)
                    .Where(rm => rm.Rubrica.Aditivo.ProjetoOid == projetoOid)
                    .ToList();
            }

            return rubricaMeses;
        }

        /// <summary>
        /// </summary>
        /// <param name="centroCustoId"></param>
        /// <param name="tipoRubricaId"></param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public List<RubricaMes> ConsultarRubricaMeses(int centroCustoId, int tipoRubricaId, int aditivoId, int ano, int mes)
        {
            List<RubricaMes> rubricaMeses;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricaMeses = contexto.RubricaMeses
                    .Include(rm => rm.Rubrica)
                    .Include(rm => rm.Rubrica.Aditivo)
                    .Where(rm => rm.Rubrica.TipoRubricaId == tipoRubricaId)
                    .Where(rm => rm.Rubrica.Aditivo.Projeto.CentroCustoId == centroCustoId)
					.Where(rm => rm.Rubrica.AditivoId == aditivoId)
                    .Where(rm => rm.NbAno == ano)
                    .Where(rm => (int) rm.CsMes == mes)
                    .ToList();
            }
            return rubricaMeses;
        }

        /// <summary>
        ///     Recupera detalhes de um mês de todas as Rúbricas de uma Classe de Rúbricas.
        /// </summary>
        /// <param name="classeRubrica">Classe de Rúbricas.</param>
        /// <param name="ano">ano da rubrica a ser recuperado</param>
        /// <param name="mes">mes da rubrica a ser recuperado</param>
        /// <returns>Rubricas por mês</returns>
        public List<RubricaMes> ConsultarRubricaMeses(CsClasseRubrica classeRubrica, int ano, int mes)
        {
            List<RubricaMes> rubricaMeses;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricaMeses = contexto.RubricaMeses
                    .Include(rm => rm.Rubrica)
                    .Include(rm => rm.Rubrica.TipoRubrica)
                    .Where(rm => rm.Rubrica.TipoRubrica.CsClasse == classeRubrica)
                    .Where(rm => rm.NbAno == ano)
                    .Where(rm => rm.CsMes == (CsMesDomain) mes)
                    .ToList();
            }

            return rubricaMeses;
        }

        /// <summary>
        ///     Retornar lista de rubrica meses de um projeto e um tipoRubrica
        /// </summary>
        /// <param name="tipoRubricaId">TipoRubricaId</param>
        /// <param name="oidProjeto">IdProjeto</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>Lista de RubricaMes</returns>
        public List<RubricaMes> ConsultarRubricaMeses(int tipoRubricaId, Guid oidProjeto, int ano, int mes)
        {
            List<RubricaMes> rubricasMeses;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricasMeses = contexto.RubricaMeses
                    .Include(rm => rm.Rubrica)
                    .Include(rm => rm.Rubrica.TipoRubrica)
                    .Include(rm => rm.Rubrica.Aditivo)
                    .Where(rm => rm.Rubrica.TipoRubricaId == tipoRubricaId)
                    .Where(rm => rm.Rubrica.Aditivo.ProjetoOid == oidProjeto)
                    .Where(rm => rm.NbAno == ano)
                    .Where(rm => rm.CsMes == (CsMesDomain) mes)
                    .ToList();
            }
            return rubricasMeses;
        }

        /// <summary>
        /// Método para consultar as rubricasMeses no período dos aditivos do projeto
        /// </summary>
        /// <param name="projetoId">Id do projeto</param>
        /// <param name="tipoRubricaId">Id do tipoRubrica</param>
        /// <param name="centroCustoId">Id do centroCusto</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>Lista de RubricaMeses</returns>
        public List<RubricaMes> ConsultarRubricaMeses(Guid projetoId, int tipoRubricaId, int ano, int mes)
        {
            List<RubricaMes> rubricaMeses;

            using (var db = ContextFactoryManager.CriarWexDb())
            {
                rubricaMeses = (from rm in db.RubricaMeses
                                where rm.Rubrica.Aditivo.ProjetoOid == projetoId &&
                                rm.Rubrica.TipoRubricaId == tipoRubricaId &&
                                (((rm.Rubrica.Aditivo.DtInicio.Year == ano && mes >= rm.Rubrica.Aditivo.DtInicio.Month) 
                                || ano > rm.Rubrica.Aditivo.DtInicio.Year)
                                && ((ano == rm.Rubrica.Aditivo.DtTermino.Year && mes <= rm.Rubrica.Aditivo.DtTermino.Month) 
                                || ano < rm.Rubrica.Aditivo.DtTermino.Year))
                                && rm.NbAno == ano && rm.CsMes == (CsMesDomain)mes
                                select rm).ToList();                                                       
            }

            return rubricaMeses;

        }

        /// <summary>
        ///     Verifica se um dado mês faz parte da rubrica e retorna o DTO referente aos dados do mês no banco caso faça
        /// </summary>
        /// <param name="rubrica">Rubrica a verificar</param>
        /// <param name="mes">Mês a verificar</param>
        /// <param name="ano">Ano a verificar</param>
        /// <returns>RubricaMesDto caso faça parte, null caso contrário</returns>
        public RubricaMes ConsultarRubricaMes(Rubrica rubrica, int mes, int ano)
        {
			// TODO: Podemos garantir que não seja preciso fazer isso?
			using (WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				contexto.Rubricas.Attach(rubrica);
				contexto.Entry(rubrica).Collection(r => r.RubricaMeses).Load();
				contexto.Entry(rubrica).Reference(r => r.TipoRubrica).Load();
			}

			return (from rm in rubrica.RubricaMeses
					where rm.NbAno == ano && rm.CsMes == (CsMesDomain)mes
					select rm).FirstOrDefault();
        }

        /// <summary>
        /// </summary>
        /// <param name="tipoRubricaId"></param>
        /// <param name="oidProjeto"></param>
        /// <returns></returns>
        public List<RubricaMes> ConsultarRubricaMeses(int tipoRubricaId, Guid oidProjeto)
        {
            List<RubricaMes> rubricaMeses = new List<RubricaMes>();
            using (var contexto = ContextFactoryManager.CriarWexDb())
            {
                List<Aditivo> aditivos = contexto.Aditivos.Where(aditivo => aditivo.ProjetoOid == oidProjeto).ToList();

                foreach (Aditivo aditivo in aditivos)
                {
                    Func<RubricaMes, bool> verificarData =
                        rm =>
                            new DateTime(rm.NbAno, (int) rm.CsMes, 1) >=
                            new DateTime(aditivo.DtInicio.Year, aditivo.DtInicio.Month, 1) &&
                            new DateTime(rm.NbAno, (int) rm.CsMes, 1) <=
                            new DateTime(aditivo.DtTermino.Year, aditivo.DtTermino.Month, 1);

                    List<RubricaMes> rubricasMesesFiltrados = contexto.RubricaMeses
                        .Where(
                            rm => rm.Rubrica.TipoRubricaId == tipoRubricaId && rm.Rubrica.AditivoId == aditivo.AditivoId)
                        .Where(verificarData)
                        .ToList();
                    rubricaMeses.AddRange(rubricasMesesFiltrados);
                }

            }
            return rubricaMeses;
        }

        /// <summary>
        /// Remove a entidade do banco de dados
        /// </summary>
        /// <param name="rubricaMes"></param>
        public void RemoverRubricaMes(RubricaMes rubricaMes)
        {
            using (var contexto = ContextFactoryManager.CriarWexDb())
            {
                contexto.RubricaMeses.Remove(rubricaMes);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a entidade do banco de dados
        /// </summary>
        /// <param name="rubricaMesId"></param>
        public void RemoverRubricaMes(int rubricaMesId)
        {
            using (var contexto = ContextFactoryManager.CriarWexDb())
            {
                var rubricaMes = contexto.RubricaMeses.First(rm => rm.RubricaMesId == rubricaMesId);
                contexto.RubricaMeses.Remove(rubricaMes);
                contexto.SaveChanges();
            }
        }

    }

}