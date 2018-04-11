using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.Externo.Totvs.Contexto;

namespace WexProject.Externo.Totvs.DAOs.Custos
{
	public class MaoDeObraDao
	{
		private static MaoDeObraDao instancia;

		public static MaoDeObraDao Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new MaoDeObraDao();
				}

				return instancia;
			}
		}

		private MaoDeObraDao()
		{
		}

        public List<EVENTOSRH> ConsultarMaosDeObra(String centroCustoCodigo, int ano, int mes)
		{
            List<EVENTOSRH> eventos = new List<EVENTOSRH>();

			using (var _db = new TotvsWexEntities())
			{

                String anomes = ano.ToString() + mes.ToString("D2");

                eventos = _db.EVENTOSRH.Where(e => e.RD_CC == centroCustoCodigo && e.FLAG == "A" && e.ANOMES == anomes).ToList();

                if (eventos.Any())
                {
                    Double maxControle = eventos.Max(o => o.CONTROLE);
                    eventos.RemoveAll(e => e.CONTROLE != maxControle);
                }

			}

            return eventos;
		}

        public int ConsultarCodigoImportacao(String centroCustoCodigo, int ano, int mes)
        {

            Double ultimaImportacao = 0;

            using (var _db = new TotvsWexEntities())
            {

                String anomes = ano.ToString() + mes.ToString("D2");

                var eventos = _db.EVENTOSRH.Where(e => e.RD_CC == centroCustoCodigo && e.FLAG == "A" && e.ANOMES == anomes);

                if (eventos.Any()) {
                    ultimaImportacao = eventos.Max(e => e.CONTROLE);
                }

            }

            return Convert.ToInt32(ultimaImportacao);
        }

	}
}
