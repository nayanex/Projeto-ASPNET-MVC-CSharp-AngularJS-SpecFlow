using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class ProjetoJaAssociadoATermoAditivoException : Exception
	{
		public ProjetoJaAssociadoATermoAditivoException()
		{
		}

		public ProjetoJaAssociadoATermoAditivoException(String message)
			: base(message)
		{
		}

		public ProjetoJaAssociadoATermoAditivoException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ProjetoJaAssociadoATermoAditivoException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
