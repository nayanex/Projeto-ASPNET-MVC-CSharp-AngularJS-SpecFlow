using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Library.Libs.SemaforoPorIntervalo;
using WexProject.Schedule.Test.UnitTest;

namespace WexProject.Schedule.Test
{
    [TestClass]
    public class SemaforoPorIntervaloTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void RnGeraIntervaloQuandoIntervalo1a10Test()
        {
            short inicio = 1;
            short final = 10;


            List<short> intervalo = new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<short> novoIntervalo = SemaforoPorIntervalo.CriarIntervalo( inicio, final );


            Assert.AreEqual( intervalo.Count, novoIntervalo.Count );
            Assert.AreEqual( 0, intervalo.Except( novoIntervalo ).Count() );

            IEnumerable<short> intervaloIntersect = intervalo.Intersect( novoIntervalo );

            Assert.AreEqual( 10, intervaloIntersect.Count() );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 12 a 19
        /// Expectativa: Então o intervalo deverá reaproveitar o semáforo do intervalo (10 a 20) já criado.
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe12a19AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 12, 19 );

            Assert.AreEqual( 1, semaforosImpactadosExistentes.Count, "Deveria ter apenas um semáforo, pois está dentro do intervalo." );

            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 8 a 22
        /// Expectativa: Então o intervalo deverá retornar 3 semáforos:
        ///              1- semáforo de 8 a 9.
        ///              2- semáforo de 21 a 22.
        ///              3- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe8a22AfetarUmIntervaloExistentede10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 8, 22 );
            SemaforoPorIntervalo semaforo9a10 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 8 && o.final == 9 );
            SemaforoPorIntervalo semaforo21a22 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 21 && o.final == 22 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.AreEqual( 3, semaforosImpactadosExistentes.Count, "Deveria ter 3 semáforos, pois deve criar 3 intervalos diferentes para serem esperarados." );

            Assert.IsNotNull( semaforo9a10, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 8, semaforo9a10.inicio, "O semáforo deveria ter como início do intervalo o valor 8." );
            Assert.AreEqual( 9, semaforo9a10.final, "O semáforo deveria ter como início do intervalo o valor 10." );

            Assert.IsNotNull( semaforo21a22, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 21, semaforo21a22.inicio, "O semáforo deveria ter como início do intervalo o valor 20." );
            Assert.AreEqual( 22, semaforo21a22.final, "O semáforo deveria ter como início do intervalo o valor 22." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 8 a 12
        /// Expectativa: Então o intervalo deverá retornar 2 semáforos:
        ///              1- semáforo de 8 a 9.
        ///              2- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe8a12AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 8, 12 );
            SemaforoPorIntervalo semaforo8a10 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 8 && o.final == 9 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.AreEqual( 2, semaforosImpactadosExistentes.Count, "Deveria ter 2 semáforos, pois deve criar 1 intervalo diferente do que já existia e reaproveitar o que já existia." );

            Assert.IsNotNull( semaforo8a10, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 8, semaforo8a10.inicio, "O semáforo deveria ter como início do intervalo o valor 8." );
            Assert.AreEqual( 9, semaforo8a10.final, "O semáforo deveria ter como início do intervalo o valor 9." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 19 a 22
        /// Expectativa: Então o intervalo deverá retornar 2 semáforos:
        ///              1- semáforo de 21 a 22.
        ///              2- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe19a22AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 19, 22 );
            SemaforoPorIntervalo semaforo20a22 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 21 && o.final == 22 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.AreEqual( 2, semaforosImpactadosExistentes.Count, "Deveria ter 2 semáforos, pois deve criar 1 intervalo diferente do que já existia e reaproveitar o que já existia." );

            Assert.IsNotNull( semaforo20a22, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 21, semaforo20a22.inicio, "O semáforo deveria ter como início do intervalo o valor 21." );
            Assert.AreEqual( 22, semaforo20a22.final, "O semáforo deveria ter como início do intervalo o valor 22." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 8 a 10
        /// Expectativa: Então o intervalo deverá retornar 2 semáforos:
        ///              1- semáforo de 8 a 9.
        ///              2- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe8a10AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 8, 10 );
            SemaforoPorIntervalo semaforo8a10 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 8 && o.final == 9 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.AreEqual( 2, semaforosImpactadosExistentes.Count, "Deveria ter 2 semáforos, pois deve criar 1 intervalo diferente do que já existia e reaproveitar o que já existia." );

            Assert.IsNotNull( semaforo8a10, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 8, semaforo8a10.inicio, "O semáforo deveria ter como início do intervalo o valor 8." );
            Assert.AreEqual( 9, semaforo8a10.final, "O semáforo deveria ter como início do intervalo o valor 9." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 19 a 20
        /// Expectativa: Então o intervalo deverá retornar 1 semáforo:
        ///              1- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe19a20AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 19, 20 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );
            SemaforoPorIntervalo semaforo19a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 19 && o.final == 20 );

            Assert.AreEqual( 1, semaforosImpactadosExistentes.Count, "Deveria ter 1 semáforo, pois deve reaproveitar o que já existia." );

            Assert.IsNull( semaforo19a20, "Deveria ser null, pois não deveria ter criado semáforo, apenas reaproveitado." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 20 e chegar outro intervalo de 10 a 20
        /// Expectativa: Então o intervalo deverá retornar 1 semáforo:
        ///              1- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe10a20AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 10, 20 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.AreEqual( 1, semaforosImpactadosExistentes.Count, "Deveria ter 1 semáforo, pois deve reaproveitar o que já existia." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 10 a 15 e chegar outro intervalo de 10 a 20
        /// Expectativa: Então o intervalo deverá retornar 1 semáforo:
        ///              1- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe10a15AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosExistentes, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforoPorIntervalo, 10, 15 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosExistentes.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );

            Assert.AreEqual( 1, semaforosImpactadosExistentes.Count, "Deveria ter 1 semáforo, pois deve reaproveitar o que já existia." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir um intervalo de 20 a 25 e chegar outro intervalo de 10 a 20
        /// Expectativa: Então o intervalo deverá retornar 2 semáforo:
        ///              1- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        ///              2- semáforo de 21 a 25
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe20a25AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo = new SemaforoPorIntervalo( 10, 20 );

            List<SemaforoPorIntervalo> semaforosImpactadosParaAguardar = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosParaAguardar, ref semaforosImpactadosNovos, ref semaforosImpactadosParaAguardar, semaforoPorIntervalo, 20, 25 );
            SemaforoPorIntervalo semaforo10a20 = semaforosImpactadosParaAguardar.FirstOrDefault( o => o.inicio == 10 && o.final == 20 );
            SemaforoPorIntervalo semaforo20a25 = semaforosImpactadosParaAguardar.FirstOrDefault( o => o.inicio == 21 && o.final == 25 );

            Assert.AreEqual( 2, semaforosImpactadosParaAguardar.Count, "Deveria ter 2 semáforo, pois deve reaproveitar o que já existia e um novo semáforo com intervalo de 20 a 25." );

            Assert.IsNotNull( semaforo10a20, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 10, semaforo10a20.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 20, semaforo10a20.final, "O semáforo deveria ter como início do intervalo o valor 20." );

            Assert.IsNotNull( semaforo20a25, "Deveria ter criado um semáforo." );
            Assert.AreEqual( 21, semaforo20a25.inicio, "O semáforo deveria ter como início do intervalo o valor 10." );
            Assert.AreEqual( 25, semaforo20a25.final, "O semáforo deveria ter como início do intervalo o valor 20." );
        }

        /// <summary>
        /// Cenário: Validar intervalos de reordenação quando existir os intervalos de  [6..10] afetarem outro intervalo de [10..20], [5..8] e [1..5] já existente.
        /// Expectativa: Então o intervalo deverá retornar 2 semáforo:
        ///              1- semáforo de 10 a 20 (reaproveitar o semáforo que já existe).
        ///              2- semáforo de 5 a 8
        ///              3- semáforo de 1 a 4
        ///              4- semáforo de 9 a 9
        /// </summary>
        [TestMethod]
        public void RnValidarIntervalosPorSemaforoQuandoUmIntervaloDe5a8UmDe1a5UmDe6a10AfetarUmIntervaloExistenteDe10a20Test()
        {
            SemaforoPorIntervalo semaforoPorIntervalo10a20 = new SemaforoPorIntervalo( 10, 20 );
            SemaforoPorIntervalo semaforoPorIntervalo5a8 = new SemaforoPorIntervalo( 5, 8 );
            SemaforoPorIntervalo semaforoPorIntervalo1a4 = new SemaforoPorIntervalo( 1, 4 );

            List<SemaforoPorIntervalo> semaforosImpactadosExistentes = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosParaAguardar = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            semaforosImpactadosExistentes.Add( semaforoPorIntervalo10a20 );
            semaforosImpactadosExistentes.Add( semaforoPorIntervalo5a8 );
            semaforosImpactadosExistentes.Add( semaforoPorIntervalo1a4 );

            for(int i = 0; i < semaforosImpactadosExistentes.Count; i++)
            {
                SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosParaAguardar, ref semaforosImpactadosNovos, ref semaforosImpactadosExistentes, semaforosImpactadosExistentes[i], 6, 10 );
            }

            Assert.AreEqual( 4, semaforosImpactadosExistentes.Count, "Deveria existir 4 semáforos." );
        }

        [TestMethod]
        public void OrdenarSemaforosTest()
        {
            List<SemaforoPorIntervalo> semaforosNovos = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosParaAguardar = new List<SemaforoPorIntervalo>();

            SemaforoPorIntervalo semaforo1 = new SemaforoPorIntervalo( 1, 5 );
            SemaforoPorIntervalo semaforo2 = new SemaforoPorIntervalo( 6, 10 );
            SemaforoPorIntervalo semaforo3 = new SemaforoPorIntervalo( 11, 15 );
            SemaforoPorIntervalo semaforo4 = new SemaforoPorIntervalo( 16, 20 );
            SemaforoPorIntervalo semaforo5 = new SemaforoPorIntervalo( 20, 25 );

            semaforosNovos.Add( semaforo2 );
            semaforosNovos.Add( semaforo4 );
            semaforosNovos.Add( semaforo1 );

            semaforosParaAguardar.Add( semaforo3 );
            semaforosParaAguardar.Add( semaforo5 );

            List<SemaforoPorIntervalo> semaforosOrdenados = SemaforoPorIntervalo.OrdenarSemaforos( semaforosParaAguardar, semaforosNovos );

            Assert.IsNotNull( semaforosOrdenados, "Deveria conter os semáforos ordenados." );
            Assert.AreEqual( semaforo1, semaforosOrdenados[0], "Deveria ser o 1 semáforo." );
            Assert.AreEqual( semaforo5, semaforosOrdenados[4], "Deveria ser o 5 semáforo." );
        }
    }
}
