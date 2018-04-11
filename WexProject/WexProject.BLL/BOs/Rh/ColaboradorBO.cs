using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using WexProject.BLL.BOs.Outros;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.BOs.RH
{
    public class ColaboradorBo
    {
		private static ColaboradorBo instancia;

		public static ColaboradorBo Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new ColaboradorBo();
				}

				return instancia;
			}
		}

		private ColaboradorBo()
		{
		}

        /// <summary>
        /// Método responsável por criar o colaborador caso não exista.
        /// É usado pelo AD
        /// <param name="extensaoEmail">Extensao do email da empresa</param>
        /// <param name="contexto">Sessão</param>
        /// <param name="login">Login do usuário</param>
        /// </summary>
        public static Colaborador CriarColaborador( string login, string extensaoEmail )
        {
            string primeiroNome;
            string ultimoNome;
            string nomeCompleto;
            DateTime dtAdmissao;

            Colaborador colaboradorPesquisado = ColaboradorDAO.ConsultarColaborador( login, o => o.Usuario );

            if(colaboradorPesquisado != null)
                return colaboradorPesquisado;

            if(login.Contains( "." ))
            {
                primeiroNome = StrUtil.UpperCaseFirst( login.Split( '.' )[0] );
                ultimoNome = StrUtil.UpperCaseFirst( login.Split( '.' )[1] );
                nomeCompleto = String.Format( "{0} {1}", StrUtil.UpperCaseFirst( primeiroNome ), StrUtil.UpperCaseFirst( ultimoNome ) );
            }
            else
            {
                primeiroNome = login;
                ultimoNome = String.Empty;
                nomeCompleto = String.Format( "{0}", StrUtil.UpperCaseFirst( login ) );
            }

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                ColaboradorUltimoFiltro colaboradorUltimoFiltro = ColaboradorUltimoFiltroBO.CriarColaboradorUltimoFiltroPadrao( contexto );

                Party party = PartyBO.CriarPartyPadrao( contexto );

                Person person = PersonBO.CriarPersonPadrao( contexto, party, primeiroNome, ultimoNome, login, extensaoEmail );

                User usuario = UserBO.CriarUserPadrao( contexto, person, login );

                dtAdmissao = DateUtil.ConsultarDataHoraAtual();

                Colaborador novoColaborador = new Colaborador()
                {
                    OidUsuario = usuario.Oid,
                    TxMatricula = null,
                    DtAdmissao = dtAdmissao,
                    OidCoordenador = null,
                    OidCargo = null,
                    OidColaboradorUltimoFiltro = colaboradorUltimoFiltro.Oid
                };

                contexto.Colaboradores.Add( novoColaborador );
                contexto.SaveChanges();

                Role roleDefault = ColaboradorDAO.ConsultarRolePorNome( "Default" );
                Role roleAdmin = ColaboradorDAO.ConsultarRolePorNome( "Administradores" );

                UserBO.CriarPermissaoParaUsuario( contexto, usuario, roleDefault );
                UserBO.CriarPermissaoParaUsuario( contexto, usuario, roleAdmin );

                ColaboradorPeriodoAquisitivoBO.CriarPeridoAquisitivoParaColaborador( contexto, novoColaborador, dtAdmissao );

                return novoColaborador;
            }
        }


        /// <summary>
        /// Método responsável por criar o colaborador caso não exista.
        /// É usado pelo AD
        /// <param name="extensaoEmail">Extensao do email da empresa</param>
        /// <param name="session">Sessão</param>
        /// <param name="login">Login do usuário</param>
        /// </summary>
        public static WexProject.BLL.Models.Rh.Colaborador RnCriarColaborador( Session session, string login, string extensaoEmail )
        {
            if(session == null || String.IsNullOrEmpty( login ) == true || String.IsNullOrEmpty( extensaoEmail ) == true)
                throw new Exception( "Os parâmetros Sessão, Login e ExtensaoEmail não podem ser nulos." );

            string firstName;
            string lastName;
            string fullName;
            DateTime dtAdmissaoCriada;

            WexProject.BLL.Models.Rh.Colaborador colaboradorPesq = WexProject.BLL.Models.Rh.Colaborador.GetColaboradorPorLogin( session, login );

            if(colaboradorPesq == null)
            {
                if(login.Contains( "." ))
                {
                    firstName = login.Split( '.' )[0];
                    lastName = login.Split( '.' )[1];
                    fullName = String.Format( "{0} {1}", StrUtil.UpperCaseFirst( firstName ), StrUtil.UpperCaseFirst( lastName ) );
                }
                else
                {
                    firstName = login;
                    lastName = "";
                    fullName = String.Format( "{0}", StrUtil.UpperCaseFirst( login ) );
                }

                WexProject.BLL.Models.Rh.Colaborador colaboradorCriado = new WexProject.BLL.Models.Rh.Colaborador( session );

                dtAdmissaoCriada = DateUtil.ConsultarDataHoraAtual();
                firstName = StrUtil.UpperCaseFirst( firstName );
                lastName = StrUtil.UpperCaseFirst( lastName );
                colaboradorCriado.Usuario.ChangePasswordOnFirstLogon = false;
                colaboradorCriado.Usuario.FirstName = firstName;
                colaboradorCriado.Usuario.LastName = lastName;
                colaboradorCriado.DtAdmissao = dtAdmissaoCriada;
                colaboradorCriado.Usuario.UserName = login;
                colaboradorCriado.Usuario.Email = login + extensaoEmail;

                DevExpress.Persistent.BaseImpl.Role roleDefault = WexProject.BLL.Models.Rh.Colaborador.GetRolePorNome( session, "Administradores" );
                colaboradorCriado.Usuario.Roles.Add( roleDefault );

                colaboradorCriado.Save();

                UsuarioDAO.CurrentUser = colaboradorCriado.Usuario;

                return colaboradorCriado;
            }

            UsuarioDAO.CurrentUser = colaboradorPesq.Usuario;

            return colaboradorPesq;
        }

        /// <summary>
        /// Método responsável por consultar os colaboradores e transformá-los em Dto para utilizar na serialização do serviço.
        /// </summary>
        /// <returns>Lista dos colaboradores em Dto</returns>
        public static List<ColaboradorDto> ConsultarColaboradores()
        {
            List<ColaboradorDto> colaboradoresDto = new List<ColaboradorDto>();

            List<Colaborador> colaboradores = ColaboradorDAO.ConsultarColaboradores();

            for(int i = 0; i < colaboradores.Count; i++)
                colaboradoresDto.Add( ColaboradorBo.DtoFactory( colaboradores[i] ) );

            return colaboradoresDto;
        }

        /// <summary>
        /// Método responsável por acessar a classe colaborador e buscar o colaborador corrente e retornar para o serviço como um Dto
        /// </summary>
        /// 
        /// <param name="login">Login do colaborador</param>
        /// <returns>Retorna um DTO de colaborador</returns>
        public static ColaboradorDto ConsultarColaboradorPorLogin( string login )
        {
            var colaborador = ColaboradorDAO.ConsultarColaborador( login, o => o.Usuario.Person );

            if(colaborador != null)
                return ColaboradorBo.DtoFactory( colaborador );
            else
                return null;
        }

		public Colaborador ConsultarColaboradorPorMatricula(int matricula)
		{
			try
			{
				return ColaboradorDAO.Instancia.ConsultarColaboradorPorMatricula(matricula.ToString());
			}
			catch (InvalidOperationException)
			{
				throw new EntidadeNaoEncontradaException(String.Format("Colaborador com Matrícula {0} não encontrado.", matricula));
			}
		}

        #region Factories

        /// <summary>
        /// Método responsável por criar um objeto ColaboradorDto
        /// </summary>
        /// <param name="colaborador">Objeto Colaborador</param>
        /// <returns>Objeto ColaboradorDto</returns>
        public static ColaboradorDto DtoFactory( Colaborador colaborador )
        {
            ColaboradorDto colaboradorDto = new ColaboradorDto()
            {
                OidColaborador = colaborador.Oid,
                OidUsuario = colaborador.Usuario.Oid,
                TxMatriculaColaborador = colaborador.TxMatricula,
                Login = colaborador.Usuario.UserName,
                TxNomeCompletoColaborador = colaborador.NomeCompleto
            };

            return colaboradorDto;
        }

        #endregion

        #region BO Web

        private ColaboradorDto ParseDto(Colaborador colaborador)
        {
            if (colaborador == null)
            {
                return null;
            }

            ColaboradorDto dto = new ColaboradorDto();

            dto.OidColaborador = colaborador.Oid;
            dto.TxNomeCompletoColaborador = colaborador.Usuario.Person.FirstName;
            if (colaborador.Usuario.Person.LastName != null)
            {
                dto.TxNomeCompletoColaborador += " " + colaborador.Usuario.Person.LastName;
            }
            dto.TxCargo = colaborador.Cargo.TxDescricao;
            dto.TxMatriculaColaborador = colaborador.TxMatricula;
            dto.OidUsuario = colaborador.Usuario.Oid;
            dto.Login = colaborador.Usuario.UserName;

            return dto;
        }

        public ColaboradorDto ConsultarColaboradorPorGuid(Guid IdColaborador)
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            Colaborador colaborador = ColaboradorDAO.Instancia.ConsultarColaboradorPorGuid(db, IdColaborador);
            return ParseDto(colaborador);
        }

        /**
         * @summary Lista todos os colaboradores com o cargo de gerente
         * */
        public List<ColaboradorDto> ListarGerentes()
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            List<ColaboradorDto> colaboradores = new List<ColaboradorDto>();
            ColaboradorDAO.Instancia.ListarGerentes(db).ForEach(delegate(Colaborador c)
            {
                colaboradores.Add(ParseDto(c));
            });
            return colaboradores;
        }

        #endregion
    }
}
