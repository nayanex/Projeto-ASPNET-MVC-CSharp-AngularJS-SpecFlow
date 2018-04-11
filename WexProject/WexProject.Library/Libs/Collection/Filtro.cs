using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WexProject.Library.Exceptions.Geral;

namespace WexProject.Library.Libs.Collection
{
	public class Filtro<T> : Dictionary<string, object> where T : class
	{
		private Type tipo;

		public Filtro()
		{
			this.tipo = typeof(T);
		}

		public Filtro(IEnumerable<KeyValuePair<string, string>> parametros)
			: this()
		{
			foreach (var param in parametros)
			{
				var prop = tipo.GetProperty(param.Key);
				if (prop == null)
				{
					throw new PropriedadeNaoExisteException(String.Format("Propriedade {0} não existe em objeto do tipo {1}", param.Key, tipo));
				}

				object valor = param.Value;

				if ((string)valor == "null")
				{
					valor = null;
				}
				else
				{
					valor = System.ComponentModel.TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(valor);
				}

				this[param.Key] = valor;
			}
		}

		public Filtro(IEnumerable<KeyValuePair<string, object>> parametros)
			: this()
		{
			foreach (var param in parametros)
			{
				var prop = tipo.GetProperty(param.Key);
				if (prop == null)
				{
					throw new PropriedadeNaoExisteException(String.Format("Propriedade {0} não existe em objeto do tipo {1}", param.Key, tipo));
				}

				object valor = param.Value;

				if (valor != null && prop.PropertyType != valor.GetType())
				{
					valor = System.ComponentModel.TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(valor);
				}

				this[param.Key] = valor;
			}
		}

		public bool Filtrar(T entidade)
		{
			foreach (var param in this)
			{
				var prop = tipo.GetProperty(param.Key);
				if (prop == null)
				{
					throw new PropriedadeNaoExisteException(String.Format("Propriedade {0} não existe em objeto do tipo {1}", param.Key, tipo));
				}

				var propValor = prop.GetValue(entidade);
				var valor = param.Value;

				if ((propValor != null && (valor == null || !propValor.Equals(valor))) || (propValor == null && valor != null))
				{
					return false;
				}
			}
			return true;
		}
	}

	public static class FiltroExtension
	{
		public static IEnumerable<T> Filtra<T>(this IEnumerable<T> lista, Filtro<T> filtro) where T : class
		{
			return lista.Where(filtro.Filtrar);
		}

		public static Filtro<T> CriarFiltro<T>(this NameValueCollection parametros) where T : class
		{
			return new Filtro<T>(parametros.Cast<string>().Select(key => new KeyValuePair<string, string>(key, parametros[key])));
		}
	}
}
