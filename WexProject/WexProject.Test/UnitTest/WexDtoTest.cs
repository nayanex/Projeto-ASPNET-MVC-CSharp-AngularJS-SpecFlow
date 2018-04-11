using System.Collections.Generic;
using System.Linq;
using WexProject.Test.Scenario.Serializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Library.Libs.Json;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class WexDtoTest
    {

        [TestMethod]
        public void TestSerializerVazioTest()
        {
            string result = JsonUtil.Serialize(new List<Pessoa>());
            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);
            Assert.AreEqual(0, resultDes.Count);
        }

        [TestMethod]
        public void TestSerializerAtributoNuloTest()
        {
            string result = JsonUtil.Serialize(new Pessoa("A", null));

            Pessoa resultDes = JsonUtil.Deserialize<Pessoa>(result);

            Assert.AreEqual("A", resultDes.Nome);
            Assert.AreEqual(null, resultDes.Idade);
        }

        [TestMethod]
        public void TestSerializerUmObjetoTest()
        {
            string result = JsonUtil.Serialize(new Pessoa("A", 22));

            Pessoa resultDes = JsonUtil.Deserialize<Pessoa>(result);

            Assert.AreEqual("A", resultDes.Nome);
            Assert.AreEqual(22, resultDes.Idade);
        }

        [TestMethod]
        public void TestSerializerUmNivelArrayTest()
        {
            Pessoa[] pessoas = new Pessoa[]{new Pessoa("A", 3), new Pessoa("B", 4), new Pessoa("C", 5)};

            string result = JsonUtil.Serialize(pessoas);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(3, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(3, resultDes[0].Idade);
            Assert.AreEqual(true, resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(4, resultDes[1].Idade);
            Assert.AreEqual(true, resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);

            Assert.AreEqual("C", resultDes[2].Nome);
            Assert.AreEqual(5, resultDes[2].Idade);
            Assert.AreEqual(true, resultDes[2].Filhos == null || resultDes[2].Filhos.Count == 0);
        }
       

        [TestMethod]
        public void TestSerializerUmNivelHashSetTest()
        {
            HashSet<Pessoa> pessoas = new HashSet<Pessoa>();
            pessoas.Add(new Pessoa("A", 3));
            pessoas.Add(new Pessoa("B", 4));
            pessoas.Add(new Pessoa("C", 5));

            string result = JsonUtil.Serialize(pessoas);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(3, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(3, resultDes[0].Idade);
            Assert.IsTrue(resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);
            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(4, resultDes[1].Idade);
            Assert.IsTrue(resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);

            Assert.AreEqual("C", resultDes[2].Nome);
            Assert.AreEqual(5, resultDes[2].Idade);
            Assert.IsTrue(resultDes[2].Filhos == null || resultDes[2].Filhos.Count == 0);
        }

        [TestMethod]
        public void TestSerializerUmNivelTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa("A", 3));
            pessoas.Add(new Pessoa("B", 4));
            pessoas.Add(new Pessoa("C", 5));

            string result = JsonUtil.Serialize(pessoas);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(3, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(3, resultDes[0].Idade);
            Assert.AreEqual(true, resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(4, resultDes[1].Idade);
            Assert.AreEqual(true, resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);

            Assert.AreEqual("C", resultDes[2].Nome);
            Assert.AreEqual(5, resultDes[2].Idade);
            Assert.AreEqual(true, resultDes[2].Filhos == null || resultDes[2].Filhos.Count == 0);
        }

        [TestMethod]
        public void TestSerializerDoisNiveisTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            Pessoa p = new Pessoa("A", 18);
            p.Filhos.Add(new Filho("A1", 2));
            p.Filhos.Add(new Filho("A2", 3));
            p.Filhos.Add(new Filho("A3", 4));
            pessoas.Add(p);

            Pessoa p2 = new Pessoa("B", 19);
            p2.Filhos.Add(new Filho("B1", 5));
            p2.Filhos.Add(new Filho("B2", 6));
            p2.Filhos.Add(new Filho("B3", 7));
            pessoas.Add(p2);

            string result = JsonUtil.Serialize(pessoas);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(2, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(18, resultDes[0].Idade);
            Assert.AreEqual(3, resultDes[0].Filhos.Count);

            Assert.AreEqual("A1", resultDes[0].Filhos.ElementAt(0).Nome);
            Assert.AreEqual(2, resultDes[0].Filhos.ElementAt(0).Idade);
            Assert.AreEqual("A2", resultDes[0].Filhos.ElementAt(1).Nome);
            Assert.AreEqual(3, resultDes[0].Filhos.ElementAt(1).Idade);
            Assert.AreEqual("A3", resultDes[0].Filhos.ElementAt(2).Nome);
            Assert.AreEqual(4, resultDes[0].Filhos.ElementAt(2).Idade);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(19, resultDes[1].Idade);
            Assert.AreEqual(3, resultDes[1].Filhos.Count);

            Assert.AreEqual("B1", resultDes[1].Filhos.ElementAt(0).Nome);
            Assert.AreEqual(5, resultDes[1].Filhos.ElementAt(0).Idade);
            Assert.AreEqual("B2", resultDes[1].Filhos.ElementAt(1).Nome);
            Assert.AreEqual(6, resultDes[1].Filhos.ElementAt(1).Idade);
            Assert.AreEqual("B3", resultDes[1].Filhos.ElementAt(2).Nome);
            Assert.AreEqual(7, resultDes[1].Filhos.ElementAt(2).Idade);
        }

        [TestMethod]
        public void TestSerializerClassesDiferentesMesmosAtributosTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa("A", 3));
            pessoas.Add(new Pessoa("B", 4));
            pessoas.Add(new Pessoa("C", 5));

            string result = JsonUtil.Serialize(pessoas);

            List<Cachorro> resultDes = JsonUtil.Deserialize<List<Cachorro>>(result);

            Assert.AreEqual(3, resultDes.Count);
            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(3, resultDes[0].Idade);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(4, resultDes[1].Idade);

            Assert.AreEqual("C", resultDes[2].Nome);
            Assert.AreEqual(5, resultDes[2].Idade);
        }


        [TestMethod]
        public void TestSerializerClassesDiferentesTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa("A", 3));
            pessoas.Add(new Pessoa("B", 4));
            pessoas.Add(new Pessoa("C", 5));

            string result = JsonUtil.Serialize(pessoas);

            List<Gato> resultDes = JsonUtil.Deserialize<List<Gato>>(result);

            Assert.AreEqual(3, resultDes.Count);
            Assert.AreEqual("A", resultDes[0].Nome);

            Assert.AreEqual("B", resultDes[1].Nome);

            Assert.AreEqual("C", resultDes[2].Nome);
        }

        [TestMethod]
        public void TestSerializerRetirandoColecaoTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            Pessoa p = new Pessoa("A", 18);
            p.Filhos.Add(new Filho("A1", 2));
            p.Filhos.Add(new Filho("A2", 3));
            p.Filhos.Add(new Filho("A3", 4));
            pessoas.Add(p);

            Pessoa p2 = new Pessoa("B", 19);
            p2.Filhos.Add(new Filho("B1", 5));
            p2.Filhos.Add(new Filho("B2", 6));
            p2.Filhos.Add(new Filho("B3", 7));
            pessoas.Add(p2);

            string result = JsonUtil.Serialize(pessoas, new string[] { "Filhos" });

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(18, resultDes[0].Idade);
            Assert.IsTrue(resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(19, resultDes[1].Idade);
            Assert.IsTrue(resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);
        }


        [TestMethod]
        public void TestSerializerRetirandoColecaoEAtributoTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            Pessoa p = new Pessoa("A", 18);
            p.Filhos.Add(new Filho("A1", 2));
            p.Filhos.Add(new Filho("A2", 3));
            p.Filhos.Add(new Filho("A3", 4));
            pessoas.Add(p);

            Pessoa p2 = new Pessoa("B", 19);
            p2.Filhos.Add(new Filho("B1", 5));
            p2.Filhos.Add(new Filho("B2", 6));
            p2.Filhos.Add(new Filho("B3", 7));
            pessoas.Add(p2);

            string result = JsonUtil.Serialize(pessoas, new string[] { "Filhos", "Idade" });
            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(null, resultDes[0].Idade);
            Assert.IsTrue(resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(null, resultDes[1].Idade);
            Assert.IsTrue(resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);
        }

        [TestMethod]
        public void TestSerializerPassandoExcluidosNuloTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa("A", 3));
            pessoas.Add(new Pessoa("B", 4));
            pessoas.Add(new Pessoa("C", 5));

            string result = JsonUtil.Serialize(pessoas, null);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(3, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(3, resultDes[0].Idade);
            Assert.IsTrue(resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(4, resultDes[1].Idade);
            Assert.IsTrue(resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);

            Assert.AreEqual("C", resultDes[2].Nome);
            Assert.AreEqual(5, resultDes[2].Idade);
            Assert.IsTrue(resultDes[2].Filhos == null || resultDes[2].Filhos.Count == 0);
        }

        [TestMethod]
        public void TestSerializerTodosNiveisTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            Pessoa p = new Pessoa("A", 18);
            p.Filhos.Add(new Filho("A1", 2));
            p.Filhos.Add(new Filho("A2", 3));
            p.Filhos.Add(new Filho("A3", 4));
            pessoas.Add(p);

            Pessoa p2 = new Pessoa("B", 19);
            p2.Filhos.Add(new Filho("B1", 5));
            p2.Filhos.Add(new Filho("B2", 6));
            p2.Filhos.Add(new Filho("B3", 7));
            pessoas.Add(p2);

            string result = JsonUtil.Serialize(pessoas, true);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(2, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(18, resultDes[0].Idade);
            Assert.AreEqual(3, resultDes[0].Filhos.Count);

            Assert.AreEqual("A1", resultDes[0].Filhos.ElementAt(0).Nome);
            Assert.AreEqual(2, resultDes[0].Filhos.ElementAt(0).Idade);
            Assert.AreEqual("A2", resultDes[0].Filhos.ElementAt(1).Nome);
            Assert.AreEqual(3, resultDes[0].Filhos.ElementAt(1).Idade);
            Assert.AreEqual("A3", resultDes[0].Filhos.ElementAt(2).Nome);
            Assert.AreEqual(4, resultDes[0].Filhos.ElementAt(2).Idade);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(19, resultDes[1].Idade);
            Assert.AreEqual(3, resultDes[1].Filhos.Count);

            Assert.AreEqual("B1", resultDes[1].Filhos.ElementAt(0).Nome);
            Assert.AreEqual(5, resultDes[1].Filhos.ElementAt(0).Idade);
            Assert.AreEqual("B2", resultDes[1].Filhos.ElementAt(1).Nome);
            Assert.AreEqual(6, resultDes[1].Filhos.ElementAt(1).Idade);
            Assert.AreEqual("B3", resultDes[1].Filhos.ElementAt(2).Nome);
            Assert.AreEqual(7, resultDes[1].Filhos.ElementAt(2).Idade);
        }


        [TestMethod]
        public void TestSerializerSomenteRootTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            Pessoa p = new Pessoa("A", 18);
            p.Filhos.Add(new Filho("A1", 2));
            p.Filhos.Add(new Filho("A2", 3));
            p.Filhos.Add(new Filho("A3", 4));
            pessoas.Add(p);

            Pessoa p2 = new Pessoa("B", 19);
            p2.Filhos.Add(new Filho("B1", 5));
            p2.Filhos.Add(new Filho("B2", 6));
            p2.Filhos.Add(new Filho("B3", 7));
            pessoas.Add(p2);

            string result = JsonUtil.Serialize(pessoas, false);

            List<Pessoa> resultDes = JsonUtil.Deserialize<List<Pessoa>>(result);

            Assert.AreEqual(2, resultDes.Count);

            Assert.AreEqual("A", resultDes[0].Nome);
            Assert.AreEqual(18, resultDes[0].Idade);
            Assert.IsTrue(resultDes[0].Filhos == null || resultDes[0].Filhos.Count == 0);

            Assert.AreEqual("B", resultDes[1].Nome);
            Assert.AreEqual(19, resultDes[1].Idade);
            Assert.IsTrue(resultDes[1].Filhos == null || resultDes[1].Filhos.Count == 0);
        }

        [TestMethod]
        public void TestSerializerValoresNumericosTest()
        {
            ValoresNumericos num = new ValoresNumericos(10.7, true);
            string serializado = JsonUtil.Serialize(num);
            ValoresNumericos resposta = JsonUtil.Deserialize<ValoresNumericos>(serializado);

            Assert.AreEqual(10.7, resposta.DoubleValue);
            Assert.AreEqual(true, resposta.BoolValue);

            num = new ValoresNumericos(55.5557, false);
            serializado = JsonUtil.Serialize(num, true);
            resposta = JsonUtil.Deserialize<ValoresNumericos>(serializado);

            Assert.AreEqual(55.5557, resposta.DoubleValue);
            Assert.AreEqual(false, resposta.BoolValue);
        }

        [TestMethod]
        public void TestSerializerObjetoComObjetoTest()
        {
            List<Gato> lista = new List<Gato>();
            lista.Add(new Gato("Gato1", "Cao1", 12));
            lista.Add(new Gato("Gato2", "Cao2", 13));
            lista.Add(new Gato("Gato3", "Cao3", 14));

            string serializado = JsonUtil.Serialize(lista, true);

            List<Gato> resultList = JsonUtil.Deserialize<List<Gato>>(serializado);

            Assert.AreEqual(3, resultList.Count);

            Assert.AreEqual("Gato1", resultList[0].Nome);
            Assert.IsNotNull(resultList[0].Cachorro);
            Assert.AreEqual("Cao1", resultList[0].Cachorro.Nome);
            Assert.AreEqual(12, resultList[0].Cachorro.Idade);

            Assert.AreEqual("Gato2", resultList[1].Nome);
            Assert.IsNotNull(resultList[1].Cachorro);
            Assert.AreEqual("Cao2", resultList[1].Cachorro.Nome);
            Assert.AreEqual(13, resultList[1].Cachorro.Idade);

            Assert.AreEqual("Gato3", resultList[2].Nome);
            Assert.IsNotNull(resultList[2].Cachorro);
            Assert.AreEqual("Cao3", resultList[2].Cachorro.Nome);
            Assert.AreEqual(14, resultList[2].Cachorro.Idade);
        }
    }
}
