using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Execucao;

namespace WexProject.BLL.Models.Geral
{
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class ProjetoUltimoFiltro : BaseObject
    {

        #region Attributes
        /// <summary>
        /// Atributo que guarda a ultima opção de motivo de cancelamento de ciclo
        /// </summary>
        private MotivoCancelamento motivoCancelamentoCiclo;
        /// <summary>
        /// Projeto Pertencente a estas configurações
        /// </summary>
        private Projeto projeto;
        #endregion

        #region Properties
        /// <summary>
        /// Ultima opção de motivo de concelamento de ciclo (CancelamentoCicloForm)
        /// </summary>
        public MotivoCancelamento MotivoCancelamentoCiclo
        {
            get
            {
                return motivoCancelamentoCiclo;
            }
            set
            {
                SetPropertyValue<MotivoCancelamento>("LastMotivoCancelamentoCiclo", ref motivoCancelamentoCiclo, value);
            }
        }
        /// <summary>
        /// Projeto Pertencente a estas configurações
        /// </summary>
        public Projeto Projeto
        {
            get { return projeto; }
            set { SetPropertyValue<Projeto>("Projeto", ref projeto, value); }
        }
        #endregion

        #region BusinessRules

        public static bool RnSetUltimoMotivoCancelamento(Session session,Projeto projeto,MotivoCancelamento motivo) 
        {
            try
            {            
                if (projeto.UltimoFiltro != null) 
                {
                    if (projeto.UltimoFiltro.MotivoCancelamentoCiclo != null)
                    {
                        if (projeto.UltimoFiltro.MotivoCancelamentoCiclo.Oid != motivo.Oid)
                        {
                            projeto.UltimoFiltro.MotivoCancelamentoCiclo = motivo;
                        }
                    }
                    else
                    {
                        projeto.UltimoFiltro.MotivoCancelamentoCiclo = motivo;
                    }
                }
                else
                {
                    projeto.UltimoFiltro = new ProjetoUltimoFiltro(projeto.Session);
                    RnSetUltimoMotivoCancelamento(session,projeto, motivo);
                    return true;
                }

                session.UpdateSchema();
                session.CommitTransaction();
            }catch
            {
                return false;
            }
            return true;
        }

        public static ProjetoUltimoFiltro GetUltimoFiltroByProject(Session session, Projeto projeto) 
        {
            ProjetoUltimoFiltro ultimo_filtro = session.FindObject<ProjetoUltimoFiltro>(CriteriaOperator.Parse("Projeto=?", projeto.Oid));

            if (ultimo_filtro!=null)
            {
                return ultimo_filtro;
            }
            return default(ProjetoUltimoFiltro);
        }

        #endregion

        #region Constructors
        public ProjetoUltimoFiltro(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        #endregion
        
    }

}
