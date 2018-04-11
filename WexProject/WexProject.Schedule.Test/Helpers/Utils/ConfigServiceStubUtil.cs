using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.Schedule.Test.Stubs;
using WexProject.Schedule.Library.Presenters;
using Moq;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Library.Libs.Logger;
using System.IO;

namespace WexProject.Schedule.Test.Helpers.Utils
{
    public class ConfigServiceStubUtil
    {
        const string ConfigLoggerFileName = "TesteLogger.config";
        /// <summary>
        /// Método para inicializar os serviços Stub
        /// </summary>
        /// <param name="planejamentoStub">Stub do Planejamento Service</param>
        /// <param name="geralStub">Stub Geral Service</param>
        public static void InicializarServicosStubCronograma( out PlanejamentoServiceUtilStub planejamentoStub, out GeralServiceUtilStub geralStub )
        {
            planejamentoStub = new PlanejamentoServiceUtilStub();
            geralStub = new GeralServiceUtilStub()
            {
                UsuariosConectadosConfig = planejamentoStub.colaboradoresConfig,
                ColaboradorLogado = planejamentoStub.colaboradorLogado,
                Colaboradores = planejamentoStub.ListarColaboradores()
            };
            //WexLogger.CreateSingletonInstance( "TestLogger", new FileInfo( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, ConfigLoggerFileName ) ) );
            CronogramaPresenter.ServicoPlanejamento = planejamentoStub;
            CronogramaPresenter.ServicoGeral = geralStub;
        }
    }
}
