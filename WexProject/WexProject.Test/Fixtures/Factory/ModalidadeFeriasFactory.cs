using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe ModalidadeFerias
    /// </summary>
    public class ModalidadeFeriasFactory : BaseFactory
    {
        /// <summary>
        /// Criação de um novo objeto de ModalidadeFerias
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="nbDias">Quantidade de dias</param>
        /// <param name="podeVender">Pode Vender?</param>
        /// <param name="csSituacao">Situação da Modalidade</param>
        /// <param name="save">É para salvar a Modalidade?</param>
        /// <returns>Objeto criado</returns>
        public static ModalidadeFerias CriarModalidadeFerias(Session session, uint nbDias, bool podeVender, CsSituacao csSituacao = CsSituacao.Ativo, bool save = true)
        {
            ModalidadeFerias modalidade = new ModalidadeFerias(session)
            {
                NbDias = nbDias,
                PodeVender = podeVender,
                CsSituacao = csSituacao
            };

            if (save)
                modalidade.Save();

            return modalidade;
        }
    }
}