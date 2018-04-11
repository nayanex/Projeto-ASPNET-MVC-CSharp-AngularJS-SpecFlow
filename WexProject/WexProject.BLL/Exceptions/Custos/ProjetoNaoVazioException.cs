using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class ProjetoNaoVazioException : Exception
	{
		public ProjetoNaoVazioException()
		{
		}

		public ProjetoNaoVazioException(String message)
			: base(message)
		{
		}

		public ProjetoNaoVazioException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ProjetoNaoVazioException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
