//-----------------------------------------------------------------------
// <copyright file="PropertyName.cs" company="Lifeprojects.de">
//     Class: PropertyName
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>23.06.2023 07:49:22</date>
//
// <summary>
// Gets property name using lambda expressions.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.MVVM.Base
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.Versioning;

    [DebuggerStepThrough]
    [Serializable]
    [SupportedOSPlatform("windows")]
    internal static class PropertyName
    {
        public static string For<T>(Expression<Func<T, object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }

        public static string For(Expression<Func<object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }

        public static string GetMemberName(Expression expression)
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
                    throw new Exception(string.Format("Cannot interpret member from {0}", expression));
                }

                return GetMemberName(unaryExpression.Operand);
            }

            throw new Exception(string.Format("Could not determine member from {0}", expression));
        }
    }
}
