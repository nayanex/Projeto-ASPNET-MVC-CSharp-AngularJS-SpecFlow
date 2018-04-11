using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.Test.Helpers.BDD.Bind;
using TechTalk.SpecFlow.Assist;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.DAOs.Geral;
using WexProject.Test.UnitTest;
using WexProject.BLL.DAOs.Escopo;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding, Scope(Tag="Entity")]
    public class StepEstoriaEntity : BaseEntityFrameworkTest
    {
        [Given( @"que existam as seguintes estorias para o '(.*)':" )]
        public void GivenQueExistamAsSeguintesEstoriasParaOProjeto01( string nomeProjeto, Table table )
        {
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorNome( contexto, nomeProjeto );

            List<EstoriaBindHelper> helpersEstoria = table.CreateSet<EstoriaBindHelper>().ToList();
            for(int i = 0; i < helpersEstoria.Count; i++)
            {
                EstoriaBindHelper helper = helpersEstoria[i];
                Beneficiado beneficiado = BeneficiadoDAO.ConsultarBeneficiadoPorNome( contexto, helper.ComoUm );
                if(beneficiado == null)
                    beneficiado = BeneficiadoFactoryEntity.Criar( contexto, helper.ComoUm );

                Modulo modulo = ModuloDAO.ConsultarModuloPorNome( contexto, helper.Modulo );

                EstoriaFactoryEntity.Criar( contexto, modulo, beneficiado, helper.Titulo, (uint)helper.Tamanho, helper.EmAnalise);
            }
        }

        [Given( @"que existam as seguintes estorias pai para o '(.*)':" )]
        public void GivenQueExistamAsSeguintesEstoriasPaiParaOProjeto( string projeto, Table table )
        {
            for(int position = 0; position < table.RowCount; position++)
            {
                string comoUm = table.Rows[position][table.Header.ToList()[0]];
                string titulo = table.Rows[position][table.Header.ToList()[1]];
                string nomeModulo = table.Rows[position][table.Header.ToList()[2]];
                uint tamanho = uint.Parse( table.Rows[position][table.Header.ToList()[3]] );
                string emAnalise = table.Rows[position][table.Header.ToList()[4]];
                string estoriaPai = table.Rows[position][table.Header.ToList()[5]];

                Beneficiado beneficiado = BeneficiadoDAO.ConsultarBeneficiadoPorNome( contexto, comoUm );

                if(beneficiado == null)
                    beneficiado = BeneficiadoFactoryEntity.Criar( contexto, comoUm );

                Estoria estoria;

                Modulo modulo = ModuloDAO.ConsultarModuloPorNome( contexto, nomeModulo );

                if(!estoriaPai.Equals( "" ))
                {
                    estoria = EstoriaFactoryEntity.Criar( contexto, modulo, beneficiado, titulo, tamanho, emAnalise, EstoriaDAO.ConsultarEstoriaPorNome( contexto, estoriaPai ));
                }
                else
                {
                    estoria = EstoriaFactoryEntity.Criar( contexto, modulo, beneficiado, titulo, tamanho, emAnalise );
                }
            }
        }
    }
}
