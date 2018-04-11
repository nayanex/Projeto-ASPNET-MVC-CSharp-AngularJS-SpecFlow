using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Extensions.Entities;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.DAOs.Geral
{
    public class ProjetoDao
    {
		/// <summary>
		/// Singleton
		/// </summary>
        private static ProjetoDao _instancia;
		
		/// <summary>
		/// Singleton
		/// </summary>
        private ProjetoDao()
		{
		}
		
		/// <summary>
		/// Singleton
		/// </summary>
        public static ProjetoDao Instancia
        {
			get { return _instancia ?? (_instancia = new ProjetoDao()); }
        }

        /// <summary>
        /// Lista todos os projetos do Banco de Dados.
        /// </summary>
        /// <returns>Lista de Projetos.</returns>
        public List<Projeto> ListarProjetos()
        {
            List<Projeto> projetos;

            using (var _db = ContextFactoryManager.CriarWexDb())
            {
                projetos = _db.Projetos.Include("ProjetoClientes")
                    .Include("ProjetoClientes.EmpresaInstituicao")
                    .Include("Gerente.Usuario.Person")
                    .Include("CentroCusto")
                    .OrderBy(p => p.TxNome)
                    .ToList();
            }

            return projetos;
        }

        /// <summary>
        /// Recupera projeto por id do Banco de Dados.
        /// </summary>
        /// <param name="projetoOid">Id do projeto a ser recuperado.</param>
        /// <returns>Entidade de projeto com o id passado.</returns>
        public Projeto ConsultarProjetoPorOid(Guid projetoOid, params Expression<Func<Projeto, object>>[] includes)
        {
            Projeto projeto;

            using (var _db = ContextFactoryManager.CriarWexDb())
            {
                projeto = _db.Projetos.MultiploInclude(includes)
                    .FirstOrDefault(o => o.Oid == projetoOid);
            }

            return projeto;
        }

        public Projeto ConsultarProjetoPorOid(Guid projetoOid)
        {
            Projeto projeto;

            using (var contexto = ContextFactoryManager.CriarWexDb())
            {
                projeto = contexto.Projetos
					.MultiploInclude(
						p => p.ProjetoClientes.Select(pc => pc.EmpresaInstituicao),
						p => p.Gerente,
						p => p.CentroCusto
					)
                    .FirstOrDefault(o => o.Oid == projetoOid);

                if (projeto != null && projeto.Gerente != null)
				{
					contexto.Entry(projeto.Gerente).Reference(o => o.Usuario);

					if (projeto.Gerente.Usuario != null)
					{
						contexto.Entry(projeto.Gerente.Usuario).Reference(o => o.Person);
					}
				}
            }

            return projeto;
        }

        /// <summary>
        /// Método responsável por buscar um projeto pelo nome
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="nome">Nome do projeto a ser procurado</param>
        /// <returns>Objeto Projeto</returns>
        public Projeto ConsultarProjetoPorNome(WexDb contexto, string nome)
        {
            return contexto.Projetos.FirstOrDefault(o => o.TxNome.ToUpper() == nome.ToUpper());
        }

        /// <summary>
        /// Consulta todos projetos que possuem Rúbrica do tipo informado.
        /// </summary>
        /// <param name="tipoRubricaId">Tipo da Rúbrica.</param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns>Lista de Projetos.</returns>
        public List<Projeto> ConsultarProjetosPorTipoRubrica(int tipoRubricaId, int ano, int mes, 
        CsProjetoSituacaoDomain situacaoProjeto = CsProjetoSituacaoDomain.EmAndamento)
        {
            using (var contexto = ContextFactoryManager.CriarWexDb())
            {
                return
                    contexto.Projetos.Where(
                        projeto =>
                            projeto.CsSituacaoProjeto == situacaoProjeto &&
                            projeto.Aditivos.Any(
                                aditivo =>
                                    aditivo.Rubricas.Any(rubrica => rubrica.TipoRubricaId == tipoRubricaId)
                                    && (((ano == aditivo.DtInicio.Year && mes >= aditivo.DtInicio.Month) || ano > aditivo.DtInicio.Year)
                                    && ((ano == aditivo.DtTermino.Year && mes <= aditivo.DtTermino.Month) || ano < aditivo.DtTermino.Year))
                                )).ToList();
            }
        }

        /**
         * @summary Cadastrar um novo projeto ou atualiza seus dados
         * */
        public Guid SalvarProjeto(WexDb db, Projeto projeto)
        {
            if (db.Projetos.Existe(p => p.Oid == projeto.Oid))
            {
                db.Entry(projeto).State = EntityState.Modified;
            }
            else
            {
                projeto.Oid = Guid.NewGuid();
                db.Projetos.Add(projeto);
            }

            db.SaveChanges();

            return projeto.Oid;
        }

        /**
         * @summary Consulta o projeto pela chave primária
         * */
        public Projeto ConsultarProjetoPorGuid(WexDb db, Guid IdProjeto)
        {
            Projeto projeto = null;
            
            projeto = db.Projetos.Find(IdProjeto);
                    
            return projeto;
        }

        /**
         * @summary Lista todos os projetos macros
         * */
        public List<Projeto> ListarProjetosMacro(WexDb db)
        {
            List<Projeto> projetosMacros = null;
            projetosMacros = db.Projetos
                .Include("ProjetoClientes")
                .Where(o => o.ProjetoMacro == null)
                .OrderBy(o => o.TxNome)
                .ToList();
            foreach (Projeto p in projetosMacros)
            {
                foreach (ProjetoCliente c in p.ProjetoClientes)
                {
                    c.EmpresaInstituicao.Oid.ToString(); // gambiarra :D
                }
            }
            return projetosMacros;
        }

        /**
         * @summary Lista todos os projetos micros (projetos que estão associados à um projeto macro)
         * @param name="IdProjetoMacro" Identificação do projeto macro
         * */
        public List<Projeto> ListarProjetosMicro(WexDb db, Guid IdProjetoMacro)
        {
            List<Projeto> projetosMicros = null;
            projetosMicros = db.Projetos
                .Include("ProjetoClientes")
                .Where(o => o.ProjetoMacro.Oid == IdProjetoMacro)
                .OrderBy(o => o.TxNome)
                .ToList();
            foreach (Projeto p in projetosMicros)
            {
                foreach (ProjetoCliente c in p.ProjetoClientes)
                {
                    c.EmpresaInstituicao.Oid.ToString(); // gambiarra :D
                }
            }
            return projetosMicros;
        }

        /**
         * @summary Lista todos os projetos macros
         * */
        public bool ExistemProjetosMicros(WexDb db, Guid IdProjeto)
        {
            bool hasProjetos = false;
            hasProjetos = (from p in db.Projetos where p.ProjetoMacro.Oid == IdProjeto select p).Count() > 0;
            return hasProjetos;
        }

        public void AssociarClientes(WexDb db, Projeto projeto, List<Guid> IdsClientes)
        {
            List<ProjetoCliente> clientes = db.ProjetosClientes.Where(o => o.IdProjeto == projeto.Oid).ToList();

            foreach (ProjetoCliente cliente in clientes)
            {
                db.ProjetosClientes.Remove(cliente);
            }

            foreach (Guid IdCliente in IdsClientes)
            {
                ProjetoCliente projetoCliente = new ProjetoCliente()
                {
                    Oid = Guid.NewGuid(),
                    IdProjeto = projeto.Oid,
                    IdCliente = IdCliente
                };
                projeto.ProjetoClientes.Add(projetoCliente);
            }
        }
    }
}
