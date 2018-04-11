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
    /// Classe  CasoTestePassoResultadoEsperadoAnexo
    /// </summary>
    [DefaultClassOptions]
    [DeferredDeletion(false)]
    [RuleCombinationOfPropertiesIsUnique("CasoTestePassoResultadoEsperadoAnexo_TxDescricao_Unique", DefaultContexts.Save, "TxDescricao, CasoTestePassoResultadoEsperado", Name = "JaExisteUmAnexoComEssaDescricaoNoResultadoEsperado",
    CustomMessageTemplate = "Já existe um anexo com essa descrição")]
    [Custom("Caption", "Projetos > Qualidade > Casos de Teste > Resultado Esperado > Anexo")]
    [OptimisticLocking( false )]
    public class CasoTestePassoResultadoEsperadoAnexo : FileAttachmentBase
    {

        #region Attributes
        /// <summary>
        /// Atributo de CasoTestePassoresultadoEsperado
        /// </summary>
        private CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txdescricao;
        #endregion

        #region Properties
        /// <summary>
        /// Associação com CasoTestePassoResultadoEsperado
        /// </summary>
        [Association("CasoTestePassoResultadoEsperadoAnexo", typeof(CasoTestePassoResultadoEsperado))]
        public CasoTestePassoResultadoEsperado CasoTestePassoResultadoEsperado
        {
            get
            {
                return casoTestePassoResultadoEsperado;
            }
            set
            {
                SetPropertyValue<CasoTestePassoResultadoEsperado>("CasoTestePassoResultadoEsperado", ref casoTestePassoResultadoEsperado, value);
            }
        }

        /// <summary>
        /// Variável que guarda a descrição do anexo
        /// </summary>
        [RuleRequiredField("CasoTestePassoResultadoEsperadoAnexo_TxDescricao_Required", DefaultContexts.Save, "Digite uma descrição para o anexo")]
        [Size(100)]
        public String TxDescricao
        {
            get
            {
                return txdescricao;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxDescricao", ref txdescricao, value.Trim());
            }
        }

        #endregion

        #region NonPersistentProperties
        #endregion

        #region BusinessRules
        /// <summary>
        /// onSaving
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
            if (CasoTestePassoResultadoEsperado.Session.InTransaction)
                CasoTestePassoResultadoEsperado.Session.CommitTransaction();
        }


        /// <summary>
        /// Regra de negócio para validar se o requisito está selecionado
        /// </summary>
        [RuleFromBoolProperty("ValidarRequisitoCasoTestePassoResultadoEsperadoAnexo", DefaultContexts.Save, InvertResult = false,
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
        /// Get que retorna os nomes dos anexos de CasoTestePassoResultadoEsperado
        /// </summary>
        /// <param name="resultadoEsperado">resultadoEsperado</param>
        /// <param name="txDescricaoAnexo">txDescricaoAnexo</param>
        /// <returns>tmp</returns>
        public static string GetOpenAnexos(CasoTestePassoResultadoEsperado resultadoEsperado, string txDescricaoAnexo)
        {
            CasoTestePassoResultadoEsperadoAnexo resultadoEsperadoAnexo = null;

            foreach (CasoTestePassoResultadoEsperadoAnexo anexo in resultadoEsperado.ResultadosEsperadosAnexos)
            {
                if (anexo.TxDescricao.ToLower().Contains(txDescricaoAnexo.ToLower()))
                {
                    resultadoEsperadoAnexo = anexo;
                    break;
                }
            }

            string tmp = string.Format("{0}{1}", System.IO.Path.GetTempPath(), resultadoEsperadoAnexo.File.FileName);

            using (FileStream file = new FileStream(tmp, FileMode.Create))
            {
                Byte[] info = resultadoEsperadoAnexo.File.Content;
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
        /// Constructor
        /// </summary>
        /// <param name="session">session</param>
        public CasoTestePassoResultadoEsperadoAnexo(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

    }

}
