using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Exceptions.Custos
{
	public class RubricaTipoDiferenteException : Exception
	{
		public RubricaTipoDiferenteException()
		{
		}

		public RubricaTipoDiferenteException(String message)
			: base(message)
		{
		}

		public RubricaTipoDiferenteException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RubricaTipoDiferenteException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
