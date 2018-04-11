/*global WEX*/

(function () {
	"use strict";

	WEX.custo = {};
	WEX.custo.aditivo = {};

	WEX.custo.timeoutSalvar = 3000;

	WEX.custo._linhaEditavel =      "<td><button title='Excluir Aditivo' class='excluiAditivo iconeExcluir'></button></td>" +
                                    "<td>" +
									"	<input type='text' form='alteraAditivo' name='TxNome' maxlength='180'>" +
									"	<span></span>" +
									"</td>" +
									"<td>" +
									"	<input type='date' form='alteraAditivo' name='DtInicio'>" +
									"	<span></span>" +
									"</td>" +
									"<td>" +
									"	<input type='date' form='alteraAditivo' name='DtTermino'>" +
									"	<span></span>" +
									"</td>" +
									"<td>" +
									"	<input id='duracaoAditivo' type='number' readonly='readonly'>" +
									"	<span></span>" +
									"</td>" +
									"<td>" +
									"	<input type='text' form='alteraAditivo' name='NbOrcamento'>" +
									"	<span></span>" +
									"</td>" +
									"<td>" +
									"	<input id='despesasAditivo' type='text' readonly='readonly' value='0,00'>" +
									"	<span></span>" +
									"</td>" +
									"<td>" +
									"	<input id='saldoAditivo' type='text' readonly='readonly'>" +
									"	<span></span>" +
									"</td>";
	WEX.custo._linhaPatrocinador =	"<tr>" +
										"	<td></td>" +
										"	<td><button title='Excluir Patrocinador' class='excluiPatrocinador iconeExcluir'></button></td>"	+
										"</tr>";
	WEX.custo._linhaCentroCusto = "<tr>" +
									"	<td></td>" +
									"	<td></td>" +
									"	<td><button title='Excluir Centro de Custo' class='excluiCentroCusto iconeExcluir'></button></td>" +
									"</tr>";
})();