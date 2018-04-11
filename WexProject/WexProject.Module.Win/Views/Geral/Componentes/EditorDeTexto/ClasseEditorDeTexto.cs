using System;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraEditors.Repository;


namespace WexProject.Module.Win.Projeto
{
    [PropertyEditor(typeof(String), false)]
    /// <summary>
    /// classe de editor de texto e herda de  WinPropertyEditor e IInplaceEditSupport
    /// </summary>
    public class ClasseEditorDeTexto : WinPropertyEditor, IInplaceEditSupport
    {
        /// <summary>
        /// Construtor da classe editor de texto
        /// </summary>
        /// <param name="objectType">tipo de objeto</param>
        /// <param name="model">instancia do model</param>
        public ClasseEditorDeTexto(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
            ControlBindingProperty = "RtfText";
        }

        /// <summary>
        /// controle de editor de texto
        /// </summary>
        private EditorDeTexto editorDeTextoControlCore;

        /// <summary>
        /// metodo editor de texto
        /// </summary>
        public EditorDeTexto EditorDeTexto
        {
            get
            {
                if (editorDeTextoControlCore != null)
                    return editorDeTextoControlCore;
                else
                    return editorDeTextoControlCore = new EditorDeTexto();
            }
        }

        /// <summary>
        /// criando controle de core
        /// </summary>
        /// <returns>retorna editorDeTextoControlCore</returns>
        protected override object CreateControlCore()
        {
            editorDeTextoControlCore = new EditorDeTexto();
            return editorDeTextoControlCore;
        }

        /// <summary>
        /// Criação de um item no repositorio
        /// </summary>
        /// <returns>retorna RepositoryItemRichTextEdit</returns>
        public RepositoryItem CreateRepositoryItem()
        {
            return new RepositoryItemRichTextEdit();
        }

    }
}
