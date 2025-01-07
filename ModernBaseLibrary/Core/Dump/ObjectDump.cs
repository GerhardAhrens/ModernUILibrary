//-----------------------------------------------------------------------
// <copyright file="ObjectDump.cs" company="Lifeprojects.de">
//     Class: ObjectDump
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>09.08.2017</date>
//
// <summary>Definition of ObjectDumper Class for DumpAttribut</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class ObjectDump
    {
        private int level;
        private readonly int indentSize;
        private readonly StringBuilder stringBuilder;
        private readonly List<int> hashListOfFoundElements;

        private ObjectDump(int indentSize)
        {
            this.indentSize = indentSize;
            this.stringBuilder = new StringBuilder();
            this.hashListOfFoundElements = new List<int>();
        }

        public static string Dump(object element)
        {
            return Dump(element, 2, string.Empty);
        }

        public static string Dump(object element, string description)
        {
            return Dump(element, 2, description);
        }

        public static string Dump(object element, int indentSize, string description)
        {
            var instance = new ObjectDump(indentSize);
            return instance.DumpElement(element, description);
        }

        private string DumpElement(object element, string description)
        {
            if (element == null || element is ValueType || element is string)
            {
                this.Write(this.FormatValue(element));
            }
            else
            {
                var objectType = element.GetType();
                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    if (string.IsNullOrEmpty(description) == false)
                    {
                        this.Write("{{{0}}}", $"{objectType.FullName}-[{description}]");
                    }
                    else
                    {
                        this.Write("{{{0}}}", objectType.FullName);
                    }

                    this.hashListOfFoundElements.Add(element.GetHashCode());
                    this.level++;
                }

                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            this.level++;
                            DumpElement(item,string.Empty);
                            this.level--;
                        }
                        else
                        {
                            if (AlreadyTouched(item) == false)
                            {
                                DumpElement(item, string.Empty);
                            }
                            else
                            {
                                this.Write("{{{0}}} <-- bidirectional reference found", item.GetType().FullName);
                            }
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var memberInfo in members)
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        var propertyInfo = memberInfo as PropertyInfo;

                        if (fieldInfo == null && propertyInfo == null)
                        {
                            continue;
                        }

                        var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;

                        if (type.IsGenericParameter == false)
                        {
                            object value = fieldInfo != null
                                               ? fieldInfo.GetValue(element)
                                               : propertyInfo.GetValue(element, null);

                            if (type.IsValueType || type == typeof(string))
                            {
                                this.Write("{0}: {1}", memberInfo.Name, this.FormatValue(value));
                            }
                            else
                            {
                                var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                                this.Write("{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }");

                                var alreadyTouched = !isEnumerable && this.AlreadyTouched(value);
                                this.level++;
                                if (!alreadyTouched)
                                {
                                    DumpElement(value, string.Empty);
                                }
                                else
                                {
                                    this.Write("{{{0}}} <-- bidirectional reference found", value.GetType().FullName);
                                }

                                this.level--;
                            }
                        }
                    }
                }

                if (typeof(IEnumerable).IsAssignableFrom(objectType) == false)
                {
                    this.level--;
                }
            }

            return this.stringBuilder.ToString();
        }

        private bool AlreadyTouched(object value)
        {
            if (value == null)
                return false;

            var hash = value.GetHashCode();
            for (var i = 0; i < this.hashListOfFoundElements.Count; i++)
            {
                if (this.hashListOfFoundElements[i] == hash)
                {
                    return true;
                }
            }

            return false;
        }

        private void Write(string value, params object[] args)
        {
            var space = new string(' ', level * this.indentSize);

            if (args != null)
            {
                value = string.Format(value, args);
            }

            this.stringBuilder.AppendLine(space + value);
        }

        private string FormatValue(object o)
        {
            if (o == null)
            {
                return ("null");
            }

            if (o is DateTime)
            {
                return (((DateTime)o).ToShortDateString());
            }

            if (o is string)
            {
                return string.Format("\"{0}\"", o);
            }

            if (o is char && (char)o == '\0')
            {
                return string.Empty;
            }

            if (o is ValueType)
            {
                return (o.ToString());
            }

            if (o is IEnumerable)
            {
                return ("...");
            }

            return ("{ }");
        }
    }
}