using System;
using System.Web.Routing;

namespace WexProject.Library.Libs.Web.Mvc
{
	public class GuidConstraint : IRouteConstraint
	{
		private bool empty;

		public GuidConstraint(bool empty = false)
		{
			this.empty = empty;
		}

		public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			var valor = values[parameterName] as string;
			Guid guid;

			if (String.IsNullOrEmpty(valor) || Guid.TryParse(valor, out guid))
			{
				return true;
			}

			return false;
		}
	}
}