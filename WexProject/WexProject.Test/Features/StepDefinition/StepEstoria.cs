using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Escopo;
using WexProject.Test.Fixtures.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Execucao;
using DevExpress.Xpo;
using System.Collections;
using WexProject.BLL.Shared.Domains.Escopo;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Estoria
    /// </summary>
    [Binding]
    public class StepEstoria : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Estorias usados no Step
        /// </summary>
        public static Dictionary<string, Estoria> EstoriasDic { get; set; }

        public Estoria estoriaOld;

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            EstoriasDic = new Dictionary<string, Estoria>();        
            estoriaOld = null;
        }
        #endregion

        #region Given

        [Given(@"que existam as seguintes estorias para o '(.*)':")]
        public void GivenQueExistamAsSeguintesEstoriasParaOProjeto01(string projeto, Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string comoUm = table.Rows[position][table.Header.ToList()[0]];
                string titulo = table.Rows[position][table.Header.ToList()[1]];
                string modulo = table.Rows[position][table.Header.ToList()[2]];
                uint tamanho = uint.Parse(table.Rows[position][table.Header.ToList()[3]]);
                string emAnalise = table.Rows[position][table.Header.ToList()[4]];

                if (!StepBeneficiado.BeneficiadosDic.ContainsKey(comoUm))
                {
                    StepBeneficiado.addBeneficiado(comoUm);
                }

                Estoria e = EstoriaFactory.Criar(SessionTest, StepModulo.ProjetoModulosDic[projeto][modulo], StepBeneficiado.BeneficiadosDic[comoUm], titulo, tamanho, emAnalise, true);

                EstoriasDic.Add(e.TxTitulo, e);
            }
        }

        [Given(@"que existam as seguintes estorias pai para o '(.*)':")]
        public void GivenQueExistamAsSeguintesEstoriasPaiParaOProjeto(string projeto, Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string comoUm = table.Rows[position][table.Header.ToList()[0]];
                string titulo = table.Rows[position][table.Header.ToList()[1]];
                string modulo = table.Rows[position][table.Header.ToList()[2]];
                uint tamanho = uint.Parse(table.Rows[position][table.Header.ToList()[3]]);
                string emAnalise = table.Rows[position][table.Header.ToList()[4]];
                string estoriaPai = table.Rows[position][table.Header.ToList()[5]];

                if (!StepBeneficiado.BeneficiadosDic.ContainsKey(comoUm))
                {
                    StepBeneficiado.addBeneficiado(comoUm);
                }

                Estoria e;
                if (!estoriaPai.Equals(""))
                {
                    e = EstoriaFactory.Criar(SessionTest, StepModulo.ProjetoModulosDic[projeto][modulo], StepBeneficiado.BeneficiadosDic[comoUm], titulo, tamanho, emAnalise, true, StepEstoria.EstoriasDic[estoriaPai]);
                }
                else
                {
                    e = EstoriaFactory.Criar(SessionTest, StepModulo.ProjetoModulosDic[projeto][modulo], StepBeneficiado.BeneficiadosDic[comoUm], titulo, tamanho, emAnalise, true);
                }         

                EstoriasDic.Add(e.TxTitulo, e);
            }
        }


        [Given(@"a estoria '(.*)' com as seguintes propriedades:")]
        public void DadoAEstoriaEstoria01ComAsSeguintesPropriedades(string estoria, Table table)
        {
            string titulo = table.Rows[0][table.Header.ToList()[1]];
            string estoriaPai = table.Rows[1][table.Header.ToList()[1]];
            string comoUm = table.Rows[2][table.Header.ToList()[1]];
            string solicitante = table.Rows[3][table.Header.ToList()[1]];
            string modulo = table.Rows[4][table.Header.ToList()[1]];

            Estoria est = EstoriaFactory.Criar(SessionTest,
                    StepModulo.ModulosDic[modulo],
                    titulo, string.Empty, string.Empty,
                    StepBeneficiado.BeneficiadosDic[comoUm],
                    string.Empty, string.Empty, string.Empty, false);

            if (!string.IsNullOrEmpty(estoriaPai))
        {
                est.EstoriaPai = EstoriasDic[estoriaPai];
        }

            if (!string.IsNullOrEmpty(solicitante))
        {
                est.ProjetoParteInteressada = StepProjetoParteInteressada.PartesInteressadasDic[solicitante];
        }

            est.Save();

            EstoriasDic.Add(estoria, est);
        }

        #endregion

        #region When

        [When(@"alterar a pontuacao da estoria '(.*)' do projeto '(.*)'")]
        public void QuandoAlterarAPontuacaoDaEstoriaEstoria01(string estoria, string projeto)
        {
             ICollection lista = Estoria.GetEstoriasDoProjeto(SessionTest, StepProjeto.ProjetosDic[projeto]);
             foreach (var item in lista)
             {
                 if ((item as Estoria).TxTitulo.Contains(estoria))
                 {
                     (item as Estoria).NbTamanho = EstoriasDic[estoria].NbTamanho + 1;
                     (item as Estoria).Save();
                 }
             }
        }

        [When(@"clicar em salvar e novo da estoria '(.*)'")]
        public void QuandoClicarEmSalvarENovoDaEstoriaEstoria01(string estoria)
        {
            estoriaOld = EstoriasDic[estoria];
        }

        #endregion

        #region Then

        [Then(@"as estorias do projeto '(.*)' devem estar com as seguintes situacoes:")]
        public void EntaoAsEstoriasDevemEstarComAsSeguintesSituacoes(string projeto, Table table)
        {
            string estoriaCol = table.Header.ToList()[0];
            string situacaoCol = table.Header.ToList()[1];
            string quandoCol = table.Header.ToList()[2];

            for (int position = 0; position < table.RowCount; position++)
            {
                string estoriaRow = table.Rows[position][estoriaCol];
                string situacaoRow = table.Rows[position][situacaoCol];
                string quandoRow = table.Rows[position][quandoCol];

                ICollection lista = Estoria.GetEstoriasDoProjeto(SessionTest, StepProjeto.ProjetosDic[projeto]);
                foreach (var item in lista)
                {
                    if ((item as Estoria).TxTitulo.Contains(estoriaRow))
                    {
                        Assert.AreEqual(situacaoRow, (item as Estoria).CsSituacao.ToString());
                        Assert.AreEqual(quandoRow, (item as Estoria)._TxQuando);
                    }
                }              
            }
        }

      

        [Then(@"a nova estoria deve estar com as seguintes propiedades:")]
        public void EntaoANovaEstoriaDeveEstarComAsSeguintesPropiedades(Table table)
        {
            string estoriaPai = table.Rows[1][table.Header.ToList()[1]];
            string comoUm = table.Rows[2][table.Header.ToList()[1]];
            string solicitante = table.Rows[3][table.Header.ToList()[1]];
            string modulo = table.Rows[4][table.Header.ToList()[1]];

            Estoria current = new Estoria(SessionTest);
            Estoria.GetDadosEstoriaCurrent(SessionTest, current, estoriaOld);

            Assert.AreEqual(EstoriasDic[estoriaPai], current.EstoriaPai);
            Assert.AreEqual(StepBeneficiado.BeneficiadosDic[comoUm], current.ComoUmBeneficiado);
            Assert.AreEqual(StepProjetoParteInteressada.PartesInteressadasDic[solicitante], current.ProjetoParteInteressada);
            Assert.AreEqual(StepModulo.ModulosDic[modulo], current.Modulo);
        }


        [Then(@"as estorias do projeto '(.*)' devem estar com a situacao")]
        public void EntaoAsEstoriasDoProjetoTesteDevemEstarComASituacao(string projeto, Table table)
        {
            Projeto proj = StepProjeto.ProjetosDic[projeto];
            for (int position = 0; position < table.RowCount; position++)
            {
                string estoriaRow = table.Rows[position][0];
                string situacaoRow = table.Rows[position][1];
                
                foreach (Modulo mod in proj.Modulos)
                {
                    bool quebra = false;
                    foreach (Estoria est in mod.Estorias)
                    {
                        if (est.TxTitulo.Contains(estoriaRow))
                        {
                            Assert.AreEqual(SituacaoEstoriaByText(situacaoRow), est.CsSituacao);
                            quebra = true;
                            break;
                        }
                    }
                    if (quebra) { break; }
                }
            }
        }


        #endregion

        /// <summary>
        /// Retorna a situação da Estória a partir do valor texto da mesma
        /// </summary>
        /// <param name="situacao">Valor texto da situação</param>
        public static CsEstoriaDomain SituacaoEstoriaByText(string situacao)
        {
            CsEstoriaDomain retorno = CsEstoriaDomain.NaoIniciado;

            switch (situacao)
            {
                case "Não Iniciado":
                    retorno = CsEstoriaDomain.NaoIniciado;
                    break;

                case "Em Analise":
                    retorno = CsEstoriaDomain.EmAnalise;
                    break;

                case "Replanejado":
                    retorno = CsEstoriaDomain.Replanejado;
                    break;

                case "Em Desenvolvimento":
                    retorno = CsEstoriaDomain.EmDesenv;
                    break;

                case "Pronto":
                    retorno = CsEstoriaDomain.Pronto;
                    break;

                case "Cancelado":
                    retorno = CsEstoriaDomain.Cancelado;
                    break;

                default:
                    new Exception("Situação da Estória não encontrada.");
                    break;
            }

            return retorno;
        }
    }
}