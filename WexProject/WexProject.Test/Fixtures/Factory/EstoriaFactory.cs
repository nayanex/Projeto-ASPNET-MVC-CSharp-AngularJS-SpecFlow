using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory estoria
    /// </summary>
    public class EstoriaFactory : BaseFactory
    {
        /// <summary>
        /// método criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">Modulo</param>
        /// <param name="txTitulo">txTitulo</param>
        /// <param name="txGostariaDe">TxGostariaDe</param>
        /// <param name="txEntaoPoderei">TxEntaoPoderei</param>
        /// <param name="beneficiado">beneficiado</param>
        /// <param name="txObservacoes">TxObservacoes</param>
        /// <param name="txReferencias">TxReferencias</param>
        /// <param name="txDuvidas">TxDuvidas</param>
        /// <param name="save">bool</param>
        /// <returns>estoria</returns>
        public static Estoria Criar(Session session, Modulo modulo, String txTitulo, String txGostariaDe, String txEntaoPoderei, Beneficiado beneficiado,
        String txObservacoes, String txReferencias, String txDuvidas, bool save = false)
        {
            var estoria = new Estoria(session);

            if (String.IsNullOrEmpty(txGostariaDe))
                estoria.TxGostariaDe = GetDescricao();

            if (String.IsNullOrEmpty(txTitulo))
                estoria.TxTitulo = GetDescricao();

            if (String.IsNullOrEmpty(txEntaoPoderei))
                estoria.TxEntaoPoderei = GetDescricao();

            if (String.IsNullOrEmpty(txObservacoes))
                estoria.TxAnotacoes = GetDescricao();

            if (String.IsNullOrEmpty(txReferencias))
                estoria.TxReferencias = GetDescricao();

            if (String.IsNullOrEmpty(txDuvidas))
                estoria.TxDuvidas = GetDescricao();

            estoria.Modulo = modulo;
            // estoria.ParteInteressada = ParteInteressada;
            estoria.TxTitulo = txTitulo;
            estoria.ComoUmBeneficiado = beneficiado;
            //estoria.BeneficiadoProjeto = BeneficiadoProjeto;

            if (save)
                estoria.Save();

            return estoria;
        }
        /// <summary>
        /// método Criar filho
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="estoriaPai">estoriaPai</param>
        /// <param name="txTitulo">TxTitulo</param>
        /// <param name="txGostariaDe">TxGostariaDe</param>
        /// <param name="txEntaoPoderei">TxEntaoPoderei</param>
        /// <param name="beneficiado">beneficiado</param>
        /// <param name="txObservacoes">TxObservacoes</param>
        /// <param name="txReferencias">TxReferencias</param>
        /// <param name="txDuvidas">TxDuvidas</param>
        /// <param name="save">bool</param>
        /// <returns>estoriaFilho</returns>
        public static Estoria CriarFilho(Session session, Estoria estoriaPai, String txTitulo, String txGostariaDe, String txEntaoPoderei, Beneficiado beneficiado,
        String txObservacoes, String txReferencias, String txDuvidas, bool save = false)
        {
            Estoria estoriaFilho = Criar(session, estoriaPai.Modulo, txTitulo, txGostariaDe, txEntaoPoderei, beneficiado, txObservacoes, txReferencias, txDuvidas, false);

            estoriaFilho.EstoriaPai = estoriaPai;

            if (save)
                estoriaFilho.Save();

            return estoriaFilho;
        }


        internal static Estoria Criar(Session SessionTest, Modulo modulo, Beneficiado beneficiado, string titulo, uint tamanho, string emAnalise, bool save)
        {
            Estoria estoria = new Estoria(SessionTest);
            estoria.Modulo = modulo;
            estoria.ComoUmBeneficiado = beneficiado;
            estoria.TxTitulo = titulo;
            estoria.NbTamanho = tamanho;

            if (emAnalise.ToUpper().Equals("SIM"))
            {
                estoria.CsEmAnalise = true;
            }
            else
            {
                estoria.CsEmAnalise = false;
            }

            if (save)
                estoria.Save();

            return estoria;
        }


        internal static Estoria Criar(Session SessionTest, Modulo modulo, Beneficiado beneficiado, string titulo, uint tamanho, string emAnalise, bool save, Estoria estoriaPai)
        {
            Estoria estoria = new Estoria(SessionTest);
            estoria.Modulo = modulo;
            estoria.EstoriaPai = estoriaPai;
            estoria.ComoUmBeneficiado = beneficiado;
            estoria.TxTitulo = titulo;
            estoria.NbTamanho = tamanho;

            if (emAnalise.ToUpper().Equals("SIM"))
            {
                estoria.CsEmAnalise = true;
            }
            else
            {
                estoria.CsEmAnalise = false;
            }

            if (save)
                estoria.Save();

            return estoria;
        }
    }
}
