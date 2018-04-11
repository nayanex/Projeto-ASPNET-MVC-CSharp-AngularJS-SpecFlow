using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class RubricaNaoVaziaException : Exception
	{
		public RubricaNaoVaziaException()
		{
		}

		public RubricaNaoVaziaException(String message)
			: base(message)
		{
		}

		public RubricaNaoVaziaException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RubricaNaoVaziaException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
