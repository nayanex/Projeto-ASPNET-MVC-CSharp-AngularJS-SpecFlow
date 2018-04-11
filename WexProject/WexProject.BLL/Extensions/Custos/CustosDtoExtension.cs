using System;
using System.Linq;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Extensions.Custos
{
	/// <summary>
	/// Métodos de extensão para realizar conversão entre modelos e DTOs.
	/// </summary>
	internal static class CustosDtoExtension
	{
		public static WexProject.BLL.Shared.DTOs.Custos.ProjetoDto ToDto(this Projeto projeto)
		{
			return new WexProject.BLL.Shared.DTOs.Custos.ProjetoDto()
			{
				Oid = projeto.Oid,
				Nome = projeto.TxNome,
				TipoProjetoId = projeto.TipoProjetoId ?? 0,
				DataInicial = projeto.DtInicioPlan ?? new DateTime(),
				DataFinal = projeto.DtTerminoPlan ?? new DateTime(),
				Valor = projeto.NbValor,
				Duracao = ((projeto.DtTerminoPlan ?? new DateTime()).Subtract(projeto.DtInicioPlan ?? new DateTime()).Days + 29) / 30,
				Status = (int) projeto.CsSituacaoProjeto,
				Clientes = projeto.ProjetoClientes.Select(pc => pc.EmpresaInstituicao.TxNome).ToList(),
				// TODO: Remover propriedade Cliente de ProjetoDto.
				Cliente = projeto.ProjetoClientes.Count > 0 ? projeto.ProjetoClientes.First().EmpresaInstituicao.TxNome : "",
				Gerente = projeto.Gerente != null ? projeto.Gerente.NomeCompleto : "",
				CentroCustoId = projeto.CentroCustoId,
				CentroCusto = projeto.CentroCusto != null ? projeto.CentroCusto.Nome : "",
				ProjetoMacroOid = projeto.ProjetoMacroOid
			};
		}

		public static EmpresaInstituicaoDto ToDto(this EmpresaInstituicao empresaInstituicao)
		{
			return new EmpresaInstituicaoDto()
			{
				Oid = empresaInstituicao.Oid,
				Nome = empresaInstituicao.TxNome,
				Sigla = empresaInstituicao.TxSigla,
				Email = empresaInstituicao.TxEmail,
				FoneFax = empresaInstituicao.TxFoneFax
			};
		}

		public static RubricaMesDto ToDto(this RubricaMes rubricaMes)
		{
			return new RubricaMesDto
			{
				RubricaMesId = rubricaMes.RubricaMesId,
				RubricaId = rubricaMes.RubricaId,
				Mes = rubricaMes.CsMes,
				Ano = rubricaMes.NbAno,
				PossuiGastosRelacionados = rubricaMes.PossuiGastosRelacionados,
				Planejado = rubricaMes.NbPlanejado,
				Replanejado = rubricaMes.NbReplanejado,
				Gasto = rubricaMes.NbGasto
			};
		}

		public static RubricaMes FromDto(this RubricaMesDto rubricaMesDto)
		{
			return new RubricaMes()
			{
				RubricaMesId = rubricaMesDto.RubricaMesId,
				RubricaId = rubricaMesDto.RubricaId,
				NbAno = rubricaMesDto.Ano,
				CsMes = rubricaMesDto.Mes,
				PossuiGastosRelacionados = rubricaMesDto.PossuiGastosRelacionados,
				NbPlanejado = rubricaMesDto.Planejado,
				NbReplanejado = rubricaMesDto.Replanejado,
				NbGasto = rubricaMesDto.Gasto
			};
		}

		/// <summary>
		/// Transforma modelo de Mão de Obra em DTO.
		/// </summary>
		/// <param name="maoDeObra">Modelo Mão de Obra.</param>
		/// <returns>DTO Mão de Obra.</returns>
		public static MaoDeObraDto ToDto(this MaoDeObra maoDeObra)
		{
			return new MaoDeObraDto()
			{
				MaoDeObraId = maoDeObra.MaoDeObraId,
                Matricula = maoDeObra.Matricula,
				Nome = maoDeObra.Nome,
                Cargo = maoDeObra.Cargo,
				PercentualAlocacao = maoDeObra.PercentualAlocacao,
				ValorTotalSemProvisoes = maoDeObra.ValorTotalSemProvisoes,
				ValorTotal = maoDeObra.ValorTotal,
				LoteId = maoDeObra.LoteId
				
			};
		}

		/// <summary>
		/// Transforma DTO de Mão de Obra em modelo.
		/// </summary>
		/// <param name="maoDeObraDto">DTO Mão de Obra.</param>
		/// <returns>Modelo Mão de Obra</returns>
		public static MaoDeObra FromDto(this MaoDeObraDto maoDeObraDto)
		{
			return new MaoDeObra()
			{
				MaoDeObraId = maoDeObraDto.MaoDeObraId,
                Matricula = maoDeObraDto.Matricula,
                Nome = maoDeObraDto.Nome,
                Cargo = maoDeObraDto.Cargo,
				PercentualAlocacao = maoDeObraDto.PercentualAlocacao,
				ValorTotalSemProvisoes = maoDeObraDto.ValorTotalSemProvisoes,
				ValorTotal = maoDeObraDto.ValorTotal,
				LoteId = maoDeObraDto.LoteId
			};
		}

		/// <summary>
		/// Tranforma modelo de Lote de Mão de Obra em DTO.
		/// </summary>
		/// <param name="lote">Modelo Lote de Mão de Obra.</param>
		/// <returns>DTO Lote de Mão de Obra.</returns>
		public static LoteMaoDeObraDto ToDto(this LoteMaoDeObra lote)
		{
			return new LoteMaoDeObraDto()
			{
				LoteId = lote.LoteId,
				DataAtualizacao = lote.DataAtualizacao,
				CentroCustoImportacao = lote.CentroCustoImportacao,
				RubricaMesId = lote.RubricaMesId
			};
		}

		/// <summary>
		/// Tranforma DTO de Lote de Mão de Obra em modelo.
		/// </summary>
		/// <param name="loteDto">DTO de Lote de Mão de Obra.</param>
		/// <returns>Modelo de Lote de Mão de Obra.</returns>
		public static LoteMaoDeObra FromDto(this LoteMaoDeObraDto loteDto)
		{
			return new LoteMaoDeObra()
			{
				LoteId = loteDto.LoteId,
				DataAtualizacao = loteDto.DataAtualizacao,
				CentroCustoImportacao = loteDto.CentroCustoImportacao,
				RubricaMesId = loteDto.RubricaMesId
			};
		}
	}
}
