using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Exceptions.Planejamento;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.BOs.Planejamento
{
    public class TarefaHistoricoTrabalhoBo
    {
        /// <summary>
        /// Método responsável por criar um histórico para uma determinada tarefa.
        /// </summary>
        /// 
        /// <param name="oidTarefa">Oid da Tarefa</param>
        /// <param name="login">Login do usuário</param>
        /// <param name="nbHoraRealizado">Horas realizadas na atividade.</param>
        /// <param name="dtRealizado">Data de realização da atividade.</param>
        /// <param name="nbHoraInicial">Hora Inicial da atividade</param>
        /// <param name="nbHoraFinal">Hora Final da atividade</param>
        /// <param name="txComentario">Comentário da atividade</param>
        /// <param name="nbHoraRestante">Horas restantes da atividade</param>
        /// <param name="oidSituacaoPlanejamento">Oid da Situação Planejamento da atividade</param>
        /// <param name="txJustificativaReducao">Justificativa de redução da atividade</param>
        public static void CriarHistoricoTarefa( Guid oidTarefa, string login, TimeSpan nbHoraRealizado, DateTime dtRealizado, TimeSpan nbHoraInicial, TimeSpan nbHoraFinal, string txComentario, TimeSpan nbHoraRestante, Guid oidSituacaoPlanejamento, string txJustificativaReducao )
        {
            if( oidTarefa == new Guid() || String.IsNullOrEmpty( login ))
                throw new ArgumentException( "Os parâmetros Login e OidTarefa não podem ser nulos." );

            TarefaHistoricoTrabalho historico = new TarefaHistoricoTrabalho();
            Tarefa tarefa = TarefaDao.ConsultarTarefaPorOid( oidTarefa );
            Colaborador colaborador = ColaboradorDAO.ConsultarColaborador( login, o => o.Usuario.Person );

            if(tarefa == null)
                return;

            SituacaoPlanejamento situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( oidSituacaoPlanejamento );
            if(tarefa.NbEstimativaInicial <= 0 && ( situacaoPlanejamento.CsTipo == CsTipoPlanejamento.Cancelamento || situacaoPlanejamento.CsTipo == CsTipoPlanejamento.Planejamento ))
            {
                TarefaBo.AtualizarDadosTarefa( tarefa.Oid, colaborador, situacaoPlanejamento, nbHoraRealizado, nbHoraRestante, tarefa.CsLinhaBaseSalva );
                return;
            }
            // Caso não possua estimativa inicial não deve salvar historico da tarefa
            if(tarefa.NbEstimativaInicial <= 0 && situacaoPlanejamento.CsTipo != CsTipoPlanejamento.Impedimento)
                throw new EstimativaInicialInvalidaException( "Histórico da Tarefa não pode ser salvo, pois não foi estimado um tempo inicial para realização da tarefa" );

            if(!tarefa.CsLinhaBaseSalva)
                TarefaBo.SalvarLinhaBaseTarefa( tarefa );

            historico.OidTarefa = tarefa.Oid;
            historico.HoraRealizado = nbHoraRealizado;
            historico.DtRealizado = dtRealizado;
            historico.HoraInicio = nbHoraInicial;
            historico.HoraFinal = nbHoraFinal;
            historico.TxComentario = txComentario;
            historico.HoraRestante = nbHoraRestante;
            historico.TxJustificativaReducao = txJustificativaReducao;
            historico.OidColaborador = colaborador.Oid;
            historico.OidSituacaoPlanejamento = situacaoPlanejamento.Oid;
            historico.TxComentario = txComentario;
            historico.TxJustificativaReducao = txJustificativaReducao;

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                contexto.TarefaHistoricoTrabalho.Add( historico );
                contexto.SaveChanges(); 
            }

            //Atualiza os dados da tarefa a partir dos dados salvos no histórico.
            TarefaBo.AtualizarDadosTarefa( tarefa.Oid, colaborador, situacaoPlanejamento, historico.HoraRealizado, historico.HoraRestante, tarefa.CsLinhaBaseSalva );
        }

        /// <summary>
        /// Método responsável por efetuar a seleção da hora de inicio da estimativa de uma tarefa para o colaborador
        /// </summary>
        /// <param name="contexto">sessão atual do banco</param>
        /// <param name="loginColaborador">login do colaborador atual</param>
        /// <returns>tarefaHistorico preenchida com</returns>
        public static InicializadorEstimativaDto SelecionarInicializadorEstimativaDto( string loginColaborador, DateTime dataSolicitacao )
        {
            SemanaTrabalho semanaTrabalho = SemanaTrabalhoBo.SemanaTrabalhoPadraoFactory();
            return SelecionarInicializadorEstimativaDto( loginColaborador, dataSolicitacao, semanaTrabalho );
        }

        /// <summary>
        /// Método responsável por efetuar a seleção da hora de inicio da estimativa de uma tarefa para o colaborador
        /// </summary>
        /// <param name="contexto">sessão atual do banco</param>
        /// <param name="loginColaborador">login do colaborador atual</param>
        /// <returns>tarefaHistorico preenchida com</returns>
        public static InicializadorEstimativaDto SelecionarInicializadorEstimativaDto( string loginColaborador, DateTime dataSolicitacao, SemanaTrabalho semanaTrabalho )
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();

            TarefaHistoricoTrabalho ultimoHistoricoTrabalho = TarefaHistoricoTrabalhoDao.ConsultarUltimoEsforcoRealizadoColaborador( contexto, loginColaborador, dataSolicitacao );

            InicializadorEstimativaDto inicializadorEstimativaDto = new InicializadorEstimativaDto();

            DateTime dataSelecionada;

            if(!ValidarTarefaHistoricoTrabalho( ultimoHistoricoTrabalho ))
            {
                dataSelecionada = dataSolicitacao.Date;
                SelecionarDataDiaUtilInicial( semanaTrabalho, ref dataSelecionada );
                return PreencherInicializacaoDiaSelecionado( semanaTrabalho, dataSelecionada );
            }

            bool ultrapassouLimiteDia = ultimoHistoricoTrabalho.HoraFinal.Days > 0;
            dataSelecionada = new DateTime( ultimoHistoricoTrabalho.DtRealizado.Ticks );
            if(ultrapassouLimiteDia)
            {
                dataSelecionada = SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
                return PreencherInicializacaoDiaSelecionado( semanaTrabalho, dataSelecionada );
            }

            SelecionarDataDiaUtilInicial( semanaTrabalho, ref dataSelecionada );
            if(dataSelecionada.Equals( dataSolicitacao.Date ))
            {
                if(dataSelecionada.Equals( ultimoHistoricoTrabalho.DtRealizado ))
                {
                    inicializadorEstimativaDto.DataEstimativa = dataSelecionada;
                    inicializadorEstimativaDto.HoraInicialEstimativa = ultimoHistoricoTrabalho.HoraFinal;
                    inicializadorEstimativaDto.DiaAtual = TarefaHistoricoTrabalhoDao.ConsultarDiaAtualDeTrabalhoDto( dataSelecionada.DayOfWeek, semanaTrabalho );
                    return inicializadorEstimativaDto;
                }
                return PreencherInicializacaoDiaSelecionado( semanaTrabalho, dataSelecionada );
            }
            else
            {
                if(SemanaTrabalhoBo.DiaAtualPossuiPeriodoTrabalho( semanaTrabalho, dataSelecionada.DayOfWeek ))
                {
                    DiaTrabalho dia = SemanaTrabalhoDao.SelecionarDiaTrabalho( semanaTrabalho, dataSelecionada.DayOfWeek );
                    long ticks = dia.PeriodosDeTrabalho.Max( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraFinal ) );
                    TimeSpan horaFinalExpediente = new TimeSpan( ticks );
                    if(ultimoHistoricoTrabalho.HoraFinal >= horaFinalExpediente)
                    {
                        dataSelecionada = SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
                        return PreencherInicializacaoDiaSelecionado( semanaTrabalho, dataSelecionada );
                    }
                    if(dataSelecionada.Equals( ultimoHistoricoTrabalho.DtRealizado ))
                    {
                        inicializadorEstimativaDto.DataEstimativa = dataSelecionada;
                        inicializadorEstimativaDto.HoraInicialEstimativa = ultimoHistoricoTrabalho.HoraFinal;
                        inicializadorEstimativaDto.DiaAtual = TarefaHistoricoTrabalhoDao.ConsultarDiaAtualDeTrabalhoDto( dataSelecionada.DayOfWeek, semanaTrabalho );
                        return inicializadorEstimativaDto;
                    }
                    return PreencherInicializacaoDiaSelecionado( semanaTrabalho, dataSelecionada );
                }
                else
                {
                    inicializadorEstimativaDto.DataEstimativa = dataSolicitacao.Date;
                    inicializadorEstimativaDto.HoraInicialEstimativa = new TimeSpan( 8, 0, 0 );
                    inicializadorEstimativaDto.DiaAtual = TarefaHistoricoTrabalhoDao.ConsultarDiaAtualDeTrabalhoDto( dataSolicitacao.DayOfWeek, semanaTrabalho );
                }
                return inicializadorEstimativaDto;
            }
        }

        /// <summary>
        /// Método para preencher uma inicialização do dia selecionado
        /// </summary>
        /// <param name="semanaTrabalho">semana de trabalho do colaborador</param>
        /// <param name="dataSelecionada">data de selecao</param>
        /// <returns></returns>
        private static InicializadorEstimativaDto PreencherInicializacaoDiaSelecionado( SemanaTrabalho semanaTrabalho, DateTime dataSelecionada )
        {
            InicializadorEstimativaDto inicializadorDto = new InicializadorEstimativaDto();
            if(SemanaTrabalhoBo.DiaAtualPossuiPeriodoTrabalho( semanaTrabalho, dataSelecionada.DayOfWeek ))
            {
                inicializadorDto.DataEstimativa = dataSelecionada;
                DiaTrabalho dia = SemanaTrabalhoDao.SelecionarDiaTrabalho( semanaTrabalho, dataSelecionada.DayOfWeek );
                long ticks = dia.PeriodosDeTrabalho.Min( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraInicial ) );
                inicializadorDto.HoraInicialEstimativa = new TimeSpan( ticks );
            }
            else
                inicializadorDto = new InicializadorEstimativaDto() { DataEstimativa = dataSelecionada, HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };

            inicializadorDto.DiaAtual = TarefaHistoricoTrabalhoDao.ConsultarDiaAtualDeTrabalhoDto( dataSelecionada.DayOfWeek, semanaTrabalho );
            return inicializadorDto;
        }

        /// <summary>
        /// Método que seleciona o proximo dia util do colaborador
        /// </summary>
        /// <param name="semanaTrabalho"></param>
        /// <param name="dataSelecionada"></param>
        private static void SelecionarDataDiaUtilInicial( SemanaTrabalho semanaTrabalho, ref DateTime dataSelecionada )
        {
            if(!ValidarSemanaTrabalho( semanaTrabalho ))
                return;

            if(!SemanaTrabalhoBo.DiaAtualPossuiPeriodoTrabalho( semanaTrabalho, dataSelecionada.DayOfWeek ))
                dataSelecionada = SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
        }

        /// <summary>
        /// Método que busca o proximo dia util do colaborador baseado na semana de trabalho cadastrada
        /// </summary>
        /// <param name="semanaTrabalho">semana de trabalho</param>
        /// <param name="dataSelecionada">data selecionada</param>
        public static DateTime SelecionarDataProximoDiaUtil( SemanaTrabalho semanaTrabalho, DateTime dataSelecionada )
        {
            if(!ValidarSemanaTrabalho( semanaTrabalho ))
                return dataSelecionada;

            DateTime dataSelecionadaOriginal = new DateTime( dataSelecionada.Ticks );
            dataSelecionada = dataSelecionada.AddDays( 1 );
            DayOfWeek diaSemana = dataSelecionada.DayOfWeek;
            while(!SemanaTrabalhoBo.DiaAtualPossuiPeriodoTrabalho( semanaTrabalho, diaSemana ))
            {
                dataSelecionada = dataSelecionada.AddDays( 1 );
                diaSemana = dataSelecionada.DayOfWeek;
                if(dataSelecionadaOriginal.DayOfWeek.Equals( diaSemana ))
                {
                    dataSelecionada = dataSelecionadaOriginal;
                    break;
                }
            }
            return dataSelecionada;
        }

        /// <summary>
        /// Método para valiar uma semana de trabalho
        /// </summary>
        /// <param name="semanaTrabalho"></param>
        /// <returns></returns>
        public static bool ValidarSemanaTrabalho( SemanaTrabalho semanaTrabalho )
        {
            if(semanaTrabalho == null || semanaTrabalho.diasTrabalho == null || semanaTrabalho.diasTrabalho.Count == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Método responsável por retornar se um historico de trabalho para a tarefa é valido
        /// </summary>
        /// <param name="historicoTrabalho"></param>
        /// <returns>True para válido e false para inválido</returns>
        public static bool ValidarTarefaHistoricoTrabalho( TarefaHistoricoTrabalho historicoTrabalho )
        {
            if(historicoTrabalho == null || historicoTrabalho.Oid.Equals( new Guid() ) || historicoTrabalho.Tarefa.Oid.Equals( new Guid() ) || historicoTrabalho.Tarefa.Oid.Equals( new Guid() ))
                return false;
            return true;
        }

        /// <summary>
        /// Método responsável por buscar a ultima hora trabalhada em um dia especifico
        /// </summary>
        /// <param name="contexto">Sessão atual do banco</param>
        /// <param name="login">login do colaborador atual</param>
        /// <param name="dataEspecifica"> data selecionada pelo colaborador</param>
        /// <returns>inicializador de estimativa</returns>
        public static InicializadorEstimativaDto SelecionarInicializadorEstimativaColaboradorParaDataEspecifica( string login, DateTime dataEspecifica )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				TimeSpan ultimaHoraTrabalhada = TarefaHistoricoTrabalhoDao.ConsultarHorarioUltimaTarefaDiaColaborador( contexto, login, dataEspecifica );
				return new InicializadorEstimativaDto() { DataEstimativa = dataEspecifica.Date, HoraInicialEstimativa = ultimaHoraTrabalhada, DiaAtual = TarefaHistoricoTrabalhoDao.ConsultarDiaAtualDeTrabalhoDtoDaSemanaPadrao( dataEspecifica.DayOfWeek ) };
			}
        }

        /// <summary>
        /// Método responsável por buscar o histórico mais atual de uma tarefa.
        /// </summary>
        /// <param name="contexto">Sessão Corrente</param>
        /// <param name="oidTarefa">Oid da Tarefa</param>
        /// <returns>Objeto TarefaHistoricoTrabalhoDto</returns>
        public static TarefaHistoricoTrabalhoDto ConsultarTarefaHistoricoAtualPorOidTarefaDto( Guid oidTarefa )
        {
            TarefaHistoricoTrabalho historicoAtual = TarefaHistoricoTrabalhoDao.ConsultarTarefaHistoricoAtualPorOidTarefa( oidTarefa );

            if(historicoAtual == null)
                return new TarefaHistoricoTrabalhoDto();
            return DtoFactory( historicoAtual );
        }

        /// Método responsável por retornar a ultima hora trabalhada pelo colaborador
        /// </summary>
        /// <param name="session"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public static TarefaHistoricoTrabalhoDto ConsultarUltimoEsforcoRealizadoColaboradorDto( WexDb contexto, Guid oidColaborador )
        {
            TarefaHistoricoTrabalho historicoAtual = TarefaHistoricoTrabalhoDao.ConsultarUltimoEsforcoRealizadoColaborador( contexto, oidColaborador );
            if(historicoAtual == null)
                return new TarefaHistoricoTrabalhoDto();
            return DtoFactory( historicoAtual );
        }

        /// <summary>
        /// Método responsável por criar um objeto de TarefaHistoricoDto e adicionar seus valores a partir de um TarefaHistoricoTrabalho.
        /// </summary>
        /// <param name="tarefaHistorico">Objeto TarefaHistoricoTrabalho</param>
        /// <returns>Objeto TarefaHistoricoDto</returns>
        public static TarefaHistoricoTrabalhoDto DtoFactory( TarefaHistoricoTrabalho tarefaHistorico )
        {
            if(tarefaHistorico == null)
                return new TarefaHistoricoTrabalhoDto();

            TarefaHistoricoTrabalhoDto tarefaHistoricoDto = new TarefaHistoricoTrabalhoDto()
            {
                OidTarefaHistorico = tarefaHistorico.Oid,
                NbRealizado = tarefaHistorico.HoraRealizado,
                DtRealizado = tarefaHistorico.DtRealizado,
                TxComentario = tarefaHistorico.TxComentario,
                NbRestante = tarefaHistorico.HoraRestante,
                OidSituacaoPlanejamento = (Guid)tarefaHistorico.OidSituacaoPlanejamento,
                TxJustificativaDeReducao = tarefaHistorico.TxJustificativaReducao,
                OidTarefa = (Guid)tarefaHistorico.OidTarefa,
                OidColaborador = (Guid)tarefaHistorico.OidColaborador,
                NbHoraInicio = tarefaHistorico.HoraInicio,
                NbHoraFinal = tarefaHistorico.HoraFinal
            };

            return tarefaHistoricoDto;
        }

        /// <summary>
        /// Método responsável por retornar a ultima hora trabalhada pelo colaborador
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public static TarefaHistoricoTrabalhoDto ConsultarUltimoEsforcoRealizadoColaboradorDto( WexDb contexto, string login )
        {
            TarefaHistoricoTrabalho historicoAtual = TarefaHistoricoTrabalhoDao.ConsultarUltimoEsforcoRealizadoColaborador( contexto, login );
            if(historicoAtual == null)
                return new TarefaHistoricoTrabalhoDto();
            return DtoFactory( historicoAtual );
        }
    }
}
