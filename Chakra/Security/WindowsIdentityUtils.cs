using System;
using System.Security.Principal;

namespace ZenProgramming.Chakra.Security
{
    /// <summary>
    /// Utilities for windows identity and impersonation
    /// </summary>
    public static class WindowsIdentityUtils
    {
        /// <summary>
        /// Verify that current user connected with process is in specified role
        /// </summary>
        /// <param name="windowsRoleToCheck">Windows role to check</param>
        /// <returns>Returns true if user is in role</returns>
        public static bool CurrentUserInRole(WindowsBuiltInRole windowsRoleToCheck)
        {
            //Recupero l'identità dell'utente corrente
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            //Se l'oggetto è nullo, emetto eccezione
            if (identity == null)
                throw new NullReferenceException("Unable to identify current Windows identity.");

            //Recupero l'istanza del principal
            var principal = new WindowsPrincipal(identity);

            //Ritorno la verifica dell'utente corrente sul ruolo
            return principal.IsInRole(windowsRoleToCheck);
        }
    }
}
