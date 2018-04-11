using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Exceptions.Geral
{
	public class PropriedadeNaoExisteException : Exception
	{
		public PropriedadeNaoExisteException()
		{
		}

		public PropriedadeNaoExisteException(String message)
			: base(message)
		{
		}

		public PropriedadeNaoExisteException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected PropriedadeNaoExisteException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
