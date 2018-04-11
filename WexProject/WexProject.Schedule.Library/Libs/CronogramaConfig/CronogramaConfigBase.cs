using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Schedule.Library.Libs.CronogramaConfig
{
	/// <summary>
	/// Classe modelo de implementação de carregamento das configurações básicas do cronogram
	/// </summary>
	public abstract class CronogramaConfigBase
	{
		protected const string PortaServidor = "PortaServidor";
		protected const string NomeServidor = "NomeServidor";

		public abstract void ConfigurarPortaServidor( out int porta );
		public abstract void ConfigurarNomeServidor( out string nomeServidor );
	}
}
