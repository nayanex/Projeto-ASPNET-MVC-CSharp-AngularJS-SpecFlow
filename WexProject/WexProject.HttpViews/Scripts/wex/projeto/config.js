/*global WEX*/

(function () {
    "use strict";

    WEX.Projeto = WEX.Projeto || {};

    WEX.Projeto.ProjetoDTO = function () {
        this.IdProjeto = null;
        this.Nome = null;
        this.InicioPlanejado = null;
        this.InicioReal = null;
        this.TerminoReal = null;
        this.Gerente = null;
        this.CentroCusto = null;
        this.ProjetoMacro = null;
        this.Situacao = null;
        this.Clientes = [];
        this.IsMacro = true;
        this.HasProjetosMicros = false;
    };

})();