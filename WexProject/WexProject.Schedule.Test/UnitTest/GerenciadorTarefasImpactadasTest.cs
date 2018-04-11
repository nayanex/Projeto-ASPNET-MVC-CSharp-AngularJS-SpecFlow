using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Library.Libs.GerenciadorComandos;
using System.Diagnostics;
using System.Threading;
using WexProject.Schedule.Test.Helpers.Utils;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Models;
using WexProject.BLL;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Contexto;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class GerenciadorTarefasImpactadasTest : BaseEntityFrameworkTest
    {
        private GerenciadorTarefasImpactadas gerenciador;
        private List<string> tarefas;
        public GerenciadorTarefasImpactadasTest()
        {
            tarefas = GerarTarefas( 10 );
        }

        [TestInitialize]
        public void Inicializar()
        {
            gerenciador = new GerenciadorTarefasImpactadas();
        }

        /// <summary>
        /// Método utilizado para gerar um quantidade de tarefas
        /// </summary>
        /// <param name="quantidade">quantidade de tarefas</param>
        /// <returns></returns>
        public static List<string> GerarTarefas( int quantidade )
        {
            List<string> tarefas = new List<string>();
            for(int i = 0; i < quantidade; i++)
            {
                tarefas.Add( Guid.NewGuid().ToString() );
            }

            return tarefas;
        }

        public Dictionary<string, Int16> GerarTarefasImpactadas( int quantidade )
        {
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            if(tarefas == null)
                tarefas = new List<string>();
            if(tarefas.Count <= 0)
                tarefas = GerarTarefas( 10 );

            Random aleatorio = new Random();

            if(quantidade > tarefas.Count)
            {
                quantidade = aleatorio.Next( tarefas.Count - 1 );
            }
            int num;
            string tarefa;
            string numero;
            while(tarefasImpactadas.Count < quantidade)
            {
                num = aleatorio.Next( tarefas.Count - 1 );
                tarefa = tarefas[num];
                if(!tarefasImpactadas.ContainsKey( tarefa ))
                {
                    numero = num.ToString();
                    tarefasImpactadas.Add( tarefa, Convert.ToInt16( numero ) );
                }
            }

            return tarefasImpactadas;
        }

        [TestMethod]
        public void QuandoATarefaImpactadaAindaNaoExistirNaListaDeTarefasAtualizada()
        {
            Dictionary<string, Int16> tarefasImpactadas = GerarTarefasImpactadas( 5 );

            List<string> tarefasEsperadas = tarefasImpactadas.Keys.ToList();
            DateTime dataAtual = DateTime.Now;
            Dictionary<string, Int16> tarefasImpactadasRetornadas = gerenciador.ListarAtualizacoesValidas( tarefasImpactadas, dataAtual );
            CollectionAssert.AreEquivalent( tarefasEsperadas, gerenciador.TarefasAtualizadas.Keys.ToList(), "Todas as tarefas esperadas deveriam estar contidas" );
            CollectionAssert.AreEquivalent( tarefasImpactadas, tarefasImpactadasRetornadas, "As tarefas impactadas retornadas deveriam ser as mesmas impactadas pois é a primeira atualização" );
        }

        [TestMethod]
        public void QuandoATarefaImpactadaJaExistirEForMaisAtualizada()
        {
            //Gerando tarefas impactadas para as tarefas criadas
            Dictionary<string, Int16> tarefasImpactadas = GerarTarefasImpactadas( 5 );
            Dictionary<string, Int16> tarefasImpactadas2 = GerarTarefasImpactadas( 5 );

            //gerando as datas de atualização
            DateTime dataAntiga = DateTime.Now;
            DateTime dataMaisAtual = DateTime.Now.AddSeconds( 2 );

            //armazenando as tarefas esperadas para o teste
            List<string> tarefasEsperadas1 = tarefasImpactadas.Keys.ToList();
            List<string> tarefasEsperadas2 = tarefasImpactadas2.Keys.ToList();

            //Removendo as que já foram atualizadas por uma mensagem mais atual 
            tarefasEsperadas2 = tarefasEsperadas2.Except( tarefasEsperadas1 ).ToList();

            //Efetuando  a listagem de atualização com as tarefas mais atualizadas
            Dictionary<string, Int16> tarefasImpactadasRetornadas1 = gerenciador.ListarAtualizacoesValidas( tarefasImpactadas, dataMaisAtual );

            //Efetuando a listagem de atualização da segunda mensagem com uma data anterior com tarefas desatualizadas
            Dictionary<string, Int16> tarefasImpactadasRetornadas2 = gerenciador.ListarAtualizacoesValidas( tarefasImpactadas2, dataAntiga );

            //Testar as tarefas retornadas pela segunda mensagem são apenas as que não possuiam versão mais atual armazenada
            Assert.AreEqual( tarefasEsperadas2.Count, tarefasImpactadasRetornadas2.Count, string.Format( "As tarefas retornadas deveriam ser a mesma quantidade de tarefas esperadas que fossem atualizadas, Quantidade: {0} ", tarefasImpactadasRetornadas2.Count ) );
            //Comparando se as tarefas impactadas retornadas estão corretas
            CollectionAssert.AreEquivalent( tarefasEsperadas2, tarefasImpactadasRetornadas2.Keys.ToList(), "As tarefas retornadas deveriam corresponder as esperadas" );

            //Unindo as tarefas mais atualizadas das 2 mensagens
            List<string> uniaoTarefasAtualizadas = tarefasEsperadas1.Union( tarefasEsperadas2 ).ToList();

            //Comparando se o gerenciador possui a lista mais atualizada de tarefas
            CollectionAssert.AreEquivalent( uniaoTarefasAtualizadas, gerenciador.TarefasAtualizadas.Keys.ToList(), "A lista de tarefas atualizadas deveria corresponder as tarefas atualizadas pela primeira e segunda atualização juntas" );

            //Resgatando as tarefas que utilizaram a data mais atualizada
            List<string> tarefasComDataMaisAtual = gerenciador.TarefasAtualizadas.Where( o => o.Value == dataMaisAtual ).Select( o => o.Key ).ToList();
            //Resgatando as tarefas que utilizaram a data mais antiga
            List<string> tarefasComDataMaisAntiga = gerenciador.TarefasAtualizadas.Where( o => o.Value == dataAntiga ).Select( o => o.Key ).ToList();

            //Comparando se o retorno da primeira mensagem  (Com data mais atual) é equivalente as tarefas impactadas retornadas pela primeira mensagem
            CollectionAssert.AreEquivalent( tarefasComDataMaisAtual, tarefasImpactadasRetornadas1.Keys.ToList(), "A lista de tarefas com a data mais atual deveria ser a mesma retornada nas tarefas impactadas 1" );

            //Comparando se o retorno da segunda mensagem  (Com data mais antiga) é equivalente as tarefas impactadas retornadas pela segunda mensagem
            CollectionAssert.AreEquivalent( tarefasComDataMaisAntiga, tarefasImpactadasRetornadas2.Keys.ToList(), "A lista de tarefas com a data desatualizada deveria ser a mesma retornada nas tarefas impactadas 2" );
        }

        [TestMethod]
        public void QuandoATarefaImpactadaJaExistirEEstiverDesatualizada()
        {
            //Gerando tarefas impactadas para as tarefas criadas
            Dictionary<string, Int16> tarefasImpactadas = GerarTarefasImpactadas( 5 );
            Dictionary<string, Int16> tarefasImpactadas2 = GerarTarefasImpactadas( 5 );

            //gerando as datas de atualização
            DateTime data1 = DateTime.Now;
            DateTime data2 = DateTime.Now.AddSeconds( 2 );

            //armazenando as tarefas esperadas para o teste
            List<string> tarefasEsperadas1 = tarefasImpactadas.Keys.ToList();
            List<string> tarefasEsperadas2 = tarefasImpactadas2.Keys.ToList();

            //Recuperando as terefas que foram atualizadas pela segunda mensagem como mais atuais
            List<string> tarefasAtualizadas = tarefasEsperadas1.Intersect( tarefasEsperadas2 ).ToList();

            //Efetuando  a listagem de atualização com as tarefas mais atualizadas
            Dictionary<string, Int16> tarefasImpactadasRetornadas1 = gerenciador.ListarAtualizacoesValidas( tarefasImpactadas, data1 );

            //Efetuando a listagem de atualização da segunda mensagem com uma data anterior com tarefas desatualizadas
            Dictionary<string, Int16> tarefasImpactadasRetornadas2 = gerenciador.ListarAtualizacoesValidas( tarefasImpactadas2, data2 );

            Assert.AreEqual( tarefasImpactadas2.Count, tarefasImpactadasRetornadas2.Count, "Deveria ter atualizado todas as tarefas impactadas por ser mais atual" );

            CollectionAssert.AreEquivalent( tarefasImpactadas2, tarefasImpactadasRetornadas2, "As tarefas impactadas retornadas deveriam ser as mesmas que foram solicitadas a atualização" );

            //Resgatando as tarefas que utilizaram a data mais atualizada
            List<string> tarefasComDataMaisAtual = gerenciador.TarefasAtualizadas.Where( o => o.Value == data2 ).Select( o => o.Key ).ToList();

            //Resgatando as tarefas que utilizaram a data mais antiga
            List<string> tarefasComDataMaisAntiga = gerenciador.TarefasAtualizadas.Where( o => o.Value == data1 ).Select( o => o.Key ).ToList();

            List<string> tarefasComDataAntigaEsperadas = tarefasEsperadas1.Except( tarefasEsperadas2 ).ToList();

            CollectionAssert.AreEquivalent( tarefasComDataAntigaEsperadas, tarefasComDataMaisAntiga, "Deveria conter apenas as tarefas que não foram atualizadas pela segunda mensagem" );


            CollectionAssert.AreEquivalent( tarefasComDataMaisAtual, tarefasEsperadas2, "As tarefas com data mais atual deveria ser as mesmas da segunda mensagem" );
        }

        [TestMethod]
        public void QuandoReceberVariasAtualizacoesDeTelaForaDeOrdem()
        {
            Colaborador colaborador = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );
            Cronograma cronograma = new Cronograma();
            cronograma.DtInicio = DateTime.Now;
            cronograma.DtFinal = DateTime.Now.AddDays( 30 );
            SituacaoPlanejamento situacao = new SituacaoPlanejamento();
            situacao.CsPadrao = CsPadraoSistema.Sim;
            situacao.CsSituacao = CsTipoSituacaoPlanejamento.Ativo;
            situacao.CsTipo = CsTipoPlanejamento.Planejamento;
            situacao.TxDescricao = "S1";
            contexto.SituacaoPlanejamento.Add( situacao );
            cronograma.SituacaoPlanejamento = situacao;
            contexto.Cronograma.Add( cronograma );
            contexto.SaveChanges();
            List<CronogramaTarefa> tarefas = new List<CronogramaTarefa>();
            List<CronogramaTarefa> listaDeTarefasImpactadas = new List<CronogramaTarefa>();
            DateTime data;
            CronogramaTarefa tarefaTemp;
            for(Int16 i = 0; i < 6; i++)
            {
                data = DateTime.Now;
                tarefaTemp = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid, string.Format( "T{0}", ( i + 1 ) ), situacao, data, "", "gabriel.matos", out listaDeTarefasImpactadas, ref data );
                tarefas.Add( tarefaTemp );
            }

            List<CronogramaTarefaDto> tarefasCronograma = new List<CronogramaTarefaDto>( tarefas.Select( o => CronogramaTarefaBo.DtoFactory( o ) ) );
            Guid oidT1 = tarefas[0].Oid;
            Guid oidT2 = tarefas[1].Oid;
            Guid oidT3 = tarefas[2].Oid;
            Guid oidT4 = tarefas[3].Oid;
            Dictionary<string, short> listaAtualizacao;

            //Movimentação 1
            TarefasMovidasDto movidas = MoverTarefa( contexto, tarefas, oidT3, 1, tarefasCronograma, "Movimento 1" );

            //Movimentação 2
            TarefasMovidasDto movidas2 = MoverTarefa( contexto, tarefas, oidT2, 4, tarefasCronograma, "Movimento 2" );

            //Movimentação 3
            TarefasMovidasDto movidas3 = MoverTarefa( contexto, tarefas, oidT3, 3, tarefasCronograma, "Movimento 3" );

            //Movimentação 4
            TarefasMovidasDto movidas4 = MoverTarefa( contexto, tarefas, oidT2, 2, tarefasCronograma, "Movimento 4" );

            //Movimentação 5
            TarefasMovidasDto movidas5 = MoverTarefa( contexto, tarefas, oidT4, 4, tarefasCronograma, "Movimento 5" );

            //Movimentação 5
            TarefasMovidasDto movidas6 = MoverTarefa( contexto, tarefas, oidT2, 1, tarefasCronograma, "Movimento 6" );

            listaAtualizacao = gerenciador.ListarAtualizacoesValidas( movidas5.TarefasImpactadas, movidas5.DataHoraAcao );
            tarefasCronograma = AtualizarTarefaImpactadas( listaAtualizacao, tarefasCronograma, "Atualização 5" );

            listaAtualizacao = gerenciador.ListarAtualizacoesValidas( movidas2.TarefasImpactadas, movidas2.DataHoraAcao );
            tarefasCronograma = AtualizarTarefaImpactadas( listaAtualizacao, tarefasCronograma, "Atualização 2" );

            listaAtualizacao = gerenciador.ListarAtualizacoesValidas( movidas.TarefasImpactadas, movidas.DataHoraAcao );
            tarefasCronograma = AtualizarTarefaImpactadas( listaAtualizacao, tarefasCronograma, "Atualização 1" );

            listaAtualizacao = gerenciador.ListarAtualizacoesValidas( movidas4.TarefasImpactadas, movidas4.DataHoraAcao );
            tarefasCronograma = AtualizarTarefaImpactadas( listaAtualizacao, tarefasCronograma, "Atualização 4" );

            listaAtualizacao = gerenciador.ListarAtualizacoesValidas( movidas6.TarefasImpactadas, movidas6.DataHoraAcao );
            tarefasCronograma = AtualizarTarefaImpactadas( listaAtualizacao, tarefasCronograma, "Atualização 6" );

            listaAtualizacao = gerenciador.ListarAtualizacoesValidas( movidas3.TarefasImpactadas, movidas3.DataHoraAcao );
            tarefasCronograma = AtualizarTarefaImpactadas( listaAtualizacao, tarefasCronograma, "Atualização 3" );

            

            Assert.AreEqual( 6, tarefasCronograma.Select( o => o.NbID ).Distinct().Count(),"Deveria possuir 6 números, pois nenhum deveria ter se repetido!" );
            Dictionary<Guid, short> ordem_Oid_E_NbID_BaseDeDados = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma.Oid ).ToDictionary( o => o.Oid, o => o.NbID );
            Dictionary<Guid, short> ordem_Oid_E_NbID_Tela = tarefasCronograma.ToDictionary( o => o.OidCronogramaTarefa, o => (short)o.NbID );
            CollectionAssert.AreEquivalent( ordem_Oid_E_NbID_BaseDeDados, ordem_Oid_E_NbID_Tela,"A ordem das tarefas na tela deveria estar em sicronia com o base, após as atualizações" );
        }

        /// <summary>
        /// Método para teste simulando a movimentação das tarefas (Serviço)
        /// </summary>
        /// <param name="tarefas">lista de tarefas</param>
        /// <param name="oidTarefaMovida">oid da tarefa movimentada</param>
        /// <param name="posicaoFinal">posição desejada para a tarefa</param>
        /// <returns></returns>
        private static TarefasMovidasDto MoverTarefa( WexDb contexto, List<CronogramaTarefa> tarefas,Guid oidTarefaMovida,Int16 posicaoFinal,List<CronogramaTarefaDto> tarefasCronograma,string alias = null)
        {
            List<CronogramaTarefa> listaDeTarefasImpactadas;
            Int16 novoNbIdTarefaMovida = 0;
            DateTime dataHoraAcao = new DateTime();
            Guid oidCronograma = new Guid();
            listaDeTarefasImpactadas = CronogramaTarefaBo.ReordenarTarefas( oidTarefaMovida, posicaoFinal, ref novoNbIdTarefaMovida, ref dataHoraAcao, ref oidCronograma );
            TarefasMovidasDto movidas = CronogramaTarefaDao.TarefasMovidasDtoFactory( listaDeTarefasImpactadas, DateTime.Now, oidTarefaMovida, novoNbIdTarefaMovida, oidCronograma );
            movidas.TarefasImpactadas.Add( movidas.OidCronogramaTarefaMovida.ToString(), movidas.NbIDTarefaMovida );
            TarefasImpactadasDebugUtil.ExibirLogTarefaMovida( movidas, tarefasCronograma,alias );
            return movidas;
        }

        /// <summary>
        /// Método para efetuar a atualização das tarefas impactadas
        /// </summary>
        /// <param name="tarefasImpactadas">lista de tarefas impactadas</param>
        /// <param name="tarefas">lista de tarefas armazenadas(View)</param>
        /// <returns>retorna a lista com a atualização das tarefas impactadas</returns>
        public static List<CronogramaTarefaDto> AtualizarTarefaImpactadas( Dictionary<string, short> tarefasImpactadas, List<CronogramaTarefaDto> tarefas,string alias = "" )
        {
            Debug.WriteLine( string.Format( "\n***********  Inicio {0}  ************** \n", alias ) );
            TarefasImpactadasDebugUtil.ExibirOrdemTarefas( tarefas, "Ordem Inicial" );
            if(tarefasImpactadas.Count > 0)
            {
                TarefasImpactadasDebugUtil.ExibirLogAtualizacaoTarefasImpactadas( tarefasImpactadas, tarefas );
                foreach(var item in tarefasImpactadas)
                {
                    AtualizarNbIdTarefa( item.Value, Guid.Parse( item.Key ), tarefas );
                }
                tarefas = new List<CronogramaTarefaDto>( tarefas.OrderBy( o => o.NbID ) );
                TarefasImpactadasDebugUtil.ExibirOrdemTarefas( tarefas, "Ordem Final" );
            }
            else
            {
                Debug.WriteLine( "Nenhuma tarefa impactada para atualizar!" );
            }

            Debug.WriteLine( string.Format( "\n***********  Fim {0}  ************** \n", alias ) );
            return tarefas;
        }

        /// <summary>
        /// Método utilizado para atualizar o nbid das tarefas (Simulando a atualização de tarefas do CronogramaPresenter)
        /// </summary>
        /// <param name="novoNbId">novo nbid</param>
        /// <param name="oidTarefa">oid da tarefa a ser atualizada</param>
        /// <param name="tarefas">lista de tarefas armazenadas(View)</param>
        public static void AtualizarNbIdTarefa( short novoNbId, Guid oidTarefa, List<CronogramaTarefaDto> tarefas )
        {
            if(tarefas == null)
                return;
            
            CronogramaTarefaDto tarefa = tarefas.FirstOrDefault( o => o.OidCronogramaTarefa == oidTarefa );
            if(tarefa != null)
            {
                tarefa.NbID = novoNbId;
            }
        }

        [TestMethod]
        public void DeveAplicarDataDeAtualizacaoMaisRecenteQuandoADataForMaisAtual() 
        {
            string oidTarefa = Guid.NewGuid().ToString();
            DateTime data1 = DateTime.Now;
            DateTime data2 = DateTime.Now.AddMilliseconds(1);
            DateTime dataMaisAtual = DateTime.Now.AddMilliseconds( 2 );

            //Aplicação da primeira data (mais antiga)
            Assert.IsTrue( gerenciador.AplicarDataAtualizacao( oidTarefa, data1 ),"Deveria aceitar a data pois é a primeira a ser aplicada a tarefa" );
            Assert.IsTrue( gerenciador.TarefasAtualizadas.ContainsKey( oidTarefa ),"Deveria conter a tarefa adicionada" );
            Assert.IsTrue( gerenciador.TarefasAtualizadas[oidTarefa].Equals( data1 ), string.Format( "A data armazenada deveria ser {0}",data1) );

            //aplicação da data mais atual das 3
            Assert.IsTrue( gerenciador.AplicarDataAtualizacao( oidTarefa, dataMaisAtual ), "Deveria aceitar a data pois é mais atual que a primeira data aplicada" );
            Assert.IsTrue( gerenciador.TarefasAtualizadas[oidTarefa].Equals( dataMaisAtual ), string.Format( "A data armazenada deveria ser {0}", dataMaisAtual ) );

            Assert.IsFalse( gerenciador.AplicarDataAtualizacao( oidTarefa, data2 ), "Não deveria aceitar pois uma data mais atual foi aplicada" );
            Assert.IsTrue( gerenciador.TarefasAtualizadas[oidTarefa].Equals( dataMaisAtual ), string.Format( "A data armazenada deveria ser {0} pois é mais atual que {1}", dataMaisAtual, gerenciador.TarefasAtualizadas[oidTarefa] ) );
        }
    }
}
