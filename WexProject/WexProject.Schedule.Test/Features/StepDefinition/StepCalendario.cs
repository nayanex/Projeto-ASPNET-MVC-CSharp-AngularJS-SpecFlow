using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WexProject.Schedule.Test.Helpers.Bind;
using WexProject.Schedule.Test.UnitTest;



namespace WexProject.Schedule.Test.Features.StepDefinition
{
    [Binding]
    public class StepCalendario : BaseEntityFrameworkTest
    {
        [Given( @"que o calendario institucional possui as seguintes definicoes:" )]
        public void DadoQueOCalendarioInstitucionalPossuiAsSeguintesDefinicoes( Table table )
        {
            table.CreateSet<CalendarioBindHelper>()
                .Select( o => o.ToCalendario() )
                .ToList()
                .ForEach( item => contexto.Calendarios.Add( item ) );
            contexto.SaveChanges();
        }
    }
}
