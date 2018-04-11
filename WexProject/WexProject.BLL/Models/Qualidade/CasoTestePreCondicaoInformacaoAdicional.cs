using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using WexProject.Library.Libs.Ordenacao;

namespace WexProject.BLL.Models.Qualidade
{
    /// <summary>
    /// Classe CasoTestePreCondicaoInformacaoAdiconal
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [Custom("Caption", "Projetos > Qualidade > Casos de Teste > Pré-condição > Informações Adicionais")]
    [OptimisticLocking( false )]
    public class CasoTestePreCondicaoInformacaoAdicional : Note, IOrdenacao
    {

        #region Attributes
        /// <summary>
        /// Atributo de CasoTestePreCondicao
        /// </summary>
        private CasoTestePreCondicao casoTestePreCondicao;

        /// <summary>
        /// Variavel de controle (True or False)
        /// </summary>
        private bool csReOrdenando;

        /// <summary>
        /// Atributo que guarda o valor antigo de NbSequencia
        /// </summary>
        private UInt16 nbSequenciaOld;

        /// <summary>
        /// Atributo de NbSequencia
        /// </summary>
        private UInt16 nbSequencia;

        /// <summary>
        /// Atributo para verificar se esta sendo criado um novo objeto ou uma edição
        /// </summary>
        private bool csSalvandoNovo;
        #endregion

        #region Properties
        /// <summary>
        /// Variável que importa CasoTestePreCondicao
        /// </summary>
        [Association("CasoTestePreCondicaoInformacaoAdicional", typeof(CasoTestePreCondicao))]
        public CasoTestePreCondicao CasoTestePreCondicao
        {
            get
            {
                return casoTestePreCondicao;
            }
            set
            {
                bool alterado = casoTestePreCondicao != value;
                if (value != null)
                {
                    SetPropertyValue<CasoTestePreCondicao>("CasoTestePreCondicao", ref casoTestePreCondicao, value);
                    if (alterado && !IsLoading && !IsDeleted)
                    {
                        csSalvandoNovo = true;
                        OrdenacaoUtil.RnCriarOrdem(this);
                        csSalvandoNovo = false;
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
                if (alterado && !IsLoading && !IsDeleted && !csSalvandoNovo && !csReOrdenando)
                {
                    OrdenacaoUtil.RnAplicarOrdenacao(this);
                }
            }
        }
        #endregion

        #region NonPersistentProperties
        #endregion

        #region BusinessRules
        /// <summary>
        /// OnSaved
        /// </summary>
        protected override void OnSaved()
        {
            base.OnSaved();
            /*if (CasoTestePreCondicao.Session.InTransaction)
                CasoTestePreCondicao.Session.CommitTransaction();*/
        }

        /// <summary>
        /// Metodo OnDeleted
        /// </summary>
        protected override void OnDeleted()
        {
            base.OnDeleted();
            OrdenacaoUtil.RnDeletarOrdenacao(this);
            CasoTestePreCondicao.NbMaiorInformacaoAdicional = (UInt16)CasoTestePreCondicao.PreCondicoesInformacoesAdicionais.Count;
        }

        /// <summary>
        /// Regra de negócio que valida se um requisito existe
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePreCondicaoInformacaoAdicional", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Selecione um requisito")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarRequisito
        {
            get
            {
                return CasoTestePreCondicao.CasoTeste.Requisito != null;
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

            if (CasoTestePreCondicao == null)
            {
                return 0;
            }
            else
            {
                return CasoTestePreCondicao.NbMaiorInformacaoAdicional;
            }

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

                foreach (CasoTestePreCondicaoInformacaoAdicional cont in CasoTestePreCondicao.PreCondicoesInformacoesAdicionais)
                {
                    if ((cont != this) && (cont.NbSequencia >= ordemInicial && cont.NbSequencia <= ordemFinal))
                    {
                        collection.Add(cont);
                    }
                }
            }
            else
            {
                foreach (CasoTestePreCondicaoInformacaoAdicional cont in CasoTestePreCondicao.PreCondicoesInformacoesAdicionais)
                {
                    if ((cont != this) && (cont.NbSequencia > ordemInicial))
                    {
                        collection.Add(cont);
                    }
                }
            }

            collection.Sort((r1, r2) =>
            {
                return ((CasoTestePreCondicaoInformacaoAdicional)r1).NbSequencia.CompareTo(((CasoTestePreCondicaoInformacaoAdicional)r2).NbSequencia);
            });

            return collection;
        }
        #endregion

        #region Utils
        #endregion

        #region User Interface
        #endregion

        #region Constructors
        /// <summary>
        /// OnLoaded
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();
            nbSequenciaOld = NbSequencia;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session">session</param>
        public CasoTestePreCondicaoInformacaoAdicional(Session session)
            : base(session)
        {

        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        /// <summary>
        /// Metodo da interface que retorna os valores reordenados
        /// </summary>
        /// <returns>csReordenando</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return csReOrdenando;
        }

        /// <summary>
        /// Metodo da interface que recebe os valores para serem reordenados
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetReOrdenando(bool value)
        {
            csReOrdenando = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor antigo da sequência
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
        /// Metodo da interface que retorna o valor de NbSequencia
        /// </summary>
        /// <returns>NbSequencia</returns>
        ushort IOrdenacao.GetNbOrdem()
        {
            return NbSequencia;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para NbSequencia
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetNbOrdem(ushort value)
        {
            if (value > CasoTestePreCondicao.NbMaiorInformacaoAdicional)
            {
                CasoTestePreCondicao.NbMaiorInformacaoAdicional = value;
            }

            NbSequencia = value;
        }

        /// <summary>
        /// Metodo que retorna o Oid
        /// </summary>
        /// <returns>Oid</returns>
        Guid IOrdenacao.GetOid()
        {
            return Oid;
        }

        /// <summary>
        /// Metodo da interdace que retorna o metodo IsDeleted
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IOrdenacao.IsDeleted()
        {
            return IsDeleted;
        }

    }
    #endregion

}