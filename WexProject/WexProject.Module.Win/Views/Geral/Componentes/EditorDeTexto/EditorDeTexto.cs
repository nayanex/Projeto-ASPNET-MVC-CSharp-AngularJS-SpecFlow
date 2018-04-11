using System;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraBars;
using DevExpress.ExpressApp.Editors;

namespace WexProject.Module.Win.Projeto
{
    /// <summary>
    /// Criação de classe para editor de texto
    /// </summary>
    public partial class EditorDeTexto : XtraUserControl
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public EditorDeTexto()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Criação de classe para edição de atributos
        /// </summary>
        public PropertyEditorAttribute Atribute
        {
            get; set;
        }

        /// <summary>
        /// Método que retorna os controles do componente RichText
        /// </summary>
        public RichEditControl RichEditControl
        {
            get
            {
                return richEditControl1;
            }
        }
        /// <summary>
        /// Método de retorno da barra
        /// </summary>
        public BarManager BarManager
        {
            get
            {
                return barManager1;
            }
        }
        /// <summary>
        /// Classe de RichText verifica se o componente possui dados, se possuir retorna este valor ou nulo
        /// Caso seja para adicionar um novo dado ele dá a possibilidade ao usuário
        /// </summary>
        public string RtfText
        {
            get
            {
                if (richEditControl1 != null)
                {
                    return richEditControl1.RtfText;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (richEditControl1 != null)
                {
                    richEditControl1.RtfText = value;
                }
            }
        }
    }
}
