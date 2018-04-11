using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe Configuracao
    /// </summary>
    public class ConfiguracaoFactory : BaseFactory
    {
        /// <summary>
        /// Criação de uma nova Configuração
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="nbQtdeMaxVenda">Quantidade máxima de venda</param>
        /// <param name="nbQtdeMaxFerias">Quantidade máxima que se pode tirar férias</param>
        /// <param name="nbDtMaxTirarFerias">Data máxima para tirar férias em um período aquisitivo</param>
        /// <param name="save">Indica se é para salvar ou não</param>
        /// <returns>Objeto de Configuracao criado</returns>
        public static Configuracao CriarConfiguracao(Session session, uint nbQtdeMaxVenda, uint nbQtdeMaxFerias, uint nbDtMaxTirarFerias, bool save = true)
        {
            Configuracao configuracao = new Configuracao(session)
            {
                NbQtdeMaxVenda = nbQtdeMaxVenda,
                NbQtdeMaxFerias = nbQtdeMaxFerias,
                NbDtMaxTirarFerias = nbDtMaxTirarFerias
            };

            if (save)
                configuracao.Save();

            return configuracao;
        }
    }
}