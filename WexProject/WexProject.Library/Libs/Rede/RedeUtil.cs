using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace WexProject.Library.Libs.Rede
{
    /// <summary>
    /// Classe utilitária para compartilhamento de métodos relativos a redes
    /// </summary>
    public class RedeUtil
    {

        /// <summary>
        /// Responsável por 
        /// </summary>
        /// <param name="nomeServidor"></param>
        /// <returns>Retorna o Número Ip da Máquina Local</returns>
        public static IPAddress GetEnderecoIp( string nomeServidor )
        {
            IPHostEntry listaIps;
            try
            {
                listaIps = GetIpHostEntry(nomeServidor);
            }
            catch(SocketException)
            {
                return null;
            }
            string stringIp;
            Regex regex = new Regex( @"^([0-9]{1,3}\.){3}[0-9]{1,3}$" );
            IPAddress ip = null;
            int[] numerosIp;
            foreach(var item in listaIps.AddressList)
            {
                stringIp = item.ToString();
                if(regex.IsMatch( stringIp ))
                {
                    numerosIp = stringIp.Split( '.' ).Select( o => Convert.ToInt32( o ) ).ToArray();
                    bool status = true;

                    if(numerosIp.Length < 4)
                        status = false;

                    for(int i = 0; i < numerosIp.Length; i++)
                    {
                        if(numerosIp[i] < 0 || numerosIp[i] > 255 || !status)
                        {
                            status = false;
                            break;
                        }
                    }

                    if(status)
                        ip = item;
                }
                
            }

            return ip;
        }

        /// <summary>
        /// Método responsável por retornar o endereço IPV4 do computador atual
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetEnderecoIpComputadorAtual() 
        {
            try
            {
                return GetEnderecoIp( Dns.GetHostName() );
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Método responsável por fazer a busca da entrada de ip do servidor
        /// </summary>
        /// <param name="nomeServidor"></param>
        /// <returns></returns>
        protected static IPHostEntry GetIpHostEntry(string nomeServidor) 
        {
            return Dns.GetHostEntry( nomeServidor );
        }
    }
}
