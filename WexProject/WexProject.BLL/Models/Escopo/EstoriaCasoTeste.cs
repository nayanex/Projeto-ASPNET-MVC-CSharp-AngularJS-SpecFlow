using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Qualidade;

namespace WexProject.BLL.Models.Escopo
{
    /// <summary>
    /// Classe de EstoriasCasoTeste
    /// </summary>
    [RuleCombinationOfPropertiesIsUnique("EstoriaCasoTesteUnique", DefaultContexts.Save, "Estoria,CasoTeste", CustomMessageTemplate = "Já existe Caso de Teste com esse nome!")]
    [OptimisticLocking( false )]
    public class EstoriaCasoTeste : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de estoria
        /// </summary>
        private Estoria estoria;

        /// <summary>
        /// Atributo de CasoTeste
        /// </summary>
        private CasoTeste casoTeste;
        #endregion

        #region Properties

        /// <summary>
        /// Associação com Estoria
        /// </summary>
        [Association("EstoriaCasosTeste", typeof(Estoria))]
        public Estoria Estoria
        {
            get
            {
                return estoria;
            }
            set
            {
                SetPropertyValue<Estoria>("Estoria", ref estoria, value);
            }
        }

        /// <summary>
        /// Import CasoTeste
        /// </summary>
        [Association("EstoriasCasoTeste", typeof(EstoriaCasoTeste))]
        [RuleRequiredField("EstoriaCasoTeste_CasoTeste_Required", DefaultContexts.Save)]
        [RuleValueComparison("EstoriaCasoTeste_CasoTeste_NotNull", DefaultContexts.Save, ValueComparisonType.NotEquals, null)]
        public CasoTeste CasoTeste
        {
            get
            {
                return casoTeste;
            }
            set
            {
                SetPropertyValue<CasoTeste>("CasoTeste", ref casoTeste, value);
            }
        }

        #endregion

        #region NonPersistentProperties
        #endregion

        #region BusinessRules
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        #endregion

        #region Utils
        /// <summary>
        /// Override de CasoTeste
        /// </summary>
        /// <returns>returns</returns>
        public override string ToString()
        {
            if (CasoTeste != null)
                return CasoTeste.TxID;
            else
                return base.ToString();
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public EstoriaCasoTeste(Session session)
            : base(session)
        {
        }
        #endregion

    }

}
