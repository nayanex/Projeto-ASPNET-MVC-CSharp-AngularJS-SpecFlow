using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class ConversorTimeSpanTest
    {
        [TestMethod]
        public void ConverterHorasDeTicksParaStringTest()
        {
            string estimativaAtual,estimativaEsperada;
            estimativaEsperada = "05:00";
            estimativaAtual = ConversorTimeSpan.ConverterHorasDeTicksParaString(180000000000);
            Assert.AreEqual(estimativaEsperada,estimativaAtual,"A estimativa convertida deveria ser a mesma da estimativaAtual");

            estimativaEsperada = "10:02";
            estimativaAtual = ConversorTimeSpan.ConverterHorasDeTicksParaString(361200000000);
            Assert.AreEqual(estimativaEsperada,estimativaAtual,"A estimativa convertida deveria ser a mesma da estimativaAtual");

            estimativaEsperada = "05:00";
            estimativaAtual = ConversorTimeSpan.ConverterHorasDeTicksParaString(180000000000);
            Assert.AreEqual(estimativaEsperada,estimativaAtual,"A estimativa convertida deveria ser a mesma da estimativaAtual pois foi arredondado");
        }

        [TestMethod]
        public void ConverterHorasDeStringParaTicksTest() 
        {
            long estimativaAtual,estimativaEsperada;
            estimativaEsperada = 361200000000;
            estimativaAtual = ConversorTimeSpan.ConverterHorasDeStringParaTicks("10:02");
            Assert.AreEqual(estimativaEsperada,estimativaAtual,"A estimativa convertida deveria ser a mesma da estimativaAtual");

            estimativaEsperada = 180000000000;
            estimativaAtual = ConversorTimeSpan.ConverterHorasDeStringParaTicks("5:00");
            Assert.AreEqual(estimativaEsperada,estimativaAtual,"A estimativa convertida deveria ser a mesma da estimativaAtual");
        }

        [TestMethod]
        public void DeveCalcularDeTimeSpanOFormatoStringAQuantidadeDeHorasInformada()
        {
            Assert.AreEqual( "24:50", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 24, 50, 0 ) ) );
            Assert.AreEqual( "99:00", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 99, 0, 0 ) ) );
            Assert.AreEqual( "99:00", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 98, 60, 0 ) ) );
            Assert.AreEqual( "99:00", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 98, 60, 15 ) ) );
        }

        [TestMethod]
        public void DeveDesconsiderarDoCalculoHorasAcimaDe9999()
        {
            //Tamanho máximo permitido 99:00
            Assert.AreEqual( "99:99", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 200, 0, 0 ) ) );
            Assert.AreEqual( "99:60", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 100, 0, 0 ) ) );
            Assert.AreEqual( "99:99", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 99, 100, 0 ) ) );
            Assert.AreEqual( "99:60", ConversorTimeSpan.CalcularStringHoras( new TimeSpan( 99, 60, 0 ) ) );
        }

        [TestMethod]
        public void DeveCalcularDoFormatoStringParaOFormatoTimeSpanAQuantidadeDeHorasInformadas()
        {
            Assert.AreEqual( new TimeSpan( 1, 0, 0 ), ConversorTimeSpan.CalcularHorasTimeSpan( "01:00" ) );
            Assert.AreEqual( new TimeSpan( 2, 30, 0 ), ConversorTimeSpan.CalcularHorasTimeSpan( "02:30" ) );
            Assert.AreEqual( new TimeSpan( 1, 1, 0, 0 ), ConversorTimeSpan.CalcularHorasTimeSpan( "25:00" ) );
            Assert.AreEqual( new TimeSpan( 35, 0, 0 ), ConversorTimeSpan.CalcularHorasTimeSpan( "35:00" ) );
            Assert.AreEqual( new TimeSpan( 99, 99, 0 ), ConversorTimeSpan.CalcularHorasTimeSpan( "99:99" ) );
        }

        [TestMethod]
        public void DeveConverterOsMinutosInformadosParaHorasQuandoMinutosNaoUltrapassaremUmaHora()
        {
            const int quantidade1 = 0;
            const int quantidade2 = 1;
            const int quantidade3 = 30;
            const int quantidade4 = 59;

            Assert.AreEqual( 0, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade1 ), string.Format( "Deveria ter convertido {0} minuto(s) para 0(zero) horas.", quantidade1 ) );
            Assert.AreEqual( 0, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade2 ), string.Format( "Deveria ter convertido {0} minuto(s) para 0(zero) horas.", quantidade2 ) );
            Assert.AreEqual( 0, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade3 ), string.Format( "Deveria ter convertido {0} minuto(s) para 0(zero) horas.", quantidade3 ) );
            Assert.AreEqual( 0, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade4 ), string.Format( "Deveria ter convertido {0} minuto(s) para 0(zero) horas.", quantidade4 ) );
        }

        [TestMethod]
        public void DeveConverterOsMinutosInformadosParaHorasQuandoMinutosUltrapassaremUmaHora()
        {
            const int quantidade1 = 60;
            const int quantidade2 = 120;
            const int quantidade3 = 1200;

            const int resultado1 = 1;
            const int resultado2 = 2;
            const int resultado3 = 20;

            Assert.AreEqual( resultado1, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade1 ), string.Format( "Deveria ter convertido {0} minuto(s) para {1} hora(s).", quantidade1, resultado1 ) );
            Assert.AreEqual( resultado2, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade2 ), string.Format( "Deveria ter convertido {0} minuto(s) para {1} hora(s).", quantidade2, resultado2 ) );
            Assert.AreEqual( resultado3, ConversorTimeSpan.ConverterMinutosParaHorasInteiras( quantidade3 ), string.Format( "Deveria ter convertido {0} minuto(s) para {1} hora(s).", quantidade3, resultado3 ) );
        }

        [TestMethod]
        public void DeveConverterParaStringEDepoisRetornarParaTimeSpanUmValorTimesPan()
        {
            TimeSpan tempo = new TimeSpan( 99, 99, 0 );
            TimeSpan tempoConvertido;
            string tempoEmString = ConversorTimeSpan.CalcularStringHoras( tempo );
            tempoConvertido = ConversorTimeSpan.CalcularHorasTimeSpan( tempoEmString );
            Assert.AreEqual( tempo, tempoConvertido,"Deveria totalizar o mesmo valor" );
            Assert.AreEqual("99:99",tempoEmString);
        }

        [TestMethod]
        public void DeveConverterUmTimeSpanPorExtenso()
        {
            TimeSpan minutos = new TimeSpan( 10 * TimeSpan.TicksPerMinute );
            TimeSpan hora = new TimeSpan( TimeSpan.TicksPerHour );
            TimeSpan horas = new TimeSpan( 2 * TimeSpan.TicksPerHour );
            TimeSpan dia = new TimeSpan(TimeSpan.TicksPerDay);
            TimeSpan dias = new TimeSpan( 2 * TimeSpan.TicksPerDay );

            Assert.AreEqual( "10min", minutos.PorExtenso() );
            Assert.AreEqual( "1h", hora.PorExtenso() );
            Assert.AreEqual( "2h", horas.PorExtenso() );
            Assert.AreEqual( "1d", dia.PorExtenso() );
            Assert.AreEqual( "2d", dias.PorExtenso() );

            Assert.AreEqual( "4d 4h 39min", new TimeSpan( 99, 99, 0 ).PorExtenso() );
            Assert.AreEqual( "1h 20min", new TimeSpan( 1, 20, 5 ).PorExtenso() );
            Assert.AreEqual( "24min", new TimeSpan( 0, 24, 0 ).PorExtenso() );
            Assert.AreEqual( "1h 24min", new TimeSpan( 1, 24, 0 ).PorExtenso() );
            Assert.AreEqual( "1h 15min", new TimeSpan( 0, 74, 60 ).PorExtenso() );
            Assert.AreEqual( "1h", new TimeSpan( 1, 0, 0 ).PorExtenso() );
            Assert.AreEqual( "1d", new TimeSpan( 24, 0, 0 ).PorExtenso() );
            Assert.AreEqual( "0", new TimeSpan( 0, 0, 15 ).PorExtenso() );
        }

        [TestMethod]
        public void DeveConverterUmTimeSpanPorExtensoEIgnorarValoresMenoresQueUmMinuto()
        {
            Assert.AreEqual( "0", new TimeSpan( 0, 0, 0 ).PorExtenso() );
            Assert.AreEqual( "0", new TimeSpan( 0, 0, 1 ).PorExtenso() );
            Assert.AreEqual( "0", new TimeSpan( 0, 0, -10 ).PorExtenso() );
            Assert.AreEqual( "0", new TimeSpan( 0, 0, 15 ).PorExtenso() );
            Assert.AreEqual( "0", new TimeSpan( 0, 0, 20 ).PorExtenso() );
            Assert.AreEqual( "0", new TimeSpan( 0, 0, 59 ).PorExtenso() );
            Assert.AreEqual( "1min", new TimeSpan( 0, 0, 60 ).PorExtenso() );
            Assert.AreEqual( "2min", new TimeSpan( 0, 0, 120 ).PorExtenso() );
            Assert.AreEqual( "6min", new TimeSpan( 0, 0, 360 ).PorExtenso() );
        }
    }
}
