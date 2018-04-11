using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Delegates;

namespace WexProject.MultiAccess.Library.Components
{
    public interface IWexMultiAccessClient
    {
        #region Eventos


        event AoFalharConexaoNoServidorEventHandler AoFalharConexaoNoServidor;

        event AoConectarNovoUsuarioEventHandler AoConectarNovoUsuario;

        event MensagemDtoEventHandler AoReceberConexaoRecusada;

        event MensagemDtoEventHandler AoUsuarioDesconectar;

        event MensagemDtoEventHandler AoSerAutenticadoComSucesso;

        event MensagemDtoEventHandler AoServidorDesconectar;

        event MensagemDtoEventHandler AoSerCriadaNovaTarefa;

        event MensagemDtoEventHandler AoIniciarEdicaoTarefa;

        event MensagemDtoEventHandler AoSerRecusadaEdicaoTarefa;

        event MensagemDtoEventHandler AoSerFinalizadaEdicaoTarefaPorOutroUsuario;

        event MensagemDtoEventHandler AoSerExcluidaTarefaPorOutroUsuario;

        event MensagemDtoEventHandler ExecutarExclusaoTarefa;

        event MensagemDtoEventHandler AoSerAutorizadaEdicaoTarefa;

        event MensagemDtoEventHandler AoOcorrerMovimentacaoPosicaoTarefa;

        event MensagemDtoEventHandler AoSerNotificadoAlteracaoDadosCronograma;

        event MensagemDtoEventHandler AoSerRecusadaEdicaoDadosCronograma;

        event MensagemDtoEventHandler AoIniciarEdicaoDadosCronograma;

        event MensagemDtoEventHandler AoSerPermitidaEdicaoDadosCronograma;

        event EventHandler LogarAoOcorrerException;

        event Action AoSerDesconectado;
        #endregion

        #region Propriedades
        /// <summary>
        /// Login do colaborador atual
        /// </summary>
        string Login { get; set; }

        /// <summary>
        /// Endereco de Ip WexMultiAcessManager
        /// </summary>
        string EnderecoIp { get; set; }

        /// <summary>
        /// Oid do Cronograma Selecionado
        /// </summary>
        string OidCronograma { get; set; }

        /// <summary>
        /// Numero da porta de conexão com WexMultiAcessManager
        /// </summary>
        int Porta { get; set; }

        /// <summary>
        /// Identificar estado da conexão
        /// </summary>
        bool Conectado { get; set; }
        #endregion

        #region Métodos
        void Conectar();

        void RnFinalizarConexao( bool forcarAtualizacao = false );
        /// <summary>
        /// responsável por comunicar o manager sobre a criação de uma nova tarefa
        /// </summary>
        /// <param name="oidNovaTarefa">guid da tarefa criada no cronograma</param>
        /// <param name="oidCronograma">guid do cronograma atual</param>
        void RnComunicarNovaTarefaCriada( string oidNovaTarefa, string oidCronograma, Dictionary<string, Int16> tarefasImpactadas, DateTime dataHoraAcao );

        /// <summary>
        /// responsável por comunicar o manager inicio da edição de uma tarefa
        /// </summary>
        /// <param name="oidTarefa">guid da tarefa editada</param>
        /// <param name="autoSalvarEdicao">booleano para informar se deve auto-salvar a edição ao receber a resposta de autorização</param>
        void RnComunicarInicioEdicaoTarefa( string idRequisicao, string oidTarefa );

        /// <summary>
        /// responsável por comunicar o manager sobre o termino da edição de uma tarefa
        /// </summary>
        /// <param name="oidTarefa">oid da tarefa  editada</param>
        void RnComunicarFimEdicaoTarefa( string oidTarefa );

        /// <summary>
        /// responsável por comunicar o manager sobre o inicio da exclusão de uma determinada tarefa
        /// </summary>
        /// <param name="tarefas">oid da tarefa a ser excluida</param>
        void RnComunicarInicioExclusaoTarefa( string[] tarefas );

        /// <summary>
        /// responsável por comunicar o resultado da exclusão de tarefas com o oid das tarefas excluidas e não excluidas
        /// </summary>
        /// <param name="tarefas">vetor oid das tarefas que foram excluidas</param>
        /// <param name="tarefasNaoExcluidas">vetor oid das tarefas que não foram excluidas</param>
        void RnComunicarFimExclusaoTarefaConcluida( string[] tarefas, Dictionary<string, Int16> tarefasImpactadas, string[] tarefasNaoExcluidas, DateTime dataHoraAcao );

        /// <summary>
        /// responsável por comunicar outros colaboradores conectados de que o nome do cronograma foi alterado
        /// </summary>
        /// <param name="nomeCronograma">novo nome do cronograma</param>
        void RnComunicarAlteracaoDadosCronograma( );

        /// <summary>
        /// responsável por comunicar o inicio da edição do nome do cronograma atual
        /// </summary>
         void RnComunicarInicioEdicaoDadosCronograma();

        /// <summary>
        /// Método responsável por comunicar a movimentação de uma tarefa
        /// </summary>
        /// <param name="posicaoAtual">posição da tarefa movida</param>
        /// <param name="posicaoFinal">posição alvo da tarefa movida</param>
        /// <param name="oidTarefa">oid da tarefa movida</param>
        /// <param name="oidTarefasImpactadas">novos nbids das tarefas reordenadas</param>
        void RnComunicarMovimentacaoTarefa( short posicaoAtual, short posicaoFinal, string oidTarefa, Dictionary<string, Int16> oidTarefasImpactadas, DateTime dataHoraAcao );

        /// <summary>
        /// responsável por desconectar o client do servidor
        /// </summary>
        void RnDesconectar();
        #endregion

    }
}
