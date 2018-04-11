using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;
using Newtonsoft.Json;
using WexProject.BLL.Shared.DTOs.Rh;
using Newtonsoft.Json.Linq;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using WexProject.Library.Libs.Web.Http;
using RestSharp;
using System.Configuration;

namespace WexProject.Schedule.Library.ServiceUtils
{
    public class GeralServiceUtil : IGeralServiceUtil
    {
        #region Constantes

        public const string REST_SERVICE_PATH = "RestWebServicePath";
        public string BASE_URL { get; private set; } 

        #endregion

        #region Atributos 

        private IRestClient restClient;

        #endregion

        public GeralServiceUtil()
        {
            BASE_URL = ConfigurationManager.AppSettings.Get( REST_SERVICE_PATH );
            restClient = new RestClient( BASE_URL );
        }

        #region Consultas

        /// Método responsável por acessar o serviço e buscar o colaborador logado pelo login do usuário
        /// </summary>
        /// <param name="login">Login do usuário</param>
        /// <returns>Objeto Dto de Colaborador</returns>
        public ColaboradorDto ConsultarColaboradorLogado( string login )
        {
            RestRequest requisicao = new RestRequest( "Colaboradores" );
            requisicao.AddParameter( "login", login );
            string json = restClient.Get( requisicao ).Content;

            return JsonConvert.DeserializeObject<ColaboradorDto>( json );
        }

        /// <summary>
        /// Método responsável por retornar uma SemanaTrabalhoDto preenchida com as informações padrão de uma semana de trabalho
        /// seus dias e suas respectivas cargas horárias
        /// </summary>
        /// <returns></returns>
        public SemanaTrabalhoDto ConsultarSemanaDeTrabalhoPadrao()
        {
            RestRequest requisicao = new RestRequest( "SemanaTrabalho" );
            string retorno = restClient.Get( requisicao ).Content;

            return JsonConvert.DeserializeObject<SemanaTrabalhoDto>( retorno );
        }

        #endregion
    }
}
