using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Library.Libs.Xaf;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Classe de testes do BaseObject PlanCalendario.
    /// </summary>
    [TestClass]
    public class CalendarioTest : BaseTest
    {
        /// <summary>
        /// Testar cadastrar calendários ativos com períodos e tipo 
        /// (útil ou não útil) conflitantes (Falhar)
        /// </summary>
        [TestMethod]
        public void Test_eFi_Crono33()
        {
            const string RESTRICAO = "CalendarioConflitante";

            CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest, CsCalendarioDomain.Folga,
            new DateTime(2010, 11, 15), new DateTime(2010, 11, 20), true);

            /**
             * Cenário1:
             * Serão criados calendários conflitantes por período.
             * O sistema deve rejeitá-los.
             */

            Calendario calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Folga, new DateTime(2010, 11, 15), new DateTime(2010, 11, 16));

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 20), new DateTime(2010, 11, 25));

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 16), new DateTime(2010, 11, 19));

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            /**
             * Cenário2:
             * Será criado um calendário por dia do ano específico, conflitando 
             * com um calendário de período existente. O sistema deve rejeitar.
             */

            calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Trabalho, 20, CsMesDomain.Novembro, 2010);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            /**
             * Cenário3:
             * Será criado um calendário por dia do ano específico, não 
             * conflitante com o calendário de período existente mas conflitante
             * com um calendario para um dia especifico. O sistema 
             * deve rejeitar.
             */

            calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Trabalho, 21, CsMesDomain.Novembro, 2010, true);

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));


            calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Trabalho, 21, CsMesDomain.Novembro, 2010);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            /**
             * Cenário4:
             * - Será criado um calendário por dia e mês, não conflitante com os 
             *   calendários existentes. O sistema deve aceitar. 
             * - Em seguida, será criado um outro calendário por dia e mês conflitante. 
             *   O sistema deve rejeitar.
             */

            calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Trabalho, 23, CsMesDomain.Novembro);

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario.Save();

            calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Trabalho, 23, CsMesDomain.Novembro);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            /**
             * Cenário5:
             * - Será criado um calendário por dia, mês e ano, conflitante com os 
             *   calendários por dia e mês existente. O sistema deve aceitar pois dará
             *   prioridade ao calendário por dia mês e ano.
             */

            calendario = CalendarioFactory.CriarCalendarioPorDiaMesAno(SessionTest,
            CsCalendarioDomain.Trabalho, 23, CsMesDomain.Novembro, 2010);

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 23), new DateTime(2010, 11, 23));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));
        }

        /// <summary>
        /// Testar cadastrar calendário ativo com período e tipo (útil ou 
        /// não útil) não conflitante (Sucesso)
        /// </summary>
        [TestMethod]
        public void Test_eFi_Crono34()
        {
            CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest, CsCalendarioDomain.Folga,
            new DateTime(2010, 11, 15), new DateTime(2010, 11, 20), true);

            Calendario calendario02 = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Folga, new DateTime(2010, 11, 10), new DateTime(2010, 11, 14));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario02,
            "CalendarioConflitante", DefaultContexts.Save));

            Calendario calendario03 = CalendarioFactory.CriarCalendarioPorPeriodo(
            SessionTest, CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 21),
            new DateTime(2010, 11, 22));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario03,
            "CalendarioConflitante", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar cadastrar calendário ativo com período e tipo (útil ou não 
        /// útil) conflitante com um calendário inativo (Sucesso)
        /// </summary>
        [TestMethod]
        public void Test_eFi_Crono35()
        {
            const string RESTRICAO = "CalendarioConflitante";

            Calendario calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest, CsCalendarioDomain.Folga,
            new DateTime(2010, 11, 15), new DateTime(2010, 11, 20));

            calendario.CsSituacao = CsSituacaoDomain.Inativo;
            calendario.Save();

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Folga, new DateTime(2010, 11, 15), new DateTime(2010, 11, 16));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 20), new DateTime(2010, 11, 25));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 16), new DateTime(2010, 11, 19));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 14), new DateTime(2010, 11, 21));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));

            calendario = CalendarioFactory.CriarCalendarioPorPeriodo(SessionTest,
            CsCalendarioDomain.Trabalho, new DateTime(2010, 11, 15), new DateTime(2010, 11, 20));

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            RESTRICAO, DefaultContexts.Save));
        }

        /// <summary>
        /// Tentar cadastrar uma data inválida no calendario do tipo dia/mês
        /// </summary>
        [TestMethod]
        public void Test_Efi_13132()
        {
            /*
                                                             *Quando esse calendario é usado, o framework não faz a busca da data mais proxima, pois os campos são ambos inteiros;
                                                             *Para corrigir foi criada uma propriedade booleana que verifica se a data é permitida ou não
                                                             */

            //tentar cadastrar o dia 30/02
            Calendario calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Folga, 30, CsMesDomain.Fevereiro);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            "PlanCalendarioCoorp_NbDia_Validacao", DefaultContexts.Save));

            //tentar -3/11
            calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Folga, -3, CsMesDomain.Novembro);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            "PlanCalendarioCoorp_NbDia_Validacao", DefaultContexts.Save));

            //tentar 31/09 - data inválida
            calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Folga, 31, CsMesDomain.Setembro);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(calendario,
            "PlanCalendarioCoorp_NbDia_Validacao", DefaultContexts.Save));

            //data válida - 29/01
            calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Trabalho, 29, CsMesDomain.Janeiro);

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(calendario,
            "PlanCalendarioCoorp_NbDia_Validacao", DefaultContexts.Save));
        }

        /// <summary>
        /// Inclui folga no final do projeto
        /// </summary>
        [TestMethod]
        public void IncluirFolgaAlteraFinalDoProjeto()
        {
            Projeto projeto = ProjetoFactory.Criar(SessionTest, 300, "", true);

            projeto.DtInicioPlan = new DateTime(2011, 09, 1);
            projeto.DtTerminoPlan = new DateTime(2011, 09, 30);
            projeto.NbCicloTotalPlan = 2;
            projeto.NbCicloDuracaoDiasPlan = 10;
            projeto.NbCicloDiasIntervalo = 1;
            projeto.Save();
            Calendario calendario = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Folga, 13, CsMesDomain.Setembro);
            calendario.Save();

            Assert.AreEqual(new DateTime(2011, 09, 30), projeto._DtTerminoReal, "A data de termino deveria ser 30/09/2011 ");
        }

        /// <summary>
        /// Verifica se há feriado no dia.
        /// </summary>
        [TestMethod]
        public void VerificaFeriadoDia()
        {
            Calendario calendario01 = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Folga, 13, CsMesDomain.Setembro);
            calendario01.Save();

            Calendario calendario02 = CalendarioFactory.CriarCalendarioPorDiaMes(SessionTest,
            CsCalendarioDomain.Trabalho, 14, CsMesDomain.Setembro);
            calendario02.Save();

            Assert.IsTrue(Calendario.GetFeriadoCalendario(SessionTest, new DateTime(2011, 09, 13)), "Feriado verificado");

            Assert.IsFalse(Calendario.GetFeriadoCalendario(SessionTest, new DateTime(2011, 09, 14)), "Trabalho verificado");

            Assert.IsFalse(Calendario.GetFeriadoCalendario(SessionTest, new DateTime(2011, 09, 17)), "Trabalho verificado");
        }
    }
}