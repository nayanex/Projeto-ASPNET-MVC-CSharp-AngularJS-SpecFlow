using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.MultiAccess.Library.Domains;

namespace WexProject.Schedule.Test.Utils
{
    public class StepContextUtil
    {
        private const string KeyAccessClient = "AccessClient";
        private const string SERVIDOR = "Servidor";
        private const string RECEBEU_ATUALIZACAO_EDICAO = "_RecebeuAtualizacaoTarefa_";
        private const string EM_EDICAO_PARA = "_EmEdicaoPara_";

        #region Geradores de Keys
        /// <summary>
        /// Método padrão para criar uma key única para salvar no ScenarioContext
        /// </summary>
        /// <param name="login">login do accessClient</param>
        /// <param name="oidCronograma">oid do cronograma accessClient</param>
        /// <returns></returns>
        private static string CriarWexMultiAccessClientKey( string login, string oidCronograma )
        {
            return CriarKeyEvento( KeyAccessClient, login, oidCronograma );
        }

        /// <summary>
        /// Método para criar Key de Evento de usuário desconectado
        /// </summary>
        /// <param name="loginAcessClient">login do access client que recebeu a mensagem</param>
        /// <param name="oidCronograma">oid cronograma em que o access client está conectado</param>
        /// <param name="loginDesconectado">login do colaborador que se desconectou</param>
        /// <returns>retornar uma string que será a key</returns>
        public static string CriarKeyEventoUsuarioDesconectado(string loginAcessClient,string oidCronograma,string loginDesconectado) 
        {
            if(string.IsNullOrEmpty( loginAcessClient ) || string.IsNullOrEmpty( oidCronograma ) || string.IsNullOrEmpty( loginDesconectado ))
                return null;
            return CriarKeyEvento( loginAcessClient, oidCronograma, loginDesconectado, CsTipoMensagem.UsuarioDesconectado );
        }

        /// <summary>
        /// Método para criar uma Key de identificação para o disparo de evento de nova tarefa criada
        /// </summary>
        /// <param name="loginAcessClient">login access client que disparou o evento</param>
        /// <param name="oidCronograma">oid cronograma que o access client esta conectado</param>
        /// <param name="oidNovaTarefa">login do usuário que criou a tarefa</param>
        /// <returns></returns>
        public static string CriarKeyEventoAoSerCriadaNovaTarefa( string loginAcessClient, string oidCronograma, string oidNovaTarefa )
        {
            if(string.IsNullOrEmpty( loginAcessClient ) || string.IsNullOrEmpty( oidCronograma ) || string.IsNullOrEmpty( oidNovaTarefa ))
                return null;
            return CriarKeyEvento( loginAcessClient, oidCronograma, oidNovaTarefa, CsTipoMensagem.NovaTarefaCriada );
        }

        /// <summary>
        /// Método para criar Key de Evento de servidor desconectado
        /// </summary>
        /// <param name="loginAcessClient">login do access client que recebeu a mensagem</param>
        /// <param name="oidCronograma">oid cronograma em que o access client está conectado</param>
        /// <returns>retornar uma string que será a key</returns>
        public static string CriarKeyEventoServidorDesconectado( string loginAcessClient, string oidCronograma )
        {
            if(string.IsNullOrEmpty( loginAcessClient ) || string.IsNullOrEmpty( oidCronograma ))
                return null;
            return CriarKeyEvento( loginAcessClient, oidCronograma, CsTipoMensagem.ServidorDesconectando );
        }

        public static string CriarKeyEventoAoIniciarEdicaoTarefa( string loginAcessClient, string oidCronograma,string autorEdicao ) 
        {
            if(string.IsNullOrEmpty( loginAcessClient ) || string.IsNullOrEmpty( oidCronograma ) || string.IsNullOrEmpty( autorEdicao ))
                return null;
            return CriarKeyEvento( loginAcessClient, oidCronograma, CsTipoMensagem.InicioEdicaoNomeCronograma, autorEdicao );
        }

        /// <summary>
        /// Método responsável por retorar uma chave composta 
        /// </summary>
        /// <param name="loginAcessClient">login do access client comunidado do fim da edição</param>
        /// <param name="oidTarefa">oid da tarefa editada</param>
        /// <param name="autorEdicao">login do autor da edição</param>
        /// <returns>uma chave composta formada por loginAccessClient + oidTarefaEmEdicao + 'EdicaoTarefaFinalizada' + autorEdicao</returns>
        public static string CriarKeyEventoFimEdicaoTarefa( string loginAcessClient, string oidTarefa, string autorEdicao )
        {
            if(string.IsNullOrEmpty( loginAcessClient ) || string.IsNullOrEmpty( oidTarefa ) || string.IsNullOrEmpty( autorEdicao ))
                return null;
            return CriarKeyEvento( loginAcessClient, oidTarefa,CsTipoMensagem.EdicaoTarefaFinalizada, autorEdicao );
        }

        /// <summary>
        /// Método para retornar uma chave composta sobre a atualização de uma tarefa editada
        /// </summary>
        /// <param name="loginAccessClient"></param>
        /// <param name="oidTarefa"></param>
        /// <returns></returns>
        public static string CriarKeyRecebeuAtualizacaoEdicaoTarefa( string loginAccessClient, string oidTarefa ) 
        {
            return CriarKeyEvento( loginAccessClient,RECEBEU_ATUALIZACAO_EDICAO , oidTarefa );
        }

        /// <summary>
        /// Método para retornar uma chave composta para comprovar que um determinado colaborador foi comunicado sobre as tarefas que já se encontravam em edição
        /// antes de que o colaborador efetuasse a conexão
        /// </summary>
        /// <param name="loginComunicado">login que foi comunicado</param>
        /// <param name="oidTarefa">tarefa que se encontravam em edição</param>
        /// <param name="autorEdicao">autor da edição</param>
        /// <returns></returns>
        public static string CriarKeyTarefaJaEstavamEmEdicao( string loginComunicado, string oidTarefa, string autorEdicao )
        {
            return CriarKeyEvento( loginComunicado, oidTarefa, EM_EDICAO_PARA, autorEdicao );
        }

        /// <summary>
        /// Método para retornar uma chave composta por 3 valores de identificação
        /// </summary>
        /// <param name="valor1">primeiro valor da chave</param>
        /// <param name="valor2">segundo valor da chave</param>
        /// <param name="valor3">terceiro valor da chave</param>
        /// <returns>uma chave composta para indexar a instância algum objeto compartilhado pelo cenário atual no ScenarioContext.Current</returns>
        private static string CriarKeyEvento( object valor1, object valor2, object valor3 )
        {
            return string.Format( "{0}_{1}_{2}", valor1, valor2, valor3 );
        }

        /// <summary>
        /// Método para retornar uma chave composta por 4 valores de identificação
        /// </summary>
        /// <param name="valor1">primeiro valor da chave</param>
        /// <param name="valor2">segundo valor da chave</param>
        /// <param name="valor3">terceiro valor da chave</param>
        /// <param name="valor4">quarto valor da chave</param>
        /// <returns>uma chave composta para indexar a instância algum objeto compartilhado pelo cenário atual no ScenarioContext.Current</returns>
        private static string CriarKeyEvento( object valor1, object valor2, object valor3, object valor4 )
        {
            return string.Format( "{0}_{1}_{2}_{3}", valor1, valor2, valor3, valor4 );
        }
        #endregion

        #region Get
        /// <summary>
        /// Pegar o WexMultiAccessClientAtual a partir do contexto do cenário
        /// </summary>
        /// <param name="login">login do accessClient</param>
        /// <param name="oidCronograma">oid do cronograma</param>
        /// <returns></returns>
        public static WexMultiAccessClientMock GetAccessClientNoContexto( string login, string oidCronograma )
        {
            string key = CriarWexMultiAccessClientKey( login, oidCronograma );
            return (WexMultiAccessClientMock)ScenarioContext.Current[key];
        }

        /// <summary>
        /// Método para buscar no Contexto do cenário a isntância do servidor
        /// </summary>
        /// <returns></returns>
        public static WexMultiAccessManagerMock GetInstanciaManager()
        {
            return (WexMultiAccessManagerMock)ScenarioContext.Current[SERVIDOR];
        }
        #endregion

        #region Set
        /// <summary>
        /// Salvar um WexMultiAccessClient no ScenarioContext.Current para compartilhar com outros steps
        /// </summary>
        /// <param name="client"></param>
        public static void SalvarAccessClientNoContextoDoCenario( WexMultiAccessClientMock client )
        {
            string key = CriarWexMultiAccessClientKey( client.Login, client.OidCronograma );
            ScenarioContext.Current.Set( client, key );
        }

        /// <summary>
        /// Salvar no contexto do cenário a instância do manager
        /// </summary>
        /// <param name="manager">Instancia criada para o cenário atual</param>
        public static void SalvarInstanciaManager(WexMultiAccessManagerMock manager) 
        {
            ScenarioContext.Current.Set( manager, SERVIDOR );
        }

        /// <summary>
        /// Salvar no Contexto uma Key Gerada pelo disparo de um evento
        /// </summary>
        public static void SalvarKey( string key )
        {
            ScenarioContext.Current.Set( true,key );
        }
        #endregion

        /// <summary>
        /// Método retorna se existe ou não a ocorrencia da chave no Contexto do Cenário atual
        /// </summary>
        /// <param name="key">chave que deve existir</param>
        /// <returns></returns>
        public static bool CenarioAtualContemAChave(string key) 
        {
            return ScenarioContext.Current.ContainsKey( key );
        }
    }
}
