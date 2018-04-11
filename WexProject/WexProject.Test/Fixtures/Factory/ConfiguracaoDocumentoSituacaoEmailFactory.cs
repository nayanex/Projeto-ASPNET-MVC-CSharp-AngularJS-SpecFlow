using System;
using WexProject.BLL.Models.Geral;
using DevExpress.Xpo;
using WexProject.BLL.Models.NovosNegocios;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe ConfiguracaoDocumentoSituacaoEmail
    /// </summary>
    public class ConfiguracaoDocumentoSituacaoEmailFactory
    {
        /// <summary>
        /// Criar email para envio com cópia
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="txEmail">Email</param>
        /// <param name="situacao">Situação</param>
        /// <param name="solicitacao">Solicitação</param>
        /// <param name="save">Se é para salvar ou não</param>
        /// <returns>Objeto de ConfiguracaoDocumentoSituacaoEmailCc</returns>
        public static ConfiguracaoDocumentoSituacaoEmailCc CriarEmailCc(Session session, string txEmail, ConfiguracaoDocumentoSituacao situacao = null, bool save = false)
        {
            ConfiguracaoDocumentoSituacaoEmailCc email = new ConfiguracaoDocumentoSituacaoEmailCc(session);

            email.TxEmail = txEmail;
            email.ConfiguracaoDocumentoSituacao = situacao;

            if (save)
                email.Save();

            return email;
        }

        /// <summary>
        /// Criar email para envio com cópia oculta
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="txEmail">Email</param>
        /// <param name="situacao">Situação</param>
        /// <param name="solicitacao">Solicitação</param>
        /// <param name="save">Se é para salvar ou não</param>
        /// <returns>Objeto de ConfiguracaoDocumentoSituacaoEmailCco</returns>
        public static ConfiguracaoDocumentoSituacaoEmailCco CriarEmailCco(Session session, string txEmail, ConfiguracaoDocumentoSituacao situacao = null, bool save = false)
        {
            ConfiguracaoDocumentoSituacaoEmailCco email = new ConfiguracaoDocumentoSituacaoEmailCco(session);

            email.TxEmail = txEmail;
            email.ConfiguracaoDocumentoSituacao = situacao;
            
            if (save)
                email.Save();

            return email;
        }
    }
}
