using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Planejamento;
using System.Data.Entity;
using WexProject.BLL.Contexto;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaTarefaDaoTest : BaseEntityFrameworkTest
    {
        /// <summary>
        /// Método responsável por verificar se a busca pelas tarefas em determinado cronograma está sendo realizada.
        /// </summary>
        [TestMethod]
        public void ConsultarCronogramaTarefasPorOidTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            DateTime dtInicio = DateTime.Now;

            List<Guid> oidTarefas = new List<Guid>();

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();

            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );

            List<CronogramaTarefa> lstCronoTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid ,
                o=>o.Tarefa.SituacaoPlanejamento, 
                o=>o.Tarefa.AtualizadoPor.Usuario.Person,
                o=>o.Cronograma);

            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, lstCronoTarefas[0].NbID );

            lstCronoTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid ,
                o=>o.Tarefa.SituacaoPlanejamento, 
                o=>o.Tarefa.AtualizadoPor.Usuario.Person,
                o=>o.Cronograma);

            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 03", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, lstCronoTarefas[1].NbID );

            lstCronoTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid,
                 o => o.Tarefa.SituacaoPlanejamento,
                o => o.Tarefa.AtualizadoPor.Usuario.Person,
                o => o.Cronograma );

            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 04", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, lstCronoTarefas[2].NbID );

            lstCronoTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, 
                o => o.Tarefa.SituacaoPlanejamento,
                o => o.Tarefa.AtualizadoPor.Usuario.Person,
                o => o.Cronograma );

            oidTarefas.Add( lstCronoTarefas[0].Oid );
            oidTarefas.Add( lstCronoTarefas[2].Oid );

            //realiza a busca pelas tarefas de acordo com a lista de oids passados e o cronograma.
            List<CronogramaTarefa> lstTarefasResult = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOid( oidTarefas, 
                o => o.Tarefa.SituacaoPlanejamento,
                o => o.Tarefa.AtualizadoPor.Usuario.Person,
                o => o.Cronograma );
            
            //verifica se as tarefas são as mesmas e a quantidade é a mesma.
            Assert.AreEqual( 2, lstTarefasResult.Count );

            Assert.AreEqual( lstCronoTarefas[0].Oid, lstTarefasResult[0].Oid ,"Deveria ser o mesmo CronogramaTarefa");
            Assert.AreEqual( lstCronoTarefas[0].OidCronograma, lstTarefasResult[0].OidCronograma , "Deveriam estar no mesmo cronograma");
            Assert.AreEqual( lstCronoTarefas[0].OidTarefa, lstTarefasResult[0].OidTarefa , "Deveriam estar relacionadas a mesma tarefa");
            Assert.AreEqual( lstCronoTarefas[0].NbID, lstTarefasResult[0].NbID ,"Deveria estar com o mesmo NbId");
            Assert.AreEqual( lstCronoTarefas[0].CsExcluido, lstTarefasResult[0].CsExcluido ,"Deveriam estar com o mesmo estado de exclusão");


            Assert.AreEqual( lstCronoTarefas[2].Oid, lstTarefasResult[1].Oid, "Deveria ser o mesmo CronogramaTarefa" );
            Assert.AreEqual( lstCronoTarefas[2].OidCronograma, lstTarefasResult[1].OidCronograma, "Deveriam estar no mesmo cronograma" );
            Assert.AreEqual( lstCronoTarefas[2].OidTarefa, lstTarefasResult[1].OidTarefa, "Deveriam estar relacionadas a mesma tarefa" );
            Assert.AreEqual( lstCronoTarefas[2].NbID, lstTarefasResult[1].NbID, "Deveria estar com o mesmo NbId" );
            Assert.AreEqual( lstCronoTarefas[2].CsExcluido, lstTarefasResult[1].CsExcluido, "Deveriam estar com o mesmo estado de exclusão" );

        }

        /// <summary>
        /// Cenário: Quando buscar todas as tarefas de um determinado cronograma.
        /// Expectativa: Deve buscar todas as tarefas daquele cronograma, ordenando-as de acordo com o NbId de cada uma.
        /// </summary>
        [TestMethod]
        public void ConsultarCronogramaTarefasPorOidCronogramaQuandoExistirMaisDeUmaTarefa()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = DateTime.Now;
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            //cria tarefa
            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );
            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );
            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 03", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );
            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 04", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );
            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 05", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );

            //lista de oid tarefas.
            List<CronogramaTarefa> cronogramaTarefas = new List<CronogramaTarefa>();
            cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.AreEqual( 1, cronogramaTarefas[0].NbID, "Deveria ser 1, pois o método de busca deveria ordenar adequadamente a partir do NbId de cada tarefa" );
            Assert.AreEqual( 2, cronogramaTarefas[1].NbID, "Deveria ser 2, pois o método de busca deveria ordenar adequadamente a partir do NbId de cada tarefa" );
            Assert.AreEqual( 3, cronogramaTarefas[2].NbID, "Deveria ser 3, pois o método de busca deveria ordenar adequadamente a partir do NbId de cada tarefa" );
            Assert.AreEqual( 4, cronogramaTarefas[3].NbID, "Deveria ser 4, pois o método de busca deveria ordenar adequadamente a partir do NbId de cada tarefa" );
            Assert.AreEqual( 5, cronogramaTarefas[4].NbID, "Deveria ser 5, pois o método de busca deveria ordenar adequadamente a partir do NbId de cada tarefa" );

        }

        [TestMethod]
        public void ConsultarTarefasPorOidCronogramaTarefaQuandoReceberUmaListaDeOidCronogramaTarefaTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.OidCronograma = cronograma1.Oid;
            novaTarefa2.OidCronograma = cronograma1.Oid;
            novaTarefa3.OidCronograma = cronograma1.Oid;
            novaTarefa4.OidCronograma = cronograma1.Oid;
            novaTarefa5.OidCronograma = cronograma1.Oid;

            //Atribui um Id inexistente para tarefa 
            CronogramaTarefaBo.AtribuirId( novaTarefa1, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa2, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa3, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa4, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa5, 0 );

            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;

            contexto.CronogramaTarefa.Add( novaTarefa1 );
            contexto.CronogramaTarefa.Add( novaTarefa2 );
            contexto.CronogramaTarefa.Add( novaTarefa3 );
            contexto.CronogramaTarefa.Add( novaTarefa4 );
            contexto.CronogramaTarefa.Add( novaTarefa5 );
            contexto.SaveChanges();

            List<Guid> oidCronogramaTarefas = new List<Guid>();
            oidCronogramaTarefas.Add( novaTarefa1.Oid );
            oidCronogramaTarefas.Add( novaTarefa2.Oid );

            List<CronogramaTarefa> tarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOid( oidCronogramaTarefas ,o=>o.Cronograma, o=>o.Tarefa.SituacaoPlanejamento, o=>o.Tarefa.AtualizadoPor.Usuario.Person.Party);
            Assert.AreEqual( 2, tarefas.Count, "Deveria possuir 2 tarefas armazenadas!" );

            CronogramaTarefa tarefaEsperada1 = tarefas.FirstOrDefault( o => o.Oid.Equals( novaTarefa1.Oid ) );
            CronogramaTarefa tarefaEsperada2 = tarefas.FirstOrDefault( o => o.Oid.Equals( novaTarefa2.Oid ) );
            Assert.IsNotNull( tarefaEsperada1, "Deveria ter recebido a tarefas esperada" );
            Assert.IsNotNull( tarefaEsperada2, "Deveria ter recebido a tarefas esperada" );

            Assert.AreEqual( novaTarefa1.Oid, tarefaEsperada1.Oid, "Deveria ser o mesmo CronogramaTarefa" );
            Assert.AreEqual( novaTarefa1.OidCronograma, tarefaEsperada1.OidCronograma, "Deveriam estar no mesmo cronograma" );
            Assert.AreEqual( novaTarefa1.OidTarefa, tarefaEsperada1.OidTarefa, "Deveriam estar relacionadas a mesma tarefa" );
            Assert.AreEqual( novaTarefa1.NbID, tarefaEsperada1.NbID, "Deveria estar com o mesmo NbId" );
            Assert.AreEqual( novaTarefa1.CsExcluido, tarefaEsperada1.CsExcluido, "Deveriam estar com o mesmo estado de exclusão" );


            Assert.AreEqual( novaTarefa2.Oid, tarefaEsperada2.Oid, "Deveria ser o mesmo CronogramaTarefa" );
            Assert.AreEqual( novaTarefa2.OidCronograma, tarefaEsperada2.OidCronograma, "Deveriam estar no mesmo cronograma" );
            Assert.AreEqual( novaTarefa2.OidTarefa, tarefaEsperada2.OidTarefa, "Deveriam estar relacionadas a mesma tarefa" );
            Assert.AreEqual( novaTarefa2.NbID, tarefaEsperada2.NbID, "Deveria estar com o mesmo NbId" );
            Assert.AreEqual( novaTarefa2.CsExcluido, tarefaEsperada2.CsExcluido, "Deveriam estar com o mesmo estado de exclusão" );

            //Assert.IsTrue( tarefas.Contains( novaTarefa1 ), "Deveria conter a tarefa na lista" );
            //Assert.IsTrue( tarefas.Contains( novaTarefa2 ), "Deveria conter a tarefa na lista" );
        }


        [TestMethod]
        public void ConsultarCronogramaTarefasCarregandoASituacaoPlanejamento()
        {
            contexto = ContextFactoryManager.CriarWexDb();
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.OidCronograma = cronograma1.Oid;
            novaTarefa2.OidCronograma = cronograma1.Oid;
            novaTarefa3.OidCronograma = cronograma1.Oid;
            novaTarefa4.OidCronograma = cronograma1.Oid;
            novaTarefa5.OidCronograma = cronograma1.Oid;

            //Atribui um Id inexistente para tarefa 
            CronogramaTarefaBo.AtribuirId( novaTarefa1, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa2, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa3, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa4, 0 );
            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa5, 0 );

            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;

            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;

            contexto.CronogramaTarefa.Add( novaTarefa1 );
            contexto.CronogramaTarefa.Add( novaTarefa2 );
            contexto.CronogramaTarefa.Add( novaTarefa3 );
            contexto.CronogramaTarefa.Add( novaTarefa4 );
            contexto.CronogramaTarefa.Add( novaTarefa5 );
            contexto.SaveChanges();

            List<Guid> oidCronogramaTarefas = new List<Guid>();
            oidCronogramaTarefas.Add( novaTarefa1.Oid );
            oidCronogramaTarefas.Add( novaTarefa2.Oid );

            

            List<CronogramaTarefa> tarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOid( oidCronogramaTarefas ,o=>o.Tarefa.SituacaoPlanejamento, o=>o.Cronograma.SituacaoPlanejamento,o=>o.Tarefa.AtualizadoPor.Usuario.Person);
            Assert.AreEqual( 2, tarefas.Count, "Deveria possuir 2 tarefas armazenadas!" );
            var t1 = tarefas.FirstOrDefault( o => o.Oid.Equals( novaTarefa1.Oid ) );
            var t2 = tarefas.FirstOrDefault( o => o.Oid.Equals( novaTarefa2.Oid ) );
            Assert.IsTrue( tarefas.Contains(t1), "Deveria encontrar a tarefa na lista" );
            Assert.IsTrue( tarefas.Contains( t2 ), "Deveria encontrar a tarefa na lista" );
        }

        /// <summary>
        /// Compara 2 objetos CronogramaTarefa
        /// </summary>
        /// <param name="t1">objeto 1</param>
        /// <param name="t2">objeto 2</param>
        /// <returns></returns>
        public bool CompararCronogramaTarefa( CronogramaTarefa t1, CronogramaTarefa t2 ) 
        {
            if(t1 == null && t2 == null)
                return true;

            if(t1 == null || t2 == null)
                return false;

            if(t1.Oid.Equals( t2.Oid ))
                return false;

            return false;
        }
    }
}
