/*global WEX*/

(function () {
    "use strict";

    WEX.GastosRelacionados = WEX.GastosRelacionados || {};

    WEX.GastosRelacionados.GastoDTO = function () {
        this.GastoId = null;
        this.CentroDeCustoId = null;
        this.CentroDeCustoDesc = null;
        this.RubricaId = null;
        this.Descricao = null;
        this.HistoricoLancamento = null;
        this.Justificativa = null;
        this.Data = null;
        this.Valor = null;
    };

})();