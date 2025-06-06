﻿//-----------------------------------------------------------------------
// <copyright file="ObjectDumperA.cs" company="Lifeprojects.de">
//     Class: ObjectDumper
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>30.03.2021</date>
//
// <summary>Definition of ObjectDumper Class for all Objects</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public class ObjectDumper
    {
        private TextWriter writer;
        private int pos;
        private int level;
        private readonly int depth;

        public static void Write(object element)
        {
            Write(element, 0);
        }

        public static void Write(object element, int depth)
        {
            Write(element, depth, Console.Out);
        }

        public static void Write(object element, int depth, TextWriter log)
        {
            ObjectDumper dumper = new ObjectDumper(depth);
            dumper.writer = log;
            dumper.WriteObject(null, element);
        }

        private ObjectDumper(int depth)
        {
            this.depth = depth;
        }

        private void Write(string s)
        {
            if (s != null)
            {
                writer.Write(s);
                pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < level; i++) writer.Write(" ");
        }

        private void WriteLine()
        {
            writer.WriteLine();
            pos = 0;
        }

        private void WriteTab()
        {
            Write(" ");
            while (pos % 8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                IEnumerable enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (level < depth)
                            {
                                level++;
                                WriteObject(prefix, item);
                                level--;
                            }
                        }
                        else
                        {
                            WriteObject(prefix, item);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance /*| BindingFlags.NonPublic */);
                    Console.WriteLine(members.Length + " Members.");
                    WriteIndent();
                    Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo m in members)
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            if (propWritten)
                            {
                                WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }
                            Write(m.Name);
                            Write("=");
                            Type t = f != null ? f.FieldType : p.PropertyType;
                            object o = null;
                            if (t.IsValueType || t == typeof(string))
                            {
                                try
                                {

                                    o = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    WriteValue(o);
                                }
                                catch (Exception ex)
                                {
                                    // Had to do this because exceptions are thrown on NumberFormatInfo and other types
                                    // at high depth levels, even though there is a legitimate value obtained. 
                                    Console.WriteLine(ex.Message);
                                    WriteValue(o);
                                }
                            }
                            else
                            {
                                if (typeof(IEnumerable).IsAssignableFrom(t))
                                {
                                    Write("...");
                                }
                                else
                                {
                                    Write("{ }");
                                }
                            }
                        }
                    }
                    if (propWritten) WriteLine();
                    if (level < depth)
                    {
                        foreach (MemberInfo m in members)
                        {
                            FieldInfo f = m as FieldInfo;
                            PropertyInfo p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                Type t = f != null ? f.FieldType : p.PropertyType;
                                object value = null;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    try
                                    {
                                        value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    }
                                    catch
                                    {
                                    }

                                    if (value != null)
                                    {
                                        level++;
                                        WriteObject(m.Name + ": ", value);
                                        level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
}
