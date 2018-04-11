using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Schedule.Library.Libs.Delegates.Tempo
{
	/// <summary>
	/// Delegate responsável pela assinatura de eventos que possuírem o timespan rejeitado em alguma validação
	/// </summary>
	/// <param name="tempoInvalido">timespan com os valores que foram invalidados</param>
	public delegate void AoValidarTempoEventHandler( TimeSpan tempoInvalido );
}
