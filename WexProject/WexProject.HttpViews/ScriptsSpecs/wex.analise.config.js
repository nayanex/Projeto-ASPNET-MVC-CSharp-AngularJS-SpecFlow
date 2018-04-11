/// <reference path="~/Scripts/wex.main.js"/>
/// <reference path="~/Scripts/wex.analise.config.js"/>
/// <reference path="~/Scripts/wex.analise.custos.js"/>
/// <reference path="~/Scripts/wex.analise.geral.js"/>
describe("wex.analise.config.js", function() {
  "use strict";

  it("WEX.analise deve estar definido", function() {
    expect(WEX.analise).toBeDefined();
  });

  it("WEX.analige.geral deve estar definido", function() {
    expect(WEX.analise.geral).toBeDefined();
  });
});

