/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.custos.config.js" />
describe('wex.custos.config.js', function() {
  "use strict";

  it("WEX.custo precisa estar definido", function() {
    expect(WEX.custo).toBeDefined();
  });
  
  it("WEX.custo.aditivo precisa estar definido", function() {
    expect(WEX.custo.aditivo).toBeDefined();
  });

  it("O tempo para salvar automaticamente precisa ser 3000ms", function() {
    expect(WEX.custo.timeoutSalvar).toBe(3000);
  });
});
