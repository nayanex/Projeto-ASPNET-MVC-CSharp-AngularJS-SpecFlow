using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WexProject.Library.Libs.Test
{
    public class ControleDeEsperaUtil
    {
        public delegate bool CondicaoDeEspera();

        public static void AguardarAte(CondicaoDeEspera condicao,long tempoEmSegundos = 3) {

            long tempoEmMilisegundos = tempoEmSegundos * 1000;
            long tempoAtual = 0;
            while (!condicao() && tempoAtual <= tempoEmMilisegundos)
            {
                Thread.Sleep(10);
                tempoAtual += 10;
            }
        }
    }
}
