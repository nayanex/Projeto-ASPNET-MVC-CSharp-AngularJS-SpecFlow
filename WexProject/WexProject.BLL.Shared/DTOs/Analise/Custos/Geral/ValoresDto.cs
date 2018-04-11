using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos.Geral
{
	public class ValoresDto
	{
		public ValoresDto()
		{
			this.Anos = new Dictionary<String, Custo[]>();
		}

		public Decimal Total { get; set; }
		public Dictionary<String, Custo[]> Anos { get; set; }

	}

	public struct Custo
	{
		public Decimal? Valor;
		public Boolean Previsao;

		public Custo(Decimal? custo)
		{
			Valor = custo;
			Previsao = false;
		}

		public Custo(Custo valor)
		{
			Valor = valor.Valor;
			Previsao = valor.Previsao;
		}

		public static implicit operator Custo (Decimal? custo)
		{
			return new Custo(custo);
		}

		public static explicit operator Decimal(Custo custo)
		{
			return custo.Valor.GetValueOrDefault(0);
		}

		public static Custo operator +(Custo valor, Custo custo)
		{
			Custo v = valor + custo.Valor;

			if (valor.Previsao || custo.Previsao)
			{
				v.Previsao = true;
			}

			return v;
		}

		public static Custo operator +(Custo valor, Decimal? custo)
		{
			Custo v = new Custo(valor);

			if (v.Valor.HasValue || custo.HasValue)
			{
				v.Valor = v.Valor.GetValueOrDefault() + custo.GetValueOrDefault();
			}
			else
			{
				v.Valor = null;
			}

			return v;
		}

		public static Custo operator -(Custo valor, Custo custo)
		{
			Custo v = valor - custo.Valor;

			if (valor.Previsao || custo.Previsao)
			{
				v.Previsao = true;
			}

			return v;
		}

		public static Custo operator -(Custo valor, Decimal? custo)
		{
			Custo v = new Custo(valor);

			if (v.Valor.HasValue || custo.HasValue)
			{
				v.Valor = v.Valor.GetValueOrDefault() - custo.GetValueOrDefault();
			}
			else
			{
				v.Valor = null;
			}

			return v;
		}
	}
}