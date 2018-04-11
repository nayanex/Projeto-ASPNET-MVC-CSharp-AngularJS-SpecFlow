using System;

using DevExpress.Xpo;

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;


namespace WexProject.BLL.Models.Geral
{

    /// <summary>
    /// Criação da classe ProjetoCliente
    /// </summary>
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class ProjetoCliente : BaseObject
    {

        #region Attributes
        /// <summary>
        /// Atributo de projeto
        /// </summary>
        private Projeto projeto;

        /// <summary>
        /// Atributo de cliente
        /// </summary>
        private EmpresaInstituicao cliente;
        #endregion


        #region Properties


        /// <summary>
        /// Associação com o projeto
        /// </summary>
        [Association("ProjetoCliente", typeof(Projeto))]
        public Projeto Projeto
        {
            get
            {
                return projeto;
            }
            set
            {
                SetPropertyValue<Projeto>("Projeto", ref projeto, value);
            }
        }


        /// <summary>
        /// Variável que importa os valores de EmpresaInstuicao
        /// </summary>
        [RuleRequiredField("ProjetoCliente_Cliente_Required", DefaultContexts.Save, "Selecione um cliente!")]
        public EmpresaInstituicao Cliente
        {
            get
            {
                return cliente;
            }
            set
            {
                SetPropertyValue<EmpresaInstituicao>("Cliente", ref cliente, value);
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
        #endregion

        #region Constructors

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">ProjetoCliente</param>
        public ProjetoCliente(Session session)
            : base(session)
        {
        }
        #endregion

    }
}
