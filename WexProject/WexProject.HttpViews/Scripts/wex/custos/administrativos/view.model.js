(function () {
    "use strict";

    WEX.custo.administrativos.ViewModel = function () {

        var viewModel = {};

        viewModel.flags = new FlagManager();

        viewModel.dataConsulta = new Date();

        viewModel.custosRubricas = [];
        viewModel.custosProjetos = new HashMap();

        viewModel.total = {
            OrcamentoAprovado: 0,
            SaldoDisponivel: 0,
            DespesaReal: 0
        };

        return viewModel;

    };

})();