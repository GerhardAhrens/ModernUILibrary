//-----------------------------------------------------------------------
// <copyright file="IQueryableExtensions.cs" company="Lifeprojects.de">
//     Class: IQueryableExtensions
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>03.04.2025 08:14:58</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Linq.Expressions;

    public static class IQueryableExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence according to a key and the sort order.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="query" />.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="key">Name of the property of <see cref="TSource"/> by which to sort the elements.</param>
        /// <param name="ascending">True for ascending order, false for descending order.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedQueryable`1" /> whose elements are sorted according to a key and sort order.</returns>
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string key, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            return ascending
                ? Queryable.OrderBy(query, lambda)
                : Queryable.OrderByDescending(query, lambda);
        }

        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }

        /// <summary>
        /// Sortiert die Elemente einer Folge nach einem Schlüssel und der Sortierreihenfolge.
        /// </summary>
        /// <typeparam name="TSource">Item vom Typ</typeparam>
        /// <param name="source">Liste von Items, die sortiert werden sollen</param>
        /// <param name="field">Propertyname</param>
        /// <param name="dir">Sortierrichtung</param>
        /// <returns>Sortierte Liste</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string field, string dir = "asc")
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "r");
            MemberExpression expression = Expression.Property(parameter, field);
            LambdaExpression lambda = Expression.Lambda(expression, parameter); // r => r.PropertyName
            Type typ = typeof(TSource).GetProperty(field).PropertyType;

            string methodeName = "OrderBy";
            if (string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase))
            {
                methodeName = "OrderByDescending";
            }
            var methode = typeof(Queryable).GetMethods().First(m => m.Name == methodeName && m.GetParameters().Length == 2);
            var methodeGeneric = methode.MakeGenericMethod(new[] { typeof(TSource), typ });
            return methodeGeneric.Invoke(source, new object[] { source, lambda }) as IOrderedQueryable<TSource>;

        }

        /// <summary>
        /// Gibt ein weiterers Sortierungskriterium nach 'OrderBy' an.
        /// </summary>
        /// <typeparam name="TSource">Item vom Typ</typeparam>
        /// <param name="source">Liste von Items, die sortiert werden sollen</param>
        /// <param name="field">Propertyname</param>
        /// <param name="dir">Sortierrichtung</param>
        /// <returns>Sortierte Liste</returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string field, string dir = "asc")
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "r");
            MemberExpression expression = Expression.Property(parameter, field);
            LambdaExpression lambda = Expression.Lambda<Func<TSource, string>>(expression, parameter); // r => r.PropertyName
            Type typ = typeof(TSource).GetProperty(field).PropertyType;

            string methodeName = "ThenBy";
            if (string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase))
            {
                methodeName = "ThenByDescending";
            }

            var methode = typeof(Queryable).GetMethods().First(m => m.Name == methodeName && m.GetParameters().Length == 2);
            var methodeGeneric = methode.MakeGenericMethod(new[] { typeof(TSource), typ });
            return methodeGeneric.Invoke(source, new object[] { source, lambda }) as IOrderedQueryable<TSource>;
        }
    }
}
