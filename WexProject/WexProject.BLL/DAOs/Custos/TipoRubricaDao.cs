using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Projeto;

namespace WexProject.BLL.DAOs.Custos
{
    public class TipoRubricaDao
    {
        /// <summary>
        ///     Singleton
        /// </summary>
        private static TipoRubricaDao _instance;

        /// <summary>
        ///     Singleton
        /// </summary>
        private TipoRubricaDao()
        {
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static TipoRubricaDao Instance
        {
            get { return _instance ?? (_instance = new TipoRubricaDao()); }
        }

        /// <summary>
        ///     Retrona lista de tipo de rubricas
        /// </summary>
        /// <returns>Uma lista de tipos de rubricas</returns>
        public List<TipoRubrica> ConsultarTiposRubricas()
        {
            List<TipoRubrica> tiposRubricas;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                tiposRubricas = contexto.TiposRubrica
                    .Include(t => t.TipoPai)
                    .ToList();
            }
            return tiposRubricas;
        }

        /// <summary>
        ///     Recupera do Banco de Dados o tipo de Rúbrica com um determinado Id.
        /// </summary>
        /// <param name="tipoRubricaId">Id do tipo de Rúbrica a recuperar.</param>
        /// <returns>Tipo de Rúbrica.</returns>
        public TipoRubrica ConsultarTipoRubrica(int tipoRubricaId)
        {
            TipoRubrica tipoRubrica;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                tipoRubrica = contexto.TiposRubrica
                    .Include(t => t.TipoProjeto)
                    .Include(t => t.TipoPai)
                    .First(t => t.TipoRubricaId == tipoRubricaId);
            }
            return tipoRubrica;
        }

        /// <summary>
        ///     Recupera do Banco de Dados os tipos de Rúbrica pertencentes a um tipo de Projeto.
        /// </summary>
        /// <param name="tipoProjetoId">Id do tipo de Projeto dos tipos de Rúbricas a recuperar.</param>
        /// <returns>Lista de tipos de Rúbricas.</returns>
        public List<TipoRubrica> ConsultarTiposRubricas(int tipoProjetoId)
        {
            List<TipoRubrica> tiposRubricas;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                tiposRubricas = contexto.TiposRubrica
                    .Include(t => t.TipoPai)
                    .Where(t => t.TipoProjetoId == tipoProjetoId)
                    .ToList();
            }
            return tiposRubricas;
        }

        /// <summary>
        ///     Consulta Tipos de Rúbricas usados em Rúbricas existentes pela Classe da Rúbrica.
        /// </summary>
        /// <param name="classeRubrica">Classe de Rúbrica a pesquisar.</param>
        /// <param name="classeProjeto">Classe do Projeto a pesquisar. Padrao: CsClasseProjeto.Patrocinado</param>
        /// <returns>Lista de Tipos de Rúbricas usados em Rúbricas existentes.</returns>
        public List<TipoRubrica> ConsultarTiposRubricas(CsClasseRubrica classeRubrica,
            CsClasseProjeto classeProjeto = CsClasseProjeto.Patrocinado)
        {
            List<TipoRubrica> tiposRubricas;
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                tiposRubricas = contexto.TiposRubrica
                    .Include(t => t.TipoProjeto)
                    .Include(t => t.TipoProjeto.ClasseProjeto)
                    .Where(t => t.CsClasse == classeRubrica && t.TipoProjeto.ClasseProjetoId == (int)classeProjeto)
                    .ToList();
            }
            return tiposRubricas;
        }

        /// <summary>
        /// </summary>
        /// <param name="tipoRubrica"></param>
        /// <returns></returns>
        public int SalvarTipoRubrica(TipoRubrica tipoRubrica)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                contexto.TiposRubrica.AddOrUpdate(tipoRubrica);
                contexto.SaveChanges();
            }
            return tipoRubrica.TipoRubricaId;
        }

    }

}