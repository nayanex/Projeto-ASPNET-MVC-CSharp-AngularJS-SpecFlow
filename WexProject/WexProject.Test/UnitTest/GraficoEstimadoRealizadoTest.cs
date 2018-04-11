using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Shared.DTOs.Escopo;
using WexProject.BLL.BOs.Escopo;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class GraficoEstimadoRealizadoTest
    {
        [TestMethod]
        public void verificarCalculoRitimoInicio()
        {
            Assert.AreEqual(10, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido(null, 50, 5));
        }
     
        [TestMethod]
        public void verificarCalculoRitimoMeioProjetoAtrasado()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 2, Realizado = 8 };

            Assert.AreEqual( 4, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 20, 5 ) );
        }

        [TestMethod]
        public void verificarCalculoRitimoFinalAtrasado()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 5, Realizado = 25 };

            Assert.AreEqual( 25, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 50, 5 ) );
        }

        [TestMethod]
        public void verificarCalculoRitimoMeioProjetoAdiantado()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 2, Realizado = 17 };

            Assert.AreEqual( 1, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 20, 5 ) );
        }

        [TestMethod]
        public void verificarCalculoRitimoMeioProjetoFinalizado()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 2, Realizado = 20 };

            Assert.AreEqual( 0, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 20, 5 ) );
        }


        [TestMethod]
        public void verificarCalculoRitimoMeioProjetoAtrasadoII()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 15, Realizado = 10 };

            Assert.AreEqual( 4, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 150, 50 ) );
        }

        [TestMethod]
        public void verificarCalculoRitimoMeioProjetoFinalizadoMenorQueZero()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 2, Realizado = 19 };

            Assert.AreEqual( 0.33, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 20, 5 ) );
        }

        [TestMethod]
        public void verificarCalculoRitimoMeioProjetoFinalizadoMenorQueZeroII()
        {
            GraficoEstimadoRealizadoDTO aux = new GraficoEstimadoRealizadoDTO() { Ciclo = 3, Realizado = 19 };

            Assert.AreEqual( 0.5, GraficoEstimadoRealizadoBO.CalcularRitmoSugerido( aux, 20, 5 ) );
        }
    }
}
