using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Geral
{
	public class EntidadeNaoEncontradaException : Exception
	{
		public EntidadeNaoEncontradaException()
		{
		}

		public EntidadeNaoEncontradaException(String message)
			: base(message)
		{
		}

		public EntidadeNaoEncontradaException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected EntidadeNaoEncontradaException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
