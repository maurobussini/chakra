using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.OAuth.Entities;

namespace ZenProgramming.Chakra.OAuth.ServiceLayers
{
    /// <summary>
    /// Service layer for manage email change on user
    /// </summary>
    /// <typeparam name="TUser">Type of user</typeparam>
    public interface IEmailChangeServiceLayer<in TUser>
        where TUser : class, IUser, new()
    {
        /// <summary>
        /// Begin "email change" process (i.e. sending 
        /// confirmation code to provided new email)
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        /// <returns>Returns validation result</returns>
        IList<ValidationResult> BeginEmailChange(TUser user, string newEmail);

        /// <summary>
        /// Completes "email change" process (i.e. verifying confirmation code)
        /// executing change on storage
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        /// <param name="confirmationCode">Confirmation code</param>
        /// <returns>Returns list of validations</returns>
        IList<ValidationResult> ConfirmEmailChange(TUser user, string newEmail, string confirmationCode);
    }
}
