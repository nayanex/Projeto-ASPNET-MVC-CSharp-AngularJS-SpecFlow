using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Planejamento
{
    public class CronogramaCapacidadePlanDataJaCadastradaException : Exception
    {
         public CronogramaCapacidadePlanDataJaCadastradaException()
        {
            
        }
        public CronogramaCapacidadePlanDataJaCadastradaException( string message )
            : base( message )
        {
            
        }
        public CronogramaCapacidadePlanDataJaCadastradaException( string message, Exception innerException )
            : base( message, innerException )
        {
            
        }
        protected CronogramaCapacidadePlanDataJaCadastradaException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
            : base( info, context )
        {
            
        }
    }
}
