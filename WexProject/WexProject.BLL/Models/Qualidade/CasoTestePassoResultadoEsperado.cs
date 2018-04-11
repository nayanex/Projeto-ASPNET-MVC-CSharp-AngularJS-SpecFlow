using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Ordenacao;
using System.Collections.Generic;
using WexProject.BLL.Shared.Domains.Qualidade;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Qualidade
{
    /// <summary>
    /// Classe para os resultados esperados pelo caso de teste
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("TxResultadoEsperado")]
    [DeferredDeletion(false)]
    [Custom("Caption", "Projetos > Qualidade > Casos de Teste > Resultado Esperado")]
    [OptimisticLocking( false )]
    public class CasoTestePassoResultadoEsperado : BaseObject, IOrdenacao
    {
        #region Attributes
        /// <summary>
        /// Atributo CasoTeste
        /// </summary>
        private CasoTestePasso passo;

        /// <summary>
        /// Atributo TxResultadoEsperado
        /// </summary>
        private String txResultadoEsperado;

        /// <summary>
        /// TiposResultados
        /// </summary>
        private CsTiposResultadosDomain csTiposResultados;
        /// <summary>
        /// Atributo que guarda o valor antigo da NbSequencia
        /// </summary>
        private UInt16 nbSequenciaOld;

        /// <summary>
        /// Atributo de controle (True or false)
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
        private bool csSalvandoNovo;
        #endregion

        #region Properties

        /// <summary>
        /// Associação com CasoTeste
        /// </summary>
        [Association("CasoTesteResultadoEsperado", typeof(CasoTestePasso))]
        public CasoTestePasso Passo
        {
            get
            {
                return passo;
            }
            set
            {
                bool alterado = passo != value;

                if (value != null)
                {
                    SetPropertyValue<CasoTestePasso>("Passo", ref passo, value);
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
        /// Associação com resultado esperado        
        /// </summary>
        [RuleRequiredField("CasoTesteResultadoEsperado_TxResultadoEsperado_Required", DefaultContexts.Save, "Digite uma descrição para o resultado esperado")]
        public String TxResultadoEsperado
        {
            get
            {
                return txResultadoEsperado;
            }
            set
            {
                if (value != null)
                {
                    txResultadoEsperado = StrUtil.RetirarEspacoVazio(txResultadoEsperado);
                    SetPropertyValue<String>("TxResultadoEsperado", ref txResultadoEsperado, value.Trim());
                }
            }
        }


        /// <summary>
        /// Associação com TiposResultados
        /// </summary>
        [RuleRequiredField("CasoTesteresultadoEsperado_TipoResultados_Required", DefaultContexts.Save)]
        public CsTiposResultadosDomain CsTiposResultado
        {
            get
            {
                return csTiposResultados;
            }
            set
            {
                SetPropertyValue<CsTiposResultadosDomain>("CsTiposResultado", ref csTiposResultados, value);
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

        /// <summary>
        /// Variável que guarda o maior valor de uma informação adicional
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
        /// Associação com ResultadoEsperadoInformacaoAdicional
        /// </summary>
        [Association("CasoTestePassoResultadoEsperadoInformacaoAdicional", typeof(CasoTestePassoResultadoEsperadoInformacaoAdicional)), Aggregated]
        public XPCollection ResultadosEsperadosInformacoesAdicionais
        {
            get
            {
                return GetCollection("ResultadosEsperadosInformacoesAdicionais");
            }
        }

        /// <summary>
        /// Associação com ResultadoEsperadoAnexo
        /// </summary>
        [Association("CasoTestePassoResultadoEsperadoAnexo", typeof(CasoTestePassoResultadoEsperadoAnexo)), Aggregated]
        public XPCollection ResultadosEsperadosAnexos
        {
            get
            {
                return GetCollection("ResultadosEsperadosAnexos");
            }
        }
        #endregion

        #region NonPersistentProperties
        /// <summary>
        /// Variável não persistente que retorna o número de informações adicionais que o resultado esperado possui
        /// </summary>
        [NonPersistent]
        public UInt16 _NbInformacaoAdicional
        {
            get
            {
                return (UInt16)ResultadosEsperadosInformacoesAdicionais.Count;
            }

        }

        /// <summary>
        /// Variável não persistenta que retorna o número de anexos que o resultado esperado possui
        /// </summary>
        [NonPersistent]
        public UInt16 _NbAnexo
        {
            get
            {
                return (UInt16)ResultadosEsperadosAnexos.Count;
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
        /// OnSaved
        /// </summary>
        protected override void OnSaved()
        {
            base.OnSaved();
            if (Passo.Session.InTransaction)
                Passo.Session.CommitTransaction();
        }

        /// <summary>
        /// OnDeleted
        /// </summary>
        protected override void OnDeleted()
        {
            base.OnDeleted();
            OrdenacaoUtil.RnDeletarOrdenacao(this);
            Passo.NbMaiorResultadoEsperado = (UInt16)Passo.ResultadosEsperados.Count;
        }

        /// <summary>
        /// Regra de negócio que valida se um requisito existe
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePassoResultadoEsperado", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Selecione um requisito")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarRequisito
        {
            get
            {
                return Passo.CasoTeste.Requisito != null;
            }
        }

        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        /// <summary>
        /// Get para retornar a maior ordem de passos
        /// </summary>
        /// <returns>Retorna o maior valor</returns>
        UInt16 IOrdenacao.GetMaiorOrdem()
        {

            if (Passo == null)
            {
                return 0;
            }
            else
            {
                return Passo.NbMaiorResultadoEsperado;
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

                foreach (CasoTestePassoResultadoEsperado cont in Passo.ResultadosEsperados)
                {
                    if ((cont != this) && (cont.NbSequencia >= ordemInicial && cont.NbSequencia <= ordemFinal))
                    {
                        collection.Add(cont);

                    }
                }
            }
            else
            {
                foreach (CasoTestePassoResultadoEsperado cont in Passo.ResultadosEsperados)
                {
                    if ((cont != this) && (cont.NbSequencia > ordemInicial))
                    {
                        collection.Add(cont);
                    }
                }
            }

            collection.Sort((r1, r2) =>
            {
                return ((CasoTestePassoResultadoEsperado)r1).NbSequencia.CompareTo(((CasoTestePassoResultadoEsperado)r2).NbSequencia);
            });

            return collection;
        }
        #endregion

        #region Utils

        /// <summary>
        /// Metodo da interface que retorna o valor antigo da sequência
        /// </summary>
        /// <returns>Valor antigo da sequência</returns>
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
        /// Metodo da interface que retorna o valor da sequência
        /// </summary>
        /// <returns>NbSequencia</returns>
        ushort IOrdenacao.GetNbOrdem()
        {
            return NbSequencia;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para a sequência
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetNbOrdem(ushort value)
        {
            if (value > Passo.NbMaiorResultadoEsperado)
            {
                Passo.NbMaiorResultadoEsperado = value;
            }

            NbSequencia = value;
        }


        /// <summary>
        /// Metodo da interface que retorna o valor de csSalvandoSequencia
        /// </summary>
        /// <returns>True or False</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return csReOrdenando;
        }

        /// <summary>
        /// Metodo da interface que atribui um valor para csSalvandoSequencia
        /// </summary>
        /// <param name="value">true or false</param>
        void IOrdenacao.SetReOrdenando(bool value)
        {
            csReOrdenando = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o Oid
        /// </summary>
        /// <returns>Oid</returns>
        Guid IOrdenacao.GetOid()
        {
            return Oid;
        }

        /// <summary>
        /// Metodo da interface que retorna o metodo IsDeleted
        /// </summary>
        /// <returns>IsDeleted</returns>
        bool IOrdenacao.IsDeleted()
        {
            return IsDeleted;
        }

        /// <summary>
        /// Metodo da interface que retorna o metodo Save
        /// </summary>
        void IOrdenacao.Save()
        {
            Save();
        }
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
        /// Inicialização da classe CasoTesteResultadoEsperado
        /// </summary>
        /// <param name="session">session</param>
        public CasoTestePassoResultadoEsperado(Session session)
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
            csReOrdenando = false;

        }
        #endregion


    }

}
