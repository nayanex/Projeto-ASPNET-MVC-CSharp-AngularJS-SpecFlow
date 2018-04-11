using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Exceptions.Planejamento
{
    public class CapacidadePlanejamentoForaDoPeriodoCronogramaException : Exception
    {
        public CapacidadePlanejamentoForaDoPeriodoCronogramaException()
        {

        }
        public CapacidadePlanejamentoForaDoPeriodoCronogramaException( string message )
            : base( message )
        {

        }
        public CapacidadePlanejamentoForaDoPeriodoCronogramaException( string message, Exception innerException )
            : base( message, innerException )
        {

        }
        protected CapacidadePlanejamentoForaDoPeriodoCronogramaException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
            : base( info, context )
        {

        }
    }
}
