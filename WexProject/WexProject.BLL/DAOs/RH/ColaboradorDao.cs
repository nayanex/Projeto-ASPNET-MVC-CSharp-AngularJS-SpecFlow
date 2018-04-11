using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WexProject.BLL.BOs.RH;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Extensions.Entities;
using WexProject.BLL.Shared.DTOs.Rh;

namespace WexProject.BLL.DAOs.RH
{
    public class ColaboradorDAO
    {
		private static ColaboradorDAO instancia;

		private ColaboradorDAO()
		{
		}

		public static ColaboradorDAO Instancia
		{
			get { return instancia ?? (instancia = new ColaboradorDAO()); }
		}
		
        /// <summary>
        /// Selecionar um colaborador por login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static Colaborador ConsultarColaborador( string login , params Expression<Func<Colaborador,object>>[] includes)
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                User usuario = contexto.Usuario.FirstOrDefault( o => o.UserName == login );
                if(usuario == null)
                    return null;

                return contexto.Colaboradores.MultiploInclude(includes).FirstOrDefault( o => o.OidUsuario == usuario.Oid );
            }
        }

		public Colaborador ConsultarColaboradorPorMatricula(String matricula)
		{
			Colaborador colaborador;

			using (var _db = new WexDb())
			{
				colaborador = (from c in _db.Colaboradores
							   where c.TxMatricula.Contains(matricula)
							   select c).First();
			}

			return colaborador;
		}

        /// <summary>
        /// Selecionar varios colaboradores pelo login em uma coleção de logins
        /// </summary>
        /// <param name="logins"></param>
        /// <returns></returns>
        public static List<Colaborador> ConsultarColaboradores( ICollection<string> logins, params Expression<Func<Colaborador,object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.Colaboradores.MultiploInclude(includes).Where( o => logins.Contains( o.Usuario.UserName ) ).ToList();
            }
        }

        /// <summary>
        /// Selecionar um colaborador pelo oid
        /// </summary>
        /// <param name="oidColaborador"></param>
        /// <returns></returns>
        public static Colaborador ConsultarColaborador( Guid oidColaborador, params Expression<Func<Colaborador,object>>[] includes)
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.Colaboradores.MultiploInclude(includes).FirstOrDefault( o => o.Oid == oidColaborador );
            }
        }

        /// <summary>
        /// Método responsável por resgatar uma role default
        /// <param name="nome">nome da role default(pode passar o nome como "Default")</param>
        /// </summary>
        public static Role ConsultarRolePorNome( string nome )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                Role role = contexto.Roles.Include( a => a.RoleBase ).FirstOrDefault( o => o.RoleBase.Name == nome );
                return role;
            }
        }

        /// <summary>
        /// Método responsável por buscar todos os colaboradores.
        /// </summary>
        /// <returns>Lista de Colaboradores</returns>
        public static List<Colaborador> ConsultarColaboradores()
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                List<Colaborador> colaboradores = contexto.Colaboradores.Include( o => o.Usuario ).Include( o => o.Usuario.Person ).OrderBy( o => o.Usuario.Person.FirstName + o.Usuario.Person.LastName ).ToList();
                return colaboradores; 
            }
        }

        /// <summary>
        /// Método responsável por consultar os colaboradores e transformá-los em Dto para utilizar na serialização do serviço.
        /// </summary>
        /// <param name="logins">logins para serem pesquisados</param>
        /// <returns>Lista dos colaboradores em Dto</returns>
        public static List<ColaboradorDto> ConsultarColaboradoresDto( ICollection<string> logins )
        {
            List<ColaboradorDto> colaboradoresDto = new List<ColaboradorDto>();
            List<Colaborador> colaboradores = ColaboradorDAO.ConsultarColaboradores( logins );

            for(int i = 0; i < colaboradores.Count; i++)
            {
                colaboradoresDto.Add( ColaboradorBo.DtoFactory( colaboradores[i] ) );
            }

            return colaboradoresDto;
        }

        #region Dao Web

        public Colaborador ConsultarColaboradorPorGuid(WexDb db, Guid IdColaborador)
        {
            return db.Colaboradores
                .Include(o => o.Usuario.Person)
                .Include(o => o.Cargo)
                .FirstOrDefault(o => o.Oid == IdColaborador);
        }

        public List<Colaborador> ListarGerentes(WexDb db)
        {
            List<Colaborador> gerentes;
            gerentes = db.Colaboradores
                .Include(o => o.Usuario.Person)
                .Include(o => o.Cargo)
                .Where(o => o.Cargo.TxDescricao.ToUpper().Contains("GERENTE DE PROJETO"))
                .OrderBy(o => o.Usuario.Person.FirstName)
                .ToList();
            return gerentes;

        }

        #endregion
    }
}
