using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.Comparacao
{
    public static class ComparadorGenerico
    {
        public static bool HouveMudancaEm<TObject>( TObject objeto, TObject objeto2, params Func<TObject, object>[] propriedades ) where TObject : class
        {
			if( objeto == null && objeto2 == null )
				return false;

			if( objeto == null || objeto2 == null )
				return true;

            if(propriedades.Count() > 0)
            {
                foreach(var func in propriedades)
                {
                    var propriedade = func( objeto );
                    var propriedadeAtual = func( objeto2 );
					if( !propriedade.Equals( propriedadeAtual ) )
                        return true;
                }
            }
            else
            {
                var tipos = typeof( TObject );
                foreach(var propriedade in tipos.GetProperties())
                {
                    var propriedade1 = propriedade.GetValue( objeto );
                    var propriedade2 = propriedade.GetValue( objeto2 );
                    if(( propriedade1 != null && ( propriedade2 == null || !propriedade1.Equals( propriedade2 ) ) ) || ( propriedade1 == null && propriedade2 != null ))
                        return true;
                }
            }
            return false;
        }
    }
}
