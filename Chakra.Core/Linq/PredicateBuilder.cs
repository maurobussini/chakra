using System;
using System.Linq.Expressions;

namespace ZenProgramming.Chakra.Core.Linq
{
    /// <summary>
    /// Represents class for build predicates with Linq
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Get a true base expression
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>Returns expression</returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// Get a false base expression
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>Returns expression</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Join two predicates with "Or" operation
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="expr1">First expression</param>
        /// <param name="expr2">Second expression</param>
        /// <returns>Returns expression</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Join two predicates with "And" operation
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="expr1">First expression</param>
        /// <param name="expr2">Second expression</param>
        /// <returns>Returns expression</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
