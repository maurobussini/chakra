using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.OAuth.Entities;

namespace ZenProgramming.Chakra.OAuth.ServiceLayers
{
    /// <summary>
    /// Service layer for manage password change on user
    /// </summary>
    /// <typeparam name="TUser">Type of user</typeparam>
    public interface IPasswordChangeServiceLayer<in TUser>
        where TUser : class, IUser, new()
    {
        /// <summary>
        /// Changes password for provided user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="confirmPassword">Confirm of new password</param>
        /// <returns>Returns validation</returns>
        IList<ValidationResult> ChangePassword(TUser user, string oldPassword, string newPassword, string confirmPassword);
    }
}
