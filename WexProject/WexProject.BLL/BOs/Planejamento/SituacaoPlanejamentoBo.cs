using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Models.Planejamento;
using WexProject.BLL.Shared.Domains.Planejamento;
using System.Windows.Forms;
using DevExpress.Xpo;
using WexProject.BLL.Models;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.BOs.Planejamento
{
    public class SituacaoPlanejamentoBO
    {
        #region DevExpress

        /// <summary>
        /// Realiza a troca de padrão quando um novo registro é definido como padrão
        /// </summary>
        public static void DesabilitarSituacaoPlanejamentoPadraoAnterior( Session session, Guid oidSituacaoPlanejamentoPadraoAtual )
        {
			List<SituacaoPlanejamento> situacoesPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacoesAtivas( session ).ToList();

			if(situacoesPlanejamento != null  && situacoesPlanejamento.Count > 0)
			{
				SituacaoPlanejamento situacaoPadraoAtual = situacoesPlanejamento.Where( o => o.Oid == oidSituacaoPlanejamentoPadraoAtual ).FirstOrDefault();

				if(situacaoPadraoAtual != null)
					situacoesPlanejamento.Remove( situacaoPadraoAtual );

				foreach(SituacaoPlanejamento situacao in situacoesPlanejamento)
					if(situacao.CsPadrao == CsPadraoSistema.Sim)
					{
						situacao.CsPadrao = CsPadraoSistema.Não;
						situacao.Save();
					}
			}
        }

        #endregion

        #region Entity

        /// <summary>
        /// Guarda a blacklist de atalhos
        /// </summary>
        public static List<Shortcut> blackListShortcuts = new List<Shortcut>() { Shortcut.Ctrl1, Shortcut.Ctrl2, Shortcut.Ctrl3, Shortcut.CtrlN, Shortcut.CtrlS, 
            Shortcut.CtrlShiftS,
			//Shortcut.CtrlShiftN,
			Shortcut.CtrlShiftD, Shortcut.F1, Shortcut.F2, Shortcut.F3, Shortcut.F4, Shortcut.F5, Shortcut.F6,
            Shortcut.F7, Shortcut.F8, Shortcut.F9, Shortcut.F10, Shortcut.F11, Shortcut.F12, Shortcut.CtrlW, Shortcut.CtrlF4, Shortcut.Ins, Shortcut.CtrlDel,
            Shortcut.CtrlF12};

        /// <summary>
        /// Método que salva uma nova Situação Padrão para o sistema e altera a antiga Situação Planejamento que está como padrão para que não seja padrão.
        /// </summary>
        public static void RetirarSituacaoPlanejamentoPadrao( WexDb contexto )
        {
            WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento antigaSituacaoPlanejamentoPadrao = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );

            if(antigaSituacaoPlanejamentoPadrao != null)
            {
                antigaSituacaoPlanejamentoPadrao.CsPadrao = CsPadraoSistema.Não;
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Verificar se atalho pode ser adicionado para uma Situação Planejamento, 
        /// isso ocorre caso ele não esteja na lista de BlackList de Atalhos que podem ser adicionados.
        /// </summary>
        /// <param name="atalho">Atalho a ser verificado</param>
        /// <returns>Retorna se pode ou não</returns>
        public static bool VerificarPossibilidadeAdicionarAtalho( Shortcut atalho )
        {
            return !SituacaoPlanejamentoBO.blackListShortcuts.Contains( atalho );
        }

        /// <summary>
        /// Consultar Situações Ativas e transformá-las em DTO
        /// </summary>
        /// <returns>Lista de Objetos SituacaoPlanejamentoDTO</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesAtivas()
        {
            List<SituacaoPlanejamentoDTO> situacoesPlanejamentoDTO = new List<SituacaoPlanejamentoDTO>();

            List<WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento> situacoesPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacoesAtivas();

            if(situacoesPlanejamento.Count > 0)
                for(int i = 0; i < situacoesPlanejamento.Count; i++)
                    situacoesPlanejamentoDTO.Add( SituacaoPlanejamentoDAO.DtoFactory( situacoesPlanejamento[i] ) );
            return situacoesPlanejamentoDTO;
        }

        /// <summary>
        /// Método responsável por buscar a situaçao de planejamento padrão
        /// É acionado pelo serviço, acessa a classe SituaçãoPlanejamento
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <returns>Objeto SituaçãoPlanejamento</returns>
        public static SituacaoPlanejamentoDTO ConsultarSituacaoPadrao(  )
        {
            using(WexDb contexto = new WexDb())
            {
                WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );

                if(situacaoPlanejamento != null)
                    return SituacaoPlanejamentoDAO.DtoFactory( situacaoPlanejamento );

                return null;
            }
        }

        /// <summary>
        /// Consultar Situações de Planejamento Inativas.
        /// </summary>
        /// <returns>Lista de Objetos SituaçãoPlanejamento Inativas</returns>
        public static List<SituacaoPlanejamentoDTO> ConsultarSituacoesInativas()
        {
            List<SituacaoPlanejamentoDTO> situacoesPlanejamentoDTO = new List<SituacaoPlanejamentoDTO>();

            List<WexProject.BLL.Entities.Planejamento.SituacaoPlanejamento> situacoesPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacoesInativas();

            if(situacoesPlanejamento.Count > 0)
                for(int i = 0; i < situacoesPlanejamento.Count; i++)
                    situacoesPlanejamentoDTO.Add( SituacaoPlanejamentoDAO.DtoFactory( situacoesPlanejamento[i] ) );

            return situacoesPlanejamentoDTO;
        }

        #endregion
    }
}
