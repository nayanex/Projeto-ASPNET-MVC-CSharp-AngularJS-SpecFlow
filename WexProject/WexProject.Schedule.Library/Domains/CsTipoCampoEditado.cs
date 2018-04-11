using System;
using System.Collections.Generic;
using System.Linq;

namespace WexProject.Schedule.Library.Domains
{
    /// <summary>
    ///  Definir o tipo de alteração que ocorreu na edição de uma tarefa.
    /// </summary>
	[Flags]
    public enum CsTipoCampoEditado
    {
		Nenhum = 0,
        SituacaoPlanejamento = 1,
        EstimativaInicial = 2,
        EstimativaRestante = 4
    }
}
