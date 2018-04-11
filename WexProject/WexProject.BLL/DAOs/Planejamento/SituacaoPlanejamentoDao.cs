using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Planejamento;
using DevExpress.Xpo;
using System.Collections;
using DevExpress.Data.Filtering;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Models;
using WexProject.Library.Libs.Img;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class SituacaoPlanejamentoDAO
    {
        #region Consultas

        #region Devexpress

        /// <summary>
        /// Retorna a situação definida como padrão
        /// </summary>
        /// <param name="session">session</param>
        /// <returns>Situação Planejamento Padrão</returns>
        public static SituacaoPlanejamento ConsultarSituacaoPadrao( Session session )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<SituacaoPlanejamento>(),
                CriteriaOperator.Parse( String.Format( "CsPadrao = '{0}' AND CsSituacao = '{1}'", CsPadraoSistema.Sim,
                CsTipoSituacaoPlanejamento.Ativo ) ), null, 1, false, false );

            foreach(SituacaoPlanejamento ct in collection)
                return ct;

            return null;
        }

        /// <summary>
        /// Método responsável por buscar a situaçao de planejamento padrão
        /// É acionado pelo serviço, acessa a classe SituaçãoPlanejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Objeto SituaçãoPlanejamento</returns>
        public static SituacaoPlanejamentoDTO ConsultarSituacaoPadraoDto( Session session )
        {
            SituacaoPlanejamento situacaoPlanejamento = ConsultarSituacaoPadrao( session );

            if(situacaoPlanejamento != null)
                return situacaoPlanejamento.DtoFactory();

            return null;
        }

        /// <summary>
        /// Resgata a situação
        /// </summary>
        /// <param name="contexto">Sessão</param>
        /// <param name="tipo">Tipo da situação</param>
        /// <returns>Situação baseada nos parametros acima</returns>
        public static SituacaoPlanejamento ConsultarSituacao( Session session, CsTipoPlanejamento tipo )
        {
            SortingCollection sortCollection = new SortingCollection();

            ICollection collection = session.GetObjects( session.GetClassInfo<SituacaoPlanejamento>(),
                CriteriaOperator.Parse( String.Format( "CsSituacao = '{0}' AND CsTipo = '{1}'",
                CsTipoSituacaoPlanejamento.Ativo, tipo ) ), sortCollection, 1, false, true );

            foreach(SituacaoPlanejamento ct in collection)
                return ct;

            return null;
        }

        /// <summary>
        /// Método para retornas só as situações ativas no sistema
        /// </summary>
        /// <param name="session">sessão</param>
        /// <returns>coleção de situações</returns>
        public static XPCollection<SituacaoPlanejamento> ConsultarSituacoesAtivas( Session session )
        {
            return new XPCollection<SituacaoPlanejamento>( session,
                CriteriaOperator.Parse( String.Format( "CsSituacao = '{0}'", CsTipoSituacaoPlanejamento.Ativo ) ) );
        }

        /// <summary>
        /// Método acionado pelo serviço para buscar todas as situações de planejamento acessando a classe SituaçãoPlanejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista de Objetos SituacaoPlanejamentoDTO</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesAtivasDto( Session session )
        {
            List<SituacaoPlanejamentoDTO> situacoesPlanejamento = new List<SituacaoPlanejamentoDTO>();

            using(XPCollection<SituacaoPlanejamento> xpSituacoesPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacoesAtivas( session ))
            {
                if(xpSituacoesPlanejamento.Count > 0)
                    foreach(SituacaoPlanejamento situacaoPlanejamento in xpSituacoesPlanejamento)
                        situacoesPlanejamento.Add( situacaoPlanejamento.DtoFactory() );
            }

            return situacoesPlanejamento;
        }

        /// <summary>
        /// Método para retornas só as situações ativas no sistema
        /// </summary>
        /// <param name="session">sessão</param>
        /// <returns>coleção de situações</returns>
        public static XPCollection ConsultarSituacoesInativas( Session session )
        {
            return new XPCollection( session, typeof( SituacaoPlanejamento ),
                CriteriaOperator.Parse( String.Format( "CsSituacao = '{0}'", CsTipoSituacaoPlanejamento.Inativo ) ) );
        }

        /// <summary>
        /// Método responsável por buscar as situações de planejamento inativas.
        /// É acionado pelo serviço, acessa a classe SituaçãoPlanejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista de Objetos SituaçãoPlanejamento</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesInativasDto( Session session )
        {
            List<SituacaoPlanejamentoDTO> situacoesPlanejamento = new List<SituacaoPlanejamentoDTO>();

            using(XPCollection xpSituacoesPlanejamentoInativas = SituacaoPlanejamentoDAO.ConsultarSituacoesInativas( session ))
            {
                if(xpSituacoesPlanejamentoInativas.Count > 0)
                    foreach(SituacaoPlanejamento situacaoPlanejamento in xpSituacoesPlanejamentoInativas)
                    {
                        situacoesPlanejamento.Add( situacaoPlanejamento.DtoFactory() );
                    }
            }

            return situacoesPlanejamento;
        }

        /// <summary>
        /// Método responsável por buscar as situações planejamento do tipo Planejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista contendo as situações</returns>
        public static List<SituacaoPlanejamento> ConsultarSituacoesPlanejamentoTipoPlanejamento( Session session )
        {
            List<SituacaoPlanejamento> situacoesPlanejamento = new List<SituacaoPlanejamento>();

            situacoesPlanejamento = new XPCollection<SituacaoPlanejamento>( session, CriteriaOperator.Parse( "CsTipo = ? And CsSituacao = ?", 3, 0 ) ).ToList();

            return situacoesPlanejamento;
        }

        /// <summary>
        /// Método responsável por buscar as situações de planejamento do tipo Planejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista de Objetos SituacaoPlanejamentoDto</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesPlanejamentoTipoPlanejamentoDto( Session session )
        {
            List<SituacaoPlanejamentoDTO> situacoesPlanejamentoDto = new List<SituacaoPlanejamentoDTO>();

            List<SituacaoPlanejamento> situacoesPlanejamento = ConsultarSituacoesPlanejamentoTipoPlanejamento( session );

            if(situacoesPlanejamento.Count > 0)
            {
                foreach(SituacaoPlanejamento situacao in situacoesPlanejamento)
                {
                    situacoesPlanejamentoDto.Add( situacao.DtoFactory() );
                }
            }

            return situacoesPlanejamentoDto;
        }

        /// <summary>
        /// Método responsável por buscar uma situaçao de planejamento especificada pelo Tipo
        /// É acionado pelo serviço, acessa a classe SituaçãoPlanejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Objeto SituaçãoPlanejamento</returns>
        public static SituacaoPlanejamentoDTO ConsultarSituacaoPlanejamentoPorTipoDto( Session session, int tipoSituacaoPlanejamento )
        {
            SituacaoPlanejamento situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacao( session, (CsTipoPlanejamento)tipoSituacaoPlanejamento );

            if(situacaoPlanejamento != null)
                return situacaoPlanejamento.DtoFactory();

            return null;
        }

        /// <summary>
        /// Método responsável por buscar as situações planejamento do tipo Execução
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista contendo as situações</returns>
        public static List<SituacaoPlanejamento> ConsultarSituacoesPlanejamentoTipoExecucao( Session session )
        {
            List<SituacaoPlanejamento> situacoesExecucao = new List<SituacaoPlanejamento>();

            situacoesExecucao = new XPCollection<SituacaoPlanejamento>( session, CriteriaOperator.Parse( "CsTipo = ? And CsSituacao = ?", 1, 0 ) ).ToList();

            return situacoesExecucao;
        }

        /// <summary>
        /// Método responsável por buscar as situações de planejamento do tipo Execucao
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista de Objetos SituacaoPlanejamentoDto</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesPlanejamentoTipoExecucaoDto( Session session )
        {
            List<SituacaoPlanejamentoDTO> situacoesExecucaoDto = new List<SituacaoPlanejamentoDTO>();

            List<SituacaoPlanejamento> situacoesExecucao = ConsultarSituacoesPlanejamentoTipoExecucao( session );

            if(situacoesExecucao.Count > 0)
            {
                foreach(SituacaoPlanejamento situacao in situacoesExecucao)
                {
                    situacoesExecucaoDto.Add( situacao.DtoFactory() );
                }
            }

            return situacoesExecucaoDto;
        }

        /// <summary>
        /// Método responsável por buscar as situações planejamento do tipo Encerramento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista contendo as situações</returns>
        public static List<SituacaoPlanejamento> ConsultarSituacoesPlanejamentoTipoEncerramento( Session session )
        {
            List<SituacaoPlanejamento> situacoesEncerramento = new List<SituacaoPlanejamento>();

            situacoesEncerramento = new XPCollection<SituacaoPlanejamento>( session, CriteriaOperator.Parse( "CsTipo = ? And CsSituacao = ?", (int)CsTipoPlanejamento.Encerramento, 0 ) ).ToList();

            return situacoesEncerramento;
        }

        /// <summary>
        /// Método responsável por buscar as situações de planejamento do tipo Encerramento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Lista de Objetos SituacaoPlanejamentoDto</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesPlanejamentoTipoEncerramentoDto( Session session )
        {
            List<SituacaoPlanejamentoDTO> situacoesEncerramentoDto = new List<SituacaoPlanejamentoDTO>();

            List<SituacaoPlanejamento> situacoesEncerramento = ConsultarSituacoesPlanejamentoTipoEncerramento( session );

            if(situacoesEncerramento.Count > 0)
            {
                foreach(SituacaoPlanejamento situacao in situacoesEncerramento)
                {
                    situacoesEncerramentoDto.Add( situacao.DtoFactory() );
                }
            }

            return situacoesEncerramentoDto;
        }

        #endregion

        #region Entity

        /// <summary>
        /// Método responsável por buscar as situações planejamento do tipo Encerramento
        /// </summary>
        /// <param name="contexto">Contexo de consultas do banco</param>
        /// <returns>Situação Planejamento Padrão</returns>
        public static WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento ConsultarSituacaoPadraoEntity( WexDb contexto )
        {
            return contexto.SituacaoPlanejamento.FirstOrDefault( o => o.CsPadrao == CsPadraoSistema.Sim && o.CsSituacao == CsTipoSituacaoPlanejamento.Ativo );
        }

        /// <summary>
        /// Método responsável por buscar uma situação planejamento pelo Oid.
        /// </summary>
        /// <param name="oidSituacaoPlanejamento">Oid da situação planejamento</param>
        /// <returns>Situação Planejamento</returns>
        public static WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento ConsultarSituacaoPlanejamentoPorOid( Guid oidSituacaoPlanejamento )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.SituacaoPlanejamento.FirstOrDefault( o => o.Oid == oidSituacaoPlanejamento ); 
            }
        }

        /// <summary>
        /// Consultar Situação Planejamento por Tipo.
        /// </summary>
        /// <param name="tipo">Tipo da situação</param>
        /// <returns>Situação Planejamento</returns>
        public static WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento ConsultarSituacaoPorTipo( CsTipoPlanejamento tipo )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.SituacaoPlanejamento.FirstOrDefault( o => o.CsSituacao == CsTipoSituacaoPlanejamento.Ativo && o.CsTipo == tipo ); 
            }
        }

        /// <summary>
        /// Consultar Situações de Planejamento que estão Ativas.
        /// </summary>
        /// <returns>Coleção de situações de planejamento</returns>
        public static List<WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento> ConsultarSituacoesAtivas()
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.SituacaoPlanejamento.Where( o => o.CsSituacao == CsTipoSituacaoPlanejamento.Ativo ).OrderBy( o => o.TxDescricao ).ToList(); 
            }
        }

        /// <summary>
        /// Consultar situações inativas.
        /// </summary>
        /// 
        /// <returns>Coleção de Situações Planejamento Inativas</returns>
        public static List<WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento> ConsultarSituacoesInativas()
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.SituacaoPlanejamento.Where( o => (int)o.CsSituacao == (int)CsTipoSituacaoPlanejamento.Inativo ).ToList(); 
            }
        }

        #endregion

        #endregion

        #region Criacao

        /// <summary>
        /// Método para criar uma nova situação planejamento
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="situacao">nova situação planejamento</param>
        public static void CriarSituacaoPlanejamento( WexDb contexto, WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento situacao )
        {
            if(situacao == null)
                return;
            contexto.SituacaoPlanejamento.Add( situacao );
            contexto.SaveChanges();
        }

        #endregion

        #region Factories

        /// <summary>
        /// Método para gerar um Dto a partir da Entidade
        /// </summary>
        /// <param name="situacao">entidade situação planejamento</param>
        /// <returns>Dto referente a entidade</returns>
        public static SituacaoPlanejamentoDTO DtoFactory( WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento situacao )
        {
            if(situacao == null)
                return null;
            SituacaoPlanejamentoDTO situacaoPlanejamentoDto = new SituacaoPlanejamentoDTO()
            {
                Oid = situacao.Oid,
                TxDescricao = situacao.TxDescricao,
                CsSituacao = situacao.CsSituacao,
                CsTipo = situacao.CsTipo,
                CsPadrao = situacao.CsPadrao,
                KeyPress = (System.Windows.Forms.Shortcut)situacao.KeyPress,
                TxKeys = situacao.TxKeys
            };

            if(situacao.BlImagem != null)
                situacaoPlanejamentoDto.BlImagemSituacaoPlanejamento = situacao.BlImagem;
            return situacaoPlanejamentoDto;
        }

        #endregion
    }
}
