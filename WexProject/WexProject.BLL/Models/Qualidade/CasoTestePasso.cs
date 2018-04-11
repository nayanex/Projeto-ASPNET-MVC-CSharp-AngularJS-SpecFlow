using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;
using System.Text;
using WexProject.Library.Libs.Ordenacao;
using System.ComponentModel;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.DB;
using WexProject.BLL.Shared.Domains.Qualidade;
using WexProject.Library.Libs.Enumerator;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Qualidade
{
    /// <summary>
    /// Classe dos passos de caso de teste
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [Custom("Caption", "Projetos > Qualidade > Casos de Teste > Passos")]
    [OptimisticLocking( false )]
    public class CasoTestePasso : BaseObject, IOrdenacao
    {
        #region Attributes
        /// <summary>
        /// Atributo de CasoTeste
        /// </summary>
        private CasoTeste casoTeste;

        /// <summary>
        /// Atributo de TxPasso
        /// </summary>
        private String txPasso;

        /// <summary>
        /// Atributo que salva se a sequência ja foi ou não alterada (True ou False)
        /// </summary>
        private bool csReOrdenando;

        /// <summary>
        /// Variável que guarda o valor antigo de uma sequência
        /// </summary>
        private UInt16 nbSequenciaOld;

        /// <summary>
        /// Atributo de Maior Resultado
        /// </summary>
        private UInt16 nbMaiorResultadoEsperado;

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
        [Association("CasoTestePasso", typeof(CasoTeste))]
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
                        csSalvandoNovo = true;
                        OrdenacaoUtil.RnCriarOrdem(this);
                        csSalvandoNovo = false;
                    }

                }
            }
        }

        /// <summary>
        /// Variavel que concatena os arquivos de casoTesteResultadoEsperado
        /// </summary>
        [Size(1000)]
        public String TxPasso
        {
            get
            {
                return txPasso;
            }
            set
            {
                if (value != null)
                {
                    txPasso = StrUtil.RetirarEspacoVazio(txPasso);
                    SetPropertyValue<String>("TxPasso", ref txPasso, value.Trim());
                }
            }
        }

        /// <summary>
        /// Variavel que guarda a sequencia dos passos
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
        /// Variável que guarda o valor do Maior Resultado Esperado
        /// </summary>
        [Browsable(false)]
        public UInt16 NbMaiorResultadoEsperado
        {
            get
            {
                return nbMaiorResultadoEsperado;
            }
            set
            {
                SetPropertyValue<UInt16>("NbMaiorResultadoEsperado", ref nbMaiorResultadoEsperado, value);
            }
        }



        /// <summary>
        /// Associação com ResultadoEsperado
        /// </summary>
        [Association("CasoTesteResultadoEsperado", typeof(CasoTestePassoResultadoEsperado)), Aggregated]
        public XPCollection ResultadosEsperados
        {
            get
            {
                return GetCollection("ResultadosEsperados");
            }
        }

        #endregion

        #region NonPersistentProperties
        /// <summary>
        /// Campo que mostra as strings concatenadas
        /// </summary>
        [NonPersistent, Size(4000)]
        public String _TxResultadoEsperado
        {
            get
            {
                return GetStringsConcatenadas();
            }
            set
            {

                if (!IsLoading && !IsSaving &&
                ResultadosEsperados.Count == 0)
                {
                    CasoTestePassoResultadoEsperado resultado = new CasoTestePassoResultadoEsperado(Session) { Passo = this, TxResultadoEsperado = value, CsTiposResultado = CsTiposResultadosDomain.RN };
                    resultado.Save();
                }
            }
        }
        #endregion

        #region BusinessRules
        /// <summary>
        /// Metodo OnDeleted
        /// </summary>
        protected override void OnDeleted()
        {
            base.OnDeleted();
            OrdenacaoUtil.RnDeletarOrdenacao(this);

            CasoTeste.NbMaiorPasso = (UInt16)CasoTeste.Passos.Count;

        }

        /// <summary>
        /// Regra de negócio que faz a concatenação da string de CasoTestePassoResultadoEsperado.TxResultadoEsperado
        /// </summary>
        /// <returns>String Concatenadas</returns>
        public String GetStringsConcatenadas()
        {
            StringBuilder str = new StringBuilder();

            ResultadosEsperados.Sorting.Add(new SortProperty("NbSequencia", SortingDirection.Ascending));

            foreach (CasoTestePassoResultadoEsperado item in ResultadosEsperados)
                str.AppendLine(String.Format("{0} - {1}", EnumUtil.DescricaoEnum(item.CsTiposResultado), item.TxResultadoEsperado));

            return str.ToString();
        }

        /// <summary>
        /// Regra de negócio que impede o sistema de salvar um passo se não tiver nenhum requisito selecionado
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePasso", DefaultContexts.Save, InvertResult = false,
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
        /// Get para retornar a maior ordem de passos
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
                return CasoTeste.NbMaiorPasso;
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

                foreach (CasoTestePasso cont in CasoTeste.Passos)
                {
                    if ((cont != this) && (cont.NbSequencia >= ordemInicial && cont.NbSequencia <= ordemFinal))
                    {
                        collection.Add(cont);
                    }
                }
            }
            else
            {
                foreach (CasoTestePasso cont in CasoTeste.Passos)
                {
                    if ((cont != this) && (cont.NbSequencia > ordemInicial))
                    {
                        collection.Add(cont);
                    }
                }
            }

            collection.Sort((r1, r2) =>
            {
                return ((CasoTestePasso)r1).NbSequencia.CompareTo(((CasoTestePasso)r2).NbSequencia);
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
        /// Metodo da interface que seta o valor antigo da sequência para a variável
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
        /// Metodo da interface que verifica se o valor da sequência é maior que a variavel CasoTeste.NbMaiorPasso
        /// Se for, o metodo atribui o valor da sequência para CasoTeste.NbMaiorPasso
        /// </summary>
        /// <param name="value">value</param>
        void IOrdenacao.SetNbOrdem(ushort value)
        {
            if (value > CasoTeste.NbMaiorPasso)
            {
                CasoTeste.NbMaiorPasso = value;
            }

            NbSequencia = value;
        }

        /// <summary>
        /// Metodo da interface que retorna o valor de csSalvandoSequencia
        /// </summary>
        /// <returns>csReordenando</returns>
        bool IOrdenacao.GetReOrdenando()
        {
            return csReOrdenando;
        }

        /// <summary>
        /// Metodo da interface que seta um valor para csSalvandoSequencia
        /// </summary>
        /// <param name="value">value</param>
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

        /// <summary>
        /// Medoto da interface que recebe o metodo save
        /// </summary>
        void IOrdenacao.Save()
        {
            Save();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Atribui valores as variaveis após o sistema ser carregado
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
        public CasoTestePasso(Session session)
            : base(session)
        {

        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            csReOrdenando = false;
            NbMaiorResultadoEsperado = 0;
        }
        #endregion


    }

}
