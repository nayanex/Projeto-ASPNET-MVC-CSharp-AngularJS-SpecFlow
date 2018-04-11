using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory com métodos básicos.
    /// </summary>
    public class BaseFactory
    {
        /// <summary>
        /// Retorna uma descricao concatenada com o nome da classe
        /// e a data e hora atual para ser utilizado nas factories
        /// que possuem campos de descrição.
        /// </summary>
        /// <returns>Nova descricao</returns>
        public static String GetDescricao()
        {
            return String.Format("{0:yyyyMMddhhmmss}", DateUtil.ConsultarDataHoraAtual());
        }
    }
}
