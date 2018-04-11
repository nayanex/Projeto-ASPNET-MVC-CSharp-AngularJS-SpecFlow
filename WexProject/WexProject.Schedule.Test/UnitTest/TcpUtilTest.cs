using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Libs;
using System.Net;
using System.Net.Sockets;
using WexProject.MultiAccess.Library.Dtos;
using Newtonsoft.Json;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TcpUtilTest
    {
        [TestMethod]
        public void AdicionarProtecaoIntegridadeMensagemTest()
        {
            const string mensagem = "uma mensagem \n de texto \n qualquer \n";
            string mensagemConvertida = TcpUtil.AdicionarStringProtecaoDeIntegridade(mensagem);
            Assert.AreEqual("uma mensagem *EnT3R* de texto *EnT3R* qualquer *EnT3R*\n",mensagemConvertida,"A mensagem de entrada deveria ter acrescentado as tags especiais que representam o /n");
            string mensagemDesconvertida = TcpUtil.RemoverStringProtecaoDeIntegridade(mensagemConvertida);
            Assert.AreEqual(mensagem,mensagemDesconvertida,"A mensagem de entrada deveria ser a mesma de saída");
        }

        [TestMethod]
        public void RemoverProtecaoIntegridadeMensagemTest()
        {
            string mensagem = "uma mensagem \n de texto \n qualquer \n";
            string mensagemConvertida = TcpUtil.AdicionarStringProtecaoDeIntegridade(mensagem);
            string mensagemDesconvertida = TcpUtil.RemoverStringProtecaoDeIntegridade(mensagemConvertida);
            Assert.AreEqual(mensagem,mensagemDesconvertida,"A mensagem de entrada deveria ser a mesma de saída");
        }

        [TestMethod]
        public void EnviarEReceberApenasUmaMensagemDeTextoTest()
        {

            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8050;
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();
            string m1 = "Este texto deve chegar ao destino completamente intacto";
            TcpClient tcp = new TcpClient();
            tcp.Connect(ipServidor,portaServidor);
            TcpClient tcpServidor = servidor.AcceptTcpClient();
            TcpUtil.EnviarMensagemTcp(m1,tcp);
            string m1Result = TcpUtil.ReceberMensagemTcp(tcpServidor);
            Assert.AreEqual(m1,m1Result,"A mensagem retornada deveria estar intacta portanto igual a de origem");
            servidor.Stop();
            tcp.Close();
        }

        [TestMethod]
        public void EnviarEReceberUmaMensagemDtoTest()
        {

            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8051;
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();
            MensagemDto mensagemDto = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { "Joao" },"C1");
            string m1 = JsonConvert.SerializeObject(mensagemDto);
            TcpClient tcp = new TcpClient();
            tcp.Connect(ipServidor,portaServidor);
            TcpClient tcpServidor = servidor.AcceptTcpClient();
            TcpUtil.EnviarMensagemTcp(m1,tcp);
            string m1Result = TcpUtil.ReceberMensagemTcp(tcpServidor);
            Assert.AreEqual(m1,m1Result,"A mensagem retornada deveria estar intacta portanto igual a de origem");
            servidor.Stop();
            tcp.Close();
        }

        [TestMethod]
        public void EnviarEReceberApenasMaisDeUmaMensagemTest()
        {
            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8052;
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();
            Dictionary<string,TcpClient> usuariosDic = new Dictionary<string,TcpClient>();
            Dictionary<string,TcpClient> usuariosEmServidorDic = new Dictionary<string,TcpClient>();
            TcpClient tcpTemp1,tcpTemp2;
            string[] nomeUsuarios = new string[] { "Joao","Pedro","Paulo","Marcos" };

            foreach (string nome in nomeUsuarios)
            {
                tcpTemp1 = new TcpClient();
                tcpTemp1.Connect(ipServidor,portaServidor);
                usuariosDic.Add(nome,tcpTemp1);
                tcpTemp2 = servidor.AcceptTcpClient();
                usuariosEmServidorDic.Add(nome,tcpTemp2);
            }

            //Enviar Várias Mensagens
            List<string> listaMensagensEsperadas = new List<string>();
            string txMensagem;
            foreach (var usuario in usuariosDic)
            {
                txMensagem = string.Format("Mensagem enviada pelo usuario:{0}",usuario.Key);
                listaMensagensEsperadas.Add(txMensagem);
                TcpUtil.EnviarMensagemTcp(txMensagem,usuario.Value);
            }

            //Receber Várias Mensagens
            List<string> listaMensagensRecebidas = new List<string>();
            foreach (var usuario in usuariosEmServidorDic)
            {
                txMensagem = TcpUtil.ReceberMensagemTcp(usuario.Value);
                listaMensagensRecebidas.Add(txMensagem);
            }

            //Asserts
            Assert.AreEqual(listaMensagensEsperadas.Count,listaMensagensRecebidas.Count);

            for (int i = 0;i < listaMensagensEsperadas.Count;i++)
            {
                Assert.AreEqual(listaMensagensEsperadas.ElementAt(i),listaMensagensRecebidas.ElementAt(i),"Deveriam ser a mesma mensagem");
            }

            servidor.Stop();
            for (int i = 0;i < usuariosDic.Count;i++)
            {
                usuariosDic.ElementAt(i).Value.Close();
                usuariosEmServidorDic.ElementAt(i).Value.Close();
            }
        }
    }
}
