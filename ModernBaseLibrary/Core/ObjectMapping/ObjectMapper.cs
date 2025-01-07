//-----------------------------------------------------------------------
// <copyright file="ObjectMapper.cs" company="Lifeprojects.de">
//     Class: ObjectMapper
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>28.04.2022</date>
//
// <summary>
// Definition of ObjectMapper
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class ObjectMapper
    {
        public static List<T> MapToObject<T>(List<Dictionary<string, string>> dictList)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<T> result = new List<T>();

            foreach (var item in dictList)
            {
                var model = Activator.CreateInstance<T>();
                foreach (var prop in properties)
                {
                    if (item.ContainsKey(prop.Name))
                    {
                        prop.SetValue(model, item[prop.Name]);
                    }
                }

                result.Add(model);
            }

            return result;
        }
    }
}
