using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Custos
{
	public class ProjetoSemTipoException: Exception
	{
		public ProjetoSemTipoException()
		{
		}

		public ProjetoSemTipoException(String message)
			: base(message)
		{
		}

		public ProjetoSemTipoException(String message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ProjetoSemTipoException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
