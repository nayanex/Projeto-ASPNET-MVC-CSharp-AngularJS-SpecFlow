/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.analise.config.js" />
/// <reference path="~/Scripts/wex.analise.geral.js" />
/// <reference path="~/Scripts/jasmine.js" />
describe('wex.analise.geral.js', function() {
    "use strict";

    it("WEX.analise.geral.GeralModulo precisa estar definido", function() {
        expect(WEX.analise.geral.GeralModulo).toBeDefined();
    });
});