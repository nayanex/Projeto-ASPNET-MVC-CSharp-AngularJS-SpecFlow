using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.BLL.DAOs.Execucao;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Test.Helpers.BDD.Bind;
using WexProject.Test.UnitTest;
using TechTalk.SpecFlow.Assist;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.DAOs.Escopo;
using WexProject.Test.Fixtures.Factory;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding, Scope( Tag = "Entity" )]
    public class StepCicloEntity : BaseEntityFrameworkTest
    {
        [Given( @"que o ciclo '(.*)' do projeto '(.*)' esteja com situacao '(.*)' com as estorias:" )]
        public void GivenQueOCiclo1DoProjetoProjeto01EstejaComSituacaoEmAndamentoComAsEstorias( int numeroCiclo, string nomeProjeto, CsSituacaoCicloDomain situacaoCiclo, Table table )
        {
            List<EstoriaBindHelper> estorias = table.CreateSet<EstoriaBindHelper>().ToList();
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorNome( contexto, nomeProjeto );

            List<CicloDesenv> ciclos = CicloDesenvDAO.ConsultarCiclosDesenvDoProjeto( contexto, projeto.Oid );

            CicloDesenv ciclo = ciclos[numeroCiclo - 1];
            ciclo.CsSituacaoCiclo = (int)situacaoCiclo;
            CicloDesenvDAO.SalvarCicloDesenv( contexto, ciclo );

            for(int i = 0; i < estorias.Count; i++)
            {
                Estoria estoria = EstoriaDAO.ConsultarEstoriaPorNome( contexto, estorias[i].Titulo );
                CicloDesenvEstoria estoriaCiclo = CicloDesenvEstoriaFactoryEntity.Criar( contexto, ciclo, estoria, estorias[i].Situacao );
            }
        }

        #region Métodos Auxiliares

        /// <summary>
        /// Retorna a situação do Ciclo a partir do valor texto da mesma
        /// </summary>
        /// <param name="situacao">Valor texto da situação</param>
        public static CsSituacaoCicloDomain SituacaoCicloByText( string situacao )
        {
            CsSituacaoCicloDomain retorno = CsSituacaoCicloDomain.NaoPlanejado;

            switch(situacao)
            {
                case "Não Planejado":
                    retorno = CsSituacaoCicloDomain.NaoPlanejado;
                    break;

                case "Concluído":
                    retorno = CsSituacaoCicloDomain.Concluido;
                    break;

                case "Em andamento":
                    retorno = CsSituacaoCicloDomain.EmAndamento;
                    break;

                case "Planejado":
                    retorno = CsSituacaoCicloDomain.Planejado;
                    break;

                case "Em atraso":
                    retorno = CsSituacaoCicloDomain.EmAtraso;
                    break;

                case "Cancelado":
                    retorno = CsSituacaoCicloDomain.Cancelado;
                    break;

                default:
                    new Exception( "Situação do Ciclo não encontrada." );
                    break;
            }

            return retorno;
        }
        #endregion
    }
}
