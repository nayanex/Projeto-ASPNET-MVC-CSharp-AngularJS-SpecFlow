using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using WexProject.BLL.Shared.DTOs.Geral;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.BOs.Geral;

namespace WexProject.BLL.Models.Geral
{
    [OptimisticLocking( false )]
    public class ProjetoColaboradorConfig : BaseObject 
    {
        /// <summary>
        /// Colaborador
        /// </summary>
        protected Colaborador colaborador;

        /// <summary>
        /// Atributo de Cor
        /// </summary>
        protected String cor;
        /// Última cor selecionada para determinado cronograma de um projeto.
        /// </summary>
        [Required]
        public String Cor
        {
            get
            {
                return cor;
            }
            set
            {
                SetPropertyValue<String>( "Cor", ref cor, value );
            }

        }

        /// <summary>
        /// Associação com o Colaborador
        /// </summary>
        [Required]
        public Colaborador Colaborador
        {
            get
            {
                return colaborador;
            }
            set
            {
                SetPropertyValue<Colaborador>( "Colaborador", ref colaborador, value );
            }
        }
        /// <summary>
        /// Atributo de projeto
        /// </summary>
        private Projeto projeto;

        /// <summary>
        /// Associação com o projeto
        /// </summary>
        [Required]
        public Projeto Projeto
        {
            get
            {
                return projeto;
            }
            set
            {
                SetPropertyValue<Projeto>( "Projeto", ref projeto, value );
            }
        }

        #region Regras de Negócio

        /// <summary>
        /// Método responsável por escolher a cor do usuário em um determinado cronograma de um projeto.
        /// </summary>
        public static string RnEscolherCor( Session session, Guid oidColaborador, Guid oidProjeto )
        {
            Projeto projeto = Projeto.GetProjetoPorOid( session, oidProjeto );
            if(projeto == null)
                throw new Exception( "A cor não pode ser selecionada, o projeto selecionado não existe!" );

            Colaborador colaborador = Colaborador.GetColaboradorPorOid( session, oidColaborador );
            if(colaborador == null)
                throw new Exception( "A cor não pode ser selecionada, o colaborador selecionado não existe!" );

            ProjetoColaboradorConfig config = ProjetoColaboradorConfig.GetProjetoColaboradorConfig( session, oidColaborador, oidProjeto );
            string cor = string.Empty;
            if(config != null)
            {
                cor = config.Cor;
            }
            else
            {
                ProjetoColaboradorConfig colaboradorConfig = new ProjetoColaboradorConfig( session );
                //List<ProjetoColaboradorConfig> configs = GetConfigColaboradores( session, oidProjeto );
                //List<string> coresProjeto = new List<string>( configs.Select( o => o.cor ) );
                List<string> coresProjeto = GetCoresPorProjeto( session, oidProjeto );
                cor = ColaboradorConfigBo.SelecionarCor( coresProjeto );
                colaboradorConfig.Projeto = projeto;
                colaboradorConfig.Colaborador = colaborador;
                colaboradorConfig.Cor = cor;
                colaboradorConfig.Save();
            }
            return cor;
        }

        public static string RnEscolherCor( Session session, string login, Guid oidProjeto )
        {
            Projeto projeto = Projeto.GetProjetoPorOid( session, oidProjeto );
            if(projeto == null)
                throw new Exception( "A cor não pode ser selecionada, o projeto selecionado não existe!" );

            Colaborador colaborador = Colaborador.GetColaboradorPorLogin( session, login );
            if(colaborador == null)
                throw new Exception( "A cor não pode ser selecionada, o colaborador selecionado não existe!" );

            ProjetoColaboradorConfig config = ProjetoColaboradorConfig.GetProjetoColaboradorConfig( session, login, oidProjeto );
            string cor = string.Empty;
            if(config != null)
            {
                cor = config.Cor;
            }
            else
            {
                ProjetoColaboradorConfig colaboradorConfig = new ProjetoColaboradorConfig( session );
                List<string> coresProjeto = GetCoresPorProjeto( session, oidProjeto );
                cor = ColaboradorConfigBo.SelecionarCor( coresProjeto );
                colaboradorConfig.Projeto = projeto;
                colaboradorConfig.Colaborador = colaborador;
                colaboradorConfig.Cor = cor;
                colaboradorConfig.Save();
            }

            return cor;
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Método responsável por resgatar o ProjetoColaboradorConfig de um colaborador em um determinado projeto
        /// </summary>
        /// <param name="session">sessçao do banco</param>
        /// <param name="oidColaborador">oid do colaborador atual</param>
        /// <param name="oidProjeto">oid do projeto atual</param>
        /// <returns></returns>
        public static ProjetoColaboradorConfig GetProjetoColaboradorConfig( Session session, Guid oidColaborador, Guid oidProjeto )
        {
            if(session == null || oidColaborador == new Guid() || oidProjeto == new Guid())
                throw new ArgumentException( "Parâmetros Sessão, OidColaborador e OidProjeto não devem ser vazios." );

            return session.FindObject<ProjetoColaboradorConfig>( CriteriaOperator.Parse( " Projeto = ? AND Colaborador = ?", oidProjeto, oidColaborador ) );
        }

        /// <summary>
        /// Método responsável por buscar cor do colaborador a partir do login do usuário
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="login">Login do usuário</param>
        /// <returns>Objeto ProjetoColaboradorConfig</returns>
        public static ProjetoColaboradorConfig GetProjetoColaboradorConfig( Session session, string login, Guid oidProjeto )
        {
            if(session == null || String.IsNullOrWhiteSpace( login ))
                throw new ArgumentException( "Parâmetros Sessão, OidColaborador não devem ser nulos." );

            return session.FindObject<ProjetoColaboradorConfig>( CriteriaOperator.Parse( "Colaborador.Usuario.UserName = ? AND Projeto = ?", login, oidProjeto ) );
        }

        /// <summary>
        /// Método responsável por buscar a cor e nome de um colaborador a partir do seu Login(UserName)
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="login">UserName do colaborador (Login)</param>
        /// <returns>Objeto DTO</returns>
        public static ProjetoColaboradorConfigDto GetProjetoColaboradorConfigDto( Session session, string login, Guid oidProjeto )
        {
            ProjetoColaboradorConfig projetoConfig = GetProjetoColaboradorConfig( session, login, oidProjeto );
            if(projetoConfig == null)
                return null;
            return projetoConfig.DtoFactory();
        }

        /// <summary>
        /// Método responsável por buscar a cor e nome de um colaborador a partir do seu Login(UserName)
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="login">UserName do colaborador (Login)</param>
        /// <returns>Objeto DTO</returns>
        public static ProjetoColaboradorConfigDto GetProjetoColaboradorConfigDto( Session session, Guid oidColaborador, Guid oidProjeto )
        {
            ProjetoColaboradorConfig projetoConfig = GetProjetoColaboradorConfig( session, oidColaborador, oidProjeto );
            if(projetoConfig == null)
                return null;
            return projetoConfig.DtoFactory();
        }

        /// <summary>
        /// Método responsável por resgatar as cores cadastradas de um determinado projeto.
        /// </summary>
        public static List<string> GetCoresPorProjeto( Session session, Guid oidProjeto )
        {
            if(session == null || oidProjeto == new Guid())
                throw new ArgumentException( "Parâmetros Sessão, oidProjeto não devem ser nulos." );
            List<string> cores = new List<string>();
            //pesquisa as cores que já existem cadastradas no banco.
            using(XPCollection<ProjetoColaboradorConfig> xpCores = new XPCollection<ProjetoColaboradorConfig>(session,
                CriteriaOperator.Parse( "Projeto = ?", oidProjeto ) ))
            {
                if(xpCores.Count >= 0)
                {
                    cores = xpCores.Select( o => o.Cor ).ToList();
                }
            }
            return cores;
        }

        /// <summary>
        /// Método responsável por buscar as cores a partir de uma lista de usernames (Logins) passada.
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="logins">Lista com logins dos usuários</param>
        /// <returns>Lista de objetos ProjetoColaboradorCondfig</returns>
        public static List<ProjetoColaboradorConfig> GetConfigColaboradores( Session session, List<string> logins, Guid oidProjeto )
        {
            if(session == null || logins == null)
                throw new ArgumentException( "Os parâmetros session e login usuários não podem ser nulos." );
            List<ProjetoColaboradorConfig> configs = new List<ProjetoColaboradorConfig>();
            using(XPCollection<ProjetoColaboradorConfig> colecaoConfigs = new XPCollection<ProjetoColaboradorConfig>(session ,GroupOperator.And( new InOperator( "Colaborador.Usuario.UserName", logins ), CriteriaOperator.Parse( "OidProjeto = ?", oidProjeto ) ) ))
            {
                configs = colecaoConfigs.ToList();
            }
            return configs;
        }

        /// <summary>
        /// Método resposável por trazer todas os ProjetosColaboradorConfig de um projeto
        /// </summary>
        /// <param name="session">sessão do banco</param>
        /// <param name="oidProjeto">oid do projeto atual</param>
        /// <returns>lista de configs armazenadas para o projeto</returns>
        public static List<ProjetoColaboradorConfig> GetConfigColaboradores( Session session, Guid oidProjeto )
        {
            if(session == null || oidProjeto == null || oidProjeto == new Guid())
                throw new ArgumentException( "Os parâmetros session e oid do projeto não podem ser nulos." );

            List<ProjetoColaboradorConfig> configs = new List<ProjetoColaboradorConfig>();
            using(XPCollection<ProjetoColaboradorConfig> colecaoConfigs = new XPCollection<ProjetoColaboradorConfig>( session,CriteriaOperator.Parse( "Projeto = ?", oidProjeto ) ))
            {
                configs = colecaoConfigs.ToList();
            }
            return configs;
        }

        /// <summary>
        /// Método responsável por buscar as cores a partir de uma lista de usernames (Logins) passados.
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="logins">Lista de Logins dos Usuários</param>
        /// <returns>Lista de Objetos Dtos de ProjetoColaboradorConfig</returns>
        public static List<ProjetoColaboradorConfigDto> GetCoresPorLoginsDto( Session session, string[] logins,Guid oidProjeto )
        {
            List<ProjetoColaboradorConfigDto> colaboradoresConfigsDto = new List<ProjetoColaboradorConfigDto>();
            List<ProjetoColaboradorConfig> colaboradoresConfigs = new List<ProjetoColaboradorConfig>();

            colaboradoresConfigs = ProjetoColaboradorConfig.GetConfigColaboradores( session, logins.ToList(),oidProjeto );

            if(colaboradoresConfigs == null || colaboradoresConfigs.Count <= 0)
                return colaboradoresConfigsDto;

            foreach(ProjetoColaboradorConfig colaboradorConfig in colaboradoresConfigs)
            {
                ProjetoColaboradorConfigDto configDto = colaboradorConfig.DtoFactory();
                colaboradoresConfigsDto.Add( configDto );
            }

            return colaboradoresConfigsDto;
        }


        #endregion

        #region Factories

        /// <summary>
        /// Método responsável por preencher o Dto
        /// </summary>
        /// <param name="colaboradorConfig">Objeto projetoColaboradorConfig</param>
        /// <returns>Objeto Dto preenchido</returns>
        public ProjetoColaboradorConfigDto DtoFactory()
        {
            ProjetoColaboradorConfigDto colaboradorConfigDto = new ProjetoColaboradorConfigDto()
            {
                CorColaborador = int.Parse( Cor ),
                NomeCompletoColaborador = Colaborador._NomeCompleto,
                OidColaborador = Colaborador.Oid,
                OidUsuario = Colaborador.Usuario.Oid,
                Login = Colaborador.Usuario.UserName
            };

            return colaboradorConfigDto;
        }

        #endregion

        #region Construtores

        public ProjetoColaboradorConfig( Session session )
            : base( session )
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }

        #endregion
    }
}
