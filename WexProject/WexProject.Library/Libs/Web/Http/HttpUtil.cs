using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.Web.Http
{
    public class HttpUtil
    {
        /// <summary>
        /// Método responsável por realizar uma requisição via GET síncrona.
        /// </summary>
        /// <param name="url">url completa com parametros</param>
        /// <returns>String (Json)</returns>
        public string HttpGet( string url )
        {
            HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create( url );
            requisicao.Method = "GET";
            requisicao.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)requisicao.GetResponse();

            using(StreamReader reader = new StreamReader( response.GetResponseStream() ))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Método responsável por realizar uma requisição via POST síncrona.
        /// </summary>
        /// <param name="url">url completa</param>
        /// <param name="json">Parâmetro serializado ( Json )</param>
        /// <returns>String (Json)</returns>
        public string HttpPost( string url, string json )
        {
            HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create( url );
            requisicao.Proxy = null;
            requisicao.ContentType = "application/json";
            requisicao.Method = "POST";

            byte[] bytes = Encoding.ASCII.GetBytes( json );

            requisicao.ContentLength = bytes.Length;

            Stream stream = requisicao.GetRequestStream();
            stream.Write( bytes, 0, bytes.Length );
            stream.Close();

            HttpWebResponse resposta = (HttpWebResponse)requisicao.GetResponse();

            if(resposta == null)
                return null;

            StreamReader streamReader = new StreamReader( resposta.GetResponseStream() );

            return streamReader.ReadToEnd().Trim();
        }

		/// <summary>
		/// Efetuar uma requisição do tipo delete para remoção de dados
		/// </summary>
		/// <param name="url">url para ser efetuada a requisição</param>
		/// <returns></returns>
		public string HttpDelete(string url) 
		{
			HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create( url );
			requisicao.ContentType = "application/json";
			requisicao.Method = "DELETE";
			HttpWebResponse response = (HttpWebResponse)requisicao.GetResponse();
			using(StreamReader reader = new StreamReader( response.GetResponseStream() ))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Efetuar uma requisição do tipo put para edição de dados
		/// </summary>
		/// <param name="url">url para ser efetuada a requisição</param>
		/// <returns></returns>
		public string HttpPut( string url ,string json )
		{
			HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create( url );
			requisicao.ContentType = "application/json";
			requisicao.Method = "PUT";
			byte[] bytes = Encoding.ASCII.GetBytes( json );
			requisicao.ContentLength = bytes.Length;
			Stream stream = requisicao.GetRequestStream();
			stream.Write( bytes, 0, bytes.Length );
			stream.Close();
			HttpWebResponse response = (HttpWebResponse)requisicao.GetResponse();
			using(StreamReader reader = new StreamReader( response.GetResponseStream() ))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Efetuar uma requisição do tipo put para edição de dados
		/// </summary>
		/// <param name="url">url para ser efetuada a requisição</param>
		/// <returns></returns>
		public void HttpPut( string url)
		{
			HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create( url );
			requisicao.ContentType = "application/json";
			requisicao.Method = "PUT";
			HttpWebResponse response = (HttpWebResponse)requisicao.GetResponse();
		}
    }
}
