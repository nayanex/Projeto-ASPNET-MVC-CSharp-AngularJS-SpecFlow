/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.projetos.config.js" />
describe('wex.projetos.config.js', function() {
  "use strict";

  it("WEX.projetos precisa estar definido", function() {
    expect(WEX.projetos).toBeDefined();
  });
});
