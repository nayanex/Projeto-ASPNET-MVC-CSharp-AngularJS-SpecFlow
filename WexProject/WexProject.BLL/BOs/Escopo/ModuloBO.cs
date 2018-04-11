using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Escopo;

namespace WexProject.BLL.BOs.Escopo
{
    public class ModuloBO
    {
        /// <summary>
        /// Método responsável por recuperar o módulo raiz 
        /// </summary>
        /// <param name="modulo">Módulo que se deseja obter o módulo raiz</param>
        /// <returns>Módulo raiz</returns>
        public static Modulo RecuperarModuloRaiz( Modulo modulo )
        {
            if(modulo.ModuloPai == null)
                return modulo;

            return RecuperarModuloRaiz( modulo.ModuloPai );
        }
    }
}
