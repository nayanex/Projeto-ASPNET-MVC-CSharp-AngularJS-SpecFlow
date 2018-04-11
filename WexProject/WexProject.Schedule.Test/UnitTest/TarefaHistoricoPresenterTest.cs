using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Library.Presenters;
using Moq;
using System.Collections;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Schedule.Test.Stubs;
using WexProject.Schedule.Test.Builders;
using WexProject.Schedule.Test.Helpers.Utils;
using WexProject.Schedule.Test.Helpers.ExtensionMethods;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.Planejamento;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Test.Helpers.ExtensionMethods;
using System.Data;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaHistoricoPresenterTest : BaseEntityFrameworkTest
    {
        /*
         * Cenário: Quando o usuário preencher o campo Realizado.
         *          
         * Expectativa: Deverá subtrair a estimativa restante e alterar a situação planejamento para Em Andamento.
         */
        [TestMethod]
        public void DeveAlterarASituacaoParaExecucaoEConsumirAHoraRealizadaDentroDaEstimativaRestanteQuandoAindaRestaremHorasRestantes()
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "0:00" ).Criar();
            CronogramaTarefaDto tarefa02 = new CronogramaTarefaDecoratorBuilder().Descricao( "Teste 01" ).EstimativaInicial( 2).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            // Cenário de Teste.
            //estimando 2 horas como realizado
            view.NbHoraRealizado = "2:00";
            //Sinalizando o presenter de que a view foi alterada
            presenter.HoraRealizadoForAlterado();
            

            //Expectativas
            Guid oidSituacaoPlanejamento = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Execução ) ).Oid;
            Assert.AreEqual( new DateTime( 2013, 08, 14 ), view.DtRealizado.Date, "Deveria ser a mesma data, pois quando o popup é aberto, a data atual é sugerida." );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ).ToString( @"hh\:mm" ), presenter.view.NbHoraInicial, "Deveria ser os mesmos valores, pois o colaborador não realizou nenhuma tarefa ainda no dia." );
            Assert.AreEqual( new TimeSpan( 10, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraFinal, "Deveria ter totalizar (hora inicial + valor realizado) (8:00 + 2:00) = 10:00" );
            Assert.AreEqual( new TimeSpan( 2, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRealizado, "deveria estar exibindo a quantidade de horas que foi estimada." );
            Assert.AreEqual( new TimeSpan( 3, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRestante, "Deveria ter consumido 2:00 da estimativa inicial." );
            Assert.AreEqual( oidSituacaoPlanejamento, view.OidSituacaoPlanejamento, "Deveria ser o oid da situação Em Andamento" );
        }

        [TestMethod]
        public void DeveAlterarASituacaoParaEncerramentoEConsumirTodasHorasRestantesQuandoAHoraDoEsforcoRealizadoUltrapassarAQuantidadeDeHorasRestantes() 
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial(5)
               .EstimativaRestante( "5:00" )
               .Realizado( "0:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            // Cenário de Teste.
            //estimando 2 horas como realizado
            view.NbHoraRealizado = "2:00";
            //Sinalizando o presenter de que a view foi alterada
            presenter.HoraRealizadoForAlterado();


            //Expectativas
            Guid oidSituacaoPlanejamento = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Execução ) ).Oid;
            Assert.AreEqual( new DateTime( 2013, 08, 14 ), view.DtRealizado.Date, "Deveria ser a mesma data, pois quando o popup é aberto, a data atual é sugerida." );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ).ToString( @"hh\:mm" ), presenter.view.NbHoraInicial, "Deveria ser os mesmos valores, pois o colaborador não realizou nenhuma tarefa ainda no dia." );
            Assert.AreEqual( new TimeSpan( 10, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraFinal, "Deveria ter totalizar (hora inicial + valor realizado) (8:00 + 2:00) = 10:00" );
            Assert.AreEqual( new TimeSpan( 2, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRealizado, "deveria estar exibindo a quantidade de horas que foi estimada." );
            Assert.AreEqual( new TimeSpan( 3, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRestante, "Deveria ter consumido 2:00 da estimativa inicial." );
            Assert.AreEqual( oidSituacaoPlanejamento, view.OidSituacaoPlanejamento, "Deveria ser o oid da situação Em Andamento" );
        }

        [TestMethod]
        public void DeveConsumirApenasAsHorasEstimadasQueNaoUltrapassemOLimiteDoDiaGuardandoOExcessoComoRestanteParaOutroDia() 
        {
            /*
             * Cenário: Quando um colaborador estimar um esforço sobre uma determinada tarefa deverá calcular até o limite do dia, e guardar a diferença como horas restantes.
             *  Expectativas:
             *      - A situação deverá ser calculada com Execução (Em Andamento)
             *      - Deverá calcular o esforço realizado limite entre a hora de inicio e o limite do dia [00:00(meia-noite)]
             *      - O Restante deve ser a diferença entre o estimativa hora final - Hora limite
             */
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "0:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 20, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            // Cenário de Teste.
            //estimando 2 horas como realizado
            view.NbHoraRealizado = "5:00";
            //Sinalizando o presenter de que a view foi alterada
            presenter.HoraRealizadoForAlterado();

            //Expectativas
            Guid oidSituacaoPlanejamento = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Execução ) ).Oid;
            TimeSpan horaInicioSugeridaServico = planejamentoServiceStub.RetornoInicializadorEstimativa.HoraInicialEstimativa;
            Assert.AreEqual( new DateTime( 2013, 08, 14 ), view.DtRealizado.Date, "Deveria ser a mesma data, pois quando o popup é aberto, a data atual é sugerida." );
            Assert.AreEqual( horaInicioSugeridaServico.ToString( @"hh\:mm" ), presenter.view.NbHoraInicial, string.Format( "Deveria ser igual ao retorno do inicializador de estimaviva {0}", horaInicioSugeridaServico.ToString( @"hh\:mm" ) ) );
            Assert.AreEqual( new TimeSpan( 1,0, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraFinal, "Deveria ter totalizar (hora inicial + valor realizado) (20:00 + 4:00) = 00:00" );
            Assert.AreEqual( new TimeSpan( 4, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRealizado, "deveria estar exibindo a quantidade de horas que foi estimada." );
            Assert.AreEqual( new TimeSpan( 1, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRestante, "Deveria ter consumido 4:00 da estimativa restantes." );
            Assert.AreEqual( oidSituacaoPlanejamento, view.OidSituacaoPlanejamento, "Deveria ser o oid da situação Em Andamento" );
        }

        /*
         * Cenário: Quando o usuário preencher o campo Realizado ultrapassando a estimativa restante.
         *          
         * Expectativa: Deverá zerar a estimativa restante e marcar a situação planejamento como pronto.
         */
        [TestMethod]
        public void DeveConsumirAsHorasRestantesELancarSituacaoComoEncerramentoQuandoForEstimadoUmEsforcoRealizadoMaiorOuIgualAEstimativaRestante()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 3 )
               .EstimativaRestante( "3:00" )
               .Realizado( "0:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 10, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();
            // Sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "4:00";
            presenter.HoraRealizadoForAlterado();
            SituacaoPlanejamentoDTO situacaoEncerramento = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault(o=>o.CsTipo == CsTipoPlanejamento.Encerramento);
            Assert.AreEqual( TimeSpan.Zero, presenter.HoraRestante, "Deveria ser 0, poiso esforço realizado foi maior do que as horas restantes" );
            Assert.AreEqual( situacaoEncerramento.Oid, view.OidSituacaoPlanejamento,"Devido a não haver mais horas restantes a situação da tarefa deve se encontrar em encerramento" );
        }

        /*
        * Cenário: Quando o usuário abrir o popUp de estimativa e a tarefa não possuir histórico.
        * 
        *          
        * Expectativa: O campo de estimativa restante deve vir preenchido com a estimativa inicial da tarefa, 
        *              a hora inicial do colaborador (campo De) deve vir preenchido com a última hora de trabalho daquele colaborador
        */
        [TestMethod]
        public void DevePreencherAViewComOsDadosQuandoForInicializado()
        {
            /*
             Cenário: Quando abrir a view de TarefaHistoricoView e for exibir os valores retornados do serviço
             * Expectativas
             *  - a view deve iniciar com a data setada no inicializador de estimativas
             *  - a view deve exibir a hora inicial setada no inicializador de estimativa
             *  - a view deve exibir como horas restantes a quantidade de horas restantes estimadas como restante da tarefa
             *  - a view deve exibir situação planejamento atual da tarefa
             */

             // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa(colaborador.Login);
             tarefa = new CronogramaTarefaDecoratorBuilder(tarefa)
                .Descricao( "Tarefa de teste" )
                .AtualizadoPor( colaborador )
                .EstimativaInicial( 7 )
                .EstimativaRestante("7:00")
                .Realizado("0:00").Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            // Método sobre test.
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            //Expectativas
            Assert.AreEqual( new DateTime(2013,08,14), view.DtRealizado.Date, "Deveria ser a mesma data, pois quando o popup é aberto, a data atual é sugerida." );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ).ToString( @"hh\:mm" ), presenter.view.NbHoraInicial, "Deveria ser os mesmos valores, pois o colaborador não realizou nenhuma tarefa ainda no dia." );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraFinal, "Deveria ser os mesmos valores, pois quando inicializar o campo NbAte deveria vir igual a hora de início" );
            Assert.AreEqual( new TimeSpan( 0, 0, 0 ).ToString( @"hh\:mm" ), view.NbHoraRealizado, "A hora realizada deve vir zerada, pois o popup sugeri isto como padrão." );
            Assert.AreEqual( presenter.EstimativaInicial.ToString( @"hh\:mm" ), view.NbHoraRestante, "Deveriam ser os mesmos valores, pois a hora restante deve ser igual a estimativa inicial quando não se há histórico." );
            Assert.AreEqual( situacaoPadrao.Oid, view.OidSituacaoPlanejamento, "Deveria ser o mesmo Oid de Não Iniciado, pois a tarefa não possui histórico ainda" );
        }

        /*
         * Cenário: Quando o usuário alterar o valor para 0 no campo estimativa restante
         * 
         * Expectativa: o campo Situação Planejamento deve ser modificado para pronto
         */
        /*
            Cenário: Quando usuário alterar a estimativa restante para zero
            * Expectativas:
            *  se não houve esforço realizado anteriormente e não há esforço atual mudar situação planejamento para cancelado (Cancelamento)
            *  se houve esforço realizado anteriormente e não há esforço atual, mudar situação planejamento para impedido (Impedimento)
            *  se houve ou não esforço realizado anteriormente e há esforço atual, mudar situação planejamento para pronto (Encerramento)
            */
        [TestMethod]
        public void DeveMudarASituacaoParaEncerramentoQuandoHaEsforcoAtualSendoEstimadoNaoHavendoOuNaoEsforcoRealizadoAnteriormenteEAHoraRestanteEstaSendoZerada() 
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "3:00" )
               .Realizado( "2:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRestante = "00:00";

            // Método sobre test.
            presenter.HoraRestanteForAlterada();

            //Expectativas
            Guid oidSituacaoEncerramento = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Encerramento ) ).Oid;
            Assert.AreEqual( oidSituacaoEncerramento, view.OidSituacaoPlanejamento );
            Assert.AreEqual( 0, presenter.HoraRealizada.Ticks, "Deveria estar zerada pois não foi estimado esforço realizado" );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), presenter.HoraInicial, "A hora inicial deveria ser 8:00hs" );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), presenter.HoraFinal, "A hora final deveria ser 8:00hs, pois não foi estimado nenhum esforço" );
            Assert.AreEqual( "08:00", view.NbHoraInicial, "Deveria apresentar 08:00 na view" );
            Assert.AreEqual( "08:00", view.NbHoraFinal, "Deveria apresentar 08:00 na view" );
            Assert.AreEqual( "00:00", view.NbHoraRealizado, "Deveria apresentar 00:00 na view" );
            Assert.AreEqual( "00:00", view.NbHoraRestante, "Deveria apresentar 00:00 na view" );
        }

        [TestMethod]
        public void DeveMudarASituacaoParaExecucaoQuandoAEstimativaTotalForIgualAoRestanteEHouveEsforcoRealizadoAnteriormente() 
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "01:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            //view.NbHoraRestante = "00:00";

            // Método sobre test.
            presenter.CalcularSituacaoPlanejamento();

            //Expectativas
            Guid oidSituacaoExecucao = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Execução ) ).Oid;
            Assert.AreEqual( oidSituacaoExecucao, view.OidSituacaoPlanejamento );
        }

        [TestMethod]
        public void DeveMudarASituacaoParaPlanejamentoQuandoAEstimativaTotalForIgualAoRestanteENaoHouveEsforcoRealizadoAnteriormente()
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "00:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            //view.NbHoraRestante = "00:00";

            // Método sobre test.
            presenter.CalcularSituacaoPlanejamento();

            //Expectativas
            Guid oidSituacaoPlanejamento = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Planejamento ) ).Oid;
            Assert.AreEqual( oidSituacaoPlanejamento, view.OidSituacaoPlanejamento );
        }

        /*
         * Cenário: Quando o usuário alterar o valor do campo estimativa restante para que seja diferente de 0.
         * 
         * Expectativa: o campo Situação Planejamento deve ser moficiado para a primeria situação do tipo execução
         */
        [TestMethod]
        public void DeveMudarASituacaoParaExecucaoQuandoAindaHouverHorasRestantes()
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "3:00" )
               .Realizado( "2:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "01:00";

            // Método sobre test.
            presenter.HoraRealizadoForAlterado();

            //Expectativas
            Guid oidSituacaoExecucao = planejamentoServiceStub.situacoesPlanejamentoAtivas.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Execução ) ).Oid;
            Assert.AreEqual( oidSituacaoExecucao, view.OidSituacaoPlanejamento );
            Assert.AreEqual( new TimeSpan(1,0,0), presenter.HoraRealizada, "Deveria ser igual ao esforço realizado" );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), presenter.HoraInicial, "A hora inicial deveria ser 8:00hs" );
            Assert.AreEqual( new TimeSpan( 9, 0, 0 ), presenter.HoraFinal, "A hora final deveria ser igual a inicial + o esforço realizado estimado" );
            Assert.AreEqual( "08:00", view.NbHoraInicial, "Deveria apresentar 08:00 na view" );
            Assert.AreEqual( "09:00", view.NbHoraFinal, "Deveria apresentar 09:00 na view" );
            Assert.AreEqual( "01:00", view.NbHoraRealizado, "Deveria apresentar 01:00 na view , pois foi o total estimado como realizado" );
            Assert.AreEqual( "02:00", view.NbHoraRestante, "Deveria apresentar 02:00 na view,pois foi consumido 1:00 como realizada" );

        }

        /*
         * Cenário: Quando o usuário subtrair horas no campo Realizado.
         * 
         * Expectativa: o campo Hora Final deve subtrair horas também.
         */
        [TestMethod]
        public void DeveRecalcularHoraFinalQuandoForemAlteradasAsHorasRealizadas()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 3 )
               .EstimativaRestante( "3:00" )
               .Realizado( "0:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();
            // Sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "2:00";
            presenter.HoraRealizadoForAlterado();
            Assert.AreEqual( new TimeSpan( 10, 0, 0 ), presenter.HoraFinal, "Após realizar 2:00 a hora final calculada deveria ser 10:00" );

            //foi diminuida a hora realizada de 2:00 para 1:00
            presenter.view.NbHoraRealizado = "1:00";
            presenter.HoraRealizadoForAlterado();
            Assert.AreEqual( new TimeSpan( 9, 0, 0 ), presenter.HoraFinal, "Após alterar o realizado para 1:00 a hora final calculada deveria ser 9:00" );
        }

        /*
        * Cenário: Quando o usuário preencher o campo Realizado e o campo 'De' o qual é o horário inicial da tarefa seja alterado e ultrapassar o horário de 12 horas
        *          
        * Expectativa: Deverá zerar a estimativa restante e marcar a situação planejamento como pronto.
        */
        [TestMethod]
        public void DeveCalcularAHoraFinalComBaseNaEstimativaDeEsforcoRealizadoDentroDoHorarioDeUmPeriodoDeTrabalho()
        {
            //Pré condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 3 )
               .EstimativaRestante( "3:00" )
               .Realizado( "0:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            SituacaoPlanejamentoDTO situacaoEncerramento = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().First( o => o.CsTipo == CsTipoPlanejamento.Encerramento );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "3:00";

            // Sistema em teste
            presenter.HoraRealizadoForAlterado();

            //Expectativas
            Assert.AreEqual( new TimeSpan( 11, 0, 0 ), presenter.HoraFinal, "Considerando a hora inicial(8:00) e a quantidade de horas de esforço realizado(3:00)" +
                ", deveria ter encerrado a tarefa 11:00" );
            Assert.AreEqual( "11:00", view.NbHoraFinal, "Considerando a hora inicial(8:00) e a quantidade de horas de esforço realizado(3:00)" +
                ", deveria ter encerrado a tarefa 11:00" );
            Assert.AreEqual( situacaoEncerramento.Oid, view.OidSituacaoPlanejamento, "Deveria ter alterado a situação para encerramento visto que esgotaram-se as horas restantes" );

        }

        [TestMethod]
        public void DeveCalcularAHoraFinalBaseadaNoEsforcoRealizadoConsiderandoAPassagemDeUmPeriodoDeTrabalhoConsiderandoOsIntervalos()
        {
            //Pré condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 3 )
               .EstimativaRestante( "3:00" )
               .Realizado( "0:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            SituacaoPlanejamentoDTO situacaoEncerramento = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().First( o => o.CsTipo == CsTipoPlanejamento.Encerramento );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "5:00";

            // Sistema em teste
            presenter.HoraRealizadoForAlterado();

            //Expectativas
            Assert.AreEqual( new TimeSpan( 14, 0, 0 ), presenter.HoraFinal, "Considerando a hora inicial(8:00) e a quantidade de horas de esforço realizado(5:00)" +
                ", deveria ter encerrado a tarefa 14:00 não contando com o intervalo entre os periodos 12:00 - 13:00" );
            Assert.AreEqual( "14:00", view.NbHoraFinal, "Considerando a hora inicial(8:00) e a quantidade de horas de esforço realizado(5:00)" +
                ", deveria ter encerrado a tarefa 14:00 não contando com o intervalo entre os periodos 12:00 - 13:00" );
            Assert.AreEqual( situacaoEncerramento.Oid, view.OidSituacaoPlanejamento, "Deveria ter alterado a situação para encerramento visto que esgotaram-se as horas restantes" );
        }

        [TestMethod]
        public void DeveCalcularAHoraFinalBaseadoNoEsforcoRealizadoConsiderandoOLimiteDaPassagemDeUmDiaParaOOutroEstimandoSomenteOTotalQueNaoUltrapasseOLimite()
        {
            //Pré condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 3 )
               .EstimativaRestante( "3:00" )
               .Realizado( "0:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 21, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            SituacaoPlanejamentoDTO situacaoExecucao = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().First( o => o.CsTipo == CsTipoPlanejamento.Execução );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "5:00";

            // Sistema em teste
            presenter.HoraRealizadoForAlterado();

            //Expectativas
            Assert.AreEqual( new TimeSpan( 1, 0, 0, 0 ), presenter.HoraFinal, "Considerando a hora inicial(21:00) e a quantidade de horas de esforço realizado(5:00)" +
                "o máximo aceitado para o dia deveria ser 3:00, totalizando um dia completo 00:00:00(Do proximo dia)" );
            Assert.AreEqual( "00:00", view.NbHoraFinal, "Considerando a hora inicial(21:00) e a quantidade de horas de esforço realizado(5:00)" +
                "o máximo aceitado para o dia deveria ser 3:00, totalizando um dia completo 00:00:00(Do proximo dia)" );
            Assert.AreEqual( situacaoExecucao.Oid, view.OidSituacaoPlanejamento, "Deveria ter alterado a situação para execução, visto que se esgotou o limite do dia, preservando 2:00 para o próximo dia" );
            Assert.AreEqual( new TimeSpan( 2, 0, 0 ), presenter.HoraRestante,"Deveria ainda possuir 2:00 pois esgotou-se o limite do dia e deve ser armazenado o resto no dia restante" );
            Assert.AreEqual( "02:00", view.NbHoraRestante, "Deve ser visualizado as horas que restaram para o próximo dia" );
        }

        [TestMethod]
        public void DeveRecalcularHoraFinalQuandoAHoraInicialForAlterada()
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "00:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 7, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "5:00";
            view.NbHoraRestante = "0:00";
            presenter.AtualizarValoresAtributos();

            //Exercício
            presenter.HoraFinalForAlterada();
            //Expectativa
            Assert.AreEqual( new TimeSpan( 12, 0, 0 ), presenter.HoraFinal, "Considerando a hora inicial de 7:00 , deveria ter sido recalculado a hora final para 12:00" );


            //Exercício 2
            view.NbHoraInicial = "9:00";
            presenter.HoraFinalForAlterada();

            //Expectativa 2
            Assert.AreEqual( new TimeSpan( 15, 0, 0 ), presenter.HoraFinal, "Considerando a hora inicial de 9:00 , deveria ter sido recalculado a hora final para 15:00 considerando o intervalo de almoço" );
        }

        #region Testes de expectativas sobre a ativação do campo de justificativa de redução

        /*
             * Expectativa: Quando o campo Hora Restante for alterado, deve ser verificado se houve redução na quantidade de horas atualmente estimadas
             * Caso tenha ocorrido a redução que não seja por meio da estimação de horas realizadas, deve-se informar a justificativa da redução
             * 
             * Cenário:
             * Usuário alterar manualmente o campo de horas restantes e reduzir o valor.
             */
        [TestMethod]
        public void NaoDeveAtivarJustificativaQuandoNaoHouverReducaoDeHorasRestantes()
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "00:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 7, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            presenter.HoraRestanteForAlterada();
            Assert.IsFalse( presenter.JustificativaReducaoAtiva, "Não deveria ativar a justificativa de redução pois não houve redução" );
        }

        [TestMethod]
        public void NaoDeveAtivarOCampoJustificativaDeReducaoQuandoForReduzidaAHoraRestanteMasFoiEstimadoHoraRealizada()
        {
           // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "00:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 7, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "01:00";
            view.NbHoraRestante = "00:00";
            presenter.HoraRestanteForAlterada();
            Assert.IsFalse( presenter.JustificativaReducaoAtiva, "Não deveria ativar a justificativa de redução pois foi estimado um esforço realizado" );
        }

        [TestMethod]
        public void DeveAtivarOCampoJustificativaDeReducaoQuandoForReduzidaAHoraRestanteMasNaoFoiEstimadoHoraRealizada()
        {
            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 5 )
               .EstimativaRestante( "5:00" )
               .Realizado( "00:00" ).Criar();
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Wednesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 7, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );

            //Cenário
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRestante = "04:00";
            presenter.HoraRestanteForAlterada();
            Assert.IsTrue( presenter.JustificativaReducaoAtiva, "Deveria ter ativado a justificativa de redução quando for alteradas as horas restantes e não foi estimado realização" );
        } 
        #endregion

        [TestMethod]
        public void DeveHabilitarAJustificativaRestanteQuandoHouverReducaoNaQuantidadeDeHorasEstimada()
        {
            /*
             * Expectativa: Quando o campo Hora Restante for alterado, deve ser verificado se houve redução na quantidade de horas atualmente estimadas
             * Caso tenha ocorrido a redução que não seja por meio da estimação de horas realizadas, deve-se informar a justificativa da redução
             * 
             * Cenário:
             * Usuário alterar manualmente o campo de horas restantes e reduzir o valor.
             */

            // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 7 )
               .EstimativaRestante( "7:00" )
               .Realizado( "0:00" ).Criar();
            DiaTrabalhoDto diaTrabalho = new DiaTrabalhoDto().DiaSemana(DayOfWeek.Tuesday).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 27 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = diaTrabalho };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            view.NbHoraRestante = "00:00";
            presenter.HoraRestanteForAlterada();
            Assert.IsTrue(presenter.JustificativaReducaoAtiva,"Deveria ter sido ativada pois houve redução de horas restantes sem estimativa de realização");
        }

        [TestMethod]
        public void AlterarSituacaoPlanejamentoParaTipoExecucaoQuandoNaoRealizarNenhumaHoraNaTarefaEHistoricoDaTarefaNaoPossuirHorasRealizadasTest()
        {
           PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 7 )
               .EstimativaRestante( "7:00" )
               .Realizado( "00:00" ).Criar();
            DiaTrabalhoDto diaTrabalho = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Tuesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 27 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = diaTrabalho };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();

            //não realiza nenhuma hora, mas altera a estimativa restante.
            ITarefaHistoricoView view = new TarefaHistoricoViewMock();
            Mock<TarefaHistoricoPresenter> historicoMock = new Mock<TarefaHistoricoPresenter>( view ) { CallBase = true };
            historicoMock.Setup( o => o.GetTarefaHistoricoTrabalhoAtual( It.IsAny<Guid>() ) ).Returns( planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho   );
            historicoMock.Setup( o => o.GetSemanaTrabalhoDto() ).Returns( CriarSemanaTrabalhoDtoPadrao() );

            TarefaHistoricoPresenter presenter = historicoMock.Object;

            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            presenter.InicializarHistoricoTarefa( tarefa, planejamentoServiceStub.colaboradorLogado.Login);

            view.NbHoraRealizado = "00:00";

            SituacaoPlanejamentoDTO situacaoExecucao = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Execução );

            view.OidSituacaoPlanejamento = situacaoExecucao.Oid;
            
            presenter.AtualizarValoresAtributos();

            presenter.AlterarSituacaoPlanejamento();

            Assert.AreEqual( situacaoExecucao.Oid, view.OidSituacaoPlanejamento, "Deveria ser o mesmo Oid de situação planejamento, pois quando o método AlterarSituacaoPlanejamento validar," +
                                                                                       "automáticamente o Oid da situação do tipo Execução seja atribuido" );

            Assert.AreEqual( situacaoExecucao.Oid, view.OidSituacaoPlanejamento, "Deveria ser o mesmo Oid de situação planejamento, pois a situação planejamento do tipo Execução deve ser ignorada, pois" +
                                                                                    "NbRealizado é Zero e nenhuma hora foi realizada nos históricos daquela tarefa." );

        }

        /// <summary>
        /// Cenário: Quando usuário alterar o campo Situação Planejamento para o tipo execução quando o campo nbHoraRestante tiver o valor zerado
        /// Expectativa: A situação planejamento que o usuário tentou atribuir deve ser ignorada e a situação que estava anteriormente deverá retornar.
        /// </summary>
        [TestMethod]
        public void DeveRetornarSituacaoAoEstadoDeEncerramentoQuandoForemZeradasAsHorasRestantesEEstimadoHorasRealizadas()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 7 )
               .EstimativaRestante( "5:00" )
               .Realizado( "2:00" ).Criar();
            DiaTrabalhoDto diaTrabalho = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Tuesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 27 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = diaTrabalho };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // System Under Test
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            Guid oidSituacaoExecucao = presenter.GetOidSituacaoPlanejamentoTipoExecucao();
            Guid oidSituacaoEncerramento = presenter.GetOidSituacaoPlanejamentoTipoEncerramento();
            
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "3:00";
            presenter.HoraRealizadoForAlterado();
            view.NbHoraRestante = "00:00";
            presenter.HoraRestanteForAlterada();
            Assert.AreEqual( oidSituacaoEncerramento, view.OidSituacaoPlanejamento,"Deveria ter ido para encerramento após a estimativa" );
            view.OidSituacaoPlanejamento = oidSituacaoExecucao;
            presenter.AlterarSituacaoPlanejamento();

            Assert.AreEqual( oidSituacaoEncerramento, view.OidSituacaoPlanejamento, "O valor da view deveria ter retornado ao valor do oid da situação de encerramento, pois não há horas restantes na tarefa para serem trabalhadas" );
        }

        /// <summary>
        /// Cenário: Quando usuário alterar o campo Situação Planejamento para o tipo encerramento quando o campo nbHoraRestante tiver o valor maior que zero.
        /// Expectativa: A situação planejamento que o usuário tentou atribuir do tipo Encerramento deve ser ignorada e a situação que estava anteriormente deverá retornar, pois a hora restante é maior que zero.
        /// </summary>
   

        /// <summary>
        /// método para auxiliar em testes unitários criando uma semana de trabalho padrão
        /// </summary>
        /// <returns>semana de trabalho dto padrão</returns>
        public static SemanaTrabalhoDto CriarSemanaTrabalhoDtoPadrao()
        {
            SemanaTrabalho semana = new SemanaTrabalho();
            SemanaTrabalhoDao.SelecionarSemanaTrabalhoPadrao( semana );
            SemanaTrabalhoDto semanaDto = SemanaTrabalhoBo.DtoFactory( semana );
            return semanaDto;
        }

        /// <summary>
        /// Cenário: Quando usuário alterar o campo Situação Planejamento para o tipo encerramento quando o campo nbHoraRestante tiver o valor zerado, 
        /// mas não possuir em nenhum histórico horas realizadas para aquela atividade.
        /// Expectativa: A situação planejamento que o usuário tentou atribuir do tipo Encerramento deve ser ignorada e a situação que estava anteriormente 
        /// deverá retornar, pois não foi dedicado nenhuma hora para aquela tarefa em todos os históricos, ela então deveria ser cancelada e não atribuida como pronta.
        /// </summary>
        [TestMethod]
        public void AlterarSituacaoPlanejamentoParaTipoEncerramentoQuandoHoraRestanteIgualA0ETarefaNaoPossuirHorasRealizadasNoHistoricoTest()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDTO situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDecoratorBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( 7 )
               .EstimativaRestante( "5:00" )
               .Realizado( "2:00" ).Criar();
            DiaTrabalhoDto diaTrabalho = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Tuesday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 27 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = diaTrabalho };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();


            Mock<TarefaHistoricoPresenter> historicoMock = new Mock<TarefaHistoricoPresenter>( view ) { CallBase = true };
            historicoMock.Setup( o => o.GetSemanaTrabalhoDto() ).Returns( CriarSemanaTrabalhoDtoPadrao() );

            TarefaHistoricoPresenter presenter = historicoMock.Object;

            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultartSituacoesPlanejamento() );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            view.NbHoraRestante = "00:00";
            SituacaoPlanejamentoDTO situacaoEncerramento = planejamentoServiceStub.ConsultartSituacoesPlanejamento().OndeOTipoFor( CsTipoPlanejamento.Encerramento );
            view.OidSituacaoPlanejamento = situacaoEncerramento.Oid;
            presenter.AtualizarValoresAtributos();

            presenter.AlterarSituacaoPlanejamento();

            Assert.AreEqual( situacaoEncerramento.Oid, view.OidSituacaoPlanejamento, "Deveria ser o mesmo Oid de situação planejamento, pois o situação" +
                                                                                    "do tipo Encerramento não pode ser atribuida quando nenhuma hora tenha" +
                                                                                    "sido realizada naquela tarefa em todos os históricos já gerados para aquela tarefa." );
        }

        /*
         Modelo: Inicialização

         // Pre-condições.
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            SituacaoPlanejamentoDto situacaoPadrao = planejamentoServiceStub.situacaoPlanejamentoPadrao;
            CronogramaTarefaDto tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            tarefa = new CronogramaTarefaDtoDataBuilder( tarefa )
               .Descricao( "Tarefa de teste" )
               .AtualizadoPor( colaborador )
               .EstimativaInicial( "5:00" )
               .EstimativaRestante( "5:00" )
               .Realizado( "0:00" ).Criar();
            CronogramaTarefaDto tarefa02 = new CronogramaTarefaDtoDataBuilder().Descricao( "Teste 01" ).EstimativaInicial( "2:00" ).Criar();
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ) };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view,
                planejamentoServiceStub, geralServiceStub );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login ); 
         */
        [TestMethod]
        public void DevePermitirSituacaoDeExecucaoQuandoHouverHorasRestantes() 
        {
            //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );
            
            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "00:00" ).EstimativaRestante( "2:00" ).EstimativaInicial( 1 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            //Pré condições ocorridas na view (Simulação da situação real)
            SituacaoPlanejamentoDTO situacao = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Execução );
            view.OidSituacaoPlanejamento = situacao.Oid;

            //Exercicio do teste
            presenter.AlterarSituacaoPlanejamento();

            //Expectativas
            Assert.AreEqual( view.OidSituacaoPlanejamento, situacao.Oid,"Deveria ter sido alterada para a situação de execução" );
        }

        [TestMethod]
        public void NaoDevePermitirSituacaoDeExecucaoQuandoNaoHouverHorasRestantes()
        {
            //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );

            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "00:00" ).EstimativaRestante( "2:00" ).EstimativaInicial( 2 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            //Pré condições ocorridas na view (Simulação da situação real)
            SituacaoPlanejamentoDTO situacao = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Execução );
            view.NbHoraRestante = "00:00";
            presenter.HoraRestanteForAlterada();
            Guid oidSituacaoAnterior = view.OidSituacaoPlanejamento;
            view.OidSituacaoPlanejamento = situacao.Oid;

            //Exercicio do teste
            presenter.AlterarSituacaoPlanejamento();

            //Expectativas
            SituacaoPlanejamentoDTO situacaoPlanejamento = planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento );
            Assert.AreEqual( view.OidSituacaoPlanejamento, situacaoPlanejamento.Oid, "Deveria ter sido alterada para a situação de planejamento visto que não possuia horas realizadas" );
        }

        [TestMethod]
        public void DeveValidarASituacaoPlanejamentoDoTipoPlanejamentoQuandoNaoPossuirEsforcoRealizado() 
        {
             //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );

            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "00:00" ).EstimativaRestante( "2:00" ).EstimativaInicial( 2 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            Assert.IsTrue( presenter.ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ), "Deveria ter validado pois não existe esforço realizado" );
        }

        [TestMethod]
        public void DeveValidarASituacaoPlanejamentoDoTipoPlanejamentoQuandoForDiminuidaHoraRestanteENaoPossuirEsforcoRealizado()
        {
            //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );

            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "00:00" ).EstimativaRestante( "2:00" ).EstimativaInicial( 2 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            Assert.IsTrue( presenter.ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ),"Deveria ter validado pois não existe esforço realizado" );
        }

        [TestMethod]
        public void NaoDeveValidarASituacaoPlanejamentoDoTipoPlanejamentoQuandoHouverEstimativaDeEsforcoRealizadoAnteriormente()
        {
            //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );

            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "02:00" ).EstimativaRestante( "3:00" ).EstimativaInicial( 5 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );

            Assert.IsFalse( presenter.ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ),"Não deveria validar pois já houve uma estimativa de 02:00 na tarefa");
        }

        [TestMethod]
        public void NaoDeveValidarASituacaoPlanejamentoDoTipoPlanejamentoQuandoHouverEstimativaDeEsforcoRealizadoAtualmente()
        {
            //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );

            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "00:00" ).EstimativaRestante( "5:00" ).EstimativaInicial( 5 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "1:00";
            presenter.HoraRealizadoForAlterado();
            Assert.IsFalse( presenter.ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ),"Não pode ser válido pois existe o esforço de 1:00 como realizado atualmente" );
        }

        [TestMethod]
        public void DeveValidarASituacaoPlanejamentoDoTipoEncerramentoQuandoHouverEsforcoRealizadoENaoHouverHorasRestantes()
        {
            //Pré Condições cénario externo
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            ColaboradorDto colaborador = planejamentoServiceStub.colaboradorLogado;
            CronogramaTarefaDecorator tarefa = planejamentoServiceStub.CriarTarefa( colaborador.Login );
            CronogramaTarefaDecoratorBuilder builderTarefa = new CronogramaTarefaDecoratorBuilder( tarefa );

            DiaTrabalhoDto dia = new DiaTrabalhoDto().DiaSemana( DayOfWeek.Friday ).AdicionarPeriodo( "8:00", "12:00" ).AdicionarPeriodo( "13:00", "18:00" );
            planejamentoServiceStub.RetornoInicializadorEstimativa = new InicializadorEstimativaDto() { DataEstimativa = new DateTime( 2013, 08, 14 ), HoraInicialEstimativa = new TimeSpan( 8, 0, 0 ), DiaAtual = dia };
            planejamentoServiceStub.RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            TarefaHistoricoViewMock view = new TarefaHistoricoViewMock();

            // Pré condições sistema em teste
            TarefaHistoricoPresenter presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.situacoesPlanejamentoAtivas );

            //Tarefa sem nenhum esforço realizado, com estimativa de 2:00 para termino
            tarefa = builderTarefa.Realizado( "00:00" ).EstimativaRestante( "5:00" ).EstimativaInicial( 5 ).Criar();
            presenter.InicializarHistoricoTarefa( tarefa, colaborador.Login );
            view.NbHoraRealizado = "5:00";
            presenter.HoraRealizadoForAlterado();
            Assert.IsFalse( presenter.ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ), "Não pode ser válido pois existe o esforço de 1:00 como realizado atualmente" );
        }
    }
}
