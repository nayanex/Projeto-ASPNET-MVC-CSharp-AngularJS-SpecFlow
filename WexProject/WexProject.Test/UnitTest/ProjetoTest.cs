using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Models.Execucao;
using System.Collections.Generic;
using WexProject.Library.Libs.Xaf;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.BLL.Shared.Domains.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe ProjetoTest
    /// </summary>
    [TestClass]
    public class ProjetoTest : BaseTest
    {
        /// <summary>
        /// método SalvarProjetoComDtTerminoMaiorQueDtInicio
        /// </summary>
        [TestMethod]
        public void SalvarProjetoComDtTerminoMaiorQueDtInicio()
        {
            /**
             * Cenário 1: Será criado um projeto A. Serão setadas as datas de inicio e fim do projeto.
             * A data final não pode ser menor que a data inicial.
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 300, "", true);
            projetoA.DtInicioPlan = new DateTime(2011, 02, 25);
            projetoA.DtTerminoPlan = new DateTime(2011, 02, 26);
            projetoA.Save();

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(projetoA,
            "NaoPermitirSalvarDtInicioMenorDtTermino", DefaultContexts.Save));
        }
        /// <summary>
        /// método SalvarProjetoComNumeroDeCiclosMenorQue2
        /// </summary>
        [TestMethod]
        public void SalvarProjetoComNumeroDeCiclosMenorQue2()
        {
            /**
             * Cenário 2: Será criado um projeto A. Será informado que a quantidade de ciclos do projeto será de 1.
             * O sistema deverá retornar inválida a tentativa de salvar o projeto
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 300, "", true);

            projetoA.NbCicloTotalPlan = 0;
            projetoA.Save();

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(projetoA,
            "ValidarNumeroCiclos", DefaultContexts.Save));
        }
        /// <summary>
        /// método SalvarProjetoComDuracaoDeCiclosMenorQue10
        /// </summary>
        [TestMethod]
        public void SalvarProjetoComDuracaoDeCiclosMenorQue10()
        {
            /**
             * Cenário 3: Será criado um projeto A. Será informado que a duração de um ciclo (9).
             * O sistema deverá retornar inválida a tentativa de salvar o projeto
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 300, "", true);

            projetoA.NbCicloDuracaoDiasPlan = 0;
            projetoA.Save();

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(projetoA,
            "ValidarDuracaoCiclos", DefaultContexts.Save));
        }
        /// <summary>
        /// método TestarAlterarAsConfiguracoesDoCicloDeDesenvolvimentoDeUmProjeto
        /// </summary>
        [TestMethod]
        public void TestarAlterarAsConfiguracoesDoCicloDeDesenvolvimentoDeUmProjeto()
        {
            /**
             * Cenário 4: Será criado um projeto com uma parte interessada
             * Comentários a seguir
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 100, "", false);

            //Passo 1

            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.NbCicloTotalPlan = 5;
            projetoA.NbCicloDuracaoDiasPlan = 10;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 07, 07), projetoA._DtTerminoReal, "A data de termino deveria ser 07/07/2011");

            //Passo 2

            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.NbCicloTotalPlan = 6;
            projetoA.NbCicloDuracaoDiasPlan = 10;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 07, 22), projetoA._DtTerminoReal, "A data de termino deveria ser 22/07/2011");

            //Passo 3

            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.NbCicloTotalPlan = 6;
            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 10, 14), projetoA._DtTerminoReal, "A data de termino deveria ser 14/10/2011");

            //Passo 4

            Calendario calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Trabalho, 01, CsMesDomain.Maio, 2011, true);
            //projetoA._AlteradoCiclo = true;
            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.NbCicloTotalPlan = 6;
            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.NbCicloDiasIntervalo = 1;

            projetoA.Save();


            Assert.AreEqual(new DateTime(2011, 10, 13), projetoA._DtTerminoReal, "A data de termino deveria ser 13/10/2011");

            //Passo 5

            calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Folga, 02, CsMesDomain.Maio, 2011, true);

            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.NbCicloTotalPlan = 6;
            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.NbCicloDiasIntervalo = 1;
            //projetoA._AlteradoCiclo = true;
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 10, 14), projetoA._DtTerminoReal, "A data de termino deveria ser 14/10/2011 ");

            //Passo 6
            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.NbCicloTotalPlan = 4;
            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 08, 17), projetoA._DtTerminoReal, "A data de termino deveria ser 18/18/2011");
        }
        /// <summary>
        /// método TestarAlterarAsConfiguracoesDoCicloDeDesenvolvimentoDeUmProjetoSemInicioRealInformado
        /// </summary>
        [TestMethod]
        public void TestarAlterarAsConfiguracoesDoCicloDeDesenvolvimentoDeUmProjetoSemInicioRealInformado()
        {
            /**
             * Cenário 5: Será criado um projeto A.
             * Não serão informados o começo real do projeto e nem o término planejado.
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 100, "", false);

            //Passo 1 - O DtTermino deverá ser vazio

            projetoA.DtInicioPlan = new DateTime(2011, 04, 25);
            projetoA.DtInicioReal = DateTime.MinValue;
            projetoA.DtTerminoPlan = DateTime.MinValue;
            projetoA.NbCicloTotalPlan = 5;
            projetoA.NbCicloDuracaoDiasPlan = 10;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(new DateTime(), projetoA._DtTerminoReal, "A data deve ser nula");

            //Passo 2 - O se o DtInicioReal estiver vazio e o DtTerminoPlan não, então o DtTerminoReal recebe DtTerminoPlan

            projetoA.DtInicioPlan = new DateTime(2011, 04, 25);
            projetoA.DtInicioReal = DateTime.MinValue;
            projetoA.DtTerminoPlan = new DateTime(2011, 06, 20);
            projetoA.NbCicloTotalPlan = 5;
            projetoA.NbCicloDuracaoDiasPlan = 10;
            projetoA.NbCicloDiasIntervalo = 1;
            // projetoA._AlteradoCiclo = true;
            projetoA.Save();

            Assert.AreEqual(projetoA.DtTerminoPlan, projetoA._DtTerminoReal, "A data do término real deve ser a mesma data do término planejado");

            //Passo 3 - Será alterado o número de ciclos planejados, porém, o valor de DtTerminoReal deverá permanecer o mesmo do término planejado

            projetoA.DtInicioPlan = new DateTime(2011, 04, 25);
            projetoA.DtInicioReal = DateTime.MinValue;
            projetoA.DtTerminoPlan = new DateTime(2011, 06, 20);
            projetoA.NbCicloTotalPlan = 5;
            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(projetoA.DtTerminoPlan, projetoA._DtTerminoReal, "A data do término real deve ser a mesma data do término planejado");

            //Passo 4 - Será informado um valor para DtInicioReal e o sistema deverá calcular corretamente o valor do término real

            projetoA.DtInicioPlan = new DateTime(2011, 04, 25);
            projetoA.DtInicioReal = new DateTime(2011, 05, 25);
            projetoA.DtTerminoPlan = new DateTime(2011, 06, 20);
            projetoA.NbCicloTotalPlan = 5;
            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.NbCicloDiasIntervalo = 1;
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 10, 17), projetoA._DtTerminoReal, "A data do término real deve ser 17/10/2011");
        }
        /// <summary>
        /// método TestarSalvarUmProjetoParaCriarOuAtualizarOsCiclosDeDesenvolvimento
        /// </summary>
        [TestMethod]
        public void TestarSalvarUmProjetoParaCriarOuAtualizarOsCiclosDeDesenvolvimento()
        {
            /**
             * Cenário 6: Será criado um projetoA.
             * Serão criados ciclos automaticamente quando os ciclos do projeto forem calculados
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 30, "", false);
            Modulo moduloA = ModuloFactory.Criar(SessionTest, projetoA, "", false);
            Beneficiado beneficiadoA = BeneficiadoFactory.Criar(SessionTest, "", true);
            Estoria estoriaA = EstoriaFactory.Criar(SessionTest, moduloA, "", "", "", beneficiadoA, "", "", "", true);
            // CicloDesenv ciclo01 = CicloFactory.Criar(SessionTest, projetoA, "", false);

            //Passo 1

            projetoA.DtInicioPlan = new DateTime(2011, 04, 25);
            projetoA.DtInicioReal = new DateTime(2011, 04, 25);
            projetoA.DtTerminoPlan = new DateTime(2011, 05, 31);
            projetoA.NbCicloTotalPlan = 5;
            projetoA.NbCicloDuracaoDiasPlan = 10;
            projetoA.NbCicloDiasIntervalo = 1;

            projetoA.Save();

            Assert.AreEqual(1, projetoA.Ciclos[0].NbCiclo, "O ciclo deveria ser 1");
            Assert.AreEqual(new DateTime(2011, 04, 25), projetoA.Ciclos[0].DtInicio, "O valor de inicio deveria ser 25/04/2011");
            Assert.AreEqual(new DateTime(2011, 05, 06), projetoA.Ciclos[0].DtTermino, "O valor de término deveria ser 06/05/2011");

            Assert.AreEqual(2, projetoA.Ciclos[1].NbCiclo, "O ciclo deveria ser 2");
            Assert.AreEqual(new DateTime(2011, 05, 10), projetoA.Ciclos[1].DtInicio, "O valor de inicio deveria ser 10/05/2011");
            Assert.AreEqual(new DateTime(2011, 05, 23), projetoA.Ciclos[1].DtTermino, "O valor de término deveria ser 23/05/2011");

            Assert.AreEqual(3, projetoA.Ciclos[2].NbCiclo, "O ciclo deveria ser 3");
            Assert.AreEqual(new DateTime(2011, 05, 25), projetoA.Ciclos[2].DtInicio, "O valor de inicio deveria ser 25/05/2011");
            Assert.AreEqual(new DateTime(2011, 06, 07), projetoA.Ciclos[2].DtTermino, "O valor de término deveria ser 07/06/2011");

            Assert.AreEqual(4, projetoA.Ciclos[3].NbCiclo, "O ciclo deveria ser 4");
            Assert.AreEqual(new DateTime(2011, 06, 09), projetoA.Ciclos[3].DtInicio, "O valor de inicio deveria ser 09/06/2011");
            Assert.AreEqual(new DateTime(2011, 06, 22), projetoA.Ciclos[3].DtTermino, "O valor de término deveria ser 22/06/2011");

            Assert.AreEqual(5, projetoA.Ciclos[4].NbCiclo, "O ciclo deveria ser 5");
            Assert.AreEqual(new DateTime(2011, 06, 24), projetoA.Ciclos[4].DtInicio, "O valor de inicio deveria ser 24/06/2011");
            Assert.AreEqual(new DateTime(2011, 07, 07), projetoA.Ciclos[4].DtTermino, "O valor de término deveria ser 07/07/2011");

            //Passo 2

            projetoA.NbCicloTotalPlan = 6;
            projetoA.Save();

            Assert.AreEqual(6, projetoA.Ciclos[5].NbCiclo, "O ciclo deveria ser 6");
            Assert.AreEqual(new DateTime(2011, 07, 11), projetoA.Ciclos[5].DtInicio, "O valor de inicio deveria ser 11/07/2011");
            Assert.AreEqual(new DateTime(2011, 07, 22), projetoA.Ciclos[5].DtTermino, "O valor de término deveria ser 22/07/2011");


            //Passo 3

            projetoA.NbCicloDuracaoDiasPlan = 20;
            projetoA.Save();

            Assert.AreEqual(1, projetoA.Ciclos[0].NbCiclo, "O ciclo deveria ser 1");
            Assert.AreEqual(new DateTime(2011, 04, 25), projetoA.Ciclos[0].DtInicio, "O valor de inicio deveria ser 25/04/2011");
            Assert.AreEqual(new DateTime(2011, 05, 20), projetoA.Ciclos[0].DtTermino, "O valor de término deveria ser 20/05/2011");

            Assert.AreEqual(2, projetoA.Ciclos[1].NbCiclo, "O ciclo deveria ser 2");
            Assert.AreEqual(new DateTime(2011, 05, 24), projetoA.Ciclos[1].DtInicio, "O valor de inicio deveria ser 24/05/2011");
            Assert.AreEqual(new DateTime(2011, 06, 20), projetoA.Ciclos[1].DtTermino, "O valor de término deveria ser 20/06/2011");

            Assert.AreEqual(3, projetoA.Ciclos[2].NbCiclo, "O ciclo deveria ser 3");
            Assert.AreEqual(new DateTime(2011, 06, 22), projetoA.Ciclos[2].DtInicio, "O valor de inicio deveria ser 22/06/2011");
            Assert.AreEqual(new DateTime(2011, 07, 19), projetoA.Ciclos[2].DtTermino, "O valor de término deveria ser 19/07/2011");

            Assert.AreEqual(4, projetoA.Ciclos[3].NbCiclo, "O ciclo deveria ser 4");
            Assert.AreEqual(new DateTime(2011, 07, 21), projetoA.Ciclos[3].DtInicio, "O valor de inicio deveria ser 21/07/2011");
            Assert.AreEqual(new DateTime(2011, 08, 17), projetoA.Ciclos[3].DtTermino, "O valor de término deveria ser 17/08/2011");

            Assert.AreEqual(5, projetoA.Ciclos[4].NbCiclo, "O ciclo deveria ser 5");
            Assert.AreEqual(new DateTime(2011, 08, 19), projetoA.Ciclos[4].DtInicio, "O valor de inicio deveria ser 19/08/2011");
            Assert.AreEqual(new DateTime(2011, 09, 15), projetoA.Ciclos[4].DtTermino, "O valor de término deveria ser 15/09/2011");

            Assert.AreEqual(6, projetoA.Ciclos[5].NbCiclo, "O ciclo deveria ser 6");
            Assert.AreEqual(new DateTime(2011, 09, 19), projetoA.Ciclos[5].DtInicio, "O valor de inicio deveria ser 19/09/2011");
            Assert.AreEqual(new DateTime(2011, 10, 14), projetoA.Ciclos[5].DtTermino, "O valor de término deveria ser 14/10/2011");

            //Passo 4

            Calendario calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Trabalho, 01, CsMesDomain.Maio, 2011, true);
            //projetoA._AlteradoCiclo = true;
            projetoA.Save();

            Assert.AreEqual(1, projetoA.Ciclos[0].NbCiclo, "O ciclo deveria ser 1");
            Assert.AreEqual(new DateTime(2011, 04, 25), projetoA.Ciclos[0].DtInicio, "O valor de inicio deveria ser 25/04/2011");
            Assert.AreEqual(new DateTime(2011, 05, 19), projetoA.Ciclos[0].DtTermino, "O valor de término deveria ser 19/05/2011");

            Assert.AreEqual(2, projetoA.Ciclos[1].NbCiclo, "O ciclo deveria ser 2");
            Assert.AreEqual(new DateTime(2011, 05, 23), projetoA.Ciclos[1].DtInicio, "O valor de inicio deveria ser 23/05/2011");
            Assert.AreEqual(new DateTime(2011, 06, 17), projetoA.Ciclos[1].DtTermino, "O valor de término deveria ser 17/06/2011");

            Assert.AreEqual(3, projetoA.Ciclos[2].NbCiclo, "O ciclo deveria ser 3");
            Assert.AreEqual(new DateTime(2011, 06, 21), projetoA.Ciclos[2].DtInicio, "O valor de inicio deveria ser 21/06/2011");
            Assert.AreEqual(new DateTime(2011, 07, 18), projetoA.Ciclos[2].DtTermino, "O valor de término deveria ser 18/07/2011");

            Assert.AreEqual(4, projetoA.Ciclos[3].NbCiclo, "O ciclo deveria ser 4");
            Assert.AreEqual(new DateTime(2011, 07, 20), projetoA.Ciclos[3].DtInicio, "O valor de inicio deveria ser 20/07/2011");
            Assert.AreEqual(new DateTime(2011, 08, 16), projetoA.Ciclos[3].DtTermino, "O valor de término deveria ser 16/08/2011");

            Assert.AreEqual(5, projetoA.Ciclos[4].NbCiclo, "O ciclo deveria ser 5");
            Assert.AreEqual(new DateTime(2011, 08, 18), projetoA.Ciclos[4].DtInicio, "O valor de inicio deveria ser 18/08/2011");
            Assert.AreEqual(new DateTime(2011, 09, 14), projetoA.Ciclos[4].DtTermino, "O valor de término deveria ser 14/09/2011");

            Assert.AreEqual(6, projetoA.Ciclos[5].NbCiclo, "O ciclo deveria ser 6");
            Assert.AreEqual(new DateTime(2011, 09, 16), projetoA.Ciclos[5].DtInicio, "O valor de inicio deveria ser 19/09/2011");
            Assert.AreEqual(new DateTime(2011, 10, 13), projetoA.Ciclos[5].DtTermino, "O valor de término deveria ser 14/10/2011");

            //Passo 5

            calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Folga, 02, CsMesDomain.Maio, 2011, true);
            //projetoA._AlteradoCiclo = true;
            projetoA.Save();

            Assert.AreEqual(1, projetoA.Ciclos[0].NbCiclo, "O ciclo deveria ser 1");
            Assert.AreEqual(new DateTime(2011, 04, 25), projetoA.Ciclos[0].DtInicio, "O valor de inicio deveria ser 25/04/2011");
            Assert.AreEqual(new DateTime(2011, 05, 20), projetoA.Ciclos[0].DtTermino, "O valor de término deveria ser 20/05/2011");

            Assert.AreEqual(2, projetoA.Ciclos[1].NbCiclo, "O ciclo deveria ser 2");
            Assert.AreEqual(new DateTime(2011, 05, 24), projetoA.Ciclos[1].DtInicio, "O valor de inicio deveria ser 24/05/2011");
            Assert.AreEqual(new DateTime(2011, 06, 20), projetoA.Ciclos[1].DtTermino, "O valor de término deveria ser 20/06/2011");

            Assert.AreEqual(3, projetoA.Ciclos[2].NbCiclo, "O ciclo deveria ser 3");
            Assert.AreEqual(new DateTime(2011, 06, 22), projetoA.Ciclos[2].DtInicio, "O valor de inicio deveria ser 22/06/2011");
            Assert.AreEqual(new DateTime(2011, 07, 19), projetoA.Ciclos[2].DtTermino, "O valor de término deveria ser 19/07/2011");

            Assert.AreEqual(4, projetoA.Ciclos[3].NbCiclo, "O ciclo deveria ser 4");
            Assert.AreEqual(new DateTime(2011, 07, 21), projetoA.Ciclos[3].DtInicio, "O valor de inicio deveria ser 21/07/2011");
            Assert.AreEqual(new DateTime(2011, 08, 17), projetoA.Ciclos[3].DtTermino, "O valor de término deveria ser 17/08/2011");

            Assert.AreEqual(5, projetoA.Ciclos[4].NbCiclo, "O ciclo deveria ser 5");
            Assert.AreEqual(new DateTime(2011, 08, 19), projetoA.Ciclos[4].DtInicio, "O valor de inicio deveria ser 19/08/2011");
            Assert.AreEqual(new DateTime(2011, 09, 15), projetoA.Ciclos[4].DtTermino, "O valor de término deveria ser 15/09/2011");

            Assert.AreEqual(6, projetoA.Ciclos[5].NbCiclo, "O ciclo deveria ser 6");
            Assert.AreEqual(new DateTime(2011, 09, 19), projetoA.Ciclos[5].DtInicio, "O valor de inicio deveria ser 19/09/2011");
            Assert.AreEqual(new DateTime(2011, 10, 14), projetoA.Ciclos[5].DtTermino, "O valor de término deveria ser 14/10/2011");

            //Passo 6

            projetoA.NbCicloTotalPlan = 4;

            projetoA.Save();

            Assert.AreEqual(1, projetoA.Ciclos[0].NbCiclo, "O ciclo deveria ser 1");
            Assert.AreEqual(new DateTime(2011, 04, 25), projetoA.Ciclos[0].DtInicio, "O valor de inicio deveria ser 25/04/2011");
            Assert.AreEqual(new DateTime(2011, 05, 20), projetoA.Ciclos[0].DtTermino, "O valor de término deveria ser 20/05/2011");

            Assert.AreEqual(2, projetoA.Ciclos[1].NbCiclo, "O ciclo deveria ser 2");
            Assert.AreEqual(new DateTime(2011, 05, 24), projetoA.Ciclos[1].DtInicio, "O valor de inicio deveria ser 24/05/2011");
            Assert.AreEqual(new DateTime(2011, 06, 20), projetoA.Ciclos[1].DtTermino, "O valor de término deveria ser 20/06/2011");

            Assert.AreEqual(3, projetoA.Ciclos[2].NbCiclo, "O ciclo deveria ser 3");
            Assert.AreEqual(new DateTime(2011, 06, 22), projetoA.Ciclos[2].DtInicio, "O valor de inicio deveria ser 22/06/2011");
            Assert.AreEqual(new DateTime(2011, 07, 19), projetoA.Ciclos[2].DtTermino, "O valor de término deveria ser 19/07/2011");

            Assert.AreEqual(4, projetoA.Ciclos[3].NbCiclo, "O ciclo deveria ser 4");
            Assert.AreEqual(new DateTime(2011, 07, 21), projetoA.Ciclos[3].DtInicio, "O valor de inicio deveria ser 21/07/2011");
            Assert.AreEqual(new DateTime(2011, 08, 17), projetoA.Ciclos[3].DtTermino, "O valor de término deveria ser 17/08/2011");

        }
        /// <summary>
        /// método TestarAlterarInicioRealDoProjetoQueJaPossuiCiclosConcluidos
        /// </summary>
        [TestMethod]
        public void TestarAlterarInicioRealDoProjetoQueJaPossuiCiclosConcluidos()
        {
            /**
             * Cenário 6: Será criado um projeto
             * Em seguida, será testado alterar o início real do projeto que ja possui ciclos concluídos
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 30, "");
            Modulo moduloA = ModuloFactory.Criar(SessionTest, projetoA, "", false);
            moduloA.NbEsforcoPlanejado = 100;
            moduloA.Save();
            Beneficiado beneficiadoA = BeneficiadoFactory.Criar(SessionTest, "", true);
            Estoria estoriaA = EstoriaFactory.Criar(SessionTest, moduloA, "", "", "", beneficiadoA, "", "", "", true);
            projetoA.DtInicioPlan = new DateTime(2011, 05, 01);
            projetoA.DtInicioReal = new DateTime(2011, 05, 01);
            projetoA.DtTerminoPlan = new DateTime(2011, 05, 31);
            projetoA.NbTamanhoTotal = 30;
            projetoA.NbCicloTotalPlan = 2;
            projetoA.NbCicloDuracaoDiasPlan = 10;
            projetoA.NbCicloDiasIntervalo = 1;

            projetoA.Save();

            //Passo 1

            projetoA.Ciclos[0].CsSituacaoCiclo = CsSituacaoCicloDomain.Concluido;
            projetoA.Ciclos[0].Save();
            projetoA.DtInicioReal = new DateTime(2011, 06, 01);
            projetoA.Save();

            if (projetoA.Ciclos[0].CsSituacaoCiclo != CsSituacaoCicloDomain.Concluido)
                Assert.AreEqual(new DateTime(2011, 06, 29), projetoA._DtTerminoReal, "A data do termino real deveria ser 29/06/2011");
            else
                Assert.AreEqual(new DateTime(2011, 05, 31), projetoA._DtTerminoReal, "A data do termino real deveria ser 31/05/2011");


            //Passo 2


            projetoA.Ciclos[0].CsSituacaoCiclo = CsSituacaoCicloDomain.NaoPlanejado;
            projetoA.Ciclos[0].Save();

            projetoA.DtInicioReal = new DateTime(2011, 06, 01);
            projetoA.Save();

            Assert.AreEqual(new DateTime(2011, 06, 01), projetoA.Ciclos[0].DtInicio, "A data do termino real deveria ser 01/06/2011");
            Assert.AreEqual(new DateTime(2011, 06, 14), projetoA.Ciclos[0].DtTermino, "A data do termino real deveria ser 14/06/2011");

            Assert.AreEqual(new DateTime(2011, 06, 16), projetoA.Ciclos[1].DtInicio, "A data do termino real deveria ser 16/06/2011");
            Assert.AreEqual(new DateTime(2011, 06, 29), projetoA.Ciclos[1].DtTermino, "A data do termino real deveria ser 29/06/2011");
        }
        /// <summary>
        /// método TestarAlterarASituacaoDosItensDoCiclo
        /// </summary>
        [TestMethod]
        public void TestarAlterarASituacaoDosItensDoCiclo()
        {
            /**
             * Cenário 7: Será cadastrado um projeto que conterá um ciclo.
             * Serão adicionadas Estórias para esse Ciclo e em sequida alterada a situação do mesmo
             * O sistema deverá recalcular as situações corretamente.
             */

            //Passo1

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 05, 25);
            projeto01.DtInicioReal = new DateTime(2011, 05, 25);
            projeto01.NbCicloTotalPlan = 2;
            projeto01.NbCicloDuracaoDiasPlan = 10;
            projeto01.NbCicloDiasIntervalo = 1;
            projeto01.Save();

            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Não Planejado'");

            //Passo 2

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "", false);
            modulo01.NbEsforcoPlanejado = 100;
            modulo01.Save();

            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "", true);
            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado, "Observacoes", "Referencias", "Duvidas");
            estoria01.CsSituacao = CsEstoriaDomain.NaoIniciado;
            estoria01.Save();
            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado, "Observacoes", "Referencias", "Duvidas");
            estoria02.CsSituacao = CsEstoriaDomain.NaoIniciado;
            estoria02.Save();
            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado, "Observacoes", "Referencias", "Duvidas");
            estoria03.CsSituacao = CsEstoriaDomain.NaoIniciado;
            estoria03.Save();
            Estoria estoria04 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado, "Observacoes", "Referencias", "Duvidas");
            estoria04.CsSituacao = CsEstoriaDomain.NaoIniciado;
            estoria04.Save();

            CicloDesenvEstoria cicloDesenvEstoria1 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria01, true);
            CicloDesenvEstoria cicloDesenvEstoria2 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria02, true);
            CicloDesenvEstoria cicloDesenvEstoria3 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria03, true);
            CicloDesenvEstoria cicloDesenvEstoria4 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria04, true);

            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsSituacaoCicloDomain.Planejado, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Não Planejado'");
            //Passo 3

            cicloDesenvEstoria1.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria1.Save();
            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsSituacaoCicloDomain.EmAndamento, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Em Andamento'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Não Planejado'");


            //Passo 4

            cicloDesenvEstoria1.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloDesenvEstoria1.Save();

            cicloDesenvEstoria2.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloDesenvEstoria2.Save();

            cicloDesenvEstoria3.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloDesenvEstoria3.Save();

            cicloDesenvEstoria4.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;
            cicloDesenvEstoria4.Save();

            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsSituacaoCicloDomain.Concluido, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Concluido'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Não Planejado'");

            //Passo 5

            cicloDesenvEstoria1.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria1.Save();
            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Pronto, cicloDesenvEstoria1.CsSituacao, "A situação da estória não pode modificar se meu ciclo estiver pronto.");
            Assert.AreEqual(CsSituacaoCicloDomain.Concluido, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Em Andamento'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do Ciclo deveria ser 'Não Planejado'");
        }
        /// <summary>
        /// método TesteAlterarSituacaoDoCiclo
        /// </summary>
        [TestMethod]
        public void TesteAlterarSituacaoDoCiclo()
        {

            /**
             * Cenário 8: Será criado um ciclo e associadas Estórias para ele.
             * A situação do ciclo será alterada e a situação das associações deverá mudar de acordo ou impedir a mudança
             */

            //Passo1

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 25);
            projeto01.DtInicioReal = new DateTime(2011, 04, 25);
            projeto01.NbCicloTotalPlan = 5;
            projeto01.NbCicloDuracaoDiasPlan = 10;
            projeto01.NbCicloDiasIntervalo = 1;
            projeto01.Save();
            ProjetoParteInteressada parteInteressada01 = ProjetoParteInteressadaFactory.Criar(SessionTest, projeto01, true);

            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[2].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[3].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[4].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");

            //Passo 2

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "", false);
            modulo01.NbEsforcoPlanejado = 100;
            modulo01.Save();

            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "", true);

            Projeto.SelectedProject = projeto01.Oid;

            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo1", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas");
            estoria01.CsSituacao = CsEstoriaDomain.NaoIniciado;
            //estoria01.NbPrioridade = 1;
            estoria01.Save();
            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo2", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas");
            estoria02.CsSituacao = CsEstoriaDomain.NaoIniciado;
            //estoria02.NbPrioridade = 2;
            estoria02.Save();
            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo3", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas");
            estoria03.CsSituacao = CsEstoriaDomain.NaoIniciado;
            //estoria03.NbPrioridade = 3;
            estoria03.Save();
            Estoria estoria04 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo4", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas");
            estoria04.CsSituacao = CsEstoriaDomain.NaoIniciado;
            //estoria04.NbPrioridade = 4;
            estoria04.Save();
            Estoria estoria05 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo5", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas");
            estoria05.CsSituacao = CsEstoriaDomain.NaoIniciado;
            //estoria05.NbPrioridade = 5;
            estoria05.Save();

            CicloDesenvEstoria cicloEstoriaDesenv01 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria01, true);
            CicloDesenvEstoria cicloEstoriaDesenv02 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria02, true);
            CicloDesenvEstoria cicloEstoriaDesenv03 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria03, true);
            CicloDesenvEstoria cicloEstoriaDesenv04 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria04, true);

            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsSituacaoCicloDomain.Planejado, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[2].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[3].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[4].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Planejado'");

            Assert.AreEqual(0, estoria01.NbPrioridade, "A prioridade da Estória deveria ser 0");
            Assert.AreEqual(0, estoria02.NbPrioridade, "A prioridade da Estória deveria ser 0");
            Assert.AreEqual(0, estoria03.NbPrioridade, "A prioridade da Estória deveria ser 0");
            Assert.AreEqual(0, estoria04.NbPrioridade, "A prioridade da Estória deveria ser 0");
            Assert.AreEqual(1, estoria05.NbPrioridade, "A prioridade da Estória deveria ser 1");


            //Passo 03

            cicloEstoriaDesenv01.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloEstoriaDesenv01.Save();
            cicloEstoriaDesenv02.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
            cicloEstoriaDesenv02.Save();
            cicloEstoriaDesenv03.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
            cicloEstoriaDesenv03.Save();
            cicloEstoriaDesenv04.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
            cicloEstoriaDesenv04.Save();

            projeto01.Ciclos[0].CsSituacaoCiclo = CsSituacaoCicloDomain.Concluido;
            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Pronto, cicloEstoriaDesenv01.CsSituacao, "A situação do item do ciclo deveria ser 'Pronto'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Replanejado, cicloEstoriaDesenv02.CsSituacao, "A situação do item do ciclo deveria ser 'Replanejado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Replanejado, cicloEstoriaDesenv03.CsSituacao, "A situação do item do ciclo deveria ser 'Replanejado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Replanejado, cicloEstoriaDesenv04.CsSituacao, "A situação do item do ciclo deveria ser 'Replanejado'");

            //Assert.AreEqual(1, estoria02.NbPrioridade, "A prioridade deveria ser 1");
            //Assert.AreEqual(2, estoria03.NbPrioridade, "A prioridade deveria ser 2");
            //Assert.AreEqual(3, estoria04.NbPrioridade, "A prioridade deveria ser 3");
            //Assert.AreEqual(4, estoria05.NbPrioridade, "A prioridade deveria ser 4");


            //Passo 4

            //Sem  RN


            //Passo 5

            cicloEstoriaDesenv02 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[1], estoria02, true);
            cicloEstoriaDesenv03 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[1], estoria03, true);
            cicloEstoriaDesenv04 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[1], estoria04, true);

            cicloEstoriaDesenv02.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloEstoriaDesenv02.Save();
            cicloEstoriaDesenv03.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
            cicloEstoriaDesenv03.Save();
            cicloEstoriaDesenv04.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloEstoriaDesenv04.Save();

            projeto01.Ciclos[1].CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;
            projeto01.Ciclos[1].Save();

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Replanejado, cicloEstoriaDesenv02.CsSituacao, "A Situação deveria ser 'Replanejado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Replanejado, cicloEstoriaDesenv03.CsSituacao, "A Situação deveria ser 'Replanejado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.Pronto, cicloEstoriaDesenv04.CsSituacao, "A Situação deveria ser 'Pronto'");


            //Passo 6

            cicloEstoriaDesenv02 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[2], estoria02, true);
            cicloEstoriaDesenv03 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[2], estoria03, true);
            cicloEstoriaDesenv02.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
            cicloEstoriaDesenv02.Save();
            cicloEstoriaDesenv03.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloEstoriaDesenv03.Save();
            projeto01.Ciclos[2].CsSituacaoCiclo = CsSituacaoCicloDomain.Planejado;
            projeto01.Ciclos[2].Save();

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.NaoIniciado, cicloEstoriaDesenv02.CsSituacao, "A Situação deveria ser 'Não Iniciado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.EmDesenv, cicloEstoriaDesenv03.CsSituacao, "A Situação deveria ser 'Não Iniciado'");


            //Passo 7

            projeto01.Ciclos[2].CsSituacaoCiclo = CsSituacaoCicloDomain.NaoPlanejado;
            projeto01.Ciclos[2].Save();

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(projeto01.Ciclos[2],
            "ValidarCicloSemAssociacoes", DefaultContexts.Save));
        }
        /// <summary>
        /// método TestarAlterarAsConfiguracoesDoProjetoQueJaPossuiCiclosConcluidos
        /// </summary>
        [TestMethod]
        public void TestarAlterarAsConfiguracoesDoProjetoQueJaPossuiCiclosConcluidos()
        {

            /**
             * Cenário 9: Será testado alterar as configurações de um projeto que ja possua ciclos concluídos
             * O sistema deverá proceder de forma correta
             */

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 25);
            projeto01.DtInicioReal = new DateTime(2011, 04, 25);
            projeto01.NbCicloTotalPlan = 5;
            projeto01.NbCicloDuracaoDiasPlan = 10;
            projeto01.NbCicloDiasIntervalo = 1;
            projeto01.Save();

            Assert.AreEqual(new DateTime(2011, 04, 25), projeto01.Ciclos[0].DtInicio, "A data de inicio do Ciclo 1 deveria ser 25/05/2011");
            Assert.AreEqual(new DateTime(2011, 05, 06), projeto01.Ciclos[0].DtTermino, "A data de inicio do Ciclo 1 deveria ser 06/05/2011");

            Assert.AreEqual(new DateTime(2011, 05, 10), projeto01.Ciclos[1].DtInicio, "A data de inicio do projeto deveria ser 10/05/2011");
            Assert.AreEqual(new DateTime(2011, 05, 23), projeto01.Ciclos[1].DtTermino, "A data de inicio do projeto deveria ser 23/05/2011");

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", true);

            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Descricao", true);
            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas", true);
            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas", true);
            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas", true);

            CicloDesenvEstoria cicloDesenvEstoria01 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria01);
            cicloDesenvEstoria01.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloDesenvEstoria01.Save();

            CicloDesenvEstoria cicloDesenvEstoria02 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[1], estoria02);
            cicloDesenvEstoria02.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloDesenvEstoria02.Save();

            CicloDesenvEstoria cicloDesenvEstoria03 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[2], estoria03);
            cicloDesenvEstoria03.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
            cicloDesenvEstoria03.Save();

            //Passo 1

            projeto01.Ciclos[0].CsSituacaoCiclo = CsSituacaoCicloDomain.Concluido;
            projeto01.Ciclos[0].Save();

            projeto01.Ciclos[1].CsSituacaoCiclo = CsSituacaoCicloDomain.Concluido;
            projeto01.Ciclos[1].Save();

            projeto01.Ciclos[2].CsSituacaoCiclo = CsSituacaoCicloDomain.Planejado;
            projeto01.Ciclos[2].Save();

            projeto01.NbCicloDuracaoDiasPlan = 5;
            projeto01.NbCicloDiasIntervalo = 2;
            projeto01.Save();

            Assert.AreEqual(new DateTime(2011, 04, 25), projeto01.Ciclos[0].DtInicio, "A data de inicio do Ciclo 1 deveria ser 25/05/2011");
            Assert.AreEqual(new DateTime(2011, 05, 06), projeto01.Ciclos[0].DtTermino, "A data de inicio do Ciclo 1 deveria ser 06/05/2011");

            Assert.AreEqual(new DateTime(2011, 05, 10), projeto01.Ciclos[1].DtInicio, "A data de inicio do projeto deveria ser 10/05/2011");
            Assert.AreEqual(new DateTime(2011, 05, 23), projeto01.Ciclos[1].DtTermino, "A data de inicio do projeto deveria ser 23/05/2011");

            Assert.AreEqual(new DateTime(2011, 05, 26), projeto01.Ciclos[2].DtInicio, "A data de inicio do projeto deveria ser 26/05/2011");
            Assert.AreEqual(new DateTime(2011, 06, 01), projeto01.Ciclos[2].DtTermino, "A data de inicio do projeto deveria ser 25/05/2011");

            //Passo 2


            cicloDesenvEstoria03.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
            cicloDesenvEstoria03.Save();

            projeto01.Ciclos[2].Save();

            projeto01.NbCicloTotalPlan = 1;
            projeto01.Save();

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(projeto01,
            "ValidarPermitirDeletarCiclos", DefaultContexts.Save));

            //Passo 3

            projeto01.NbCicloTotalPlan = 2;
            projeto01.Save();

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(projeto01,
            "ValidarPermitirDeletarCiclos", DefaultContexts.Save));
        }

    }
}