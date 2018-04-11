using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Execucao;
using DevExpress.Data.Filtering;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Library.Libs.Str;

namespace WexProject.BLL.Models.Geral
{

    /// <summary>
    /// Classe criada para cadastro dos projetos.
    /// </summary>
    [Custom("Caption", "Projeto > Projeto")]
    [DefaultClassOptions]
    [OptimisticLocking( false )]
    public class Projeto : BaseObject
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
        /// Atributo de NbTamanhoTotal
        /// </summary>
        private UInt32 nbTamanhoTotal;

        /// <summary>
        /// Atributo de DtInicioPlan
        /// </summary>
        private DateTime dtInicioPlan;

        /// <summary>
        /// Atributo de DtTerminoPlan
        /// </summary>
        private DateTime dtTerminoPlan;

        /// <summary>
        /// Atributo de DtInicioReal
        /// </summary>
        private DateTime dtInicioReal;

        /// <summary>
        /// Atributo de NbCicloTotalPlan
        /// </summary>
        private UInt16 nbCicloTotalPlan;

        /// <summary>
        /// Atributo de NbCicloDuracaoDiasPlan
        /// </summary>
        private UInt16 nbCicloDuracaoDiasPlan;

        /// <summary>
        /// Atributo de NbCicloDiasIntervalo
        /// </summary>
        private UInt16 nbCicloDiasIntervalo;

        /// <summary>
        /// Atributo de CsProjetoSituacaoDomain
        /// </summary>
        private CsProjetoSituacaoDomain _txSituacaoProjeto;

        /// <summary>
        /// Atributo que guarda o valor da dataHora final do projeto
        /// </summary>
        private DateTime dtTermino;

        /// <summary>
        /// Variável que guarda o valor antigo da dataHora de início
        /// </summary>
        [Browsable(false)]
        private DateTime oldDate;

        /// <summary>
        /// Atributo de DtTerminoReal
        /// </summary>
        private DateTime dtTerminoReal;

        /// <summary>
        /// Variável que guarda o valor antigo do total de ciclos
        /// </summary>
        [Browsable(false)]
        private UInt16 nbCicloTotalPlanOld;

        /// <summary>
        /// Variável que verifica se a RN de validação para deletar os ciclos está válida
        /// </summary>
        private bool verificacaoValida;

        /// <summary>
        /// Variável que verifica se acesso é pela tela de projeto ou de ciclo
        /// </summary>
        private Boolean acessoTelaProjeto;

        /// <summary>
        /// Variável guarda o total de pontos antigo do projeoto
        /// </summary>
        [Browsable(false)]
        private UInt32 nbTamanhoTotalOld;

        /// <summary>
        /// Último Filtro usado no Projeto
        /// </summary>
        private ProjetoUltimoFiltro ultimoFiltro;

        /// <summary>
        /// Projeto Old
        /// </summary>
        private Projeto projetoOld;

        /// <summary>
        /// Valor do Ritmo do Time
        /// </summary>
        private UInt16 nbRitmoTime;

        #endregion

        #region Properties
        /// <summary>
        /// Propriedade que faz a verificação a seguir :
        /// Ao alterar o tamanho de um Projeto, esse precisa ser sempre maior ou igual
        /// a somatória dos tamanhos dos módulos
        /// </summary>
        [RuleFromBoolProperty("ModificarTamanhoProjetoMenorSomaModulos", DefaultContexts.Save, " O tamanho do projeto deve ser maior ou igual a somatória dos tamanhos dos módulos")]
        [NonPersistent, Browsable(false)]
        public bool ModificarTamanhoProjetoMenorSomaModulos
        {
            get
            {
                int soma = 0;

                if (this.Modulos != null && this.Modulos.Count > 0)
                {
                    foreach (Modulo mod in this.Modulos)
                    {
                        if (mod.Filhos.Count == 0)
                            soma += (int)mod.NbPontosTotal;
                    }

                }

                return this.NbTamanhoTotal >= soma;
            }
        }

        /// <summary>
        /// Cria uma variável de projeto selecionado
        /// </summary>
        public static Guid SelectedProject { get; set; }

        /// <summary>
        /// Variavel que guarda o nome do projeto.
        /// </summary>
        [RuleUniqueValue("Projeto_TxNome_Unique", DefaultContexts.Save)]
        [RuleRequiredField("Projeto_TxNome_Required", DefaultContexts.Save, "Informe um nome para o projeto!")]
        public String TxNome
        {
            get
            {
                return txNome;
            }
            set
            {
                if (value != null)
                {
                    txNome = StrUtil.RetirarEspacoVazio(txNome);
                    SetPropertyValue<String>("TxNome", ref txNome, value.Trim());
                }
            }
        }

        /// <summary>
        /// Importação dos clientes do projeto
        /// </summary>
        [RuleRequiredField("Projeto_EmpresaInstituicao_Required", DefaultContexts.Save, "Selecione uma Empresa/Instituição!")]
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
        /// Variável que guarda o total de pontos do projeto
        /// </summary>
        public UInt32 NbTamanhoTotal
        {
            get
            {
                return nbTamanhoTotal;
            }
            set
            {
                SetPropertyValue<UInt32>("NbTamanhoTotal", ref nbTamanhoTotal, value);
            }
        }

        /// <summary>
        /// Propriedade que guarda a dataHora inicial planejada para o projeto
        /// </summary>
        [RuleRequiredField("Projeto_DtInicioPlan_Required", DefaultContexts.Save, "Informe uma dataHora de início planejado")]
        public DateTime DtInicioPlan
        {
            get
            {
                return dtInicioPlan;
            }
            set
            {
                SetPropertyValue<DateTime>("DtInicioPlan", ref dtInicioPlan, value);
            }
        }

        /// <summary>
        /// Proprieade que guarda a dataHora de inicio real do projeto
        /// </summary>
        public DateTime DtInicioReal
        {
            get
            {
                return dtInicioReal;
            }
            set
            {
                bool alterado = dtInicioReal != value;

                if (value != null)
                {
                    SetPropertyValue<DateTime>("DtInicioReal", ref dtInicioReal, value);
                    if (alterado && !IsLoading && !IsDeleted)
                    {
                        RnCalcularTerminoReal();
                    }
                }
            }
        }

        /// <summary>
        /// Proprieade que guarda a dataHora final planejada para o projeto
        /// </summary>
        [RuleRequiredField("Projeto_DtTerminoPlan_Required", DefaultContexts.Save, "Informe uma dataHora de término planejado")]
        public DateTime DtTerminoPlan
        {
            get
            {
                return dtTerminoPlan;
            }
            set
            {
                SetPropertyValue<DateTime>("DtTerminoPlan", ref dtTerminoPlan, value);
            }
        }

        /// <summary>
        /// Propriedade que guarda o total de ciclos planejados
        /// </summary>
        public UInt16 NbCicloTotalPlan
        {
            get
            {
                return nbCicloTotalPlan;
            }
            set
            {
                bool alterado = nbCicloTotalPlan != value;

                SetPropertyValue<UInt16>("NbCicloDuracaoTotalPlan", ref nbCicloTotalPlan, value);
                if (alterado && !IsLoading && !IsDeleted && !IsSaving)
                {
                    RnCalcularTerminoReal();
                }
            }
        }

        /// <summary>
        /// Propriedade que guarda o valor de duração em dias de um ciclo
        /// </summary>
        public UInt16 NbCicloDuracaoDiasPlan
        {
            get
            {
                return nbCicloDuracaoDiasPlan;
            }
            set
            {
                bool alterado = nbCicloDuracaoDiasPlan != value;

                SetPropertyValue<UInt16>("NbCicloDuracaoDiasPlan", ref nbCicloDuracaoDiasPlan, value);
                if (alterado && !IsLoading && !IsDeleted)
                {
                    RnCalcularTerminoReal();
                }
            }
        }

        /// <summary>
        /// Propriedade que guarda o valor de intervalos em dias entre os ciclos
        /// </summary>
        public UInt16 NbCicloDiasIntervalo
        {
            get
            {
                return nbCicloDiasIntervalo;
            }
            set
            {
                bool alterado = nbCicloDiasIntervalo != value;

                SetPropertyValue<UInt16>("NbCicloDiasIntervalo", ref nbCicloDiasIntervalo, value);
                if (alterado && !IsLoading && !IsDeleted)
                {
                    RnCalcularTerminoReal();

                }
            }
        }

        /// <summary>
        /// Propriedade que armazena a dataHora de término real do projeto
        /// </summary>
        public DateTime DtTerminoReal
        {
            get
            {
                return dtTerminoReal;
            }
            set
            {
                SetPropertyValue<DateTime>("DtTerminoReal", ref dtTerminoReal, value);
            }
        }

        [Browsable(false)]
        public ProjetoUltimoFiltro UltimoFiltro
        {
            get { return ultimoFiltro; }
            set { SetPropertyValue<ProjetoUltimoFiltro>("UltimoFiltro", ref ultimoFiltro, value); }
        }

        /// <summary>
        /// Associação com a classe ProjetoParteInteressada
        /// </summary>
        [Association("ProjetoParteInteressada", typeof(ProjetoParteInteressada)), Aggregated]
        public XPCollection<ProjetoParteInteressada> ProjetoParteInteressada
        {
            get
            {
                return GetCollection <ProjetoParteInteressada>("ProjetoParteInteressada");
            }
        }


        /// <summary>
        /// Associação de Módulos
        /// </summary>
        [Association("Modulos", typeof(Modulo)), Aggregated]
        public XPCollection Modulos
        {
            get
            {
                return GetCollection("Modulos");
            }
        }

        /// <summary>
        /// Associação com a classe ProjetoCliente
        /// </summary>
        [RuleRequiredField("Projeto_Cliente_Required", DefaultContexts.Save, "Informe um cliente para o projeto")]
        [Association("ProjetoCliente", typeof(ProjetoCliente)), Aggregated]
        public XPCollection Cliente
        {
            get
            {
                return GetCollection("Cliente");
            }
        }

        /// <summary>
        /// Associação com ParteInteressada
        /// </summary>
        [Association("ParteInteressada", typeof(ParteInteressada)), Aggregated, Browsable(false)]
        public XPCollection ParteInteressada
        {
            get
            {
                return GetCollection("ParteInteressada");
            }
        }

        /// <summary>
        /// Associação com a classe CicloDesenv
        /// </summary>
        [Association("ProjetoCicloDesenv", typeof(CicloDesenv)), Aggregated, Browsable(false)]
        public XPCollection<CicloDesenv> Ciclos
        {
            get
            {
                return GetCollection<CicloDesenv>("Ciclos");
            }
        }

        /// <summary>
        /// Valor do Ritmo do Time
        /// </summary>
        [Custom("Caption", "Ritmo Atual")]
        [RuleValueComparison("Projeto_RitmoTime_RuleValueComparison", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, Name = "Ritmo Atual")]
        public UInt16 NbRitmoTime
        {
            get
            {
                return nbRitmoTime;
            }
            set
            {
                SetPropertyValue<UInt16>("NbRitmoTime", ref nbRitmoTime, value);
            }
        }

        #endregion

        #region NonPersistentProperties
        /// <summary>
        /// método que identifica se o usuário está 
        /// acessando pela tela de projeto ou ciclo
        /// </summary>
        [NonPersistent, Browsable(false)]
        public Boolean _AcessoTelaProjeto
        {
            get
            {
                return acessoTelaProjeto;
            }
            set
            {
                SetPropertyValue<Boolean>("_AcessoTelaProjeto", ref acessoTelaProjeto, value);
            }

        }
        /// <summary>
        /// Propriedade não persistente que calcula o percentual concluído do projeto
        /// </summary>
        [NonPersistent]
        public Double _NbPerConcluido
        {
            get
            {
                double somatorioEstorias = Estoria.GetSomaDasEstoriasProntas(Session, this);
                double total;

                if (NbTamanhoTotal != 0)
                {
                    total = (somatorioEstorias * 100) / NbTamanhoTotal;
                    total = Math.Round(total, 2);
                    return total;

                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Propriedade não persistente que guarda a dataHora final real do projeto
        /// </summary>
        [NonPersistent]
        public DateTime _DtTerminoReal
        {
            get
            {
                return dtTermino;
            }
            set
            {
                SetPropertyValue<DateTime>("_DtTerminoReal", ref dtTermino, value);
            }
        }

        /// <summary>
        /// Propriedade não persistente que informa a situação do Projeto
        /// </summary>
        [NonPersistent]
        public CsProjetoSituacaoDomain _TxSituacaoProjeto
        {
            get
            {
                return _txSituacaoProjeto;
            }
        }
        #endregion

        #region BusinessRules
        /// <summary>
        /// OnSaving
        /// </summary>
        protected override void OnSaving()
        {
            //verifica se está acessando pela tela de projeto ou modulo
            _AcessoTelaProjeto = true;
            
            RnCalcularTerminoReal();
            DtTerminoReal = _DtTerminoReal;
            oldDate = DtInicioReal;
            nbCicloTotalPlanOld = NbCicloTotalPlan;
            base.OnSaving();
        }
        /// <summary>
        /// Na deleção ocorrer deleção de partes interessadas assosciadas ao projeto.
        /// </summary>
        protected override void OnDeleting()
        {
            // Ativando o delete quando remover da coleção
            ProjetoParteInteressada.DeleteObjectOnRemove = true;
            while (ProjetoParteInteressada.Count > 0)
                ProjetoParteInteressada.Remove(ProjetoParteInteressada[0]);

            base.OnDeleting();
        }

        /// <summary>
        /// Não permite deletar um projeto se ele tiver associações
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirDeletarProjetoSeTiverAssociacoes", DefaultContexts.Delete, "O Projeto possui associação(ões)!")]
        [NonPersistent, Browsable(false)]
        public bool RnDeletarProjetoSemAssociacao
        {
            get
            {
                return !(Modulos.Count > 0);
            }
        }

        /// <summary>
        /// Regra de negócio que valida se a dataHora de termino planejado é menor que a dataHora de inicio
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirSalvarDtInicioMenorDtTermino", DefaultContexts.Save, InvertResult = true,
        CustomMessageTemplate = "A dataHora final deve ser maior que a dataHora inicial")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarDtTerminoMaiorDtInicio
        {
            get
            {
                return DtInicioPlan > DtTerminoPlan;
            }
        }

        /// <summary>
        /// Regra de negócio que valida se o numero de ciclos é maior ou igual a 2
        /// </summary>
        [RuleFromBoolProperty("ValidarNumeroCiclos", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Pelo menos um ciclo deve existir")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarNumeroCiclos
        {
            get
            {
                return NbCicloTotalPlan >= 1;
            }
        }

        /// <summary>
        /// Regra de negócio que valida se a duração de um ciclo é maior ou igual a 10
        /// </summary>
        [RuleFromBoolProperty("ValidarDuracaoCiclos", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "A duração de um ciclo deve ser de no minimo 10 dias")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarDucaraoCiclos
        {
            get
            {
                return NbCicloDuracaoDiasPlan >= 10;
            }
        }

        /// <summary>
        /// Regra de negócio que valida se ao alterar a dataHora de início de um projeto, não tem ciclos com a situação pronta
        /// </summary>
        [RuleFromBoolProperty("ValidarCicloProntos", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Já existe(m) ciclo(s) concluído(s).")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarCicloProntos
        {
            get
            {
                int nbCiclosProntos = CicloDesenv.GetCiclosProntos(Session, this).Count;

                if (oldDate != DtInicioReal)
                    return nbCiclosProntos == 0;
                else
                    return true;
            }
        }

        /// <summary>
        /// Regra de negócio que valida se ao alterar a dataHora de início de um projeto, não tem ciclos com a situação cancelada
        /// </summary>
        [RuleFromBoolProperty("ValidarCicloCancelado", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Já existe(m) ciclo(s) cancelado(s).")]
        [NonPersistent, Browsable(false)]
        public bool RnValidarCiclosCancelados
        {
            get
            {
                int nbCiclosCancelados = CicloDesenv.GetCiclosCancelados(Session, this).Count;

                if (oldDate != DtInicioReal)
                    return nbCiclosCancelados == 0;
                else
                    return true;
            }
        }

        /// <summary>
        /// Regra de negócio que valida se ao deletar ciclos, não se coloque um valor menor que o somatório de ciclos concluídos ou cancelados
        /// </summary>
        [RuleFromBoolProperty("ValidarPermitirDeletarCiclos", DefaultContexts.Save, InvertResult = false,
        CustomMessageTemplate = "Já existe(m) ciclo(s) cancelado(s) ou concluído(s)")]
        [NonPersistent, Browsable(false)]
        public bool RnPermitirDeletarCiclos
        {
            get
            {
                int nbCiclosProntos = CicloDesenv.GetCiclosProntos(Session, this).Count;
                int nbCiclosCancelados = CicloDesenv.GetCiclosCancelados(Session, this).Count;

                if (NbCicloTotalPlan >= (nbCiclosProntos + nbCiclosCancelados))
                {
                    verificacaoValida = true;
                    return true;
                }
                else
                    return false;
            }
        }
        

        /// <summary>
        /// Regra de negócio que calcula a dataHora de término real do projeto
        /// </summary>
        public void RnCalcularTerminoReal()
        {
            if (projetoOld != null && (projetoOld.DtInicioReal == DtInicioReal &&
                projetoOld.NbCicloTotalPlan == NbCicloTotalPlan &&
                projetoOld.NbCicloDuracaoDiasPlan == NbCicloDuracaoDiasPlan &&
                projetoOld.NbCicloDiasIntervalo == NbCicloDiasIntervalo))
            {
                return;
            }

            if (DtInicioReal != DateTime.MinValue)
            {

                DateTime dtInicio = DtInicioReal;
                _DtTerminoReal = dtInicio.AddDays(-1);
                for (int nbCiclo = 1; nbCiclo <= NbCicloTotalPlan; nbCiclo++)
                {
                    int dia = 1;
                    try
                    {
                        Ciclos.Sorting.Add(new SortProperty("NbCiclo", SortingDirection.Ascending));

                        if (Ciclos.Count > 0 && DtInicioReal != oldDate)
                            VerificarSeOCicloEstaConcluido(Ciclos[0].CsSituacaoCiclo);

                        //Metodo que verifica as datas de inicio e final do ultimo ciclo setado como concluído ou cancelado
                        if (nbCiclo == 1)
                        {
                            foreach (CicloDesenv item in Ciclos)
                            {
                                if (item.CsSituacaoCiclo == CsSituacaoCicloDomain.Concluido || item.CsSituacaoCiclo == CsSituacaoCicloDomain.Cancelado)
                                {
                                    DateTime verificarItemDataTermino = item.DtTermino.AddDays(NbCicloDiasIntervalo);

                                    do
                                    {
                                        verificarItemDataTermino = verificarItemDataTermino.AddDays(1);

                                        if (Calendario.IsDiaUtil(Session, verificarItemDataTermino))
                                        {
                                            dtInicio = verificarItemDataTermino;
                                            _DtTerminoReal = verificarItemDataTermino.AddDays(-1);
                                        }

                                    }
                                    while (!Calendario.IsDiaUtil(Session, verificarItemDataTermino));

                                }
                                else
                                    break;
                            }
                        }

                        //Metodo que faz os calculos das datas
                        if (Ciclos.Count < nbCiclo || (Ciclos.Count >= nbCiclo && Ciclos[nbCiclo - 1].CsSituacaoCiclo != CsSituacaoCicloDomain.Concluido && Ciclos[nbCiclo - 1].CsSituacaoCiclo != CsSituacaoCicloDomain.Cancelado))
                        {
                            do
                            {
                                _DtTerminoReal = _DtTerminoReal.AddDays(1);
                                if (Calendario.IsDiaUtil(Session, _DtTerminoReal))
                                    dia += 1;

                            }
                            while (dia <= NbCicloDuracaoDiasPlan);


                            if (nbCiclo > CicloDesenv.GetUltimoCiclo(this))
                            {
                                if (IsSaving)
                                {
                                    CicloDesenv ciclos = new CicloDesenv(Session) { Projeto = this, NbCiclo = nbCiclo, DtInicio = dtInicio, DtTermino = _DtTerminoReal };
                                    ciclos.Save();
                                }
                            }
                            else
                            {
                                Ciclos[nbCiclo - 1].DtInicio = dtInicio;
                                Ciclos[nbCiclo - 1].DtTermino = _DtTerminoReal;
                            }

                            dia = 1;

                            if (nbCiclo < NbCicloTotalPlan)
                            {
                                int cont = 0;

                                do
                                {
                                    _DtTerminoReal = _DtTerminoReal.AddDays(1);
                                    if (Calendario.IsDiaUtil(Session, dtTermino))
                                        dia += 1;
                                }
                                while (dia <= NbCicloDiasIntervalo);

                                dtInicio = _DtTerminoReal;

                                do
                                {
                                    dtInicio = dtInicio.AddDays(1);
                                    if (Calendario.IsDiaUtil(Session, dtInicio))
                                        cont = 1;

                                }
                                while (cont == 0);
                            }


                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Já existe um ciclo concluído");
                        break;
                    }

                }

                if (NbCicloTotalPlan < nbCicloTotalPlanOld && verificacaoValida)
                {
                    if (NbCicloTotalPlan < CicloDesenv.GetUltimoCiclo(this))
                    {
                        Ciclos.Sorting.Add(new SortProperty("NbCiclo", SortingDirection.Ascending));
                        ArrayList lista = new ArrayList(Ciclos);

                        foreach (CicloDesenv item in lista)
                        {
                            if (item.NbCiclo > NbCicloTotalPlan && (item.CsSituacaoCiclo != CsSituacaoCicloDomain.Cancelado && item.CsSituacaoCiclo != CsSituacaoCicloDomain.Concluido))
                            {
                                item.Delete();
                            }
                        }
                    }

                    verificacaoValida = false;
                }
            }
            else
                if (DtInicioReal == DateTime.MinValue && DtTerminoPlan != DateTime.MinValue)
                {
                    dtTermino = DtTerminoPlan;
                }

        }

        /// <summary>
        /// Verifica se ja tem algum ciclo concluído no projeto
        /// </summary>
        /// <param name="s">s</param>
        public static void VerificarSeOCicloEstaConcluido(CsSituacaoCicloDomain s)
        {
            if (s == CsSituacaoCicloDomain.Concluido || s == CsSituacaoCicloDomain.Cancelado)
            {
                throw new Exception();
            }
        }

        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        /// <summary>
        /// Retorna o nome do Projeto Selecionado
        /// </summary>
        /// <param name="session">session</param>
        /// <returns>returns</returns>
        public static Projeto GetProjetoAtual(Session session)
        {
            return session.GetObjectByKey<Projeto>(SelectedProject);
        }

        /// <summary>
        /// Método responsável por buscar um projeto a partir do Oid.
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="oidProjeto">Oid do projeto (ID)</param>
        /// <returns>Objeto Projeto</returns>
        public static Projeto GetProjetoPorOid(Session session, Guid oidProjeto)
        {
            return session.FindObject<Projeto>(CriteriaOperator.Parse("Oid = ?", oidProjeto));
        }
        #endregion

        #region Utils
        #endregion

        #region Constructors

        /// <summary>
        /// OnLoaded
        /// </summary>
        protected override void OnLoaded()
        {
            nbTamanhoTotalOld = NbTamanhoTotal;

            if (projetoOld == null)
            {
                projetoOld = MemberwiseClone() as Projeto;
            }

            base.OnLoaded();
            oldDate = DtInicioReal;
            nbCicloTotalPlanOld = NbCicloTotalPlan;
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">Projeto</param>
        public Projeto(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// AfterConstruction
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            verificacaoValida = false;
            oldDate = DateTime.MinValue;
            DtInicioPlan = DateTime.MinValue;
            DtInicioReal = DateTime.MinValue;
            DtTerminoPlan = DateTime.MinValue;
            DtTerminoReal = DateTime.MinValue;
            dtTermino = DateTime.MinValue;
            NbCicloTotalPlan = 0;
            NbCicloDiasIntervalo = 1;
            NbCicloDuracaoDiasPlan = 10;
            UltimoFiltro = new ProjetoUltimoFiltro(Session) { Projeto = this };
        }
        #endregion

    }
}