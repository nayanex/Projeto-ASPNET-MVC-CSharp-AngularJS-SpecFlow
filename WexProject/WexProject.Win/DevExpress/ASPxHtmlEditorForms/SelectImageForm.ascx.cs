using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHtmlEditor.Localization;
using DevExpress.Web.ASPxClasses.Internal;
using DevExpress.Web.ASPxFileManager;

/// <summary>
/// classse SelectImageForm
/// </summary>
public partial class SelectImageForm : HtmlEditorUserControl
{
    /// <summary>
    /// PrepareChildControls
    /// </summary>
    protected override void PrepareChildControls()
    {
        Localize();

        base.PrepareChildControls();

        PrepareFileManager();
    }
    /// <summary>
    /// Localize
    /// </summary>
    public void Localize()
    {
        SelectButton.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonSelect);
        CancelButton.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonCancel);
    }
    /// <summary>
    /// GetChildDxEdits
    /// </summary>
    /// <returns>ASPxEditBase</returns>
    protected override ASPxEditBase[] GetChildDxEdits()
    {
        return new ASPxEditBase[] { };
    }
    /// <summary>
    /// GetChildDxButtons
    /// </summary>
    /// <returns>ASPxButton</returns>
    protected override ASPxButton[] GetChildDxButtons()
    {
        return new ASPxButton[] { SelectButton, CancelButton };
    }
    /// <summary>
    /// GetChildDxHtmlEditorRoundPanels
    /// </summary>
    /// <returns>ASPxHtmlEditorRoundPanel</returns>
    protected override ASPxHtmlEditorRoundPanel[] GetChildDxHtmlEditorRoundPanels()
    {
        return new ASPxHtmlEditorRoundPanel[] { };
    }
    /// <summary>
    /// PrepareFileManager
    /// </summary>
    protected void PrepareFileManager()
    {
        FileManager.Styles.CopyFrom(HtmlEditor.StylesFileManager);
        FileManager.ControlStyle.CopyFrom(HtmlEditor.StylesFileManager.Control);
        FileManager.Images.CopyFrom(HtmlEditor.ImagesFileManager);
        FileManager.Settings.Assign(HtmlEditor.SettingsImageSelector.CommonSettings);
        FileManager.SettingsEditing.Assign(HtmlEditor.SettingsImageSelector.EditingSettings);
        FileManager.SettingsFolders.Assign(HtmlEditor.SettingsImageSelector.FoldersSettings);
        FileManager.SettingsToolbar.Assign(HtmlEditor.SettingsImageSelector.ToolbarSettings);
        FileManager.SettingsUpload.Assign(HtmlEditor.SettingsImageSelector.UploadSettings);
        FileManager.SettingsUpload.ValidationSettings.Assign(HtmlEditor.SettingsImageUpload.ValidationSettings);
        FileManager.SettingsPermissions.Assign(HtmlEditor.SettingsImageSelector.PermissionSettings);

        if (string.IsNullOrEmpty(FileManager.Settings.RootFolder))
            FileManager.Settings.RootFolder = HtmlEditor.SettingsImageUpload.UploadImageFolder;

        FileManager.FolderCreating += new FileManagerFolderCreateEventHandler(FileManager_FolderCreating);
        FileManager.ItemDeleting += new FileManagerItemDeleteEventHandler(FileManager_ItemDeleting);
        FileManager.ItemMoving += new FileManagerItemMoveEventHandler(FileManager_ItemMoving);
        FileManager.ItemRenaming += new FileManagerItemRenameEventHandler(FileManager_ItemRenaming);
        FileManager.FileUploading += new FileManagerFileUploadEventHandler(FileManager_FileUploading);
    }
    /// <summary>
    /// FileManager_FolderCreating
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">FileManagerFolderCreateEventArgs</param>
    protected void FileManager_FolderCreating(object sender, FileManagerFolderCreateEventArgs e)
    {
        HtmlEditor.RaiseImageSelectorFolderCreating(e);
    }
    /// <summary>
    /// FileManager_FolderCreating
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">FileManagerItemDeleteEventArgs</param>
    protected void FileManager_ItemDeleting(object sender, FileManagerItemDeleteEventArgs e)
    {
        HtmlEditor.RaiseImageSelectorItemDeleting(e);
    }
    /// <summary>
    /// FileManager_ItemMoving
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">FileManagerItemMoveEventArgs</param>
    protected void FileManager_ItemMoving(object sender, FileManagerItemMoveEventArgs e)
    {
        HtmlEditor.RaiseImageSelectorItemMoving(e);
    }
    /// <summary>
    /// FileManager_ItemRenaming
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">FileManagerItemRenameEventArgs</param>
    protected void FileManager_ItemRenaming(object sender, FileManagerItemRenameEventArgs e)
    {
        HtmlEditor.RaiseImageSelectorItemRenaming(e);
    }
    /// <summary>
    /// FileManager_FileUploading
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">FileManagerFileUploadEventArgs</param>
    protected void FileManager_FileUploading(object sender, FileManagerFileUploadEventArgs e)
    {
        HtmlEditor.RaiseImageSelectorFileUploading(e);
    }
    /// <summary>
    /// FileManager_CustomJSProperties
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">DevExpress</param>
    protected void FileManager_CustomJSProperties(object sender, DevExpress.Web.ASPxClasses.CustomJSPropertiesEventArgs e)
    {
        e.Properties["cp_RootFolderRelativePath"] = FileManager.GetAppRelativeRootFolder();
    }
}
