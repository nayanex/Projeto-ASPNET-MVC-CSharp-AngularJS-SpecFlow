using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WexProject.BLL.Contexto;
using WexProject.BLL.Contexto.DbFirst;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.TotvsWex;

namespace WexProject.BLL.DAOs.TotvsWex
{
    /// <summary>
    /// </summary>
    public class NotasFiscaisDao
    {
        /// <summary>
        ///     Método para listar os centros de custo vindos do Microsiga
        /// </summary>
        /// <returns>Lista de Centros de Custo</returns>
        public static IEnumerable<CentroCusto> ListarCentroDeCustosTotvs()
        {
            var contexto = new TotvsWexEntities();

            var centroCustos = from cc in contexto.RUBRICA
                select new
                {
                    cc.CENTRO_CUSTO,
                    cc.DESC_CC
                };

            return centroCustos.Distinct()
                .ToList()
                .Select(cc => new CentroCusto
                {
                    Codigo = Convert.ToInt32(cc.CENTRO_CUSTO.Trim()),
                    Nome = cc.DESC_CC
                });
        }

        /// <summary>
        ///     Método para listar as rubricas vindas do Microsiga
        /// </summary>
        /// <returns>Lista de rubricas</returns>
        public static IEnumerable<NotaFiscal> ListaRubricasTotvs()
        {
            using (var contexto = new TotvsWexEntities())
            {
                var notasFiscais = (from notaFiscal in contexto.RUBRICA
                    where notaFiscal.FLAG == "A"
                    select new
                    {
                        Data = notaFiscal.DATA,
                        Lote = notaFiscal.LOTE,
                        SubLote = notaFiscal.SUBLOTE,
                        Documento = notaFiscal.DOCUMENTO,
                        Linha = notaFiscal.LINHA,
                        Descricao = notaFiscal.DESC_CCONTABIL,
                        CentroCustoCodigo = notaFiscal.CENTRO_CUSTO,
                        HistoricoLancamento = notaFiscal.HIST_LANC,
                        Valor = notaFiscal.VALOR_DEBITO - notaFiscal.VALOR_CREDITO,
                        Chave = notaFiscal.CHAVE
                    }).ToList();


                return notasFiscais.Distinct()
                    .ToList()
                    .Select(notaFiscal => new NotaFiscal
                    {
                        Data = notaFiscal.Data,
                        Lote = notaFiscal.Lote,
                        SubLote = notaFiscal.SubLote,
                        Documento = notaFiscal.Documento,
                        Linha = notaFiscal.Linha,
                        Descricao = notaFiscal.Descricao,
                        HistoricoLancamento = notaFiscal.HistoricoLancamento,
                        Valor = (Decimal) notaFiscal.Valor,
                        ChaveImportacao = notaFiscal.Chave,
                        CentroCusto = new CentroCusto
                        {
                            Codigo = Convert.ToInt32(notaFiscal.CentroCustoCodigo.Trim())
                        }
                    });
            }
        }

        /// <summary>
        ///     Adiciona ou atualiza uma nota fiscal
        /// </summary>
        /// <param name="notaFiscal">Nota fiscal</param>
        /// <returns>Id da nota fiscal</returns>
        public static int SalvarNotaFiscal(NotaFiscal notaFiscal)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if (notaFiscal.Id == 0)
                {
                    contexto.NotaFiscal.Add(notaFiscal);
                }
                else
                {
                    contexto.Entry(notaFiscal).State = EntityState.Modified;
                }
                contexto.SaveChanges();
            }
            return notaFiscal.Id;
        }

        /// <summary>
        ///     Consulta as notas ficais de uma rubrica específica
        /// </summary>
        /// <param name="rubrica">Rubrica</param>
        /// <returns>Lista de notas fiscais</returns>
        public static IEnumerable<NotaFiscal> ConsultarNotasFiscais(Rubrica rubrica)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return
                    contexto.NotaFiscal.Where(notaFiscal => notaFiscal.RubricaId == rubrica.RubricaId).ToList();
            }
        }

        /// <summary>
        ///     Consulta as notas ficais de uma rubrica específica em uma determinada data
        /// </summary>
        /// <param name="rubrica">Rubrica</param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns>Lista de notas fiscais</returns>
        public static IEnumerable<NotaFiscal> ConsultarNotasFiscais(Rubrica rubrica, int ano, int mes)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return
                    contexto.NotaFiscal.Where(
                        notaFiscal => notaFiscal.RubricaId == rubrica.RubricaId && notaFiscal.Data.HasValue &&
                                      notaFiscal.Data.Value.Year == ano &&
                                      notaFiscal.Data.Value.Month == mes).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="centroCusto">Centro de custo</param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns>Lista de notas fiscais</returns>
        public static IEnumerable<NotaFiscal> ConsultarNotasFiscais(CentroCusto centroCusto, int ano, int mes)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return
                    contexto.NotaFiscal.Where(
                        notaFiscal => notaFiscal.CentroDeCustoId == centroCusto.CentroCustoId &&
                            notaFiscal.Data.HasValue &&
                            notaFiscal.Data.Value.Year == ano &&
                            notaFiscal.Data.Value.Month == mes).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="notaFiscalId"></param>
        /// <returns></returns>
        public static NotaFiscal ConsultarNotaFiscal(int notaFiscalId)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.NotaFiscal.Find(notaFiscalId);
            }
        }

        /// <summary>
        ///     Verifica se existe alguma nota fiscal importada
        /// </summary>
        /// <param name="chaveImportacao">Código do importação</param>
        /// <returns></returns>
        public static bool VerificarNotasFiscaisImportadas(int chaveImportacao)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.NotaFiscal.Any(notaFiscal => notaFiscal.ChaveImportacao == chaveImportacao);
            }
        }
    }
}