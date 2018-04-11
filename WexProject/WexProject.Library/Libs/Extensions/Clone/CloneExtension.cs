using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.Extensions.Clone
{
    public static class CloneExtension
    {
        public static TObject Clonar<TObject>( this TObject objeto )
        {
			if( objeto == null )
				return default( TObject );

            Type tipo = typeof( TObject );
            MethodInfo metodo = tipo.GetMethod( "MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic );
            return (TObject)metodo.Invoke( objeto, null );
        }
    }
}
