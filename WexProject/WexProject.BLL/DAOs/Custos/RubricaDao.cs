using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WexProject.BLL.BOs.TotvsWex;
using WexProject.BLL.Contexto;
using WexProject.BLL.Extensions.Entities;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;

namespace WexProject.BLL.DAOs.Custos
{
    public class RubricaDao
    {

        /// <summary>
        /// Singleton
        /// </summary>
        private static RubricaDao _instance;

        /// <summary>
        /// Singleton
        /// </summary>
        private RubricaDao()
        {
        }

        /// <summary>
        /// Singleton
        /// </summary>
        public static RubricaDao Instance
        {
            get { return _instance ?? (_instance = new RubricaDao()); }
        }

        /// <summary>
        ///     Consulta uma Rubrica por seu id.
        /// </summary>
        /// <param name="rubricaId">Id da Rubrica.</param>
        /// <returns>Rubrica recuperada do Banco de Dados.</returns>
        public Rubrica ConsultarRubrica(int rubricaId)
        {
            Rubrica rubrica;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubrica = contexto.Rubricas
                    .MultiploInclude(
                        r => r.Filhos,
                        r => r.RubricaMeses,
                        r => r.Aditivo,
                        r => r.TipoRubrica
                    )
                    .Single(r => r.RubricaId == rubricaId);
            }

            return rubrica;
        }

        /// <summary>
        ///     Consulta uma Rubrica de um Aditivo por Tipo de Rubrica.
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo.</param>
        /// <param name="tipoRubricaId">Id do Tipo de Rubrica.</param>
        /// <returns>Rubrica recuperada do Banco de Dados.</returns>
        public Rubrica ConsultarRubrica(int aditivoId, int tipoRubricaId)
        {
            Rubrica rubricaPai;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricaPai = contexto.Rubricas
                    .FirstOrDefault(r => r.AditivoId == aditivoId
                                         && r.TipoRubricaId == tipoRubricaId);
            }

            return rubricaPai;
        }

        /// <summary>
        ///     Consulta Rubricas de um Aditivo.
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo.</param>
        /// <returns>Lista de Rubricas recuperadas do Banco de Dados.</returns>
        public List<Rubrica> ConsultarRubricas(int aditivoId)
        {
            List<Rubrica> rubricas;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricas = contexto.Rubricas
                    .MultiploInclude(
                        r => r.Filhos,
                        r => r.RubricaMeses,
                        r => r.Aditivo,
                        r => r.TipoRubrica
                    )
                    .Where(r => r.AditivoId == aditivoId)
                    .ToList();
            }

            return rubricas;
        }

        /// <summary>
        ///     Consulta Rubricas de um Aditivo por Classe de Rubrica.
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo.</param>
        /// <param name="classeRubrica">Classe de Rubrica.</param>
        /// <returns>Lista de Rubricas recuperadas do Banco de Dados.</returns>
        public List<Rubrica> ConsultarRubricas(int aditivoId, CsClasseRubrica classeRubrica)
        {
            List<Rubrica> rubricas;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricas = contexto.Rubricas
                    .MultiploInclude(
                        r => r.Filhos,
                        r => r.RubricaMeses,
                        r => r.Aditivo,
                        r => r.TipoRubrica
                    )
                    .Where(r => r.AditivoId == aditivoId)
                    .Where(r => (r.TipoRubrica.CsClasse & classeRubrica) > 0)
                    .ToList();
            }

            return rubricas;
        }

		/// <summary>
		///     Consulta Rubricas de Desenvolvimento 
		///     e que não sejam rubricas de Recursos Humanos de um Aditivo.
		/// </summary>
		/// <param name="aditivoId">Id do Aditivo.</param>
		/// <param name="classeRubrica">Classe de Rubrica.</param>
		/// <returns>Lista de Rubricas recuperadas do Banco de Dados.</returns>
		public List<Rubrica> ConsultarRubricasNotasFiscais(int aditivoId, CsClasseRubrica classeRubrica)
		{
			List<Rubrica> rubricas;

			using (WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				rubricas = contexto.Rubricas
					.MultiploInclude(
						r => r.Filhos,
						r => r.RubricaMeses,
						r => r.Aditivo,
						r => r.TipoRubrica
					)
					.Where(r => r.AditivoId == aditivoId)
					.Where(r => (r.TipoRubrica.CsClasse & classeRubrica) > 0)
					.Where(r => (r.TipoRubrica.CsClasse & CsClasseRubrica.RecursosHumanos) == 0)
					.ToList();
			}

			return rubricas;
		}

        /// <summary>
        /// Método para consultar rubricas dentro do periodo dos aditivos do projeto
        /// </summary>
        /// <param name="centroCustoId">Id do centroCusto</param>
        /// <param name="tipoRubricaId">Id do tipoRubricaId</param>
        /// <param name="ano">Ano</param>
        /// <param name="mes">Mes</param>
        /// <returns>Lista de Rubricas</returns>
		public List<Rubrica> ConsultarRubricas(Guid projetoId, int tipoRubricaId, int ano, int mes)
        {
            List<Rubrica> rubricas;

            using (var db = ContextFactoryManager.CriarWexDb())
            {
                rubricas = db.Rubricas
                            .Where(r => r.TipoRubricaId == tipoRubricaId)
							.Where(r => r.Aditivo.ProjetoOid == projetoId)
                            .Where(r => 
                                (((ano == r.Aditivo.DtInicio.Year && mes >= r.Aditivo.DtInicio.Month) 
                                || ano > r.Aditivo.DtInicio.Year)
                                && ((ano == r.Aditivo.DtTermino.Year && mes <= r.Aditivo.DtTermino.Month) 
                                || ano < r.Aditivo.DtTermino.Year)))
                            .ToList();
            }

            return rubricas;
        }

        /// <summary>
        ///     Consulta Rubricas por Centro de Custo e Tipo de Rubrica.
        /// </summary>
        /// <param name="centroCustoId">Id do Centro de Custo.</param>
        /// <param name="tipoRubricaId">Id do Tipo de Rubrica.</param>
        /// <returns>Lista de Rubricas recuperadas do Banco de Dados.</returns>
        public List<Rubrica> ConsultarRubricas(int centroCustoId, int tipoRubricaId)
        {
            List<Rubrica> rubricas;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                rubricas = contexto.Rubricas
                    .Where(r => r.TipoRubricaId == tipoRubricaId)
                    .Where(r => r.Aditivo.Projeto.CentroCustoId == centroCustoId)
                    .OrderBy(r => r.Aditivo.DtInicio)
                    .ToList();
            }

            return rubricas;
        }

        /// <summary>
        ///     Adiciona ou atualiza rubrica no banco
        /// </summary>
        /// <param name="rubrica">Objeto rubrica a ser adicionado ou atualizado</param>
        /// <returns>Id da rubrica no banco</returns>
        public int SalvarRubrica(Rubrica rubrica)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if (rubrica.RubricaId == 0)
                {
                    contexto.Rubricas.Add(rubrica);
                }
                else
                {
                    contexto.Entry(rubrica).State = EntityState.Modified;
                }

                contexto.SaveChanges();
            }

            return rubrica.RubricaId;
        }

        /// <summary>
        ///     Remove rubrica do banco
        /// </summary>
        /// <param name="rubricaId">Id da rubrica</param>
        /// <param name="filhos">Lista de Rúbricas filhas da Rúbrica a ser deletada.</param>
        /// <returns>Id da rubrica removida</returns>
        public int RemoverRubrica(int rubricaId, out List<int> filhos)
        {

            Rubrica rubrica;

            filhos = new List<int>();

            using (var contexto = ContextFactoryManager.CriarWexDb())
            {

                rubrica = contexto.Rubricas.Find(rubricaId);

                foreach (var rubricaFilha in rubrica.Filhos.ToList())
                {
                    filhos.Add(rubricaFilha.RubricaId);
                    contexto.Rubricas.Remove(rubricaFilha);
                }

                contexto.Rubricas.Remove(rubrica);

                contexto.SaveChanges();

            }

            return rubrica.RubricaId;

        }
    }
}