using System;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Collections;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;
using WexProject.Library.Libs.Ordenacao;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.Enumerator;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Execucao
{
    /// <summary>
    /// Classe para os ciclos do projeto
    /// 
    /// Obs.: Cores do Situação de Ciclo settadas no Win.Model
    ///       no conditional formatting da propiedade _SituacaoCiclo
    /// </summary>
    [Custom("Caption", "Projetos:Execução:Ciclos")]
    [Appearance("CicloDesenv_Itens_Hide", Criteria = "CsSituacaoCiclo == 'Cancelado'", 
        Enabled = false, TargetItems = "Projeto, TxMeta, CsSituacaoCiclo, DesenvEstorias")]
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class CicloDesenv : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de NbCiclo
        /// </summary>
        private int nbCiclo;

        /// <summary>
        /// Atributo de DtInicio
        /// </summary>
        private DateTime dtInicio;

        /// <summary>
        /// Atributo de DtTermino
        /// </summary>
        private DateTime dtTermino;

        /// <summary>
        /// Atributo de TxMeta
        /// </summary>
        private String txMeta;

        /// <summary>
        /// Atributo de Projeto
        /// </summary>
        private Projeto projeto;

        /// <summary>
        /// Atributo de CsSituacaoCiclo
        /// </summary>
        private CsSituacaoCicloDomain csSituacaoCiclo;

        /// <summary>
        /// Atributo de NbAlcanceMeta
        /// </summary>
        private Double nbAlcanceMeta;

        /// <summary>
        /// Atributo que recebe a maior sequencia de um item do ciclo
        /// </summary>
        private UInt16 nbMaiorCicloDesenvEstoria;

        /// <summary>
        /// Atributo para armazenar o total de pontos realizados em um ciclo.
        /// </summary>
        private double nbPontosRealizados;

        /// <summary>
        /// Armazena o total de pontos planejados em ciclo de desenvolvimento.
        /// </summary>
        private double nbPontosPlanejados;

        /// <summary>
        /// Guarda a parcial de metas
        /// </summary>
        private Double nbQtMetas = 0;

        /// <summary>
        /// Guarda a parcial de estorias
        /// </summary>
        private Double nbQtEstorias = 0;

        /// <summary>
        /// Propriedade que guarda o motivo do cancelamento quando o ciclo for cancelado
        /// </summary>
        private MotivoCancelamento motivoCancelamento;

        /// <summary>
        /// Propriedade que verifica se o ciclo foi cancelado
        /// </summary>
        private bool isCancelado;

        #endregion

        #region Properties

        /// <summary>
        /// Import de projeto
        /// </summary>
        [Indexed]
        [Browsable(false)]
        [Association("ProjetoCicloDesenv", typeof(Projeto))]
        public Projeto Projeto
        {
            get
            {
                return projeto;
            }
            set
            {
                SetPropertyValue<Projeto>("Projeto", ref projeto, value);
            }
        }

        /// <summary>
        /// Variável que guarda o ciclo
        /// </summary>
        [RuleRequiredField("CicloDesenv_NbCiclo_Required", DefaultContexts.Save)]
        public int NbCiclo
        {
            get
            {
                return nbCiclo;
            }
            set
            {
                SetPropertyValue<int>("NbCiclo", ref nbCiclo, value);
            }
        }

        /// <summary>
        /// Variável que guarda a dataHora do inicio do ciclo
        /// </summary>
        [RuleRequiredField("CicloDesenv_DtInicio_Required", DefaultContexts.Save)]
        public DateTime DtInicio
        {
            get
            {
                return dtInicio;
            }
            set
            {
                SetPropertyValue<DateTime>("DtInicio", ref dtInicio, value);
            }
        }

        /// <summary>
        /// Variável que guarda a dataHora do término do ciclo
        /// </summary>
        [RuleRequiredField("CicloDesenv_DtTermino_Required", DefaultContexts.Save)]
        public DateTime DtTermino
        {
            get
            {
                return dtTermino;
            }
            set
            {
                SetPropertyValue<DateTime>("DtTermino", ref dtTermino, value);
            }
        }

        /// <summary>
        /// Variável que guarda a descrição da meta
        /// </summary>
        [RuleRequiredField("CicloDesenv_TxMeta_Required", DefaultContexts.Save, "Informe uma meta", TargetCriteria = "Projeto._AcessoTelaProjeto == true")]
        public String TxMeta
        {
            get
            {
                return txMeta;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxMeta", ref txMeta, value.Trim());
            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos planejados
        /// </summary>
        public Double NbPontosPlanejados
        {
            get
            {
                return nbPontosPlanejados;
            }

            set
            {
                SetPropertyValue<Double>("NbPontosPlanejados", ref nbPontosPlanejados, value);                
            }

        }

        /// <summary>
        /// Variável que guarda o número de pontos realizados
        /// </summary>
        public Double NbPontosRealizados
        {
            get
            {
                return nbPontosRealizados;
            }

            set
            {
                SetPropertyValue<Double>("NbPontosRealizados", ref nbPontosRealizados, value);
            }
        }

        /// <summary>
        /// Variável que guarda a situação do ciclo
        /// </summary>
        [ImmediatePostData]
        public CsSituacaoCicloDomain CsSituacaoCiclo
        {
            get
            {
                return csSituacaoCiclo;
            }
            set
            {
                SetPropertyValue<CsSituacaoCicloDomain>("CsSituacaoCiclo", ref csSituacaoCiclo, value);
            }
        }

        /// <summary>F
        /// Variável que guarda o valor de o quanto foi alcança
        /// </summary>
        public Double NbAlcanceMeta
        {
            get
            {
                return Math.Round(nbAlcanceMeta, 2);
            }
            set
            {
                SetPropertyValue<Double>("NbAlcanceMeta", ref nbAlcanceMeta, value);
            }
        }

        /// <summary>
        /// Propriedade que guarda o valor da maior sequência de um item do ciclo
        /// </summary>
        public UInt16 NbMaiorCicloDesenvEstoria
        {
            get
            {
                return nbMaiorCicloDesenvEstoria;
            }
            set
            {
                SetPropertyValue<UInt16>("NbMaiorCicloDesenvEstoria", ref nbMaiorCicloDesenvEstoria, value);
            }
        }

        /// <summary>
        /// ASsociação de CicloDesenv com Estoria
        /// </summary>
        [Association("CicloDesenv", typeof(CicloDesenvEstoria)), Aggregated]
        public XPCollection DesenvEstorias
        {
            get
            {
                return GetCollection("DesenvEstorias");
            }
        }

        /// <summary>
        /// Propriedade que guarda o motivo do cancelamento quando o ciclo for cancelado
        /// </summary>
        [Custom("Caption", "Motivo de Cancelamento")]
        [Browsable(false)]
        public MotivoCancelamento MotivoCancelamento
        {
            get
            {
                return motivoCancelamento;
            }
            set
            {
                SetPropertyValue<MotivoCancelamento>("MotivoCancelamento", ref motivoCancelamento, value);

                ProjetoUltimoFiltro.RnSetUltimoMotivoCancelamento(Session,Projeto, value);
            }
        }

        /// <summary>
        /// Propriedade que verifica se o ciclo foi cancelado
        /// </summary>
        [Custom("Caption", "Cancelado")]
        [Browsable(false)]
        public bool IsCancelado
        {
            get
            {
                return isCancelado;
            }
            set
            {
                SetPropertyValue<bool>("IsCancelado", ref isCancelado, value);
            }
        }
        #endregion

        #region NonPersistent

        /// <summary>
        /// Armazena os que foram removidos do ciclo para que seja
        /// salvo a situacao da estoria ao salvar o ciclo.
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public List<Estoria> _DeletedItems;

        /// <summary>
        /// Mostra o número do ciclo. Ex: Ciclo "1"
        /// </summary>
        [NonPersistent]
        public String _TxCiclo
        {
            get
            {
                return "Ciclo " + NbCiclo;
            }
        }

        /// <summary>
        /// Atributo não persistente que mostra a concatenação da String TxMeta com as estórias associadas
        /// </summary>
        [NonPersistent]
        public String _TxMeta
        {
            get
            {
                return GetMetaPlusEstorias();
            }
        }

        /// <summary>
        /// Propriedade que concatena o valor da meta com %
        /// </summary>
        [NonPersistent]
        public String _TxAlcanceMeta
        {
            get
            {
                if (NbPontosPlanejados > 0)
                    return String.Format("{0}%", NbAlcanceMeta);
                else
                    return "-";
            }
        }

        /// <summary>
        /// Lista de Estórias que vão para o backlog
        /// </summary>
        [NonPersistent, Browsable(false)]
        public List<CicloDesenvEstoria> _ListaPrioridades
        {
            get;
            set;
        }

        /// <summary>
        /// Lista de Estórias que vão para o próximo ciclo
        /// </summary>
        [NonPersistent, Browsable(false)]
        public List<CicloDesenvEstoria> _ListaProximoCiclo
        {
            get;
            set;
        }

        /// <summary>
        /// Informa como está situação atual do ciclo
        /// </summary>
        [NonPersistent]
        public string _SituacaoCiclo
        {
            get 
            {
                string situacao = string.Empty;

                if (CsSituacaoCiclo == CsSituacaoCicloDomain.Cancelado && MotivoCancelamento!=null) 
                {                    
                    situacao = "Cancelado - " + MotivoCancelamento.TxDescricao;
                }
                else
                {
                    situacao = EnumUtil.DescricaoEnum(CsSituacaoCiclo);
                }

                return situacao;
            }
        }

        /// <summary>
        /// Informa qual a próxima data útil depois da data atual
        /// </summary>
        [NonPersistent,Browsable(false)]
        public DateTime _DataProximoCiclo
        {
            get 
            {
                return Calendario.ProximaDataUtil(Session, DateTime.Now.Date.AddDays(1));
            }
        }

        [NonPersistent, Browsable(false)]
        public uint _NbEstimadoCicloProjeto
        {
            get
            {
                if (Projeto == null) return 0;

                return Projeto.NbTamanhoTotal / Projeto.NbCicloTotalPlan;
            }
        }

        [NonPersistent, Browsable(false)]
        public long _NbTendencia
        {
            get
            {
                if (Projeto == null) return 0;

                return NbCiclo * Projeto.NbRitmoTime;
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Medoto OnSaving
        /// </summary>
        protected override void OnSaving()
        {
            if (Projeto != null)
                Projeto._AcessoTelaProjeto = false;

            if (CsSituacaoCiclo != CsSituacaoCicloDomain.Cancelado)
                RnCalcularSituacaoCiclo();

            foreach (CicloDesenvEstoria item in DesenvEstorias)
            {
                RnRefefinirSituacaoDosItensDoCiclo(item);

                if (item.Estoria.Ciclo == null || item.Estoria.Ciclo.Equals(this))
                {
                    RnIdentificarCiclo(item);
                    item.RnRedefinirSituacaoEstoria();
                    item.RnRedefinirPrioridade();
                }

                RnCalcularPontosDoCiclo(item);

                if (item.Estoria != null && item.EstoriaOld != null && item.Estoria != item.EstoriaOld)
                {
                    item.RnCalcularPrioridadeNaEdicao();
                }

                if (item.Oid == new Guid() ||
                item.Estoria.CsSituacao != item.Estoria._CsSituacaoOld ||
                item.Estoria.NbPrioridade != item.Estoria._NbPrioridadeOld)
                    item.Estoria.Save();
            }

            if (nbQtEstorias > 0)
                NbAlcanceMeta = (Double)((nbQtMetas * 100) / nbQtEstorias);

            RnRepriorizarEstoriasExcluidas();
            RnRepriorizarBacklogAtualSeNecessario();
            base.OnSaving();
        }

        /// <summary>
        /// Metodo OnSaved
        /// </summary>
        protected override void OnSaved()
        {
            int a = NbCiclo;
            base.OnSaved();
            RnRecalcularSituacaoModulo();
        }

        /// <summary>
        /// Caso a lista de prioridades esteja comecando com uma entrega que tenha 
        /// prioridade diferente de 1 entao significa que uma entrega foi incluida 
        /// no ciclo e as entregas que ficaram no backlog deverao ser repriorizadas.
        /// </summary>
        public void RnRepriorizarBacklogAtualSeNecessario()
        {
            ushort prioridade = 1;
            ICollection estorias = Estoria.GetEstoriasPorProjeto(Session, Projeto);
            bool existNovaEstoriaCiclo = false;
            if (estorias != null)
            {
                foreach (Estoria estoria in estorias)
                {
                    if (estoria.NbPrioridade != 0)
                    {
                        if (estoria.NbPrioridade != 1 && existNovaEstoriaCiclo)
                        {
                            estoria.NbPrioridade = prioridade;
                            ((IOrdenacao)estoria).SetReOrdenando(true);
                            estoria.Save();
                        }
                        prioridade = estoria.NbPrioridade;
                        prioridade++;
                    }
                    else
                        existNovaEstoriaCiclo = true;
                }
            }
        }

        /// <summary>
        /// Regra de negócio que calcula a situação do Ciclo
        /// </summary>
        public void RnCalcularSituacaoCiclo()
        {
            UInt16 nbCicloNaoIniciado = 0;
            UInt16 nbCicloEmAndamento = 0;
            UInt16 nbCicloPronto = 0;

            foreach (CicloDesenvEstoria item in DesenvEstorias)
            {
                if (CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Concluido))
                {
                    if (item.CsSituacao.Equals(CsSituacaoEstoriaCicloDomain.EmDesenv))
                        item.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
                    if (item.CsSituacao.Equals(CsSituacaoEstoriaCicloDomain.NaoIniciado))
                        item.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;
                }

                if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.EmDesenv)
                    nbCicloEmAndamento += 1;
                else if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.NaoIniciado)
                    nbCicloNaoIniciado += 1;
                else if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.Pronto || item.CsSituacao == CsSituacaoEstoriaCicloDomain.Replanejado)
                    nbCicloPronto += 1;
            }
            if (nbCicloEmAndamento > 0 || (nbCicloPronto > 0 && nbCicloNaoIniciado > 0))
                CsSituacaoCiclo = CsSituacaoCicloDomain.EmAndamento;
            else
                if (nbCicloPronto > 0 && nbCicloEmAndamento == 0 && nbCicloNaoIniciado == 0)
                    CsSituacaoCiclo = CsSituacaoCicloDomain.Concluido;
                else
                    if (nbCicloNaoIniciado > 0 && nbCicloEmAndamento == 0 && nbCicloPronto == 0)
                        CsSituacaoCiclo = CsSituacaoCicloDomain.Planejado;
                    else
                        if (nbCicloPronto == 0 && nbCicloNaoIniciado == 0 && nbCicloEmAndamento == 0)
                            CsSituacaoCiclo = CsSituacaoCicloDomain.NaoPlanejado;

        }

        /// <summary>
        /// Metodo que redefine a situacao dos itens do ciclo
        /// </summary>
        /// <param name="item">Item da estória</param>
        public void RnRefefinirSituacaoDosItensDoCiclo(CicloDesenvEstoria item)
        {
            if (CsSituacaoCiclo == CsSituacaoCicloDomain.Concluido)
            {
                if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.EmDesenv)
                    item.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;
                else
                    if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.NaoIniciado)
                        item.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;
            }
            else
                if (CsSituacaoCiclo == CsSituacaoCicloDomain.Cancelado)
                {
                    if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.EmDesenv ||
                        item.CsSituacao == CsSituacaoEstoriaCicloDomain.NaoIniciado)
                    {
                        item.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;
                    }
                }
                else
                    if (CsSituacaoCiclo == CsSituacaoCicloDomain.Planejado)
                        item.CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado;
        }


        /// <summary>
        /// Metodo que calcula os pontos do ciclo e o alcance da meta
        /// </summary>
        /// <param name="item">item da estoria (ciclo)</param>
        public void RnCalcularPontosDoCiclo(CicloDesenvEstoria item)
        {
            if (item != null)
            {
                if (item.Meta)
                    nbQtEstorias += item.Estoria.NbTamanho;

                if (item.Meta && item.CsSituacao == CsSituacaoEstoriaCicloDomain.Pronto)
                    nbQtMetas += item.Estoria.NbTamanho;

                if (item.Estoria != null)
                    item.Estoria._TxQuando = _TxCiclo;
            }
        }

        /// <summary>
        /// Metodo que identifica o ciclo em que uma estória ta associada
        /// </summary>
        /// <param name="item">item de ciclo desenvolvimento estoria</param>
        public void RnIdentificarCiclo(CicloDesenvEstoria item)
        {
            if (item != null && item.Estoria != null && item.Estoria.Ciclo != this)
                item.Estoria.Ciclo = this;
        }

        /// <summary>
        /// Totaliza o total de pontos planejados e realizados 
        /// em um ciclo de desenvolvimento.
        /// </summary>
        public void RnCalcularPontosPlanejadosERealizados()
        {
            NbPontosPlanejados = 0;
            NbPontosRealizados = 0;

            foreach (CicloDesenvEstoria item in DesenvEstorias)
            {
                if (item.Estoria != null)
                {
                NbPontosPlanejados += item.Estoria.NbTamanho;

                if (item.CsSituacao == CsSituacaoEstoriaCicloDomain.Pronto)
                    NbPontosRealizados += item.Estoria.NbTamanho;
            }
        }
        }

        /// <summary>
        /// Lista que guarda os Módulos das estórias, para que não sejam salvos Módulos mais de uma vez
        /// </summary>
        public void RnRecalcularSituacaoModulo()
        {
            List<Modulo> lista = new List<Modulo>();

            foreach (CicloDesenvEstoria item in DesenvEstorias)
                if (lista.IndexOf(item.Estoria.Modulo) == -1)
                    lista.Add(item.Estoria.Modulo);

            foreach (Modulo modulo in lista)
            {
                modulo.RnCalcularPontosSituacao();
                modulo.Save();
            }

            if (Session.InTransaction)
                Session.CommitTransaction();
        }

        /// <summary>
        /// Regra de negócio que valida se um ciclo possui ou não associações
        /// </summary>
        [RuleFromBoolProperty("ValidarCicloSemAssociacoes", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "O ciclo não deve ter nenhuma associação para ser atribuido como 'Não Planejado'")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarCiclosSemAssociacoes
        {
            get
            {
                if (CsSituacaoCiclo != CsSituacaoCicloDomain.NaoPlanejado)
                    return DesenvEstorias.Count >= 0;
                else
                    return DesenvEstorias.Count == 0;
            }
        }

        /// <summary>
        /// Validando se o cilco te pelo menos uma meta
        /// </summary>
        [RuleFromBoolProperty("ValidarCicloComPeloMenosUmaMeta", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "O ciclo deve conter pelo menos uma meta")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarCicloComPeloMenosUmaMeta
        {
            get
            {
                int cont = 0;
                if (DesenvEstorias.Count > 0)
                {
                    foreach (CicloDesenvEstoria item in DesenvEstorias)
                    {
                        if (item.Meta == true)
                        {
                            cont = 1;
                            break;
                        }
                    }
                    return cont > 0;
                }
                else
                    return cont == 0;

            }
        }

        /// <summary>
        /// Verifica se o ciclo está com status de Não Planejado, se estiver desabilita botão cancelar ciclo, senão habilita;
        /// </summary>
        /// <returns>true para habilitar e false para desabilitar</returns>
        public bool RnCancelamentoSituacaoNaoIniciado()
        {
            if (!CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Cancelado) && !CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Concluido) &&
                DateUtil.ConsultarDataHoraAtual().Date >= DtInicio.Date)
                return true;

            return false;
        }

        /// <summary>
        /// As estorias que tiverem sido removidas, devem ter 
        /// sua prioridade retornada para o topo do backlog.
        /// </summary>
        private void RnRepriorizarEstoriasExcluidas()
        {
            ushort prioridade = 1;
            _DeletedItems.Reverse(0, _DeletedItems.Count);
            foreach (Estoria estoria in _DeletedItems)
            {
               // estoria.CsSituacao = CsEstoriaDomain.NaoIniciado;
                estoria.NbPrioridade = prioridade;
                ((IOrdenacao)estoria).SetReOrdenando(true);
                estoria.Save();
                prioridade++;
            }

            if (_DeletedItems.Count > 0)
            {
                ICollection estorias = Estoria.GetEstoriasPorProjeto(Session, Projeto);
                if (estorias != null)
                {
                    foreach (Estoria estoria in estorias)
                    {
                        if (estoria.NbPrioridade != 0)
                        {
                            estoria.NbPrioridade = prioridade;
                            ((IOrdenacao)estoria).SetReOrdenando(true);
                            estoria.Save();
                        }
                        prioridade++;
                    }
                }
            }

            _DeletedItems.Clear();
        }

        /// <summary>
        /// Monta uma lista de estórias que devem ser remanejadas para o próximo ciclo
        /// </summary>
        public void RnCriarListasDestinoEstorias(XPCollection estorias)
        {
            foreach (CicloDesenvEstoria item in estorias)
            {
                _ListaProximoCiclo.Add(item);
        }
        }

        /// <summary>
        /// Troca a posição em ua lista
        /// </summary>
        /// <param name="posicao">ciclo com o ciclodesenv</param>
        /// <param name="listaSelecionada">0 - Lista Proximo Ciclo, 1 - Lista Prioridades</param>
        /// <param name="sobe">true - subida, false - descida</param>
        private void RnTrocaPosicaoListaPendentes(CicloDesenvEstoria posicao, int listaSelecionada, bool sobe)
        {
            if (listaSelecionada.Equals(0))
            {
                if (_ListaProximoCiclo == null)
                {
                    return;
                }

                int index = _ListaProximoCiclo.IndexOf(posicao);

                if (sobe && ((_ListaProximoCiclo.IndexOf(posicao)) == 0) || !sobe && ((_ListaProximoCiclo.IndexOf(posicao)) == (_ListaProximoCiclo.Count - 1)))
                    return;

                if (sobe && index > 0)
                {
                    _ListaProximoCiclo.Insert(index - 1, posicao);
                    _ListaProximoCiclo.RemoveAt(index + 1);
                }
                else if (index < _ListaProximoCiclo.Count && index >= 0)
                {
                    _ListaProximoCiclo.Insert(index + 2, posicao);
                    _ListaProximoCiclo.RemoveAt(index);
                }
            }
            else
            {
                if (_ListaPrioridades == null)
                    return;

                int index = _ListaPrioridades.IndexOf(posicao);

                if (sobe && ((_ListaPrioridades.IndexOf(posicao)) == 0) || !sobe && ((_ListaPrioridades.IndexOf(posicao)) == (_ListaPrioridades.Count - 1)))
                    return;

                if (sobe && index > 0)
                {
                    _ListaPrioridades.Insert(index - 1, posicao);
                    _ListaPrioridades.RemoveAt(index + 1);
                }
                else if (index < _ListaPrioridades.Count && index >= 0)
                {
                    _ListaPrioridades.Insert(index + 2, posicao);
                    _ListaPrioridades.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// Regra que cria um loop para subida e descida de estórias em uma lista de estórias pendentes
        /// </summary>
        /// <param name="selecionados">posições selecionadas</param>
        /// <param name="listaSelecionada">0 - Lista Proximo Ciclo, 1 - Lista Prioridades</param>
        /// <param name="sobe">true - subida, false- descida</param>
        public void RnTrocarPosicoesListaPendentes(List<CicloDesenvEstoria> selecionadas, int listaSelecionada, bool sobe)
        {
            if (listaSelecionada.Equals(0))
            {
                if (selecionadas.Count == _ListaProximoCiclo.Count)
                    return;
            }
            else
            {
                if (selecionadas.Count == _ListaPrioridades.Count)
                    return;
            }

            if (!sobe)
                selecionadas.Reverse();

            foreach (CicloDesenvEstoria estoriaSelecionada in selecionadas)
            {
                RnTrocaPosicaoListaPendentes(estoriaSelecionada, listaSelecionada, sobe);
            }
        }

        /// <summary>
        /// Indica se é para exibir ou não a janela de itens pendentes
        /// </summary>
        /// <returns>Se é para exibir ou não</returns>
        public bool IsExibirJanelaDestinoItensPendentes()
        {
            if (CsSituacaoCiclo != CsSituacaoCicloDomain.Concluido)
            {
                return false;
            }

            return CriarListasItensPendentes();
        }

        /// <summary>
        /// Criação das Listas de Itens Pendentes
        /// </summary>
        /// <returns>True, se criou e False se não criou</returns>
        public bool CriarListasItensPendentes(bool save = true)
        {
            int count;

            // Reiniciando valores
            _ListaPrioridades = new List<CicloDesenvEstoria>();
            _ListaProximoCiclo = new List<CicloDesenvEstoria>();

            DesenvEstorias.Filter = CriteriaOperator.Parse("CsSituacao = ? OR CsSituacao = ?",
                CsSituacaoEstoriaCicloDomain.NaoIniciado, CsSituacaoEstoriaCicloDomain.EmDesenv);

            // quantidade de registros no filtro
            count = DesenvEstorias.Count;

            if (count > 0)
            {
                RnCriarListasDestinoEstorias(DesenvEstorias);
                DesenvEstorias.Filter = null;

                // Próximo Ciclo
                CicloDesenv ciclo = GetProximoCicloNaoFinalizado();
                if (ciclo == null)
                {
                    _ListaPrioridades = new List<CicloDesenvEstoria>(_ListaProximoCiclo);
                    _ListaProximoCiclo = new List<CicloDesenvEstoria>();

                    if (save)
                    {
                        RnSalvarDestinoEstoriasPendentes();
                    }

                    return false;
                }

                return true;
            }

            DesenvEstorias.Filter = null;

            return false;
        }

        /// <summary>
        /// Verifica se algum dos próximos ciclos estão em uma situação diferente a não planejado, se sim não mostra, senão não mostra
        /// </summary>
        /// <returns>true or false</returns>
        public bool RnMostrarInicioProximoCiclo()
        {
            Projeto.Ciclos.Sorting.Add(new SortProperty("NbCiclo", SortingDirection.Ascending));

            if (DateUtil.ConsultarDataHoraAtual().CompareTo(DtTermino) > 0 ||
                Projeto.Ciclos.IndexOf(this) + 1 >= Projeto.Ciclos.Count)
            {
                return false;
            }

            for (int i = Projeto.Ciclos.IndexOf(this) + 1; i < Projeto.Ciclos.Count; i++)
            {
                if (!Projeto.Ciclos[i].CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Cancelado) &&
                    !Projeto.Ciclos[i].CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Concluido))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Envia as estórias selecionadas da lista de 'Prioridades' para a lista de 'Próximo Ciclo'
        /// </summary>
        /// <param name="selecionadas">Lista de Estórias selecionadas</param>
        public void RnEnviarEstoriasSelecionadasDePrioridadesParaProximoCiclo(List<CicloDesenvEstoria> selecionadas)
        {
            List<CicloDesenvEstoria> selecionadasCollection = new List<CicloDesenvEstoria>(selecionadas);

            foreach (CicloDesenvEstoria estoria in selecionadasCollection)
            {
                if (estoria == null) continue;

                if (_ListaPrioridades.Contains(estoria))
                {
                    _ListaPrioridades.Remove(estoria);
                }

                if (!_ListaProximoCiclo.Contains(estoria))
                {
                    _ListaProximoCiclo.Add(estoria);
                }
            }
        }

        /// <summary>
        /// Envia as estórias selecionadas da lista de 'Próximo Ciclo' para a lista de 'Prioridades'
        /// </summary>
        /// <param name="selecionadas">Lista de Estórias selecionadas</param>
        public void RnEnviarEstoriasSelecionadasDeProximoCicloParaPrioridades(List<CicloDesenvEstoria> selecionadas)
        {
            List<CicloDesenvEstoria> selecionadasCollection = new List<CicloDesenvEstoria>(selecionadas);

            foreach (CicloDesenvEstoria estoria in selecionadasCollection)
            {
                if (estoria == null) continue;

                if (_ListaProximoCiclo.Contains(estoria))
                {
                    _ListaProximoCiclo.Remove(estoria);
                }

                if (!_ListaPrioridades.Contains(estoria))
                {
                    _ListaPrioridades.Add(estoria);
                }
            }
        }

        /// <summary>
        /// Salva o destino das Estórias pendentes
        /// </summary>
        public void RnSalvarDestinoEstoriasPendentes()
        {
            ushort position = 1;

            // Prioridades
            foreach(CicloDesenvEstoria estoria in _ListaPrioridades)
            {
                estoria.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;
                estoria.Estoria.NbPrioridade = position;

                estoria.Estoria.Save();

                position++;
            }

            CicloDesenv nextCiclo = GetProximoCicloNaoFinalizado();
            position = 1;

            // Próximo Ciclo
            foreach (CicloDesenvEstoria estoria in _ListaProximoCiclo)
            {
                estoria.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;

                if (nextCiclo != null)
                {
                    new CicloDesenvEstoria(Session)
                    {
                        CsSituacao = CsSituacaoEstoriaCicloDomain.NaoIniciado,
                        Ciclo = nextCiclo,
                        Estoria = estoria.Estoria,
                        Meta = true,
                        NbSequencia = position
                    }.Save();

                    position++;
                }
            }
        }

        /// <summary>
        /// Cancelar o Ciclo Atual
        /// </summary>
        /// <param name="motivo">Motivo de Cancelamento</param>
        /// <param name="dtInicioNextCiclo">Data de Início do Próximo Ciclo</param>
        public void RnCancelarCiclo(MotivoCancelamento motivo, DateTime dtInicioNextCiclo)
        {
            IsCancelado = true;
            CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;

            MotivoCancelamento = motivo;

            if (dtInicioNextCiclo != DateTime.MinValue && RnMostrarInicioProximoCiclo())
            {
                DtTermino = Calendario.PrimeiroDiaUtilAnteriorData(Session, dtInicioNextCiclo);

                for (int position = Projeto.Ciclos.IndexOf(this) + 1; position < Projeto.Ciclos.Count; position++)
                {
                    Projeto.Ciclos[position].DtInicio = dtInicioNextCiclo;
                    Projeto.DtTerminoReal = Projeto.Ciclos[position].DtTermino = Calendario.AcrescimoDiasUteisData(Session,
                        dtInicioNextCiclo, Projeto.NbCicloDuracaoDiasPlan);

                    dtInicioNextCiclo = Calendario.AcrescimoDiasUteisData(Session,
                        Projeto.Ciclos[position].DtTermino.AddDays(1), Projeto.NbCicloDiasIntervalo);
                }

                Projeto.Save();
            }

            if (_ListaPrioridades != null && _ListaProximoCiclo != null &&
                (_ListaPrioridades.Count > 0 || _ListaProximoCiclo.Count > 0))
            {
                RnSalvarDestinoEstoriasPendentes();
            }
        }

        /// <summary>
        /// Método que retorna a mensagem de erro usado no form de cancelamento de ciclo,
        /// quando a data do proximo ciclo não está entre as datas de final do ciclo cancelado
        /// e inicio do proximo ciclo
        /// </summary>
        public string RnDataProximoCiclo(DateTime data)
        {
            int index = Projeto.Ciclos.IndexOf(this) + 1;

            Projeto.Ciclos.Sorting.Add(new SortProperty("NbCiclo", SortingDirection.Ascending));
            if (index >= Projeto.Ciclos.Count)
            {
                return string.Empty;
            }

            DateTime dtInicioProximoCiclo = Projeto.Ciclos[index].DtInicio;
            if (data <= DtInicio || dtInicioProximoCiclo < data)
            {
                return String.Format("A data de Início do Próximo Ciclo deve estar entre {0:dd/MM/yyyy} e {1:dd/MM/yyyy}",
                    DtInicio.AddDays(1), dtInicioProximoCiclo);
            }

            return string.Empty;
        }

        /// <summary>
        /// Validação do Motivo de Cancelamento
        /// </summary>
        /// <param name="motivo">Motivo de Cancelamento</param>
        /// <returns>Se é um Motivo válido</returns>
        public static string RnValidarMotivoCancelamento(MotivoCancelamento motivo)
        {
            if (motivo == null)
            {
                return "É necessário informar um Motivo de Cancelamento";
            }

            return string.Empty;
        }

        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// String que concatena o campo TxMeta com as Estórias associadas
        /// </summary>
        /// <returns>String concatenada</returns>
        public String GetMetaPlusEstorias()
        {
            StringBuilder str = new StringBuilder();

            if (!string.IsNullOrEmpty(TxMeta))
            {
                str.AppendLine(TxMeta);
            }

            DesenvEstorias.Sorting.Add(new SortProperty("NbSequencia", SortingDirection.Ascending));

            foreach (CicloDesenvEstoria item in DesenvEstorias)
            {
                if (item.Meta == true)
                    str.AppendLine(String.Format("- {0}({1} pts, Meta) - {2}", StrUtil.LimitarTamanhoColuna(100, item.Estoria._TxIdEstoria_Titulo), item.Estoria.NbTamanho, EnumUtil.DescricaoEnum(item.CsSituacao)));
                else
                    str.AppendLine(String.Format("- {0}({1} pts) - {2}", StrUtil.LimitarTamanhoColuna(100, item.Estoria._TxIdEstoria_Titulo), item.Estoria.NbTamanho, EnumUtil.DescricaoEnum(item.CsSituacao)));
            }

            return str.ToString();
        }
        /// <summary>
        /// Get que retorna o ultimo ciclo cadastrado
        /// </summary>
        /// <param name="projeto">projeto</param>
        /// <returns>Último ciclo cadastrado</returns>
        public static UInt16 GetUltimoCiclo(Projeto projeto)
        {
            UInt16 soma = 0;

            try
            {
                object obj = projeto.Session.Evaluate(typeof(CicloDesenv),
                CriteriaOperator.Parse("Max(NbCiclo)"), CriteriaOperator.Parse("Projeto.Oid = ?", projeto.Oid));

                if (obj != null)
                    soma = Convert.ToUInt16(obj);
            }
            catch
            {
            }

            return soma;
        }

        /// <summary>
        /// Get que retorna todos os ciclos de um projeto
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <returns>Todos os ciclos de um projeto</returns>
        public static XPCollection GetCiclos(Session session, Projeto projeto)
        {
            return new XPCollection(session, typeof(CicloDesenv),
            CriteriaOperator.Parse(String.Format("Projeto.Oid = '{0}'", projeto.Oid)));
        }

        /// <summary>
        /// Collection que retorna os ciclos com a situacao concluida
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <returns>Ciclos com a situação concluida</returns>
        public static XPCollection GetCiclosProntos(Session session, Projeto projeto)
        {
            return new XPCollection(session, typeof(CicloDesenv),
            CriteriaOperator.Parse(String.Format("Projeto.Oid = '{0}' And CsSituacaoCiclo = 'Concluido' ", projeto.Oid)));
        }

        /// <summary>
        /// Collection que retorna os ciclos com a situação cancelada
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <returns>Ciclos cancelados</returns>
        public static XPCollection GetCiclosCancelados(Session session, Projeto projeto)
        {
            return new XPCollection(session, typeof(CicloDesenv),
            CriteriaOperator.Parse(String.Format("Projeto.Oid = '{0}' And CsSituacaoCiclo = 'Cancelado' ", projeto.Oid)));
        }

        /// <summary>
        /// Obtenção do próximo Ciclo não finalizado
        /// </summary>
        /// <returns>Objeto de CicloDesenv</returns>
        public CicloDesenv GetProximoCicloNaoFinalizado()
        {
            CicloDesenv ciclo = null;

            Projeto.Ciclos.Filter = CriteriaOperator.Parse("NbCiclo > ? AND CsSituacaoCiclo != ? AND CsSituacaoCiclo != ?", NbCiclo,
                CsSituacaoCicloDomain.Concluido, CsSituacaoCicloDomain.Cancelado);

            if (Projeto.Ciclos.Count > 0)
            {
                Projeto.Ciclos.Sorting.Add(new SortProperty("NbCiclo", SortingDirection.Ascending));
                ciclo = Projeto.Ciclos[0];
            }

            // Retirada do Filtro
            Projeto.Ciclos.Filter = null;

            return ciclo;
        }

        #endregion

        #region Utils

        public Double GetPontosMeta()
        {
            Double pontosMeta = 0;
            foreach (CicloDesenvEstoria item in DesenvEstorias)
            {
                if (item.Meta)
                {
                    pontosMeta += item.Estoria.NbTamanho;
                }
            }
            return pontosMeta;
        }

        #endregion

        #region User Interface

        /// <summary>
        /// Metodo que verifica se o ciclo está concluido
        /// </summary>
        /// <param name="active">parametro que verifica se o ciclo foi concluido</param>
        /// <returns>retorna o estado do ciclo</returns>
        [EditorStateRule("DisabledSeCicloEstiverConcluido", "DesenvEstorias", ViewType.DetailView)]
        public EditorState DisabledSeCicloEstiverConcluido(out bool active)
        {
            active = CsSituacaoCiclo == CsSituacaoCicloDomain.Concluido;
            return EditorState.Disabled;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtores da classe
        /// </summary>
        /// <param name="session">session</param>
        public CicloDesenv(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _DeletedItems = new List<Estoria>();
            NbCiclo = 0;
            DtInicio = DateTime.MinValue;
            DtTermino = DateTime.MinValue;
            NbAlcanceMeta = 0;
            NbMaiorCicloDesenvEstoria = 0;
            Projeto = Projeto.GetProjetoAtual(Session);
        }

        /// <summary>
        /// Ao carregar o objeto.
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();
            _DeletedItems = new List<Estoria>();
        }

        #endregion

    }
}