using System;
using WexProject.BLL.Models.Geral;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe ConfiguracaoDocumentoSituacao
    /// </summary>
    public class ConfiguracaoDocumentoSituacaoFactory : BaseFactory
    {
        /// <summary>
        /// Cria um objeto de ConfiguracaoDocumentoSituacao
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="descricao">Descrição da configuração</param>
        /// <param name="cor">Cor da configuração</param>
        /// <param name="tipoCor">O tipo da cor da configuração</param>
        /// <param name="save">Indica se é para salvar ou não</param>
        /// <returns>Objeto de ConfiguracaoDocumentoSituacao</returns>
        public static ConfiguracaoDocumentoSituacao CriarConfiguracaoDocumentoSituacao(Session session, string descricao, string cor, CsColorDomain tipoCor, bool save = false)
        {
            ConfiguracaoDocumentoSituacao situacao = new ConfiguracaoDocumentoSituacao(session);

            situacao.ConfiguracaoDocumento = ConfiguracaoDocumentoFactory.CriarConfiguracaoDocumento(session, CsTipoDocumento.SolicitacaoOrcamento, true);
            situacao.TxDescricao = descricao;
            situacao.TxNomeCor = cor;
            situacao.TypeColor = tipoCor;

            if (save)
                situacao.Save();

            return situacao;
        }

        /// <summary>
        /// Cria um objeto de ConfiguracaoDocumentoSituacao
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="configuracaoDocumento">Objeto de ConfiguracaoDocumento</param>
        /// <param name="descricao">Descrição da Situação</param>
        /// <param name="save">É para salvar?</param>
        /// <returns>Objeto de ConfiguracaoDocumentoSituacao</returns>
        public static ConfiguracaoDocumentoSituacao CriarConfiguracaoDocumentoSituacaoComConfiguracao(Session session, ConfiguracaoDocumento configuracaoDocumento, string descricao, bool save = false)
        {
            ConfiguracaoDocumentoSituacao situacao = new ConfiguracaoDocumentoSituacao(session);

            situacao.ConfiguracaoDocumento = configuracaoDocumento;
            situacao.TxDescricao = descricao;

            if (save)
                situacao.Save();

            return situacao;
        }
    }
}
