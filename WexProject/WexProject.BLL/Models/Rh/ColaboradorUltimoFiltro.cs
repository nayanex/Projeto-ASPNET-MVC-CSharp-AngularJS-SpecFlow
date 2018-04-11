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
    /// Classe de �ltimo filtro
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [NavigationItem(false)]
    [OptimisticLocking( false )]
    public class ColaboradorUltimoFiltro : BaseObject
    {
        #region Atributos

        /// <summary>
        /// �ltimo filtro de situa��o (SEOT)
        /// </summary>
        private Guid lastSituacaoFilterSeot;

        /// <summary>
        /// �ltimo filtro de usu�rio (SEOT)
        /// </summary>
        private Guid lastUsuarioFilterSeot;

        /// <summary>
        /// �ltimo filtro de per�odo (Per�odo de F�rias)
        /// </summary>
        private int lastPeriodoFilterPlanejamentoFerias;

        /// <summary>
        /// �ltimo filtro de situa��o (Per�odo de F�rias)
        /// </summary>
        private int lastSituacaoFilterPlanejamentoFerias;
        /// <summary>
        /// Ultimo filtro de superior imediato (PlanejamentoFerias)
        /// </summary>
        private string lastSuperiorImediatoFilterPlanejamentoFerias;
        /// <summary>
        /// Ultimo filtro de situa�ao de ferias (PlanejamentoFerias)
        /// </summary>
        private string lastSituacaoFeriasFilterPlanejamentoFerias;

        /// <summary>
        /// �ltimo Motivo de Cancelamento escolhido pelo usu�rio
        /// </summary>
        private MotivoCancelamento lastMotivoCancelamento;

        /// <summary>
        /// �ltima empresa/institui��o salva pelo usu�rio
        /// </summary>
        private EmpresaInstituicao lastEmpresaInstituicaoSEOT;

        /// <summary>
        /// �ltimo tipo de solicita��o definido pelo usu�rio na SEOT
        /// </summary>
        private TipoSolicitacao lastTipoSolicitacaoSEOT;

        /// <summary>
        /// Oid �ltimo projeto selecionado definido pelo usu�rio
        /// </summary>
        private Guid oidUltimoProjetoSelecionado;

        #endregion

        #region Propriedades

        /// <summary>
        /// Oid �ltimo projeto selecionado pelo colaborador.
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
        /// Ultimo filtro de situa�ao de ferias (PlanejamentoFerias)
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
        /// �ltimo filtro de situa��o (SEOT)
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
        /// �ltimo filtro de usu�rio (SEOT)
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
        /// �ltimo filtro de per�odo (Per�odo de F�rias)
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
        /// �ltimo filtro de situa��o (Per�odo de F�rias)
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
        /// �ltima empresa/institui��o salva pelo usu�rio
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
        /// �ltimo tipo de solicita��o definido pelo usu�rio na SEOT
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
        /// <param name="session">Sess�o</param>
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

        #region Regras de Neg�cio
        /// <summary>
        /// Seta o �ltimo projeto selecionado pelo colaborador
        /// </summary>
        /// <param name="ultimoProjetoSelecionado">�ltimo projeto selecionado</param>
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
        /// Pega �ltimo projeto selecionado pelo colaborador
        /// </summary>
        /// <param name="session">Sess�o atual</param>
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
        /// M�todo que busca o �ltimo projeto selecionado por um colaborador.
        /// </summary>
        /// <param name="session">Sess�o Corrente</param>
        /// <param name="oidColaborador">Oid do Colaborador</param>
        /// <returns>Oid do projeto selecionado ou Oid vazio (Caso n�o ache nenhum)</returns>
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