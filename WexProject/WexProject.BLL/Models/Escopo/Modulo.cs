using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalEditorState;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.Base;
using WexProject.BLL.Shared.Domains.Escopo;
using WexProject.Library.Libs.Str;
using DevExpress.ExpressApp;

namespace WexProject.BLL.Models.Escopo
{

    /// <summary>
    /// Classe de Modulos
    /// </summary>
    [DefaultClassOptions]
    [RuleIsReferenced("RuleIsReferenced_moduloRequisito", DefaultContexts.Delete, typeof(Requisito), "Modulo", InvertResult = true,
    CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
    MessageTemplateMustBeReferenced = "Existe um Módulo sendo referenciado por um Requisito")]
    [RuleIsReferenced("RuleIsReferenced_moduloEstoria", DefaultContexts.Delete, typeof(Estoria), "Modulo", InvertResult = true,
    CriteriaEvaluationBehavior = PersistentCriteriaEvaluationBehavior.BeforeTransaction,
    MessageTemplateMustBeReferenced = "Existe um Módulo sendo referenciado por uma Estória")]
    [RuleCombinationOfPropertiesIsUnique("Modulo_Projeto_TxNome_Unique", DefaultContexts.Save, "Projeto, TxNome", Name = "JaExisteModuloProjeto",
    CustomMessageTemplate = "Já existe um módulo com esse nome no projeto")]
    [OptimisticLocking( false )]
    public class Modulo : BaseObject
    {
        #region Attributes
        /// <summary>
        /// Atributo do modulo pai
        /// </summary>
        private Modulo moduloPai;

        /// <summary>
        /// Atributo de TxNome
        /// </summary>
        private String txNome;

        /// <summary>
        /// Atributo de NbPontosNaoIniciado
        /// </summary>
        private double nbPontosTotal;

        /// <summary>
        /// Atributo de Projeto
        /// </summary>
        private Projeto projeto;

        /// <summary>
        /// Atributo de TxID
        /// </summary>
        private string txID;

        /// <summary>
        /// Atributo de TxDescricao
        /// </summary>
        private String txDescricao;

        /// <summary>
        /// Atributo de NbEsforcoPlanejado
        /// </summary>
        private UInt32 nbEsforcoPlanejado;

        /// <summary>
        /// Atributo de NbPontosNaoIniciados
        /// </summary>
        private double nbPontosNaoIniciado;

        /// <summary>
        /// Atributo de nbTotalProjetoNaoIniciado
        /// </summary>
        private double nbTotalProjetoNaoIniciado;

        /// <summary>
        /// Atributo de NbPontosEmAnalise
        /// </summary>
        private double nbPontosEmAnalise;

        /// <summary>
        /// Atributo de NbPontosEmDesenv
        /// </summary>
        private Double nbPontosEmDesenv;

        /// <summary>
        /// Atributo de NbPontosPronto
        /// </summary>
        private Double nbPontosPronto;

        /// <summary>
        /// Atributo de NbPontosDesvio
        /// </summary>
        private double nbPontosDesvio;
        #endregion

        #region Properties
        /// <summary>
        /// Variável que guarda o nome do projeto
        /// </summary>
        [Indexed]
        [Association("Modulos", typeof(Projeto))]
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
        /// Variável que guarda o ID do Modulo
        /// </summary>
        public string TxID
        {
            get
            {
                
                return txID;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<string>("TxID", ref txID, value.Trim());
            }
        }

        /// <summary>
        /// Variável que guarda o nome do Modulo
        /// </summary>
        [RuleRequiredField("Modulo_TxNome_Required", DefaultContexts.Save, "Informe um nome para o módulo!")]
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
        /// Campo que guarda a descrição do modulo
        /// </summary>
        public String TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value != null)
                {
                    txDescricao = StrUtil.RetirarEspacoVazio(txDescricao);
                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value.Trim());
                }
            }
        }

        /// <summary>
        /// Import do ModuloPai
        /// </summary>
        [Association("ModuloPaiAssociation", typeof(Modulo))]
        public Modulo ModuloPai
        {
            get
            {
                return moduloPai;
            }
            set
            {
                if (!IsDeleted)
                    SetPropertyValue("ModuloPai", ref moduloPai, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor do esforço planejado
        /// </summary>
        [ImmediatePostData(true)]
        [Appearance("NbEsforcoPlanejado_Appearance", TargetItems = "NbEsforcoPlanejado", Enabled = false)]
        [RuleRequiredField("Modulo_NbEsforcoPlanejado_Required", DefaultContexts.Save, "Informe um esforço para o módulo")]
        public UInt32 NbEsforcoPlanejado
        {
            get
            {
                return nbEsforcoPlanejado;
            }
            set
            {
                SetPropertyValue<UInt32>("NbEsforcoPlanejado", ref nbEsforcoPlanejado, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos totais do projeto
        /// </summary>
        [ImmediatePostData(true)]
        [RuleRequiredField("Modulo_nbPontosPlanejados_Required", DefaultContexts.Save, "Informe o número de pontos planejados")]
        public double NbPontosTotal
        {
            get
            {
                return nbPontosTotal;
            }
            set
            {
                bool alterado = nbPontosTotal != value;

                SetPropertyValue<double>("NbPontosTotal", ref nbPontosTotal, value);
                RnCalculaEsforcoPeloPontosPlanejados();
                if (alterado && !IsLoading)
                    RnCalcularPontosSituacao();

            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos não iniciados do projeto
        /// </summary>
        [RuleRequiredField("Modulo_NbPontosNaoIniciado_Required", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public double NbPontosNaoIniciado
        {
            get
            {
                return nbPontosNaoIniciado;
            }
            set
            {
                SetPropertyValue<double>("NbPontosNaoIniciado", ref nbPontosNaoIniciado, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos em analise do projeto
        /// </summary>
        [RuleRequiredField("Modulo_NbPontosEmAnalise_Required", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public double NbPontosEmAnalise
        {
            get
            {
                return nbPontosEmAnalise;
            }
            set
            {
                SetPropertyValue<double>("NbPontosEmAnalise", ref nbPontosEmAnalise, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos totais do projeto
        /// </summary>
        [RuleRequiredField("Modulo_NbPontosEmDesenv_Required", DefaultContexts.Save)]
        public Double NbPontosEmDesenv
        {
            get
            {
                return nbPontosEmDesenv;
            }
            set
            {
                SetPropertyValue<Double>("NbPontosEmDesenv", ref nbPontosEmDesenv, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos totais do projeto
        /// </summary>
        [RuleRequiredField("Modulo_NbPontosPronto_Required", DefaultContexts.Save)]
        public Double NbPontosPronto
        {
            get
            {
                return nbPontosPronto;
            }
            set
            {
                SetPropertyValue<Double>("NbPontosPronto", ref nbPontosPronto, value);
            }
        }

        /// <summary>
        /// Variável que guarda o valor dos pontos totais do projeto
        /// </summary>
        [RuleRequiredField("Modulo_NbPontosDesvio _Required", DefaultContexts.Save)]
        public double NbPontosDesvio
        {
            get
            {
                return nbPontosDesvio;
            }
            set
            {
                SetPropertyValue<double>("NbPontosDesvio", ref nbPontosDesvio, value);
            }
        }

        /// <summary>
        /// Associação com o ModuloPai
        /// </summary>
        [Association("ModuloPaiAssociation"), Aggregated, Browsable(false)]
        public XPCollection<Modulo> Filhos
        {
            get
            {
                return GetCollection<Modulo>("Filhos");
            }
        }
        #endregion

        #region NonPersistentProperties
      
        /// <summary>
        /// Campo que filtra os nomes que não possuem filhos
        /// </summary>
        [NonPersistent]
        public String _TxNomeDashboard
        {
            get
            {
                if (Filhos.Count == 0)
                {
                    txNome = StrUtil.RetirarEspacoVazio(txNome);
                    return TxNome;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Variável que faz a concatenação dos nomes dos módulos
        /// </summary>
        [NonPersistent]
        public String _TxNomeCompleto
        {
            get
            {
                if (ModuloPai != null)
                {
                    return String.Format("{0} - {1} > {2}", TxID, ModuloPai.TxNome, TxNome);
                }
                else
                {
                    return String.Format("{0} - {1}", TxID, TxNome);
                }
            }

            set
            {
                txNome = StrUtil.RetirarEspacoVazio(txNome);
                SetPropertyValue<string>("TxNome", ref txNome, value);
            }
        }

        /// <summary>
        /// Propriedade que concatena o nome do Módulo com o ID e o nome do Módulo pai
        /// </summary>
        [NonPersistent]
        public String _TxNomeModulo
        {
            get
            {
                if (ModuloPai != null)
                {
                    return String.Format("{0} > {1}", ModuloPai.TxNome, TxNome);
                }
                else
                {
                    txNome = StrUtil.RetirarEspacoVazio(txNome);
                    return TxNome;
                }
            }
        }


        /// <summary>
        /// Variavel que filtra o que será mostrado no grid de situação
        /// </summary>
        [NonPersistent]
        public String _TxSituacao
        {
            get
            {
                if (Filhos.Count != 0)
                    return "";
                String txSituacao = "";


                nbTotalProjetoNaoIniciado = Estoria.GetTotalPontosEstoria(this);
                if (NbPontosTotal > nbTotalProjetoNaoIniciado)
                    NbPontosNaoIniciado = NbPontosTotal - nbTotalProjetoNaoIniciado;

                if (NbPontosNaoIniciado != 0)
                {
                    txSituacao += String.Format("Não iniciado: {0}%", _NbPercNaoIniciado);
                }
                if (NbPontosPronto != 0)
                {
                    txSituacao += !String.IsNullOrEmpty(txSituacao) ? "; " : "";
                    txSituacao += String.Format("Pronto: {0}%", _NbPercPronto);
                }
                if (NbPontosEmAnalise != 0)
                {
                    txSituacao += !String.IsNullOrEmpty(txSituacao) ? "; " : "";
                    txSituacao += String.Format("Em Análise: {0}%", _NbPercEmAnalise);
                }
                if (nbPontosEmDesenv != 0)
                {
                    txSituacao += !String.IsNullOrEmpty(txSituacao) ? "; " : "";
                    txSituacao += String.Format("Em Desenv: {0}%", _NbPercEmDesenv);
                }
                if (NbPontosDesvio != 0)
                {
                    txSituacao += !String.IsNullOrEmpty(txSituacao) ? "; " : "";
                    txSituacao += String.Format("Desvio: {0}%", _NbPercDesvio);
                }
                return txSituacao;
            }

        }

        /// <summary>
        /// Variável que calcula o percentual de pontos não iniciado
        /// </summary>
        [NonPersistent]
        public double _NbPercNaoIniciado
        {
            get
            {
                double contadorNaoIniciado = 0;

                if (NbPontosTotal != 0)
                {
                    contadorNaoIniciado = (NbPontosNaoIniciado * 100) / NbPontosTotal;
                    contadorNaoIniciado = Math.Round(contadorNaoIniciado, 2);
                    return contadorNaoIniciado;
                }
                else
                {
                    return 0;
                }

            }
        }



        /// <summary>
        /// Variável que calcula o percentual de pontos em análise
        /// </summary>
        [NonPersistent]
        public double _NbPercEmAnalise
        {

            get
            {
                double contadorEmAnalise = 0;

                if (NbPontosTotal != 0)
                {
                    contadorEmAnalise = (NbPontosEmAnalise * 100) / NbPontosTotal;
                    contadorEmAnalise = Math.Round(contadorEmAnalise, 2);
                    return contadorEmAnalise;
                }
                else
                {
                    return 0;
                }

            }
        }

        /// <summary>
        /// Variável que calcula o percentual de pontos em desenvolvimento
        /// </summary>
        [NonPersistent]
        public double _NbPercEmDesenv
        {

            get
            {
                double contadorEmDesenv = 0;

                if (NbPontosTotal != 0)
                {
                    contadorEmDesenv = (NbPontosEmDesenv * 100) / NbPontosTotal;
                    contadorEmDesenv = Math.Round(contadorEmDesenv, 2);
                    return contadorEmDesenv;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// Variável que calcula o percentual de pontos prontos
        /// </summary>
        [NonPersistent]
        public double _NbPercPronto
        {
            get
            {
                double contadorPronto;

                if (NbPontosTotal != 0)
                {
                    contadorPronto = (NbPontosPronto * 100) / NbPontosTotal;
                    contadorPronto = Math.Round(contadorPronto, 2);
                    return contadorPronto;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// Variável que calcula o percentual de pontos em desvio
        /// </summary>
        [NonPersistent]
        public double _NbPercDesvio
        {
            get
            {

                double contadorDesvio = 0;

                if (NbPontosDesvio > 0 && NbPontosTotal != 0)
                {
                    contadorDesvio = (NbPontosDesvio * 100) / NbPontosTotal;
                    contadorDesvio = Math.Round(contadorDesvio, 2);
                    return contadorDesvio;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Variável que guarda o módulo pai antigo
        /// </summary>
        [NonPersistent]
        public Modulo _ModuloPaiOld
        {
            get;
            set;
        }

        /// <summary>
        /// Variavel que guarda o Total de Pontos do Projeto
        /// </summary>
        [VisibleInListView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        [Custom("Caption", "Total de Pontos")]
        public double _NbPontosTotalProjeto
        {
            get
            {
                if (projeto != null)
                {
                    return projeto.NbTamanhoTotal;
                }
                else
                {
                    Projeto = Projeto.GetProjetoAtual(Session);
                    return 0;
                }
            }
        }

        /// <summary>
        /// Propiedade que retorna os pontos do projeto que estão sendo utilizados 
        /// nos módulos, usado no formulário do módulo.
        /// </summary>
        [VisibleInListView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        [Custom("Caption", "Pontos Utilizados")]
        public double _NbPontosUtilizados
        {
            get
            {
                return TotalPontosDosModulosEmUmProjeto();
            }
        }


        /// <summary>
        /// Propiedade que calcula a diferença de pontos do projeto
        /// </summary>
        [VisibleInListView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        [Custom("Caption", "Diferença de Pontos")]
        public string _NbDiferencaDePontosProjeto
        {
            get
            {

                double diferencaDePontos = _NbPontosTotalProjeto - _NbPontosUtilizados;

                string txDiferencaDePontos = "";

                if (diferencaDePontos >= 0)
                    txDiferencaDePontos = "+" + diferencaDePontos;
                else
                    txDiferencaDePontos = diferencaDePontos.ToString();

                return txDiferencaDePontos;
            }
        }

        #endregion

        #region BusinessRules

        /// <summary>
        /// Ativa a Função ID ao clicar no botão salvar
        /// </summary>
        protected override void OnSaving()
        {
            if (NbEsforcoPlanejado > 100)
                NbEsforcoPlanejado = 100;
            else
                if (NbEsforcoPlanejado < 0 || NbEsforcoPlanejado == 0)
                    NbEsforcoPlanejado = 0;

            if (Oid.Equals(new Guid()) || _ModuloPaiOld != ModuloPai)
                RnCriarID();

            //Verifica se tamanho 
            RnCalcularEsforcoModuloPai();
            RnCalcularPontosModuloPai();
            RnCalcularPontosSituacao();
            base.OnSaving();
        }

        /// <summary>
        /// Método que verifica se o tamanho do módulo é maior que zero
        /// </summary>
        [RuleFromBoolProperty("SalvarTamanhoModuloMenorZero", DefaultContexts.Save, "O tamanho do módulo não pode ser número menor que zero")]
        [NonPersistent, Browsable(false)]
        public bool RnSalvarTamanhoMaiorZero
        {
            get
            {
                bool errorView = true;
                if (NbPontosTotal < 0)
                    errorView = false;

                return errorView;
            }
        }

        public static Modulo RnRecuperarModuloRaiz(Modulo m)
        {
            if (m.ModuloPai == null) {
                return m;
            }
            return RnRecuperarModuloRaiz(m.moduloPai);
        }


        /// <summary>
        /// Método que faz a verificação a seguir :
        /// Não deixar inserir uma quantidade de pontos que faça que o total 
        /// de pontos dos módulos de um Projeto ultrapasse o total de pontos definido para o mesmo
        /// </summary>
        [RuleFromBoolProperty("SalvarModulosProjetoSomaPontosPlanejadosMaiorTamanhoProjeto", DefaultContexts.Save, "A soma dos pontos planejados dos módulos é maior que o tamanho do projeto")]
        [NonPersistent, Browsable(false)]
        public bool RnSalvarModulosSomaPontosMaiorProjeto
        {
            get
            {
                UInt32 soma = 0;


                if (Projeto != null)
                {
                    foreach (Modulo mod in Projeto.Modulos)
                    {
                        if (mod.Filhos.Count == 0 && mod.NbPontosTotal >= 0)
                            soma += (UInt32)mod.NbPontosTotal;
                    }

                    return soma <= Projeto.NbTamanhoTotal || Projeto.NbTamanhoTotal == 0;
                }
                else
                {
                    Projeto = Projeto.GetProjetoAtual(Session);
                    return true;
                }

            }
        }

        /// <summary>
        /// Função que calcula esforço do modo combase nos pontos planejados
        /// </summary>
        private void RnCalculaEsforcoPeloPontosPlanejados()
        {

            if (Projeto != null)
            {
                if (Projeto.NbTamanhoTotal != 0 && NbPontosTotal >= 0)
                    NbEsforcoPlanejado = ((UInt32)NbPontosTotal * 100) / Projeto.NbTamanhoTotal;
                else
                    if (IsLoading)
                    {
                        //Se colocar uma negação não possui o mesmo efeito.
                    }
                    else
                        NbEsforcoPlanejado = 0;
            }
        }

        /// <summary>
        /// Função que cria os IDs dos Modulos
        /// </summary>
        private void RnCriarID()
        {
            if (Projeto != null)
            {
                if (IsDeleted)
                    return;

                    int crt, cont = 0;
                string id;

                if (ModuloPai == null)
                    crt = GetModuloPorProjeto(Session, Projeto).Count;
                else
                    crt = GetTotalFilhos(ModuloPai);

                do
                {
                    crt += 1;
                    if (ModuloPai == null && crt < 10)
                    {
                        id = "0" + String.Format("{0}", crt);
                    }
                    else if (ModuloPai == null)
                    {
                        id = String.Format("{0}", crt);
                    }
                    else
                    {
                        if (crt < 10)
                            id =  String.Format("{0}.{1}{2}", ModuloPai.TxID, cont, crt);
                        else
                            id =  String.Format("{0}.{1}", ModuloPai.TxID, crt);
                    }
                }
                while (GetModulosPorID(Projeto, id).Count != 0);

                TxID = id;
            }
        }

        /// <summary>
        /// Regra de negócio que calcula o esforço dos modulos pai
        /// </summary>
        private void RnCalcularEsforcoModuloPai()
        {
            if (ModuloPai == null)
                return;

            UInt32 esforcoPai = 0;
            foreach (Modulo filho in ModuloPai.Filhos)
                if (!filho.IsDeleted)
                    esforcoPai += filho.NbEsforcoPlanejado;

            ModuloPai.NbEsforcoPlanejado = esforcoPai;
        }

        /// <summary>
        /// Regra de negócio que calcula os pontos dos modulos pai
        /// </summary>
        private void RnCalcularPontosModuloPai()
        {
            if (ModuloPai == null)
                return;

            double pontosPai = 0;
            foreach (Modulo filhoPonto in ModuloPai.Filhos)
                if (!filhoPonto.IsDeleted)
                    pontosPai += filhoPonto.NbPontosTotal;

            ModuloPai.NbPontosTotal = pontosPai;
        }

        /// <summary>
        /// Função que não permite deletar um módulo que tenha uma associação
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirDeletarSeTiverAssociacoes", DefaultContexts.Delete, "Não é possível excluir módulos associados.")]
        [NonPersistent, Browsable(false)]
        public bool RnDeletarModulosSemAssociacao
        {
            get
            {
                return Filhos.Count == 0;
            }
        }
        /// <summary>
        /// Função que não permite salvar se a soma dos esforços dos módulos dos projetos seja maior que 100%
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirSalvarSeOsModulosDoProjetoTiveremASomaDosEsforcosMaiorQue100", DefaultContexts.Save, "A soma dos esforços ultrapassa 100%")]
        [NonPersistent, Browsable(false)]
        public bool RnSalvarModulosComSomaEsforcoMaiorQue100
        {
            get
            {
                UInt32 soma = 0;
                if (Projeto == null)
                    return true;
                else
                {
                    foreach (Modulo mod in Projeto.Modulos)
                    {
                        if (mod.Filhos.Count == 0)
                            soma += mod.NbEsforcoPlanejado;
                    }
                    return soma <= 100;
                }
            }
        }


        /// <summary>
        /// Função que não permite salvar se o esforço for 0
        /// </summary>
        //Retirar validação para permitir cadastrar esforço maior que 0
        // [RuleFromBoolProperty("NaoPermitirSalvarSeOEsforcoDoModuloFor0", DefaultContexts.Save, "O esforço precisa ser maior que '0'")]
        [NonPersistent, Browsable(false)]
        public bool RnSalvarModulosComEsforcoMaiorQue0
        {
            get
            {
                if (Oid.Equals(new Guid()) || Filhos.Count == 0)
                    return NbEsforcoPlanejado > 0;
                else
                    return true;
            }
        }


        /// <summary>
        /// Função que não permite salvar um módulo se não tiver projeto selecionado
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirSalvarModuloSeNaoTiverProjetoSelecionado", DefaultContexts.Save, "Primeiro selecione um projeto")]
        [NonPersistent, Browsable(false)]
        public bool RnSalvarModulosComProjeto
        {
            get
            {
                if (Projeto == null)
                    Projeto = Projeto.GetProjetoAtual(Session);

                if (Projeto == null)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Regra de negócio que calcula os pontos do campo Situação
        /// </summary>
        public void RnCalcularPontosSituacao()
        {
            ICollection estorias = Estoria.GetEstoriasPorModulo(this);

            double ptEmAnalise = 0;
            double ptPronto = 0;
            double ptEmDesenv = 0;

            foreach (Estoria estoria in estorias)
            {
                if (estoria.CsSituacao == CsEstoriaDomain.NaoIniciado || estoria.CsSituacao == CsEstoriaDomain.Replanejado)
                    ptEmAnalise += estoria.NbTamanho;
                else if (estoria.CsSituacao == CsEstoriaDomain.Pronto)
                    ptPronto += estoria.NbTamanho;
                else if (estoria.CsSituacao == CsEstoriaDomain.EmDesenv)
                    ptEmDesenv += estoria.NbTamanho;
            }

            NbPontosEmAnalise = ptEmAnalise;
            NbPontosPronto = ptPronto;
            NbPontosEmDesenv = ptEmDesenv;

            double pontosNaoIniciado = NbPontosTotal - NbPontosEmAnalise - NbPontosPronto - NbPontosEmDesenv;
            if (pontosNaoIniciado >= 0)
            {
                NbPontosNaoIniciado = pontosNaoIniciado;
                NbPontosDesvio = 0;
            }
            else
            {
                NbPontosNaoIniciado = 0;
                NbPontosDesvio = pontosNaoIniciado * -1;
            }
        }
        #endregion

        #region NewInstance
        #endregion

        #region DBQueries (Gets)
        /// <summary>
        /// Colletion que retorna as estórias do modulo selecionado selecionado
        /// </summary>
        [NonPersistent, Browsable(false)]
        public XPCollection<Estoria> Estorias
        {
            get
            {
                XPCollection<Estoria> estorias = new XPCollection<Estoria>(Session);
                if (!string.IsNullOrEmpty(TxID))
                {
                    estorias.Criteria = CriteriaOperator.Parse(String.Format("TxID not like '{0}%' And Modulo.Oid = '{1}'", TxID, Oid));
                }
                return estorias;
            }
        }
        /// <summary>
        /// Função que filtra os módulos para que não seja listado no campo ModuloPai, módulos que não podem ser pais.
        /// </summary>
        [NonPersistent, Browsable(false)]
        public XPCollection<Modulo> CandidatosModuloPai
        {
            get
            {
                XPCollection<Modulo> modulos = new XPCollection<Modulo>(Session);
                if (!string.IsNullOrEmpty(TxID))
                {
                    modulos.Criteria = CriteriaOperator.Parse(String.Format("TxID not like '{0}%'", TxID));
                }
                return modulos;
            }
        }

        /// <summary>
        /// Get que retorna os modulos por projeto
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <returns>Modulos por projeto</returns>
        public static XPCollection GetModuloPorProjeto(Session session, Projeto projeto)
        {
            if (projeto == null)
            {
                return new XPCollection();
            }
            else
            {
                return new XPCollection(session, typeof(Modulo),
                CriteriaOperator.Parse(String.Format("ModuloPai is null AND Projeto.Oid = '{0}'", projeto.Oid)));
            }
        }

        /// <summary>
        /// Get que retorna os modulos por projeto
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <returns>Modulos por projeto</returns>
        public static XPCollection<Modulo> GetModuloPorProjeto(Session session, Guid projeto)
        {
            XPCollection<Modulo> resultList = null;
            if (projeto == null)
            {
                resultList = new XPCollection<Modulo>();
            }
            else
            {
                resultList = new XPCollection<Modulo>(session, CriteriaOperator.Parse(String.Format("ModuloPai is null AND Projeto.Oid = '{0}'", projeto)));
            }
            resultList.Sorting.Add(new SortProperty("TxNome", DevExpress.Xpo.DB.SortingDirection.Ascending));
            return resultList;
        }

        /// <summary>
        /// Busca a sessão e o id do projeto no BD
        /// </summary>
        /// <param name="projeto">projeto</param>
        /// <param name="apenasModulosFilhos">apenasModulosFilhos</param>
        /// <returns>ID</returns>
        public static XPCollection<Modulo> GetModulosPorProjeto(Projeto projeto, bool apenasModulosFilhos = true)
        {
            string condicaoFilhos;
            if (apenasModulosFilhos)
                condicaoFilhos = "ModuloPai is null AND";
            else
                condicaoFilhos = "";

            XPCollection<Modulo> consulta = new XPCollection<Modulo>(projeto.Session,
            CriteriaOperator.Parse(String.Format("{0} Projeto.Oid = '{1}'", condicaoFilhos, projeto.Oid)));


            if (consulta.Count > 0)
                return consulta;
            else
                return null;


        }

        /// <summary>
        /// BUsca a sessão, o id do projeto e txid no BD
        /// </summary>
        /// <param name="projeto">projeto</param>
        /// <param name="txID">txID</param>
        /// <returns>return</returns>
        private static ICollection GetModulosPorID(Projeto projeto, String txID)
        {
            if (projeto != null)
            {
                return projeto.Session.GetObjects(projeto.Session.GetClassInfo<Modulo>(),
                CriteriaOperator.Parse(String.Format("Projeto.Oid = '{0}' AND TxID = '{1}'", projeto.Oid, txID)), null, 0, false, true);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Busca a sessão, o id projeto e o id do modulopai no BD
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <param name="moduloPai">modulopai</param>
        /// <returns>return</returns>
        private static XPCollection GetModulosFilhos(Session session, Projeto projeto, Modulo moduloPai)
        {
            if (projeto == null || moduloPai == null)
            {
                return new XPCollection();
            }
            else
            {
                return new XPCollection(session, typeof(Modulo),
                CriteriaOperator.Parse(String.Format("Projeto.Oid = '{0}' AND ModuloPai.Oid = '{1}'", projeto.Oid, moduloPai.Oid)));
            }
        }
        /// <summary>
        /// Retorna o total de modulos filhos do modulo pai passado por parametro.
        /// </summary>
        /// <param name="moduloPai">moduloPai</param>
        /// <returns>Total de Filhos</returns>
        private static int GetTotalFilhos(Modulo moduloPai)
        {
            int total = 0;

            try
            {
                object obj = moduloPai.Session.Evaluate(typeof(Modulo),
                CriteriaOperator.Parse("Count(*)"), CriteriaOperator.Parse("ModuloPai.Oid = ? ", moduloPai.Oid));

                if (obj != null)
                    total = (int)obj;
            }
            catch
            {
            }
            return total;
        }

        #endregion

        #region Utils
        /// <summary>
        /// Override de _TxNomeCompleto
        /// </summary>
        /// <returns>returns</returns>
        public override string ToString()
        {
            return _TxNomeCompleto;
        }
        #endregion

        /// <summary>
        /// Somatória dos pontos de todos os modulos
        /// </summary>
        /// <returns>Retorna um double com o resultado da soma de todos os pontos
        /// de todos os modulos relacionados a um projeto      </returns>
        private double TotalPontosDosModulosEmUmProjeto()
        {

            double pontosModulos = 0;
            if (Projeto != null)
            {
                foreach (Modulo modulo in Projeto.Modulos)
                {
                    pontosModulos += modulo.NbPontosTotal;
                }

                return pontosModulos;
            }
            else
                return 0;
        }

        #region UserInterface

        /// <summary>
        /// Interface que torna o campo NbEsforcoPlanejado read-only quando este tem filhos
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>NbEsforcoPlanejado read-only</returns>
        [EditorStateRule("DisabledEsforcoPlanejado", "NbEsforcoPlanejado", ViewType.DetailView)]
        public EditorState DisabledEsforcoPlanejado(out bool active)
        {
            active = Filhos.Count > 0;
            return EditorState.Disabled;
        }

        /// <summary>
        /// Interface que esconde o campo TxID quando o usuário está cadastrando um novo Modulo
        /// </summary>
        /// <param name="active">active</param>
        /// <returns>TxID hidden</returns>
        [EditorStateRule("HiddenTxID", "TxID", ViewType.DetailView)]
        public EditorState HiddenTxID(out bool active)
        {
            active = Oid.Equals(new Guid());
            return EditorState.Hidden;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Metodo que atribui o novo valor do módulo pai a variável _ModuloPaiOld
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();
            _ModuloPaiOld = ModuloPai;
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">session</param>
        public Modulo(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Inicializa as variáveis e recebe o GetProjetoAtual
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            NbPontosEmAnalise = 0;
            NbPontosEmDesenv = 0;
            NbPontosPronto = 0;
            Projeto = Projeto.GetProjetoAtual(Session);
        }
        #endregion
    }
}
