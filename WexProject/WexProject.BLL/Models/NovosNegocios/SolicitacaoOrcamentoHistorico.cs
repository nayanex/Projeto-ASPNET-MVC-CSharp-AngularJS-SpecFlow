using System;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Rh;
using WexProject.Library.Libs.DataHora;

namespace WexProject.BLL.Models.NovosNegocios
{
    /// <summary>
    /// Classe SolicitacaoOrcamentoHistoricos
    /// </summary>
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class SolicitacaoOrcamentoHistorico : BaseObject
    {

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public SolicitacaoOrcamentoHistorico(Session session)
            : base(session)
        {
        }

        #region Attributes

        /// <summary>
        /// Comentario do Historico
        /// </summary>
        private string comentario;

        /// <summary>
        /// Data/Hora da Criação do Historico
        /// </summary>
        private DateTime dataHora;

        /// <summary>
        /// Responsavel pela SEOT
        /// </summary>
        private Colaborador responsavelHistorico;

        /// <summary>
        /// Atributo de Situação
        /// </summary>
        private ConfiguracaoDocumentoSituacao situacao;

        /// <summary>
        /// Em qual Solicitação o Historico esta ligado
        /// </summary>
        private SolicitacaoOrcamento solicitacaoOrcamento;

        /// <summary>
        /// atributo que diz quem alterou a SEOT
        /// </summary>
        private Colaborador atualizadoPor;

        #endregion

        #region Properties

        /// <summary>
        /// Propriedade que liga o historico a SEOT
        /// </summary>
        [Association("SolicitacaoOrcamento_SolicitacaoOrcamentoHistoricos", typeof(SolicitacaoOrcamento))]
        public SolicitacaoOrcamento SolicitacaoOrcamento
        {
            get
            {
                return solicitacaoOrcamento;
            }
            set
            {
                SetPropertyValue<SolicitacaoOrcamento>("SolicitacaoOrcamento", ref solicitacaoOrcamento, value);
            }
        }

        /// <summary>
        /// Propiedade que guarda a dataHora/hora
        /// </summary>
        [Custom("Caption", "Data/Hora")]
        public DateTime DataHora
        {
            get
            {
                return dataHora;
            }
            set
            {
                SetPropertyValue<DateTime>("Data", ref dataHora, value);
            }
        }

        /// <summary>
        /// Variavel que armazena quem alterou a SEOT
        /// </summary>
        [Custom("Caption", "Atualizado Por")]
        public Colaborador AtualizadoPor
        {
            get
            {
                return atualizadoPor;
            }
            set
            {
                SetPropertyValue<Colaborador>("AtualizadoPor", ref atualizadoPor, value);
            }
        }

        /// <summary>
        /// Propiedade que guarda o Comentario
        /// </summary>
        [Custom("Caption", "Comentário")]
        [Size(SizeAttribute.Unlimited)]
        public string Comentario
        {
            get
            {
                return comentario;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("Comentario", ref comentario, value.Trim());
            }
        }

        /// <summary>
        /// Propiedade que guarda a situação
        /// </summary>
        [Custom("Caption", "Situação")]
        public ConfiguracaoDocumentoSituacao Situacoes
        {
            get
            {
                return situacao;
            }
            set
            {
                SetPropertyValue<ConfiguracaoDocumentoSituacao>("Situacoes", ref situacao, value);
            }
        }

        /// <summary>
        /// Propiedade que guarda o responsavel pela alteração da SEOT
        /// </summary>
        [Custom("Caption", "Responsável pela Solicitação")]
        public Colaborador ResponsavelHistorico
        {
            get
            {
                return responsavelHistorico;
            }
            set
            {
                SetPropertyValue<Colaborador>("ResponsavelHistorico", ref responsavelHistorico, value);
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Util para sempre atribuir horario/dataHora atuais
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DataHora = DateUtil.ConsultarDataHoraAtual();
        }

        #endregion

        #region DBQuery
        /// <summary>
        /// Retorno o último histórico alterado de uma Solicitação de Orçamento
        /// </summary>
        /// <param name="orcamento">Solicitação de Orçamento para ser obter o último histórico</param>
        /// <returns>O último histórico alterado de uma Solicitação de Orçamento</returns>
        public static SolicitacaoOrcamentoHistorico GetUltimoHistorico(SolicitacaoOrcamento orcamento) 
        {
            var collection = orcamento.SolicitacaoOrcamentoHistoricos;
            collection.Sorting.Add(new SortProperty("DataHora", DevExpress.Xpo.DB.SortingDirection.Ascending));
            return collection[0];
        }

        #endregion

    }
}
