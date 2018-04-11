using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Classe de Configura��o
    /// </summary>
    [DeferredDeletion(false)]
    [DefaultClassOptions]
    [Custom("Caption", "Configura��o")]
    [OptimisticLocking( false )]
    public class Configuracao : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Valor da quantidade m�xima que se pode vender
        /// </summary>
        private uint nbQtdeMaxVenda;

        /// <summary>
        /// Valor da quantidade m�xima que se pode tirar f�rias
        /// </summary>
        private uint nbQtdeMaxFerias;

        /// <summary>
        /// Valor da data m�xima que se pode tirar f�rias dentro de um
        /// per�odo aquisitivo
        /// </summary>
        private uint nbDtMaxTirarFerias;

        #endregion

        #region Propriedades

        /// <summary>
        /// Valor da quantidade m�xima que se pode vender
        /// </summary>
        [Custom("Caption", "Qtde m�xima de venda")]
        [RuleValueComparison("Configuracao_NbQtdeMaxVenda_Comparison", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, 0, Name = "Qtde m�xima de venda")]
        [RuleRequiredField("Configuracao_NbQtdeMaxVenda_Required", DefaultContexts.Save,
            Name = "NbQtdeMaxVenda", CustomMessageTemplate = "Informe uma quantidade m�xima de venda")]
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
        /// Valor da quantidade m�xima que se pode tirar f�rias
        /// </summary>
        [Custom("Caption", "Qtde m�xima de f�rias")]
        [RuleValueComparison("Configuracao_NbQtdeMaxFerias_Comparison", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, 0, Name = "Qtde m�xima de f�rias")]
        [RuleRequiredField("Configuracao_NbQtdeMaxFerias_Required", DefaultContexts.Save,
            Name = "NbQtdeMaxFerias", CustomMessageTemplate = "Informe uma quantidade m�xima de f�rias")]
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
        /// Valor da data m�xima que se pode tirar f�rias dentro de um
        /// per�odo aquisitivo
        /// </summary>
        [Custom("Caption", "Data m�xima para tirar f�rias")]
        [RuleValueComparison("Configuracao_NbDtMaxTirarFerias_Comparison", DefaultContexts.Save,
            ValueComparisonType.GreaterThanOrEqual, 0, Name = "Data m�xima para tirar f�rias")]
        [RuleRequiredField("Configuracao_NbDtMaxTirarFerias_Required", DefaultContexts.Save,
            Name = "NbDtMaxTirarFerias", CustomMessageTemplate = "Informe uma data m�xima para tirar f�rias")]
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
        /// Valida��o da quantidade m�xima de F�rias
        /// </summary>
        [RuleFromBoolProperty("Configuracao_RnVerificarDiasMaxFerias", DefaultContexts.Save,
            "A quantidade m�xima de F�rias deve ser maior ou igual �s quantidades definidas nas Modalidades de F�rias ativas.")]
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
        /// Valida��o da quantidade m�xima de venda de F�rias
        /// </summary>
        [RuleFromBoolProperty("Configuracao_RnVerificarDiasMaxVendaFerias", DefaultContexts.Save,
            "A quantidade m�xima de Venda de F�rias deve ser maior ou igual �s quantidades definidas nas Modalidades de F�rias (de venda) ativas.")]
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
        /// Obten��o dos dados definidos na Configura��o
        /// </summary>
        /// <param name="session">Sess�o atual</param>
        /// <returns>Os dados definidos na Configura��o</returns>
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
        /// <param name="session">Sess�o atual</param>
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