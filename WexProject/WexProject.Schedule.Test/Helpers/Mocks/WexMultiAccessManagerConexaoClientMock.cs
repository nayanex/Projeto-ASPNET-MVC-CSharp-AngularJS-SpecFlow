using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.MultiAccess.Library;
using WexProject.MultiAccess.Library.Libs;
using WexProject.MultiAccess.Library.Dtos;
using System.Net.Sockets;
using System.Diagnostics;
using WexProject.Schedule.Test.Helpers.Domains;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    /// <summary>
    /// Classe Para testes que Pode Mockar o Processamento das threads de Conexão Cliente
    /// </summary>
    public class WexMultiAccessManagerConexaoClientMock : WexMultiAccessManager
    {
       /// <summary>
        /// Assinatura do evento AoEnviarConfirmacaoConexao
       /// </summary>
       /// <param name="usuarios">usuários online no cronograma determinado</param>
       /// <param name="oidCronograma"cronograma></param>
       /// <param name="login">usuario receptor</param>
       /// <returns></returns>
        public  delegate  void AoEnviarConfirmacaoConexaoEventHandler(string[] usuarios,string oidCronograma);

        /// <summary>
        /// Evento disparado na chamada de método do EnviarConfirmacaoConexaoAoCliente
        /// </summary>
        public event AoEnviarConfirmacaoConexaoEventHandler AoEnviarConfirmacaoConexao;

        /// <summary>
        /// Construtor para fins de teste por padrão Desabilita o funcionamento das threads de ProcessarEscrita ou ProcessarLeitura da Classe de Comunicação
        /// ConexaoCliente
        /// </summary>
        public WexMultiAccessManagerConexaoClientMock()
        {
            EstadoConexaoCliente = CsEstadoConexaoCliente.Leitura_E_Escrita_Desativadas;
        }

        /// <summary>
        /// Propriedade Responsável por publicar para fins de teste o dicionário de conexões por cronograma
        /// </summary>
        public Dictionary<string,Dictionary<string,ConexaoCliente>> CronogramasConectados
        {
            set { cronogramasConectados = value; }
            get { return cronogramasConectados; }
        }

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
            get
            {
                return filaProcessamento;
            }
        }
        /// <summary>
        /// Desabilitar alguma das threads de ConexaoCLiente
        /// Opções para desabilitar:
        /// ThreadProcessarEscrita
        /// ThreadProcessarLeitura
        /// Ambas
        /// Default:
        /// Ambas
        /// </summary>
        public CsEstadoConexaoCliente EstadoConexaoCliente { get; set; }

        /// <summary>
        /// Propriedade Util em testes para Habilitar e Desabilitar as threads
        /// </summary>
        public bool StatusServidor { get; set;}

        /// <summary>
        /// Fabricar uma ConexaoCliente Mockada desabilitando uma ou mais threads
        /// </summary>
        /// <param name="login">login do colaborador</param>
        /// <param name="tcp">conexao tcp ativa</param>
        /// <returns>Classe de Comunicação ConexaoCliente com thread(s) mockada(s)</returns>
        protected override ConexaoCliente ConexaoClienteFactory(string login,TcpClient tcp)
        {
            switch (EstadoConexaoCliente)
            {
                case CsEstadoConexaoCliente.Leitura_Desativada:
                    return new ConexaoClienteMock(login,tcp,filaProcessamento) { PermissaoDeLeitura = false };
                case CsEstadoConexaoCliente.Escrita_Desativada:
                    return new ConexaoClienteMock(login,tcp,filaProcessamento) { PermissaoDeEscrita = false };
                case CsEstadoConexaoCliente.Leitura_E_Escrita_Desativadas:
                    return new ConexaoClienteMock(login,tcp,filaProcessamento) { PermissaoDeEscrita = false,PermissaoDeLeitura = false };
                default:
                    return new ConexaoClienteMock(login,tcp,filaProcessamento) { PermissaoDeEscrita = false,PermissaoDeLeitura = false };
            }
        }

        /// <summary>
        /// IniciaAtendimento Mockado para não efetuar nenhum processamento a fim de evitar inicio da thread de atendimento
        /// </summary>
        public override void IniciaAtendimento()
        {
            // Não deve fazer nada
        }
        /// <summary>
        /// ManterAtentimento Mockado para não efetuar nenhum processamento de atendimento (threadManterAtendimento)
        /// </summary>
        public override void ManterAtendimento()
        {
            // Não deve fazer nada
        }
        /// <summary>
        /// Método publico criado para realizar chamada para testes do método protegido
        /// RnResumirMensagens
        /// </summary>
        /// <param name="lista">Lista de Mensagens a ser resumida</param>
        /// <returns>Lista de Mensagens Resumida</returns>
        public List<MensagemDto> RnResumirMensagensPublico(List<MensagemDto> lista)
        {
            return RnResumirMensagens(lista);
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
        /// Invocar o método protegido RnRemoverUsuarioQueSeDesconectou
        /// Nota: Caso não seja especificado um cronograma removerá o
        /// usuário de todos os cronogramas em que estiver conectado
        /// </summary>
        /// <param name="login">Login a ser removido</param>
        public void RnRemoverUsuarioQueSeDesconectouPublicadoTest(string login)
        {
            RnRemoverUsuarioQueSeDesconectou(login);
        }
        /// <summary>
        /// Invocar o métdo protegido de RnRemoverUsuarioQueSeDesconectou
        /// Removerá o usuário do cronograma especificado
        /// </summary>
        /// <param name="login">Login a ser removido</param>
        /// <param name="oidCronograma">Cronograma especifico de onde o usuário será desconectado</param>
        public void RnRemoverUsuarioQueSeDesconectouTest(string login,string oidCronograma)
        {
            RnRemoverUsuarioQueSeDesconectou(login,oidCronograma);
        }

        /// <summary>
        /// Listar as mensagens que serão enviadas pela conexão cliente ao respectivo WexMultiAccessClient
        /// </summary>
        public void ListarMensagensDeEscritaConexoesCliente() { 
        
             foreach (var cronograma in CronogramasConectados)
            {
                foreach (var conexoes in cronograma.Value)
                {
                    ListarMensagensFilaDeEscrita( conexoes.Key, conexoes.Value.FilaEscrita);
                }
            }
        
        }
        /// <summary>
        /// Efetuar Debug da lista de mensagens recebida na fila de escrita
        /// </summary>
        /// <param name="login">login do cliente atual</param>
        /// <param name="fila">fila de escrita do cliente atual</param>
        /// <param name="loginCliente">nome da threadProcessarEscrita do Cliente</param>
        private void ListarMensagensFilaDeEscrita(string login,Queue<MensagemDto> fila)
        {
            MensagemDto m;
            string[] usuarios;
            string cronograma;
            Debug.WriteLine(string.Format("Listagem de Itens na Fila de escrita {0}",login));
            for (int i = 0;i < fila.Count;i++)
            {
                m = fila.ElementAt(i);
                usuarios = (string[])m.Propriedades[Constantes.USUARIOS];
                cronograma = (string)m.Propriedades[Constantes.OIDCRONOGRAMA];
                Debug.WriteLine(string.Format("Mensagem Tipo:{0}",m.Tipo));
                for (int j = 0;j < usuarios.Length;j++)
                {
                    Debug.WriteLine(string.Format("{0} =>{1}:{2}",login,cronograma,usuarios[j]));
                }
            }
        }

        /// <summary>
        /// Sobrescrita do método para testes acrescentando a interceptação do envio da Mensagem de Conectado com Sucesso
        /// Capturando a lista de usuários que estavam previamente conectados(Que já estavam online).
        /// </summary>
        /// <param name="cronograma">Nome do Cronograma</param>
        /// <param name="usuarios">Vetor de usuários que estavam online</param>
        /// <returns>MensagemDto do tipo ConectadoComSucesso</returns>
        protected  override MensagemDto GetMensagemConexaoComSucesso(string cronograma, string[] usuarios,Dictionary<string,string> edicoes)
        {
            MensagemDto objetoMensagem = base.GetMensagemConexaoComSucesso(cronograma,usuarios,edicoes);
            if (AoEnviarConfirmacaoConexao != null)
                AoEnviarConfirmacaoConexao(usuarios, cronograma);
            return objetoMensagem;
        }
    }
}
