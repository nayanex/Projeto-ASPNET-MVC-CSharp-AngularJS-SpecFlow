using System;

using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Data.Filtering;

using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.Library.Libs.DataHora;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe Calendário
    /// </summary>
    [Custom("Caption", "Institucional > Calendario")]
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class Calendario : BaseObject
    {
        #region Attributes

        /// <summary>
        /// Data de Início do registro de calendário.
        /// </summary>
        private DateTime dtInicio;

        /// <summary>
        /// Data de Término do registro de calendário.
        /// </summary>
        private DateTime dtTermino;

        /// <summary>
        /// Define quando o calendário será considerado nos cronogramas.
        /// </summary>
        private CsVigenciaDomain csVigencia;

        /// <summary>
        /// Tipo de Calendário (Trabalho/Folga)
        /// </summary>
        private CsCalendarioDomain csCalendario;

        #endregion

        #region Properties
        /// <summary>
        /// Representa a descrição/justificativa para exceção ao planejamento padrão.
        /// </summary>
        [RuleRequiredField("PlanCalendario_TxDescricao_required", DefaultContexts.Save, Name = "Descrição")]
        [RuleUniqueValue("PlanCalendario_TxDescricao_unique", DefaultContexts.Save, Name = "Descrição")]
        public String TxDescricao { get; set; }

        /// <summary>
        /// Data de Início do registro de calendário.
        /// </summary>
        [AppearanceAttribute("Calendario_DtInicio_Appearance",
        Visibility = ViewItemVisibility.Hide,
        Criteria = "CsVigencia <> 'PorDiaMesAno'",
        TargetItems = "DtInicio"
        )]
        public DateTime DtInicio
        {
            get
            {
                if ((CsVigencia == CsVigenciaDomain.PorDiaMes) && (dtInicio == DateTime.MinValue))
                {
                    string txInicio = String.Format("{0}-{1}-{2}", DateUtil.ConsultarDataHoraAtual().Year, (int)CsMes, NbDia);
                    try
                    {
                        dtInicio = DateTime.Parse(txInicio);
                    }
                    catch
                    {
                        dtInicio = DateTime.MinValue;
                    }
                }
                return dtInicio;
            }
            set
            {
                SetPropertyValue("DtInicio", ref dtInicio, value);
            }
        }

        /// <summary>
        /// Data de Início do registro de calendário.
        /// </summary>
        [ImmediatePostData]
        [AppearanceAttribute("Calendario_Periodo_Appearance",
        Visibility = ViewItemVisibility.Hide,
        Criteria = "CsVigencia <> 'PorPeriodo'",
        TargetItems = "Periodo"
        )]
        public DateTime Periodo
        {
            get
            {
                return dtInicio;
            }
            set
            {
                SetPropertyValue("Periodo", ref dtInicio, value);
            }
        }

        /// <summary>
        /// Data de Término do registro de calendário.
        /// </summary>
        [AppearanceAttribute("Calendario_DtTermino_Appearance",
        Visibility = ViewItemVisibility.Hide,
        Criteria = "CsVigencia <> 'PorPeriodo'",
        TargetItems = "DtTermino"
        )]
        public DateTime DtTermino
        {
            get
            {
                return dtTermino;
            }
            set
            {
                if (dtTermino != value)
                    dtTermino = value.Date;
            }
        }

        /// <summary>
        /// Dia do mês.
        /// </summary>
        [AppearanceAttribute("Calendario_NbDia_Appearance",
        Visibility = ViewItemVisibility.Hide,
        Criteria = "CsVigencia <> 'PorDiaMes'",
        TargetItems = "DtTermino"
        )]
        public int NbDia { get; set; }

        /// <summary>
        /// Mês do Ano
        /// </summary>
        [AppearanceAttribute("Calendario_CsMes_Appearance",
        Visibility = ViewItemVisibility.Hide,
        Criteria = "CsVigencia <> 'PorDiaMes'",
        TargetItems = "DtTermino"
        )]
        public CsMesDomain CsMes { get; set; }

        /// <summary>
        /// Tipo de Calendário (Trabalho/Folga)
        /// </summary>
        [RuleRequiredField("Calendario_CsCalendario_required", DefaultContexts.Save, Name = "Tipo de Calendário")]
        public CsCalendarioDomain CsCalendario
        {
            get
            {
                return csCalendario;
            }
            set
            {
                SetPropertyValue("CsCalendario", ref csCalendario, value);
            }
        }

        /// <summary>
        /// Define quando o calendário será considerado nos cronogramas.
        /// </summary>
        [RuleRequiredField("Calendario_CsVigencia_required", DefaultContexts.Save, Name = "Vigência")]
        [ImmediatePostData]
        public CsVigenciaDomain CsVigencia
        {
            get
            {
                return csVigencia;
            }
            set
            {
                SetPropertyValue("CsVigencia", ref csVigencia, value);
            }
        }

        /// <summary>
        /// Situação (Ativo/Inativo).
        /// </summary>
        [RuleRequiredField("Calendario_CsSituacao_required", DefaultContexts.Save, Name = "Situação")]
        public CsSituacaoDomain CsSituacao { get; set; }

        #endregion

        #region NonPersistent Properties
        /// <summary>
        /// Atributo nao persistente para obter a string quando para exibir no grid.
        /// </summary>
        public string _TxQuando
        {
            get
            {
                string tx = "";
                switch (CsVigencia)
                {
                    case CsVigenciaDomain.PorDiaMes:
                        tx = string.Format("a cada {0}/{1}", NbDia, CsMes.GetHashCode());
                        break;
                    case CsVigenciaDomain.PorDiaMesAno:
                        tx = string.Format("{0:d}", DtInicio);
                        break;
                    case CsVigenciaDomain.PorPeriodo:
                        tx = string.Format("{0:d} a {1:d}", DtInicio, DtTermino);
                        break;
                }

                return tx;
            }
        }

        /// <summary>
        /// Valida a o dia do cadastro de dia/mês do calendario.
        /// </summary>
        [NonPersistent, VisibleInListView(false), VisibleInDetailView(false)]
        [RuleFromBoolProperty("PlanCalendarioCoorp_NbDia_Validacao", DefaultContexts.Save, CustomMessageTemplate = "O dia é inválido para cadastro.")]
        public bool _IsDiaValido
        {
            get
            {
                //if (!IsSaving) 
                //    return true;
                bool resultado = false;
                switch (CsVigencia)
                {
                    case CsVigenciaDomain.PorDiaMes:
                        if ((CsMes == CsMesDomain.Fevereiro) && (NbDia == 29))
                            //caso o dia 29 de fevereiro seja cadastro, ele sempre poderá ser adicionado.
                            resultado = true;
                        else
                        {
                            try
                            {
                                DateTime dtValidacao = DateTime.Parse(String.Format("{0}-{1}-{2}", DateUtil.ConsultarDataHoraAtual().Year, (int)CsMes, NbDia));
                                resultado = true;
                            }
                            catch
                            {
                                resultado = false;
                            }
                        }
                        break;
                    case CsVigenciaDomain.PorPeriodo:
                    case CsVigenciaDomain.PorDiaMesAno:
                    default:
                        resultado = true;
                        break;
                }
                return resultado;
            }
        }
        #endregion

        #region BusinessRules
        /*
                                        /// <summary>
                                        /// Pega todos os prrojetos da seção
                                        /// </summary>
                                        /// <returns>uma coleção de projetos</returns>
                                        [AppearanceAttribute("GetProjetos_Projeto_Appearance",
                                            Enabled = false,
                                            Criteria = "Projeto.DtInicioReal >= Periodo && Projeto.DtTerminoReal <= Periodo",
                                            TargetItems = "Projeto"),Browsable(false)]
                                        public XPCollection<Projeto> GetProjetos()
                                        {
                                            using (XPCollection<Projeto> collection = new XPCollection<Projeto>(Session))
                                            {
                                                if (collection.Count != 0)
                                                    return collection;
                                                else
                                                    return null;
                                            }
                                        }

                                        /// <summary>
                                        /// Recalcula a data final dos projetos
                                        /// </summary>
                                        public void RecalculaFinal()
                                        {
                                            if (GetProjetos() != null)
                                            {
                                                foreach (Projeto projeto in GetProjetos())
                                                {
                                                    projeto.RnCalcularTerminoReal();
                                                }
                                            }
                                        }*/

        /// <summary>
        /// Aplica regras de negócio ao salvar.
        /// </summary>
        protected override void OnSaving()
        {

            //RecalculaFinal();

            base.OnSaving();

        }

        /// <summary>
        /// Remove as calendarios vigentes ao remover um corporativo
        /// </summary>
        protected override void OnDeleting()
        {
            base.OnDeleting();
        }

        /// <summary>
        /// Verifica o calendário está com conflitos.
        /// </summary>
        /// <returns>Verdadeiro se o calendário não estiver conflitando 
        /// com nenhum outro    </returns>
        [RuleFromBoolProperty("CalendarioConflitante", DefaultContexts.Save,
        "Não poderá ser cadastrado calendários com períodos conflitantes.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarConflito
        {
            get
            {
                switch (CsVigencia)
                {
                    case CsVigenciaDomain.PorDiaMesAno:
                        if (GetCalendariosConflitantePorDiaMesAno() != null)
                            return false;
                        break;
                    case CsVigenciaDomain.PorDiaMes:
                        if (GetCalendariosConflitantePorDiaMes() != null)
                            return false;
                        break;
                    case CsVigenciaDomain.PorPeriodo:
                        if (GetCalendariosConflitantePorPeriodo() != null)
                            return false;
                        break;
                }
                return true;
            }
        }

        /// <summary>
        /// Verifica se o BOM atual está liberado ou não
        /// e bloqueia a exclusão se o bom estiver liberado
        /// </summary>
        [RuleFromBoolProperty("CalendarioVigenciaInformada", DefaultContexts.Save,
        "É necessário informar dados válidos para a vigência do calendário.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificarVigenciaInformada
        {
            get
            {
                bool resultado = true;
                if (CsVigencia == CsVigenciaDomain.PorDiaMes &&
                (NbDia == 0 || CsMes == 0))
                {
                    resultado = false;
                }
                else
                    if (CsVigencia == CsVigenciaDomain.PorDiaMesAno &&
                    ( Periodo == null || Periodo == new DateTime()))
                    {
                        resultado = false;
                    }
                    else
                        if (CsVigencia == CsVigenciaDomain.PorPeriodo &&
                        ((DtInicio == null || DtInicio == DateTime.MinValue) ||
                        (DtTermino == null || DtTermino == DateTime.MinValue)))
                        {
                            resultado = false;
                        }

                if (resultado && CsVigencia == CsVigenciaDomain.PorPeriodo &&
                (DtInicio > DtTermino || DtTermino < DtInicio) )
                    resultado = false;

                return resultado;
            }
        }

        #endregion

        #region NewInstance        

        #endregion

        #region DBQueries (Gets)

        #region GetCalendariosConflitantePorPeriodo
        /// <summary>
        /// Retorna os calendários que estão conflitando com o calendário atual.
        /// </summary>
        /// <returns>Lista de calendários conflitantes</returns>
        public XPCollection<Calendario> GetCalendariosConflitantePorPeriodo()
        {
            /**
             * Conflito 1 - Intercecao pela esquerda. Ex: [25/01 - 03/02] => Novo e [01/02 - 10/02] => Existente 
             * Conflito 2 - Intercecao pela direita. Ex: [05/02 - 20/02] => Novo e [01/02 - 10/02] => Existente 
             * Conflito 3 - Intercecao pelos extremos. Ex: [30/01 - 20/02] => Novo e [01/02 - 10/02] => Existente 
             * Conflito 4 - Intercecao interna. Ex: [02/02 - 09/02] => Novo e [01/02 - 10/02] => Existente 
             */
            using (XPCollection<Calendario> collection = new XPCollection<Calendario>(
            Session, CriteriaOperator.Parse(
            "((((? < DtInicio And ? >= DtInicio And ? < DtTermino) Or " // conflito 1
            +
            "  (? > DtInicio And ? <= DtTermino And ? > DtTermino) Or " // conflito 2
            +
            "  (? < DtInicio And ? > DtTermino) Or " // conflito 3
            +
            "  (? >= DtInicio And ? <= DtTermino)) And " // conflito 4
            +
            " (CsVigencia = ?)) Or " +
            "(DtInicio <= ? And DtTermino >= ? And CsVigencia = ?)) And " +
            "(Oid != ? And CsSituacao = ?)",
            DtInicio.Date, DtTermino.Date, DtTermino.Date,
            DtInicio.Date, DtInicio.Date, DtTermino.Date,
            DtInicio.Date, DtTermino.Date,
            DtInicio.Date, DtTermino.Date,
            (int)CsVigenciaDomain.PorPeriodo,
            DtInicio.Date, DtInicio.Date, (int)CsVigenciaDomain.PorDiaMesAno,
            Oid, CsSituacaoDomain.Ativo)))
            {
                try
                {
                    if (collection.Count != 0)
                        return collection;
                    else
                        return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        #endregion

        /// <summary>
        /// Retorna os calendários que estão conflitando com o calendário atual por dia, mês e ano.
        /// </summary>
        /// <returns>Lista de calendários conflitantes</returns>
        public XPCollection<Calendario> GetCalendariosConflitantePorDiaMesAno()
        {

            using (XPCollection<Calendario> collection = new XPCollection<Calendario>(
            Session, CriteriaOperator.Parse("((DtInicio = ? And CsVigencia = ?) Or " +
            " (? >= DtInicio  And ? <= DtTermino And CsVigencia = ?)) And " +
            "(Oid != ? And CsSituacao = ?)", DtInicio.Date, (int)CsVigenciaDomain.PorDiaMesAno,
            DtInicio.Date, DtInicio.Date,
            (int)CsVigenciaDomain.PorPeriodo,
            Oid, CsSituacaoDomain.Ativo)))
            {
                try
                {
                    if (collection.Count != 0)
                        return collection;
                    else
                        return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Retorna os calendários que estão conflitando com o calendário atual por dia e mês.
        /// </summary>
        /// <returns>Lista de calendários conflitantes</returns>
        public XPCollection<Calendario> GetCalendariosConflitantePorDiaMes()
        {
            using (XPCollection<Calendario> collection = new XPCollection<Calendario>(
            Session, CriteriaOperator.Parse(String.Format(
            "(NbDia = {0} And CsMes = {1} And CsVigencia = {2}) And " +
            "(Oid != ? And CsSituacao = ?)",
            NbDia, (int)CsMes,
            (int)CsVigenciaDomain.PorDiaMes),
            Oid, CsSituacaoDomain.Ativo)))
            {
                try
                {
                    if (collection.Count != 0)
                        return collection;
                    else
                        return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Método para obter a lista de calendários coorporativos da GBR.
        /// </summary>
        /// <returns>a lista de objetos.</returns>
        public XPCollection<Calendario> GetCalendarios()
        {
            using (XPCollection<Calendario> collection = new XPCollection<Calendario>(
            Session, CriteriaOperator.Parse("CsSituacao = ?", CsSituacaoDomain.Ativo)))
            {
                if (collection.Count != 0)
                    return collection;
                else
                    return null;
            }
        }

        /// <summary>
        /// Verifica se a dataHora é util ou não para a GBR        
        /// </summary>
        /// <param name="session">sessão atual</param>
        /// <param name="data">dataHora</param>
        /// <returns>primeiro obj da coleção</returns>
        public static Calendario GetCalendarioData(Session session, DateTime data)
        {
            using (XPCollection<Calendario> collection =
            new XPCollection<Calendario>(session, CriteriaOperator.Parse(
            "(((DtInicio = ? AND CsVigencia = ?) OR " +
            "(? >= DtInicio  AND ? <= DtTermino AND CsVigencia = ?) OR " +
            "(NbDia = ? AND CsMes = ? AND CsVigencia = ?)" +
            ") AND CsSituacao = ?)",
            data.Date, (int)CsVigenciaDomain.PorDiaMesAno,
            data.Date, data.Date, (int)CsVigenciaDomain.PorPeriodo,
            data.Day, data.Month, (int)CsVigenciaDomain.PorDiaMes,
            CsSituacaoDomain.Ativo)))
            {
                if (collection.Count > 0)
                    return collection[0];
                else
                    return null;
            }

        }

        /// <summary>
        /// Verifica se o dia não é um dia de folga nos trabalhos da FPF
        /// </summary>
        /// <param name="session">Sessão do cronograma</param>
        /// <param name="data">Data que deseja ser verificado como folga</param>
        /// <returns>verdadeiro se for folga e falso se não houver nada no dia</returns>
        public static bool GetFeriadoCalendario(Session session, DateTime data)
        {
            using (XPCollection<Calendario> collection =
            new XPCollection<Calendario>(session, CriteriaOperator.Parse(
            "(((DtInicio = ? AND CsVigencia = ?) OR " +
            "(? >= DtInicio  AND ? <= DtTermino AND CsVigencia = ?) OR " +
            "(NbDia = ? AND CsMes = ? AND CsVigencia = ?)" +
            ") AND CsSituacao = ?)" + " AND CsCalendario = ?",
            data.Date, (int)CsVigenciaDomain.PorDiaMesAno,
            data.Date, data.Date, (int)CsVigenciaDomain.PorPeriodo,
            data.Day, data.Month, (int)CsVigenciaDomain.PorDiaMes,
            CsSituacaoDomain.Ativo , CsCalendarioDomain.Folga)))
            {
                if (collection.Count > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region Utils

        /// <summary>
        /// Armazena todos os Cronogramas q estão abertos
        /// </summary>
        private static List<int> listViewsActive;

        /// <summary>
        /// Adiciona uma instancia de Cronograma na lista
        /// </summary>
        public static void AddListViewsActive()
        {
            if (listViewsActive == null)
            {
                listViewsActive = new List<int>();
            }
            listViewsActive.Add(0);
        }

        /// <summary>
        /// Remove uma instancia de Cronograma na lista
        /// </summary>
        public static void RemoveListViewsActive()
        {
            if (listViewsActive != null && listViewsActive.Count > 0)
            {
                listViewsActive.RemoveAt(0);
            }
        }

        /// <summary>
        /// Retorna true se houver algum cronograma aberto
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsListViewActive()
        {
            return listViewsActive != null && listViewsActive.Count > 0;
        }

        /// <summary>
        /// Regra para validar a deleção do Calendário
        /// </summary>
        [RuleFromBoolProperty("CalendarioNaoExcluirComPlan", DefaultContexts.Delete,
        "É necessário fechar todos os cronogramas abertos para excluir um calendário.")]
        [NonPersistent, Browsable(false)]
        public bool RnVerificaBOMLiberado
        {
            get
            {
                return !IsListViewActive();
            }
        }

        /// <summary>
        /// Verifica se a dataHora é util ou não para a GBR tendo em vista o calendario planejado da organização
        /// sábados e domingos.
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="data">dataHora a ser validada</param>
        /// <param name="fimSemana">fim de semana como dia util</param>
        /// <returns>true - dia util/ false - folga</returns>
        public static bool IsDiaUtil(Session session, DateTime data, bool fimSemana = false)
        {

            bool resultado = true;
            CriteriaOperator consulta;

            if (!fimSemana)
            {
                switch (data.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        resultado = false;
                        break;
                }
            }

            if (resultado)
                consulta = CriteriaOperator.Parse("((DtInicio = ? AND CsVigencia = ?) OR " +
                "(? >= DtInicio  AND ? <= DtTermino AND CsVigencia = ?) OR " +
                "(NbDia = ? AND CsMes = ? AND CsVigencia = ?)" +
                ") AND CsSituacao = ?  AND CsCalendario = ?",
                data.Date, (int)CsVigenciaDomain.PorDiaMesAno,
                data.Date, data.Date, (int)CsVigenciaDomain.PorPeriodo,
                data.Day, data.Month, (int)CsVigenciaDomain.PorDiaMes,
                (int)CsSituacaoDomain.Ativo, (int)CsCalendarioDomain.Folga);

            else
                consulta = CriteriaOperator.Parse("((DtInicio = ? AND CsVigencia = ?) OR " +
                "(? >= DtInicio  AND ? <= DtTermino AND CsVigencia = ?) OR " +
                "(NbDia = ? AND CsMes = ? AND CsVigencia = ?)" +
                ") AND CsSituacao = ?  AND CsCalendario = ?",
                data.Date, (int)CsVigenciaDomain.PorDiaMesAno,
                data.Date, data.Date, (int)CsVigenciaDomain.PorPeriodo,
                data.Day, data.Month, (int)CsVigenciaDomain.PorDiaMes,
                (int)CsSituacaoDomain.Ativo, (int)CsCalendarioDomain.Trabalho);

            int total = (int)session.Evaluate(typeof(Calendario),
            CriteriaOperator.Parse("Count()"), consulta);

            if (total > 0)
                return !resultado;
            else
                return resultado;
        }

        /// <summary>
        /// busca a dataHora util mais próxima com base no calendario GBR.
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="data">dataHora para a pesquisa</param>
        /// <returns>dataHora util</returns>
        public static DateTime ProximaDataUtil(Session session, DateTime data)
        {
            while (!IsDiaUtil(session, data))
            {
                data = data.AddDays(1);
            }
            return data.Date;
        }

        /// <summary>
        /// Acréscimo de dias úteis à uma data
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="data">Data</param>
        /// <param name="quantidade">Quantidade de dias a acescentar</param>
        /// <returns>Data Final</returns>
        public static DateTime AcrescimoDiasUteisData(Session session, DateTime data, ushort quantidade)
        {
            DateTime dataFinal = data;

            if (quantidade <= 0)
            {
                return ProximaDataUtil(session, dataFinal.AddDays(1));
            }

            quantidade--; // para contar a data atual

            while (quantidade > 0)
            {
                dataFinal = ProximaDataUtil(session, dataFinal.AddDays(1));
                quantidade--;
            }

            return dataFinal;
        }

        /// <summary>
        /// Primeiro dia util antes de uma data
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="data">Data</param>
        /// <returns>Primeiro dia util antes da data</returns>
        public static DateTime PrimeiroDiaUtilAnteriorData(Session session, DateTime data)
        {
            do
            {
                data = data.AddDays(-1);
            } while (!IsDiaUtil(session, data));

            return data.Date;
        }

        /// <summary>
        /// Retorna a quantidade de dias uteis entre duas datas, considerando o calendario GBR
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="stData">dataHora de inicio</param>
        /// <param name="ndData">dataHora final</param>
        /// <returns>total de dias uteis</returns>
        public static int TotalDiasUteis(Session session, DateTime stData, DateTime ndData)
        {
            int total = 0; //total de dias entre as datas
            //int contFinalSemana = 0; int contSemana = 0; //contadores de datas
            //string sSemana = null, sFinalSemana = null; //strings de consulta
            DateTime dtInicio, dtFinal;

            if ((ndData == DateTime.MinValue) || (stData == DateTime.MinValue))
                return -1;

            if (stData.Date == ndData.Date)
                return 0; //caso as datas sejam identicas, o metodo retorna 0
            else
            {
                if (stData > ndData)
                {
                    dtInicio = ndData;
                    dtFinal = stData;
                }
                else
                {
                    dtFinal = ndData;
                    dtInicio = stData;
                }
                total = dtFinal.Date.Subtract(dtInicio.Date).Days;
            }

            #region Removendo Finais de Semana
            //reduz os sabados e domingos do período  (jonathas.barroso)    
            int resto = total % 7; //contando dias de semanas incompletas      
            int semanas = (total - resto) / 7; //conta as semanas completas  

            if (resto != 0) //verifica os fins de semana da semana incompleta
            {
                int i = 0; //contador do laço
                int limite = resto; //total de laços que devem ser executados
                DateTime data = dtInicio; //start

                while (i < limite)
                {
                    switch (data.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            resto--;
                            break;
                    }
                    i++;
                    data = data.AddDays(1);
                }
            }

            total = (semanas * 5) + resto;//5 dias para cada semana + dias de semana incompleta
            #endregion

            #region BuscaCalendariosFolga
            //por dia/mês
            string filtro = String.Format("CsSituacao = {0} And CsCalendario = {2} And CsVigencia = {1}",
            (int)CsSituacaoDomain.Ativo, (int)CsVigenciaDomain.PorDiaMes, (int)CsCalendarioDomain.Folga);
            using (XPCollection<Calendario> calendarios = new XPCollection<Calendario>(session, CriteriaOperator.Parse(filtro)))
            {
                foreach (Calendario calendario in calendarios)
                {
                    for (int i = dtInicio.Year; i <= dtFinal.Year; i++)
                    {
                        try //tenta criar a dataHora do calendario
                        {
                            calendario.DtInicio = DateTime.Parse(String.Format("{0}-{1}-{2}", i, calendario.CsMes, calendario.NbDia));
                        }
                        catch //caso dê erro, ele não calcula 
                        {
                            calendario.DtInicio = DateTime.MinValue;
                        }

                        if ((!calendario.DtInicio.Equals(DateTime.MinValue)) &&
                        (dtFinal >= calendario.DtInicio) &&
                        (calendario.DtInicio >= dtInicio))
                            //se a dataHora estiver incluida no período, é abatido do total
                            total--;
                    }
                }
            }

            //por dia/mês/ano
            filtro = String.Format("CsSituacao = {0} And CsVigencia = {1} And CsCalendario = {4} And (DtInicio >= '{2:yyyy-MM-dd}') And (DtInicio <= '{3:yyyy-MM-dd}')",
            (int)CsSituacaoDomain.Ativo, (int)CsVigenciaDomain.PorDiaMesAno, dtInicio, dtFinal, (int)CsCalendarioDomain.Folga);
            object calendDiaMesAno = session.Evaluate(typeof(Calendario), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse(filtro));

            total -= (int)calendDiaMesAno;

            //gerando a consulta
            filtro = String.Format("CsSituacao = {0} And CsCalendario = {4} And CsVigencia = {1} And (('{2:yyyy-MM-dd}' < DtInicio And '{2:yyyy-MM-dd}' >= DtInicio And '{3:yyyy-MM-dd}' < DtTermino) Or " +
            "('{2:yyyy-MM-dd}' > DtInicio And '{3:yyyy-MM-dd}' <= DtTermino And '{3:yyyy-MM-dd}' > DtTermino) Or " +
            "('{2:yyyy-MM-dd}' < DtInicio And '{3:yyyy-MM-dd}' > DtTermino) Or " +
            "('{2:yyyy-MM-dd}' >= DtInicio And '{3:yyyy-MM-dd}' <= DtTermino))",
            (int)CsSituacaoDomain.Ativo, (int)CsVigenciaDomain.PorPeriodo, dtInicio, dtFinal, (int)CsCalendarioDomain.Folga);

            // por periodo
            using (XPCollection<Calendario> collection = new XPCollection<Calendario>(session, CriteriaOperator.Parse(filtro)))
            {
                foreach (Calendario calendario in collection)
                    //verifica as datas do período do calendario para o período informado
                    for (DateTime i = calendario.DtInicio; i <= calendario.DtTermino; i = i.AddDays(1))
                        //pois o calendario pode conter datas fora deste período
                        if ((dtFinal >= i) && (i >= dtInicio))
                            total--;
            }

            #endregion

            #region BuscaCalendariosTrabalho
            // por dia/mês
            filtro = String.Format("CsSituacao = {0} And CsCalendario = {2} And CsVigencia = {1}",
            (int)CsSituacaoDomain.Ativo, (int)CsVigenciaDomain.PorDiaMes, (int)CsCalendarioDomain.Trabalho);
            using (XPCollection<Calendario> calendarios = new XPCollection<Calendario>(session, CriteriaOperator.Parse(filtro)))
            {
                foreach (Calendario calendario in calendarios)
                {
                    for (int i = dtInicio.Year; i <= dtFinal.Year; i++)
                    {
                        try //tenta criar a dataHora do calendario
                        {
                            calendario.DtInicio = DateTime.Parse(String.Format("{0}-{1}-{2}", i, calendario.CsMes, calendario.NbDia));
                        }
                        catch //caso dê erro, ele não calcula 
                        {
                            calendario.DtInicio = DateTime.MinValue;
                        }

                        if ((!calendario.DtInicio.Equals(DateTime.MinValue)) &&
                        (dtFinal >= calendario.DtInicio) &&
                        (calendario.DtInicio >= dtInicio))
                            //se a dataHora estiver incluida no período, é abatido do total
                            total++;
                    }
                }
            }
            //por dia/mês/ano
            filtro = String.Format("CsSituacao = {0} And CsVigencia = {1} And CsCalendario = {4} And " +
            "(DtInicio >= '{2:yyyy-MM-dd}') And (DtInicio <= '{3:yyyy-MM-dd}')",
            (int)CsSituacaoDomain.Ativo, (int)CsVigenciaDomain.PorDiaMesAno, dtInicio, dtFinal, (int)CsCalendarioDomain.Trabalho);
            calendDiaMesAno = session.Evaluate(typeof(Calendario), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse(filtro));

            total += (int)calendDiaMesAno;

            // por periodo
            filtro = String.Format("CsSituacao = {0} And CsCalendario = {4} And CsVigencia = {1} And (" +
            "('{2:yyyy-MM-dd}' < DtInicio And '{2:yyyy-MM-dd}' >= DtInicio And '{3:yyyy-MM-dd}' < DtTermino) Or " +
            "('{2:yyyy-MM-dd}' > DtInicio And '{3:yyyy-MM-dd}' <= DtTermino And '{3:yyyy-MM-dd}' > DtTermino) Or " +
            "('{2:yyyy-MM-dd}' < DtInicio And '{3:yyyy-MM-dd}' > DtTermino) Or " +
            "('{2:yyyy-MM-dd}' >= DtInicio And '{3:yyyy-MM-dd}' <= DtTermino))",
            (int)CsSituacaoDomain.Ativo, (int)CsVigenciaDomain.PorPeriodo, dtInicio, dtFinal, (int)CsCalendarioDomain.Trabalho);
            using (XPCollection<Calendario> collection = new XPCollection<Calendario>(session, CriteriaOperator.Parse(filtro)))
            {
                foreach (Calendario calendario in collection)
                    //verifica as datas do período do calendario para o período informado
                    for (DateTime i = calendario.DtInicio; i <= calendario.DtTermino; i = i.AddDays(1))
                        //pois o calendario pode conter datas fora deste período 
                        if ((dtFinal >= i) && (i >= dtInicio))
                            total++;
            }
            #endregion

            return total;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public Calendario(Session session)
            : base(session)
        {

        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CsSituacao = CsSituacaoDomain.Ativo;
            CsVigencia = CsVigenciaDomain.PorDiaMes;
            CsCalendario = CsCalendarioDomain.Folga;
        }
        #endregion
    }

}
