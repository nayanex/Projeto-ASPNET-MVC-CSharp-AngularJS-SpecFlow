using System;

namespace WexProject.Library.Libs.Vetor
{
    /// <summary>
    /// Classe utilitária com funções para Array.
    /// </summary>
    public class ArrayUtil
    {
        /// <summary>
        /// Retorna o índice de um valor dentro de um array.
        /// </summary>
        /// <param name="value">Valor procurado</param>
        /// <param name="array">Array</param>
        /// <returns>Retorna o índice do valor no array</returns>
        public static int IndexOf(Object value, Array array)
        {
            int i;
            for (i = 0; i < array.Length; i++)
                if (array.GetValue(i).ToString() == value.ToString())
                    return i;
            return -1;
        }

        /// <summary>
        /// Retorna verdadeiro se o value constar no array.
        /// </summary>
        /// <param name="value">Valor procurado</param>
        /// <param name="array">Array</param>
        /// <returns>Verdadeiro/Falso</returns>
        public static bool ValueExist(Object value, Array array)
        {
            return IndexOf(value, array) != -1;
        }

    }
}
