using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace WexProject.Module.Win.Services.Geral
{
    public class GeralService
    {
        private static string BASE_URL = ConfigurationManager.AppSettings.Get( "RestWebServicePath" );

        #region Regras de Negócio

        /// <summary>
        /// Método responsável por enviar a solicitação para salvar o último projeto selecionado pelo colaborador.
        /// </summary>
        /// <param name="oidColaborador">Oid do colaborador</param>
        /// <param name="oidProjeto">Oid do projeto</param>
        public static void SalvarUltimoProjetoSelecionado( Guid oidColaborador, Guid oidProjeto )
        {
            //PUT: {base_url}/Colaboradores/UltimoProjetoSelecionado?oidColaborador={oidColaborador}&oidProjeto=?{oidProjeto}
            RestClient restClient = new RestClient( BASE_URL );
            RestRequest requisicao = new RestRequest( "Colaboradores/UltimoProjetoSelecionado" );
            requisicao.RequestFormat = DataFormat.Json;
            requisicao.AddParameter( "OidColaborador", oidColaborador );
            requisicao.AddParameter( "OidProjeto", oidProjeto );
            restClient.Put( requisicao );
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Método responsável por acessar o serviço e deserializar o retorno.
        /// </summary>
        /// <param name="oidColaborador">Oid do colaborador</param>
        /// <returns>Oid do projeto</returns>
        public static Guid ConsultarUltimoProjetoSelecionado( Guid oidColaborador )
        {
            //PUT: {base_url}/Colaboradores/UltimoProjetoSelecionado?oidColaborador={oidColaborador}&oidProjeto=?{oidProjeto}
            RestClient restClient = new RestClient( BASE_URL );
            RestRequest requisicao = new RestRequest( "Colaboradores/UltimoProjetoSelecionado" );
            requisicao.RequestFormat = DataFormat.Json;
            requisicao.AddParameter( "id", oidColaborador );

            return JsonConvert.DeserializeObject<Guid>( restClient.Get( requisicao ).Content );
        }

        #endregion
    }
}
