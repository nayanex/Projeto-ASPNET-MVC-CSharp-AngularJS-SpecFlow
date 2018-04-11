using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Libs;
using System.Collections;
using WexProject.MultiAccess.Library.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using WexProject.Library.Libs;
using WexProject.Library.Libs.DataHora;


namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class MensagemTest 
    {
        [TestMethod]
        public void TestarMensagemSerializacao()
        {
            const string cronogramaOid = "C1";
            const string cronogramaOid2 = "C2";
            string[] vetorUsuarios1 = new string[] {"Joao"};
            string[] vetorUsuarios2 = new string[] { "Pedro" };
            string[] vetorUsuarios3 = new string[]{"Joao","Pedro"};
            string[] vetorUsuarios4 = new string[] { "Paulo","Jose" };
            MensagemDto objeto = Mensagem.RnCriarMensagemNovoUsuarioConectado(vetorUsuarios1,cronogramaOid);
            MensagemDto objeto2 = Mensagem.RnCriarMensagemNovoUsuarioConectado(vetorUsuarios2, cronogramaOid);
            MensagemDto objeto3 = Mensagem.RnCriarMensagemNovoUsuarioConectado(vetorUsuarios3, cronogramaOid2);
            MensagemDto objeto4 = Mensagem.RnCriarMensagemNovoUsuarioConectado(vetorUsuarios4, cronogramaOid2);
     
            string mensagemJson1 = JsonConvert.SerializeObject(objeto);
            string mensagemJson2 = JsonConvert.SerializeObject(objeto2);
            string mensagemJson3 = JsonConvert.SerializeObject(objeto3);
            string mensagemJson4 = JsonConvert.SerializeObject(objeto4);

            MensagemDto objetoEsperado1 = JsonConvert.DeserializeObject<MensagemDto>(mensagemJson1);
            MensagemDto objetoEsperado2 = JsonConvert.DeserializeObject<MensagemDto>(mensagemJson2);
            MensagemDto objetoEsperado3 = JsonConvert.DeserializeObject<MensagemDto>(mensagemJson3);
            MensagemDto objetoEsperado4 = JsonConvert.DeserializeObject<MensagemDto>(mensagemJson4);

            objetoEsperado1.Propriedades["usuarios"] = Mensagem.ExtrairUsuariosMensagemDto(objetoEsperado1);
            objetoEsperado2.Propriedades["usuarios"] = Mensagem.ExtrairUsuariosMensagemDto(objetoEsperado2);
            objetoEsperado3.Propriedades["usuarios"] = Mensagem.ExtrairUsuariosMensagemDto(objetoEsperado3);
            objetoEsperado4.Propriedades["usuarios"] = Mensagem.ExtrairUsuariosMensagemDto(objetoEsperado4);
            string[] nomes1 = (string[])objetoEsperado1.Propriedades["usuarios"];
            string[] nomes2 = (string[])objetoEsperado2.Propriedades["usuarios"];
            string[] nomes3 = (string[])objetoEsperado3.Propriedades["usuarios"];
            string[] nomes4 = (string[])objetoEsperado4.Propriedades["usuarios"];
            Assert.IsTrue(vetorUsuarios1.SequenceEqual(nomes1),"Deveria ter os mesmos indices e valores");
            Assert.IsTrue(vetorUsuarios2.SequenceEqual(nomes2),"Deveria ter os mesmos indices e valores");
            Assert.IsTrue(vetorUsuarios3.SequenceEqual(nomes3),"Deveria ter os mesmos indices e valores");
            Assert.IsTrue(vetorUsuarios4.SequenceEqual(nomes4),"Deveria ter os mesmos indices e valores");
        
        }

        [TestMethod]
        public void ReceberUsuariosMensagemDtoQuandoVetorUsuariosForVazio() 
        {
            const string cronogramaOid = "C1";
            string[] usuarios = new string[]{};

            MensagemDto objeto = Mensagem.RnCriarMensagemNovoUsuarioConectado(usuarios,cronogramaOid);
            string mensagemJson4 = JsonConvert.SerializeObject(objeto);
            MensagemDto objetoEsperado = JsonConvert.DeserializeObject<MensagemDto>(mensagemJson4);
            objetoEsperado.Propriedades["usuarios"] = Mensagem.ExtrairUsuariosMensagemDto(objetoEsperado);
            string[] nomes = (string[])objetoEsperado.Propriedades["usuarios"];
            Assert.AreEqual(0,nomes.Length,"Não deveria possuir nenhum indice pois o vetor serializado era não possuia nenhum valor armazenado");
        }

        [TestMethod]
        public void TestarSerializacaoEDeserializacaoMensagemCriarNovaTarefa()
        {
            DateUtil.CurrentDateTime = DateTime.Now;

            //TODO: Verificar Falha
            MensagemDto mensagemCriada1 = Mensagem.RnCriarMensagemNovaTarefaCriada( "T1", "Joao", "C1", null, DateUtil.CurrentDateTime );
            MensagemDto mensagemCriada2 = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { "Joao","Marcos" },"C1");
            string mensagemJson1 = JsonConvert.SerializeObject(mensagemCriada1);
            string mensagemJson2 = JsonConvert.SerializeObject(mensagemCriada2);

            MensagemDto mensagemDeserializada1 = Mensagem.DeserializarMensagemDto(mensagemJson1);
            MensagemDto mensagemDeserializada2 = Mensagem.DeserializarMensagemDto(mensagemJson2);

            Assert.AreEqual(mensagemCriada1.Propriedades[Constantes.AUTOR_ACAO].ToString() ,mensagemDeserializada1.Propriedades[Constantes.AUTOR_ACAO].ToString());
            Assert.AreEqual(mensagemCriada1.Propriedades[Constantes.OIDCRONOGRAMA].ToString() ,mensagemDeserializada1.Propriedades[Constantes.OIDCRONOGRAMA].ToString());

            string[] usuariosMensagemCriada,usuariosMensagemRecebida;
            usuariosMensagemCriada = (string[])mensagemCriada2.Propriedades[Constantes.USUARIOS];
            usuariosMensagemRecebida = (string[])mensagemDeserializada2.Propriedades[Constantes.USUARIOS];
            Assert.IsTrue(usuariosMensagemCriada.SequenceEqual(usuariosMensagemRecebida));
            Assert.AreEqual(mensagemCriada2.Propriedades[Constantes.OIDCRONOGRAMA].ToString() ,mensagemDeserializada2.Propriedades[Constantes.OIDCRONOGRAMA].ToString());
        }

        [TestMethod]
        public void EfetuarCopiaDaMensagemDtoTest()
        {
            MensagemDto mensagemDto = new MensagemDto();
            //O tipo da mensagem é irrelevante ao teste pois a mensagem criada aqui será somente para simular a modificação da coleção propriedades e os impactos que ocorrem nesse caso
            mensagemDto.Tipo = MultiAccess.Library.Domains.CsTipoMensagem.ConexaoEfetuadaComSucesso;

            mensagemDto.Propriedades.Add("Joao","25");
            mensagemDto.Propriedades.Add("Pedro" ,"24");
            mensagemDto.Propriedades.Add("Marcelo" ,"23");
            mensagemDto.Propriedades.Add("Marcos" ,"22");

            Dictionary<string ,MensagemDto> mensagens = new Dictionary<string ,MensagemDto>();

            //Simulação de envio
            /*
             * Ao utilizar o método de simulação de envio da mensagem o dono destinatário da mensagem deve receber uma cópia
             * que exclui a si mesmo da mensagem modificando a mensagem  que agora não possui o próprio login do receptor
             */
            foreach (DictionaryEntry item in mensagemDto.Propriedades)
            {
                SimularModificacaoColecaoDaMensagem(Mensagem.CopiarMensagemDto(mensagemDto) ,(string)item.Key ,mensagens);
            }

            MensagemDto mensagemJoao ,mensagemPedro ,mensagemMarcos ,mensagemMarcelo;
            //recebendo uma cópia da mensagem original
            mensagemJoao = Mensagem.CopiarMensagemDto(mensagemDto);
            //efetuando alteração na cópia
            mensagemJoao.Propriedades.Remove("Joao");
            //comparando se a cópia alterada de Joao é identica a cópia modificada no método SimulacaoEnviarMensagemCriacaoNovaTarefaPara
            CollectionAssert.AreEquivalent(mensagemJoao.Propriedades ,mensagens["Joao"].Propriedades);

            mensagemPedro = Mensagem.CopiarMensagemDto(mensagemDto);
            //efetuando alteração na cópia
            mensagemPedro.Propriedades.Remove("Pedro");
            //comparando se a cópia alterada de Pedro é identica a cópia modificada no método SimulacaoEnviarMensagemCriacaoNovaTarefaPara
            CollectionAssert.AreEquivalent(mensagemPedro.Propriedades ,mensagens["Pedro"].Propriedades);

            mensagemMarcelo = Mensagem.CopiarMensagemDto(mensagemDto);
            //efetuando alteração na cópia
            mensagemMarcelo.Propriedades.Remove("Marcelo");
            //comparando se a cópia alterada de Marcelo é identica a cópia modificada no método SimulacaoEnviarMensagemCriacaoNovaTarefaPara
            CollectionAssert.AreEquivalent(mensagemMarcelo.Propriedades ,mensagens["Marcelo"].Propriedades);

            mensagemMarcos = Mensagem.CopiarMensagemDto(mensagemDto);
            //efetuando alteração na cópia
            mensagemMarcos.Propriedades.Remove("Marcos");
            //comparando se a cópia alterada de Marcos é identica a cópia modificada no método SimulacaoEnviarMensagemCriacaoNovaTarefaPara
            CollectionAssert.AreEquivalent(mensagemMarcos.Propriedades ,mensagens["Marcos"].Propriedades);

        }

        /// <summary>
        /// Simulação do envio removendo o usuário dono da mensagem da propria mensagem utilizado no teste EfetuarCopiaDaMensagemDtoTest
        /// </summary>
        /// <param name="mensagem">Cópia de mensagem original</param>
        /// <param name="login">Distinatário receptor da mensagem</param>
        /// <param name="mensagens"> Dicionário de mensagens para armazenar o a mensagem alterada para comparação posterior no teste unitário</param>
        public static void SimularModificacaoColecaoDaMensagem(MensagemDto mensagem,string login,Dictionary<string,MensagemDto> mensagens)
        {
            //removendo o próprio usuário da mensagem
            if (mensagem.Propriedades.ContainsKey(login))
                mensagem.Propriedades.Remove(login);
            string msg;
            //caso contenha mais de uma propriedade ou seja mais do que a propriedade "cronogramaOid" é reportado sobre o envio da mensagem
            if (mensagem.Propriedades.Count > 1)
            {
                msg = String.Format("Mensagem  do tipo{0} enviada para {1} Contendo:\n",mensagem.Tipo,login);
                foreach (DictionaryEntry item in mensagem.Propriedades)
                {
                    msg += String.Format("\"{0}\": \"{1}\"\n",item.Key,item.Value);
                }
            }
            else
            {
                msg = "Não foi enviada mensagem para"+login;
            }
            Debug.WriteLine(msg);
            //adicao da mensagem modificada para o devido usuário no dicionário de mensagens modificadas para utilização no teste unitário
            mensagens.Add(login,mensagem);
        }

        [TestMethod]
        public void EfetuarTratamentoExtracaoDicionarioDaMensagemQuandoReceberUmaMensagemDtoComODicionarioSerializadoEmHashtable()
        {
            Dictionary<string,Int16> tarefasImpactadas = new Dictionary<string,short>();
            tarefasImpactadas.Add("T2",2);
            tarefasImpactadas.Add("T3",3);
            tarefasImpactadas.Add("T4",4);
            tarefasImpactadas.Add("T5",5);
            tarefasImpactadas.Add("T6",6);
            tarefasImpactadas.Add("T7",7);
            tarefasImpactadas.Add("T8",8);
            tarefasImpactadas.Add("T9",9);

            DateUtil.CurrentDateTime = DateTime.Now;

            MensagemDto mensagem = Mensagem.RnCriarMensagemMovimentacaoTarefa( 1, 10, "T1", tarefasImpactadas, "Joao", "C1", DateUtil.CurrentDateTime );

            string mensagemJson = JsonConvert.SerializeObject(mensagem);

            MensagemDto mensagemEsperada = JsonConvert.DeserializeObject<MensagemDto>(mensagemJson);
            mensagemEsperada = Mensagem.EfetuarTratamentoExtracaoDicionarioDaMensagem<string ,Int16>(mensagemEsperada ,Constantes.TAREFAS_IMPACTADAS);

            CollectionAssert.AreEqual((Dictionary<string ,Int16>)mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS] ,(Dictionary<string ,Int16>)mensagemEsperada.Propriedades[Constantes.TAREFAS_IMPACTADAS]);
            Assert.AreEqual(DateUtil.CurrentDateTime, mensagem.Propriedades[Constantes.DATAHORA_ACAO]);
        }
    }
}
