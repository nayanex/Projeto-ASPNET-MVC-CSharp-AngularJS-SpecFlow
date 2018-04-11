//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using WexProject.Library.Libs.Planejamento.Utils;
//using System.Threading;

//namespace WexProject.Test.Helpers.Mocks
//{
//    public class SemaforoPorIntervaloMock : SemaforoPorIntervalo
//    {
//        #region Atributos

//        /// <summary>
//        /// Atributo responsável por guardar o semáforo correspondente a um determinado intervalo
//        /// </summary>
//        public Semaphore semaforo;

//        /// <summary>
//        /// Atributo responsável por guardar o início do intervalo
//        /// </summary>
//        public short inicio;

//        /// <summary>
//        /// Atributo responsável por guardar o final do intervalo
//        /// </summary>
//        public short final;

//        /// <summary>
//        /// Atributo responsável por guardar quantas threads se encontram em espera em determinado semáforo.
//        /// </summary>
//        public short emEspera;

//        #endregion

//        #region Regras de Negócio

//        #endregion

//        #region Construtor

//        /// <summary>
//        /// Construtor que recebe parâmetros e cria semáforo correspondente a determinado intervalo.
//        /// </summary>
//        /// <param name="inicio">Início do intervalo</param>
//        /// <param name="final">Final do intervalo</param>
//        /// <param name="posicoesDisponiveis">Posições disponíveis para acesso</param>
//        /// <param name="posicoesLimiteAcesso">Limite de posições disponíveis em um semáforo</param>
//        public SemaforoPorIntervaloMock( short inicio, short final, short posicoesDisponiveis = 0, short posicoesLimiteAcesso = 1 )
//            : base( inicio, final, posicoesDisponiveis, posicoesLimiteAcesso )
//        {
//            semaforo = base.semaforo;
//            inicio = base.inicio;
//            final = base.final;
//            emEspera = base.emEspera;
//        }

//        #endregion

//    }
//}
