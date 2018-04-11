using System;
using System.Collections.Generic;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Delegates;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Libs.Delegates.WebService;
using WexProject.BLL.Shared.DTOs.Geral;
namespace WexProject.Schedule.Library.ServiceUtils.Interfaces
{
    public interface IPlanejamentoServiceUtil
    {
        event RespostaAsyncServiceHandler AoCompletarMovimentacaoTarefa;
        event RespostaAsyncServiceHandler AoCompletarSolicitacaoCriarNovaTarefa;
        event RespostaAsyncServiceHandler AoCompletarSolicitacaoExclusaoTarefas;
        List<ColaboradorDto> ListarColaboradores();
        CronogramaDto ConsultarCronogramaPorNome( string nomeCronograma );
        List<CronogramaDto> ListarCronogramas();
        CronogramaTarefaGridItem ConsultarCronogramaTarefaPorOid( Guid oidCronogramaTarefa );
        List<CronogramaTarefaGridItem> ConsultarCronogramaTarefasPorOid( List<string> oidCronogramaTarefas );
        List<CronogramaTarefaGridItem> ConsultarCronogramaTarefasPorOidCronograma( Guid oidCronograma );
        InicializadorEstimativaDto ConsultarHorarioUltimaTarefaDiaColaborador( string login, DateTime data );
        InicializadorEstimativaDto ConsultarInicializadorEstimativaInicialColaborador( string login );
        SituacaoPlanejamentoDTO ConsultarSituacaoPlanejamentoPadrao();
        List<SituacaoPlanejamentoDTO> ConsultarSituacoesInativas();
        List<SituacaoPlanejamentoDTO> ConsultartSituacoesPlanejamento();
        List<SituacaoPlanejamentoDTO> ConsultarSituacoesPlanejamentoTipadas();
        TarefaHistoricoTrabalhoDto ConsultarTarefaHistoricoTrabalhoAtual( Guid oidTarefa );
        List<TarefaLogAlteracaoDto> ConsultarTarefaLogAlteracaoPorOid( string oidTarefa );
        CronogramaDto ConsultarUltimoCronogramaSelecionado( string login );
        bool EditarCronograma( CronogramaDto cronograma );
        CronogramaDto CriarCronogramaPadrao();
        void CriarHistoricoTarefa( Guid oidTarefa, string login, TimeSpan nbHoraRealizado, DateTime dtRealizado, TimeSpan nbHoraInicial, TimeSpan nbHoraFinal, string txComentario, TimeSpan nbHoraRestante, Guid oidSituacaoPlanejamento, string txJustificativaReducao );
        void CriarNovaTarefa( Guid oidCronograma, string txDescricao = "", string oidSituacao = "", DateTime dtInicio = new DateTime(), string responsaveis = "", string login = "", string txObservacao = "", Int16 nbEstimativaInicial = 0, short nbIDReferencia = 0 );
        System.Collections.Hashtable EditarTarefa( CronogramaTarefaDto tarefa );

        bool ExcluirCronograma( string oidCronograma );
        void ExcluirTarefas( List<Guid> oidTarefas, Guid oidCronograma );
        void MoverTarefa( Guid oidCronogramaTarefaSelecionada, short nbIDDestino );
        void SalvarUltimoCronogramaSelecionado( string login, string oidCronograma );

        List<CronogramaColaboradorConfigDto> ConsultarConfigUsuariosConectados( string[] logins, string oidCronograma );
        void EscolherCorColaborador( string login, string oidCronograma );

        BurndownGraficoDto ConsultarDadosGraficoBurndown( Guid oidCronograma );
    }
}
