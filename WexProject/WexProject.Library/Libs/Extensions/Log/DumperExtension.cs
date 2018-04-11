using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace WexProject.Library.Libs.Extensions.Log
{
    public static class DumperExtension
    {
        /// <summary>
        /// Criar uma Dump string para enviar para ser utilizada no Debug
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDumpString(this object value)
        {
             return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        /// <summary>
        /// Logar um Dump da variavel atual
        /// </summary>
        /// <param name="value">Objeto</param>
        /// <param name="descricao"> criar uma descrição para o Objeto atual</param>
        public static void Dump( this object value , string descricao = null)
        {
            if(!string.IsNullOrWhiteSpace( descricao ))
                Debug.WriteLine( descricao );
            Debug.Write(value.ToDumpString());
        }

        /// <summary>
        /// Logar um Dump da variavel atual
        /// </summary>
        /// <param name="value">Objeto</param>
        /// <param name="descricao"> criar uma descrição para o Objeto atual</param>
        public static TObject Dump<TObject>( this TObject value, string descricao = null )
        {
            if(!string.IsNullOrWhiteSpace( descricao ))
                Debug.WriteLine( descricao );
            Debug.Write( ((object)value).ToDumpString() );
            return value;
        }

        /// <summary>
        /// Método que acessa o caminho callStack percorrido pelo método chamado
        /// </summary>
        /// <param name="descricao">descrição textual opcional</param>
        public static void ShowStackTrace( string descricao = null )
        {
            StackTrace st = new StackTrace( true );
            if(!string.IsNullOrWhiteSpace( descricao ))
                Debug.WriteLine( descricao );
            else
                Debug.WriteLine( "" );

            for(int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame( i );
                if(sf == null || sf.GetFileLineNumber() == 0)
                    continue;
                Debug.WriteLine( "Metodo: {1}\nLinha: {2}\nArquivo:{0}", sf.GetFileName(),
                    sf.GetMethod(), sf.GetFileLineNumber() );
            }
        }
    }
}
