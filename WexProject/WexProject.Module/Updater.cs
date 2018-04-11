using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using System;
using WexProject.BLL.Models.Rh;
using WexProject.Library.Properties;



namespace WexProject.Module
{
    /// <summary>
    /// Classe updater
    /// </summary>
    public class Updater : ModuleUpdater
    {
        /// <summary>
        /// Updater
        /// </summary>
        /// <param name="objectSpace">objectSpace</param>
        /// <param name="currentDBVersion">currentDBVersion</param>
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion)
        {

        }
        /// <summary>
        /// Atualiza a base após um update do programa
        /// </summary>
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            String userName = ADResources.AdminUser;
            String fullName = ADResources.AdminName;
            String passWord = ADResources.AdminPass;

            // If a user doesn't exist in the database, create this user 

            User admin = ObjectSpace.FindObject<User>(new BinaryOperator("UserName", userName));

            if (admin == null)
            {
                admin = ObjectSpace.CreateObject<User>();
                admin.UserName = userName;
                admin.FirstName = fullName;
                // Set a password if the standard authentication type is used 
                admin.SetPassword(passWord);
            }

            // If a role with the Administrators name doesn't exist in the database, create this role 
            Role adminRole = ObjectSpace.FindObject<Role>(new BinaryOperator("Name", "Administradores"));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<Role>();
                adminRole.Name = "Administradores";
            }
            // Delete all permissions assigned to the Administrators and Users roles 
            while (adminRole.PersistentPermissions.Count > 0)
            {
                ObjectSpace.Delete(adminRole.PersistentPermissions[0]);
            }
            // Allow full access to all objects to the Administrators role 
            adminRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            // Allow editing the Application Model to the Administrators role 
            adminRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            // Save the Administrators role to the database 
            adminRole.Save();
            // Add the Administrators role to the user1 
            admin.Roles.Add(adminRole);
            // Save the users to the database 
            admin.Save();

            ObjectSpace.CommitChanges();

            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
