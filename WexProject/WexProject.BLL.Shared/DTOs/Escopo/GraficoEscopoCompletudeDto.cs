using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Escopo
{
    [DataContract]
    public class GraficoEscopoCompletudeDTO
    {
        [DataMember]
        public string Modulo { get; set; }
        [DataMember]
        public double PontoNaoIniciados { get; set; }
        [DataMember]
        public double PercNaoInciado { get; set; }
        [DataMember]
        public double PontosEmAnalise { get; set; }
        [DataMember]
        public double PercEmAnalise { get; set; }
        [DataMember]
        public double PontosEmDesenv { get; set; }
        [DataMember]
        public double PercEmDesenv { get; set; }
        [DataMember]
        public double PontosProntos { get; set; }
        [DataMember]
        public double PercProntos { get; set; }
        [DataMember]
        public double PontosDesvio { get; set; }
        [DataMember]
        public double PercDesvio { get; set; }
        [DataMember]
        public double PontosMudanca { get; set; }
        [DataMember]
        public double PercMudanca { get; set; }

        [DataMember]
        public double TotalPontosModulo { get; set; }

        public double SomaModulo { get; set; }
    }
}
