using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;
using log4net;
using log4net.Config;

namespace WexProject.BLL.DAOs.Custos
{
	/// <summary>
	/// DAO de Mãos de Obra.
	/// </summary>
	internal class MaosDeObraDao : IMaosDeObraDao
	{
		private static MaosDeObraDao instancia;

		/// <summary>
		/// Instância `singleton` do DAO de Mão de Obra.
		/// </summary>
		public static MaosDeObraDao Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new MaosDeObraDao();
				}

				return instancia;
			}
		}

		private MaosDeObraDao()
		{
		}

		/// <summary>
		/// Consulta no Banco de Dados, Mãos de Obra e Lote mais atualizados de um determinado Mês e Ano de um Projeto.
		/// Retorna os resultados através de Referências.
		/// </summary>
		/// <param name="centroCustoId">Id do Centro de Custo.</param>
		/// <param name="rubricaMesId">Id do Mês da Rúbrica.</param>
		/// <param name="lote">Referência de Lote de Mão de Obra.</param>
		/// <param name="maosDeObra">Referência de Mão de Obra.</param>
		public void ConsultarMaosDeObra(int centroCustoId, int rubricaMesId, out LoteMaoDeObra lote, out List<MaoDeObra> maosDeObra)
		{
            maosDeObra = null;

			using (var _db = new WexDb())
			{
                lote = (from l in _db.Lotes.Include("MaosDeObra").Include("RubricaMes")
						where l.CentroCustoImportacao == centroCustoId && l.RubricaMesId == rubricaMesId
						orderby l.LoteId descending
						select l).FirstOrDefault();			
			}

            if (lote != null)
            {
                maosDeObra = lote.MaosDeObra.OrderBy(m => m.Matricula).ToList();
                lote.MaosDeObra = null;
            }
            
		}

		/// <summary>
		/// Salva dados de Mão de Obra no Banco de Dados.
		/// </summary>
		/// <param name="maoDeObra">Modelo de Mão de Obra.</param>
		/// <returns>Id da Mão de Obra salva.</returns>
		public int SalvarMaoDeObra(MaoDeObra maoDeObra)
		{
			using (var _db = new WexDb())
			{
				if (maoDeObra.MaoDeObraId == 0)
				{
					_db.MaosDeObra.Add(maoDeObra);
				}
				else
				{
					_db.Entry(maoDeObra).State = EntityState.Modified;
				}

				_db.SaveChanges();
			}

			return maoDeObra.MaoDeObraId;
		}

		/// <summary>
		/// Consulta no Banco de Dados, Lote de Mão de Obra mais atualizado de um determinado Mês e Ano de um Projeto.
		/// </summary>
		/// <param name="centroCustoId">Id de Centro de Custo.</param>
		/// <param name="ano">Ano do lote.</param>
		/// <param name="mes">Mês do lote.</param>
		/// <returns>Modelo de Lote de Mão de Obra.</returns>
		public LoteMaoDeObra ConsultarLote(int centroCustoId, int ano, int mes)
		{
			LoteMaoDeObra lote;

    		using (var _db = new WexDb())
			{
				lote = (from l in _db.Lotes
						where l.CentroCustoImportacao == centroCustoId && l.RubricaMes.NbAno == ano && (int) l.RubricaMes.CsMes == mes
                        orderby l.LoteId descending
						select l).FirstOrDefault();
			}

			return lote;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lote">Modelo de Lote de Mão de Obra.</param>
		/// <returns>Id de Lote salvo.</returns>
		public int SalvarLote(LoteMaoDeObra lote)
		{
			using (var _db = new WexDb())
			{

				if (lote.LoteId == 0)
				{
					_db.Lotes.Add(lote);
				}
				else
				{
					_db.Entry(lote).State = EntityState.Modified;
				}
				    
                _db.SaveChanges();

			}

			return lote.LoteId;
		}
	}
}
