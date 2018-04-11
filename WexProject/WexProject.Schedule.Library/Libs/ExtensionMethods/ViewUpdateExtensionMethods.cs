using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.XtraBars;

namespace WexProject.Schedule.Library.Libs.ExtensionMethods
{
    public static class ViewUpdateExtensionMethods
    {
        public static TResult SafeInvoke<T, TResult>( this T invoker, Func<T, TResult> chamada ) where T : ISynchronizeInvoke
        {
            if(invoker.InvokeRequired)
            {
				IAsyncResult retornoAssincrono = invoker.BeginInvoke( chamada , new object[] { invoker } );
                object resultadoExecucao = invoker.EndInvoke( retornoAssincrono );
                return (TResult)resultadoExecucao;
            }
            else
                return chamada( invoker );
        }

        public static void SafeInvoke<T>( this T objetoInvocador, Action<T> chamada ) where T : ISynchronizeInvoke
        {
            if(objetoInvocador.InvokeRequired)
                objetoInvocador.BeginInvoke( chamada, new object[] { objetoInvocador } );
            else
                chamada( objetoInvocador );
        }
    }
}
