using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.Extensions.EnumExtension
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Método responsável por  retornar o valor inteiro de um EnumValue
        /// </summary>
        /// <param name="valorEnum">valor do enum</param>
        /// <returns>retorna o valor Int32 correspondente a um valor do respectivo enum</returns>
        public static int ToInt( this Enum valorEnum )
        {
            return (int)( (object)valorEnum );
        }
    }
}
