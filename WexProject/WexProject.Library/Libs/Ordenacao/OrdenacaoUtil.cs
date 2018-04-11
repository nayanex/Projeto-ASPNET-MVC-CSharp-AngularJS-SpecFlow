using System;
using System.Collections.Generic;

namespace WexProject.Library.Libs.Ordenacao
{
    /// <summary>
    /// Classe utilitária com funções para Array.
    /// </summary>
    public class OrdenacaoUtil
    {
        /// <summary>
        /// Regra de negocio que reordena as sequências
        /// </summary>
        /// <param name="objeto">objeto</param>
        /// <param name="operacao">operacao</param>
        /// <param name="salvar">salvar</param>
        public static void RnRepriorizar(IOrdenacao objeto, CsOrdenacaoDomain operacao, bool salvar = true)
        {
            if ((objeto.GetReOrdenando() || objeto.GetOrdemOld() == objeto.GetNbOrdem()) && !objeto.IsDeleted())
            {
                objeto.SetReOrdenando(false);
                return;
            }

            List<Object> itens = null;
            UInt16 prioridade = 0;

            if (operacao == CsOrdenacaoDomain.DescerOrdem)
            {
                if (objeto.GetOrdemOld() != 0)
                {
                    itens = objeto.GetItensPorOrdem(objeto.GetOrdemOld() + 1, objeto.GetNbOrdem());
                    prioridade = objeto.GetOrdemOld();
                }
                else
                {
                    itens = objeto.GetItensPorOrdem(objeto.GetOrdemOld());
                    prioridade = (UInt16)(objeto.GetNbOrdem() + 1);
                }
            }
            else
                if (operacao == CsOrdenacaoDomain.SubirOrdem)
                {
                    if (!objeto.GetOid().Equals(new Guid()) && objeto.GetNbOrdem() != 0)
                        itens = objeto.GetItensPorOrdem(objeto.GetNbOrdem(), objeto.GetOrdemOld() - 1);
                    else if (objeto.GetNbOrdem() != 0)
                        itens = objeto.GetItensPorOrdem(objeto.GetNbOrdem() - 1);
                    else
                        itens = objeto.GetItensPorOrdem(objeto.GetOrdemOld() + 1);

                    prioridade = (UInt16)(objeto.GetNbOrdem() + 1);
                }
                else
                {
                    if (operacao == CsOrdenacaoDomain.ExcluirOrdem)
                    {
                        itens = objeto.GetItensPorOrdem(objeto.GetNbOrdem());
                        prioridade = objeto.GetNbOrdem();
                    }
                }
            if (itens.Count > 0)
            {
                foreach (IOrdenacao item in itens)
                {
                    item.SetReOrdenando(true);
                    item.SetNbOrdem(prioridade);
                    item.SetOrdemOld(prioridade);
                    if (salvar)
                        item.Save();
                    item.SetReOrdenando(false);
                    prioridade += 1;
                }
            }
        }

        /// <summary>
        /// Regra de negócio que verifica se a sequência está subindo ou descendo
        /// </summary>
        /// <param name="objeto">objeto</param>
        public static void RnAplicarOrdenacao(IOrdenacao objeto)
        {
            int maiorPrioridade = objeto.GetMaiorOrdem();

            if (objeto.GetNbOrdem() < (UInt16)objeto.GetOrdemOld())
                RnRepriorizar(objeto, CsOrdenacaoDomain.SubirOrdem);
            else
                if (objeto.GetNbOrdem() > objeto.GetOrdemOld() && objeto.GetNbOrdem() <= maiorPrioridade)
                    RnRepriorizar(objeto, CsOrdenacaoDomain.DescerOrdem);

            objeto.SetOrdemOld(objeto.GetNbOrdem());
            objeto.SetReOrdenando(false);
        }

        /// <summary>
        /// Regra de negócio que verifica se a sequência está sendo deletada
        /// </summary>
        /// <param name="objeto">objeto</param>
        public static void RnDeletarOrdenacao(IOrdenacao objeto)
        {
            RnRepriorizar(objeto, CsOrdenacaoDomain.ExcluirOrdem);
            objeto.SetOrdemOld(objeto.GetNbOrdem());
        }

        /// <summary>
        /// Regra de negócio que cria as sequências
        /// </summary>
        /// <param name="objeto">objeto</param>
        public static void RnCriarOrdem(IOrdenacao objeto)
        {
            int total = objeto.GetMaiorOrdem();

            if (objeto.GetNbOrdem() >= total || objeto.GetNbOrdem() == 0)
            {
                objeto.SetNbOrdem((UInt16)(total + 1));
                objeto.SetOrdemOld(objeto.GetNbOrdem());
            }
        }
    }
}