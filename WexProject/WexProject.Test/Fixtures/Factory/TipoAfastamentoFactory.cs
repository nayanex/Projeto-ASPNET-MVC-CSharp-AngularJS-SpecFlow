using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe TipoAfastamento
    /// </summary>
    public class TipoAfastamentoFactory : BaseFactory
    {
        /// <summary>
        /// Criação de um novo objeto de TipoAfastamento
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="txDescricao">Descrição do Tipo de Afastamento</param>
        /// <param name="isRemunerado">Remunerado?</param>
        /// <param name="isParaFeriasRealizadas">Para férias realizadas?</param>
        /// <param name="csSituacao">Situação do Tipo de Afastamento</param>
        /// <param name="save">É para salvar?</param>
        /// <returns>Objeto de TipoAfastamento criado</returns>
        public static TipoAfastamento CriarTipoAfastamento(Session session, string txDescricao, bool isRemunerado = true, bool isParaFeriasRealizadas = false, CsSituacao csSituacao = CsSituacao.Ativo, bool save = true)
        {
            TipoAfastamento tipo = new TipoAfastamento(session)
            {
                TxDescricao = txDescricao,
                IsParaFeriasRealizadas = isParaFeriasRealizadas,
                IsRemunerado = isRemunerado,
                CsSituacao = csSituacao
            };

            if(save)
                tipo.Save();

            return tipo;
        }
    }
}