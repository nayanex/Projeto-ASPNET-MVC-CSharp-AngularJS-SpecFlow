using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding]
    public class StepUsuario
    {
        [Given(@"usuario logado for '(.*)'"), When(@"usuario logado for '(.*)'")]
        public static void DadoUsuarioLogadoForColaborador01(string colaborador)
        {
            //atribui o colaborador como usuário atual logado.
            UsuarioDAO.CurrentUser = StepColaborador.ColaboradoresDic[colaborador].Usuario;
            //atribui o login do colaborador como login do usuário atual logado.
            UsuarioDAO.CurrentUser.UserName = StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName;
        }
    }
}
