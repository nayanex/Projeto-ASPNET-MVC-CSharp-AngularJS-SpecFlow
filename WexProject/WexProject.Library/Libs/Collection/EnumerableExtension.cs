using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.Library.Libs.Nullable;

namespace WexProject.Library.Libs.Collection
{
	public static class EnumerableExtension
	{
		public static Nullable<Tbase> Sum<Tobj, Tbase>(this IEnumerable<Tobj> lista, Func<Tobj, Nullable<Tbase>> prop) where Tobj : class where Tbase : struct
		{
			Nullable<Tbase> soma = null;

			foreach (var obj in lista)
			{
				soma = soma.Add(prop.Invoke(obj));
			}

			return soma;
		}
	}
}
