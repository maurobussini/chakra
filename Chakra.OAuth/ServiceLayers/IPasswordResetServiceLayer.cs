using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.OAuth.Entities;

namespace ZenProgramming.Chakra.OAuth.ServiceLayers
{
    /// <summary>
    /// Service layer for manage password reset on user
    /// </summary>
    /// <typeparam name="TUser">Type of user</typeparam>
    public interface IPasswordResetServiceLayer<in TUser>
        where TUser : class, IUser, new()
    {
        /// <summary>
        /// Begin 'password reset' process (i.e. sending 
        /// confirmation code to user email)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Returns validation result</returns>
        IList<ValidationResult> BeginPasswordReset(TUser user);

        /// <summary>
        /// Complete 'password reset' process (i.e. verifying confirmation code
        /// sento user email address) and setting new random password on the user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="confirmationCode">Confirmation code</param>
        /// <returns>Returns list of validations</returns>
        IList<ValidationResult> ConfirmPasswordReset(TUser user, string confirmationCode);

        /// <summary>
        /// Executes reset password and 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns password reset</returns>
        string ResetPassword(TUser user);
    }
}
