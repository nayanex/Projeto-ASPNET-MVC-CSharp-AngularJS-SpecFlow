using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Extensions.Custos;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.BLL.BOs.Custos
{
	public class TermoAditivoBo
	{
		private static TermoAditivoBo instancia;

		public static TermoAditivoBo Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new TermoAditivoBo();
				}

				return instancia;
			}
		}

		private TermoAditivoBo()
		{
		}

		public Boolean IsTermoAditivoVazio(TermoAditivo termoAditivo)
		{
			if (termoAditivo.NbAporteTotal != 0)
			{
				return false;
			}

			if (termoAditivo.Projetos.Count != 0)
			{
				return false;
			}

			return true;
		}

		public int SalvarTermoAditivo(TermoAditivoDto termoAditivoDto)
		{
			var termoAditivo = new TermoAditivo()
			{
				TermoAditivoId = termoAditivoDto.TermoAditivoId,
				TxNome = termoAditivoDto.Nome,
				TxDescricao = termoAditivoDto.Descricao,
				DtInicio = termoAditivoDto.DataInicio,
				DtTermino = termoAditivoDto.DataTermino,
				PatrocinadorId = termoAditivoDto.Patrocinador.Oid,
				NbAporteTotal = termoAditivoDto.AporteTotal
			};

			return TermoAditivoDao.Instancia.SalvarTermoAditivo(termoAditivo);
		}

		public int ExcluirTermoAditivo(int termoAditivoId, Boolean force)
		{
			var termoAditivo = TermoAditivoDao.Instancia.ConsultarTermoAditivoPorId(termoAditivoId);

			if (termoAditivo == null)
			{
				throw new TermoAditivoNaoEncontradoException(String.Format("Termo Aditivo com id ({0}) não encontrado!", termoAditivoId));
			}

			if (force || IsTermoAditivoVazio(termoAditivo))
			{
				return TermoAditivoDao.Instancia.ExcluirTermoAditivo(termoAditivo);
			}

			throw new TermoAditivoNaoVazioException(String.Format("Termo Aditivo {0} não está vazio. Use force para remover.", termoAditivoId));
		}

		public List<TermoAditivoDto> ListarTermoAditivo()
		{
			var termosAditivosDto = (from ta in TermoAditivoDao.Instancia.ListarTermoAditivo()
									 select ta.ToDto()).ToList();

			return termosAditivosDto;
		}

		public void AssociarProjeto(int termoAditivoId, Guid projetoOid)
		{
			var projeto = ProjetoDao.Instancia.ConsultarProjetoPorOid(projetoOid);

			// TODO: MEO DEOS! Falta fazer a verificação de datas e talz!

			if (projeto.TermoAditivoId.HasValue && projeto.TermoAditivoId.Value != 0)
			{
				var termoAditivo = TermoAditivoDao.Instancia.ConsultarTermoAditivoPorId(projeto.TermoAditivoId.Value);
				throw new ProjetoJaAssociadoATermoAditivoException(String.Format("Projeto {0}({1}) já está associado ao Termo Aditivo {2}({3})", projeto.TxNome, projeto.Oid, termoAditivo.TxNome, termoAditivoId));
			}

			projeto.TermoAditivoId = termoAditivoId;

			ProjetoDao.Instancia.SalvarProjeto(projeto);
		}


		public void DisassociarProjeto(int termoAditivoId, Guid projetoOid)
		{
			var projeto = ProjetoDao.Instancia.ConsultarProjetoPorOid(projetoOid);

			if (projeto.NbValor > 0)
			{
				throw new ProjetoComValorException(String.Format("Projeto {0}({1}) possui valor de {2}. Use force para disassociar.", projeto.TxNome, projeto.Oid, projeto.NbValor));
			}

			projeto.TermoAditivoId = null;

			ProjetoDao.Instancia.SalvarProjeto(projeto);
		}
	}
}
