using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class ProjetoSemCentroCustoException : Exception
	{
		public ProjetoSemCentroCustoException()
		{
		}

		public ProjetoSemCentroCustoException(String message)
			: base(message)
		{
		}

		public ProjetoSemCentroCustoException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ProjetoSemCentroCustoException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
