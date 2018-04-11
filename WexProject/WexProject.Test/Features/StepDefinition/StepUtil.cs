using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Util de Step
    /// </summary>
    [Binding]
    public class StepUtil : BaseTest
    {
        #region Given

        [Given(@"a data atual for '(.*)'")]
        public void DadoADataAtualFor20032012(string dataAtual)
        {
            DateUtil.CurrentDateTime = DateTime.Parse(dataAtual);
        }
        #endregion

        #region When
        [When(@"o usuario '(.*)' selecionar o projeto '([\w\s]+)'")]
        public void QuandoOUsuarioSelecionarOProjeto(string colaborador, string projeto)
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
        #endregion

        #region Then

        #endregion
    }
}
