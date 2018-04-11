using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.BOs.Planejamento
{
    public class CronogramaColaboradorConfigBo
    {
        /// <summary>
        /// Método responsável por escolher e retornar uma cor para um colaborador em um determinado cronograma
        /// </summary>
        /// <param name="contexto">Contexto do Banco</param>
        /// <param name="oidColaborador">Oid identificação do colaborador</param>
        /// <param name="oidCronograma">Oid identificação do cronograma</param>
        /// <returns>Caso existam o colaborador e o cronograma retornará um string contendo a cor selecionada para o usuário naquele cronograma</returns>
        public static string EscolherCorColaborador( string login, Guid oidCronograma )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                string cor = null;
                CronogramaColaboradorConfig config = CronogramaColaboradorConfigDao.ConsultarCronogramaColaboradorConfig( contexto, login, oidCronograma );

                if(config == null)
                {
                    config = CronogramaColaboradorConfigDao.SalvarCronogramaColaboradorConfig( contexto, login, oidCronograma );

                    if(config == null)
                        return null;
                }
                else
                {
                    if(config.Cor != null)
                        return Convert.ToString( config.Cor );
                }

                List<string> coresArmazenadas = CronogramaColaboradorConfigDao.ConsultarCoresPorCronograma( contexto, oidCronograma );
                cor = ColaboradorConfigBo.SelecionarCor( coresArmazenadas );
                config.Cor = Convert.ToInt32( cor );
                contexto.SaveChanges();
                return cor;
            }
        }

        /// <summary>
        /// Método responsável por consultar vários usuários conectados
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="login">logins dos usuários conectado</param>
        /// <param name="oidCronograma">Oid do cronograma do usuário conectado</param>
        /// <returns>Objetos de UsuarioConectadoDto</returns>
        public static List<CronogramaColaboradorConfigDto> ConsultarCronogramaColaboradorConfigs( string[] logins, Guid oidCronograma )
        {
            List<CronogramaColaboradorConfig> configs = CronogramaColaboradorConfigDao.ConsultarCronogramaColaboradorConfig( new List<string>( logins ), oidCronograma );

            if(configs == null || configs.Count == 0)
                return null;

            return CronogramaColaboradorConfigBo.CronogramaColaboradorConfigFactory( oidCronograma, configs );
        }

        /// <summary>
        /// Método responsável por consultar o usuario conectado e criar um Dto de UsuarioConectadoDto para retornar.
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="login">login do usuário conectado</param>
        /// <param name="oidCronograma">Oid do cronograma do usuário conectado</param>
        /// <returns>Objeto UsuarioConectadoDto</returns>
        public static CronogramaColaboradorConfigDto ConsultarCronogramaColaboradorConfigs( string login, Guid oidCronograma )
        {
            Colaborador colaborador = ColaboradorDAO.ConsultarColaborador( login, o => o.Usuario.Person.Party );
            
            if(colaborador == null)
                return null;

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                CronogramaColaboradorConfig config = CronogramaColaboradorConfigDao.ConsultarCronogramaColaboradorConfig( contexto, login, oidCronograma );

                if(config == null)
                    return null;

                return CronogramaColaboradorConfigBo.CronogramaColaboradorConfigFactory( colaborador, oidCronograma, config );
            }
        }

        #region Factory

        /// <summary>
        /// Método responsável por criar uma lista de  Dto Usuario Conectado com os dados dos colaboradores e configs dos colaboradores no cronograma
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma em que estão conectados</param>
        /// <param name="configs">Configurações dos conectados</param>
        /// <returns>Lista do usuários conectados</returns>
        public static List<CronogramaColaboradorConfigDto> CronogramaColaboradorConfigFactory( Guid oidCronograma, List<CronogramaColaboradorConfig> configs )
        {
            List<CronogramaColaboradorConfigDto> usuariosConectadosDto = new List<CronogramaColaboradorConfigDto>();

            CronogramaColaboradorConfigDto usuarioConectado = null;
            foreach(CronogramaColaboradorConfig config in configs)
            {
                if(config == null)
                    return null;

                usuarioConectado = new CronogramaColaboradorConfigDto()
                {
                    OidCronograma = oidCronograma,
                    OidColaborador = config.Colaborador.Oid,
                    Login = config.Colaborador.Usuario.UserName,
                    NomeCompletoColaborador = config.Colaborador.NomeCompleto,
                    Foto = config.Colaborador.Usuario.Person.Party.Photo,
                    Cor = config.Cor.ToString()
                };
                usuariosConectadosDto.Add( usuarioConectado );
            }

            return usuariosConectadosDto;
        }

        /// <summary>
        /// Método responsável por criar um Dto de Usuario Conectado com os dados do colaborador e config do colaborador no cronograma
        /// </summary>
        /// <param name="colaborador">Objeto do colaborador</param>
        /// <param name="oidCronograma">Oid do cronograma que o usuário está conectado</param>
        /// <param name="config">Config do colaborador no cronograma</param>
        /// <returns>Dto de UsuarioConectado</returns>
        public static CronogramaColaboradorConfigDto CronogramaColaboradorConfigFactory( Colaborador colaborador, Guid oidCronograma, CronogramaColaboradorConfig config )
        {
            CronogramaColaboradorConfigDto usuarioConectado = new CronogramaColaboradorConfigDto()
            {
                OidCronograma = oidCronograma,
                OidColaborador = colaborador.Oid,
                Login = colaborador.Usuario.UserName,
                NomeCompletoColaborador = colaborador.NomeCompleto,
                Foto = colaborador.Usuario.Person.Party.Photo,
                Cor = config.Cor.ToString()
            };

            return usuarioConectado;
        }

        #endregion
    }
}
