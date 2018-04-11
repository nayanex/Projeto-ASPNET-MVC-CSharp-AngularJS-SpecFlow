using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using System.Data.Entity;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.Contexto;
using WexProject.BLL.Extensions.Entities;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class TarefaHistoricoTrabalhoDao
    {
        public static SemanaTrabalho SemanaTrabalho { get; set; }

        /// <summary>
        /// Método que busca o histórico mais atual de uma tarefa.
        /// </summary>
        /// <param name="oidTarefa">Oid de Tarefa</param>
        /// <returns>Objeto TarefaHistoricoTrabalho</returns>
        public static TarefaHistoricoTrabalho ConsultarTarefaHistoricoAtualPorOidTarefa ( Guid oidTarefa, params Expression<Func<TarefaHistoricoTrabalho, object>>[] includes )
        {
            if(oidTarefa == null)
                throw new ArgumentException( "O parâmetro oidTarefa não pode ser nulo." );
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.TarefaHistoricoTrabalho.MultiploInclude( includes ).OrderByDescending( o => o.DtRealizado ).FirstOrDefault( o => o.OidTarefa == oidTarefa && !o.Tarefa.CsExcluido );
            }
        }

        /// <summary>
        /// Resgata a hora final da ultima tarefa realizada no dia selecionado pelo colaborador
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="login">Login do usuário do colaborador</param>
        /// <param name="data">Data selecionada pelo colaborador</param>
        /// <returns>Hora Final</returns>
        public static TimeSpan ConsultarHorarioUltimaTarefaDiaColaborador( WexDb contexto, string login, DateTime data )
        {
            TarefaHistoricoTrabalho ultimaTarefaColaborador = contexto.TarefaHistoricoTrabalho.Include( o => o.Colaborador.Usuario.Person ).Include( o => o.Tarefa ).OrderByDescending( o => o.DtRealizado ).
                                                                FirstOrDefault( o => o.Colaborador.Usuario.UserName.Equals( login ) && o.DtRealizado.Year == data.Year && o.DtRealizado.Month == data.Month && o.DtRealizado.Day == data.Day  && !o.Tarefa.CsExcluido  );
            if(ultimaTarefaColaborador == null)
                return new TimeSpan( 8, 0, 0 );
            else
                return ultimaTarefaColaborador.HoraFinal;
        }

        /// <summary>
        /// método responsável por buscar o ultimo esforço realizado pelo colaborador
        /// </summary>
        /// <param name="contexto">sessao atual do banco</param>
        /// <param name="login">login do colaborador</param>
        public static TarefaHistoricoTrabalho ConsultarUltimoEsforcoRealizadoColaborador( WexDb contexto, Guid oidColaborador )
        {
            TarefaHistoricoTrabalho ultimoHistorico = contexto.TarefaHistoricoTrabalho.Include( o => o.Colaborador.Usuario ).Include( o => o.Tarefa ).Where(o => o.Colaborador.Usuario.UserName.Equals( oidColaborador ) ).
                OrderByDescending( o => o.DtRealizado ).OrderByDescending( o => o.NbHoraFinal ).FirstOrDefault();

            return ultimoHistorico;
        }


        /// <summary>
        /// método responsável por buscar o ultimo esforço realizado pelo colaborador
        /// </summary>
        /// <param name="contexto">sessao atual do banco</param>
        /// <param name="login">login do colaborador</param>
        public static TarefaHistoricoTrabalho ConsultarUltimoEsforcoRealizadoColaborador( WexDb contexto, string login )
        {
            TarefaHistoricoTrabalho ultimoHistorico = contexto.TarefaHistoricoTrabalho
                .Include( o => o.Colaborador.Usuario )
                .Include( o => o.Tarefa )
                .Where( o => o.Colaborador.Usuario.UserName.Equals( login ) )
                .OrderByDescending( o => o.DtRealizado )
                .ThenByDescending( o => o.NbHoraFinal)
                .FirstOrDefault( o => !o.CsExcluido);

			   return ultimoHistorico;
        }

        /// <summary>
        /// Método responsável por consultar qual foi o último esforço realizado de um colaborador em uma determinada data.
        /// </summary>
        /// <param name="contexto">Conexão com o banco</param>
        /// <param name="dataUltimoEsforco">Data que se deseja obter o último histórico de trabalho do colaborador</param>
        /// <returns>Ultimo Histórico</returns>
        public static TarefaHistoricoTrabalho ConsultarUltimoEsforcoRealizadoColaborador( WexDb contexto, string login, DateTime dataUltimoEsforco )
        {
            TarefaHistoricoTrabalho ultimoHistorico = contexto.TarefaHistoricoTrabalho
                .Include( o => o.Colaborador.Usuario )
                .Include( o => o.Tarefa )
                .Where( o => o.DtRealizado == dataUltimoEsforco && o.Colaborador.Usuario.UserName.Equals( login ) )
                .OrderByDescending( o => o.DtRealizado )
                .ThenByDescending( o => o.NbHoraFinal )
                .FirstOrDefault( o => !o.CsExcluido );

            return ultimoHistorico;
        }

        /// <summary>
        /// Método para buscar uma semana de trabalho
        /// </summary>
        /// <returns></returns>
        public static SemanaTrabalho ConsultarSemanaTrabalho()
        {
            if(SemanaTrabalho == null)
                return SemanaTrabalhoBo.SemanaTrabalhoPadraoFactory();
            else
                return SemanaTrabalho;
        }

        /// <summary>
        /// Método responsável por retornar o dia de trabalho atual baseado na dia da semana
        /// </summary>
        /// <param name="diaDaSemana">dia da semana (DayOfWeek)</param>
        /// <param name="semanaTrabalho">Semana atual de trabalho</param>
        /// <returns></returns>
        public static DiaTrabalhoDto ConsultarDiaAtualDeTrabalhoDto( DayOfWeek diaDaSemana, SemanaTrabalho semanaTrabalho )
        {
            DiaTrabalho diaAtual = SemanaTrabalhoDao.SelecionarDiaTrabalho( semanaTrabalho, diaDaSemana );
            if(diaAtual == null)
            {
                diaAtual = new DiaTrabalho( diaDaSemana );
                DiaTrabalhoBo.AdicionarPeriodoDeTrabalho( "8:00", "18:00", diaAtual );
            }
            return DiaTrabalhoBo.DtoFactory( diaAtual );
        }

        /// <summary>
        /// Método responsável por retornar o dia de trabalho atual baseado na dia da semana
        /// </summary>
        /// <param name="diaDaSemana">dia da semana (DayOfWeek)</param>
        /// <returns></returns>
        public static DiaTrabalhoDto ConsultarDiaAtualDeTrabalhoDtoDaSemanaPadrao( DayOfWeek diaDaSemana )
        {
            return ConsultarDiaAtualDeTrabalhoDto( diaDaSemana, SemanaTrabalhoBo.SemanaTrabalhoPadraoFactory() );
        }
    }
}
