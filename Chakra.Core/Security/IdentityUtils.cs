using System.Security.Principal;
using System.Threading;

namespace ZenProgramming.Chakra.Core.Security
{
    /// <summary>
    /// Utilities for identity
    /// </summary>
    public static class IdentityUtils
    {
        private static IPrincipal _CurrentPrincipal;

        /// <summary>
        /// Get current available principal
        /// </summary>
        /// <returns></returns>
        public static IPrincipal ResolvePrincipal()
        {
            //Verifico se siamo in contesto "web"
            return _CurrentPrincipal ?? Thread.CurrentPrincipal;
        }

        public static void RegisterPrincipal(IPrincipal instance)
        {
            _CurrentPrincipal = instance;
        }
    }
}
