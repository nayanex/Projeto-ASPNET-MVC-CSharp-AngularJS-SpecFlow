/*exported WEX*/

"use strict";

var WEX = {};

/* Extensões de tipos nativos */

// Não é possível excluir esta função sem refatorar WEX.projetos.consertaDatas() Ass.: Thiago ;*
String.prototype.fromJSONDate = function () {
	var data;

	data = /\/(Date\(\d+\))\//;

	return eval(this.replace(data, "new $1;"));
};

Object.contem = function (objeto, atributo) {
	var chave;

	for (chave in objeto) {
		if (objeto[chave] === atributo) {
			return true;
		}
	}

	return false;
};

Object.replace = function (antigo, novo) {
	var attr;

	for (attr in antigo) {
		if (antigo.hasOwnProperty(attr)) {
			delete antigo[attr];
		}
	}

	for (attr in novo) {
		if (novo.hasOwnProperty(attr)) {
			antigo[attr] = novo[attr];
		}
	}

	return antigo;
};

Array.prototype.find = function (objeto) {
	return this.filter(function (elemento) {
		return Object.contem(elemento, objeto);
	});
};

Array.prototype.replace = function (valoresNovos) {
	Array.prototype.splice.apply(this, [0, this.length].concat(valoresNovos));
};

Array.prototype.remove = function (elem) {
	var index;

	index = this.indexOf(elem);

	if (index > -1) {
		this.splice(index, 1);
	}
};

Array.prototype.unique = function () {
	var o = {}, i, l = this.length, r = [];
	for (i = 0; i < l; i += 1) o[this[i]] = this[i];
	for (i in o) r.push(o[i]);
	return r;
};

var FlagManager = function () {

	var flags = {};

	var _flag = function (args) {
	    return Array.prototype.slice.call(args).join("_");
	}

	this.check = function () {
	    return _flag(arguments) in flags || false;
	};

	this.up = function () {
	    flags[_flag(arguments)] = true;
	};

	this.down = function () {
	    delete flags[_flag(arguments)];
	}

	this.list = function () {
	    var flagList = [];
	    for (var flag in flags) {
	        stateList.push(flag);
	    }
	    return flagList;
	};

	this.clear = function () {
	    for (var flag in flags) {
	        delete flags[flag];
	    }
	};

};

var HashMap = function () {

	var map = {};

	this.put = function (key, value) {
	    map[key] = value;
	};

	this.get = function (key) {
	    return map[key];
	};

	this.containsKey = function (key) {
	    return key in map;
	};

	this.remove = function (key) {
	    delete map[key];
	};

	this.clear = function () {
	    for (var key in map) {
	        delete map[key];
	    }
	};

};