using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using System.Data.Entity;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class CronogramaUltimaSelecaoDao
    {
        #region Consultar

        /// <summary>
        /// Método que busca o último cronograma selecionado pelo usuário.
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="login">Login do usuário</param>
        /// <returns>Objeto Cronograma que representa o último selecionado</returns>
        public static Cronograma ConsultarUltimoCronogramaSelecionado( WexDb contexto, string login )
        {
            if(contexto == null || login == null)
                throw new ArgumentException( "Os parâmetros session e login usuário não podem ser nulos." );

            CronogramaUltimaSelecao ultimaSelecao = contexto.CronogramaUltimaSelecao.Include( o => o.Cronograma).Include( o => o.Cronograma.SituacaoPlanejamento).Include( o => o.Usuario).FirstOrDefault( o => o.Usuario.UserName == login );

            if(ultimaSelecao == null)
                return null;

            if(ultimaSelecao.Cronograma == null || ultimaSelecao.Cronograma.CsExcluido)
                return null;

            return ultimaSelecao.Cronograma;
        }

        /// <summary>
        /// Método responsável por recuperar o último cronograma acessado por um usuário
        /// É usado pela tela de cronograma
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="login">Login do Usuário</param>
        /// <returns>Objeto CronogramaDTO</returns>
        public static CronogramaDto ConsultarUltimoCronogramaSelecionadoDto( WexDb contexto, string login )
        {
            CronogramaDto cronogramaDTO = new CronogramaDto();

            Cronograma cronograma = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionado( contexto, login );

            if(cronograma != null)
            {
                cronogramaDTO.Oid = cronograma.Oid;
                cronogramaDTO.TxDescricao = cronograma.TxDescricao;
                cronogramaDTO.OidSituacaoPlanejamento = cronograma.SituacaoPlanejamento.Oid;
                cronogramaDTO.TxDescricaoSituacaoPlanejamento = cronograma.SituacaoPlanejamento.TxDescricao;
                cronogramaDTO.DtInicio = (DateTime)cronograma.DtInicio;
                cronogramaDTO.DtFinal = (DateTime)cronograma.DtFinal;
            }
            else
                cronogramaDTO = null;

            return cronogramaDTO;
        }

        #endregion

        #region Salvar

        /// <summary>
        /// Método responsável por salvar o último cronograma selecionado por um usuário.
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="login">Login do usuário</param>
        /// <param name="oidCronograma">Oid do Cronograma selecionado</param>
        public static void SalvarUltimoCronogramaSelecionado( WexDb contexto, string login, Guid? oidCronograma )
        {
            if(contexto == null || login == null || !oidCronograma.HasValue)
                throw new ArgumentException( "Os parâmetros session, oidUsuario e oidCronograma não podem nulos." );

            User usuario = contexto.Usuario.FirstOrDefault( o => o.UserName == login );

            DateTime dataAcesso = DateTime.Now;
            Cronograma cronogramaSelecionado = contexto.Cronograma.FirstOrDefault( o => o.Oid == oidCronograma.Value );

            CronogramaUltimaSelecao ultimoCronogramaSelecionado = contexto.CronogramaUltimaSelecao.FirstOrDefault( o => o.OidUsuario == usuario.Oid );

            if(ultimoCronogramaSelecionado != null)
            {
                ultimoCronogramaSelecionado.DataAcesso = dataAcesso;
                ultimoCronogramaSelecionado.Usuario = usuario;
                ultimoCronogramaSelecionado.Cronograma = cronogramaSelecionado;
                contexto.SaveChanges();
            }
            else
            {
                CronogramaUltimaSelecao ultimoCronograma = new CronogramaUltimaSelecao();
                ultimoCronograma.DataAcesso = dataAcesso;
                ultimoCronograma.Usuario = usuario;
                ultimoCronograma.Cronograma = cronogramaSelecionado;
                contexto.CronogramaUltimaSelecao.Add( ultimoCronograma );
                contexto.SaveChanges();
            }
        }

        #endregion
    }
}
