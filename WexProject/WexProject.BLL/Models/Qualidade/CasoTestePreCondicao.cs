using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using WexProject.Library.Libs.Ordenacao;
using System.Collections.Generic;
using DevExpress.Persistent.Validation;


namespace WexProject.BLL.Models.Qualidade
{
    /// <summary>
    /// Classe para as Pré-Condições
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("TxDescricao")]
    [DeferredDeletion(false)]
    [Custom("Caption", "Pré-condição")]
    [OptimisticLocking( false )]
    public class CasoTestePreCondicao : Note, IOrdenacao
    {
        #region Attributes
        /// <summary>
        /// Atributo de CasoTeste
        /// </summary>
        private CasoTeste casoTeste;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txDescricao;

        /// <summary>
        /// Atributo para armazenar o valor antigo da sequência
        /// </summary>
        private UInt16 nbSequenciaOld;

        /// <summary>
        /// Atributo para salvar se a sequência ja foi alterada ou não (True ou False)
        /// </summary>
        private bool csReOrdenando;

        /// <summary>
        /// Atributo de NbMaiorInformacaoAdicional
        /// </summary>
        private UInt16 nbMaiorInformacaoAdicional;

        /// <summary>
        /// Atributo de NbSequencia
        /// </summary>
        private UInt16 nbSequencia;

        /// <summary>
        /// Atributo para verificar se esta sendo criado um novo objeto ou uma edição
        /// </summary>
        private bool csCriandoUmNovo;
        #endregion

        #region Properties
        /// <summary>
        /// Associação com CasoTeste
        /// </summary>
        [Association("CasoTestePreCondicoes", typeof(CasoTeste))]
        public CasoTeste CasoTeste
        {
            get
            {
                return casoTeste;
            }
            set
            {
                bool alterado = casoTeste != value;
                if (value != null)
                {
                    SetPropertyValue<CasoTeste>("CasoTeste", ref casoTeste, value);
                    if (alterado && !IsLoading && !IsDeleted)
                    {
                        csReOrdenando = false;
                        csCriandoUmNovo = true;
                        OrdenacaoUtil.RnCriarOrdem(this);
                        csCriandoUmNovo = false;
                    }
                }
            }
        }

        /// <summary>
        /// Variável que guarda o valor da sequência
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
                    OrdenacaoUtil.RnAplicarOrdenacao(this);
            }
        }

        /// <summary>
        /// Variável que guarda a descrição de uma pré-condição
        /// </summary>
        [RuleRequiredField("CasoTestePreCondicao_TxDescricao_Required", DefaultContexts.Save, Name="Descrição")]
        [Size(4000)]
        public String TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value);
            }
        }

        /// <summary>
        /// Variável que guarda a Maior informação adicional
        /// </summary>
        public UInt16 NbMaiorInformacaoAdicional
        {
            get
            {
                return nbMaiorInformacaoAdicional;
            }
            set
            {
                SetPropertyValue<UInt16>("NbMaiorInformacaoAdicional", ref nbMaiorInformacaoAdicional, value);
            }
        }

        /// <summary>
        /// Assiciação com CasoTestePreCondicaInformacaoAdicional
        /// </summary>
        [Association("CasoTestePreCondicaoInformacaoAdicional", typeof(CasoTestePreCondicaoInformacaoAdicional)), Aggregated]
        public XPCollection PreCondicoesInformacoesAdicionais
        {
            get
            {
                return GetCollection("PreCondicoesInformacoesAdicionais");
            }
        }

        /// <summary>
        /// Assiciação com CasoTestePreCondicaoAnexo
        /// </summary>
        [Association("CasoTestePreCondicaoAnexo", typeof(CasoTestePreCondicaoAnexo)), Aggregated]
        public XPCollection CasoTestePreCondicaoAnexos
        {
            get
            {
                return GetCollection("CasoTestePreCondicaoAnexos");
            }
        }
        #endregion

        #region NonPersistentProperties
        /// <summary>
        /// Proprieade não persistente que retorna a quantidade de informações adicionais uma pré-condição tem
        /// </summary>
        [NonPersistent]
        public UInt16 _NbInformacoesAdicionais
        {
            get
            {
                return (UInt16)PreCondicoesInformacoesAdicionais.Count;
            }

        }

        /// <summary>
        /// Pripridade não persistente que retorna a quantidade de anexos que uma pré-condição tem
        /// </summary>
        [NonPersistent]
        public UInt16 _NbAnexos
        {
            get
            {
                return (UInt16)CasoTestePreCondicaoAnexos.Count;
            }
        }
        #endregion

        #region BusinessRules
        /// <summary>
        /// OnSaving
        /// </summary>
        protected override void OnSaving()
        {
            base.OnSaving();
        }

        /// <summary>
        /// Metodo OnDeleted
        /// </summary>
        protected override void OnDeleted()
        {
            base.OnDeleted();
            OrdenacaoUtil.RnDeletarOrdenacao(this);
            CasoTeste.NbMaiorPrecondicao = (UInt16)CasoTeste.PreCondicoes.Count;
        }

        /// <summary>
        /// Regra de negócia que não permite que o sistema salva uma pr-condição se não estiver com o requisito selecionado
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePreCondicao", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Selecione um requisito")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarRequisito
        {
            get
            {
                return CasoTeste.Requisito != null;
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
            if (CasoTeste == null)
            {
                return 0;
            }
            else
            {
                return CasoTeste.NbMaiorPrecondicao;
            }
        }

        /// <summary>
        /// Collection para retornar os itens por ordem
        /// </summary>
        /// <param name="ordemInicial">ordemInicial</param>
        /// <param name="ordemFinal">ordemFinal</param>
        /// <returns>Coleção com os itens</returns>
        List<Object> IOrdenacao.GetItensPorOrdem(int ordemInicial, int ordemFinal )
        {
            List<Object> collection = new List<Object>();
            if (ordemFinal != -1)
            {

                foreach (CasoTestePreCondicao cont in CasoTeste.PreCondicoes)
                {
                    if ((cont != this) && (cont.NbSequencia >= ordemInicial && cont.NbSequencia <= ordemFinal))
                    {
                        collection.Add(cont);
                    }
                }
            }
            else
            {
                foreach (CasoTestePreCondicao cont in CasoTeste.PreCondicoes)
                {
                    if ((cont != this) && (cont.NbSequencia > ordemInicial))
                    {
                        collection.Add(cont);
                    }
                }
            }

            collection.Sort((r1, r2) =>
            {
                return ((CasoTestePreCondicao)r1).NbSequencia.CompareTo(((CasoTestePreCondicao)r2).NbSequencia);
            });

            return collection;
        }

        #endregion

        #region Utils
        /// <summary>
        /// Metodo da interface que retorna o valor antigo da sequência
        /// </summary>
        /// <returns>nbSequenciaOld</returns>
        ushort IOrdenacao.GetOrdemOld()
        {
            return nbSequenciaOld;
        }

        /// <summary>
        /// Metodo da interface que atribui o valor antigo da sequência para a variável
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetOrdemOld(ushort value)
        {
            nbSequenciaOld = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor da sequência
        /// </summary>
        /// <returns>NbSequencia</returns>
        ushort IOrdenacao.GetNbOrdem()
        {
            return NbSequencia;
        }

        /// <summary>
        /// Metodo da interface que verifica se o valor da sequência é maior que a variavel CasoTeste.NbMaiorPrecondicao
        /// Se for, o metodo atribui o valor da sequência para CasoTeste.NbMaiorPrecondicao
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetNbOrdem(ushort value)
        {
            if (value > CasoTeste.NbMaiorPrecondicao)
                CasoTeste.NbMaiorPrecondicao = value;

            NbSequencia = value;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Atribui o valor de sequencia antigo para uma variavel
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();
            nbSequenciaOld = NbSequencia;
        }
        /// <summary>
        /// Inicialização da classe PreCondicoes
        /// </summary>
        /// <param name="session">session</param>
        public CasoTestePreCondicao(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            NbMaiorInformacaoAdicional = 0;
            //csSalvandoSequencia = false;
        }
        #endregion
        /// <summary>
        /// Metodo da interface que retorna o valor de csSalvandoSequencia
        /// </summary>
        /// <returns>csSalvandoSequencia</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return csReOrdenando;
        }

        /// <summary>
        /// Metodo da interface que seta um valor para csSalvandoSequencia
        /// </summary>
        /// <param name="value">valuw</param>
        void IOrdenacao.SetReOrdenando(bool value)
        {
            csReOrdenando = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o Oid do Objeto
        /// </summary>
        /// <returns>Oid</returns>
        Guid IOrdenacao.GetOid()
        {
            return Oid;
        }

        /// <summary>
        /// Metodo da interface que retorna se o Objeto está deletado
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IOrdenacao.IsDeleted()
        {
            return IsDeleted;
        }
    }

}
