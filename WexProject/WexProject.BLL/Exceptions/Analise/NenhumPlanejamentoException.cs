using System;

namespace WexProject.BLL.Exceptions.Analise
{
    public class NenhumPlanejamentoException : Exception
    {
        public NenhumPlanejamentoException()
        {
        }

        public NenhumPlanejamentoException(String message) : base(message)
        {
        }
    }
}