using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class TermoAditivoNaoVazioException : Exception
	{
		public TermoAditivoNaoVazioException()
		{
		}

		public TermoAditivoNaoVazioException(String message)
			: base(message)
		{
		}

		public TermoAditivoNaoVazioException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected TermoAditivoNaoVazioException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
