using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.Library.Libs.Delegates
{
    /// <summary>
    /// Delegate com assinatura default com um objeto como retorno das requisições assincronas
    /// </summary>
    /// <param name="objetoRetorno">Retorno Service</param>
    public delegate void RespostaAsyncServiceHandler( object objetoRetorno );
}
