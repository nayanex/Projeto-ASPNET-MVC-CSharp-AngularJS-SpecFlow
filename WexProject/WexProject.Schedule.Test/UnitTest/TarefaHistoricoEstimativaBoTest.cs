using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaHistoricoEstimativaBoTest : BaseEntityFrameworkTest
    {
        [TestMethod, ExpectedException( typeof( ArgumentException ), "Deveria ter levantado uma exceção de argumento para o Oid 0000-0000-0000-0000" )]
        public void NaoDeveCriarHistoricoParaUmaTarefaDeOidVazio()
        {
            TarefaHistoricoEstimativaBo.CriarHistorico( DateTime.Now, new Guid(), 0 );
        }
    }
}
