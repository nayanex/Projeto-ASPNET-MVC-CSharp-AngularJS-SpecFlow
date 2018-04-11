using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Shared.Domains.Rh;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.Xaf;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe Colaborador
    /// </summary>
    [TestClass]
    public class ColaboradorTest : BaseTest
    {
        [TestMethod]
        public void RnAutenticarColaboradorQuandoJaExistirUmColaboradorTest()
        {
            Colaborador colaborador = new Colaborador(SessionTest);
            colaborador.TxMatricula = "23456";
            colaborador.Usuario.UserName = "adeilton.ramos";
            colaborador.Usuario.Email = "adeilton.ramos@fpf.br";
            colaborador.Save();

            Colaborador.RnAutenticarColaborador(SessionTest, "adeilton.ramos", "adeilton.ramos@fpf.br");

            Colaborador colaboradorRetorno = SessionTest.FindObject<Colaborador>(CriteriaOperator.Parse("Usuario.UserName = ?", "adeilton.ramos"));

            Assert.AreEqual(colaborador, colaboradorRetorno);
        }

        [TestMethod]
        public void RnAutenticarColaboradorQuandoNaoExistirUmColaboradorTest()
        {
            Colaborador.RnAutenticarColaborador(SessionTest, "adeilton.ramos", "adeilton.ramos@fpf.br");

            Colaborador colaboradorRetorno = SessionTest.FindObject<Colaborador>(CriteriaOperator.Parse("Usuario.UserName = ?", "adeilton.ramos"));

            Assert.IsNotNull(colaboradorRetorno);
        }

        /// <summary>
        /// Testar listar, para cada colaborador, todos os planejamentos de férias atuais não realizados
        /// (independente de período aquisitivo) em ordem crescente de data de início.
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_002_TestarListarTodosOsPlanejamentosFeriasAtuaisNaoRealizados()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Modalidade: 30 dias
            ModalidadeFerias modalidade30 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 30, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Usuário: Maria Silva
            Colaborador colaboradorMariaSilva = ColaboradorFactory.CriarColaborador(SessionTest, "456",
                new DateTime(2011, 2, 1), "maria.silva@fpf.br", "Maria", string.Empty, "Silva", "maria.silva", cargoJr2);

            // Período aquisitivo para João Souza
            ColaboradorPeriodoAquisitivo periodoJoaoSouza = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaboradorJoaoSouza, new DateTime(2011, 1, 1), true);

            // Período aquisitivo para Maria Silva
            ColaboradorPeriodoAquisitivo periodoMariaSilva = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaboradorMariaSilva, new DateTime(2011, 2, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoJoaoSouza,
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoJoaoSouza,
                modalidade5, new DateTime(2012, 3, 1));

            // Planejamento de Férias: 01/03/2012 ao dia 29/03/2012. Colaborador: Maria Silva
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoMariaSilva,
                modalidade30, new DateTime(2012, 3, 1));

            #endregion

            #region Resultados

            Assert.AreEqual("15 dia(s) - 10/02/2012 - 24/02/2012 (Planejado)\n5 dia(s) - 01/03/2012 - 05/03/2012 (Planejado)\n",
                colaboradorJoaoSouza._PlanoFeriasAtual);

            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradorJoaoSouza._CsSituacaoFerias);

            Assert.AreEqual("30 dia(s) - 01/03/2012 - 30/03/2012 (Planejado)\n", colaboradorMariaSilva._PlanoFeriasAtual);

            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradorMariaSilva._CsSituacaoFerias);

            #endregion
        }

        /// <summary>
        /// Testar listar, para cada colaborador, planejamentos de
        /// férias atuais (atrasados)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_003_TestarListarTodosOsPlanejamentosFeriasAtuaisAtrasados()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 11);

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Período aquisitivo para João Souza
            ColaboradorPeriodoAquisitivo periodoJoaoSouza = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaboradorJoaoSouza, new DateTime(2011, 1, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoJoaoSouza,
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoJoaoSouza,
                modalidade5, new DateTime(2012, 3, 1));

            #endregion

            #region Resultados

            Assert.AreEqual("15 dia(s) - 10/02/2012 - 24/02/2012 (Em Atraso)\n5 dia(s) - 01/03/2012 - 05/03/2012 (Planejado)\n",
                colaboradorJoaoSouza._PlanoFeriasAtual);

            Assert.AreEqual(CsSituacaoFerias.EmAtraso, colaboradorJoaoSouza._CsSituacaoFerias);

            #endregion
        }

        /// <summary>
        /// Testar criar colaboradores com "Usuário de Rede", "E-mail" e/ou
        /// "Matrícula" repetidos (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_004_TestarCriarColaboradoresComUsuarioEmailMatriculaRepetidos()
        {
            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Cargo: Programador Jr 3
            Cargo cargoJr3 = CargoFactory.Criar(SessionTest, "Programador Jr 3", true);

            ColaboradorFactory.CriarColaborador(SessionTest, "123", new DateTime(2011, 01, 01),
                "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            Colaborador colaboradorJoaoSouza02 = ColaboradorFactory.CriarColaborador(SessionTest, "123", new DateTime(2011, 03, 10),
                "joao.souza@fpf.br", "João", "Silva", "Souza", "joao.souza", cargoJr3, false);

            // Validação da Matrícula
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(colaboradorJoaoSouza02,
                "Colaborador_TxMatricula_Unique", DefaultContexts.Save));

            // Validação do nome do Usuário
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(colaboradorJoaoSouza02.Usuario,
                "User Name is unique", DefaultContexts.Save));

            // Validação da Email
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(colaboradorJoaoSouza02,
                "Colaborador_Email_Unique", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar alterar a data de admissão de um colaborador ativo para deve
        /// gerar ou recalcular os períodos aquisitivos
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_005_TestarAlterarDataAdmissaoColaboradorAtivo()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Colaborador
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123", new DateTime(2000, 01, 01),
                "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            #endregion

            #region Lista de Períodos Aquisitivos esperada

            List<List<DateTime>> periodos = new List<List<DateTime>>();

            // 01/01/2000 - 31/12/2000
            periodos.Add(new List<DateTime>() { new DateTime(2000, 1, 1), new DateTime(2000, 12, 31) });

            // 01/01/2001 - 31/12/2001
            periodos.Add(new List<DateTime>() { new DateTime(2001, 1, 1), new DateTime(2001, 12, 31) });

            // 01/01/2002 - 31/12/2002
            periodos.Add(new List<DateTime>() { new DateTime(2002, 1, 1), new DateTime(2002, 12, 31) });

            // 01/01/2003 - 31/12/2003
            periodos.Add(new List<DateTime>() { new DateTime(2003, 1, 1), new DateTime(2003, 12, 31) });

            // 01/01/2004 - 31/12/2004
            periodos.Add(new List<DateTime>() { new DateTime(2004, 1, 1), new DateTime(2004, 12, 31) });

            // 01/01/2005 - 31/12/2005
            periodos.Add(new List<DateTime>() { new DateTime(2005, 1, 1), new DateTime(2005, 12, 31) });

            // 01/01/2006 - 31/12/2006
            periodos.Add(new List<DateTime>() { new DateTime(2006, 1, 1), new DateTime(2006, 12, 31) });

            // 01/01/2007 - 31/12/2007
            periodos.Add(new List<DateTime>() { new DateTime(2007, 1, 1), new DateTime(2007, 12, 31) });

            // 01/01/2008 - 31/12/2008
            periodos.Add(new List<DateTime>() { new DateTime(2008, 1, 1), new DateTime(2008, 12, 31) });

            // 01/01/2009 - 31/12/2009
            periodos.Add(new List<DateTime>() { new DateTime(2009, 1, 1), new DateTime(2009, 12, 31) });

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 31/12/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 12, 31) });

            // 01/01/2012 - 31/12/2012
            periodos.Add(new List<DateTime>() { new DateTime(2012, 1, 1), new DateTime(2012, 12, 31) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            int position = 0;
            foreach(ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            colaboradorJoaoSouza.Reload();
            colaboradorJoaoSouza.DtAdmissao = new DateTime(2005, 4, 5);
            colaboradorJoaoSouza.Save();

            #region Lista de Períodos Aquisitivos esperada

            periodos = new List<List<DateTime>>();

            // 05/04/2005 - 04/04/2006
            periodos.Add(new List<DateTime>() { new DateTime(2005, 4, 5), new DateTime(2006, 4, 4) });

            // 05/04/2006 - 04/04/2007
            periodos.Add(new List<DateTime>() { new DateTime(2006, 4, 5), new DateTime(2007, 4, 4) });

            // 05/04/2007 - 04/04/2008
            periodos.Add(new List<DateTime>() { new DateTime(2007, 4, 5), new DateTime(2008, 4, 4) });

            // 05/04/2008 - 04/04/2009
            periodos.Add(new List<DateTime>() { new DateTime(2008, 4, 5), new DateTime(2009, 4, 4) });

            // 05/04/2009 - 04/04/2010
            periodos.Add(new List<DateTime>() { new DateTime(2009, 4, 5), new DateTime(2010, 4, 4) });

            // 05/04/2010 - 04/04/2011
            periodos.Add(new List<DateTime>() { new DateTime(2010, 4, 5), new DateTime(2011, 4, 4) });

            // 05/04/2011 - 04/04/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 4, 5), new DateTime(2012, 4, 4) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }
        }

        /// <summary>
        /// Testar colocar um colaborador como ativo (deve gerar ou recalcular
        /// os períodos aquisitivos até a data de hoje)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_006_TestarColocarColaboradorComoAtivo()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Colaborador
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123", new DateTime(2005, 4, 5),
                "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2, false);

            #endregion

            colaboradorJoaoSouza.Usuario.IsActive = false;
            colaboradorJoaoSouza.Save();

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(0, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            colaboradorJoaoSouza.Reload();
            colaboradorJoaoSouza.Usuario.IsActive = true;
            colaboradorJoaoSouza.Save();

            #region Lista de Períodos Aquisitivos esperada

            List<List<DateTime>> periodos = new List<List<DateTime>>();

            // 05/04/2005 - 04/04/2006
            periodos.Add(new List<DateTime>() { new DateTime(2005, 4, 5), new DateTime(2006, 4, 4) });

            // 05/04/2006 - 04/04/2007
            periodos.Add(new List<DateTime>() { new DateTime(2006, 4, 5), new DateTime(2007, 4, 4) });

            // 05/04/2007 - 04/04/2008
            periodos.Add(new List<DateTime>() { new DateTime(2007, 4, 5), new DateTime(2008, 4, 4) });

            // 05/04/2008 - 04/04/2009
            periodos.Add(new List<DateTime>() { new DateTime(2008, 4, 5), new DateTime(2009, 4, 4) });

            // 05/04/2009 - 04/04/2010
            periodos.Add(new List<DateTime>() { new DateTime(2009, 4, 5), new DateTime(2010, 4, 4) });

            // 05/04/2010 - 04/04/2011
            periodos.Add(new List<DateTime>() { new DateTime(2010, 4, 5), new DateTime(2011, 4, 4) });

            // 05/04/2011 - 04/04/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 4, 5), new DateTime(2012, 4, 4) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            int position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }
        }

        /// <summary>
        /// Testar salvar um colaborador onde a data de admissão seja maior
        /// que um início de qualquer planejamento de férias cadastrado para o mesmo (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_007_TestarSalvarColaboradorDataAdmissaoSejaMaiorInicioQualquerPlanejamentoFeriasCadastrado()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade5, new DateTime(2012, 3, 1));

            #endregion

            // Alteração da data de admissão do colaborador para o dia 11/02/2012
            colaboradorJoaoSouza.Reload();
            colaboradorJoaoSouza.DtAdmissao = new DateTime(2012, 2, 11);

            // Validação da data de admissão
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(colaboradorJoaoSouza,
                "ValidacaoDtAdmissao_Colaborador", DefaultContexts.Save));

            // Alteração da data de admissão do colaborador para o dia 02/03/2012
            colaboradorJoaoSouza.DtAdmissao = new DateTime(2012, 3, 6);

            // Validação da data de admissão
            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(colaboradorJoaoSouza,
                "ValidacaoDtAdmissao_Colaborador", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar listar os itens do planejamento de férias
        /// para cada período aquisitivo de cada colaborador, ordenados decrescente por data de início
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_008_TestarListarItensPlanejamentoFeriasPeriodoAquisitivoColaborador()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Planejamento de Férias: 10/02/2011 ao dia 24/02/2011. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade15, new DateTime(2011, 2, 10), true);

            // Planejamento de Férias: 01/03/2011 ao dia 05/03/2011. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade5, new DateTime(2011, 3, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[1],
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[1],
                modalidade5, new DateTime(2012, 3, 1));

            #endregion

            #region Resultados

            Assert.AreEqual("15 dia(s) - 10/02/2011 - 24/02/2011 (Realizado)\n5 dia(s) - 01/03/2011 - 05/03/2011 (Realizado)\n",
                colaboradorJoaoSouza.PeriodosAquisitivos[0]._PlanejamentoFerias);

            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradorJoaoSouza.PeriodosAquisitivos[0]._CsSituacaoFerias);

            Assert.AreEqual("15 dia(s) - 10/02/2012 - 24/02/2012 (Planejado)\n5 dia(s) - 01/03/2012 - 05/03/2012 (Planejado)\n",
                colaboradorJoaoSouza.PeriodosAquisitivos[1]._PlanejamentoFerias);

            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradorJoaoSouza.PeriodosAquisitivos[1]._CsSituacaoFerias);

            #endregion
        }

        /// <summary>
        /// Testar listar os itens do planejamento de férias atrasados
        /// para cada período aquisitivo de cada colaborador
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_009_TestarListarItensPlanejamentoFeriasAtrasadosPeriodoAquisitivoColaborador()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 11);

            // Planejamento de Férias: 10/02/2011 ao dia 24/02/2011. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade15, new DateTime(2011, 2, 10), true);

            // Planejamento de Férias: 01/03/2011 ao dia 05/03/2011. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade5, new DateTime(2011, 3, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[1],
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[1],
                modalidade5, new DateTime(2012, 3, 1));

            #endregion

            #region Resultados

            Assert.AreEqual("15 dia(s) - 10/02/2011 - 24/02/2011 (Realizado)\n5 dia(s) - 01/03/2011 - 05/03/2011 (Realizado)\n",
                colaboradorJoaoSouza.PeriodosAquisitivos[0]._PlanejamentoFerias);

            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradorJoaoSouza.PeriodosAquisitivos[0]._CsSituacaoFerias);

            Assert.AreEqual("15 dia(s) - 10/02/2012 - 24/02/2012 (Em Atraso)\n5 dia(s) - 01/03/2012 - 05/03/2012 (Planejado)\n",
                colaboradorJoaoSouza.PeriodosAquisitivos[1]._PlanejamentoFerias);

            Assert.AreEqual(CsSituacaoFerias.EmAtraso, colaboradorJoaoSouza.PeriodosAquisitivos[1]._CsSituacaoFerias);

            #endregion
        }

        /// <summary>
        /// Testar calcular a situação de cada Planejamento de Férias (Planejado, Em Atraso)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_010_TestarCalcularSituacaoPlanejamentoFerias()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 11);

            // Planejamento de Férias: 10/02/2011 ao dia 24/02/2011. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade15, new DateTime(2011, 2, 10), true);

            // Planejamento de Férias: 01/03/2011 ao dia 05/03/2011. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[0],
                modalidade5, new DateTime(2011, 3, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[1],
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, colaboradorJoaoSouza.PeriodosAquisitivos[1],
                modalidade5, new DateTime(2012, 3, 1));

            #endregion

            #region Resultados

            // Situação do Planejamento
            Assert.AreEqual(CsSituacaoFerias.EmAtraso, colaboradorJoaoSouza.PeriodosAquisitivos
                [1].Planejamentos[0].CsSituacao);

            // Situação do Planejamento
            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradorJoaoSouza.PeriodosAquisitivos
                [1].Planejamentos[1].CsSituacao);

            #endregion
        }

        /// <summary>
        /// Testar salvar planejamento de férias que ultrapasse a
        /// data máxima para o período aquisitivo
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_012_TestarSalvarPlanejamentoFeriasUltrapasseDataMaximaPeriodoAquisitivo()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias: 01/02/2012 ao dia 10/02/2012. Colaborador: João Souza
            FeriasPlanejamento planejamento01 = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 2, 1));

            // Planejamento de Férias: 30/01/2012 ao dia 09/02/2012. Colaborador: João Souza
            FeriasPlanejamento planejamento02 = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 30));

            #endregion

            #region Resultados

            // Validação do período de férias
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(planejamento01,
                "FeriasPlanejamento_ValidacaoPeriodoFerias", DefaultContexts.Save));

            // Validação do período de férias
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(planejamento02,
                "FeriasPlanejamento_ValidacaoPeriodoFerias", DefaultContexts.Save));

            #endregion
        }

        /// <summary>
        /// Testar salvar planejamento de férias com período que conflita
        /// entre algum período aquisitivo (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_013_TestarSalvarPlanejamentoFeriasPeriodoConflitaEntrePeriodoAquisitivo()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Planejamento de Férias: 10/02/2011 ao dia 24/02/2011
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade15, new DateTime(2011, 2, 10), true);

            // Planejamento de Férias: 01/03/2011 ao dia 05/03/2011
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade5, new DateTime(2011, 3, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade15, new DateTime(2012, 2, 10));

            #endregion

            #region Passos

            // Planejamento de Férias: 15/02/2012 ao dia 29/02/2012
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade15, new DateTime(2012, 2, 15), false, CsSimNao.Não, false);

            #endregion

            #region Resultados

            // Validação do período de férias
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(planejamento,
                "FeriasPlanejamento_ValidacaoPeriodoFeriasConflitantes", DefaultContexts.Save));

            #endregion
        }

        /// <summary>
        /// Testar alterar a modalidade de férias (deve ser recalculado o
        /// término de acordo com o total de dias da nova modalidade)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_014_TestarAlterarModalidadeFerias()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias: 01/03/2012 ao dia 15/02/2012
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade15, new DateTime(2012, 3, 1), false, CsSimNao.Não, false);

            // Verificação da data de retorno
            Assert.AreEqual(new DateTime(2012, 3, 15), planejamento._DtRetorno,
                "A data de retorno deveria ser 15/02/2012");

            planejamento.Modalidade = modalidade10;

            // Verificação da data de retorno
            Assert.AreEqual(new DateTime(2012, 3, 10), planejamento._DtRetorno,
                "A data de retorno deveria ser 15/02/2012");

            #endregion
        }

        /// <summary>
        /// Testar salvar o planejamento de férias e recalcular o
        /// total de ferias planejadas
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_015_TestarSalvarPlanejamentoFeriasRecalcularTotalFeriasPlanejadas()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias: 01/03/2012 ao dia 10/02/2012
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 3, 1));

            // Verificação da quantidade de férias já planejadas
            Assert.AreEqual(uint.Parse("10"), colaboradorJoaoSouza.PeriodosAquisitivos[0].NbFeriasPlanejadas,
                "Deveria conter 10 dias");

            // Planejamento de Férias: 15/03/2012 ao dia 24/02/2012
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 3, 15));

            // Verificação da quantidade de férias já planejadas
            Assert.AreEqual(uint.Parse("20"), colaboradorJoaoSouza.PeriodosAquisitivos[0].NbFeriasPlanejadas,
                "Deveria conter 20 dias");

            #endregion
        }

        /// <summary>
        /// Testar salvar planejamento de férias com data de início
        /// menor que a data de admissão (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_016_TestarSalvarPlanejamentoFeriasDataInicioMenorDataAdmissao()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2010, 12, 25), false, CsSimNao.Não, false);

            // Validação da Data de Início
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(planejamento,
                "FeriasPlanejamento_ValidacaoDtInicio", DefaultContexts.Save));

            #endregion
        }

        /// <summary>
        /// Testar criar e alterar o planejamento de férias e verificar
        /// se o autor e a ultima atualização foram atualizados automaticamente
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_017_TestarCriarAlterarPlanejamentoFeriasVerificarAutorUltimaAtualizacaoForamAtualizados()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Usuário: João Silva
            Colaborador colaboradorJoaoSilva = ColaboradorFactory.CriarColaborador(SessionTest, "789",
                new DateTime(2011, 3, 1), "joao.silva@fpf.br", "João", string.Empty, "Silva", "joao.silva", cargoJr2);

            // Usuário: Maria Silva
            Colaborador colaboradorMariaSilva = ColaboradorFactory.CriarColaborador(SessionTest, "456",
                new DateTime(2011, 2, 1), "maria.silva@fpf.br", "Maria", string.Empty, "Silva", "maria.silva", cargoJr2);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            #endregion

            #region Passos 01 e 02

            // Current User: joao.silva
            UsuarioDAO.CurrentUser = colaboradorJoaoSilva.Usuario;

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1, 10, 0, 0);

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 3, 1));

            // Planejado por
            Assert.AreEqual("joao.silva - 01/02/2012 10:00", planejamento.TxPlanejado);

            // Atualizado por
            Assert.AreEqual(string.Empty, planejamento.TxAtualizado);

            #endregion

            #region Passos 03 e 04

            // Current User: maria.silva
            UsuarioDAO.CurrentUser = colaboradorMariaSilva.Usuario;

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1, 10, 35, 0);

            // Alteração da Modalidade
            planejamento.Modalidade = modalidade15;
            planejamento.Save();

            // Planejado por
            Assert.AreEqual("joao.silva - 01/02/2012 10:00", planejamento.TxPlanejado);

            // Atualizado por
            Assert.AreEqual("maria.silva - 01/02/2012 10:35", planejamento.TxAtualizado);

            #endregion
        }

        /// <summary>
        /// Testar salvar um planejamento de férias realizado sem existir
        /// um tipo de afastamento para férias realizadas
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_018_TestarSalvarPlanejamentoFeriasRealizadoSemExistirTipoAfastamentoParaFeriasRealizadas()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 1), true);

            // Verificação de criação de Afastamento
            Assert.AreEqual(1, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido criado um Afastamento.");

            // Verificação do Tipo do Afastamento
            Assert.AreEqual(true, colaboradorJoaoSouza.Afastamentos[0].TipoAfastamento.IsParaFeriasRealizadas,
                "O Afastamento criado deveria ser para Férias realizadas.");

            #endregion
        }

        /// <summary>
        /// Testar salvar um planejamento de férias realizado
        /// (criar automaticamente um afastamento para o colaborador)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_019_TestarSalvarPlanejamentoFeriasRealizado()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamento = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Férias", true, true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 1), true);

            // Verificação de criação de Afastamento
            Assert.AreEqual(1, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido criado um Afastamento.");

            // Verificação do Tipo do Afastamento
            Assert.AreEqual(tipoAfastamento, colaboradorJoaoSouza.Afastamentos[0].TipoAfastamento,
                "O Afastamento deveria ser o mesmo que existe para Férias Realizadas.");

            #endregion
        }

        /// <summary>
        /// Testar salvar um planejamento de férias que antes estava realizado
        /// como planejado (deve remover o registro de afastamento criado)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_020_TestarSalvarPlanejamentoFeriasAntesEstavaRealizadoComoPlanejado()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamento = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Férias", true, true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passo 01

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 1), true);

            // Verificação de criação de Afastamento
            Assert.AreEqual(1, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido criado um Afastamento.");

            // Verificação do Tipo do Afastamento
            Assert.AreEqual(tipoAfastamento, colaboradorJoaoSouza.Afastamentos[0].TipoAfastamento,
                "O Afastamento deveria ser o mesmo que existe para Férias Realizadas.");

            #endregion

            #region Passo 02

            planejamento.Realizadas = false;
            planejamento.Save();

            // Verificação de exclusão de Afastamento
            Assert.AreEqual(0, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido excluído o Afastamento.");

            // Verificação do Afastamento
            Assert.AreEqual(null, planejamento.Afastamento,
                "O Afastamento deveria ter sido removido.");

            #endregion
        }

        /// <summary>
        /// Testar excluir um planejamento de férias realizado e remover o
        /// registro de afastamento criado automaticamente
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_021_TestarExcluirPlanejamentoFeriasRealizadoRemoverRegistroAfastamentoCriadoAutomaticamente()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamento = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Férias", true, true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passo 01

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 1), true);

            // Verificação de criação de Afastamento
            Assert.AreEqual(1, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido criado um Afastamento.");

            // Verificação do Tipo do Afastamento
            Assert.AreEqual(tipoAfastamento, colaboradorJoaoSouza.Afastamentos[0].TipoAfastamento,
                "O Afastamento deveria ser o mesmo que existe para Férias Realizadas.");

            #endregion

            #region Passo 02

            // Remoção do Planejamento
            planejamento.Delete();

            // Verificação de exclusão de Afastamento
            Assert.AreEqual(0, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido excluído o Afastamento.");

            #endregion
        }

        /// <summary>
        /// Testar salvar um planejamento de férias com venda maior que o máximo de férias que
        /// se pode vender, que está na configuração (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_022_TestarSalvarPlanejamentoFeriasVendaMaiorMaximoFeriasPodeVenderConfiguracao()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade15, new DateTime(2012, 1, 1), true, CsSimNao.Sim, false);

            // Validação do período de venda de férias
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(planejamento,
                "FeriasPlanejamento_ValidacaoVendaFerias", DefaultContexts.Save));

            #endregion
        }

        /// <summary>
        /// Salvar um planejamento de férias com venda menor que o máximo de férias que se pode vender,
        /// que está nas configurações. Fazer um planejamento para o mesmo período, onde a soma da quantidade
        /// de dias ultrapasse o máximo de férias que se pode tirar (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_023_TestarFazerPlanejamentoPeríodoOndeSomaQuantidadeDiasUltrapasseMaximoFeriasPodeTirarConfiguracao()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Modalidade: 30 dias
            ModalidadeFerias modalidade30 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 30, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Planejamento de Férias
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 1), true);

            #endregion

            #region Passos

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade30, new DateTime(2012, 1, 15));

            // Validação da quantidade de Férias Planejadas
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(planejamento.Periodo,
                "ColaboradorPeriodoAquisitivo_ValidacaoQuantidadeFerias", DefaultContexts.Save));

            #endregion
        }

        /// <summary>
        /// Testar salvar afastamentos com períodos conflitantes (falhar)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_024_TestarSalvarAfastamentosPeriodosConflitantes()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamento = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Afastamento");

            // Criação de um Afastamento para o Colaborador João Souza
            ColaboradorAfastamentoFactory.CriarColaboradorAfastamento(SessionTest, colaboradorJoaoSouza, new DateTime(2011, 5, 5),
                new DateTime(2011, 6, 29), tipoAfastamento, string.Empty);

            #endregion

            #region Passos

            // Criação de um Afastamento para o Colaborador João Souza
            ColaboradorAfastamento afastamento = ColaboradorAfastamentoFactory.CriarColaboradorAfastamento(SessionTest,
                colaboradorJoaoSouza, new DateTime(2011, 5, 15), new DateTime(2011, 6, 30), tipoAfastamento, string.Empty, false);

            // Validação da quantidade de Férias Planejadas
            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(afastamento,
                "ColaboradorAfastamento_ValidacaoPeriodo", DefaultContexts.Save));

            #endregion
        }

        /// <summary>
        /// Testar salvar um afastamento não-remunerado (deve recalcular
        /// todos os períodos aquisitivos do colaborador)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_025_TestarSalvarAfastamentoNaoRemunerado()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamento = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Afastamento", false);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 17);

            #endregion

            #region Passos

            // Criação de um Afastamento para o Colaborador João Souza
            ColaboradorAfastamentoFactory.CriarColaboradorAfastamento(SessionTest, colaboradorJoaoSouza, new DateTime(2011, 5, 15),
                new DateTime(2011, 6, 30), tipoAfastamento, string.Empty);

            #region Lista de Períodos Aquisitivos esperada

            List<List<DateTime>> periodos = new List<List<DateTime>>();

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 14/05/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 5, 14) });

            // 01/07/2011 - 16/02/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 7, 1), new DateTime(2012, 2, 16) });

            // 17/02/2012 - 16/02/2013
            periodos.Add(new List<DateTime>() { new DateTime(2012, 2, 17), new DateTime(2013, 2, 16) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            int position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            #endregion
        }

        /// <summary>
        /// Testar editar um afastamento alterando o tipo (não-remunerado para
        /// remunerado e vice versa) (deve recalcular todos os períodos aquisitivos do colaborador)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_026_TestarEditarAfastamentoAlterandoTipo()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamentoNaoRemunerado = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Afastamento Não-Remunerado", false);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamentoRemunerado = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Afastamento Remunerado");

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 17);

            // Criação de um Afastamento para o Colaborador João Souza
            ColaboradorAfastamento afastamento15Mai30Jun = ColaboradorAfastamentoFactory.CriarColaboradorAfastamento(SessionTest,
                colaboradorJoaoSouza, new DateTime(2011, 5, 15), new DateTime(2011, 6, 30), tipoAfastamentoNaoRemunerado, string.Empty);

            // Criação de um Afastamento para o Colaborador João Souza
            ColaboradorAfastamento afastamento15Jul15Out = ColaboradorAfastamentoFactory.CriarColaboradorAfastamento(SessionTest,
                colaboradorJoaoSouza, new DateTime(2011, 7, 15), new DateTime(2011, 8, 15), tipoAfastamentoRemunerado, string.Empty);

            #endregion

            #region Passo 01

            #region Lista de Períodos Aquisitivos esperada

            List<List<DateTime>> periodos = new List<List<DateTime>>();

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 14/05/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 5, 14) });

            // 01/07/2011 - 16/02/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 7, 1), new DateTime(2012, 2, 16) });

            // 17/02/2012 - 16/02/2013
            periodos.Add(new List<DateTime>() { new DateTime(2012, 2, 17), new DateTime(2013, 2, 16) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            int position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            #endregion

            #region Passo 02

            afastamento15Jul15Out.SetAfastamentoOldToNull();
            afastamento15Jul15Out.Reload();
            afastamento15Jul15Out.TipoAfastamento = tipoAfastamentoNaoRemunerado;
            afastamento15Jul15Out.Save();

            #region Lista de Períodos Aquisitivos esperada

            periodos = new List<List<DateTime>>();

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 14/05/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 5, 14) });

            // 01/07/2011 - 14/07/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 7, 1), new DateTime(2011, 7, 14) });

            // 16/08/2011 - 19/03/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 8, 16), new DateTime(2012, 3, 19) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            #endregion

            #region Passo 03

            afastamento15Jul15Out.SetAfastamentoOldToNull();
            afastamento15Jul15Out.Reload();
            afastamento15Jul15Out.TipoAfastamento = tipoAfastamentoRemunerado;
            afastamento15Jul15Out.Save();

            #region Lista de Períodos Aquisitivos esperada

            periodos = new List<List<DateTime>>();

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 14/05/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 5, 14) });

            // 01/07/2011 - 16/02/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 7, 1), new DateTime(2012, 2, 16) });

            // 17/02/2012 - 16/02/2013
            periodos.Add(new List<DateTime>() { new DateTime(2012, 2, 17), new DateTime(2013, 2, 16) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            #endregion
        }

        /// <summary>
        /// Testar excluir um afastamento não-remunerado (deve recalcular
        /// todos os períodos aquisitivos do colaborador)
        /// </summary>
        [TestMethod]
        public void ColaboradorTest_027_TestarExcluirAfastamentoNaoRemunerado()
        {
            #region Pré-condições

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2010, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamentoNaoRemunerado = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Afastamento Não-Remunerado", false);

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamentoRemunerado = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Afastamento Remunerado");

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 17);

            // Criação de um Afastamento para o Colaborador João Souza
            ColaboradorAfastamento afastamento15Mai30Jun = ColaboradorAfastamentoFactory.CriarColaboradorAfastamento(SessionTest,
                colaboradorJoaoSouza, new DateTime(2011, 5, 15), new DateTime(2011, 6, 30), tipoAfastamentoNaoRemunerado, string.Empty);

            #endregion

            #region Passo 01

            #region Lista de Períodos Aquisitivos esperada

            List<List<DateTime>> periodos = new List<List<DateTime>>();

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 14/05/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 5, 14) });

            // 01/07/2011 - 16/02/2012
            periodos.Add(new List<DateTime>() { new DateTime(2011, 7, 1), new DateTime(2012, 2, 16) });

            // 17/02/2012 - 16/02/2013
            periodos.Add(new List<DateTime>() { new DateTime(2012, 2, 17), new DateTime(2013, 2, 16) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            int position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            #endregion

            #region Passo 02

            afastamento15Mai30Jun.Reload();
            afastamento15Mai30Jun.Delete();

            #region Lista de Períodos Aquisitivos esperada

            periodos = new List<List<DateTime>>();

            // 01/01/2010 - 31/12/2010
            periodos.Add(new List<DateTime>() { new DateTime(2010, 1, 1), new DateTime(2010, 12, 31) });

            // 01/01/2011 - 31/12/2011
            periodos.Add(new List<DateTime>() { new DateTime(2011, 1, 1), new DateTime(2011, 12, 31) });

            // 01/01/2012 - 31/12/2012
            periodos.Add(new List<DateTime>() { new DateTime(2012, 1, 1), new DateTime(2012, 12, 31) });

            #endregion

            // Verificação da quantidade de períodos aquisitivos criados
            Assert.AreEqual(periodos.Count, colaboradorJoaoSouza.PeriodosAquisitivos.Count);

            position = 0;
            foreach (ColaboradorPeriodoAquisitivo periodo in colaboradorJoaoSouza.PeriodosAquisitivos)
            {
                // Data de início esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][0].Date, periodo.DtInicio.Date);

                // Data de término esperada do Período Aquisitivo
                Assert.AreEqual(periodos[position][1].Date, periodo.DtTermino.Date);

                position++;
            }

            #endregion
        }

        /// <summary>
        /// método TestarControleFeriasPeriodo
        /// </summary>
        [TestMethod]
        public void TestarControleFeriasPeriodo()
        {
            
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Modalidade: 15 dias
            ModalidadeFerias modalidade15 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 15, false);

            // Modalidade: 30 dias
            ModalidadeFerias modalidade30 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 30, false);

            // Usuário: João Souza
            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Usuário: Maria Silva
            Colaborador colaboradora = ColaboradorFactory.CriarColaborador(SessionTest, "456",
                new DateTime(2011, 2, 1), "maria.silva@fpf.br", "Maria", string.Empty, "Silva", "maria.silva", cargoJr2);

            // Período aquisitivo para João Souza
            ColaboradorPeriodoAquisitivo periodoColaborador = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaborador, new DateTime(2011, 1, 1), true);

            // Período aquisitivo para Maria Silva
            ColaboradorPeriodoAquisitivo periodoColaboradora = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaboradora, new DateTime(2011, 2, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoColaborador,
                modalidade15, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoColaborador,
                modalidade5, new DateTime(2012, 3, 1));

            // Planejamento de Férias: 01/03/2012 ao dia 29/03/2012. Colaborador: Maria Silva
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoColaboradora,
                modalidade30, new DateTime(2012, 3, 1));


            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradora._CsSituacaoFerias);

            //Criar uma solicitação de orçamento com a situação "Em revisão técnica"
            //Inicio
            //Passo 1
            DateTime dtPrazo = DateTime.Now;
            string data = dtPrazo.ToString("dd/MM/yyyy");

            User usuario01 = ColaboradorFactory.CriarUsuario( SessionTest, "nome.completo", "Nome", "Completo",
                "nome@fpf.br", true);

            UsuarioDAO.CurrentUser = colaborador.Usuario;

            Colaborador.RnSalvarPeriodoUltimoPlanejamentoFerias(colaborador.Session, colaborador, 1);

            Assert.AreEqual(1, colaborador.ColaboradorUltimoFiltro.LastPeriodoFilterPlanejamentoFerias, "O colaborador selecionado é igual a sua última seleção");

            UsuarioDAO.CurrentUser = colaboradora.Usuario;

            Colaborador.RnSalvarPeriodoUltimoPlanejamentoFerias(colaboradora.Session, colaboradora, 2);

            Assert.AreEqual(2, colaboradora.ColaboradorUltimoFiltro.LastPeriodoFilterPlanejamentoFerias, "O colaborador selecionado é igual a sua última seleção");

            Assert.AreEqual(1, colaborador.ColaboradorUltimoFiltro.LastPeriodoFilterPlanejamentoFerias, "O colaborador selecionado é igual a sua última seleção");
        }

        /// <summary>
        /// método TestarControleFeriasSituacao
        /// </summary>
        [TestMethod]
        public void TestarControleFeriasSituacao()
        {

            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Modalidade: 5 dias
            ModalidadeFerias modalidade5 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 5, false);

            // Usuário: João Souza
            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Usuário: Maria Silva
            Colaborador colaboradora = ColaboradorFactory.CriarColaborador(SessionTest, "456",
                new DateTime(2011, 2, 1), "maria.silva@fpf.br", "Maria", string.Empty, "Silva", "maria.silva", cargoJr2);

            // Período aquisitivo para João Souza
            ColaboradorPeriodoAquisitivo periodoColaborador = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaborador, new DateTime(2011, 1, 1), true);

            // Período aquisitivo para Maria Silva
            ColaboradorPeriodoAquisitivo periodoColaboradora = PeriodoAquisitivoFactory.CriarPeriodoAquisitivo(SessionTest,
                colaboradora, new DateTime(2011, 2, 1), true);

            // Planejamento de Férias: 10/02/2012 ao dia 24/02/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoColaborador,
                modalidade5, new DateTime(2012, 2, 10));

            // Planejamento de Férias: 01/03/2012 ao dia 05/03/2012. Colaborador: João Souza
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoColaborador,
                modalidade5, new DateTime(2012, 3, 1));

            // Planejamento de Férias: 01/03/2012 ao dia 29/03/2012. Colaborador: Maria Silva
            FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest, periodoColaboradora,
                modalidade5, new DateTime(2012, 3, 1));


            Assert.AreEqual(CsSituacaoFerias.Planejado, colaboradora._CsSituacaoFerias);

            UsuarioDAO.CurrentUser = colaborador.Usuario;

            Colaborador.RnSalvarPeriodoUltimaSituacaoFerias(colaborador.Session, colaborador, 1);

            Assert.AreEqual(1, colaborador.ColaboradorUltimoFiltro.LastSituacaoFilterPlanejamentoFerias, "O colaborador selecionado é igual a sua última seleção");

            UsuarioDAO.CurrentUser = colaboradora.Usuario;

            Colaborador.RnSalvarPeriodoUltimaSituacaoFerias(colaboradora.Session, colaboradora, 2);

            Assert.AreEqual(2, colaboradora.ColaboradorUltimoFiltro.LastSituacaoFilterPlanejamentoFerias, "O colaborador selecionado é igual a sua última seleção");

            Assert.AreEqual(1, colaborador.ColaboradorUltimoFiltro.LastSituacaoFilterPlanejamentoFerias, "O colaborador selecionado é igual a sua última seleção");
        }

        /// <summary>
        /// Testar método CriarColaborador quando nao houver ponto no login.
        /// </summary>
        [TestMethod]
        public void RnCriarColaboradorQuandoNaoHouverPontosNoLoginTest()
        {
            DateUtil.CurrentDateTime = DateTime.Now;
            Colaborador colaboradorCriado = Colaborador.RnCriarColaborador(SessionTest, "anderson", "@fpf.br");

            Assert.IsNotNull(colaboradorCriado, "Deve retornar um objeto colaborador, confirmando que colaborador foi criado");

            Colaborador colaborador = Colaborador.GetColaboradorPorLogin(SessionTest, "anderson");

            Assert.IsNotNull(colaborador, "colaborador não deve ser nulo");

            Assert.AreEqual("Anderson", colaborador.Usuario.FirstName, "Deveria ter criado o colaborador com o nome sendo o login com primeira letra maiúscula");
            Assert.AreEqual("", colaborador.Usuario.LastName, "Deveria ter criado o sobrenome vazio pois o login nao tem ponto.");
            Assert.AreEqual("Anderson", colaborador.Usuario.FullName, "Deveria ter criado o nome completo a partir do UserName passado, modificando a primeira letra para maiúscula");
            Assert.AreEqual(DateUtil.CurrentDateTime, colaborador.DtAdmissao, "Deveria ter criado a data de admissão com a data corrente");
            Assert.AreEqual("anderson", colaborador.Usuario.UserName, "Deveria ter criado o UserName a partir do parâmetro passado");
            Assert.AreEqual("anderson@fpf.br", colaborador.Usuario.Email, "Deveria ter criado o email com o UserName e o formato de email da empresa");
            Assert.IsNotNull(colaborador.Usuario.Roles, "Deveria ter criado uma Role para o colaborador");
        }

        /// <summary>
        /// Testar método CriarColaborador quando houver ponto no login.
        /// </summary>
        [TestMethod]
        public void RnCriarColaboradorQuandoHouverPontosNoLoginTest()
        {
            DateUtil.CurrentDateTime = DateTime.Now;
            Colaborador colaboradorCriado = Colaborador.RnCriarColaborador(SessionTest, "anderson.lins", "@fpf.br");

            Assert.IsNotNull(colaboradorCriado, "Deve retornar um objeto colaborador, confirmando que colaborador foi criado");

            Colaborador colaborador = Colaborador.GetColaboradorPorLogin(SessionTest, "anderson.lins");

            Assert.IsNotNull(colaborador, "colaborador não deve ser nulo");

            Assert.AreEqual("Anderson", colaborador.Usuario.FirstName, "Deveria ter criado o colaborador com o nome sendo a parte do login antes do ponto com primeira letra maiúscula");
            Assert.AreEqual("Lins", colaborador.Usuario.LastName, "Deveria ter criado o sobrenome com a parte do login após o ponto.");
            Assert.AreEqual("Anderson Lins", colaborador.Usuario.FullName, "Deveria ter criado o nome completo a partir do UserName passado, modificando a primeira letra para maiúscula e adicionando espaço entre o primeiro e último nome");
            Assert.AreEqual(DateUtil.CurrentDateTime, colaborador.DtAdmissao, "Deveria ter criado a data de admissão com a data corrente");
            Assert.AreEqual("anderson.lins", colaborador.Usuario.UserName, "Deveria ter criado o UserName a partir do parâmetro passado");
            Assert.AreEqual("anderson.lins@fpf.br", colaborador.Usuario.Email, "Deveria ter criado o email a partir do parâmetro passado e o formato de email da empresa");
            Assert.IsNotNull(colaborador.Usuario.Roles, "Deveria ter criado uma Role para o colaborador");
        }
    }
}