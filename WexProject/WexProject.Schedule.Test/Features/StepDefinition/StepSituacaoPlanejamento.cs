using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WexProject.BLL.Shared.Domains.Planejamento;

using WexProject.Schedule.Test.Features.Helpers.SituacaoPlanejamentoHelper;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.Schedule.Test.UnitTest;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    [Binding]
    public class StepSituacaoPlanejamento : BaseEntityFrameworkTest
    {
        [Given( @"que exista\(m\) a\(s\) situacao\(oes\) de planejamento a seguir:" )]
        public void DadoQueExistaMASSituacaoOesDePlanejamentoASeguir( Table table )
        {
            CsPadraoSistema padraoSistema = CsPadraoSistema.Não;
            CsTipoPlanejamento tipoPlanejamento = CsTipoPlanejamento.Cancelamento;

            for(int i = 0; i < table.RowCount; i++)
            {
                padraoSistema = SituacaoPlanejamentoBddHelper.ConverterSituacaoPadraoSistemaStrinParaSituacaoPadraoSistemaDomain( table.Rows[i][table.Header.ToList()[3]] );

                tipoPlanejamento = SituacaoPlanejamentoBddHelper.ConverterTipoPlanejamentoStringParaTipoPlanejamentoDomain( table.Rows[i][table.Header.ToList()[0]] );

                CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, table.Rows[i][table.Header.ToList()[1]], CsTipoSituacaoPlanejamento.Ativo, tipoPlanejamento, padraoSistema );
            }
        }
    }
}
