using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.Schedule.Library.Libs.CronogramaConfig;

namespace WexProject.Schedule.Test.Helpers.CronogramaConfig
{
	public class CronogramaConfigStub : CronogramaConfigBase
	{
		public static int PortaServidor { get; set; }
		public static string NomeServidor { get; set; }


		public override void ConfigurarPortaServidor( out int porta )
		{
			porta = PortaServidor;
		}

		public override void ConfigurarNomeServidor( out string nomeServidor )
		{
			nomeServidor = NomeServidor;
		}
	}
}
