using System;
using System.Collections.Generic;
using WexProject.BLL.Util;

namespace WexProject.BLL.Exceptions.Geral
{
	public class ValidationException : Exception
	{
        public string Field { get; set; }
        public string Message { get; set; }
        public List<Validation> Validacoes { get; set; }
	}
}
