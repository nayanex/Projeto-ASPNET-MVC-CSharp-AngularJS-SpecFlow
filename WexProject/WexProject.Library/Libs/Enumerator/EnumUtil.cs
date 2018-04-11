using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace WexProject.Library.Libs.Enumerator
{
    /// <summary>
    /// classe utilitária para manipular a descrição dos enumerados.
    /// Para utilizar este recurso, basta adicionar a tag [Description("descrição do item")] 
    /// antes do item do enumerado que deseja obter uma descrição.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Método que retorna a descrição do enumerado setado no atributo [Description]
        /// </summary>
        /// <param name="item">item do enum</param>
        /// <returns>string</returns>
        public static string DescricaoEnum(Enum item)
        {
            var tipo = item.GetType();
            FieldInfo fi = tipo.GetField(item.ToString());

            var atributos = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (atributos.Length > 0)
                return atributos[0].Description;

            else
                return String.Empty;
        }

        /// <summary>
        /// Método que retorna a descrição do enumerado setado a partir do valor e tipo do Enum
        /// </summary>
        /// <param name="valueEnum">Valor do Enum</param>
        /// <param name="typeEnum">Tipo do Enum</param>
        /// <returns>o nome do Enum</returns>
        public static string DescricaoEnum(Type typeEnum,object valueEnum)
        {

            string name = string.Empty;

            name = Enum.GetName(typeEnum, valueEnum);

            return name;
    }

        /// <summary>
        /// Metodo que retorna o valor de um enum 
        /// </summary>
        /// <param name="anItem">item do enum</param>
        /// <returns>um object que conterá o valor do item do enum ou nulo caso não exista o item passadao entre parametro</returns>
        public static object ValueEnum(object anItem) 
        {
            Enum item = anItem as Enum;
            if (item != null)
            {
                var type = item.GetType();
                FieldInfo info = type.GetField(item.ToString());
                object value = info.GetRawConstantValue();
                return value;
}
            return item;
        }

        /// <summary>
        /// Metodo que retorno o valor de um item de um enum
        /// </summary>
        /// <param name="type">tipo do Enum</param>
        /// <param name="nameItem">descrição do item</param>
        /// <returns>valor da descrição do item</returns>
        public static object ValueEnum(Type type,string nameItem) 
        {            
            FieldInfo info = type.GetField(nameItem);
            object value = info.GetRawConstantValue();
            return value;            
        }

    }
}
