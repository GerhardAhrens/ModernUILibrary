/*
 * <copyright file="CustomAttributeReader.cs" company="Lifeprojects.de">
 *     Class: CustomAttributeReader
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>06.04.2017</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Class with Definition for CustomAttributeReader Definition
 * </summary>
 *
 * <WebLink>
 * </WebLink>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class CustomAttributeReader
    {
        public static IList<string> GetAttributs<TAttribut>(Type customAttribut) where TAttribut : Attribute
        {
            IList<string> propertyFields = new List<string>();
            var properties = typeof(TAttribut).GetProperties();

            foreach (var property in properties.AsParallel())
            {
                if (customAttribut == null)
                {
                    propertyFields.Add(property.Name);
                }
                else
                {
                    var attributes = property.GetCustomAttributes(false);
                    foreach (var attribute in attributes.AsParallel())
                    {
                        if (attribute.GetType().Name == customAttribut.Name)
                        {
                            propertyFields.Add(property.Name);
                        }
                    }
                }
            }

            return propertyFields;
        }

        public static IList<string> GetAttributs<TAttribut>(Type customAttribut, BindingFlags bindingAttr)
        {
            IList<string> propertyFields = new List<string>();
            var properties = typeof(TAttribut).GetProperties(bindingAttr);

            foreach (var property in properties.AsParallel())
            {
                if (customAttribut == null)
                {
                    propertyFields.Add(property.Name);
                }
                else
                {
                    var attributes = property.GetCustomAttributes(false);
                    foreach (var attribute in attributes.AsParallel())
                    {
                        if (attribute.GetType().Name == customAttribut.Name)
                        {
                            propertyFields.Add(property.Name);
                        }
                    }
                }
            }

            return propertyFields;
        }

        public static IList<string> GetClassAttributs<TModel>(Type customAttribut) 
        {
            IList<string> propertyFields = new List<string>();

            var attributesClass = typeof(TModel).GetCustomAttributes(customAttribut, true);
            foreach (var attributeCalls in attributesClass.AsParallel())
            {
                if (attributeCalls.GetType().Name == customAttribut.Name)
                {
                    var properties = typeof(TModel).GetProperties();
                    foreach (var property in properties.AsParallel())
                    {
                        propertyFields.Add(property.Name);
                    }
                }
            }

            return propertyFields;
        }

        private List<TValue> GetClassAttribute<TValue>(Assembly assembly) where TValue : Attribute
        {
            List<TValue> attrList = new List<TValue>();

            var propertiesEx = from type in assembly.GetTypes()
                               let attrs = type.GetCustomAttributes(typeof(TValue), true)
                               where attrs.Any()
                               select new { Property = type, Attr = (TValue)attrs.First() };

            foreach (var attribute in propertiesEx.AsParallel())
            {
                if (attribute.Attr is TValue)
                {
                    dynamic attrMember = attribute.Attr;
                    attrMember.MemberName = attribute.Property.FullName;
                    attrList.Add(attrMember as TValue);
                }
            }

            return attrList;
        }

    }
}