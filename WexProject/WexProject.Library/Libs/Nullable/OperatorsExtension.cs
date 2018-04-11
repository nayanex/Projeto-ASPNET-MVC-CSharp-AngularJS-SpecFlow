using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.Nullable
{
	public static class OperatorsExtension
	{
		public static Nullable<T> Add<T>(this Nullable<T> x, Nullable<T> y) where T : struct
		{
			dynamic dx, dy;

			if (x.HasValue || y.HasValue)
			{
				dx = x.GetValueOrDefault();
				dy = y.GetValueOrDefault();

				return dx + dy;
			}

			return null;
		}

		public static Nullable<T> Sub<T>(this Nullable<T> x, Nullable<T> y) where T : struct
		{
			dynamic dx, dy;

			if (x.HasValue || y.HasValue)
			{
				dx = x.GetValueOrDefault();
				dy = y.GetValueOrDefault();

				return dx - dy;
			}

			return null;
		}
	}
}
