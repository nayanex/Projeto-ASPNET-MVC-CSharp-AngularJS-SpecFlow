using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WexProject.Library.Libs.Str;

namespace WexProject.Test
{
    /// <summary>
    /// This is a test class for StrUtilTest and is intended
    /// to contain all StrUtilTest Unit Tests
    ///</summary>
    [TestClass]
    public class StrUtilTest
    {
        [TestMethod]
        public void RetirarConteudoDesnecessarioStringComSeparadorTeste()
        {
            char separador;
            string texto, novo;

            // Texto sem conteúdo desnecessário
            separador = ';';
            texto = "valor01;valor02;valor03";
            novo = "valor01;valor02;valor03";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");

            // Texto com conteúdo desnecessário no início
            separador = ';';
            texto = ";valor01;valor02;valor03";
            novo = "valor01;valor02;valor03";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");

            // Texto com conteúdo desnecessário no meio
            separador = ';';
            texto = "valor01;;valor02;;valor03";
            novo = "valor01;valor02;valor03";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");

            // Texto com conteúdo desnecessário no fim
            separador = ';';
            texto = "valor01;valor02;valor03;";
            novo = "valor01;valor02;valor03";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");

            // Texto com espaços
            separador = ';';
            texto = "valor01 ;valor02 ; valor03 ";
            novo = "valor01;valor02;valor03";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");

            // Texto com espaços e conteúdos desnecessários
            separador = ';';
            texto = "valor01 ; ;   ,   ;valor02 ; valor03 ";
            novo = "valor01;,;valor02;valor03";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");

            // Texto vazio
            separador = ';';
            texto = "";
            novo = "";
            Assert.AreEqual(novo, StrUtil.RetirarConteudoDesnecessarioStringComSeparador(texto, separador),
                "O texto não veio conforme o esperado.");
        }
    }
}
