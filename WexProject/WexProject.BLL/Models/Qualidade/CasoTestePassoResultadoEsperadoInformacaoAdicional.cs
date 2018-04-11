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
    /// Classe para CasoTestePassoResultadoEsperadoInformacaoAdicional
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [Custom("Caption", "Projetos > Qualidade > Casos de Teste > Resultado Esperado > Informações Adicionais")]
    [OptimisticLocking( false )]
    public class CasoTestePassoResultadoEsperadoInformacaoAdicional : Note, IOrdenacao
    {

        #region Attributes
        /// <summary>
        /// Atributo de CasoTestePassoResultadoEsperado
        /// </summary>
        private CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado;

        /// <summary>
        /// Atributo de controle (True or false)
        /// </summary>
        private bool csReOrdenando;

        /// <summary>
        /// Atributo que guarda o valor antigo de NbSequencia
        /// </summary>
        private UInt16 nbSequenciaOld;

        /// <summary>
        /// Atributo para verificar se esta sendo criado um novo objeto ou uma edição
        /// </summary>
        private bool csSalvandoNovo;

        /// <summary>
        /// Atributo de NbSequencia
        /// </summary>
        private UInt16 nbSequencia;
        #endregion

        #region Properties
        /// <summary>
        /// Variável que importa CasoTEstePassoResultadoEsperado
        /// </summary>
        [Association("CasoTestePassoResultadoEsperadoInformacaoAdicional", typeof(CasoTestePassoResultadoEsperado))]
        public CasoTestePassoResultadoEsperado CasoTestePassoResultadoEsperado
        {
            get
            {
                return casoTestePassoResultadoEsperado;
            }
            set
            {
                bool alterado = casoTestePassoResultadoEsperado != value;
                if (value != null)
                {
                    SetPropertyValue<CasoTestePassoResultadoEsperado>("CasoTestePassoResultadoEsperado", ref casoTestePassoResultadoEsperado, value);
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
            if (CasoTestePassoResultadoEsperado.Session.InTransaction)
                CasoTestePassoResultadoEsperado.Session.CommitTransaction();
        }

        /// <summary>
        /// Metodo OnDeleted
        /// </summary>
        protected override void OnDeleted()
        {
            base.OnDeleted();
            OrdenacaoUtil.RnDeletarOrdenacao(this);
            CasoTestePassoResultadoEsperado.NbMaiorInformacaoAdicional = (UInt16)CasoTestePassoResultadoEsperado.ResultadosEsperadosInformacoesAdicionais.Count;
        }

        /// <summary>
        /// Regra de negócio que valida se o requisito existe
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePassoResultadoEsperadoInformacaoAdicional", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Selecione um requisito")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarRequisito
        {
            get
            {
                return CasoTestePassoResultadoEsperado.Passo.CasoTeste.Requisito != null;
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

            if (CasoTestePassoResultadoEsperado == null)
            {
                return 0;
            }
            else
            {
                return CasoTestePassoResultadoEsperado.NbMaiorInformacaoAdicional;
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

                foreach (CasoTestePassoResultadoEsperadoInformacaoAdicional cont in CasoTestePassoResultadoEsperado.ResultadosEsperadosInformacoesAdicionais)
                {
                    if ((cont != this) && (cont.NbSequencia >= ordemInicial && cont.NbSequencia <= ordemFinal))
                    {
                        collection.Add(cont);
                    }
                }
            }
            else
            {
                foreach (CasoTestePassoResultadoEsperadoInformacaoAdicional cont in CasoTestePassoResultadoEsperado.ResultadosEsperadosInformacoesAdicionais)
                {
                    if ((cont != this) && (cont.NbSequencia > ordemInicial))
                    {
                        collection.Add(cont);
                    }
                }
            }

            collection.Sort((r1, r2) =>
            {
                return ((CasoTestePassoResultadoEsperadoInformacaoAdicional)r1).NbSequencia.CompareTo(((CasoTestePassoResultadoEsperadoInformacaoAdicional)r2).NbSequencia);
            });

            return collection;
        }
        #endregion

        #region Utils

        /// <summary>
        /// Metodo da interface que retorna o valor antido da sequência
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
            if (value > CasoTestePassoResultadoEsperado.NbMaiorInformacaoAdicional)
            {
                CasoTestePassoResultadoEsperado.NbMaiorInformacaoAdicional = value;
            }

            NbSequencia = value;
        }
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
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public CasoTestePassoResultadoEsperadoInformacaoAdicional(Session session)
            : base(session)
        {

        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //nbSequenciaOld = NbSequencia;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor de salvandoSequencia
        /// </summary>
        /// <returns>salvandoSequencia</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return csReOrdenando;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para salvandoSequencia
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetReOrdenando(bool value)
        {
            csReOrdenando = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor do Oid
        /// </summary>
        /// <returns>Oid</returns>
        Guid IOrdenacao.GetOid()
        {
            return Oid;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor de !IsDeleted
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IOrdenacao.IsDeleted()
        {
            return IsDeleted;
        }
    }
    #endregion

}
