using System;
using System.Text.RegularExpressions;

namespace WexProject.Library.Libs.Email
{
    /// <summary>
    /// Classe Útil a Emails
    /// </summary>
    public class EmailUtil
    {
        /// <summary>
        /// Validação de email
        /// </summary>
        /// <param name="txEmail">Email</param>
        /// <returns>True, se for um email válido e False se não for</returns>
        public static bool ValidarEmail(string txEmail)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (string.IsNullOrEmpty(txEmail) || !rg.IsMatch(txEmail))
                return false;

            return true;
        }
    }
}
