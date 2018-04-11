/*global WEX*/

(function () {
    "use strict";

    WEX.MaoDeObra = WEX.MaoDeObra || {};

    WEX.MaoDeObra.CustoMaoObraDTO = function () {
        this.Matricula = null;
        this.Nome = null;
        this.PercentualAlocacao = null;
        this.TotalGastoSemProvisoes = null;
        this.TotalProvisaoFerias13o = null;
        this.ProvisaoDemissao = null;
        this.ValorTotal = null;
        this.Data = null;
        this.SomaValorTotal = null;
    };

   

    WEX.MaoDeObra.LoteDTO = function () {
        this.Numero = null;
        this.Data = null;
        this.EstaAtualizado = null;
    };


     
})();