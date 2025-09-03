//-----------------------------------------------------------------------
// <copyright file="ExpressionPropertyName.cs" company="Lifeprojects.de">
//     Class: ExpressionPropertyName
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.05.2025 14:27:39</date>
//
// <summary>
// Klasse zur Auswertung von Expression
// </summary>
//-----------------------------------------------------------------------

namespace ModernSqlServer.Generator
{
    using System;
    using System.Linq.Expressions;

    public class ExpressionPropertyName
    {
        public static string For<TProperty>(Expression<Func<TProperty, object>> expression)
        {
            return GetMemberName(expression);
        }

        public static string For(Expression<Func<string>> expression)
        {
            return GetMemberName(expression);
        }

        private static string GetMemberName(LambdaExpression expression)
        {
            var currentExpression = expression.Body;

            while (true)
            {
                switch (currentExpression.NodeType)
                {
                    case ExpressionType.Parameter:
                        return ((ParameterExpression)currentExpression).Name;
                    case ExpressionType.MemberAccess:
                        return ((MemberExpression)currentExpression).Member.Name;
                    case ExpressionType.Call:
                        return ((MethodCallExpression)currentExpression).Method.Name;
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        currentExpression = ((UnaryExpression)currentExpression).Operand;
                        break;
                    case ExpressionType.Invoke:
                        currentExpression = ((InvocationExpression)currentExpression).Expression;
                        break;
                    case ExpressionType.ArrayLength:
                        return "Length";
                    default:
                        throw new Exception("not a proper member selector");
                }
            }
        }

        public static string NameOfProperty<T>(Expression<Func<T, object>> expression)
        {
            // Validate argument.
            _ = expression ?? throw new ArgumentNullException(nameof(expression));
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("The expression must be a member access expression", nameof(expression));
            }

            // Now that we know we have a member expression in the body
            // we can cast it, get a reference to the MemberInfo
            // and return the property name.
            var memberExpression = (MemberExpression)expression.Body;
            return memberExpression.Member.Name;
        }
    }
}
