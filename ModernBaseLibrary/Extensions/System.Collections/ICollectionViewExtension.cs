/*
 * <copyright file="ICollectionViewExtension.cs" company="Lifeprojects.de">
 *     Class: ICollectionViewExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extension Class for ICollectionView Types
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Extension
{
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Data;

    public static class ICollectionViewExtension
    {
        public static bool IsNullOrEmpty<T>(this ICollectionView @this)
        {
            return @this == null || @this.OfType<T>().Count() == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this ICollectionView @this)
        {
            return @this != null || @this.OfType<T>().Count() > 0;
        }

        public static int Count<TSource>(this ICollectionView @this)
        {
            if (@this == null)
            {
                return 0;
            }
            else
            {
                return @this.OfType<TSource>().Count();
            }
        }

        public static int Count<TSource>(this ICollectionView @this, Func<TSource, bool> predicate)
        {
            if (@this == null)
            {
                return 0;
            }
            else
            {
                return @this.OfType<TSource>().Count(predicate);
            }
        }

        public static List<TSource> ToList<TSource>(this ICollectionView @this)
        {
            if (@this == null)
            {
                return new List<TSource>();
            }
            else
            {
                return @this.OfType<TSource>().ToList<TSource>();
            }
        }

        public static IEnumerable<TSource> ToIEnumerable<TSource>(this ICollectionView @this)
        {
            if (@this == null)
            {
                return new List<TSource>();
            }
            else
            {
                return @this.OfType<TSource>().ToList<TSource>();
            }
        }

        public static DataTable ToDataTable<TSource>(this ICollectionView @this)
        {
            DataTable tb = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (props != null)
            {
                foreach (var prop in props)
                {
                    tb.Columns.Add(prop.Name, prop.PropertyType);
                }

                foreach (var item in @this)
                {
                    var values = new object[props.Length];
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }

                    tb.Rows.Add(values);
                }
            }

            return tb;
        }

        public static DataTable ToDataTable(this ICollectionView @this)
        {
            List<object> li = new List<object>();
            var myEnumarator = @this.SourceCollection.GetEnumerator();
            while (myEnumarator.MoveNext())
            {
                Type t = myEnumarator.Current.GetType();
                li.Add(myEnumarator.Current);
            }

            return default;
        }

        public static DataTable ToDataTable<TSource>(this ICollectionView @this, Func<TSource, bool> action) where TSource : class
        {
            DataTable dt = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (props != null)
            {
                foreach (var prop in props)
                {
                    dt.Columns.Add(prop.Name, prop.PropertyType);
                }

                foreach (TSource item in @this.OfType<TSource>().Where(action))
                {
                    var values = new object[props.Length];
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }

                    dt.Rows.Add(values);
                }
            }

            return dt;
        }

        public static DataTable ToDataTable<TSource>(this ICollectionView @this, Func<TSource, bool> actionWhere, Func<TSource, object> actionOrder) where TSource : class
        {
            DataTable dt = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (props != null)
            {
                foreach (var prop in props)
                {
                    dt.Columns.Add(prop.Name, prop.PropertyType);
                }

                foreach (TSource item in @this.OfType<TSource>().Where(actionWhere).OrderBy(actionOrder))
                {
                    var values = new object[props.Length];
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }

                    dt.Rows.Add(values);
                }
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns>IQueryable</returns>
        /// <example>
        /// var query = overviewSource.AsQueryable<PasswordPin>().ToWhere(x => x.AccessTyp == AccessTyp.License).ToList();
        /// </example>
        public static IQueryable<TSource> ToWhere<TSource>(this IQueryable<TSource> @this, Expression<Func<TSource, bool>> action) where TSource : class
        {
            var expression = Expression.Equal(Expression.Condition(action.Body, Expression.Constant(false), Expression.Constant(true), typeof(bool)), Expression.Constant(false));

            var methodCallExpression = Expression.Call(typeof(Queryable),
                "where",
                new Type[] { @this.ElementType },
                @this.Expression,
                Expression.Lambda<Func<TSource, bool>>(expression, action.Parameters));

            return @this.Provider.CreateQuery<TSource>(methodCallExpression);
        }
    }
}
