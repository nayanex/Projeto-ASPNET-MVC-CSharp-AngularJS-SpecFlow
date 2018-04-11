using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Globalization;

namespace WexProject.Library.Libs.Json
{
    /// <summary>
    /// Classe Resposável por tratar a Serialização de DTO's para Json
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// Resposável por enviar dados a serem serializados
        /// </summary>
        /// <typeparam name="T">Tipo de Dado a ser serializado</typeparam>
        /// <param name="data">Dado tipado</param>
        /// <returns>string serializada</returns>
        public static string Serialize<T>(this T data)
        {
            return SerializarInformacoes(data, new string[]{}, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="excluidos"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T data, string[] excluidos)
        {
            if (excluidos == null) {
                return SerializarInformacoes(data, new string[]{}, true);
            } else {
                return SerializarInformacoes(data, excluidos, true);
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="allNodes"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T data, bool allNodes)
        {
            return SerializarInformacoes(data, new string[] {}, allNodes);
        }
        /// <summary>
        /// Método Responsável pela serialização das informações
        /// </summary>
        /// <typeparam name="T">Tipo de Informação a ser serializada</typeparam>
        /// <param name="data">informação a ser serializada</param>
        /// <param name="excluidos">Vetor de informações que não são necessárias serem serializadas</param>
        /// <param name="all"></param>
        /// <returns>Retornar uma string Json dos dados de entrada</returns>
        private static string SerializarInformacoes<T>(this T data, string[] excluidos, bool all) {
            
            if (data == null)
                return string.Empty;

            StringBuilder result = new StringBuilder("");
            if (typeof(IEnumerable).IsAssignableFrom(data.GetType()) && data.GetType() != typeof(string)) {

                result = SerializarEnumerado<T>(data, excluidos, all, result);

            } else {

                result = SerializarObjeto<T>(data, excluidos, all, result);
            }
            
            return result.ToString();
        }
        /// <summary>
        /// Responsável por serializar objetos
        /// </summary>
        /// <typeparam name="T">Tipo de Objeto</typeparam>
        /// <param name="data">Objeto</param>
        /// <param name="excluidos">Vetor de nome dos atributos a serem desconsiderados na serialização</param>
        /// <param name="all"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static StringBuilder SerializarObjeto<T>(T data, string[] excluidos, bool all, StringBuilder result)
        {
            result.Append("{");
            result.Append(addValue(data, excluidos, all));
            if (result.ToString().EndsWith(","))
            {
                result = new StringBuilder(result.ToString().Substring(0, result.Length - 1));
            }
            result.Append("}");
            return result;
        }

        private static StringBuilder SerializarEnumerado<T>(T data, string[] excluidos, bool all, StringBuilder result)
        {
            result.Append("[");

            foreach (Object classe in data as IEnumerable)
            {
                result.Append("{");
                result.Append(addValue(classe, excluidos, all));
                if (result.ToString().EndsWith(","))
                {
                    result = new StringBuilder(result.ToString().Substring(0, result.Length - 1));
                }
                result.Append("}");
                result.Append(",");
            }
            if (result.ToString().EndsWith(","))
            {
                result = new StringBuilder(result.ToString().Substring(0, result.Length - 1));
            }
            result.Append("]");
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="excluidos"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        private static string addValue<T>(this T data, string[] excluidos, bool all)
        {
            StringBuilder result = new StringBuilder("");
            foreach (var item in data.GetType().GetProperties()) {
                PropertyInfo pi = (PropertyInfo) item;
                var value = pi.GetValue( data, null );
                if (value != null && (!excluidos.Contains(item.Name))) {
                    if (typeof(IEnumerable).IsAssignableFrom(value.GetType()) && value.GetType() != typeof(string)) {
                        if (all) {
                            result.Append("\"" + item.Name + "\":" + SerializarInformacoes(value, excluidos, all));
                        } else {
                            result.Append("\"" + item.Name + "\":[]");
                        }
                    } else {
                        if (item.PropertyType == typeof(string) || item.PropertyType == typeof(Guid)) {
                            result.Append("\"" + item.Name + "\":\"" + value + "\"");
                        } else if (item.PropertyType == typeof(double) || item.PropertyType == typeof(double?) ) {
                            result.Append("\"" + item.Name + "\":" + (double.Parse(value.ToString())).ToString(CultureInfo.InvariantCulture));
                        } else if (item.PropertyType == typeof(int) || item.PropertyType == typeof(int?) ) {
                            result.Append("\"" + item.Name + "\":" + (int.Parse(value.ToString())).ToString(CultureInfo.InvariantCulture));
                        } else if (item.PropertyType == typeof(bool) || item.PropertyType == typeof(bool?) 
                                    || item.PropertyType == typeof(Boolean) || item.PropertyType == typeof(Boolean?) ) {
                                result.Append("\"" + item.Name + "\":" + value.ToString().ToLower());
                        }else {
                            if (all) {
                                result.Append("\"" + item.Name + "\":" + SerializarInformacoes(value, excluidos, all));
                            } else {
                                result.Append("\"" + item.Name + "\":");
                            }
                        }
                    }
                    result.Append(",");
                }
            }
            return result.ToString();
        }
        /// <summary>
        /// Responsável por Deserializar as informações a partir de uma string Json
        /// </summary>
        /// <typeparam name="T">Tipo de Dado a ser deserializado</typeparam>
        /// <param name="jsonData">Dados em string Json</param>
        /// <returns>Tipo de dado Tipado</returns>
        public static T Deserialize<T>(this string jsonData)
        {
            try
            {              
                DataContractJsonSerializer slzr = new DataContractJsonSerializer(typeof(T));
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
                T data = (T)slzr.ReadObject(stream);
                stream.Close();

                return data;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
