using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using System.Data.Entity;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class CronogramaColaboradorConfigDao
    {
        #region Consultar

        /// <summary>
        /// Método para retornar as configurações de um colaborador em um determinado cronograma
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="login">login do colaborador</param>
        /// <param name="oidCronograma">oid do cronograma atual</param>
        /// <returns>ColaboradorCronogramaConfig com as configurações do colaborador no cronograma</returns>
        public static CronogramaColaboradorConfig ConsultarCronogramaColaboradorConfig( WexDb contexto, string login, Guid oidCronograma )
        {
            return contexto.CronogramaColaboradorConfig.FirstOrDefault( o => o.OidCronograma == oidCronograma && o.Colaborador.Usuario.UserName == login );
        }

        /// <summary>
        /// Método responsável por efetuar a listagem das cores escolhidas para um determinado cronograma
        /// </summary>
        /// <param name="contexto">Contexto do Banco</param>
        /// <param name="oidCronograma">Oid de Identificação do cronograma</param>
        /// <returns>lista de cores já cadastradas no banco</returns>
        public static List<string> ConsultarCoresPorCronograma( WexDb contexto, Guid oidCronograma )
        {
            List<string> coresArmazenadas;
            var cores = new List<int?>();

            cores = contexto.CronogramaColaboradorConfig.Where( o => o.OidCronograma == oidCronograma && o.Cor != null ).Select( o => o.Cor ).ToList();

            coresArmazenadas = cores.Select( o => o.Value.ToString() ).ToList();

            return coresArmazenadas;
        }

        /// <summary>
        /// Método responsável por retornar uma lista de CronogramaColaboradorConfig dos colaboradores para um determinado cronograma
        /// </summary>
        /// <param name="logins">Lista de logins dos Colaboradores Atuais</param>
        /// <param name="oidCronograma">Oid de identificação do cronograma atual</param>
        /// <returns>Lista de CronogramaColaboradorConfig</returns>
        public static List<CronogramaColaboradorConfig> ConsultarCronogramaColaboradorConfig( List<string> logins, 
            Guid oidCronograma )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                logins = logins.Distinct().ToList();

                try
                {
                    return contexto.CronogramaColaboradorConfig.Include( o => o.Colaborador.Usuario.Person.Party ).Where( o => o.OidCronograma == oidCronograma && logins.Contains( o.Colaborador.Usuario.UserName ) ).ToList();
                }
                catch(Exception e)
                {
                    var exceptionComStackTrace = new Exception( String.Format( "Mensagem: {0} - StackTrace: {1}", e.Message, e.StackTrace ) );

                    throw exceptionComStackTrace;
                }
            }
        }

        /// <summary>
        /// Método responsável por retornar todos os CronogramaColaboradorConfigs encontrados em um determinado cronograma
        /// </summary>
        /// <param name="contexto">contexto de conexão com o banco</param>
        /// <param name="oidCronograma">oid de identificação do cronograma atual</param>
        /// <returns>retornar uma coleção de CronogramaColaboradorConfig</returns>
        public static List<CronogramaColaboradorConfig> ConsultarTodosCronogramaColaboradorConfig( WexDb contexto, Guid oidCronograma )
        {
            return contexto.CronogramaColaboradorConfig.Where( o => o.OidCronograma == oidCronograma ).ToList();
        }

        #endregion

        #region Salvar

        /// <summary>
        /// Método utilizado para criar uma configuração para um colaborador em um determinado cronograma
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="login"></param>
        /// <param name="oidCronograma">oid do cronograma atual</param>
        /// <returns>ColaboradorCronogramaConfig para armazenar as configurações do colaborador</returns>
        public static CronogramaColaboradorConfig SalvarCronogramaColaboradorConfig( WexDb contexto, string login, Guid oidCronograma )
        {
            Cronograma cronograma = CronogramaDao.ConsultarCronogramaPorOid( oidCronograma );
            if(cronograma == null)
                return null;

            Colaborador colaborador = ColaboradorDAO.ConsultarColaborador( login );

            if(colaborador == null)
                return null;

            CronogramaColaboradorConfig config = ConsultarCronogramaColaboradorConfig( contexto, login, oidCronograma );

            if(config != null)
                return config;

            config = new CronogramaColaboradorConfig()
            {
                OidCronograma = cronograma.Oid,
                OidColaborador = colaborador.Oid
            };

            contexto.CronogramaColaboradorConfig.Add( config );
            contexto.SaveChanges();

            return config;
        }

        #endregion

        #region Excluir


        /// <summary>
        /// Método responsável por excluir todos cronograma colaborador configs do cronograma atual
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="oidCronogramaAtual">Oid do cronograma</param>
        public static void ExcluirTodosConfigsCronogramaAtual( WexDb contexto, Guid oidCronogramaAtual )
        {
            List<CronogramaColaboradorConfig> colecao = CronogramaColaboradorConfigDao.ConsultarTodosCronogramaColaboradorConfig( contexto, oidCronogramaAtual );

            if(colecao != null && colecao.Count > 0)
            {
                int contador = colecao.Count;
                for(int i = 0; i < contador; i++)
                {
                    CronogramaColaboradorConfig ultimaSelecao = colecao[i];
                    contexto.CronogramaColaboradorConfig.Remove( ultimaSelecao );
                    contexto.SaveChanges();
                }
            }
        }

        #endregion
    }
}
