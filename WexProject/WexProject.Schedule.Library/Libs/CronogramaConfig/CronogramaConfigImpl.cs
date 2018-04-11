using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Schedule.Library.Libs.CronogramaConfig
{
	/// <summary>
	/// Classe responsável por carregar as configurações básicas do cronograma
	/// </summary>
	public class CronogramaConfigImpl : CronogramaConfigBase
	{
		public override void ConfigurarPortaServidor( out int porta )
		{
			porta = Convert.ToInt32( ConfigurationManager.AppSettings.Get( PortaServidor ) );
		}

		public override void ConfigurarNomeServidor( out string nomeServidor )
		{
			nomeServidor = ConfigurationManager.AppSettings.Get( NomeServidor );
		}
	}
}
