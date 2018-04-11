using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;
using WexProject.BLL.Models.Execucao;
using WexProject.BLL.Models.NovosNegocios;
using WexProject.BLL.Models.Geral;
using DevExpress.Data.Filtering;

namespace WexProject.BLL.Models.Rh
{

    /// <summary>
    /// Classe de Último filtro
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [NavigationItem(false)]
    [OptimisticLocking( false )]
    public class ColaboradorUltimoFiltro : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Último filtro de situação (SEOT)
        /// </summary>
        private Guid lastSituacaoFilterSeot;

        /// <summary>
        /// Último filtro de usuário (SEOT)
        /// </summary>
        private Guid lastUsuarioFilterSeot;

        /// <summary>
        /// Último filtro de período (Período de Férias)
        /// </summary>
        private int lastPeriodoFilterPlanejamentoFerias;

        /// <summary>
        /// Último filtro de situação (Período de Férias)
        /// </summary>
        private int lastSituacaoFilterPlanejamentoFerias;
        /// <summary>
        /// Ultimo filtro de superior imediato (PlanejamentoFerias)
        /// </summary>
        private string lastSuperiorImediatoFilterPlanejamentoFerias;
        /// <summary>
        /// Ultimo filtro de situaçao de ferias (PlanejamentoFerias)
        /// </summary>
        private string lastSituacaoFeriasFilterPlanejamentoFerias;

        /// <summary>
        /// Último Motivo de Cancelamento escolhido pelo usuário
        /// </summary>
        private MotivoCancelamento lastMotivoCancelamento;

        /// <summary>
        /// Última empresa/instituição salva pelo usuário
        /// </summary>
        private EmpresaInstituicao lastEmpresaInstituicaoSEOT;

        /// <summary>
        /// Último tipo de solicitação definido pelo usuário na SEOT
        /// </summary>
        private TipoSolicitacao lastTipoSolicitacaoSEOT;

        /// <summary>
        /// Oid Último projeto selecionado definido pelo usuário
        /// </summary>
        private Guid oidUltimoProjetoSelecionado;

        #endregion

        #region Propriedades

        /// <summary>
        /// Oid Último projeto selecionado pelo colaborador.
        /// </summary>
        public Guid OidUltimoProjetoSelecionado
        {
            get
            {
                return oidUltimoProjetoSelecionado;
            }
            set
            {
                if (oidUltimoProjetoSelecionado == value)
                    return;

                SetPropertyValue<Guid>("OidUltimoProjetoSelecionado", ref oidUltimoProjetoSelecionado, value);
            }

        }

        /// <summary>
        /// Ultimo filtro de superior imediato (PlanejamentoFerias)
        /// </summary>
        [Size(-1)]
        public string LastSuperiorImediatoFilterPlanejamentoFerias
        {
            get
            {
                return lastSuperiorImediatoFilterPlanejamentoFerias;
            }
            set
            {
                if (lastSuperiorImediatoFilterPlanejamentoFerias == value)
                    return;

                SetPropertyValue<string>("LastSuperiorImediatoFilterPlanejamentoFerias", ref lastSuperiorImediatoFilterPlanejamentoFerias, value);
            }

        }

        /// <summary>
        /// Ultimo filtro de situaçao de ferias (PlanejamentoFerias)
        /// </summary>
        [Size(-1)]
        public string LastSituacaoFeriasFilterPlanejamentoFerias
        {
            get
            {
                return lastSituacaoFeriasFilterPlanejamentoFerias;
            }
            set
            {
                if (lastSituacaoFeriasFilterPlanejamentoFerias == null)
                    lastSituacaoFeriasFilterPlanejamentoFerias = "";

                SetPropertyValue<string>("LastSituacaoFeriasFilterPlanejamentoFerias", ref lastSituacaoFeriasFilterPlanejamentoFerias, value);
            }

        }

        /// <summary>
        /// Último filtro de situação (SEOT)
        /// </summary>
        public Guid LastSituacaoFilterSeot
        {
            get
            {
                return lastSituacaoFilterSeot;
            }
            set
            {
                if (lastSituacaoFilterSeot == value)
                    return;

                SetPropertyValue<Guid>("LastSituacaoFilterSeot", ref lastSituacaoFilterSeot, value);
            }
        }

        /// <summary>
        /// Último filtro de usuário (SEOT)
        /// </summary>
        public Guid LastUsuarioFilterSeot
        {
            get
            {
                return lastUsuarioFilterSeot;
            }
            set
            {
                if (lastUsuarioFilterSeot == value)
                    return;

                SetPropertyValue<Guid>("LastUsuarioFilterSeot", ref lastUsuarioFilterSeot, value);
            }
        }

        /// <summary>
        /// Último filtro de período (Período de Férias)
        /// </summary>
        [Size(-1)]
        public int LastPeriodoFilterPlanejamentoFerias
        {
            get
            {
                return lastPeriodoFilterPlanejamentoFerias;
            }
            set
            {
                if (lastPeriodoFilterPlanejamentoFerias == value)
                    return;

                SetPropertyValue<int>("LastPeriodoFilterPlanejamentoFerias", ref lastPeriodoFilterPlanejamentoFerias, value);
            }
        }

        /// <summary>
        /// Último filtro de situação (Período de Férias)
        /// </summary>
        [Size(-1)]
        public int LastSituacaoFilterPlanejamentoFerias
        {
            get
            {
                return lastSituacaoFilterPlanejamentoFerias;
            }
            set
            {
                if (lastSituacaoFilterPlanejamentoFerias == value)
                    return;

                SetPropertyValue<int>("LastSituacaoFilterPlanejamentoFerias", ref lastSituacaoFilterPlanejamentoFerias, value);
            }
        }

        /// <summary>
        /// Última empresa/instituição salva pelo usuário
        /// </summary>
        [Size(-1)]
        public EmpresaInstituicao LastEmpresaInstituicaoSEOT
        {
            get
            {
                return lastEmpresaInstituicaoSEOT;
            }
            set
            {
                if (lastEmpresaInstituicaoSEOT == value || value == null)
                    return;

                SetPropertyValue<EmpresaInstituicao>("LastEmpresaInstituicaoSEOT", ref lastEmpresaInstituicaoSEOT, value);
            }
        }

        /// <summary>
        /// Último tipo de solicitação definido pelo usuário na SEOT
        /// </summary>
        [Size(-1)]
        public TipoSolicitacao LastTipoSolicitacaoSEOT
        {
            get
            {
                return lastTipoSolicitacaoSEOT;
            }
            set
            {
                if (lastTipoSolicitacaoSEOT == value || value == null)
                    return;

                SetPropertyValue<TipoSolicitacao>("LastTipoSolicitacaoSEOT", ref lastTipoSolicitacaoSEOT, value);
            }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public ColaboradorUltimoFiltro(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }

        /// <summary>
        /// Depois de construir o objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        #endregion

        #region Regras de Negócio
        /// <summary>
        /// Seta o último projeto selecionado pelo colaborador
        /// </summary>
        /// <param name="ultimoProjetoSelecionado">Último projeto selecionado</param>
        public static void RnSalvarUltimoProjetoSelecionado(Session session, Guid oidColaborador, Guid oidProjeto)
        {
            XPCollection<Colaborador> xpColaborador = new XPCollection<Colaborador>(session, CriteriaOperator.Parse("Oid = ?", oidColaborador));

            Colaborador colaborador = xpColaborador[0];

            colaborador.ColaboradorUltimoFiltro.OidUltimoProjetoSelecionado = oidProjeto;

            colaborador.ColaboradorUltimoFiltro.Save();
        }

        #endregion

        #region Consultas
        /// <summary>
        /// Pega último projeto selecionado pelo colaborador
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="oidColaborador">Oid do colaborador</param>
        public static Projeto GetUltimoProjetoSelecionadoPorColaborador(Session session, Guid oidColaborador)
        {
            Guid oidUltimoProjeto = new Guid();

            //pesquisa colaborador.
            XPCollection<Colaborador> xpColaborador = new XPCollection<Colaborador>(session, CriteriaOperator.Parse("Oid = ?", oidColaborador));
            Colaborador colaborador = xpColaborador[0];

            oidUltimoProjeto = colaborador.ColaboradorUltimoFiltro.OidUltimoProjetoSelecionado;

            //realiza pesquisa por projeto.
            XPCollection<Projeto> xpProjeto = new XPCollection<Projeto>(session, CriteriaOperator.Parse("Oid = ?", oidUltimoProjeto));

            if (xpProjeto.Count > 0)
            {
                Projeto projeto = xpProjeto[0];
                return projeto;
            }
            else
            {
                return new Projeto(session);
            }

        }

        /// <summary>
        /// Método que busca o último projeto selecionado por um colaborador.
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="oidColaborador">Oid do Colaborador</param>
        /// <returns>Oid do projeto selecionado ou Oid vazio (Caso não ache nenhum)</returns>
        public static Guid GetUltimoProjetoSelecionadoPorColaboradorDto( Session session, Guid oidColaborador )
        {
            //recupera projeto.
            Projeto projeto = ColaboradorUltimoFiltro.GetUltimoProjetoSelecionadoPorColaborador( session, oidColaborador );

            if(projeto == null)
                return new Guid();

            return projeto.Oid;
        }
        

        #endregion
                
    }

}