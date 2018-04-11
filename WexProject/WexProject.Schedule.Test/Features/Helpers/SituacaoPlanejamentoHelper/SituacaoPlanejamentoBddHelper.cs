using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.Schedule.Test.Features.Helpers.SituacaoPlanejamentoHelper
{
    public class SituacaoPlanejamentoBddHelper
    {
        /// <summary>
        /// Método responsável por converter o parâmetro vindo como string para o Domain CsTipoPlanejamento.
        /// </summary>
        /// <param name="tipoPlanejamento">string com o tipo do planejamento</param>
        /// <returns>o tipo de planejamento em Domain</returns>
        public static CsTipoPlanejamento ConverterTipoPlanejamentoStringParaTipoPlanejamentoDomain( string tipoPlanejamento )
        {
            if(tipoPlanejamento == CsTipoPlanejamento.Cancelamento.ToString())
            {
                return CsTipoPlanejamento.Cancelamento;
            }
            else if(tipoPlanejamento == CsTipoPlanejamento.Encerramento.ToString())
            {
                return CsTipoPlanejamento.Encerramento;
            }
            else if(tipoPlanejamento == CsTipoPlanejamento.Execução.ToString())
            {
                return CsTipoPlanejamento.Execução;
            }
            else if(tipoPlanejamento == CsTipoPlanejamento.Impedimento.ToString())
            {
                return CsTipoPlanejamento.Impedimento;
            }
            else if(tipoPlanejamento == CsTipoPlanejamento.Planejamento.ToString())
            {
                return CsTipoPlanejamento.Planejamento;
            }

            return CsTipoPlanejamento.Planejamento;
        }

        /// <summary>
        ///  Método responsável por converter o parâmetro vindo como string para o Domain CsPadraoSistema.
        /// </summary>
        /// <param name="situacaoPadraoSistema">string com o padrão do sistema</param>
        /// <returns>Domain com o padrão do sistema</returns>
        public static CsPadraoSistema ConverterSituacaoPadraoSistemaStrinParaSituacaoPadraoSistemaDomain( string situacaoPadraoSistema )
        {
            if(situacaoPadraoSistema == CsPadraoSistema.Sim.ToString())
            {
                return CsPadraoSistema.Sim;
            }
            else
            {
                return CsPadraoSistema.Não;
            }
        }
    }
}
