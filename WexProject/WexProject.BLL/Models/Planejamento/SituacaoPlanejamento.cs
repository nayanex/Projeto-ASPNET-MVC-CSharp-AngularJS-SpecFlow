using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Drawing;
using DevExpress.Xpo.Metadata;
using System.Collections;
using System.Linq;
using DevExpress.Utils;
using System.Collections.Generic;
using System.Windows.Forms;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Img;
using WexProject.BLL.BOs.Planejamento;

namespace WexProject.BLL.Models.Planejamento
{
    /// <summary>
    /// Classe que guarda as situa��es de planejamento para um cronograma
    /// </summary>
    [DefaultClassOptions]
    [Custom("Caption", "Situa��o do Planejamento")]
    [OptimisticLocking( false )]
    public class SituacaoPlanejamento : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Nome da situa��o para cadastro.
        /// </summary>
        private String txDescricao;

        /// <summary>
        /// Guarda a situa��o (Ativo ou Inativo) do tipo de situa��o do planejamento.
        /// </summary>
        private CsTipoSituacaoPlanejamento csSituacao;

        /// <summary>
        /// Guarda o tipo da situa��o planejemento.
        /// </summary>
        private CsTipoPlanejamento csTipo;

        /// <summary>
        /// Guarda a imagem da bandeira para aquela situa��o para o planejamento.
        /// </summary>
        private Image blImagem;

        /// <summary>
        /// Guarda se a situa��o � padr�o do sistema.
        /// </summary>
        private CsPadraoSistema csPadrao;

        /// <summary>
        /// Resgata o atalho passado pelo usu�rio
        /// </summary>
        private Shortcut keyPress;

        /// <summary>
        /// Guarda a blacklist de atalhos
        /// </summary>
        public static List<Shortcut> blackListShortcuts = new List<Shortcut>()
		{ 
			Shortcut.Ctrl1,
			Shortcut.Ctrl2,
			Shortcut.Ctrl3, 
			Shortcut.CtrlN,
			Shortcut.CtrlS, 
            Shortcut.CtrlShiftS, 
			//Shortcut.CtrlShiftN, 
			Shortcut.CtrlShiftD,
			Shortcut.F1, 
			Shortcut.F2, 
			Shortcut.F3,
			Shortcut.F4,
			Shortcut.F5, 
			Shortcut.F6,
            Shortcut.F7, 
			Shortcut.F8,
			Shortcut.F9,
			Shortcut.F10, 
			Shortcut.F11, 
			Shortcut.F12,
			Shortcut.CtrlW,
			Shortcut.CtrlF4,
			Shortcut.Ins, 
			Shortcut.CtrlDel,
            Shortcut.CtrlF12
		};

        /// <summary>
        /// Guarda a string do keyproperty do atalho
        /// </summary>
        private String txKeys;

        /// <summary>
        /// Mostra o erro caso a condi��o seja satisfeita.
        /// </summary>
        public static bool viewErro = true;
        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade que guarda o nome da situa��o para o planejamento
        /// </summary>
        [RuleUniqueValue("SituacaoPlanejamento_TxDescricao_Unique", DefaultContexts.Save, "J� existe uma situa��o cadastrada com esse nome, tente novamente.")]
        [RuleRequiredField("SituacaoPlanejamento_TxDescricao_Required", DefaultContexts.Save, "Campo Nome da Situa��o de Planejamento n�o pode ser vazio.")]
        [Custom("Caption", "Nome")]
        [Size(30)]
        public String TxDescricao
        {
            get
            {
                return txDescricao;
            }
            set
            {
                if (value != null)
                    SetPropertyValue<String>("TxDescricao", ref txDescricao, value);
            }
        }

        /// <summary>
        /// Guarda a op��o selecionada pelo usu�rio no enum de tipos de situa��o (Ativo ou Inativo)
        /// </summary>
        [Custom("Caption", "Situa��o")]
        public CsTipoSituacaoPlanejamento CsSituacao
        {
            get
            {
                return csSituacao;
            }
            set
            {
                SetPropertyValue<CsTipoSituacaoPlanejamento>("CsSituacao", ref csSituacao, value);
            }
        }

        /// <summary>
        /// Guarda a op��o selecionada pelo usu�rio no enum de tipos de eventos.
        /// </summary>
        [Custom("Caption", "Tipo")]
        public CsTipoPlanejamento CsTipo
        {
            get
            {
                return csTipo;
            }
            set
            {
                SetPropertyValue<CsTipoPlanejamento>("CsTipo", ref csTipo, value);
            }
        }


        /// <summary>
        /// Guarda a imagem associada aquela situa��o de planejamento
        /// </summary>
        [Custom("Caption", "Imagem")]
        [RuleRequiredField("SituacaoPlanejamento_BlImagem_Required", DefaultContexts.Save, "Voc� necessita inserir uma imagem para Planejamento de Situa��o.")]
        [ValueConverter(typeof(ImageValueConverter))]
        [Size(1)]
        public Image BlImagem
        {
            get
            {
                return blImagem;
            }
            set
            {
                if (blImagem != value)
                    blImagem = value;
            }
        }

        /// <summary>
        /// Guarda a op��o selecionada para situa��o padr�o das tarefas.
        /// </summary>
        [Custom("Caption", "Padr�o ao criar tarefas")]
        public CsPadraoSistema CsPadrao
        {
            get
            {
                return csPadrao;
            }
            set
            {
                SetPropertyValue<CsPadraoSistema>("CsPadrao", ref csPadrao, value);
            }
        }

        /// <summary>
        /// Guarda a op��o selecionada para atalho de situa��o.
        /// </summary>
        [RuleUniqueValue("SituacaoPlanejamento_KeyPress_Unique", DefaultContexts.Save,
            "J� existe uma atalho para essa sele��o. Por favor selecione outro.")]
        [Custom("Caption", "Atalho")]
        public Shortcut KeyPress
        {
            get
            {
                return keyPress;
            }
            set
            {
                if (SituacaoPlanejamentoBO.VerificarPossibilidadeAdicionarAtalho(value))
                {
                    SetPropertyValue<Shortcut>("KeyPress", ref keyPress, value);
                    viewErro = true;
                }
            }
        }

        /// <summary>
        /// Guarda a string do keyproperty do atalho
        /// </summary>
        public String TxKeys
        {
            get
            {
                return txKeys;
            }
            set
            {
                SetPropertyValue<String>("TxKeys", ref txKeys, value);
            }
        }
        #endregion

        /// <summary>
        /// Fun��o que n�o permite salvar um hist�rico com Restante sendo 0 e situa��o diferente de pronto
        /// </summary>
        [RuleFromBoolProperty("NaoPermitirSalvarAtalhoPreDefinido", DefaultContexts.Save,
            "N�o � poss�vel associar o atalho pois o mesmo j� est� sendo usado pelo sistema.")]
        [NonPersistent, Browsable(false)]
        public bool VerErroAtalho
        {
            get
            {
                return viewErro;
            }
        }

        /////CRIAR REGRA NO ENTITY
        ///// <summary>
        ///// Fun��o que n�o permite deletar uma situa��o de planejamento j� usada
        ///// </summary>
        //[RuleFromBoolProperty( "NaoPermitirDeletarSeTarefaAssociada", DefaultContexts.Delete,
        //    "A Situa��o n�o pode ser deletada pois est� associada a uma tarefa." )]
        //[NonPersistent, Browsable( false )]
        //public bool NaoPermitirDeletarSeTarefaAssociada
        //{
        //    get
        //    {
        //        return ConsultarSituacaoUsada( Session );
        //    }
        //}

        #region Factories

        /// <summary>
        /// M�todo respons�vel por criar um objeto SituacaoPlanejamentoDto a partir do Objeto SituacaoPlanejamento
        /// </summary>
        /// <param name="situacaoPlanejamento">Objeto SituacaoPlanejamento</param>
        /// <returns>Objeto SituacaoPlanejamentoDto</returns>
        public SituacaoPlanejamentoDTO DtoFactory()
        {
            SituacaoPlanejamentoDTO situacaoPlanejamentoDto = new SituacaoPlanejamentoDTO()
            {
                Oid = Oid,
                TxDescricao = TxDescricao,
                CsSituacao = CsSituacao,
                CsTipo = CsTipo,
                CsPadrao = CsPadrao,
                KeyPress = KeyPress,
                TxKeys = TxKeys
            };

            if(BlImagem != null)
                situacaoPlanejamentoDto.BlImagemSituacaoPlanejamento = ImageUtil.ConverterImagemParaBytes( BlImagem );

            return situacaoPlanejamentoDto;
        }

        #endregion

        #region Construtores
        /// <summary>
        /// Quando o usu�rio tentar salvar.
        /// </summary>
        protected override void OnSaving()
        {
            if (CsPadrao == CsPadraoSistema.Sim)
                SituacaoPlanejamentoBO.DesabilitarSituacaoPlanejamentoPadraoAnterior( this.Session, this.Oid );

            TxKeys = new KeyShortcut(KeyPress).ToString();

            base.OnSaving();
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="session">sess�o</param>
        public SituacaoPlanejamento(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        #endregion
    }
}
