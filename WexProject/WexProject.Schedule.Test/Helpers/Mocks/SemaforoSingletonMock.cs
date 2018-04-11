using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Library.Libs.SemaforoPorIntervalo;
using System.Collections;

namespace WexProject.Scheduele.Test.Helpers.Mocks
{
    public class SemaforoSingletonMock : SemaforoSingleton
    {
        #region Atributos Mock

        /// <summary>
        /// MOCK
        /// Atributo estático responsável por armazenar os cronogramas, sua respectiva lista contendo vários semáforos.
        /// </summary>
        public static Dictionary<Guid, SemaforosControle> cronogramaSemaforosMock = SemaforoSingleton.semaforosPorCronograma;


        #endregion

        #region Métodos Mock


        /// <summary>
        /// Método responsável por setar a instância mockada na classe pai (SemaforoSingleton)
        /// </summary>
        /// <param name="semaforo">Instancia do semáforo</param>
        public static void SetInstancia( SemaforoSingleton semaforo )
        {
            instanciaSemaforoSingleton = semaforo;
        }

        /// <summary>
        /// MOCK
        /// Método responsável controlar o acesso de threads simultaneamente enquanto leem e escrevem tanto no dicionário de cronogramas quanto na lista de semáforos de cada cronograma.
        /// Valida o acesso por cronograma e intervalo.
        /// </summary>
        /// <param name="oidCronograma">Oid cronograma</param>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        public override Hashtable ControlarSemaforos( Guid oidCronograma, short min, short max )
        {
            return base.ControlarSemaforos( oidCronograma, min, max );
        }

        /// <summary>
        /// MOCK
        /// Método responsável por liberar recursos do semáforo criado que não possua mais ninguém em espera.
        /// </summary>
        /// <param name="semaforoIntervalo">Objeto SemaforoPorIntevalo</param>
        public static void DestruirSemaforo( SemaforoPorIntervalo semaforoPorIntervalo)
        {
            SemaforoPorIntervalo.DestruirSemaforo( semaforoPorIntervalo );
        }


        #endregion

    }
}
