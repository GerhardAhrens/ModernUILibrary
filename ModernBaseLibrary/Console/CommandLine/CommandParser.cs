/*
 * <copyright file="CommandParser.cs" company="Lifeprojects.de">
 *     Class: CommandParser
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>18.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Zerlegen einer Commandline in Verbindung mit CommandLine-Model Klasse die verschiedene
 * Attribute (Flag, Help) beinhaltet
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

namespace ModernBaseLibrary.CommandLine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public sealed class CommandParser
    {
        private const string HELP_KEY_REGEX = @"^(--help)|(-h)|(-\?)|(\\\?)$";
        private const string KEY_REGEX = @"^--?(\w|\?)+$";

        public CommandParser()
        {
        }

        public CommandParser(string[] _args = null)
        {
            this._args = _args;
        }

        private string[] _args { get; set; }

        public Dictionary<string, List<string>> GetDictionary(string[] args = null, Type targetType = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var ret = new Dictionary<string, List<string>>();
            string key = string.Empty;
            foreach (string arg in args)
            {
                if (CmdLineKeyDetection.GetShortKeyDetector().IsKey(arg)) //short-option
                {
                    if (CmdLineKeyDetection.GetShortKeyDetector().IsJoinedToValue(arg))
                    {
                        string[] split = arg.Split('=');
                        foreach (char potentialKey in split.First().AsEnumerable().Skip(1))
                        {
                            key = "-" + potentialKey;
                            if (!ret.ContainsKey(key) || ret[key].Count == 0)
                            {
                                ret[key] = new List<string>();
                            }
                        }

                        //to hold the value of the key before the equals sign incase of aggregation like -asu=mykeels
                        ret[key].Add(split.Last());
                    }
                    else if (CmdLineKeyDetection.GetShortKeyDetector().IsAggregated(arg, this.GetShortKeys(targetType))) //works on aggregate short keys like -sa
                    {
                        foreach (string potentialKey in CmdLineKeyDetection.GetShortKeyDetector().GetAggregatedKeys(arg, this.GetShortKeys(targetType)))
                        {
                            ret[potentialKey] = new List<string>();
                            key = potentialKey;
                        }
                    }
                    else if (CmdLineKeyDetection.GetShortKeyDetector().IsFollowedByValue(arg))
                    {
                        var result = CmdLineKeyDetection.GetShortKeyDetector().IsFollowedByValue(arg, this.GetShortKeys(targetType));
                        string[] keys = result.Item2.Key;
                        string value = result.Item2.Value;
                        foreach (string potentialKey in keys)
                        {
                            key = potentialKey;
                            if (!ret.ContainsKey(key) || ret[key].Count == 0)
                            {
                                ret[key] = new List<string>();
                            }
                        }

                        ret[key].Add(value);
                    }
                    else
                    {
                        if (!ret.ContainsKey(key) || ret[key].Count == 0)
                        {
                            ret[arg] = new List<string>(); //works for normal short keys like -u
                        }

                        key = arg;
                    }
                }
                else if (CmdLineKeyDetection.GetLongKeyDetector().IsKey(arg)) //long option
                {
                    if (CmdLineKeyDetection.GetLongKeyDetector().IsJoinedToValue(arg))
                    {
                        string[] split = arg.Split('=');
                        key = split.First();
                        if (ret.ContainsKey(key))
                        {
                            ret[key].Add(split.Last());
                        }
                        else
                        {
                            ret[key] = new List<string>() { split.Last() };
                        }
                    }
                    else
                    {
                        if (!ret.ContainsKey(arg)) ret[arg] = new List<string>();
                        key = arg;
                    }
                }
                else if (arg == "--") //terminator
                {
                    if (!ret.ContainsKey(arg))
                    {
                        ret[arg] = new List<string>();
                    }

                    key = arg;
                }
                else //non-option
                {
                    if (key == "" && !ret.ContainsKey(key))
                    {
                        ret.Add(key, new List<string>()); //options
                    }
                    else
                    {
                        if (ret.ContainsKey(key))
                        {
                            ret[key].Add(arg);
                        }
                        else
                        {
                            ret.Add(key, new List<string>() { arg });
                        }
                    }
                }
            }

            return ret;
        }

        public TData Parse<TData>(string[] args = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var _dict = GetDictionary(args, typeof(TData));
            TData ret = Activator.CreateInstance<TData>();
            PropertyInfo[] properties = ret.GetType().GetProperties();
            foreach (var property in properties)
            {
                var flag = property.GetCustomAttribute<FlagAttribute>();
                var help = property.GetCustomAttribute<HelpAttribute>();
                var transform = property.GetCustomAttribute<TransformAttribute>();
                flag = flag ?? new FlagAttribute(property.Name);
                string keyPattern = $@"(^-{flag.ShortName}$)|(^--{flag.Name}$)|^--$";
                string key = _dict.Keys.FirstOrDefault(_key => Regex.IsMatch(_key, keyPattern, RegexOptions.IgnoreCase));
                if (key == null && flag.Required)
                {
                    throw new Exception($"Could not find argument key for --{property.Name} which is required");
                }
                else if (key != null)
                {
                    if (key == "--")
                    {
                        if (typeof(TData).GetInterfaces().Contains(typeof(ICmdLineModel))) ((ICmdLineModel)ret).Extras = _dict[key].ToArray();
                    }
                    else this.SetPropertyValue<TData>(ret, property, _dict[key], transform);
                }
                if (_dict.ContainsKey(""))
                {
                    if (typeof(TData).GetInterfaces().Contains(typeof(ICmdLineModel))) ((ICmdLineModel)ret).Options = _dict[""].ToArray();
                }
            }
            return ret;
        }

        private char[] GetShortKeys(Type targetType)
        {
            List<string> ret = new List<string>();
            if (targetType != null)
            {
                PropertyInfo[] properties = targetType.GetProperties();
                foreach (var property in properties)
                {
                    var flag = property.GetCustomAttribute<FlagAttribute>();
                    if (flag != null)
                    {
                        ret.Add(flag.ShortName);
                    }
                }

                return ret.Where((key) => !string.IsNullOrEmpty(key)).Select((key) => key.ElementAt(0)).ToArray();
            }
            else
            {
                return new char[] { };
            }
        }

        private void SetPropertyValue<TData>(TData ret, PropertyInfo property, List<string> values, TransformAttribute transform = null)
        {
            string flagValue = string.Join(null, values);
            switch (property.PropertyType.Name)
            {
                case "String":
                    try
                    {
                        property.SetValue(ret, flagValue);
                    }
                    catch
                    {
                        throw new Exception($"Could not convert argument value of \"{flagValue}\" to String");
                    }

                    break;
                case "Int32":
                    try
                    { 
                        property.SetValue(ret, Convert.ToInt32(flagValue)); 
                    } 
                    catch
                    { 
                        throw new Exception($"Could not convert argument value of \"{flagValue}\" to Int32"); 
                    }

                    break;
                case "Int64":
                    try 
                    { 
                        property.SetValue(ret, Convert.ToInt64(flagValue));
                    } 
                    catch
                    { 
                        throw new Exception($"Could not convert argument value of \"{flagValue}\" to Int64"); 
                    }

                    break;
                case "DateTime":
                    try 
                    { 
                        property.SetValue(ret, Convert.ToDateTime(flagValue));
                    } 
                    catch
                    { 
                        throw new Exception($"Could not convert argument value of \"{flagValue}\" to DateTime"); 
                    }

                    break;
                case "Double":
                    try
                    { 
                        property.SetValue(ret, Convert.ToDouble(flagValue));
                    }
                    catch 
                    {
                        throw new Exception($"Could not convert argument value of \"{flagValue}\" to Double"); 
                    }

                    break;
                case "Boolean":
                    if (string.IsNullOrWhiteSpace(flagValue))
                    {
                        property.SetValue(ret, true); //default to true if no value is specified cos it's a boolean
                    }
                    else
                    {
                        try
                        {
                            property.SetValue(ret, Convert.ToBoolean(flagValue));
                        }
                        catch
                        {
                            throw new Exception($"Could not convert argument value of \"{flagValue}\" to Boolean");
                        }
                    }

                    break;
                case "List`1":
                    Type listType = property.PropertyType;
                    if (property.GetValue(ret) == null)
                    {
                        property.SetValue(ret, System.Activator.CreateInstance(listType)); //instantiate the List if not instantiated
                    }

                    if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type targetType = listType.GetGenericArguments().First();
                        values.ForEach((string value) =>
                        {
                            this.SetListValue((IList)property.GetValue(ret), targetType, value);
                        });
                    }

                    property.GetValue(ret).GetType().GetProperties();
                    break;
                default:
                    if (transform != null)
                    {
                        property.SetValue(ret, transform.Execute.DynamicInvoke(flagValue));
                    }
                    else
                    {
                        if (property.PropertyType.BaseType.Name == "Enum")
                        {
                            try { property.SetValue(ret, Enum.Parse(property.PropertyType, flagValue)); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to Enum"); }
                        }
                        else throw new Exception($"Type of [{property.Name}] is not recognized");
                    }
                    break;
            }
        }

        private void SetListValue(IList list, Type listTargetType, string value)
        {
            if (list != null)
            {
                switch (listTargetType.Name)
                {
                    case "String":
                        try { list.Add(Convert.ToString(value)); } catch { throw new Exception($"Could not convert argument value of \"{value}\" to String"); }
                        break;
                    case "Int32":
                        try { list.Add(Convert.ToInt32(value)); } catch { throw new Exception($"Could not convert argument value of \"{value}\" to Int32"); }
                        break;
                    case "Int64":
                        try { list.Add(Convert.ToInt64(value)); } catch { throw new Exception($"Could not convert argument value of \"{value}\" to Int64"); }
                        break;
                    case "DateTime":
                        try { list.Add(Convert.ToDateTime(value)); } catch { throw new Exception($"Could not convert argument value of \"{value}\" to DateTime"); }
                        break;
                    case "Boolean":
                        if (string.IsNullOrWhiteSpace(value)) list.Add(true); //default to true if no value is specified cos it's a boolean
                        else try { list.Add(Convert.ToBoolean(value)); } catch { throw new Exception($"Could not convert argument value of \"{value}\" to Boolean"); }
                        break;
                    default:
                        if (listTargetType.BaseType.Name == "Enum")
                        {
                            try { list.Add(Enum.Parse(listTargetType, value)); } catch { throw new Exception($"Could not convert argument value of \"{value}\" to Enum"); }
                        }
                        else throw new Exception($"Type of [{listTargetType.Name}] is not recognized");
                        break;
                }
            }
        }

        private bool MatchesCliKey(PropertyInfo property, string key)
        {
            var flag = property.GetCustomAttribute<FlagAttribute>();
            flag = flag ?? new FlagAttribute(property.Name);
            string keyPattern = $@"(^-{flag.ShortName}$)|(^--{flag.Name}$)";
            return Regex.IsMatch(key, keyPattern);
        }

        private string _getPropKeyHelpInfo(PropertyInfo property, bool includeDescription = false)
        {
            var flag = property.GetCustomAttribute<FlagAttribute>();
            var help = property.GetCustomAttribute<HelpAttribute>();
            flag = flag ?? new FlagAttribute(property.Name);
            string shortName = flag.ShortName != null ? $"-{flag.ShortName} " : null;
            string name = flag.Name != null ? $"--{flag.Name}" : null;
            string helpText = includeDescription ? (help != null ? $" ({help.Usage})" : string.Empty) : string.Empty;
            return (flag.Required ? $"{shortName}{name}{helpText}" : $"[{shortName}{name}{helpText}]");
        }

        public string GetHelpInfo<TData>(string[] args = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var _dict = GetDictionary(args);
            string key = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (Regex.IsMatch(arg, KEY_REGEX) != Regex.IsMatch(arg, HELP_KEY_REGEX))
                {
                    key = arg;
                }
                else if (Regex.IsMatch(arg, HELP_KEY_REGEX))
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        //first help text in argument
                        var help = typeof(TData).GetCustomAttribute<HelpAttribute>();
                        return $"========== Help Information ==========\n {help?.Usage}\n\n" +
                                                $"{System.AppDomain.CurrentDomain.FriendlyName} " +
                                                String.Join(" ", System.Activator.CreateInstance<TData>().GetType().GetProperties().Select(prop => _getPropKeyHelpInfo(prop)).ToArray()) +
                                                "\n========== End Help Information ==========";
                    }
                    else
                    {
                        var property = System.Activator.CreateInstance<TData>().GetType().GetProperties().FirstOrDefault(prop => this.MatchesCliKey(prop, args[i - 1]));
                        if (property != null)
                        {
                            var help = property.GetCustomAttribute<HelpAttribute>();
                            if (help != null)
                            {
                                return "Usage: " + _getPropKeyHelpInfo(property, true);
                            }
                            else return $"No Help Attribute found on Property [{property.Name}]";
                        }
                        else return $"Invalid Help Request";
                    }
                }
            }

            return null;
        }
    }
}
