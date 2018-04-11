using System;
using WexProject.BLL.Models.Geral;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe ConfiguracaoDocumento
    /// </summary>
    public class ConfiguracaoDocumentoFactory : BaseFactory
    {
        /// <summary>
        /// Cria um objeto de ConfiguracaoDocumento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="tipo">Tipo da Configuração</param>
        /// <param name="save">Indica se é para salvar ou não</param>
        /// <returns>Objeto de ConfiguracaoDocumento</returns>
        public static ConfiguracaoDocumento CriarConfiguracaoDocumento(Session session, CsTipoDocumento tipo, bool save = false)
        {
            ConfiguracaoDocumento configuracao = new ConfiguracaoDocumento(session);

            configuracao.CsTipoDocumento = tipo;

            if (save)
                configuracao.Save();

            return configuracao;
        }
    }
}
