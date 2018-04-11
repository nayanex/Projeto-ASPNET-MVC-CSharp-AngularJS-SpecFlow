using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.NovosNegocios;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.Entities.RH
{
    public partial class Colaborador
    {
        #region Construtor

        public Colaborador()
        {
            this.Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public System.Guid Oid { get; set; }

        public string TxMatricula { get; set; }

        public Nullable<System.DateTime> DtAdmissao { get; set; }

        public Nullable<int> OptimisticLockField { get; set; }

        public Nullable<int> GCRecord { get; set; }

        #endregion

        #region NaoMapeados

		[NotMapped]
		public string NomeCompleto
		{
			get
			{
				String nome = "";

				if (Usuario != null && Usuario.Person != null)
				{
					nome = Usuario.Person.FirstName + " " + Usuario.Person.LastName;
				}
				return nome;
			}
		}

        #endregion

        #region Chaves Estrangeiras

        [Column( "Coordenador" ), ForeignKey( "Coordenador" )]
        public Nullable<System.Guid> OidCoordenador { get; set; }

        [ForeignKey( "Usuario" )]
        public Nullable<System.Guid> OidUsuario { get; set; }

        [ForeignKey( "Cargo" )]
        public Nullable<System.Guid> OidCargo { get; set; }

        [Column( "ColaboradorUltimoFiltro" ), ForeignKey( "ColaboradorUltimoFiltro" )]
        public Nullable<System.Guid> OidColaboradorUltimoFiltro { get; set; }

        #endregion

        #region Propriedades Navegacionais

        public Cargo Cargo { get; set; }
        public ColaboradorUltimoFiltro ColaboradorUltimoFiltro { get; set; }
        public ICollection<Colaborador> Colaborador1 { get; set; }
        public Colaborador Coordenador { get; set; }
        public User Usuario { get; set; }
        public ICollection<ColaboradorAfastamento> ColaboradorAfastamentoes { get; set; }
        public ICollection<ColaboradorPeriodoAquisitivo> ColaboradorPeriodoAquisitivoes { get; set; }
        public ICollection<Projeto> Projetoes { get; set; }
        public ICollection<Ferias> Ferias { get; set; }
        public ICollection<ParteInteressada> ParteInteressadas { get; set; }
        public ICollection<ProjetoColaboradorConfig> ProjetoColaboradorConfigs { get; set; }
        public ICollection<SolicitacaoOrcamento> SolicitacaoOrcamentoes { get; set; }
        public ICollection<SolicitacaoOrcamentoHistorico> SolicitacaoOrcamentoHistoricoes { get; set; }
        public ICollection<SolicitacaoOrcamentoHistorico> SolicitacaoOrcamentoHistoricoes1 { get; set; }
        public ICollection<Tarefa> Tarefas { get; set; }
        public ICollection<TarefaHistoricoTrabalho> TarefaHistoricoTrabalhoes { get; set; }
        public ICollection<TarefaLogAlteracao> TarefaLogAlteracaos { get; set; }
        public ICollection<TarefaResponsaveis> TarefaResponsaveis { get; set; }

        #endregion
    }
}
