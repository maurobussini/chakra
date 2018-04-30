using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.OAuth.Entities;

namespace ZenProgramming.Chakra.OAuth.ServiceLayers
{
    /// <summary>
    /// Service layer for manage user
    /// </summary>
    /// <typeparam name="TUser">Type of user entity</typeparam>
    public interface IUserServiceLayer<TUser>
        where TUser: class, IUser, new()
    {
        /// <summary>
        /// Begin process for sign-up user on plaform
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="email">Email</param>
        /// <returns>Returns validation result</returns>
        IList<ValidationResult> BeginSignUp(string userName, string email);

        /// <summary>
        /// Confirm process of sign-up on platform
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="email">Email</param>
        /// <param name="confirmationCode">Confirmation code</param>
        /// <returns>Returns validations</returns>
        IList<ValidationResult> ConfirmSignUp(string userName, string email, string confirmationCode);

        /// <summary>
        /// Sign-in using credentials and return user
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="password">Password</param>
        /// <returns>Returns authenticated user</returns>
        TUser SignIn(string userName, string password);

        /// <summary>
        /// Get single user by username
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Returns instance or null</returns>
        TUser GetUser(string userName);

        /// <summary>
        /// Executes creation of user using provided entity
        /// </summary>
        /// <param name="user">User to create</param>
        /// <returns>Returns validations</returns>
        IList<ValidationResult> CreateUser(TUser user);

        /// <summary>
        /// Updates provided user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Returns validations</returns>
        IList<ValidationResult> UpdateUser(TUser user);
    }
}
