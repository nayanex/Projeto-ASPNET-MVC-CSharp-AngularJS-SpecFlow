using System;
using System.Linq;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Planejamento;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.Collections.Generic;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Criação da classe ProjetoParteInteressada
    /// </summary>
    [RuleCombinationOfPropertiesIsUnique("ProjetoParteInteressada_Projeto_TxParteInteressada_Unique", DefaultContexts.Save, "TxParteInteressada, Projeto", Name = "JaExisteParteinteressadaProjeto",
    CustomMessageTemplate = "Já existe uma parte interessada com esse nome no projeto!")]
    [DefaultClassOptions]
    [Custom("Caption", "Partes Interessadas do Projeto")]
    [OptimisticLocking( false )]
    public class ProjetoParteInteressada : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Atributo de Projeto
        /// </summary>
        private Projeto projeto;

        /// <summary>
        /// Atributo de TxParteInteressada
        /// </summary>
        private ParteInteressada txParteInteressada;

        /// <summary>
        /// Atributo de ParteInteressadaPapel
        /// </summary>
        private Papel parteInteressadaPapel;

        /// <summary>
        /// Atributo que verifica a origem do projeto
        /// </summary>
        [Browsable(false)]
        public bool csVerificarOrigemProjeto;

        /// <summary>
        /// Aributo que verifica a origem da parte interessada
        /// </summary>
        [Browsable(false)]
        public bool csVerificarOrigemParteInteressada;

        #endregion

        #region Properties
        /// <summary>
        /// Variável que guarda o projeto que está sendo usado
        /// </summary>
        //Visibility = ViewItemVisibility.Hide,
        [AppearanceAttribute("ProjetoParteInteressada_Projeto_Appearance",
        Enabled = false,
        Criteria = "csVerificarOrigemProjeto = True",
        TargetItems = "Projeto")]
        [Association("ProjetoParteInteressada", typeof(Projeto))]
        [RuleRequiredField("ProjetoParteInteressada_Projeto_required", DefaultContexts.Save, "Selecione um Projeto")]
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
        /// Associação com ParteInteressada
        /// </summary>
        //Enabled = false,
        [AppearanceAttribute("ProjetoParteInteressada_TxParteInteressada_Appearance",
        Visibility = ViewItemVisibility.Hide,
        Criteria = "csVerificarOrigemParteInteressada = True",
        TargetItems = "TxParteInteressada"
        )]
        [Association("ParteInteressadaProjetoParteInteressada", typeof(ParteInteressada))]
        [RuleRequiredField("ProjetoParteInteressada_TxParteInteressada_required", DefaultContexts.Save, "Selecione uma Parte Interessada para o Projeto")]
        public ParteInteressada TxParteInteressada
        {
            get
            {
                return txParteInteressada;
            }
            set
            {
                SetPropertyValue<ParteInteressada>("TxParteInteressada", ref txParteInteressada, value);
            }
        }

        /// <summary>
        /// Variável que guarda o papel da parte interessada
        /// </summary>
        [RuleRequiredField("ProjetoParteInteressada_Papel_required", DefaultContexts.Save, "Selecione um Papel Exercido no Projeto")]
        public Papel ParteInteressadaPapel
        {
            get
            {
                return parteInteressadaPapel;
            }
            set
            {
                SetPropertyValue<Papel>("ParteInteressadaPapel", ref parteInteressadaPapel, value);
            }
        }

        #endregion

        #region NonPersistentProperties
        
        #endregion

        #region BusinessRules
        /// <summary>
        /// metodo que relaciona um Projeto
        /// </summary>
        /// <param name="projeto">objeto de projeto</param>
        public void RnSelecionarProjeto(Projeto projeto)
        {
            Projeto = projeto;

        }
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)

        

        public static List<Object> GetProjetosBySuperioresImediatos(params string[] s_oids)
        {
            //TODO: Refatorar para utilizar o serviço.   

            return new List<object>();
        }
            
        public static List<Object> GetProjetosByNoSuperioresImediatos(Session session, params string[] s_oids)
        {
            //TODO: Refatorar para utilizar o serviço.   

            return new List<object>();
        }
                    

        #endregion

        #region Utils
        /// <summary>
        /// Override de TxParteInteressada
        /// </summary>
        /// <returns>returns</returns>
        public override string ToString()
        {
            if (TxParteInteressada != null)
                return TxParteInteressada.TxParteInteressadaNome;
            else
                return base.ToString();
        }
        #endregion

        #region UserInterface
        #endregion

        #region Constructors
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        /// <param name="session">session</param>
        public ProjetoParteInteressada(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// afterConstruction da classe ProjetoParteInteressada
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Projeto = Projeto.GetProjetoAtual(Session);
        }
        #endregion
    }
}