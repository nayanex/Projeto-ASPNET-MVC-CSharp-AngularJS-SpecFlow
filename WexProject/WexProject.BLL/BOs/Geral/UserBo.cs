using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.Library.Libs.Img;

namespace WexProject.BLL.BOs.Geral
{
    public class UserBo
    {
        #region Factories

        /// <summary>
        /// Método responsável por criar um Dto de Usuario Conectado com os dados do colaborador e config do colaborador no cronograma
        /// </summary>
        /// <param name="colaborador">Objeto do colaborador</param>
        /// <param name="oidCronograma">Oid do cronograma que o usuário está conectado</param>
        /// <param name="config">Config do colaborador no cronograma</param>
        /// <returns>Dto de UsuarioConectado</returns>
        public static UsuarioConectadoDto UsuarioConectadoFactory( Colaborador colaborador, Guid oidCronograma, CronogramaColaboradorConfig config )
        {
            UsuarioConectadoDto usuarioConectado = new UsuarioConectadoDto( colaborador.Oid, oidCronograma )
            {
                LoginColaborador = colaborador.Usuario.UserName,
                NomeCompletoColaborador = colaborador.NomeCompleto,
                Foto = colaborador.Usuario.Person.Party.Photo,
                Cor = config.Cor.ToString()
            };

            return usuarioConectado;
        }

        /// <summary>
        /// Método responsável por criar uma lista de  Dto Usuario Conectado com os dados dos colaboradores e configs dos colaboradores no cronograma
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma em que estão conectados</param>
        /// <param name="configs">Configurações dos conectados</param>
        /// <returns>Lista do usuários conectados</returns>
        public static List<UsuarioConectadoDto> UsuarioConectadoFactory( Guid oidCronograma, List<CronogramaColaboradorConfig> configs )
        {
            List<UsuarioConectadoDto> usuariosConectadosDto = new List<UsuarioConectadoDto>();

            UsuarioConectadoDto usuarioConectado = null;
            foreach(CronogramaColaboradorConfig config in configs)
            {
                if(config == null)
                    return null;

                usuarioConectado = new UsuarioConectadoDto( config.Colaborador.Oid, oidCronograma )
                {
                    LoginColaborador = config.Colaborador.Usuario.UserName,
                    NomeCompletoColaborador = config.Colaborador.NomeCompleto,
                    Foto = config.Colaborador.Usuario.Person.Party.Photo,
                    Cor = config.Cor.ToString()
                };
                usuariosConectadosDto.Add( usuarioConectado );
            }

            return usuariosConectadosDto;
        }


        #endregion
    }
}
