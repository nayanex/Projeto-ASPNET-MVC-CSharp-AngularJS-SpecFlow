using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.BLL.Exceptions.Planejamento
{
    /// <summary>
    /// Excessão criada para ser lançada referente as possiveis erros sobre o campo Estimativa Inicial de uma tarefa no cronograma
    /// </summary>
    public class EstimativaInicialInvalidaException : Exception
    {
        public EstimativaInicialInvalidaException()
        {
        }
        /// <summary>
        /// construtor da exception configurando a com uma mensagem relativa ao lançamento da excessão
        /// </summary>
        /// <param name="message"></param>
        public EstimativaInicialInvalidaException(string message)
            : base(message)
        {
            
        }
        public EstimativaInicialInvalidaException(string message ,Exception innerException)
            : base(message ,innerException)
        {
            
        }
        protected EstimativaInicialInvalidaException(System.Runtime.Serialization.SerializationInfo info ,System.Runtime.Serialization.StreamingContext context)
            : base(info ,context)
        {
            
        }
    }
}
