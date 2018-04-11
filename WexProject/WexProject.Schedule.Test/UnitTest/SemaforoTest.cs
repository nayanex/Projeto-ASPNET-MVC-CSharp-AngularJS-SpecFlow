using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;
using Moq;
using WexProject.Library.Libs.SemaforoPorIntervalo;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Library.Libs.Test;
using WexProject.Schedule.Test.UnitTest;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.Scheduele.Test.Helpers.Mocks;
using System.Collections;
using WexProject.BLL.Entities.Planejamento;


namespace WexProject.Schedule.Test
{
    [TestClass]
    public class SemaforoTest : BaseEntityFrameworkTest
    {
        #region Atributos 

        /// <summary>
        /// Guarda todos os cronogramas existentes para o teste.
        /// </summary>
        private List<Cronograma> cronogramas = new List<Cronograma>();

        /// <summary>
        /// Objeto passado pelas threads para o método Aguarde do SemáforoSingleton.
        /// </summary>
        private struct cronogramaIntervalo
        {
            public Guid oidCronograma;
            public short inicio;
            public short final;
            public int tempoDeEspera;
        }

        #endregion

        /// <summary>
        /// Método responsável por criar o contexto para os testes.
        /// </summary>
        [TestInitialize]
        public void CriarContextoTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "C1", situacaoPlanejamento, new DateTime(), new DateTime(), true );
            Cronograma cronograma2 = CronogramaFactoryEntity.CriarCronograma( contexto, "C2", situacaoPlanejamento, new DateTime(), new DateTime(), true );
            Cronograma cronograma3 = CronogramaFactoryEntity.CriarCronograma( contexto, "C3", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            cronogramas.Add( cronograma1 );
            cronogramas.Add( cronograma2 );
            cronogramas.Add( cronograma3 );

        }

        /// <summary>
        /// Método de teste responsável por acionar o método RnControlarVerificacaoSemaforosImpactados da classe SemaforoSingleton.
        /// Obs: é utilizado pelas threads em paralelo.
        /// </summary>
        /// <param name="cronogramaIntervalo">Objeto (lista) contendo o Guid do cronograma e seu respectivo intervalo (range)</param>
        public void AcionarControleVerificacaoSemaforosImpactados(object cronogramaIntervalo)
        {
            int tempoEspera = ( (cronogramaIntervalo)cronogramaIntervalo ).tempoDeEspera;
            Thread.Sleep( tempoEspera );
            
            Debug.WriteLine( String.Format( "Thread: {0} acessou o método aguarde com os seguintes dados, Guid: {1}, min: {2}, max: {3}", Thread.CurrentThread.Name, 
                ( (cronogramaIntervalo)cronogramaIntervalo ).oidCronograma, ( (cronogramaIntervalo)cronogramaIntervalo ).inicio, ( (cronogramaIntervalo)cronogramaIntervalo ).final ) );

            SemaforoSingleton.GetInstancia().ControlarSemaforos( ( (cronogramaIntervalo)cronogramaIntervalo ).oidCronograma, ( (cronogramaIntervalo)cronogramaIntervalo ).inicio, ( (cronogramaIntervalo)cronogramaIntervalo ).final );
        }

        /// <summary>
        /// Método de teste responsável por acionar o método ConsultarSemaforosImpactadosPorCronograma da classe SemaforoSingleton.
        /// Obs: é utilizado pelas threads em paralelo.
        /// </summary>
        /// <param name="cronogramaIntervalo">Objeto (lista) contendo o Guid do cronograma e seu respectivo intervalo (range)</param>
        public void AcionarConsultaSemaforosImpactadosPorCronograma( object cronogramaIntervalo )
        {
            int tempoEspera = ( (cronogramaIntervalo)cronogramaIntervalo ).tempoDeEspera;
            Thread.Sleep( tempoEspera );

            Debug.WriteLine( String.Format( "Thread: {0} acessou o método aguarde com os seguintes dados, Guid: {1}, min: {2}, max: {3}", Thread.CurrentThread.Name,
                ( (cronogramaIntervalo)cronogramaIntervalo ).oidCronograma, ( (cronogramaIntervalo)cronogramaIntervalo ).inicio, ( (cronogramaIntervalo)cronogramaIntervalo ).final ) );

            SemaforoSingleton.GetInstancia().ConsultarSemaforosImpactadosPorCronograma( ( (cronogramaIntervalo)cronogramaIntervalo ).oidCronograma, ( (cronogramaIntervalo)cronogramaIntervalo ).inicio, ( (cronogramaIntervalo)cronogramaIntervalo ).final,new Hashtable() );
        }

        /// <summary>
        /// Cenário: Quando três threads acessarem o método aguarde simultaneamente antes que a validação por cronograma seja feita.
        /// Expectativa: 1- A thread 1 deverá ter criado um novo índice no dicionário de cronogramas, 
        ///                 deverá estar finalizada e ter criado um semáforo.
        ///              2- As demais threads deverão estar em espera, pois a primeira thread não liberou o lock.
        /// </summary>
        [Priority( 1 ), TestMethod]
        public void RnControlarVerificacaoSemaforosImpactadosQuandoTresThreadsDeCronogramasDiferentesAcessaremSimultaneamenteTest()
        {
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };
            mockSemaforo.Setup( o => o.LiberarEscritaCronogramas() );

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 1,
                final = 10,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[1].Oid,
                inicio = 11,
                final = 15,
                tempoDeEspera = 1000
            };

            cronogramaIntervalo cronoIntervalo3 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[2].Oid,
                inicio = 5,
                final = 12,
                tempoDeEspera = 1000
            };

            Thread thread1 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1"
            };

            Thread thread2 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            Thread thread3 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 3"
            };

            
            thread1.Start( cronoIntervalo1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Running;
            } );


            thread2.Start( cronoIntervalo2 );
            thread3.Start( cronoIntervalo3 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.WaitSleepJoin &&
                       thread3.ThreadState == System.Threading.ThreadState.WaitSleepJoin &&
                       SemaforoSingletonMock.lockerCronogramas.WaitingReadCount == 2;
            }, 15 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread 1, deveria estar em estado de finalizada, pois a thread 1 terminou de realizar o processamento necessário dentro do método Aguarde e está em processamento em outro método." );
            Assert.AreEqual( System.Threading.ThreadState.WaitSleepJoin, thread2.ThreadState, "A thread 2 deveria estar em estado de espera, pois a thread 1 está escrevendo na lista e a thread 2 não pode impactar a thread 2." );
            Assert.AreEqual( System.Threading.ThreadState.WaitSleepJoin, thread3.ThreadState, "A thread 3 deveria estar em estado de espera, pois a thread 1 está escrevendo na lista e a thread 3 não pode impactar a thread 1 e nem a 2." );
            Assert.AreEqual( 1, SemaforoSingletonMock.cronogramaSemaforosMock.Count, "Deveria ter 1 cronograma na lista." );
            Assert.AreEqual( 1, SemaforoSingletonMock.cronogramaSemaforosMock[cronogramas[0].Oid].semaforos.Count, "Deveria ter criado um semáforo." );
        }

        /// <summary>
        /// Cenário: Quando duas threads acessarem o método aguarde simultaneamente antes que a validação por cronograma seja feita,
        ///          e duas threads validarem ao mesmo tempo a existência de um cronograma e não encontratem e seguirem para a escrita
        ///          
        /// Expectativa: 1- as duas deverão entrar em modo de escrita, 
        ///                 porém 1 estará escrevendo e a outra esperando o término da escrita da atual, 
        /// </summary>
        [Priority( 2 ), TestMethod]
        public void RnControlarVerificacaoSemaforosImpactadosQuandoDuasThreadsDeCronogramasDiferentesAcessaremSimultaneamenteUmaEstiverEscrevendoNaListaEUmaEsperandoParaEscreverNaListaTest()
        {
            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };
            mockSemaforo.Setup(o => o.EsperarLeituraCronogramas());
            mockSemaforo.Setup( o => o.LiberarLeituraCronogramas());
            mockSemaforo.Setup( o => o.LiberarEscritaCronogramas() );

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 1,
                final = 10,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[1].Oid,
                inicio = 11,
                final = 15,
                tempoDeEspera = 1000
            };

            Thread thread1 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Running;
            } );

            thread2.Start( cronoIntervalo2 );
            
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.WaitSleepJoin &&
                       SemaforoSingletonMock.lockerCronogramas.WaitingWriteCount == 1;
            }, 100 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread 1, deveria estar em estado de finalizada, pois a thread 1 terminou de realizar o processamento necessário dentro do método Aguarde e está em processamento em outro método." );
            Assert.AreEqual( System.Threading.ThreadState.WaitSleepJoin, thread2.ThreadState, "A thread 2 deveria estar em estado de espera, pois a thread 1 ainda está realizando o processamento e a thread 2 não pode impactar a thread 2." );
            Assert.AreEqual( 1, SemaforoSingletonMock.lockerCronogramas.WaitingWriteCount, "Deveria ter 1 thread em espera de escrita, pois a thread 1 está realizando a escrita." );
            Assert.AreEqual( 1, SemaforoSingletonMock.cronogramaSemaforosMock.Count, "Deveria ter 1 cronograma na lista." );
            Assert.AreEqual( 1, SemaforoSingletonMock.cronogramaSemaforosMock[cronogramas[0].Oid].semaforos.Count, "Deveria ter criado um semáforo." );
        }

        /// <summary>
        /// Cenário: Quando duas threads acessarem o método aguarde simultaneamente antes que a validação por cronograma seja feita,
        ///          e duas threads validarem ao mesmo tempo a existência de um cronograma e não encontratem e seguirem para a escrita
        ///          
        /// Expectativa: 1- as duas deverão entrar em modo de escrita, 
        ///                 porém 1 estará escrevendo e a outra esperando o término da escrita da atual, ao término da espera então a 2 thread 
        ///                 deve verificar se já existe um cronograma no dicionário e não deve criá-lo novamente, pois a primeira thread já criou
        /// </summary>
        [Priority( 1 ), TestMethod]
        public void RnControlarVerificacaoSemaforosImpactadosQuandoDuasThreadsDeCronogramasIguaisAcessaremSimultaneamenteEUmaEstiverEscrevendoNaListaEUmaEsperandoNaEscritaParaAdicionarOMesmoCronogramaTest()
        {
            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };
            mockSemaforo.Setup( o => o.EsperarLeituraCronogramas() );
            mockSemaforo.Setup( o => o.LiberarLeituraCronogramas() );

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 1,
                final = 10,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 11,
                final = 15,
                tempoDeEspera = 500
            };

            Thread thread1 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarControleVerificacaoSemaforosImpactados )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Running;
            } );

            thread2.Start( cronoIntervalo2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.Stopped;
            }, 15 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread 1, deveria estar em estado de finalizada, pois a thread 1 terminou de realizar o processamento necessário dentro do método Aguarde e está em processamento em outro método." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread 2 deveria estar em estado de finalizada, pois a thread 2 esperou a thread 1 e depois finalizou seu processamento." );
            Assert.AreEqual( 1, SemaforoSingletonMock.cronogramaSemaforosMock.Count, "Deveria ter 1 cronograma na lista, pois a thread 1 já adicionou-o." );
        }

        /// <summary>
        /// Cenário: Quando 2 threads (com intervalos entre [5..10] e [8..15]) acessarem o método ConsultarSemaforosImpactadosPorCronograma 
        ///          simultaneamente, porém com cronogramas diferentes.
        ///          
        /// Expectativa: 1 - As 2 threads devem estar com seus processos finalizados, pois uma não pode impactar a outra já que estão em cronogramas diferentes.
        ///              2 - deve ter criado 2 índices no dicionário de cronogramas e 2 semáforos (1 de contexto e 1 da thread) para cada lista de semáforos.
        ///              3 - O método deve retornar duas listas vazias para cada thread (em análise).
        /// </summary>
        [TestMethod]
        public void ConsultarSemaforosImpactadosPorCronogramaQuandoThreadsAcessaremSimultanementeDeCronogramasDiferentesTest()
        {
            SemaforoPorIntervalo semaforoIntervalo1 = new SemaforoPorIntervalo( 80, 100 );
            SemaforoPorIntervalo semaforoIntervalo2 = new SemaforoPorIntervalo( 80, 100 );

            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC1 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };
            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC2 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };

            semaforoPorCronogramaC1.semaforos.Add( semaforoIntervalo1 );
            semaforoPorCronogramaC2.semaforos.Add( semaforoIntervalo2 );

            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };
            mockSemaforo.Setup( o => o.LiberarEscritaSemaforos( It.IsAny<Guid>() ) );

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[1].Oid, semaforoPorCronogramaC2 );
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[0].Oid, semaforoPorCronogramaC1 );

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 5,
                final = 10,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[1].Oid,
                inicio = 8,
                final = 15,
                tempoDeEspera = 0
            };

            Thread thread1 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );
            thread2.Start( cronoIntervalo2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                thread2.ThreadState == System.Threading.ThreadState.Stopped;
            }, 60 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( 2, SemaforoSingleton.semaforosPorCronograma.Count, "Deveria existir dois índices no dicionário de cronograma, pois 2 threads de cronogramas diferentes efetuaram o processo.");
            Assert.AreEqual( 2, SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos.Count, "Deveria existir uma lista contendo 2 semáforos criados." );
            Assert.AreEqual( 2, SemaforoSingleton.semaforosPorCronograma[cronogramas[1].Oid].semaforos.Count, "Deveria existir uma lista contendo 2 semáforos criados." );
        }
        
        /// <summary>
        /// Cenário: Quando 2 threads estiverem acessando simultaneamente o método ConsultarSemaforosImpactadosPorCronograma com intervalos ([10..20] e [21..28]) não conflitantes.
        ///          
        /// Expectativa: 1 - Os 2 processos das threads devem estar finalizados
        ///              2 - Deve ter 1 cronograma no índice no dicionário de cronogramas
        ///              3 - Deve ter criado 3 semáforos(1 da criação do contexto e 2 das threads) e adicionado na lista de semáforos do cronograma.
        ///              4 - O método deve retornar uma lista vazia para cada thread.
        /// </summary>
        [TestMethod]
        public void ConsultarSemaforosImpactadosPorCronogramaQuandoThreadsDoMesmoCronogramaComIntervalosNaoConflitantesTest()
        {
            SemaforoPorIntervalo semaforoIntervalo1 = new SemaforoPorIntervalo( 80, 100 );

            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC1 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };
            
            semaforoPorCronogramaC1.semaforos.Add( semaforoIntervalo1 );
            
            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[0].Oid, semaforoPorCronogramaC1 );

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 10,
                final = 20,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 21,
                final = 28,
                tempoDeEspera = 0
            };

            Thread thread1 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );
            thread2.Start( cronoIntervalo2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.Stopped;
            }, 60 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( 1, SemaforoSingleton.semaforosPorCronograma.Count, "Deveria existir um índice no dicionário de cronograma, pois 2 threads de cronogramas iguais efetuaram o processo." );
            Assert.AreEqual( 3, SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos.Count, "Deveria existir uma lista contendo 3 semáforos criados." );

            List<SemaforoPorIntervalo> semaforosCriados = SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos;

            Assert.IsNotNull(semaforosCriados.Where( o => o.inicio == cronoIntervalo1.inicio && o.final == cronoIntervalo1.final ), "Deveria existir o semáforo, pois a thread 1 criou.");
            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == cronoIntervalo2.inicio && o.final == cronoIntervalo2.final ), "Deveria existir o semáforo, pois a thread 2 criou." );
        }
        
        /// <summary>
        /// Cenário: Quando 2 threads estiverem acessando simultaneamente o método ConsultarSemaforosImpactadosPorCronograma com intervalos ([10..20] e [5..15]) conflitantes.
        ///          
        /// Expectativa: 1 - Os 2 processos das threads devem estar finalizados
        ///              2 - Deve ter 1 cronograma no índice no dicionário de cronogramas
        ///              3 - Deve ter criado 2 semáforos e adicionado na lista de semáforos do cronograma.
        ///              4 -  A thread com intervalo de [5..15] deve conter na sua lista de semáforos impactados à esperar 1 semáforo de 10..20
        /// </summary>
        [TestMethod]
        public void ConsultarSemaforosImpactadosPorCronogramaQuandoThreadsDoMesmoCronogramaComIntervalosConflitantesTest()
        {
            SemaforoPorIntervalo semaforoIntervalo1 = new SemaforoPorIntervalo( 80, 100 );

            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC1 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };

            semaforoPorCronogramaC1.semaforos.Add( semaforoIntervalo1 );

            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[0].Oid, semaforoPorCronogramaC1 );

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 10,
                final = 20,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 5,
                final = 15,
                tempoDeEspera = 5000
            };

            Thread thread1 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );
            thread2.Start( cronoIntervalo2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.Stopped;
            }, 60 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( 1, SemaforoSingleton.semaforosPorCronograma.Count, "Deveria existir um índice no dicionário de cronograma, pois 2 threads de cronogramas iguais efetuaram o processo." );
            Assert.AreEqual( 3, SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos.Count, "Deveria existir uma lista contendo 3 semáforos criados." );

            List<SemaforoPorIntervalo> semaforosCriados = SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos;

            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == cronoIntervalo1.inicio && o.final == cronoIntervalo1.final ), "Deveria existir o semáforo, pois a thread 1 criou." );
            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == cronoIntervalo2.inicio && o.final == cronoIntervalo2.final ), "Deveria existir o semáforo, pois a thread 2 criou." );
        }
        
        /// <summary>
        /// Cenário: Quando 2 threads estiverem acessando simultaneamente o método ConsultarSemaforosImpactadosPorCronograma com intervalos de 
        ///          [1..20] e [5..15] e já existir um semáforo [10..20].
        ///          
        /// Expectativa: 1 - Os 2 processos de cada thread já devem estar finalizados.
        ///              2 - Deve ter 1 cronograma no índice no dicionário de cronogramas
        ///              3 - Devem existir 2 semáforos ([10..20], [1..9]) e o intervalo de [5..15] deve reaproveitar os 2 semáforos, portanto não criando nenhum. 
        ///              4 - Deve verificar se os retornos das threads estão com os semáforos apropriados. (TESTAR SE É POSSÍVEL)
        /// </summary>
        [TestMethod]
        public void ConsultarSemaforosImpactadosPorCronogramaQuandoThreadsDoMesmoCronogramaComTresIntervalosConflitantesTest()
        {
            //semáforo já existente
            SemaforoPorIntervalo semaforoIntervalo1 = new SemaforoPorIntervalo( 10, 20 );

            //semáforo já existente no cronograma
            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC1 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };

            semaforoPorCronogramaC1.semaforos.Add( semaforoIntervalo1 );

            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[0].Oid, semaforoPorCronogramaC1 );

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 1,
                final = 20,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 5,
                final = 15,
                tempoDeEspera = 5000
            };

            Thread thread1 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );
            thread2.Start( cronoIntervalo2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.Stopped;
            }, 60 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( 1, SemaforoSingleton.semaforosPorCronograma.Count, "Deveria existir um índice no dicionário de cronograma, pois 2 threads de cronogramas iguais efetuaram o processo." );
            Assert.AreEqual( 2, SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos.Count, "Deveria existir uma lista contendo 2 semáforos criados." );

            List<SemaforoPorIntervalo> semaforosCriados = SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos;

            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == cronoIntervalo1.inicio && o.final == cronoIntervalo1.final ), "Deveria existir o semáforo, pois a thread 1 criou." );
            Assert.IsNull( semaforosCriados.Where( o => o.inicio == cronoIntervalo2.inicio && o.final == cronoIntervalo2.final ).FirstOrDefault() , "Não Deveria existir o semáforo, pois a thread 2 não criou e reaproveitou os semáforos existentes." );
        }

        /// <summary>
        /// Cenário: Quando 2 threads simultaneas estiverem acessando o método ConsultarSemaforosImpactadosPorCronograma com intervalos 
        ///          de [5..15] e [8..22] e já existir um intervalo de [10..20]
        ///          
        /// Expectativa: 1 - Os processos de cada thread já devem estar finalizados.
        ///              2 - Deve ter 1 cronograma no índice no dicionário de cronogramas
        ///              3 - Devem existir 3 semáforos [10..20], [5..9] e [21..22]
        ///              4 - Deve verificar se os retornos das threads estão com os semáforos apropriados. (TESTAR SE É PÓSSÍVEL)
        /// </summary>
        [TestMethod]
        public void ConsultarSemaforosImpactadosPorCronogramaQuandoIntervaloAbrangerOutrosIntervalosTest()
        {
            //semáforo já existente
            SemaforoPorIntervalo semaforoIntervalo1 = new SemaforoPorIntervalo( 10, 20 );

            //semáforo já existente no cronograma
            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC1 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };

            semaforoPorCronogramaC1.semaforos.Add( semaforoIntervalo1 );

            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[0].Oid, semaforoPorCronogramaC1 );

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 5,
                final = 15,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 8,
                final = 22,
                tempoDeEspera = 5000
            };

            Thread thread1 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            thread1.Start( cronoIntervalo1 );
            thread2.Start( cronoIntervalo2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                       thread2.ThreadState == System.Threading.ThreadState.Stopped;
            }, 60 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( 1, SemaforoSingleton.semaforosPorCronograma.Count, "Deveria existir um índice no dicionário de cronograma, pois 2 threads de cronogramas iguais efetuaram o processo." );
            Assert.AreEqual( 3, SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos.Count, "Deveria existir uma lista contendo 3 semáforos criados." );

            List<SemaforoPorIntervalo> semaforosCriados = SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos;

            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == semaforoIntervalo1.inicio && o.final == semaforoIntervalo1.final ), "Deveria existir o semáforo, pois já estava criado." );
            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == 5 && o.final == 9 ), "Deveria existir o semáforo, pois a thread 1 criou." );
            Assert.IsNotNull( semaforosCriados.Where( o => o.inicio == 21 && o.final == 22 ), "Deveria existir o semáforo, pois a thread 2 criou." );
        }

        /// <summary>
        /// Cenário: Quando 6 threads simultaneas estiverem acessando o método ConsultarSemaforosImpactadosPorCronograma com intervalos 
        ///          de [5 a 8], [1 a 5], [8 a 10], [2 a 5], [2 a 4] e [1 a 6] e já existir um intervalo de [10..20]
        ///          
        /// Expectativa: 1 - Os processos de cada thread já devem estar finalizados.
        ///              2 - Deve ter 1 cronograma no índice no dicionário de cronogramas
        ///              3 - Devem existir 4 semáforos ([10..20], [5..10], [1..5], [15..20])
        ///              4 - Deve verificar se os retornos das threads estão com os semáforos apropriados. (TESTAR SE É PÓSSÍVEL)
        /// </summary>
        [TestMethod]
        public void ConsultarSemaforosImpactadosPorCronogramaQuandoExistiremVariosConflitantesTest()
        {
            #region contexto

            //semáforo já existente
            SemaforoPorIntervalo semaforoIntervalo1 = new SemaforoPorIntervalo( 10, 20 );

            //semáforo já existente no cronograma
            SemaforoSingleton.SemaforosControle semaforoPorCronogramaC1 = new SemaforoSingleton.SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };

            semaforoPorCronogramaC1.semaforos.Add( semaforoIntervalo1 );

            //mockando o método que realiza a liberação de um semáforo.
            Mock<SemaforoSingletonMock> mockSemaforo = new Mock<SemaforoSingletonMock>()
            {
                CallBase = true
            };

            SemaforoSingletonMock.SetInstancia( mockSemaforo.Object );

            SemaforoSingletonMock.cronogramaSemaforosMock.Clear();
            SemaforoSingleton.semaforosPorCronograma.Add( cronogramas[0].Oid, semaforoPorCronogramaC1 );

            cronogramaIntervalo cronoIntervalo1 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 5,
                final = 8,
                tempoDeEspera = 0
            };

            cronogramaIntervalo cronoIntervalo2 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 1,
                final = 5,
                tempoDeEspera = 6000
            };

            cronogramaIntervalo cronoIntervalo3 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 6,
                final = 10,
                tempoDeEspera = 8000
            };

            cronogramaIntervalo cronoIntervalo4 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 2,
                final = 5,
                tempoDeEspera = 1000
            };

            cronogramaIntervalo cronoIntervalo5 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 2,
                final = 4,
                tempoDeEspera = 2000
            };

            cronogramaIntervalo cronoIntervalo6 = new cronogramaIntervalo()
            {
                oidCronograma = cronogramas[0].Oid,
                inicio = 1,
                final = 6,
                tempoDeEspera = 3000
            };

            Thread thread1 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread 1",
            };

            Thread thread2 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 2"
            };

            Thread thread3 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 3"
            };

            Thread thread4 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 4"
            };

            Thread thread5 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 5"
            };

            Thread thread6 = new Thread( AcionarConsultaSemaforosImpactadosPorCronograma )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 6"
            };

            #endregion

            thread1.Start( cronoIntervalo1 );
            thread2.Start( cronoIntervalo2 );
            thread3.Start( cronoIntervalo3 );
            thread4.Start( cronoIntervalo4 );
            thread5.Start( cronoIntervalo5 );
            thread6.Start( cronoIntervalo6 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == System.Threading.ThreadState.Stopped &&
                        thread2.ThreadState == System.Threading.ThreadState.Stopped &&
                        thread3.ThreadState == System.Threading.ThreadState.Stopped &&
                        thread4.ThreadState == System.Threading.ThreadState.Stopped &&
                        thread5.ThreadState == System.Threading.ThreadState.Stopped &&
                        thread6.ThreadState == System.Threading.ThreadState.Stopped;
            }, 180 );

            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread1.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( System.Threading.ThreadState.Stopped, thread2.ThreadState, "A thread deveria estar com o processo finalizado, pois não depende de nenhuma outra, ou seja, não impacta nenhuma outra." );
            Assert.AreEqual( 1, SemaforoSingleton.semaforosPorCronograma.Count, "Deveria existir um índice no dicionário de cronograma, pois 2 threads de cronogramas iguais efetuaram o processo." );
            Assert.AreEqual( 5, SemaforoSingleton.semaforosPorCronograma[cronogramas[0].Oid].semaforos.Count, "Deveria existir uma lista contendo 3 semáforos criados." );
        }
    }
}
