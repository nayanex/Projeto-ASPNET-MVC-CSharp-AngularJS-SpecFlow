using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using System.Configuration;

namespace ShortcutsDemoApp
{
    /// <summary>
    /// Clase responsável por criar atalho na área de trabalho
    /// e barra de inicialização rápida 
    /// </summary>
    [RunInstaller(true)]
    public class ShortcutInstaller : Installer
    {
        #region Attribute
        /// <summary>
        /// Local de instalação
        /// </summary>
        private string _location = null;

        /// <summary>
        /// Nome
        /// </summary>
        private string _name = null;

        /// <summary>
        /// Descrição
        /// </summary>
        private string _description = null;
        #endregion

        #region Properties
        /// <summary>
        /// Retorna o caminho da barra de inicialização rápida
        /// </summary>
        private string QuickLaunchFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                       "\\Microsoft\\Internet Explorer\\Quick Launch";
            }
        }

        /// <summary>
        /// Retorno o endereço de instalção do atalho
        /// </summary>
        private string ShortcutTarget
        {
            get
            {
                if (_location == null)
                    _location = Assembly.GetExecutingAssembly().Location;
                return _location;
            }
        }

        /// <summary>
        /// Retorna o nome do atalho
        /// </summary>
        private string ShortcutName
        {
            get
            {
                if (_name == null)
                {
                    Assembly myAssembly = Assembly.GetExecutingAssembly();

                    try
                    {
                        object titleAttribute = myAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
                        _name = ((AssemblyTitleAttribute)titleAttribute).Title;
                    }
                    catch { }

                    if ((_name == null) || (_name.Trim() == string.Empty))
                        _name = myAssembly.GetName().Name;
                }
                return _name;
            }
        }

        /// <summary>
        /// Retorna a descrição
        /// </summary>
        private string ShortcutDescription
        {
            get
            {
                if (_description == null)
                {
                    Assembly myAssembly = Assembly.GetExecutingAssembly();

                    try
                    {
                        object descriptionAttribute = myAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];
                        _description = ((AssemblyDescriptionAttribute)descriptionAttribute).Description;
                    }
                    catch { }

                    if ((_description == null) || (_description.Trim() == string.Empty))
                        _description = "Launch " + ShortcutName;
                }
                return _description;
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Método disparado ao iniciar a instalação
        /// </summary>
        /// <param name="savedState">dicionário de estados</param>
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);

            const string DESKTOP_SHORTCUT_PARAM = "DESKTOP_SHORTCUT";
            const string QUICKLAUNCH_SHORTCUT_PARAM = "QUICKLAUNCH_SHORTCUT";
            const string ALLUSERS_PARAM = "ALLUSERS";

            // The installer will pass the ALLUSERS, DESKTOP_SHORTCUT and QUICKLAUNCH_SHORTCUT   
            // parameters. These have been set to the values of radio buttons and checkboxes from the
            // MSI user interface.
            // ALLUSERS is set according to whether the user chooses to install for all users (="1") 
            // or just for themselves (="").
            // If the user checked the checkbox to install one of the shortcuts, then the corresponding 
            // parameter value is "1".  If the user did not check the checkbox to install one of the 
            // desktop shortcut, then the corresponding parameter value is an empty string.

            // First make sure the parameters have been provided.
            if (!Context.Parameters.ContainsKey(ALLUSERS_PARAM))
                throw new Exception(string.Format("O parâmetro {0} não foi fornecido para a classe {1}.", ALLUSERS_PARAM, this.GetType()));
            if (!Context.Parameters.ContainsKey(DESKTOP_SHORTCUT_PARAM))
                throw new Exception(string.Format("O parâmetro {0} não foi fornecido para a classe {1}.", DESKTOP_SHORTCUT_PARAM, this.GetType()));
            if (!Context.Parameters.ContainsKey(QUICKLAUNCH_SHORTCUT_PARAM))
                throw new Exception(string.Format("O parâmetro {0} não foi fornecido para a classe {1}.", QUICKLAUNCH_SHORTCUT_PARAM, this.GetType()));

            bool allusers = Context.Parameters[ALLUSERS_PARAM] != string.Empty;
            bool installDesktopShortcut = Context.Parameters[DESKTOP_SHORTCUT_PARAM] != string.Empty;
            bool installQuickLaunchShortcut = Context.Parameters[QUICKLAUNCH_SHORTCUT_PARAM] != string.Empty;

            if (installDesktopShortcut)
            {
                // If this is an All Users install then we need to install the desktop shortcut for 
                // all users.  .Net does not give us access to the All Users Desktop special folder,
                // but we can get this using the Windows Scripting Host.
                string desktopFolder = null;

                if (allusers)
                {
                    try
                    {
                        // This is in a Try block in case AllUsersDesktop is not supported
                        object allUsersDesktop = "AllUsersDesktop";
                        WshShell shell = new WshShell();
                        desktopFolder = shell.SpecialFolders.Item(ref allUsersDesktop).ToString();
                    }
                    catch { }
                }
                if (desktopFolder == null)
                    desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                CreateShortcut(desktopFolder, ShortcutName, ShortcutTarget, ShortcutDescription);
            }

            if (installQuickLaunchShortcut)
            {
                CreateShortcut(QuickLaunchFolder, ShortcutName, ShortcutTarget, ShortcutDescription);
            }

            // Chamada do método que define o NewVersionServer
           // SettingConfiguration();

        }

        /// <summary>
        /// Define o valor da chave 'NewVersionServer' para atualização da aplicação
        /// </summary>
        private void SettingConfiguration()
        {
            string targetDirectory = Context.Parameters["TARGETDIR"].ToString();
            string pathValue = Context.Parameters["TXT_PATH"];

            string exePath = string.Format("{0}\\Teste.Win.exe", targetDirectory);

            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            //
            config.AppSettings.Settings["NewVersionServer"].Value = @pathValue;
            config.Save();
        }



        /// <summary>
        /// Ao desinstalar a aplicação
        /// </summary>
        /// <param name="savedState">dicionário de estados</param>
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            DeleteShortcuts();
        }

        /// <summary>
        /// Ao cancelar a instalação
        /// </summary>
        /// <param name="savedState">dicionário de estados</param>
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);

            DeleteShortcuts();
        }


        #endregion

        #region RuleBusiness
        /// <summary>
        /// Método reponsável por criar o atalho
        /// </summary>
        /// <param name="folder">Diretório do Atalho</param>
        /// <param name="name">Nome do Atalho</param>
        /// <param name="target">... do Atalho</param>
        /// <param name="description">Descrição do Atalho</param>
        private static void CreateShortcut(string folder, string name, string target, string description)
        {
            string shortcutFullName = Path.Combine(folder, name + ".lnk");

            try
            {
                WshShell shell = new WshShell();
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(shortcutFullName);
                link.TargetPath = target;
                link.Description = description;
                link.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("The shortcut \"{0}\" could not be created.\n\n{1}", shortcutFullName, ex.ToString()),
                       "Create Shortcut", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Método reponsável preparar a exclusão dos atalhos
        /// </summary>
        private void DeleteShortcuts()
        {
            try
            {
                // This is in a Try block in case AllUsersDesktop is not supported
                object allUsersDesktop = "AllUsersDesktop";
                WshShell shell = new WshShell();
                string desktopFolder = shell.SpecialFolders.Item(ref allUsersDesktop).ToString();
                DeleteShortcut(desktopFolder, ShortcutName);
            }
            catch { }

            DeleteShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ShortcutName);

            DeleteShortcut(QuickLaunchFolder, ShortcutName);
        }

        /// <summary>
        /// Método reponsável por excluir os atalhos
        /// </summary>
        /// <param name="folder">Diretório do atalho</param>
        /// <param name="name">Nome do Atalho</param>              
        private void DeleteShortcut(string folder, string name)
        {
            string shortcutFullName = Path.Combine(folder, name + ".lnk");
            FileInfo shortcut = new FileInfo(shortcutFullName);
            if (shortcut.Exists)
            {
                try
                {
                    shortcut.Delete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("The shortcut \"{0}\" could not be deleted.\n\n{1}", shortcutFullName, ex.ToString()),
                          "Delete Shortcut", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

    }
}
