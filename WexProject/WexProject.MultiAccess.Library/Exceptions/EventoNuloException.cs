using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.MultiAccess.Library.Exceptions
{
    [Serializable]
    public class EventoNuloException : Exception
    {
        public EventoNuloException() { }
        public EventoNuloException( string message ) : base( message ) { }
        public EventoNuloException( string message, Exception inner ) : base( message, inner ) { }
        protected EventoNuloException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context )
            : base( info, context ) { }
    }
}
