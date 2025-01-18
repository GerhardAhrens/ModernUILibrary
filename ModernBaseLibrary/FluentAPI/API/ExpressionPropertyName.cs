//-----------------------------------------------------------------------
// <copyright file="ExpressionPropertyName.cs" company="Lifeprojects.de">
//     Class: ExpressionPropertyName
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>21.07.2020</date>
//
// <summary>
// Die Klasse löst über den Expression-Parameter den übergeben Property Name auf
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Linq.Expressions;

    internal class ExpressionPropertyName
    {
        public static string For<TProperty>(Expression<Func<TProperty, object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }

        public static string For(Expression<Func<string>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;

                if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    return $"{GetMemberName(memberExpression.Expression)}.{memberExpression.Member.Name}";
                }

                return memberExpression.Member.Name;
            }

            if (expression is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression;

                if (unaryExpression.NodeType != ExpressionType.Convert)
                {
                    throw new Exception($"Cannot interpret member from {expression}");
                }

                return GetMemberName(unaryExpression.Operand);
            }

            throw new Exception($"Could not determine member from {expression}");
        }
    }
}
