using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Escopo
{
    /// <summary>
    /// Classe do solicitante
    /// </summary>
    [Custom("Caption", "Projetos > Escopo > Básicos > Solicitantes")]
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class Solicitante : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo de TxNome
        /// </summary>
        private String txNome;

        /// <summary>
        /// Atributo de EmpresaInstituicao
        /// </summary>
        private EmpresaInstituicao empresaInstituicao;

        /// <summary>
        /// Atributo de Cargo
        /// </summary>
        private Cargo cargo;
        #endregion

        #region Properties
        /// <summary>
        /// Variável que guarda o nome do Solicitante
        /// </summary>
        [RuleRequiredField("Solicitante_TxNome_Required", DefaultContexts.Save)]
        public String TxNome
        {
            get
            {
                return txNome;
            }
            set
            {
                txNome = StrUtil.RetirarEspacoVazio(txNome);
                SetPropertyValue<String>("TxNome", ref txNome, value.Trim());
            }
        }

        /// <summary>
        /// Import do Cliente
        /// </summary>
        [RuleRequiredField("Solicitante_EmpresaInstituicao_Required", DefaultContexts.Save)]
        public EmpresaInstituicao EmpresaInstituicao
        {
            get
            {
                return empresaInstituicao;
            }
            set
            {
                SetPropertyValue<EmpresaInstituicao>("EmpresaInstituicao", ref empresaInstituicao, value);
            }
        }

        /// <summary>
        /// Import do Cargo
        /// </summary>
        [RuleRequiredField("Solicitante_Cargo_Required", DefaultContexts.Save)]
        public Cargo Cargo
        {
            get
            {
                return cargo;
            }
            set
            {
                SetPropertyValue<Cargo>("Cargo", ref cargo, value);
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
        /// <param name="session">Solicitante</param>
        public Solicitante(Session session)
            : base(session)
        {
        }
        #endregion
    }

}
