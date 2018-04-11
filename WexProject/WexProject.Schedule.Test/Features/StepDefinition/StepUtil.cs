using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    /// <summary>
    /// Util de Step
    /// </summary>
    [Binding]
    public class StepUtil
    {
        #region Given

        [Given( @"que o dia atual seja '(.*)'" )]
        public void DadoQueODiaAtualSeja( string dataAtual )
        {
            DateUtil.CurrentDateTime = DateTime.Parse( dataAtual );
        }

        #endregion
    }
}
