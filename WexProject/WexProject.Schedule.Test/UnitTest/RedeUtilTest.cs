using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Library.Libs.Rede;
using System.Net.Sockets;
using System.Net;
using Moq;
using Moq.Protected;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class RedeUtilTest
    {
        [TestMethod]
        public void DeveLocalizarOEnderecoIpDoServidor()
        {
            IPAddress enderecoIpServidor = RedeUtil.GetEnderecoIp( "Lab6-gabriel" );
            Assert.IsNotNull( enderecoIpServidor, "Deveria ter descoberto o ip do servidor" );
            enderecoIpServidor = RedeUtil.GetEnderecoIp( "Lab1-andersonli" );
            Assert.IsNotNull( enderecoIpServidor, "Deveria ter descoberto o ip do servidor" );
        }

        [TestMethod]
        public void DeveRetornarNuloQuandoOServidorDeDestinoNaoExistir()
        {
            IPAddress enderecoIpServidor = RedeUtil.GetEnderecoIp( "Lab1000-gabriel" );
            Assert.IsNull( enderecoIpServidor, "Não deveria possuir nenhum valor pois não existe o servidor solicitado" );
        }
    }
}
