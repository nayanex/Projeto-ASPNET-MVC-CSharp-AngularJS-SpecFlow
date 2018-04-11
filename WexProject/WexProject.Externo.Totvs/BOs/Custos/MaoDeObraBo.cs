using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.Externo.Custos;
using WexProject.Externo.Totvs.DAOs.Custos;

namespace WexProject.Externo.Totvs.BOs.Custos
{
	public class MaoDeObraBo : IMaosDeObraExterno
	{
		private static MaoDeObraBo instancia;

		public static MaoDeObraBo Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new MaoDeObraBo();
				}

				return instancia;
			}
		}

		private MaoDeObraBo()
		{
		}

		public List<MaoDeObraDto> ConsultarMaosDeObra(int centroCusto, int ano, int mes)
		{
			List<MaoDeObraDto> maosDeObra = new List<MaoDeObraDto>();
            var eventosRh = MaoDeObraDao.Instancia.ConsultarMaosDeObra(centroCusto.ToString(), ano, mes);

			foreach (var eventoRh in eventosRh)
			{
				maosDeObra.Add(new MaoDeObraDto()
				{
					Matricula = Int32.Parse(eventoRh.RA_MAT),
					Nome = eventoRh.RA_NOME,
                    Cargo = eventoRh.RJ_DESC,
                    ValorTotalSemProvisoes = (Decimal) eventoRh.FOL_PROV,
                    ValorTotal = (Decimal) eventoRh.TOTALFIM
				});
			}

			return maosDeObra;
		}

        public int ConsultarCodigoImportacao(int centroCustoCodigo, int ano, int mes)
        {
            return MaoDeObraDao.Instancia.ConsultarCodigoImportacao(centroCustoCodigo.ToString(), ano, mes);
        }

	}
}
