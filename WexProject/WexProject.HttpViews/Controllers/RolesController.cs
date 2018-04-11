using System;
using System.Web.Mvc;
using System.Web.Security;

namespace WexProject.HttpViews.Controllers
{
    public class RolesController : Controller
    {
        //
        // /Roles/

        [HttpGet]
        [Authorize(Roles = "Sudo")]
        public ActionResult Index()
        {
            string[] rolesArray;
            rolesArray = Roles.GetAllRoles();
            return Json(new { roles = rolesArray }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("Index")]
        [Authorize(Roles = "Sudo")]
        public ActionResult RolesAdd(string role)
        {
            try
            {
                if (Roles.RoleExists(role))
                {
                    return Json(new { error = "Papel '" + role + "' já existe. Por favor, especifique um nome diferente." },
                        JsonRequestBehavior.DenyGet);
                }

                Roles.CreateRole(role);
                return Json(new { success = "Papel '" + role + "' criado com sucesso." }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [Authorize(Roles = "Sudo")]
        public ActionResult RolesDel(string role)
        {
            try
            {
                if (Roles.RoleExists(role))
                {
                    Roles.DeleteRole(role);
                    return Json(new { success = "Papel '" + role + "' removido com sucesso." }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { error = "Papel '" + role + "' não existe. Por favor, especifique um nome diferente." },
                        JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }

        //
        // Roles and Users

        [HttpGet]
        [ActionName("CurrentUser")]
        [Authorize]
        public ActionResult GetByCurrentUser()
        {
            string[] userRoles;
            userRoles = Roles.GetRolesForUser(User.Identity.Name);
            return Json(new { roles = userRoles }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("user")]
        [Authorize(Roles = "Sudo")]
        public ActionResult GetByUserName(string name)
        {
            string[] userRoles;
            userRoles = Roles.GetRolesForUser(name);
            return Json(new { roles = userRoles }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("UsersIn")]
        [Authorize(Roles = "Sudo")]
        public ActionResult GetUsersInRole(string role)
        {
            string[] usersInRoles;
            usersInRoles = Roles.GetUsersInRole(role);
            return Json(new { roles = usersInRoles }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("paper")]
        [Authorize(Roles = "Sudo")]
        public ActionResult RolesAddUser(string role, string username)
        {
            try
            {
                Roles.AddUserToRole(username, role);
                return Json(new { success = "Usuário '" + username + "' adicionado com sucesso ao papel '" + role + "'." },
                    JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpDelete]
        [ActionName("paper")]
        [Authorize(Roles = "Sudo")]
        public ActionResult RolesDelUser(string role, string username)
        {
            try
            {
                Roles.RemoveUserFromRole(username, role);
                return Json(new { success = "Usuário '" + username + "' removido com sucesso do papel '" + role + "'." },
                    JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}
