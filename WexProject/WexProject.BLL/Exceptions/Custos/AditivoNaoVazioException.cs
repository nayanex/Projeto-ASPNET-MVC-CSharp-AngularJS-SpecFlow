using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class AditivoNaoVazioException : Exception
	{
		public AditivoNaoVazioException()
		{
		}

		public AditivoNaoVazioException(String message)
			: base(message)
		{
		}

		public AditivoNaoVazioException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected AditivoNaoVazioException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
