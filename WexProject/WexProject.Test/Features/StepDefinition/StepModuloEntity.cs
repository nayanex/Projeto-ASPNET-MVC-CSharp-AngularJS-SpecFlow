using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WexProject.BLL;
using WexProject.BLL.DAOs.Escopo;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Geral;
using WexProject.Test.Fixtures.Factory;
using WexProject.Test.Helpers;
using WexProject.Test.Helpers.BDD.Bind;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding, Scope( Tag = "Entity" )]
    public class StepModuloEntity : BaseEntityFrameworkTest
    {
        [Given( @"que existam os seguintes modulos para o '(.*)':" )]
        public void GivenQueExistamOsSeguintesModulosParaOProjeto( string nomeProjeto, Table table )
        {
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorNome( contexto, nomeProjeto );

            List<ModuloBindHelper> modulos = table.CreateSet<ModuloBindHelper>().ToList();
            
            for(int i = 0; i < modulos.Count; i++)
            {
                ModuloBindHelper bindHelper = modulos[i];
                if(string.IsNullOrWhiteSpace( bindHelper.ModuloPai ))
                    ModuloFactory.Criar( contexto, projeto, bindHelper.Nome, (uint)bindHelper.Tamanho );
                else 
                    ModuloFactory.CriarModuloFilho( contexto, ModuloDAO.ConsultarModuloPorNome( contexto, bindHelper.ModuloPai ), bindHelper );
            }
        }
    }
}
