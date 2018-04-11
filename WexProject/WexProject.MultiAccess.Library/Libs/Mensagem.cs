using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Domains;
using WexProject.Library.Libs;

namespace WexProject.MultiAccess.Library.Libs
{
    /// <summary>
    /// Classe Padronizada de Criação de vários tipos de MensagemDto
    /// </summary>
    public class Mensagem
    {

        #region Atributos

        /// <summary>
        /// Armazena as propriedades do Objeto MensagemDto
        /// </summary>
        private static Hashtable mensagemPropriedades;

        /// <summary>
        /// Armazena o Objeto MensagemDto que deverá ser retornado na mensagem;
        /// </summary>
        private static MensagemDto objetoMensagem;
        #endregion


        #region Métodos para criação de mensagens
        /// <summary>
        /// Responsável por criar uma MensagemDto quando uma tarefa for criada por um usuário.
        /// </summary>
        /// <param name="oidNovaTarefa">Oid da nova tarefa</param>
        /// <param name="login">login do usuário</param>
        /// <param name="oidCronograma">Oid do cronograma atual</param>
        /// <returns>MensagemDto do Tipo CriarNovaTarefa</returns>
        public static MensagemDto RnCriarMensagemNovaTarefaCriada( string oidNovaTarefa, string login, string oidCronograma, Dictionary<string, Int16> tarefasImpactadas, DateTime dataHoraAcao )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.NovaTarefaCriada };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.OIDTAREFA, oidNovaTarefa );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS_IMPACTADAS, tarefasImpactadas );
            objetoMensagem.Propriedades.Add( Constantes.DATAHORA_ACAO, dataHoraAcao );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de recusa de Conexão com o servidor
        /// </summary>
        /// <param name="motivo">Motivo da falha na conexão</param>
        /// <returns>Uma MensagemDto de Recusa de Conexão</returns>
        public static MensagemDto RnCriarMensagemConexaoRecusada( string motivo )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.ConexaoRecusadaServidor };
            objetoMensagem.Propriedades.Add( Constantes.MOTIVO, motivo );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de ConexaoEfetuadaComSucesso
        /// </summary>
        /// <param name="usuarios">Vetor de usuários que já estão conectados no cronograma</param>
        /// <param name="oidCronograma">Cronograma em que estão Conectados</param>
        /// <param name="autorEdicaoNomeCronograma">login do usuário que está editando o nome do cronograma (se houver)</param>
        /// <returns>Objeto MensagemDto do tipo ConexaoEfetuadaComSucesso</returns>
        public static MensagemDto RnCriarMensagemConexaoEfetuadaComSucesso( string[] usuarios, string oidCronograma, Dictionary<string, string> edicoes ,string autorEdicaoNomeCronograma = null)
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.ConexaoEfetuadaComSucesso };
            objetoMensagem.Propriedades.Add( Constantes.USUARIOS, usuarios );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.EDICOES_CRONOGRAMA, edicoes );
            if(!string.IsNullOrEmpty( autorEdicaoNomeCronograma ))
            {
                objetoMensagem.Propriedades.Add( Constantes.LOGIN_AUTOR_EDICAO_NOME_CRONOGRAMA, autorEdicaoNomeCronograma );
            }
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de NovoUsuarioConectado
        /// </summary>
        /// <param name="usuarios">string Nome(s) do(s) usuario(s)(Caso Mais do que um usuário separar por ',')</param>
        /// <param name="oidCronograma">string nome do cronograma</param>
        /// <returns>Objeto MensagemDto do tipo NovoUsuarioConectado</returns>
        public static MensagemDto RnCriarMensagemNovoUsuarioConectado( string usuarios, string oidCronograma )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.NovosUsuariosConectados };
            string[] vetorUsuarios;
            if(usuarios.Contains( ',' ))
                vetorUsuarios = usuarios.Split( ',' );
            else
                vetorUsuarios = new string[] { usuarios };
            objetoMensagem.Propriedades.Add( Constantes.USUARIOS, vetorUsuarios );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );

            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de NovoUsuarioConectado
        /// </summary>
        /// <param name="usuarios">Vetor armazenando o nome dos usuários conectados</param>
        /// <param name="oidCronograma">Nome do Cronograma Atual</param>
        /// <returns>Objeto MensagemDto do tipo NovoUsuarioConectado</returns>
        public static MensagemDto RnCriarMensagemNovoUsuarioConectado( string[] usuarios, string oidCronograma )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.NovosUsuariosConectados };
            objetoMensagem.Propriedades.Add( Constantes.USUARIOS, usuarios );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de UsuariosDesconectados
        /// </summary>
        /// <param name="usuarios">Vetor armazenando o nome dos usuários desconectados</param>
        /// <param name="oidCronograma">cronograma atual do qual foi desconectado</param>
        /// <returns>Uma MensagemDto de UsuáriosDesconectados</returns>
        public static MensagemDto RnCriarMensagemUsuarioDesconectado( string[] usuarios, string oidCronograma, bool forcarAtualizacao = false)
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.UsuarioDesconectado };
            objetoMensagem.Propriedades.Add( Constantes.USUARIOS, usuarios );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.FORCAR_ATUALIZACAO, forcarAtualizacao );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de UsuariosDesconectados
        /// </summary>
        /// <param name="login">Login do colaborador a ser desconectado</param>
        /// <param name="oidCronograma">cronograma atual do qual foi desconectado</param>
        /// <returns>Uma MensagemDto de UsuáriosDesconectados</returns>
        public static MensagemDto RnCriarMensagemUsuarioDesconectado( string login, string oidCronograma, bool forcarAtualizacao = false )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.UsuarioDesconectado };
            objetoMensagem.Propriedades.Add( Constantes.USUARIOS, new string[] { login } );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.FORCAR_ATUALIZACAO, forcarAtualizacao );

            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de aviso de que o servidor está desconectando
        /// </summary>
        /// <returns>MensagemDto do Tipo ServidorDesconectado</returns>
        public static MensagemDto RnCriarMensagemServidorDesconectando( string motivo )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.ServidorDesconectando };
            objetoMensagem.Propriedades.Add( Constantes.MOTIVO, motivo );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de aviso de que o um usuário está editando uma tarefa
        /// </summary>
        /// <param name="oidTarefa">guid da tarefa editada</param>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login colaborador que está editando a mensagem</param>
        /// <param name="autoSalvarEdicao">parametro para informar se deve ou não salvar imediatamente após receber autorização de edição</param>
        /// <returns>MensagemDto do tipo InicioEdicaoTarefa</returns>
        public static MensagemDto RnCriarMensagemInicioEdicaoTarefa( string oidTarefa, string login, string oidCronograma, string idRequisicao = "" )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.InicioEdicaoTarefa };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.OIDTAREFA, oidTarefa );
            objetoMensagem.Propriedades.Add( Constantes.ID_REQUISICAO, idRequisicao );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de aviso de que o um usuário está editando uma tarefa
        /// </summary>
        /// <param name="tarefaUsuarioDic">Dicionário de tarefas indexando o login do usuário ao valor do oidTarefa da tarefa que está sendo editada por ele</param>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <returns>MensagemDto do tipo InicioEdicaoTarefa</returns>
        public static MensagemDto RnCriarMensagemInicioEdicaoTarefaResumida( Dictionary<string, string> tarefaUsuarioDic, string oidCronograma )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.InicioEdicaoTarefa };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTORES_ACAO, tarefaUsuarioDic );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de aviso de que o um usuário já está editando uma tarefa
        /// </summary>
        /// <param name="oidTarefa">guid da tarefa editada</param>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login colaborador que está editando a mensagem</param>
        /// <param name="autoSalvarEdicao">parametro para informar se deve ou não salvar imediatamente após receber autorização de edição</param>
        /// <returns>MensagemDto do tipo InicioEdicaoTarefa</returns>
        public static MensagemDto RnCriarMensagemRecusarEdicaoTarefa( string oidTarefa, string login, string oidCronograma, string idRequisicao = "")
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.EdicaoTarefaRecusada };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.OIDTAREFA, oidTarefa );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.ID_REQUISICAO, idRequisicao );
            
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de aviso de que o um usuário terminou a edição de uma tarefa para libera-la
        /// </summary>
        /// <param name="oidTarefa">guid da tarefa editada</param>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <param name="login">login colaborador que está editando a mensagem</param>
        /// <returns>MensagemDto do tipo FinalizarEdicaoTarefa</returns>
        public static MensagemDto RnCriarMensagemFinalizarEdicaoTarefa( string oidTarefa, string login, string oidCronograma )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.EdicaoTarefaFinalizada };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.OIDTAREFA, oidTarefa );
            return objetoMensagem;
        }

        /// <summary>
        /// Responsável por criar uma MensagemDto de aviso de que o um usuário finalizou edição de uma tarefa
        /// </summary>
        /// <param name="tarefaUsuarioDic">Dicionário de tarefas indexando o login do usuário ao valor do oidTarefa da tarefa que foi editada por ele</param>
        /// <param name="oidCronograma">guid cronograma</param>
        /// <returns>MensagemDto do tipo FinalizarEdicaoTarefa</returns>
        public static MensagemDto RnCriarMensagemFinalizarEdicaoTarefaResumida( Dictionary<string, string> tarefaUsuarioDic, string oidCronograma )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.InicioEdicaoTarefa };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTORES_ACAO, tarefaUsuarioDic );
            return objetoMensagem;
        }


        /// <summary>
        /// Responsável por criar uma MensagemDto do tipo SolicitarExclusaoTarefa de aviso ao Manager de quais tarefas o usuário deseja que sejam excluídas
        /// </summary>
        /// <param name="tarefas">vetor de oidTarefa, com as tarefas que desejam que sejam excluídas</param>
        /// <param name="login">login do  colaborador que deseja excluir as tarefas</param>
        /// <param name="oidCronograma">guid do cronograma atual do colaborador</param>
        /// <returns>MensagemDto do tipo SolicitarExclusaoTarefa</returns>
        public static MensagemDto RnCriarMensagemInicioExclusaoTarefas( string[] tarefas, string login, string oidCronograma )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.ExclusaoTarefaIniciada };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS, tarefas );
            return objetoMensagem;
        }

        /// <summary>
        /// Mensagem utilizada pelo MultiAccessManager para sinalizar o MultiAccessClient de quais tarefas ele pode excluir 
        /// O manager retornará as tarefas excluindo os seguintes casos:
        /// a) A tarefa já se encontram em processo de exclusão por outro usuário então será recusada
        /// b) A tarefa se encontra atualmente em processo de edição por outro usuário, também será recusada
        /// </summary>
        /// <param name="tarefas">listagem de tarefas que podem ser excluidas</param>
        /// <param name="oidCronograma">guid do cronograma atual do colaborador</param>
        /// /// <param name="login">login do  colaborador que  excluiu as tarefas</param>
        /// <param name="tarefasEmEdicao">(Opcional) Caso haja tarefas que não podem ser excluidas por se encontrarem em edição </param>
        /// <returns>MensagemDto do tipo permissão</returns>
        public static MensagemDto RnCriarMensagemEfetuarExclusaoTarefas( string[] tarefas, string[] tarefasNaoExcluidas, string oidCronograma, string login )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.ExclusaoTarefaPermitida };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS, tarefas );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS_NAO_EXCLUIDAS, tarefasNaoExcluidas );
            return objetoMensagem;
        }

        /// <summary>
        /// Mensagem de comunicação de que determinadas tarefas foram excluídas, será comunicada aos outros usuários
        /// </summary>
        /// <param name="tarefas">lista de tarefas efetivamente excluidas</param>
        /// <param name="oidCronograma">guid do cronograma atual o qual teve tarefas excluídas</param>
        /// <param name="login">colaborador que excluíu as tarefas</param>
        /// <returns>Mensagem de confirmação de exclusão de tarefas</returns>
        public static MensagemDto RnCriarMensagemComunicarExclusaoTarefaConcluida( string[] tarefas, Dictionary<string, Int16> tarefasImpactadas, string oidCronograma, string login, DateTime dataHoraAcao, string[] tarefasNaoExcluidas = null )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.ExclusaoTarefaFinalizada };
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS_IMPACTADAS, tarefasImpactadas );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS, tarefas );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS_NAO_EXCLUIDAS, tarefasNaoExcluidas );
            objetoMensagem.Propriedades.Add( Constantes.DATAHORA_ACAO, dataHoraAcao );
            return objetoMensagem;
        }

        /// <summary>
        /// Método responsável por criar uma mensagem do tipo EdicaoTarefaAutorizada
        /// </summary>
        /// <param name="oidCronograma">oid do cronograma atual</param>
        /// <param name="login">login do colaborador</param>
        /// <param name="oidTarefa">oid da tarefa selecionada para edicao</param>
        /// <param name="autoSalvarEdicao">parametro para informar se deve ou não salvar imediatamente após receber autorização de edição</param>
        /// <returns>mensagem de permissão para editar uma tarefa</returns>
        public static MensagemDto RnCriarMensagemEdicaoTarefaAutorizada( string login, string oidCronograma, string oidTarefa, string idRequisicao = "" )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.EdicaoTarefaAutorizada };
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.OIDTAREFA, oidTarefa );
            objetoMensagem.Propriedades.Add( Constantes.ID_REQUISICAO, idRequisicao );
            return objetoMensagem;
        }

        /// <summary>
        /// método responsável por criar uma mensagem de movimentação da linha no grid
        /// </summary>
        /// <param name="posicaoAtual">NbId da posição inicial da tarefa</param>
        /// <param name="posicaoFinal">NbId da posição final da tarefa</param>
        /// <param name="oidTarefa">oid da tarefa a ser movida</param>
        /// <param name="oidTarefasImpactadas">dicionário de tarefas impactadas e suas novas posições</param>
        /// <param name="login">login do colaborador</param>
        /// <param name="oidCronograma">oid do cronograma atual</param>
        /// <returns>mensagem dto a ser enviada sobre a movimentação de uma linha no grid</returns>
        public static MensagemDto RnCriarMensagemMovimentacaoTarefa( Int16 posicaoAtual, Int16 posicaoFinal, string oidTarefa, Dictionary<string, Int16> oidTarefasImpactadas, string login, string oidCronograma, DateTime dataHoraAcao )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.MovimentacaoPosicaoTarefa };
            objetoMensagem.Propriedades.Add( "posicaoInicial", posicaoAtual );
            objetoMensagem.Propriedades.Add( "posicaoFinal", posicaoFinal );
            objetoMensagem.Propriedades.Add( Constantes.OIDTAREFA, oidTarefa );
            objetoMensagem.Propriedades.Add( Constantes.TAREFAS_IMPACTADAS, oidTarefasImpactadas );
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, login );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            objetoMensagem.Propriedades.Add( Constantes.DATAHORA_ACAO, dataHoraAcao );
            return objetoMensagem;
        }

        /// <summary>
        /// Método responsável por criar a mensagem que sinaliza a alteração do nome do cronograma atual
        /// </summary>
        /// <param name="oidCronograma">oid do cronograma atual</param>
		/// 
        /// <param name="loginAutor">login do autor da modificação</param>
        /// <returns></returns>
		public static MensagemDto RnCriarMensagemFimEdicaoDadosCronograma( string oidCronograma , string loginAutor )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.DadosCronogramaAlterados };
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, loginAutor );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            return objetoMensagem;
        }

        /// <summary>
        /// Método responsável por criar uma mensagem de solicitação de edição do nome do crongrama
        /// </summary>
        /// <param name="oidCronograma">oid de indentificação do crongrama</param>
        /// <param name="loginAutor">login do usuário autor da edição</param>
        /// <returns></returns>
        public static MensagemDto RnCriarMensagemInicioEdicaoNomeCronograma( string oidCronograma, string loginAutor ) 
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.InicioEdicaoNomeCronograma };
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, loginAutor );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            return objetoMensagem;
        }

        /// <summary>
        /// Método responsável por  criar uma mensagem de recusa de edição do nome do crongrama
        /// </summary>
        /// <param name="oidCronograma">oid de indentificação do crongrama</param>
        /// <param name="loginAutor">login do usuário autor da edição</param>
        /// <returns></returns>
        public static MensagemDto RnCriarMensagemRecusaEdicaoNomeCronograma( string oidCronograma, string loginAutor ) 
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.EdicaoNomeCronogramaRecusada };
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, loginAutor );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            return objetoMensagem;
        }

        /// <summary>
        /// Método responsável por criar uma mensagem de permissão de edição do nome do cronograma
        /// </summary>
        /// <param name="oidCronograma">oid de indentificação do crongrama</param>
        /// <param name="loginAutor">login do usuário autor da edição</param>
        /// <param name="autoSalvarEdicao">definir se a edição deve ser automaticamente salva quando receber a permissão</param>
        /// <returns></returns>
        public static MensagemDto RnCriarMensagemPermitirEdicaoNomeCronograma( string oidCronograma, string loginAutor )
        {
            objetoMensagem = new MensagemDto() { Tipo = CsTipoMensagem.EdicaoNomeCronogramaPermitida };
            objetoMensagem.Propriedades.Add( Constantes.AUTOR_ACAO, loginAutor );
            objetoMensagem.Propriedades.Add( Constantes.OIDCRONOGRAMA, oidCronograma );
            return objetoMensagem;
        }

        #endregion

        /// <summary>
        /// Serializar uma MensagemDto
        /// </summary>
        /// <param name="objetoMensagem">Objeto Mensagem a ser serializado</param>
        /// <returns>Objeto convertido em string Json</returns>
        public static string Serializar( MensagemDto objetoMensagem )
        {
            return JsonConvert.SerializeObject( objetoMensagem );
        }

        /// <summary>
        /// Deserializar uma MensagemDto
        /// </summary>
        /// <param name="mensagemJson">string Json a Ser deserializada</param>
        /// <returns>Um objeto MensagemDto</returns>
        public static MensagemDto Deserializar( string mensagemJson )
        {
            return JsonConvert.DeserializeObject<MensagemDto>( mensagemJson );
        }

        /// <summary>
        /// Deserializar um array json vindo na hashtable propriedades
        /// Solução para o Bug do Json.Net que não deserializar diretamente um vetor dentro de um hashtable
        /// Devido aos valores do Hashtable serem fracamente tipados
        /// </summary>
        /// <param name="mensagem">MensagemDto</param>
        /// <returns>Vetor com o nome dos usuários contidos na mensagem</returns>
        public static string[] ExtrairUsuariosMensagemDto( MensagemDto mensagem )
        {
            return JsonConvert.DeserializeObject<string[]>( mensagem.Propriedades[Constantes.USUARIOS].ToString());
        }

        public static void CorrigirDeserializacaoPropriedade<TResult>(MensagemDto mensagem,string nomePropriedade)
        {
            if(!mensagem.Propriedades.ContainsKey( nomePropriedade ) || mensagem.Propriedades[nomePropriedade] == null)
                return;
            mensagem.Propriedades[nomePropriedade] = JsonConvert.DeserializeObject<TResult>( mensagem.Propriedades[nomePropriedade].ToString() );
        }

        /// <summary>
        /// Método utilizado para efetuar a deserialização de uma MensagemDto
        /// </summary>
        /// <param name="mensagemJson">MensagemDto serializada em Json</param>
        /// <returns>Objeto MensagemDto deserializado, devidamente tratado</returns>
        public static MensagemDto DeserializarMensagemDto( string mensagemJson )
        {
            MensagemDto mensagemDto = JsonConvert.DeserializeObject<MensagemDto>( mensagemJson );
            switch(mensagemDto.Tipo)
            {
                case CsTipoMensagem.NovosUsuariosConectados:
                case CsTipoMensagem.UsuarioDesconectado:
                case CsTipoMensagem.ConexaoEfetuadaComSucesso:
                    CorrigirDeserializacaoPropriedade<string[]>( mensagemDto, Constantes.USUARIOS );
                    CorrigirDeserializacaoPropriedade<Dictionary<string, string>>( mensagemDto, Constantes.EDICOES_CRONOGRAMA );
                    break;
                case CsTipoMensagem.ExclusaoTarefaIniciada:
                    CorrigirDeserializacaoPropriedade<string[]>( mensagemDto, Constantes.TAREFAS );
                    break;
                case CsTipoMensagem.ExclusaoTarefaPermitida:
                case CsTipoMensagem.ExclusaoTarefaFinalizada:
                    CorrigirDeserializacaoPropriedade<string[]>( mensagemDto, Constantes.TAREFAS );
                    CorrigirDeserializacaoPropriedade<string[]>( mensagemDto, Constantes.TAREFAS_NAO_EXCLUIDAS );
                    CorrigirDeserializacaoPropriedade<Dictionary<string, Int16>>( mensagemDto, Constantes.TAREFAS_IMPACTADAS );
                    break;
                case CsTipoMensagem.MovimentacaoPosicaoTarefa:
                    mensagemDto = EfetuarTratamentoExtracaoVetorDaMensagem( mensagemDto, Constantes.USUARIOS );
                    mensagemDto = EfetuarTratamentoExtracaoDicionarioDaMensagem<string, Int16>( mensagemDto, Constantes.TAREFAS_IMPACTADAS );
                    break;
                case CsTipoMensagem.NovaTarefaCriada:
                    mensagemDto = EfetuarTratamentoExtracaoDicionarioDaMensagem<string, Int16>( mensagemDto, Constantes.TAREFAS_IMPACTADAS );
                    break;
                case CsTipoMensagem.InicioEdicaoTarefa:
                case CsTipoMensagem.EdicaoTarefaFinalizada:
                    mensagemDto = EfetuarTratamentoDeserializacaoMensagemEdicaoTarefa( mensagemDto );
                    break;
            }
            return mensagemDto;
        }

        /// <summary>
        /// Método responsável por efetuar o tratamento do dicionário das mensagens de Edição Tarefa quando resumidas
        /// </summary>
        /// <param name="mensagem">MensagemDto Com a possivel falha de deserialização no dicionário</param>
        /// <returns>MensagemDto de Edicao Tarefa Quando Resumida Corrigida</returns>
        public static MensagemDto EfetuarTratamentoDeserializacaoMensagemEdicaoTarefa( MensagemDto mensagem )
        {
            if(mensagem.Propriedades.ContainsKey( Constantes.AUTORES_ACAO ) && mensagem.Propriedades[Constantes.AUTORES_ACAO] != null)
                mensagem = EfetuarTratamentoExtracaoDicionarioDaMensagem<string, string>( mensagem, Constantes.AUTORES_ACAO );
            return mensagem;
        }

        /// <summary>
        /// Responsável por efetuar correção da serialização de um vetor contido em algum indice da hashtable
        /// </summary>
        /// <param name="mensagem">MensagemDto que possua uma Hashtable com um vetor armazenado</param>
        /// /// <param name="indice">Indice da Hashtable que possui um vetor armazenado</param>
        /// <returns>MensagemDto com serialização Corrigida</returns>
        private static MensagemDto EfetuarTratamentoExtracaoVetorDaMensagem( MensagemDto mensagem, string indice )
        {
            if(mensagem.Propriedades.ContainsKey( indice ) && mensagem.Propriedades[indice] != null)
                mensagem.Propriedades[indice] = JsonConvert.DeserializeObject<string[]>( mensagem.Propriedades[indice].ToString() );
            return mensagem;
        }

        /// <summary>
        /// Responsável por efetuar correção da serialização de um vetor contido em algum indice da hashtable
        /// </summary>
        /// <param name="mensagem">MensagemDto que possua uma Hashtable com um vetor armazenado</param>
        /// /// <param name="indice">Indice da Hashtable que possui um vetor armazenado</param>
        /// <returns>MensagemDto com serialização Corrigida</returns>
        public static MensagemDto EfetuarTratamentoExtracaoDicionarioDaMensagem<T1, T2>( MensagemDto mensagem, string indice )
        {
            if(mensagem.Propriedades.ContainsKey( indice ) && mensagem.Propriedades[indice] != null)
                mensagem.Propriedades[indice] = JsonConvert.DeserializeObject<Dictionary<T1, T2>>( mensagem.Propriedades[indice].ToString() );
            return mensagem;
        }

        /// <summary>
        /// Responsável por replicar a mensagem evitando a herança de referencia
        /// do  objeto MensagemDto Original
        /// </summary>
        /// <param name="mensagem">MensagemDto original</param>
        /// <returns>Cópia Da MensagemDto com referencia diferente da referancia do objeto original</returns>
        public static MensagemDto CopiarMensagemDto( MensagemDto mensagem )
        {
            return new MensagemDto { Tipo = mensagem.Tipo, Propriedades = new Hashtable( mensagem.Propriedades ) };
        }
    }
}
