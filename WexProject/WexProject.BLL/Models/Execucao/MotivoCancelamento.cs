using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections;
using System.Collections.Generic;
using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.BLL.Models.Execucao
{
    /// <summary>
    /// Classe de motivo de cancelamento do ciclo
    /// </summary>
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class MotivoCancelamento : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Guarda a descri��o do motivo de cancelamento
        /// </summary>
        private String txDescricao;

        /// <summary>
        /// Guarda o status do motivo de cancelamento
        /// </summary>
        private CsStatusMotivoCancelamento csSituacao;
        #endregion
        
        #region Properties

        /// <summary>
        /// Guarda a descri��o do motivo de cancelamento
        /// </summary>
        [Size(150)]
        [RuleUniqueValue("MotivoCancelamento_TxDescricao_Unique", DefaultContexts.Save, "J� existe um motivo com esta descri��o.")]
        [RuleRequiredField("MotivoCancelamento_TxDescricao_Required", DefaultContexts.Save)]
        [Custom("Caption", "Descri��o")]
        public String TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value!= null)
                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value.Trim());
            }
        }

        /// <summary>
        /// Guarda o status do motivo de cancelamento
        /// </summary>
        [Custom("Caption", "Situa��o")]
        [VisibleInLookupListView(false)]
        [RuleRequiredField("MotivoCancelamento_CsSituacao_Required", DefaultContexts.Save)]
        public CsStatusMotivoCancelamento CsSituacao
        {
            get
            {
                return csSituacao;
            }
            set
            {
                SetPropertyValue<CsStatusMotivoCancelamento>("CsSituacao", ref csSituacao, value);
            }
        }
        #endregion

        #region Non Persistent Properties
        #endregion
        
        #region Bussiness Rules

        /// <summary>
        /// Fun��o que n�o permite deletar um motivo que est� sendo utilizado
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirDeletarSeMotivoAssociadoAssociada", DefaultContexts.Delete,
            "O Motivo de Cancelamento est� sendo usado por um Cancelamento de Ciclo.")]
        [NonPersistent, Browsable(false)]
        public bool RnNaoPermitirDeletarSeMotivoAssociado
        {
            get
            {
                return GetMotivoUsado(Session);
            }
        }

        #endregion

        #region New Instance
        #endregion

        #region Gets (DBQueries)

        /// <summary>
        /// Retorna se a situa��o est� sendo utilizada por alguma tarefa
        /// </summary>
        /// <param name="session">Sess�o</param>
        /// <returns>Sim se tiver, n�o sen�o</returns>
        private bool GetMotivoUsado(Session session)
        {
            ICollection collection = session.GetObjects(session.GetClassInfo<CicloDesenv>(),
                CriteriaOperator.Parse(String.Format("MotivoCancelamento = '{0}'", Oid)), null, 1, false, false);

            if (collection.Count > 0)
                return false;

            return true;
        }

        /// <summary>
        /// Resgata todos os motivos com status ativos cadastrados
        /// </summary>
        /// <param name="session">sess�o</param>
        /// <returns>cole��o de motivos ativos</returns>
        public static XPCollection<MotivoCancelamento> GetMotivosAtivos(Session session)
        {
            ICollection collection = session.GetObjects(session.GetClassInfo<MotivoCancelamento>(),
                CriteriaOperator.Parse(String.Format("CsSituacao = '{0}'", CsStatusMotivoCancelamento.Ativo)), null, 0, false, true);
    
            XPCollection<MotivoCancelamento> result = new XPCollection<MotivoCancelamento>(session, false);

            foreach (MotivoCancelamento ct in collection)
                result.Add(ct);

            return result;
        }

        /// <summary>
        /// Resgata todos os motivos com status inativos cadastrados
        /// </summary>
        /// <param name="session">sess�o</param>
        /// <returns>cole��o de motivos ativos</returns>
        public static XPCollection<MotivoCancelamento> GetMotivosInativos(Session session)
        {
            ICollection collection = session.GetObjects(session.GetClassInfo<MotivoCancelamento>(),
                CriteriaOperator.Parse(String.Format("CsSituacao = '{0}'", CsStatusMotivoCancelamento.Inativo)), null, 0, false, true);

            XPCollection<MotivoCancelamento> result = new XPCollection<MotivoCancelamento>(session, false);

            foreach (MotivoCancelamento ct in collection)
                result.Add(ct);

            return result;
        }
        #endregion

        #region Utils


        /// <summary>
        /// M�todo que retorna a descri��o do motivo para utiliza��o em outros objetos.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TxDescricao;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor de classe
        /// </summary>
        public MotivoCancelamento(Session session)
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
        /// Ap�s a constru��o
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    #endregion
    }

}