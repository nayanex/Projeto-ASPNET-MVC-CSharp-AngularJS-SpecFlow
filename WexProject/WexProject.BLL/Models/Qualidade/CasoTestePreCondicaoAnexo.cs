using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using System.IO;

namespace WexProject.BLL.Models.Qualidade
{
    /// <summary>
    /// Classe CasoTestePreCondicaoAnexo
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [RuleCombinationOfPropertiesIsUnique("CasoTestePreCondicao_Anexos_TxDescricao_Unique", DefaultContexts.Save, "TxDescricao, CasoTestePreCondicao", Name = "JaExisteUmAnexoComEssaDescricaoNaPreCondicao",
    CustomMessageTemplate = "Já existe um anexo com essa descrição")]
    [Custom("Caption", "Projetos > Qualidade > Casos de Teste > Pré-condição > Anexo")]
    [OptimisticLocking( false )]
    public class CasoTestePreCondicaoAnexo : FileAttachmentBase
    {

        #region Attributes
        /// <summary>
        /// Atributo de CasoTestePreCondicao
        /// </summary>
        private CasoTestePreCondicao casoTestePreCondicao;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txDescricao;
        #endregion

        #region Properties
        /// <summary>
        /// Variável que importa CasoTestePreCondicao
        /// </summary>
        [Association("CasoTestePreCondicaoAnexo", typeof(CasoTestePreCondicao))]
        public CasoTestePreCondicao CasoTestePreCondicao
        {
            get
            {
                return casoTestePreCondicao;
            }
            set
            {
                SetPropertyValue<CasoTestePreCondicao>("CasoTestePreCondicao", ref casoTestePreCondicao, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor da descrição
        /// </summary>
        [RuleRequiredField("CasoTestePreCondicaoAnexo_TxDescricao_Required", DefaultContexts.Save, "Digite uma descrição para o Anexo")]
        [Size(100)]
        public String TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value.Trim());
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
            if (CasoTestePreCondicao.Session.InTransaction)
                CasoTestePreCondicao.Session.CommitTransaction();
        }

        /// <summary>
        /// Regra de negócio que valida se um requisito existe
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePreCondicaoAnexo", DefaultContexts.Save, InvertResult = false,
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
        /// Get que retorna os nomes dos anexos de CasoTestePreCondicao
        /// </summary>
        /// <param name="preCondicao">preCondicao</param>
        /// <param name="txDescricaoAnexo">txDescricaoAnexo</param>
        /// <returns>tmp</returns>
        public static string GetOpenAnexos(CasoTestePreCondicao preCondicao, string txDescricaoAnexo)
        {
            CasoTestePreCondicaoAnexo preCondicaoAnexo = null;

            foreach (CasoTestePreCondicaoAnexo anexo in preCondicao.CasoTestePreCondicaoAnexos)
            {
                if (anexo.TxDescricao.ToLower().Contains(txDescricaoAnexo.ToLower()))
                {
                    preCondicaoAnexo = anexo;
                    break;
                }
            }

            string tmp = string.Format("{0}{1}", System.IO.Path.GetTempPath(), preCondicaoAnexo.File.FileName);

            using (FileStream file = new FileStream(tmp, FileMode.Create))
            {
                Byte[] info = preCondicaoAnexo.File.Content;
                // Add as informacoes do arquivo.
                file.Write(info, 0, info.Length);
                file.Close();
            }
            return tmp;
        }

        #endregion

        #region Utils
        #endregion

        #region User Interface
        #endregion

        #region Constructors
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">session</param>
        public CasoTestePreCondicaoAnexo(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// AfterConstructor
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

    }

}
