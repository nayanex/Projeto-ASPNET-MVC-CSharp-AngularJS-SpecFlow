using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Entities.Geral;

namespace WexProject.Schedule.Test.Stubs
{
    public class GeralServiceUtilStub : IGeralServiceUtil
    {
        /// <summary>
        /// Lista de config dos colaboradores conectados
        /// </summary>
        public List<CronogramaColaboradorConfigDto> UsuariosConectadosConfig { get; set; }

        /// <summary>
        /// Informação do colaborador
        /// </summary>
        public ColaboradorDto ColaboradorLogado { get; set; }

        /// <summary>
        /// Armazenar informações dos colaboradores
        /// </summary>
        public List<ColaboradorDto> Colaboradores { get; set; }

        /// <summary>
        /// Semana de trabalho atual
        /// </summary>
        public SemanaTrabalhoDto SemanaTrabalhoAtual { get; set; }
        public GeralServiceUtilStub()
        {
            UsuariosConectadosConfig = new List<CronogramaColaboradorConfigDto>();
            ColaboradorLogado = new ColaboradorDto();
            SemanaTrabalhoAtual = new SemanaTrabalhoDto();
        }

        /// <summary>
        /// Retorntar o colaborador logado
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public ColaboradorDto ConsultarColaboradorLogado( string login )
        {
            return Colaboradores.FirstOrDefault( o => o.Login.Equals( login ) );
        }

        /// <summary>
        /// Retornar as informações dos usuários conectados
        /// </summary>
        /// <param name="logins"></param>
        /// <param name="oidCronograma"></param>
        /// <returns></returns>
        public List<CronogramaColaboradorConfigDto> ConsultarConfigUsuariosConectados( string[] logins, string oidCronograma )
        {
            return UsuariosConectadosConfig;
        }

        /// <summary>
        /// Retornar a semana de trabalho
        /// </summary>
        /// <returns></returns>
        public SemanaTrabalhoDto ConsultarSemanaDeTrabalhoPadrao()
        {
            return SemanaTrabalhoAtual;
        }

        /// <summary>
        /// ConsultarIncluindoRelacionamentos a cor selecionada para o colaborador atual
        /// </summary>
        /// <param name="login"></param>
        /// <param name="oidCronograma"></param>
        /// <returns></returns>
        public void EscolherCorColaborador( string login, string oidCronograma )
        {
        }
    }
}
