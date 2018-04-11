using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using RestSharp;
using WexProject.BLL.Shared.DTOs.Escopo;
using WexProject.Library.Libs.Json;

namespace WexProject.Module.Win.Services.Escopo
{
    public class EscopoService
    {
        private static string BASE_URL = ConfigurationManager.AppSettings.Get( "RestWebServicePath" );

        #region Consultas

        /// <summary>
        /// Método responsável por acessar o serviço e buscar os dados do gráfico de acordo com o oid do projeto
        /// </summary>
        /// <param name="oidProjeto">Oid do projeto</param>
        /// <returns>Lista de Dto</returns>
        public static List<GraficoEstimadoRealizadoDTO> GetDadosGraficoEstimadoVsRealizadoProjeto( Guid oidProjeto )
        {
            RestClient client = new RestClient( BASE_URL );
            RestRequest requisicao = new RestRequest( "GraficoEstimadoRealizado" );
            requisicao.AddParameter( "id", oidProjeto );
            string json = client.Get( requisicao ).Content;

            return JsonUtil.Deserialize<List<GraficoEstimadoRealizadoDTO>>( json );
        }

        /// <summary>
        /// Método responsável por acessar o serviço e buscar os dados do gráfico de acordo com o oid do projeto
        /// </summary>
        /// <param name="oidProjeto">Oid do projeto</param>
        /// <returns>Lista de Dto</returns>
        public static List<GraficoEscopoCompletudeDTO>  GetDadosGraficoEscopoCompletude( Guid oidProjeto )
        {
            RestClient client = new RestClient( BASE_URL );
            RestRequest requisicao = new RestRequest( "GraficoEscopoCompletude" );
            requisicao.AddParameter( "id", oidProjeto );
            string json = client.Get( requisicao ).Content;

            return JsonUtil.Deserialize<List<GraficoEscopoCompletudeDTO>>( json );
        }

        #endregion
    }
}
