using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Windows.Forms;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using WexProject.Library.Libs.Ordenacao;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.BLL.Shared.Domains.Escopo;

namespace WexProject.BLL.Models.Execucao
{
    /// <summary>
    /// Classe CicloDesenvEstoria
    /// </summary>
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("CicloDesenvEstoria_CicloDesenv_Unique", DefaultContexts.Save, "Estoria, Ciclo", Name = "JaExisteUmaEstoriaNoCiclo",
    CustomMessageTemplate = "Já existe uma estória com esse titulo no Ciclo")]
    [DeferredDeletion(false)]
    [OptimisticLocking( false )]
    public class CicloDesenvEstoria : BaseObject, IOrdenacao
    {
        #region Attributes

        /// <summary>
        /// Atributo de Estoria
        /// </summary>
        private Estoria estoria;

        /// <summary>
        /// Atributo de NbSequencia
        /// </summary>
        private UInt16 nbSequencia;

        /// <summary>
        /// Atributo de CsSituacao
        /// </summary>
        private CsSituacaoEstoriaCicloDomain csSituacao;

        /// <summary>
        /// Atributo de Ciclo
        /// </summary>
        private CicloDesenv ciclo;

        /// <summary>
        /// Atributo Meta
        /// </summary>
        private bool meta;

        /// <summary>
        /// Atributo que guarda o valor antido da Sequencia
        /// </summary>
        private UInt16 nbSequenciaOld;

        /// <summary>
        /// Atributo que guarda true ou false para verificar se está sendo criado um novo item de um ciclo
        /// </summary>
        private bool csCriandoUmNovo;

        /// <summary>
        /// Atributo que verifica se o objeto já foi ou não reordenado
        /// </summary>
        private bool csReOrdenando;

        /// <summary>
        /// Atributo que guarda uma estoria anterior
        /// </summary>
        private Estoria estoriaOld;

        /// <summary>
        /// Guarda o valor do objeto antigo
        /// </summary>
        private CicloDesenvEstoria cicloDesenvEstoriaOld;

        #endregion

        #region Properties

        /// <summary>
        /// propriedade de meta
        /// </summary>
        public bool Meta
        {
            get
            {
                return meta;
            }
            set
            {
                SetPropertyValue<bool>("Meta", ref meta, value);
            }
        }
        /// <summary>
        /// Import de Estoria
        /// </summary>
        [ImmediatePostData]
        [Association("EstoriaCicloDesenv", typeof(Estoria))]
        public Estoria Estoria
        {
            get
            {
                return estoria;
            }
            set
            {
                if (value != null)
                {
                    bool alterado = estoria != value;
                    SetPropertyValue<Estoria>("Estoria", ref estoria, value);

                    if (alterado && !IsLoading && !IsDeleted && (Estoria.NbTamanho > 13))
                        MessageBox.Show("Você não pode incluir estórias maiores que 13");

                    // Caso a estoria esteja sendo readicionada ao ciclo,
                    // remove da lista de adicionados para que a situacao 
                    // na mesma nao seja alterada para nao iniciado ao salvar.
                    if (!IsLoading && Ciclo._DeletedItems.IndexOf(Estoria) != -1)
                        Ciclo._DeletedItems.Remove(Estoria);
                }
            }
        }
        /// <summary>
        /// Variável que guarda o valor da sequência do ciclo
        /// </summary>
        public UInt16 NbSequencia
        {
            get
            {
                return nbSequencia;
            }
            set
            {
                bool alterado = nbSequencia != value;

                SetPropertyValue<UInt16>("NbSequencia", ref nbSequencia, value);
                if (alterado && !IsLoading && !IsDeleted && !csCriandoUmNovo && !csReOrdenando)
                {
                    OrdenacaoUtil.RnAplicarOrdenacao(this);
                }
            }
        }

        /// <summary>
        /// Import de CsSituacaoEstoriaCicloDomain
        /// </summary>
        public CsSituacaoEstoriaCicloDomain CsSituacao
        {
            get
            {
                return csSituacao;
            }
            set
            {
                SetPropertyValue<CsSituacaoEstoriaCicloDomain>("CsSituacao", ref csSituacao, value);
                if (!IsLoading && Ciclo != null)
                    Ciclo.RnCalcularPontosPlanejadosERealizados();
            }
        }

        /// <summary>
        /// Associação com CicloDesenvEstoria
        /// </summary>
        [Association("CicloDesenv", typeof(CicloDesenv))]
        public CicloDesenv Ciclo
        {
            get
            {
                return ciclo;
            }
            set
            {
                bool alterado = ciclo != value;
                if (value != null)
                {
                    SetPropertyValue<CicloDesenv>("Ciclo", ref ciclo, value);
                    if (alterado && !IsLoading && !IsDeleted)
                    {
                        Ciclo.RnCalcularSituacaoCiclo();
                        csReOrdenando = false;
                        csCriandoUmNovo = true;
                        OrdenacaoUtil.RnCriarOrdem(this);
                        csCriandoUmNovo = false;
                    }
                }

            }
        }

        /// <summary>
        /// Guarda a estória anterior
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public Estoria EstoriaOld
        {
            get
            {
                return estoriaOld;
            }
            set
            {
                estoriaOld = value;
            }
        }
        #endregion

        #region NonPersistentProperties

        /// <summary>
        /// Classe para recuperar apenas objetos que satisfaçam a condição
        /// </summary>
        [NonPersistent]
        [Browsable(false)]
        public XPCollection<Estoria> _PrioridadeEstoria
        {
            get
            {
                XPCollection<Estoria> estorias = new XPCollection<Estoria>(Session, CriteriaOperator.Parse("NbPrioridade > 0 && CsSituacao <> 'Pronto'"));
                estorias.DeleteObjectOnRemove = false;
                foreach (CicloDesenvEstoria estoria in Ciclo.DesenvEstorias)
                    estorias.Remove(estoria.Estoria);
                return estorias;
            }
        }


        /// <summary>
        /// Propriedade que retorna a concetenação do nome da estória com o ID
        /// </summary>
        [NonPersistent]
        public String _TxNomeEstoria
        {
            get
            {
                string name = string.Empty;
                if (Estoria != null)
                {
                    name = Estoria._TxIdEstoria_Titulo;
            }
                return name;
        }
        }

        /// <summary>
        /// Permite a edição do tamanho da estoria diretamente
        /// no grid dos itens do ciclo.
        /// </summary>
        [ImmediatePostData]
        [NonPersistent]
        public String _NbPontos
        {
            get
            {
                if (!IsLoading && Estoria != null)
                    return Estoria.NbTamanho.ToString();
                else
                    return "0";
            }

            set
            {
                if (!IsLoading && Estoria != null)
                {
                    Estoria.NbTamanho = Convert.ToDouble(value);
                    Ciclo.RnCalcularPontosPlanejadosERealizados();
                }
            }
        }

        /// <summary>
        /// variavel que concatena o valor do tamoanho da estoria com não permitido se for maior que 13
        /// </summary>
        [Browsable(false)]
        [NonPersistent]
        public string _TamanhoEstoria
        {
            get
            {
                if (Estoria.NbTamanho > 13)
                    return Estoria.NbTamanho + " (não permitido)";
                else
                    return Estoria.NbTamanho + "";
            }
        }

        /// <summary>
        /// Propriedade que retorna a coleção das estórias com prioridade maior do que 0 e ordenadas em ordem crescente 
        /// </summary>
        [Browsable(false)]
        public XPCollection _Estoria
        {
            get
            {
                XPCollection collection = new XPCollection();
                collection = new XPCollection(Session, typeof(Estoria), CriteriaOperator.Parse("NbPrioridade > ? And NbTamanho <= 13 And Modulo.Projeto.Oid = ?", 0, Projeto.SelectedProject), new SortProperty("NbPrioridade", SortingDirection.Ascending));

                if (collection != null)
                    return collection;
                else
                    return null;

            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Quando estiver deletando o objeto
        /// </summary>
        protected override void OnDeleting()
        {
            Estoria.RnRecalcularSituacaoEstoria(this);

            base.OnDeleting();
        }

        /// <summary>
        /// Regra de negório para devolver a prioridade 1 ao 
        /// estória removida do ciclo.
        /// </summary>
        protected override void OnDeleted()
        {
            Ciclo._DeletedItems.Add(Estoria);
            Ciclo.RnCalcularPontosPlanejadosERealizados();

            base.OnDeleted();
        }

        /// <summary>
        /// Setando o Ciclo na Estória
        /// </summary>
        protected override void OnSaving()
        {
            if (Estoria != null && Estoria.Ciclo != Ciclo && Ciclo.CsSituacaoCiclo != CsSituacaoCicloDomain.Concluido)
            {
                Estoria.Ciclo = Ciclo;
                Estoria.Save();
            }

            base.OnSaving();
        }

        /// <summary>
        /// Regra de negócio que redefine as prioridades 
        /// dos itens do ciclo
        /// </summary>
        public void RnRedefinirPrioridade()
        {
            if (Estoria != null)
            {
                if (CsSituacao != CsSituacaoEstoriaCicloDomain.Replanejado)
                    Estoria.NbPrioridade = 0;
                else if (Estoria.NbPrioridade == 0)
                    Estoria.NbPrioridade = 1;
            }
        }

        /// <summary>
        /// Regra de negócio que redefine a situação das estórias associadas ao ciclo
        /// </summary>
        public void RnRedefinirSituacaoEstoria()
        {
            if (Estoria != null)
            {
                if (CsSituacao == CsSituacaoEstoriaCicloDomain.NaoIniciado)
                {
                    Estoria.CsSituacao = CsEstoriaDomain.NaoIniciado;
                }
                else if (CsSituacao == CsSituacaoEstoriaCicloDomain.EmDesenv)
                {
                        Estoria.CsSituacao = CsEstoriaDomain.EmDesenv;
                }
                else if (CsSituacao == CsSituacaoEstoriaCicloDomain.Pronto)
                {
                            Estoria.CsSituacao = CsEstoriaDomain.Pronto;
                }
                else if (CsSituacao == CsSituacaoEstoriaCicloDomain.Replanejado)
                {
                                Estoria.CsSituacao = CsEstoriaDomain.Replanejado;
            }
        }
        }

        /// <summary>
        /// Regra que reprioriza estorias sem ciclo
        /// </summary>
        public void RnRepriorizarEstoriasSemCiclo()
        {
            EstoriaOld.CsSituacao = CsEstoriaDomain.NaoIniciado;
            EstoriaOld.NbPrioridade = 1;
            EstoriaOld.Save();
        }

        /// <summary>
        /// Regra de negócio que calcula a prioridade das estórias que são substituidas nos itens do ciclo 
        /// </summary>
        public void RnCalcularPrioridadeNaEdicao()
        {
            EstoriaOld.Ciclo = null;
            EstoriaOld.NbPrioridade = 1;
            EstoriaOld.Save();
        }

        /// <summary>
        /// Validação de mudança de situação da Estória
        /// </summary>
        [RuleFromBoolProperty("ValidarMudancaSituacao", DefaultContexts.Save, InvertResult = false,
            CustomMessageTemplate = "A situação da Estória está como Pronta em outro Ciclo, logo não pode ser alterada")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarMudancaSituacao
        {
            get
            {
                if (cicloDesenvEstoriaOld != null && !Oid.Equals(new Guid()) && cicloDesenvEstoriaOld.CsSituacao != CsSituacao)
                {
                    foreach (CicloDesenvEstoria estoria in Estoria.CicloDesenvEstoria)
                    {
                        if (estoria.CsSituacao == CsSituacaoEstoriaCicloDomain.Pronto && estoria != this)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Regra de negócio que calcula a prioridade das estórias que são substituidas nos itens do ciclo 
        /// </summary>
        [RuleFromBoolProperty("ValidarEstoriaExistente", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Você não pode salvar sem estória associada")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarEstoriaExistente
        {
            get
            {
                return Estoria != null;
            }
        }

        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)

        /// <summary>
        /// Get para retornar a maior ordem da pré-condições
        /// </summary>
        /// <returns>Retorna o maior valor</returns>
        UInt16 IOrdenacao.GetMaiorOrdem()
        {

            if (Ciclo == null)
                return 0;
            else
                return Ciclo.NbMaiorCicloDesenvEstoria;
        }

        /// <summary>
        /// Collection para retornar os itens por ordem
        /// </summary>
        /// <param name="ordemInicial">ordemInicial</param>
        /// <param name="ordemFinal">ordemFinal</param>
        /// <returns>Coleção com os itens</returns>
        List<Object> IOrdenacao.GetItensPorOrdem(int ordemInicial, int ordemFinal)
        {
            List<Object> collection = new List<Object>();

            if (ordemFinal != -1)
            {
                foreach (CicloDesenvEstoria cont in Ciclo.DesenvEstorias)
                {
                    if ((cont != this) && (cont.NbSequencia >= ordemInicial && cont.NbSequencia <= ordemFinal))
                        collection.Add(cont);
                }
            }
            else
            {
                foreach (CicloDesenvEstoria cont in Ciclo.DesenvEstorias)
                {
                    if ((cont != this) && (cont.NbSequencia > ordemInicial))
                        collection.Add(cont);
                }
            }

            collection.Sort((r1, r2) =>
            {
                return ((CicloDesenvEstoria)r1).NbSequencia.CompareTo(((CicloDesenvEstoria)r2).NbSequencia);
            });

            return collection;
        }

        #endregion

        #region Utils
        /// <summary>
        /// Metodo da interface que retorna o valor true of false para csReordenando
        /// </summary>
        /// <returns>True or False</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return csReOrdenando;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para csReordenando
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetReOrdenando(bool value)
        {
            csReOrdenando = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor de nbSequenciaOld
        /// </summary>
        /// <returns>nbSequenciaOld</returns>
        ushort IOrdenacao.GetOrdemOld()
        {
            return nbSequenciaOld;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para nbSequenciaOld
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetOrdemOld(ushort value)
        {
            nbSequenciaOld = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor da sequencia
        /// </summary>
        /// <returns>NbSequencia</returns>
        ushort IOrdenacao.GetNbOrdem()
        {
            return NbSequencia;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para NbSequencia
        /// </summary>
        /// <param name="value">vlaue</param>
        void IOrdenacao.SetNbOrdem(ushort value)
        {
            if (value > Ciclo.NbMaiorCicloDesenvEstoria)
                Ciclo.NbMaiorCicloDesenvEstoria = value;

            NbSequencia = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o Oid do objeto
        /// </summary>
        /// <returns>Oid</returns>
        Guid IOrdenacao.GetOid()
        {
            return Oid;
        }

        /// <summary>
        /// Metodo da interface que retorna se o objeto ja foi deletado
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IOrdenacao.IsDeleted()
        {
            return IsDeleted;
        }

        /// <summary>
        /// Ao transformar para string
        /// </summary>
        /// <returns>Nome da Estória</returns>
        public override string ToString()
        {
            return _TxNomeEstoria;
        }
        #endregion

        #region User Interface
        #endregion

        #region Constructors

        /// <summary>
        /// Metodo que atribue uma estoria como sendo estoriaOld
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (EstoriaOld == null)
                EstoriaOld = Estoria;

            if (cicloDesenvEstoriaOld == null)
                cicloDesenvEstoriaOld = (CicloDesenvEstoria)MemberwiseClone();
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public CicloDesenvEstoria(Session session)
            : base(session)
        {

        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            nbSequenciaOld = NbSequencia;
        }

        #endregion
    }
}