using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.MultiAccess.Library;
using WexProject.MultiAccess.Library.Libs;
using WexProject.MultiAccess.Library.Dtos;
using System.Threading;
using WexProject.MultiAccess.Library.Domains;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    /// <summary>
    /// WexMultiAccessManager Mock Genérico para testes Publicando atributos protegidos
    /// </summary>
    public class WexMultiAccessManagerMock : WexMultiAccessManager
    {
        #region Construtores
        /// <summary>
        /// Acréscimo do evento no construtor
        /// </summary>
        public WexMultiAccessManagerMock()
        {
            ContadorMensagens = new Dictionary<int,int>();
            ListaTodasMensagensProcessadas = new List<MensagemDto>();
            TarefasExcluidas = new List<string>();
            filaDeFilasResumidas = new List<List<MensagemDto>>();
            SolicitacoesExclusao = new Dictionary<string,List<string>>();
            foreach (int tipo in Enum.GetValues(typeof(CsTipoMensagem)))
            {
                ContadorMensagens.Add(tipo,0);
            }
        }
        #endregion

        /// <summary>
        ///  Dicionário para conferir a quantidade de cada tipo de mensagem que foram recebidas pelo manager
        /// </summary>
        public Dictionary<int,int> ContadorMensagens { get; set; }

        /// <summary>
        /// Propriedade Responsável por
        /// </summary>
        public Dictionary<string,Dictionary<string,ConexaoCliente>> CronogramasConectados
        {
            set { cronogramasConectados = value; }
            get { return cronogramasConectados; }
        }

        /// <summary>
        /// Propriedade para Armazenar oid das tarefas excluidas para fins de teste 
        /// </summary>
        public List<string> TarefasExcluidas;

        /// <summary>
        /// Dicionário que armazena as tarefas que foram solicitadas exclusão 
        /// Key - oidCronograma
        /// Value - List -> oidTarefa das tarefas solicitas a exclusão
        /// </summary>
        public Dictionary<string,List<string>> SolicitacoesExclusao {get;set;}

        /// <summary>
        /// Armazena uma lista de filas de mensagem Dto Cada Vez que é processado o método de resumir mensagens
        /// Interceptando todas as mensagens que passaram pelo Manager
        /// </summary>
        public List<List<MensagemDto>> filaDeFilasResumidas;

        /// <summary>
        /// Responsável por retornar o dicionário 
        /// de usuário conectados.
        /// </summary>
        public Dictionary<string,List<string>> UsuariosConectados
        {
            get { return usuariosConectados; }
        }

        /// <summary>
        /// Publicado atributo para ser acessivel nos testes.
        /// </summary>
        public Queue<MensagemDto> FilaProcessamento
        {
            get { return filaProcessamento; }
            set { filaProcessamento = value; }
        }
        /// <summary>
        /// Atributo Publicado com esta propriedade para fins de teste
        /// </summary>
        public Thread ThreadManterAtendimento
        {
            get { return threadAceitarNovosClientes; }
        }

        /// <summary>
        /// Atributo Publicado com esta propriedade para fins de teste
        /// </summary>
        public Thread ThreadProcessarEventos
        {
            get { return threadProcessarEventos; }
        }

        /// <summary>
        /// Propriedade Publicando para testes o dicionario de tarefasEmEdicaoPorCronograma
        /// </summary>
        public Dictionary<string,Dictionary<string,string>> TarefasEmEdicao { get { return tarefasEmEdicaoPorCronograma; } set { tarefasEmEdicaoPorCronograma = value; } }

        /// <summary>
        /// Propriedade Publicando atributo para testes unitários
        /// </summary>
        public Dictionary<string,Dictionary<string,string>> TarefasEmExclusao { get { return tarefasEmExclusaoPorCronograma; } set { tarefasEmExclusaoPorCronograma = value; } }

        /// <summary>
        /// Propriedade para fins de testes que armazenada todas as mensagens recebidas no manager
        /// </summary>
        public List<MensagemDto> ListaTodasMensagensProcessadas { get; set; }

        /// <summary>
        /// Propriedade publicando para testes o atributo cronogramasNomeEmEdicao
        /// </summary>
        public Dictionary<string, string> CronogramasNomeEmEdicao
        {
            get { return cronogramasComDadosEmEdicao; }
            set { cronogramasComDadosEmEdicao = value; }
        }

        /// <summary>
        /// Método Publico Responsável por utilizar o método privado resumir mensagem
        /// </summary>
        /// <param name="lista">Lista de MensagemDto</param>
        /// <returns>Lista resumida de mensagens semelhantes ao mesmo cronograma</returns>
        public List<MensagemDto> RnResumirMensagensPublicado(List<MensagemDto> lista)
        {
            return RnResumirMensagens(lista);
        }

        /// <summary>
        /// Método para testes, avaliar se a fila de processamento contém mensagens de um determinado tipo
        /// </summary>
        /// <param name="tipo">Tipo da Mensagem</param>
        /// <returns>
        /// True - Contém
        /// False - Não Contém
        /// </returns>
        public bool FilaContemMensagensDoTipo(CsTipoMensagem tipo)
        {
            if (filaProcessamento.Count(o => o.Tipo == tipo) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Sobrescrita do Método RnResumirMensagens de forma a interceptar todas as mensagens(MensagemDto) que processadas pelo método
        /// </summary>
        /// <param name="fila">Fila de Mensagens a ser resumida</param>
        /// <returns>Fila resumida de mensagens</returns>
        protected override List<MensagemDto> RnResumirMensagens(List<MensagemDto> fila)
        {
            List<MensagemDto> filaResumida = base.RnResumirMensagens(fila);
            filaDeFilasResumidas.Add(filaResumida);
            return filaResumida;
        }

        /// <summary>
        /// Método para teste, povoar hash de usuarios conectados
        /// </summary>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="cronogramas">Lista de cronogramas em que esta conectado</param>
        public void PreencherHashUsuariosConectados(string colaborador,List<string> cronogramas)
        {
            usuariosConectados.Add(colaborador,cronogramas);
        }
        /// <summary>
        /// Método para teste, injetar uma hash de CronogramasConectados na hash do manager
        /// </summary>
        /// <param name="hashPreenchida">Hash de cronogramasconectados preenchida</param>
        public void PreencherHashCronogramasConectados(Dictionary<string,Dictionary<string,ConexaoCliente>> hashPreenchida)
        {
            cronogramasConectados = hashPreenchida;
        }

        /// <summary>
        /// Método para testes, para injetar tarefas em Edição
        /// </summary>
        /// <param name="oidCronograma">oidCronograma da mensagem</param>
        /// <param name="login">login do colaborador que iniciou a edição</param>
        /// <param name="oidTarefa">oid da tarefa editada</param>
        public void AdicionarTarefaATarefasEmEdicaoParaTestes(string oidCronograma,string login,string oidTarefa)
        {
            AdicionarTarefaATarefasEmEdicao(oidCronograma,login,oidTarefa);
        }

        /// <summary>
        /// Método publicado para testes unitários em RnProcessarMensagem
        /// </summary>
        /// <param name="mensagem">mensagem a ser processada</param>
        /// <returns>Mensagem gerada ao final do processamento</returns>
        public MensagemDto RnProcessarMensagemParaTestes(MensagemDto mensagem)
        {
            return RnProcessarMensagem(mensagem);
        }

        /// <summary>
        /// Método para auxilio em testes unitários sobrescrevendo o RnProcessarMensagem para armazenar
        /// todas as mensagens Recebidas no RnProcessarMensagem
        /// </summary>
        /// <param name="mensagemAtual">mensagem a ser processada</param>
        /// <returns>mensagem processada</returns>
        protected override MensagemDto RnProcessarMensagem(MensagemDto mensagemAtual)
        {
            MensagemDto resposta;
            lock(ListaTodasMensagensProcessadas)
            {
                ListaTodasMensagensProcessadas.Add( mensagemAtual ); 
            }
            ContadorMensagens[(int)mensagemAtual.Tipo]++;
            resposta = base.RnProcessarMensagem(mensagemAtual);
            if(resposta != null)
                if(mensagemAtual.Tipo != resposta.Tipo)
                    ContadorMensagens[(int)resposta.Tipo]++;

            return resposta;
        }

        /// <summary>
        /// Método de Auxilio em testes unitários sobrescrevendo o comportarmento atual
        /// acrescentando a interceptação da MensagemDto Atual.
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo Permitir</param>
        public override void RnResponderSolicitacaoExclusaoTarefa(MensagemDto mensagem)
        {
            lock(ListaTodasMensagensProcessadas)
            {
                ListaTodasMensagensProcessadas.Add( mensagem ); 
            }
            ContadorMensagens[(int)mensagem.Tipo]++;
            base.RnResponderSolicitacaoExclusaoTarefa(mensagem);
        }

        /// <summary>
        /// Método responsável por interceptar o oid tarefa de todas as mensagens excluidas
        /// (método para fins de teste)
        /// </summary>
        /// <param name="mensagem">MensagemDto do tipo </param>
        /// <returns>ComunicarExclusaoConcluida</returns>
        protected override MensagemDto ProcessarMensagemExclusaoTarefaFinalizada(MensagemDto mensagem)
        {
            MensagemDto novaMensagemDto  = base.ProcessarMensagemExclusaoTarefaFinalizada(mensagem);
            string[] oidTarefas = (string[])novaMensagemDto.Propriedades["tarefas"];

            foreach (string oidTarefa in oidTarefas)
	        {
                TarefasExcluidas.Add(oidTarefa);
	        }
            return novaMensagemDto;
        }

        protected override void ProcessarMensagemInicioExclusaoTarefa(MensagemDto mensagem)
        {
            string oidCronograma = (string)mensagem.Propriedades["oidCronograma"];
            string[] oidTarefas = (string[])mensagem.Propriedades["tarefas"];
            if (!SolicitacoesExclusao.ContainsKey(oidCronograma))
                SolicitacoesExclusao.Add(oidCronograma,new List<string>());

            foreach (string oidTarefa in oidTarefas)
            {
                if (!SolicitacoesExclusao[oidCronograma].Contains(oidTarefa))
                    SolicitacoesExclusao[oidCronograma].Add(oidTarefa);
            }
            base.ProcessarMensagemInicioExclusaoTarefa(mensagem);
        }

        /// <summary>
        /// Método para auxiliar em testes unitários adicionando um colaborador para testes
        /// </summary>
        public void AdicionarUsuarioConectado(string login, string oidCronograma) 
        {
            usuariosConectados.Add( login, new List<string>() { oidCronograma } );
            if(cronogramasConectados == null)
                cronogramasConectados = new Dictionary<string, Dictionary<string, ConexaoCliente>>();
            if(!cronogramasConectados.ContainsKey( oidCronograma ))
                cronogramasConectados.Add( oidCronograma, new Dictionary<string,ConexaoCliente>() );
            if(!cronogramasConectados[oidCronograma].ContainsKey( login ))
                cronogramasConectados[oidCronograma].Add( login, null );
        }
    }
}
