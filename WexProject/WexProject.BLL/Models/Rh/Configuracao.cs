using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Classe de Configuração
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Configuração")]
    [OptimisticLocking( false )]
    public class Configuracao : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Valor da quantidade máxima que se pode vender
        /// </summary>
        private uint nbQtdeMaxVenda;

        /// <summary>
        /// Valor da quantidade máxima que se pode tirar férias
        /// </summary>
        private uint nbQtdeMaxFerias;

        /// <summary>
        /// Valor da data máxima que se pode tirar férias dentro de um
        /// período aquisitivo
        /// </summary>
        private uint nbDtMaxTirarFerias;

        #endregion

        #region Propriedades

        /// <summary>
        /// Valor da quantidade máxima que se pode vender
        /// </summary>
        [Custom("Caption", "Qtde máxima de venda")]
        [RuleValueComparison("Configuracao_NbQtdeMaxVenda_Comparison", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, 0, Name = "Qtde máxima de venda")]
        [RuleRequiredField("Configuracao_NbQtdeMaxVenda_Required", DefaultContexts.Save,
            Name = "NbQtdeMaxVenda", CustomMessageTemplate = "Informe uma quantidade máxima de venda")]
        public uint NbQtdeMaxVenda
        {
            get
            {
                return nbQtdeMaxVenda;
            }
            set
            {
                if (nbQtdeMaxVenda == value)
                    return;

                SetPropertyValue<uint>("NbQtdeMaxVenda", ref nbQtdeMaxVenda, value);
            }
        }

        /// <summary>
        /// Valor da quantidade máxima que se pode tirar férias
        /// </summary>
        [Custom("Caption", "Qtde máxima de férias")]
        [RuleValueComparison("Configuracao_NbQtdeMaxFerias_Comparison", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, 0, Name = "Qtde máxima de férias")]
        [RuleRequiredField("Configuracao_NbQtdeMaxFerias_Required", DefaultContexts.Save,
            Name = "NbQtdeMaxFerias", CustomMessageTemplate = "Informe uma quantidade máxima de férias")]
        public uint NbQtdeMaxFerias
        {
            get
            {
                return nbQtdeMaxFerias;
            }
            set
            {
                if (nbQtdeMaxFerias == value)
                    return;

                SetPropertyValue<uint>("NbQtdeMaxFerias", ref nbQtdeMaxFerias, value);
            }
        }

        /// <summary>
        /// Valor da data máxima que se pode tirar férias dentro de um
        /// período aquisitivo
        /// </summary>
        [Custom("Caption", "Data máxima para tirar férias")]
        [RuleValueComparison("Configuracao_NbDtMaxTirarFerias_Comparison", DefaultContexts.Save,
            ValueComparisonType.GreaterThanOrEqual, 0, Name = "Data máxima para tirar férias")]
        [RuleRequiredField("Configuracao_NbDtMaxTirarFerias_Required", DefaultContexts.Save,
            Name = "NbDtMaxTirarFerias", CustomMessageTemplate = "Informe uma data máxima para tirar férias")]
        public uint NbDtMaxTirarFerias
        {
            get
            {
                return nbDtMaxTirarFerias;
            }
            set
            {
                if (nbDtMaxTirarFerias == value)
                    return;

                SetPropertyValue<uint>("NbDtMaxTirarFerias", ref nbDtMaxTirarFerias, value);
            }
        }

        #endregion

        #region BusinessRules



        /// <summary>
        /// Validação da quantidade máxima de Férias
        /// </summary>
        [RuleFromBoolProperty("Configuracao_RnVerificarDiasMaxFerias", DefaultContexts.Save,
            "A quantidade máxima de Férias deve ser maior ou igual às quantidades definidas nas Modalidades de Férias ativas.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarDiasMaxFerias
        {
            get
            {
                ModalidadeFerias modalidade = ModalidadeFerias.GetModalidadeFeriasAtivaMaiorDia(Session);

                if (modalidade == null || modalidade.NbDias <= NbQtdeMaxFerias)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Validação da quantidade máxima de venda de Férias
        /// </summary>
        [RuleFromBoolProperty("Configuracao_RnVerificarDiasMaxVendaFerias", DefaultContexts.Save,
            "A quantidade máxima de Venda de Férias deve ser maior ou igual às quantidades definidas nas Modalidades de Férias (de venda) ativas.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarDiasMaxVendaFerias
        {
            get
            {
                ModalidadeFerias modalidade = ModalidadeFerias.GetModalidadeFeriasAtivaMaiorDia(Session, true);

                if (modalidade == null || modalidade.NbDias <= NbQtdeMaxVenda)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Obtenção dos dados definidos na Configuração
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <returns>Os dados definidos na Configuração</returns>
        public static Configuracao GetInstancia( Session session )
        {
            using(XPCollection<Configuracao> itens = new XPCollection<Configuracao>( session ))
            {
                if(itens.Count == 0)
                {
                    Configuracao config = new Configuracao( session )
                    {
                        NbDtMaxTirarFerias = 12,
                        NbQtdeMaxFerias = 30,
                        NbQtdeMaxVenda = 10
                    };

                    config.Save();

                    return config;
                }

                return itens[0];
            }
        }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão atual</param>
        public Configuracao(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }

        /// <summary>
        /// Depois de construir um objeto (novo)
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        #endregion
    }
}