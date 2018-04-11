using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using WexProject.BLL.Models.Execucao;
using System.Collections;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Rh;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System.Drawing;
using WexProject.BLL.Shared.DTOs.Escopo;
using WexProject.BLL.Shared.DTOs.Execucao;
using WexProject.BLL.BOs.Escopo;
using WexProject.BLL.BOs.Execucao;
using WexProject.BLL.DAOs;
using WexProject.Library.Libs.Enumerator;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.BLL.Shared.Domains.Escopo;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Rh;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Projeto
    /// </summary>
    [Binding]
    class StepProjeto : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Projetos usados no Step
        /// </summary>
        public static Dictionary<string, Projeto> ProjetosDic { get; set; }

        public static Dictionary<string, List<GraficoEstimadoRealizadoDTO>>
            DadosGraficoEstimadoRealizadoProjetoDic { get; set; }

        public static Dictionary<string, List<GraficoRitmoTimeDTO>>
            DadosGraficoRitmoTimeProjetoDic { get; set; }

        public static Dictionary<string, List<GraficoEscopoCompletudeDTO>>
         DadosGraficoEscopoCompletudeDic { get; set; }

        private Hashtable dados { get; set; }

        private static Dictionary<string, string> CoresDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas (antes dos cenários)
        /// </summary>
        [BeforeScenario]
        public static void ReiniciarValores()
        {
            ProjetosDic = new Dictionary<string, Projeto>();
            DadosGraficoEstimadoRealizadoProjetoDic = new Dictionary<string, List<GraficoEstimadoRealizadoDTO>>();
            DadosGraficoRitmoTimeProjetoDic = new Dictionary<string, List<GraficoRitmoTimeDTO>>();            
            DadosGraficoRitmoTimeProjetoDic = new Dictionary<string, List<GraficoRitmoTimeDTO>>();
            DadosGraficoEscopoCompletudeDic = new Dictionary<string, List<GraficoEscopoCompletudeDTO>>();
            CoresDic = new Dictionary<string, string>();
        }

        [BeforeScenario]
        public static void InstanciaCores()
        {
            CoresDic.Add("preto", Color.Black.ToString());
            CoresDic.Add("preta", Color.Black.ToString());

        }

        #endregion

        #region Given

        [Given(@"que o projeto '([\w\s]+)' tenha escolhido a cor '([\w\s]+)' para o colaborador '([\w\s]+)'")]
        public static void DadoQueOProjetoTenhaEscolhidoACorParaOColaborador(string projeto, string cor, string colaborador)
        {
            ProjetoColaboradorConfig projColConfig = new ProjetoColaboradorConfig(SessionTest);
            projColConfig.Colaborador = StepColaborador.ColaboradoresDic[colaborador];
            projColConfig.Projeto = StepProjeto.ProjetosDic[projeto];
            projColConfig.Cor = CoresDic[cor];
            projColConfig.Save();
        }


        /// <summary>
        /// Dado que exista(m) o(s) projeto(s) a seguir:
        /// | nome       | tamanho | total de ciclos | ritmo do time |
        /// | projeto 01 | 50      | 1               | 10            |
        /// </summary>
        /// <param name="table">tabela com nome, tamanho, total de ciclos e ritmo do time</param>
        [Given(@"que exista\(m\) o\(s\) projeto\(s\) a seguir:")]
        public static void DadoQueExistaMOSProjetoSASeguir(Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string nome = table.Rows[position][table.Header.ToList()[0]];
                string tamanho = table.Rows[position][table.Header.ToList()[1]];
                string totalCiclos = table.Rows[position][table.Header.ToList()[2]];
                string ritmoTime = table.Rows[position][table.Header.ToList()[3]];

                Projeto p = ProjetoFactory.CriarProjetoRitmo(SessionTest, UInt32.Parse(tamanho), nome, true, ushort.Parse(totalCiclos), ushort.Parse(ritmoTime));
                p.DtInicioReal = DateTime.Now;
                p.DtInicioPlan = DateTime.Now;
                p.Save();
                ProjetosDic.Add(nome, p);
            }
        }

        [Given(@"o projeto '([\w\s]+)' tenha seu tamanho alterado para '(.*)'")]
        public static void DadoOProjetoATenhaSeuTamanhoAlteradoPara100(string projeto, ushort tamanhoTotal, Table table)
        {
            ProjetosDic[projeto].Reload();
            ProjetosDic[projeto].NbTamanhoTotal = tamanhoTotal;
            ProjetosDic[projeto].Save();
        }

        [Given(@"ciclo '([\w\s]+)' do projeto '([\w\s]+)' na situação '([\w\s]+)'$")]
        public static void DadoCiclo5DoProjetoProjeto01NaSituacaoNaoPlanejado(string ciclo, string projeto, string situacao)
        {
            PropertyInfo info = typeof(CicloDesenv).GetProperty("CsSituacaoCiclo");
            info.SetValue(ProjetosDic[projeto].Ciclos[int.Parse(ciclo) - 1], EnumUtil.ValueEnum(typeof(CsSituacaoCicloDomain), situacao), null);
            ProjetosDic[projeto].Ciclos[int.Parse(ciclo) - 1].Save();
        }

        [Given(@"ciclo '([\w\s]+)' do projeto '([\w\s]+)' na situação '([\w\s]+)' com as estórias:")]
        public static void DadoCiclo3DoProjetoProjeto01NaSituacaoConcluidoComAsEstorias(string ciclo, string projeto, string situacao, Table table)
        {
            string estoriaCol = table.Header.ToList()[0];
            string situacaoCol = table.Header.ToList()[1];
            string pontosCol = table.Header.ToList()[2];
            string tipoCol = table.Header.ToList()[3];
            string valorNegocioCol = table.Header.ToList()[4];

            Modulo modulo = ModuloFactory.Criar(SessionTest, ProjetosDic[projeto], string.Format("Módulo {0}", projeto), true);

            for (int position = 0; position < table.RowCount; position++)
            {
                string estoriaRow = table.Rows[position][estoriaCol];
                string situacaoRow = table.Rows[position][situacaoCol];
                string pontosRow = table.Rows[position][pontosCol];
                string tipoRow = table.Rows[position][tipoCol];
                string valorNegocioRow = table.Rows[position][valorNegocioCol];
                Estoria estoria = null;
                if (!StepEstoria.EstoriasDic.ContainsKey(estoriaRow))
                {
                    estoria = EstoriaFactory.Criar(SessionTest, modulo, estoriaRow, "", "", BeneficiadoFactory.Criar(SessionTest, "bene 1" + estoriaRow, true), "", "", "", false);
                    estoria.NbTamanho = double.Parse(pontosRow);
                    StepEstoria.EstoriasDic.Add(estoriaRow, estoria);
                }
                else
                {
                    estoria = StepEstoria.EstoriasDic[estoriaRow];
                }
                PropertyInfo info = typeof(Estoria).GetProperty("CsTipo");
                info.SetValue(estoria, EnumUtil.ValueEnum(typeof(CsTipoEstoriaDomain), tipoRow), null);

                info = typeof(Estoria).GetProperty("CsValorNegocio");
                info.SetValue(estoria, EnumUtil.ValueEnum(typeof(CsValorNegocioDomain), valorNegocioRow), null);

                estoria.Save();

                CicloDesenvEstoria cicloDesenvEstoria = CicloDesenvEstoriaFactory.Criar(SessionTest, ProjetosDic[projeto].Ciclos[int.Parse(ciclo) - 1], estoria, false);
                info = typeof(CicloDesenvEstoria).GetProperty("CsSituacao");
                info.SetValue(cicloDesenvEstoria, EnumUtil.ValueEnum(typeof(CsSituacaoEstoriaCicloDomain), situacaoRow), null);
                cicloDesenvEstoria.Save();

            }
            DadoCiclo5DoProjetoProjeto01NaSituacaoNaoPlanejado(ciclo, projeto, situacao);
        }

        [Given(@"ciclo '([\w\s]+)' do projeto '([\w\s]+)' na situação '([\w\s]+)' com as estórias e metas:")]
        public static void GivenCiclo1DoProjetoProjeto01NaSituacaoConcluidoComAsEstoriasEMetas(string ciclo, string projeto, string situacao, Table table)
        {
            string estoriaCol = table.Header.ToList()[0];
            string situacaoCol = table.Header.ToList()[1];
            string pontosCol = table.Header.ToList()[2];
            string tipoCol = table.Header.ToList()[3];
            string valorNegocioCol = table.Header.ToList()[4];
            string metaCol = table.Header.ToList()[5];

            Modulo modulo = ModuloFactory.Criar(SessionTest, ProjetosDic[projeto], string.Format("Módulo {0}", projeto), true);

            for (int position = 0; position < table.RowCount; position++)
            {
                string estoriaRow = table.Rows[position][estoriaCol];
                string situacaoRow = table.Rows[position][situacaoCol];
                string pontosRow = table.Rows[position][pontosCol];
                string tipoRow = table.Rows[position][tipoCol];
                string valorNegocioRow = table.Rows[position][valorNegocioCol];
                string metaRow = table.Rows[position][metaCol];

                Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, estoriaRow, "", "", BeneficiadoFactory.Criar(SessionTest, "bene 1" + estoriaRow, true), "", "", "", false);
                estoria.NbTamanho = double.Parse(pontosRow);

                PropertyInfo info = typeof(Estoria).GetProperty("CsTipo");
                info.SetValue(estoria, EnumUtil.ValueEnum(typeof(CsTipoEstoriaDomain), tipoRow), null);

                info = typeof(Estoria).GetProperty("CsValorNegocio");
                info.SetValue(estoria, EnumUtil.ValueEnum(typeof(CsValorNegocioDomain), valorNegocioRow), null);

                estoria.Save();

                CicloDesenvEstoria cicloDesenvEstoria = CicloDesenvEstoriaFactory.CriarComMeta(SessionTest, ProjetosDic[projeto].Ciclos[int.Parse(ciclo) - 1], estoria, metaRow, false);
                info = typeof(CicloDesenvEstoria).GetProperty("CsSituacao");
                info.SetValue(cicloDesenvEstoria, EnumUtil.ValueEnum(typeof(CsSituacaoEstoriaCicloDomain), situacaoRow), null);
                cicloDesenvEstoria.Save();

            }
            DadoCiclo5DoProjetoProjeto01NaSituacaoNaoPlanejado(ciclo, projeto, situacao);
        }


        [Given(@"existam as seguintes partes interessadas para o projeto '([\w\s]+)':")]
        public static void GivenExistamAsSeguintesPartesInteressadasParaOProjetoProjeto01(string projeto, Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string colab = table.Rows[position][0];
                string cargo = table.Rows[position][1];

                if (!StepCargo.CargoDic.ContainsKey(cargo))
                {
                    StepCargo.CreateCargo(cargo);
                }
                ProjetosDic[projeto].ParteInteressada.Add(ParteInteressadaFactory.Criar(SessionTest, CsSimNao.Sim, StepColaborador.ColaboradoresDic[colab], StepCargo.CargoDic[cargo], true));
            }
        }

        [Given(@"que o projeto '([\w\s]+)' tenha sido selecionado anteriormente pelo usuario '(.*)'")]
        public void DadoQueOProjetoTenhaSidoSelecionadoAnteriormentePeloUsuario(string projeto, string colaborador)
        {
            //atribui o oid na propriedade do ultimo filtro.
            StepColaborador.ColaboradoresDic[colaborador].ColaboradorUltimoFiltro.OidUltimoProjetoSelecionado = StepProjeto.ProjetosDic[projeto].Oid;
            Projeto.SelectedProject = StepProjeto.ProjetosDic[projeto].Oid;
        }

        [Given(@"que nenhum projeto tenha sido selecionado anteriormente pelo usuario '(.*)'")]
        public void DadoQueNenhumProjetoTenhaSidoSelecionadoAnteriormentePeloUsuario(string colaborador)
        {
            //atribui o oid na propriedade do ultimo filtro.
            StepColaborador.ColaboradoresDic[colaborador].ColaboradorUltimoFiltro.OidUltimoProjetoSelecionado = new Guid();
        }
        #endregion

        #region Then

        [Then(@"os dados para o gráfico estimado vs realizado do projeto '([\w\s]+)' devem ser:")]
        public void EntaoOsDadosParaOGraficoEstimadoVsRealizadoDoProjetoProjeto01DevemSer(string projeto, Table table)
        {
            string cicloCol = table.Header.ToList()[0];
            string estimadoCol = table.Header.ToList()[1];
            string realizadoCol = table.Header.ToList()[2];
            string tendenciaCol = table.Header.ToList()[3];
            string ritimoSugeridoCol = table.Header.ToList()[4];

            Assert.IsTrue(DadosGraficoEstimadoRealizadoProjetoDic.ContainsKey(projeto),
                "O projeto deveria existir no dicionário");

            Assert.AreEqual(table.RowCount, DadosGraficoEstimadoRealizadoProjetoDic[projeto].Count(),
                "As quantidades de registros deveriam ser as mesmas " + table.RowCount + " : " + DadosGraficoEstimadoRealizadoProjetoDic[projeto].Count());

            List<GraficoEstimadoRealizadoDTO> lista = DadosGraficoEstimadoRealizadoProjetoDic[projeto];
            for (int position = 0; position < table.RowCount; position++)
            {
                Assert.AreEqual(table.Rows[position][cicloCol], lista[position].Ciclo.ToString());
                Assert.AreEqual(table.Rows[position][estimadoCol], lista[position].Estimado.ToString());
                Assert.AreEqual(table.Rows[position][realizadoCol], lista[position].Realizado.ToString());
                Assert.AreEqual(table.Rows[position][tendenciaCol], lista[position].Tendencia.ToString());
                Assert.AreEqual(table.Rows[position][ritimoSugeridoCol], lista[position].RitimoSugerido.ToString());
            }
        }


        [Then(@"os dados para o gráfico de ritmo do time do projeto '([\w\s]+)' devem ser:")]
        public void EntaoOsDadosParaOGraficoDeRitmoDoTimeDoProjetoProjeto01DevemSer(string projeto, Table table)
        {
            string cicloCol = table.Header.ToList()[0];
            string ritmoCol = table.Header.ToList()[1];
            string metaCol = table.Header.ToList()[2];
            string planejadoCol = table.Header.ToList()[3];

            Assert.IsTrue(DadosGraficoRitmoTimeProjetoDic.ContainsKey(projeto),
                "O projeto deveria existir no dicionário");

            Assert.AreEqual(table.RowCount, DadosGraficoRitmoTimeProjetoDic[projeto].Count(),
                "As quantidades de registros deveriam ser as mesmas");

            List<GraficoRitmoTimeDTO> lista = DadosGraficoRitmoTimeProjetoDic[projeto];
            for (int position = 0; position < table.RowCount; position++)
            {
                Assert.AreEqual(table.Rows[position][cicloCol], lista[position].Ciclo.ToString());
                Assert.AreEqual(table.Rows[position][ritmoCol], lista[position].Ritmo.ToString());
                Assert.AreEqual(table.Rows[position][metaCol], lista[position].Meta.ToString());
                Assert.AreEqual(table.Rows[position][planejadoCol], lista[position].Planejado.ToString());
            }
        }

        [Then(@"os dados para o grafico de escopo vs completude do projeto '([\w\s]+)' devem ser:")]
        public void EntaoOsDadosParaOGraficoDeEscopoVsCompletudeDoProjetoProjeto01DevemSer(string projeto, Table table)
        {
            string moduloCol = table.Header.ToList()[0];
            string pontosNaoIniciadosCol = table.Header.ToList()[1];
            string percNaoIniciadosCol = table.Header.ToList()[2];
            string pontoEmAnaliseCol = table.Header.ToList()[3];
            string percEmAnaliseCol = table.Header.ToList()[4];
            string pontoDesenvCol = table.Header.ToList()[5];
            string percDesenvCol = table.Header.ToList()[6];
            string pontosProntosCol = table.Header.ToList()[7];
            string percProntosCol = table.Header.ToList()[8];
            string pontosDesvioCol = table.Header.ToList()[9];
            string percDesvioCol = table.Header.ToList()[10];
            string pontosMudancaCol = table.Header.ToList()[11];
            string percMudancaCol = table.Header.ToList()[12];
            string totalModuloCol = table.Header.ToList()[13];

            Assert.IsTrue(DadosGraficoEscopoCompletudeDic.ContainsKey(projeto),
                "O projeto deveria existir no dicionário");
            Assert.AreEqual(table.RowCount, DadosGraficoEscopoCompletudeDic[projeto].Count(),
                "As quantidades de registros deveriam ser as mesmas");

            List<GraficoEscopoCompletudeDTO> lista = DadosGraficoEscopoCompletudeDic[projeto];
            for (int position = 0; position < table.RowCount; position++)
            {
                Assert.AreEqual(table.Rows[position][moduloCol], lista[position].Modulo);

                Assert.AreEqual(double.Parse(table.Rows[position][pontosNaoIniciadosCol]), lista[position].PontoNaoIniciados);
                Assert.AreEqual(double.Parse(table.Rows[position][percNaoIniciadosCol]), lista[position].PercNaoInciado);
                Assert.AreEqual(double.Parse(table.Rows[position][pontoEmAnaliseCol]), lista[position].PontosEmAnalise);
                Assert.AreEqual(double.Parse(table.Rows[position][percEmAnaliseCol]), lista[position].PercEmAnalise);
                Assert.AreEqual(double.Parse(table.Rows[position][pontoDesenvCol]), lista[position].PontosEmDesenv);
                Assert.AreEqual(double.Parse(table.Rows[position][percDesenvCol]), lista[position].PercEmDesenv);
                Assert.AreEqual(double.Parse(table.Rows[position][pontosProntosCol]), lista[position].PontosProntos);
                Assert.AreEqual(double.Parse(table.Rows[position][percProntosCol]), lista[position].PercProntos);
                Assert.AreEqual(double.Parse(table.Rows[position][pontosDesvioCol]), lista[position].PontosDesvio);
                Assert.AreEqual(double.Parse(table.Rows[position][percDesvioCol]), lista[position].PercDesvio);
                Assert.AreEqual(double.Parse(table.Rows[position][pontosMudancaCol]), lista[position].PontosMudanca);
                Assert.AreEqual(double.Parse(table.Rows[position][percMudancaCol]), lista[position].PercMudanca);
                Assert.AreEqual(double.Parse(table.Rows[position][totalModuloCol]), lista[position].TotalPontosModulo);
            }
        }

        [Then(@"o projeto '([\w\s]+)' deveria ter sido salvo como ultimo projeto selecionado para o usuario '(.*)'")]
        public void EntaoOProjetoDeveriaTerSidoSalvoComoUltimoProjetoSelecionadoParaOUsuario(string projeto, string colaborador)
        {
            Projeto projetoAtual = ProjetosDic[projeto];

            Projeto ultimoProjetoSelecionado = ColaboradorUltimoFiltro.GetUltimoProjetoSelecionadoPorColaborador(SessionTest, StepColaborador.ColaboradoresDic[colaborador].Oid);

            Assert.AreEqual(projetoAtual.Oid, ultimoProjetoSelecionado.Oid, "Os projetos devem ser os mesmos.");
        }

        [Then(@"o projeto '([\w\s]+)' deve ficar preenchido automaticamente")]
        public void EntaoOProjetoDeveFicarPreenchidoAutomaticamente(string projeto)
        {
            string colaborador = UsuarioDAO.CurrentUser.UserName;

            Projeto ultimoProjetoSelecionado = ColaboradorUltimoFiltro.GetUltimoProjetoSelecionadoPorColaborador( SessionTest, StepColaborador.ColaboradoresDic[colaborador].Oid );

            Assert.AreEqual(projeto, ultimoProjetoSelecionado.TxNome);

            Projeto.SelectedProject = ultimoProjetoSelecionado.Oid;
        }

        [Then(@"o filtro de projeto deve ficar em branco")]
        public static void EntaoOFiltroDeProjetoDeveFicarEmBranco()
        {
            //recupera colaborador logado.
            string colaborador = UsuarioDAO.CurrentUser.UserName;
            //recupera último projeto selecionado.
            Guid oidUltimoProjetoSelecionado = StepColaborador.ColaboradoresDic[colaborador].ColaboradorUltimoFiltro.OidUltimoProjetoSelecionado;
            //compara
            Assert.AreEqual(new Guid(), oidUltimoProjetoSelecionado);
        }

        [Then(@"o projeto '([\w\s]+)' devera ter escolhido uma cor aleatoria para o colaborador '([\w\s]+)'")]
        public static void EntaoOProjetoDeveraTerEscolhidoUmaCorAleatoriaParaOColaborador(string projeto, string colaborador)
        {
            string cor = null;
            ProjetoColaboradorConfig projColConfigResult;

            
            projColConfigResult = ProjetoColaboradorConfig.GetProjetoColaboradorConfig(SessionTest, StepColaborador.ColaboradoresDic[colaborador].Oid, ProjetosDic[projeto].Oid);
            
            Assert.IsNotNull(projColConfigResult, "Deveria existir um objeto no retorno da pesquisa");
            cor = projColConfigResult.Cor;
            Assert.IsNotNull(cor, "Deveria ter criado uma cor.");
        }

        [Then(@"o projeto '(.*)' devera ter escolhido a cor '(.*)' para o colaborador '(.*)'")]
        public static void EntaoOProjetoDeveraTerEscolhidoACorParaOColaborador(string projeto, string cor, string colaborador)
        {
            string corColaborador;
            ProjetoColaboradorConfig projColConfigResult;

            
            projColConfigResult = ProjetoColaboradorConfig.GetProjetoColaboradorConfig(SessionTest, StepColaborador.ColaboradoresDic[colaborador].Oid, ProjetosDic[projeto].Oid);

            Assert.IsNotNull(projColConfigResult, "Deveria existir um objeto.");
            corColaborador = projColConfigResult.Cor;
            Assert.AreEqual(CoresDic[cor], corColaborador);
        }

        [Then(@"o projeto '(.*)' nao deve ter escolhido a cor '(.*)' para o colaborador '(.*)'")]
        public static void EntaoOProjetoNaoDeveTerEscolhidoACorParaOColaborador(string projeto, string cor, string colaborador)
        {
            string corEscolhida;

            XPCollection<ProjetoColaboradorConfig> xpCorColaborador = new XPCollection<ProjetoColaboradorConfig>(SessionTest,
                CriteriaOperator.Parse(" Projeto = ? AND Colaborador = ?", StepProjeto.ProjetosDic[projeto].Oid,
                StepColaborador.ColaboradoresDic[colaborador].Oid));

            corEscolhida = xpCorColaborador[0].Cor;

            Assert.AreNotEqual(CoresDic[cor], corEscolhida);
        }




        #endregion
        #region When

        [When(@"montar os dados necessários para o gráfico de estimado vs realizado do projeto '([\w\s]+)'")]
        public static void QuandoMontarOsDadosNecessariosParaOGraficoDeEstimadoVsRealizadoDoProjetoProjeto01(string projeto)
        {
            List<GraficoEstimadoRealizadoDTO> result = GraficoEstimadoRealizadoBO.CalcularGraficoEstimadoVsRealizadoProjeto( StepProjeto.ProjetosDic[projeto].Oid, SessionTest );
            DadosGraficoEstimadoRealizadoProjetoDic.Add( projeto, result );
        }

        [When(@"montar os dados necessários para o gráfico de ritmo do time do projeto '([\w\s]+)'")]
        public static void QuandoMontarOsDadosNecessariosParaOGraficoDeRitmoDoTimeDoProjetoProjeto01(string projeto)
        {
            List<GraficoRitmoTimeDTO> result = GraficoRitmoTimeBO.CalcularGraficoRitmoTimeProjeto(StepProjeto.ProjetosDic[projeto].Oid, SessionTest);
            DadosGraficoRitmoTimeProjetoDic.Add(projeto, result);
        }

        [When(@"montar os dados necessarios para o grafico de escopo vs completude do projeto '([\w\s]+)'")]
        public static void QuandoMontarOsDadosNecessariosParaOGraficoDeEscopoVsCompletudeDoProjeto(string projeto)
        {
            List<GraficoEscopoCompletudeDTO> result = GraficoEscopoCompletudeBO.CalcularGraficoEscopoCompletude(StepProjeto.ProjetosDic[projeto].Oid, SessionTest);
            DadosGraficoEscopoCompletudeDic.Add(projeto, result);
        }

        [When(@"o projeto '([\w\s]+)' for salvo:")]
        public static void QuandoOProjetoAForSalvo(string projeto)
        {
            ProjetosDic[projeto].Save();
        }

        [When(@"o projeto '([\w\s]+)' for selecionado pelo usuario '(.*)'")]
        public static void QuandoOProjetoForSelecionadoPeloUsuario(string projeto, string colaborador)
        {
            //seta o projeto atual.
            Projeto.SelectedProject = StepProjeto.ProjetosDic[projeto].Oid;

            //seta propriedade de último projeto.
            StepColaborador.ColaboradoresDic[colaborador].ColaboradorUltimoFiltro.OidUltimoProjetoSelecionado = StepProjeto.ProjetosDic[projeto].Oid;

            //recupera atual projeto selecionado.
            Projeto projetoSelecionado = Projeto.GetProjetoAtual(SessionTest);

            //salva como filtro de último projeto.
            StepColaborador.ColaboradoresDic[colaborador].Save();

        }

        [When(@"o projeto '(.*)' escolher uma cor aleatoria para o colaborador '(.*)'")]
        public static void QuandoOProjetoEscolherUmaCorAleatoriaParaOColaborador(string projeto, string colaborador)
        {
            ProjetoColaboradorConfig.RnEscolherCor(SessionTest, StepColaborador.ColaboradoresDic[colaborador].Oid, 
                StepProjeto.ProjetosDic[projeto].Oid);
        }

        [When(@"o projeto '(.*)' selecionar a cor escolhida anteriormente para o colaborador '(.*)'")]
        public static void QuandoOProjetoSelecionarACorEscolhidaAnteriormenteParaOColaborador(string projeto, string colaborador)
        {
            ProjetoColaboradorConfig projColConfigResult;

            
            projColConfigResult = ProjetoColaboradorConfig.GetProjetoColaboradorConfig(SessionTest, StepColaborador.ColaboradoresDic[colaborador].Oid, ProjetosDic[projeto].Oid);

        }
        #endregion

        #region Util
        public static Projeto CriarProjeto(uint tamanho, string projeto, Boolean save)
        {
            Projeto p = ProjetoFactory.Criar(SessionTest, tamanho, projeto, save);
            ProjetosDic.Add(projeto, p);
            return p;
        }
        #endregion
    }

}