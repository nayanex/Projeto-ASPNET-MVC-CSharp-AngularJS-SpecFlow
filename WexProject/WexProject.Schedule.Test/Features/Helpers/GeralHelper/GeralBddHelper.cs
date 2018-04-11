using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Schedule.Test.Features.Helpers.GeralHelper
{
    public class GeralBddHelper
    {
        /// <summary>
        /// Método responsável por converter a data passada no BDD de string para formato Datetime.
        /// Ex: 10/08; 05/10 (Formato brasileiro)
        /// </summary>
        /// <param name="data">data informada no BDD</param>
        /// <returns>Objeto Datetime</returns>
        public static DateTime ConverterDataEmStringParaDateTime( string data )
        {
            int[] dataArray = data.Split( '/' ).Select( o => Convert.ToInt32( o ) ).ToArray();

            if(dataArray.Count() == 2)
            {
                return new DateTime( DateTime.Today.Year, dataArray[1], dataArray[0] );
            }

            return new DateTime();
        }
    }
}
