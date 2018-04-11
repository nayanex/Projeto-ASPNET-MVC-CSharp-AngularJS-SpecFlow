using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WexProject.Library.Exceptions.Geral;

namespace WexProject.Library.Libs.Generic
{
	public class ModeloDinamico<T> where T : class
	{
		private List<PropertyInfo> campos = new List<PropertyInfo>();

		public ModeloDinamico(String[] campos)
		{
			var tipo = typeof(T);

			foreach (var campo in campos)
			{
				var prop = tipo.GetProperty(campo);
				if (prop == null)
				{
					throw new PropriedadeNaoExisteException(String.Format("Propriedade {0} não existe em objeto do tipo {1}", campo, tipo));
				}

				this.campos.Add(prop);
			}
		}

		public ModeloDinamico(String campos)
			: this(campos.Split(','))
		{
		}

		public ObjetoDinamico Objeto(T entidade)
		{
			var objeto = new ObjetoDinamico();

			foreach (var campo in campos)
			{
				objeto.Add(campo.Name, campo.GetValue(entidade));
			}

			return objeto;
		}

	}

	public class ObjetoDinamico : Dictionary<string, object>
	{
	}
}
