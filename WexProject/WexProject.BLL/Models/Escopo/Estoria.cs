using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using DevExpress.Xpo.DB;
using WexProject.BLL.Models.Geral;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.ExpressApp;
using WexProject.Library.Libs.Ordenacao;
using System.Collections.Generic;
using System.Collections;
using WexProject.BLL.Models.Execucao;
using WexProject.BLL.Shared.Domains.Escopo;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Escopo
{
    /// <summary>
    /// Classe de estórias
    /// </summary>
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("Estoria_Modulo_TxTitulo_Unique", DefaultContexts.Save, "TxTitulo, Modulo.Projeto", Name = "JaExisteUmaEstoriaNoModulo",
    CustomMessageTemplate = "Já existe uma estória com esse titulo no Projeto")]
    [DeferredDeletion(true)]
    [OptimisticLocking( false )]
    public class Estoria : BaseObject, IOrdenacao
    {
        #region Attributes
        /// <summary>
        /// Atributo de módulo
        /// </summary>
        private Modulo modulo;

        /// <summary>
        /// Atributo para gardar se uma prioridade ja foi salva
        /// </summary>
        [Browsable(false)]
        private bool salvandoPrioridades;

        /// <summary>
        /// Atributo que guarda o valor antigo de uma estória
        /// </summary>
        private Estoria estoriaPaiOld;

        /// <summary>
        /// Atributo que guarda o valor antigo do módulo
        /// </summary>
        private Modulo moduloOld;

        /// <summary>
        /// Atributo de TxID
        /// </summary>
        private String txID;

        /// <summary>
        /// Atributo de TxTitulo
        /// </summary>
        private String txTitulo;

        /// <summary>
        /// Atributo de CsTipoEstoriaDomain
        /// </summary>
        private CsTipoEstoriaDomain csTipo;

        /// <summary>
        /// Atributo de TxGostariaDe
        /// </summary>
        private String txGostariaDe;

        /// <summary>
        /// Atributo de TxEntaoPoderei
        /// </summary>
        private String txEntaoPoderei;

        /// <summary>
        /// Atributo de Solicitante
        /// </summary>
        private ProjetoParteInteressada projetoParteInteressada;

        /// <summary>
        /// Atributo de CsValorNegocioDomain
        /// </summary>
        private CsValorNegocioDomain csValorNegocio;

        /// <summary>
        /// Atributo de ProjetoParteInteressada
        /// </summary>
        private Beneficiado beneficiadoProjeto;

        /// <summary>
        /// Atributo de TxObservacoes
        /// </summary>
        private String txAnotacoes;

        /// <summary>
        /// Atributo de TxReferencias
        /// </summary>
        private String txReferencias;

        /// <summary>
        /// Atributo de TxDuvidas
        /// </summary>
        private String txDuvidas;

        /// <summary>
        /// Atributo de EstoriaPai
        /// </summary>
        private Estoria estoriaPai;

        /// <summary>
        /// Atributo de NbPrioridade
        /// </summary>
        private UInt16 nbPrioridade;

        /// <summary>
        /// Atributo de CsSituacao
        /// </summary>
        private CsEstoriaDomain csSituacao;

        /// <summary>
        /// Atributo de TxPremissas
        /// </summary>
        private String txPremissas;

        /// <summary>
        /// Atributo de NbTamanho
        /// </summary>
        private double nbTamanho;

        /// <summary>
        /// Atributo para guardar o projeto selecionado
        /// </summary>
        private Projeto projetoSelecionado;

        /// <summary>
        /// Atributo de Ciclo
        /// </summary>
        private CicloDesenv ciclo;

        private bool csEmAnalise;
        #endregion

        #region Properties

        /// <summary>
        /// Variável que guarda o ID da estória
        /// </summary>
        [Size(20)]
        public String TxID
        {
            get
            {
                return txID;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxID", ref txID, value.Trim());
            }
        }

        /// <summary>
        /// Variável que guarda oo titulo da estória
        /// </summary>
        [RuleRequiredField("Estoria_TxTitulo_Required", DefaultContexts.Save, "Informe um titulo para a estória")]
        [Size(SizeAttribute.Unlimited)]
        public String TxTitulo
        {
            get
            {
                return txTitulo;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxTitulo", ref txTitulo, value.Trim());
            }
        }

        /// <summary>
        /// Importa o modulo do projeto
        /// </summary>
        [RuleRequiredField("Estoria_TxModulo_Required", DefaultContexts.Save, "Selecione um módulo")]
        public Modulo Modulo
        {
            get
            {
                return modulo;
            }
            set
            {
                SetPropertyValue<Modulo>("Modulo", ref modulo, value);
            }
        }

        /// <summary>
        /// Import de Ciclo
        /// </summary>
        [Browsable(false)]
        public CicloDesenv Ciclo
        {
            get
            {
                return ciclo;
            }
            set
            {
                SetPropertyValue<CicloDesenv>("Ciclo", ref ciclo, value);
            }
        }

        /// <summary>
        /// Importa as choices de Tipo
        /// </summary>
        [RuleRequiredField("Estoria_CsTipo_Required", DefaultContexts.Save, "Selecione um tipo para a estória")]
        public CsTipoEstoriaDomain CsTipo
        {
            get
            {
                return csTipo;
            }
            set
            {
                SetPropertyValue<CsTipoEstoriaDomain>("CsTipo", ref csTipo, value);
            }
        }

        /// <summary>
        /// Variável do campo Gostaria De:
        /// </summary>
        [ImmediatePostData]
        [Size(SizeAttribute.Unlimited)]
        public String TxGostariaDe
        {
            get
            {
                return txGostariaDe;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxGostariaDe", ref txGostariaDe, value.Trim());

                RnPreencherValorTitulo();
            }
        }

        /// <summary>
        /// Variável do campo Entao Poderei:
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String TxEntaoPoderei
        {
            get
            {
                return txEntaoPoderei;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxEntaoPoderei", ref txEntaoPoderei, value.Trim());
            }
        }

        /// <summary>
        /// Importa Solicitante
        /// </summary>
        [Indexed]
        [RuleRequiredField("Estoria_ProjetoParteInteressada_Required", DefaultContexts.Save, "Informe um solicitante")]
        public ProjetoParteInteressada ProjetoParteInteressada
        {
            get
            {
                return projetoParteInteressada;
            }
            set
            {
                SetPropertyValue<ProjetoParteInteressada>("ProjetoParteInteressada", ref projetoParteInteressada, value);
            }
        }

        /// <summary>
        /// Importa choides de ValorNegocio
        /// </summary>
        [RuleRequiredField("Estoria_CsValorNegocio_Required", DefaultContexts.Save, "Selecione um valor de negócio")]
        public CsValorNegocioDomain CsValorNegocio
        {
            get
            {
                return csValorNegocio;
            }
            set
            {
                SetPropertyValue<CsValorNegocioDomain>("CsValorNegocio", ref csValorNegocio, value);
            }
        }

        /// <summary>
        /// Importa Beneficiado
        /// </summary>
        [ImmediatePostData]
        public Beneficiado ComoUmBeneficiado
        {
            get
            {
                return beneficiadoProjeto;
            }
            set
            {
                SetPropertyValue<Beneficiado>("BeneficiadoProjeto", ref beneficiadoProjeto, value);
                RnPreencherValorTitulo();
            }
        }

        /// <summary>
        /// Variável do campo Observações
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String TxAnotacoes
        {
            get
            {
                return txAnotacoes;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxObservacoes", ref txAnotacoes, value.Trim());
            }
        }

        /// <summary>
        /// Variável do campo Referencias
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String TxReferencias
        {
            get
            {
                return txReferencias;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxReferencias", ref txReferencias, value.Trim());
            }
        }

        /// <summary>
        /// Variável do campo Dúvidas
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String TxDuvidas
        {
            get
            {
                return txDuvidas;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxDuvidas", ref txDuvidas, value.Trim());
            }
        }

        /// <summary>
        /// Import da EstoriaPai
        /// </summary>
        [Association("EstoriaPaiAssociation", typeof(Estoria))]
        public Estoria EstoriaPai
        {
            get
            {
                return estoriaPai;
            }
            set
            {
                SetPropertyValue<Estoria>("EstoriaPai", ref estoriaPai, value);
            }
        }


        /// <summary>
        /// Variável que guarda o valor da prioridade da estória
        /// </summary>
        [ImmediatePostData]
        public UInt16 NbPrioridade
        {
            get
            {
                return nbPrioridade;
            }
            set
            {
                try
                {
                    SetPropertyValue<UInt16>("NbPrioridade", ref nbPrioridade, value);
                }
                catch (InvalidOperationException e)
                {
                    // Erro da nova versao do framework ainda nao resolvido.
                    // Aberto chamado de suporte.
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Variável que guarda o valor do tamanho da estória
        /// </summary>
        [RuleRequiredField("Estoria_NbTamanho_Required", DefaultContexts.Save, "Selecione um tamanho para a estória")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public double NbTamanho
        {
            get
            {
                return nbTamanho;
            }
            set
            {
                value = Convert.ToDouble(value);
                SetPropertyValue<double>("NbTamanho", ref nbTamanho, value);
            }
        }

        /// <summary>
        /// Variável que armazena as premissas
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public String TxPremissas
        {
            get
            {
                return txPremissas;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxPremissas", ref txPremissas, value.Trim());
            }
        }

        /// <summary>
        /// Declaração da choice de situação
        /// </summary>
        [RuleRequiredField("Estoria_CsSituacao_Required", DefaultContexts.Save, "Informe uma situação")]
        public CsEstoriaDomain CsSituacao
        {
            get
            {
                return csSituacao;
            }
            set
            {
                    SetPropertyValue<CsEstoriaDomain>("CsSituacao", ref csSituacao, value);
            }
        }

        /// <summary>
        /// Associação com EstoriaCasoTeste
        /// </summary>
        [Association("EstoriaCasosTeste", typeof(EstoriaCasoTeste)), Aggregated]
        public XPCollection CasosTeste
        {
            get
            {
                return GetCollection("CasosTeste");
            }
        }

        /// <summary>
        /// Associação com estória filho
        /// </summary>
        [Association("EstoriaPaiAssociation"), Aggregated, Browsable(false)]
        public XPCollection<Estoria> EstoriaFilho
        {
            get
            {
                return GetCollection<Estoria>("EstoriaFilho");
            }
        }

        /// <summary>
        /// Associação de Estória com CicloDesenvEstoria
        /// </summary>
        [Association("EstoriaCicloDesenv", typeof(CicloDesenvEstoria)), Aggregated, Browsable(false)]
        public XPCollection<CicloDesenvEstoria> CicloDesenvEstoria
        {
            get
            {
                return GetCollection<CicloDesenvEstoria>("CicloDesenvEstoria");
            }
        }

        [Custom("Caption", "Em Análise?")]
        public bool CsEmAnalise
        {
            get
            {
                return csEmAnalise;
            }
            set
            {
                SetPropertyValue<bool>("CsEmAnalise", ref csEmAnalise, value);
            }
        }

        #endregion

        #region NonPersistentProperties
        /// <summary>
        /// Variável que transforma o valor de tamanho de string para inteiro
        /// </summary>
        [NonPersistent]
        public string _NbTamanho
        {
            get
            {
                return NbTamanho.ToString();
            }
            set
            {
                NbTamanho = Convert.ToDouble(value);
            }
        }
        
        /// <summary>
        /// Variável que transforma o valor de tamanho de string para inteiro
        /// </summary>
        [NonPersistent, Custom("Caption", "Módulo")]
        public string _TamanhoProjeto
        {
            get
            {
                if(Modulo != null)
                    return String.Format("{0}; Tamanho = {1}", Modulo._TxNomeCompleto, GetSomaPontosProjeto());

                return string.Empty;
            }
        }

        /// <summary>
        /// Variável que formata o valor das prioridades
        /// </summary>
        [NonPersistent]
        public String _TxQuando
        {
            get
            {
                if (Ciclo == null || NbPrioridade > 0)
                    return String.Format("P{0}", NbPrioridade);
                else
                    return Ciclo._TxCiclo;
            }

            set
            {
                if (String.Compare(value.Substring(0, 1).ToUpper(), "P", false) == 0)
                {
                    string formataString = value.Substring(1);
                    NbPrioridade = Convert.ToUInt16(formataString);
                }

            }
        }

        /// <summary>
        /// Campo que concatena o Id da estória com o titulo
        /// </summary>
        [NonPersistent]
        public String _TxIdEstoria_Titulo
        {
            get
            {
                if (EstoriaFilho == null)
                    return null;
                else
                    return String.Format("{0} - {1}", TxID, TxTitulo);

            }
        }

        /// <summary>
        /// Armazena a informacao antes da edicao.
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public CsEstoriaDomain _CsSituacaoOld { get; set; }

        /// <summary>
        /// Armazena a informacao antes da edicao.
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public ushort _NbPrioridadeOld { get; set; }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Ativa a função RnCriarEstoriaID ao clicar no botão save
        /// </summary>
        protected override void OnSaving()
        {
            if (Oid.Equals(new Guid()) || estoriaPaiOld != EstoriaPai ||
            (moduloOld != null && moduloOld != Modulo))
                RnCriarEstoriaID();

            RnSetarPrioridadeZeroComFilhos();

            if (NbPrioridade > 0 && _NbPrioridadeOld > 0)
                OrdenacaoUtil.RnAplicarOrdenacao(this);
            else if (_NbPrioridadeOld == 0 && NbPrioridade >= 1 &&
                     Oid != new Guid() && CsSituacao != CsEstoriaDomain.Pronto)
                // Utilizado quando a entrega é removida(replanejada) de um ciclo
                // e retorna para o backlog. Ou seja, ela desce da prioridade 0 para 1.
                OrdenacaoUtil.RnRepriorizar(this, CsOrdenacaoDomain.DescerOrdem);

            if (estoriaPai != null)
                GetSomaEstoriasFilhas();

            // Chamada do método para recálculo dos pontos do ciclo
            if (Ciclo != null && !Oid.Equals(new Guid()))
            {
                Ciclo.RnCalcularPontosPlanejadosERealizados();
                Ciclo.Save();
            }

            base.OnSaving();
        }

        /// <summary>
        /// Metodo OnSaved
        /// </summary>
        protected override void OnSaved()
        {
            base.OnSaved();

            Modulo.RnCalcularPontosSituacao();
            Modulo.Save();

            if (moduloOld != null && Modulo != moduloOld)
            {
                moduloOld.RnCalcularPontosSituacao();
                moduloOld.Save();
            }

            _NbPrioridadeOld = NbPrioridade;
            _CsSituacaoOld = CsSituacao;
        }
        /// <summary>
        /// sobrescrevendo ondeleting
        /// </summary>
        protected override void OnDeleting()
        {
            if (estoriaPai != null)
                GetGetSomaEstoriasFilhasDeletar();
            base.OnDeleting();
        }
        /// <summary>
        /// Ativas as RNs ao se clicar no botão de delete
        /// </summary>
        protected override void OnDeleted()
        {
            Modulo.RnCalcularPontosSituacao();
            if (moduloOld != null)
                moduloOld.RnCalcularPontosSituacao();

            OrdenacaoUtil.RnDeletarOrdenacao(this);

            base.OnDeleted();
        }

        /// <summary>
        /// Função que cria os IDs das Estórias
        /// </summary>
        private void RnCriarEstoriaID()
        {
            if (IsDeleted)
                return;

            int crt, cont = 0;
            String id;

            if (EstoriaPai == null)
                crt = GetEstoriaPorModulo(Session, Modulo).Count;
            else
                crt = GetEstoriaFilhos(Session, Modulo, EstoriaPai).Count;
            do
            {
                crt += 1;
                if (EstoriaPai == null)
                {
                    if (crt < 10)
                        id = "PBI_" + String.Format("{0}.{1}{2}", Modulo.TxID, cont, crt);
                    else
                        id = "PBI_" + String.Format("{0}.{1}", Modulo.TxID, crt);
                }
                else
                    if (crt < 10)
                        id = String.Format("{0}.{1}{2}", EstoriaPai.TxID, cont, crt);
                    else
                        id = String.Format("{0}.{1}", EstoriaPai.TxID, crt);
            }
            while
            (GetEstoriaPorID(Session, Modulo, id).Count != 0);

            TxID = id;

        }

        /// <summary>
        /// Função que não permite excluir uma estória se ela tiver associações
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirDeletarEstoriaSeTiverAssociacoes", DefaultContexts.Delete, "A estória possui associação(s)!")]
        [NonPersistent, Browsable(false)]
        public bool RnDeletarEstoriasSemAssociacao
        {
            get
            {
                if (EstoriaFilho != null)
                    return !(EstoriaFilho.Count > 0);
                else
                    return true;
            }
        }
        /// <summary>
        /// Função que não permite excluir uma estória se ela tiver associações
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirModificarTamanhoDeEstoriaPai", DefaultContexts.Save, "A estória possui associação(s) e seu tamanho é calculado baseado nisso.")]
        [NonPersistent, Browsable(false)]
        public bool RnNaoPermitirModificarTamanhoDeEstoriaPai
        {
            get
            {
                if (EstoriaFilho.Count != 0)
                    return true;
                else
                    return !(EstoriaFilho.Count > 0);
            }
        }
        /// <summary>
        /// Valida a exclusão se uma estória não estiver associada a um caso de teste 
        /// </summary>
        [RuleFromBoolProperty("EstoriaCasoTesteAssociation", DefaultContexts.Delete, InvertResult = true,
        CustomMessageTemplate = "A Estoria está associado a um Caso de Teste e não pode ser excluído.")]
        [NonPersistent, VisibleInDetailView(false), VisibleInListView(false)]
        public bool RnValidarAssociacaoEstoriaCasoTeste
        {
            get
            {
                return CasosTeste.Count > 0;
            }
        }

        /// <summary>
        /// Regra de negócio para validar as prioridade. Só podendo inserir números maiores ou iguais a 0
        /// </summary>
        [RuleFromBoolProperty("ValidarPrioridadeMaiorQue0", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "A prioridade deve ser maior que 0")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarPrioridade
        {
            get
            {
                return NbPrioridade >= 0;
            }
        }
        /// <summary>
        /// Metodo para selecionar o projeto
        /// </summary>
        /// <param name="projeto">Projeto selecionado</param>
        public void RnSelecionarProjeto(Projeto projeto)
        {
            projetoSelecionado = projeto;
            if (projeto != null)
                OrdenacaoUtil.RnCriarOrdem(this);
        }

        /// <summary>
        /// Método para setar valor de prioridade 0 quando a estória tiver filhos.
        /// </summary>
        public void RnSetarPrioridadeZeroComFilhos()
        {
            if (estoriaPaiOld != null)
            {
                if (EstoriaPai != estoriaPaiOld)
                {
                    if (EstoriaFilho != null)
                    {
                        if (estoriaPaiOld.EstoriaFilho.Count == 0)
                        {
                            estoriaPaiOld.NbPrioridade = NbPrioridade;
                            estoriaPaiOld.Save();
                            OrdenacaoUtil.RnAplicarOrdenacao(estoriaPaiOld);
                        }
                    }
                }
            }

            if (EstoriaPai != null)
                EstoriaPai.NbPrioridade = 0;

        }

        /// <summary>
        /// Preenchimento do valor do título com a concatenação do "Como um beneficiado" e "Gostaria de"
        /// </summary>
        private void RnPreencherValorTitulo()
        {
            if (IsLoading || IsSaving)
                return;

            TxTitulo = string.Empty;

            if (ComoUmBeneficiado != null)
                TxTitulo = String.Format("{0} {1}",
                    StrUtil.UpperCaseFirst(ComoUmBeneficiado.TxDescricao),
                    StrUtil.LowerCaseFirst(TxGostariaDe));
            else
                TxTitulo = StrUtil.LowerCaseFirst(TxGostariaDe);
        }

        /// <summary>
        /// Recalcular a situação da Estória (método chamado na exclusão no Ciclo)
        /// </summary>
        /// <param name="estoria">Objeto de CicloDesenvEstoria</param>
        public void RnRecalcularSituacaoEstoria(CicloDesenvEstoria estoria)
        {
            if (estoria.Estoria == null)
                return;

            if (estoria.Estoria.CicloDesenvEstoria.Count == 1)
            {
                estoria.Estoria.CsSituacao = CsEstoriaDomain.NaoIniciado;
                estoria.Estoria.Save();
            }
            else if (estoria.CsSituacao != CsSituacaoEstoriaCicloDomain.Replanejado)
            {
                estoria.Estoria.CsSituacao = CsEstoriaDomain.Replanejado;
                estoria.Estoria.Save();
            }
        }

        #endregion

        #region NewInstance

        /// <summary>
        /// Método usado para passar dados de uma Estória (que está em uma sessão) para outra (que está em outra
        /// sessão)
        /// </summary>
        /// <param name="session">Nova sessão</param>
        /// <param name="current">Estoria Corrente</param>
        /// <param name="estoria">Estoria Antiga</param>
        public static void GetDadosEstoriaCurrent(Session session, Estoria current, Estoria estoria)
        {
            if (estoria != null)
            {
                current.ComoUmBeneficiado = session.GetObjectByKey<Beneficiado>(estoria.ComoUmBeneficiado.Oid);
                current.ProjetoParteInteressada = session.GetObjectByKey<ProjetoParteInteressada>(estoria.ProjetoParteInteressada.Oid);
                current.Modulo = session.GetObjectByKey<Modulo>(estoria.Modulo.Oid);
                if (estoria.EstoriaPai != null)
                {
                    current.EstoriaPai = session.GetObjectByKey<Estoria>(estoria.EstoriaPai.Oid);
                }
            }
        }

        #endregion

        #region DBQueries (Gets)
        /// <summary>
        /// Retorna a maior prioridade de uma estória
        /// </summary>
        /// <returns>Número da maior prioridade</returns>
        UInt16 IOrdenacao.GetMaiorOrdem()
        {
            UInt16 total = 0;

            try
            {
                object obj = Session.Evaluate(typeof(Estoria), CriteriaOperator.Parse("MAX(NbPrioridade)"), CriteriaOperator.Parse("Modulo.Projeto.Oid = ? ",
                    Projeto.SelectedProject));

                if (obj != null)
                    total = (UInt16)obj;
            }
            catch
            {
            }


            return total;
        }

        /// <summary>
        /// Retorna as prioridades iniciais e finais de uma estória
        /// </summary>
        /// <param name="ordemInicial">ordemInicial</param>
        /// <param name="ordemFinal">ordemFinal</param>
        /// <returns>Prioridades de uma estória</returns>
        List<Object> IOrdenacao.GetItensPorOrdem(int ordemInicial, int ordemFinal)
        {
            XPCollection collection;
            SortProperty newSortProperty = new SortProperty("NbPrioridade", SortingDirection.Ascending);
            
            List<Object> retorno = new List<Object>();

            if (ordemFinal != -1)
                collection = new XPCollection(Session, typeof(Estoria), CriteriaOperator.Parse("NbPrioridade >= ? AND NbPrioridade <= ? AND Modulo.Projeto.Oid = ?",
                ordemInicial, ordemFinal, modulo.Projeto.Oid), newSortProperty);
            else
                collection = new XPCollection(Session, typeof(Estoria), CriteriaOperator.Parse("NbPrioridade > ?  AND Modulo.Projeto.Oid = ?",
                ordemInicial, modulo.Projeto.Oid), newSortProperty);

            if (collection.Count > 0)
            {
                foreach (Object item in collection)
                    retorno.Add(item);
            }

            return retorno;
        }

        /// <summary>
        /// Retorna a menor prioridade de uma estória
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="projeto">Instancia do projeto para buscar menor prioridade</param>
        /// <returns>Número da menor prioridade</returns>
        public static ICollection GetEstoriasPorProjeto(Session session, Projeto projeto)
        {
            ICollection estorias = null;
            SortingCollection sortCollection = new SortingCollection();
            sortCollection.Add(new SortProperty("NbPrioridade", SortingDirection.Ascending));

            try
            {
                estorias = session.GetObjects(session.GetClassInfo<Estoria>(),
                    CriteriaOperator.Parse("Modulo.Projeto.Oid = ? And NbPrioridade > 0", projeto.Oid),
                    sortCollection, 0, false, false);
            }
            catch
            {
            }

            return estorias;
        }


        /// <summary>
        /// Retorna a menor prioridade de uma estória
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="projeto">Instancia do projeto para buscar menor prioridade</param>
        /// <returns>Número da menor prioridade</returns>
        public static ICollection GetEstoriasDoProjeto(Session session, Projeto projeto)
        {
            ICollection estorias = null;
            SortingCollection sortCollection = new SortingCollection();
            sortCollection.Add(new SortProperty("NbPrioridade", SortingDirection.Ascending));

            try
            {
                estorias = session.GetObjects(session.GetClassInfo<Estoria>(),
                    CriteriaOperator.Parse("Modulo.Projeto.Oid = ?", projeto.Oid),
                    sortCollection, 0, false, false);
            }
            catch
            {
            }

            return estorias;
        }

        /// <summary>
        /// Retorna a menor prioridade de uma estória
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="projeto">Instancia do projeto para buscar menor prioridade</param>
        /// <returns>Número da menor prioridade</returns>
        public static XPCollection<Estoria> GetEstoriasPorProjetoByOid(Session session, Guid projeto)
        {

            return new XPCollection<Estoria>(session, CriteriaOperator.Parse("Modulo.Projeto.Oid = ?", projeto));
        }

        /// <summary>
        /// Retornar todas as estorias do modulo.
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <returns>Número da menor prioridade</returns>
        public static ICollection GetEstoriasPorModulo(Modulo modulo)
        {
            ICollection estorias = null;
            SortingCollection sortCollection = new SortingCollection();
            sortCollection.Add(new SortProperty("NbPrioridade", SortingDirection.Ascending));

            try
            {
                estorias = modulo.Session.GetObjects(modulo.Session.GetClassInfo<Estoria>(),
                    CriteriaOperator.Parse("Modulo.Oid = ? And EstoriaFilho.Count == 0", modulo.Oid),
                    sortCollection, 0, false, false);
            }
            catch
            {
            }

            return estorias;
        }


        /// <summary>
        /// Busca a sessão e o id do modulo no BD
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">modulo</param>
        /// <returns>return</returns>
        private static XPCollection GetEstoriaPorModulo(Session session, Modulo modulo)
        {
            return new XPCollection(session, typeof(Estoria),
            CriteriaOperator.Parse(String.Format("EstoriaPai is null AND Modulo.Oid = '{0}'", modulo.Oid)));
        }
        /// <summary>
        /// Busca a sessão e o TxID no BD
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">modulo</param>
        /// <param name="txID">txID</param>
        /// <returns>TxID</returns>
        private static ICollection GetEstoriaPorID(Session session, Modulo modulo, String txID)
        {
            if (modulo != null)
            {
                return modulo.Session.GetObjects(modulo.Session.GetClassInfo<Estoria>(),
                CriteriaOperator.Parse(String.Format("Modulo.Oid = '{0}' AND TxID = '{1}'", modulo.Oid, txID)), null, 0, false, true);
            }
            else
                return null;
        }

        /// <summary>
        /// Busca a sessão, id do modulo e id da estoria no BD
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="modulo">Modulo</param>
        /// <param name="estoriaPai">EstoriaPai</param>
        /// <returns>return</returns>
        private static XPCollection GetEstoriaFilhos(Session session, Modulo modulo, Estoria estoriaPai)
        {
            return new XPCollection(session, typeof(Estoria),
            CriteriaOperator.Parse(String.Format("Modulo.Oid = '{0}' AND EstoriaPai = '{1}' ", modulo.Oid, estoriaPai.Oid)));
        }

        /// <summary>
        /// Retorna o somatorio de pontos das estorias que estao atualmente em analise.
        /// </summary>
        /// <param name="modulo">modulo</param>
        /// <returns>Somatório de NbTamanho</returns>
        public static double GetSomaPontosEmAnalise(Modulo modulo)
        {
            double soma = 0;
            ICollection estorias = null;

            try
            {
                estorias = modulo.Session.GetObjects(
                    modulo.Session.GetClassInfo<Estoria>(),
                    CriteriaOperator.Parse("Modulo.Oid = ? And CsSituacao != ? And CsSituacao != ? And EstoriaFilho.Count == 0",
                        modulo.Projeto.Oid, CsEstoriaDomain.EmDesenv, CsEstoriaDomain.Pronto), null, 0, false, false);

                foreach (Estoria estoria in estorias)
                    soma += estoria.NbTamanho;
            }
            catch
            {
            }

            return soma;
        }

        /// <summary>
        /// Collection que retorna o somatório dos pontos das estórias em desenvolvimento
        /// </summary>
        /// <param name="modulo">modulo</param>
        /// <returns>Retorna o somatório dos pontos das estórias em desenvolvimento</returns>
        public static double GetSomaPontosEmDesenv(Modulo modulo)
        {
            double soma = 0;

            try
            {
                object obj = modulo.Session.Evaluate(typeof(Estoria),
                    CriteriaOperator.Parse("Sum(NbTamanho)"), 
                    CriteriaOperator.Parse("Modulo.Oid = ? And CsSituacao = ? And EstoriaFilho.Count == 0", 
                        modulo.Oid, CsEstoriaDomain.EmDesenv));

                if (obj != null)
                    soma = Convert.ToDouble(obj);
            }
            catch
            {
            }

            return soma;
        }

        /// <summary>
        /// Collection que retorna o somatório dos pontos das estórias prontas
        /// </summary>
        /// <param name="modulo">modulo</param>
        /// <returns>Retorna o somatório dos pontos das estórias prontas</returns>
        public static double GetSomaPontosProntos(Modulo modulo)
        {
            double soma = 0;

            try
            {
                object obj = modulo.Session.Evaluate(typeof(Estoria),
                    CriteriaOperator.Parse("Sum(NbTamanho)"),
                    CriteriaOperator.Parse("Modulo.Oid = ? And CsSituacao = ? And EstoriaFilho.Count == 0", 
                    modulo.Oid, CsEstoriaDomain.Pronto));

                if (obj != null)
                    soma = Convert.ToDouble(obj);
            }
            catch
            {
            }

            return soma;
        }
        /// <summary>
        /// pega soma dos filhos
        /// </summary>
        public void GetGetSomaEstoriasFilhasDeletar()
        {
            estoriaPai.NbTamanho -= NbTamanho;

            if (estoriaPai.NbTamanho == 0)
                estoriaPai.NbTamanho = NbTamanho;
        }
        /// <summary>
        /// Método que captura a soma das estorias filhas da estoria
        /// </summary>
        public void GetSomaEstoriasFilhas()
        {
            this.EstoriaPai.NbTamanho = 0;
            foreach (Estoria estoria in EstoriaPai.EstoriaFilho)
                estoriaPai.NbTamanho += estoria.NbTamanho;
        }
        /// <summary>
        /// Pega a soma de pontos do projeto
        /// </summary>
        /// <returns>retorna o tamanho total do projeto</returns>
        public double GetSomaPontosProjeto()
        {
            double totalTamanhoProjeto = 0;

            try
            {
                object obj = ProjetoParteInteressada.Projeto.Session.Evaluate(typeof(Estoria),
                CriteriaOperator.Parse("Sum(NbTamanho)"), CriteriaOperator.Parse("Modulo.Oid = ? AND EstoriaPai is null", this.Modulo.Oid));

                if (obj != null)
                    totalTamanhoProjeto = Convert.ToDouble(obj);
            }
            catch
            {
            }

            return totalTamanhoProjeto;
        }
        /// <summary>
        /// Soma de tamanho total das Estórias
        /// </summary>
        /// <param name="modulo">modulo</param>
        /// <returns>Retorna o total de tamanho de estorias</returns>
        public static double GetTotalPontosEstoria(Modulo modulo)
        {
            double totalEstoria = 0;

            try
            {
                object obj = modulo.Session.Evaluate(typeof(Estoria),
                CriteriaOperator.Parse("Sum(NbTamanho)"), CriteriaOperator.Parse("Modulo.Oid = ? And EstoriaPai is null", modulo.Oid));

                if (obj != null)
                    totalEstoria = Convert.ToDouble(obj);
            }
            catch
            {
            }

            return totalEstoria;
        }

        /// <summary>
        /// Soma das estorias prontas
        /// </summary>
        /// <param name="session">Seção</param>
        /// <param name="projeto">objeto projeto</param>
        /// <returns>total</returns>
        public static double GetSomaDasEstoriasProntas(Session session, Projeto projeto)
        {
            double total = 0;

            try
            {
                object obj = session.Evaluate(typeof(Estoria), CriteriaOperator.Parse("Sum(NbTamanho)"), 
                    CriteriaOperator.Parse("Modulo.Projeto.Oid = ? And CsSituacao = 'Pronto' And EstoriaPai is null", 
                    projeto.Oid));

                if (obj != null)
                    total = (double)obj;
            }
            catch
            {
            }
            return total;
        }
        #endregion

        #region Utils
        /// <summary>
        /// Metodo da interface que retorna o atributo salvandoPrioridades
        /// </summary>
        /// <returns>salvandoPrioridades</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return salvandoPrioridades;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para salvandoPrioridades
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetReOrdenando(bool value)
        {
            salvandoPrioridades = value;
        }

        /// <summary>
        /// Metodo da Interface que retorna o valor antigo da prioridade
        /// </summary>
        /// <returns>prioridadeOld</returns>
        ushort IOrdenacao.GetOrdemOld()
        {
            return _NbPrioridadeOld;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para prioridadeOld
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetOrdemOld(ushort value)
        {
            _NbPrioridadeOld = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor de NbPrioridade
        /// </summary>
        /// <returns>NbPrioridade</returns>
        ushort IOrdenacao.GetNbOrdem()
        {
            return NbPrioridade;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para NbPrioridade
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetNbOrdem(ushort value)
        {
            NbPrioridade = value;
        }

        /// <summary>
        /// Metodo da interface que retorna Oid
        /// </summary>
        /// <returns>Oid</returns>
        Guid IOrdenacao.GetOid()
        {
            return Oid;
        }

        /// <summary>
        /// Metodo da interface que retorna o metodo Save
        /// </summary>
        void IOrdenacao.Save()
        {
            Save();
        }

        /// <summary>
        /// Metodo da interface que retorna o metodo IsDeleted
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IOrdenacao.IsDeleted()
        {
            return IsDeleted;

        }
        #endregion

        #region Override
        #endregion

        #region UserInterface
        /// <summary>
        /// Interface que esconde o campo TxID se o usuário estiver cadastrando uma nova Estória
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>Hidden TxID</returns>
        [EditorStateRule("HiddenEstoriaTxID", "TxID", ViewType.DetailView)]
        public EditorState HiddenTxID(out bool active)
        {
            active = Oid.Equals(new Guid());
            return EditorState.Hidden;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Método que substitui o valor da estória pai antiga pelo valor novo
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            estoriaPaiOld = EstoriaPai;
            
            if (moduloOld == null)
                moduloOld = Modulo;

            _NbPrioridadeOld = NbPrioridade;
            _CsSituacaoOld = CsSituacao;
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public Estoria(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            salvandoPrioridades = false;
            if (Projeto.SelectedProject != new Guid())
                OrdenacaoUtil.RnCriarOrdem(this);
        }
        #endregion
    }
}