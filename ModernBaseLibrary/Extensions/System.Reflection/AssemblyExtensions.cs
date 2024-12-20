/*
 * <copyright file="AssemblyExtensions.cs" company="Lifeprojects.de">
 *     Class: AssemblyExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for Assemply Types
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public static class AssemblyExtensions
    {
        private static readonly Assembly assmbly = null;

        static AssemblyExtensions()
        {
            assmbly = Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Die Methodenerweiterung gibt alle geladenen Assemblies als Liste zurück
        /// </summary>
        /// <param name="pAssembly"></param>
        /// <param name="pIncludeAssembly"></param>
        /// <returns></returns>
        public static IList<AssemblyName> GetAssemblyLoaded(this Assembly pAssembly, List<string> pIncludeAssembly = null)
        {
            IList<AssemblyName> getAssemblyLoaded = new List<AssemblyName>();
            AssemblyName[] assmNames = pAssembly.GetReferencedAssemblies();
            for (int i = 0; i < assmNames.Length; i++)
            {
                AssemblyName item = assmNames[i];
                string resultName = $"{item.Name},{item.Version}";
                getAssemblyLoaded.Add(item);
            }
            return (getAssemblyLoaded);
        }

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetClassInterface<TInterface>(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                var it = typeof(TInterface);
                return assembly.GetTypes().Where(it.IsAssignableFrom);
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static string ExecutingPath(this Assembly @this)
        {
            return Path.GetDirectoryName(assmbly.Location);
        }

        public static T GetCustomAttribute<T>(this Assembly @this, bool inherit = false) where T : Attribute
        {
            T result = null;

            // Try to find the configuration attribute for the default logger if it exists
            object[] configAttributes = Attribute.GetCustomAttributes(@this, typeof(T), inherit);

            // get just the first one
            if (!configAttributes.IsNullOrEmpty())
            {
                result = (T)configAttributes[0];
            }

            return result;
        }

        /// <summary>
        /// Gets the attributes from an assembly.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to find.</typeparam>
        /// <param name="@this">The calling assembly to search.</param>
        /// <returns>An enumeration of attributes of type T that were found.</returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Assembly @this, bool inherit = false) where T : Attribute
        {
            // Try to find the configuration attribute for the default logger if it exists
            object[] configAttributes = Attribute.GetCustomAttributes(@this, typeof(T), inherit);

            if (configAttributes != null)
            {
                foreach (T attribute in configAttributes)
                {
                    yield return attribute;
                }
            }
        }

        public static IList<string[]> ReadCustomAttributes<T>(this Assembly pAssembly)
        {
            IList<string[]> getCustomAttributes = null;
            IEnumerable<MemberInfo> foundAttribut =
                from x in pAssembly.GetTypes()
                    .SelectMany((Type x) => x.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    .Union(pAssembly.GetTypes())
                where Attribute.IsDefined(x, typeof(T))
                select x;

            if (foundAttribut.Count<MemberInfo>() > 0)
            {
                getCustomAttributes = new List<string[]>();
            }
            else
            {
                List<T> attribute = pAssembly.GetCustomAttributes(typeof(T), true).Cast<T>().ToList();
                if (attribute != null)
                {
                    getCustomAttributes = new List<string[]>();
                    foreach (T item in attribute)
                    {
                        string[] internAttr = new string[] { "", "", "" };
                        internAttr[0] = "Class";
                        internAttr[1] = "AssemblyInfo";
                        internAttr[2] = item.ToString();
                        getCustomAttributes.Add(internAttr);
                    }
                }
            }

            foreach (MemberInfo memberInfo in foundAttribut)
            {
                string memInfo = memberInfo.Name;
                string className = (memberInfo.DeclaringType == null) ? "Class" : memberInfo.DeclaringType.Name;
                string memberType = memberInfo.MemberType.ToString();
                string sortKey = (memberInfo.Name == "Class") ? className : string.Format("{0}.{1}", className, memInfo);
                object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), true);
                for (int i = 0; i < customAttributes.Length; i++)
                {
                    Attribute cutomerAttr = (Attribute)customAttributes[i];
                    string[] internAttr = new string[7] { "", "", "", "", "", "", "" };
                    internAttr[0] = sortKey;
                    internAttr[1] = memberType;
                    internAttr[2] = cutomerAttr.ToString();
                    if (cutomerAttr is RequirementAttribute)
                    {
                        RequirementAttribute reqAttribute = ((RequirementAttribute)(cutomerAttr));
                        internAttr[3] = reqAttribute.Id;
                        internAttr[4] = reqAttribute.DependsOnRequirementId;
                        internAttr[5] = reqAttribute.Comment;
                        internAttr[6] = reqAttribute.Status.ToString();
                    }
                    getCustomAttributes.Add(internAttr);
                }
            }

            return getCustomAttributes;
        }

        public static Bitmap LoadBitmapFromResource(this Assembly @this, string imageResourcePath)
        {
            if (string.IsNullOrEmpty(imageResourcePath) == false)
            {
                var stream = @this.GetManifestResourceStream(imageResourcePath);
                if (stream != null)
                {
                    return stream != null ? new Bitmap(stream) : null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static string ReadTextResource(this Assembly @this, string resName)
        {
            string text = string.Empty;
            using (Stream strm = @this.GetManifestResourceStream(resName))
            {
                using (StreamReader sr = new StreamReader(strm, Encoding.Default))
                {
                    text = sr.ReadToEnd();
                }
            }
            return text;
        }

        public static IEnumerable<Assembly> GetAllReferenceAssemblies(this Assembly @this)
        {
            var results = new List<Assembly>();

            results.Add(@this);

            foreach (var name in @this.GetReferencedAssemblies().AsParallel())
            {
                Assembly loaded = Assembly.Load(name);
                results.AddRange(loaded.GetAllReferenceAssemblies());
            }

            return results;
        }

        public static IEnumerable<Assembly> GetAllReferenceAssemblies(this Assembly @this, string filter)
        {
            var results = new List<Assembly>();

            results.Add(@this);

            foreach (var name in @this.GetReferencedAssemblies().Where(p => p.Name.ToLower().Contains(filter.ToLower()) == true).AsParallel())
            {
                Assembly loaded = Assembly.Load(name);
                results.AddRange(loaded.GetAllReferenceAssemblies());
            }

            return results;
        }

        /// <summary>
        ///     Determines whether any custom attributes are applied to an assembly. Parameters specify the assembly, and the
        ///     type of the custom attribute to search for.
        /// </summary>
        /// <param name="@this">An object derived from the  class that describes a reusable collection of modules.</param>
        /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
        /// <returns>true if a custom attribute of type  is applied to ; otherwise, false.</returns>
        public static bool IsDefined(this Assembly @this, Type attributeType)
        {
            return Attribute.IsDefined(@this, attributeType);
        }

        /// <summary>
        ///     Determines whether any custom attributes are applied to an assembly. Parameters specify the assembly, the
        ///     type of the custom attribute to search for, and an ignored search option.
        /// </summary>
        /// <param name="@this">An object derived from the  class that describes a reusable collection of modules.</param>
        /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
        /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
        /// <returns>true if a custom attribute of type  is applied to ; otherwise, false.</returns>
        public static bool IsDefined(this Assembly @this, Type attributeType, Boolean inherit)
        {
            return Attribute.IsDefined(@this, attributeType, inherit);
        }

        public static IEnumerable<Type> GetAllInterfaces([NotNull] this Assembly assembly)
        {
            assembly = assembly.IsArgumentNotNull();

            var interfaces = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                interfaces.AddRange(type.GetInterfaces());
            }

            return interfaces;
        }

        public static IEnumerable<T> GetInstances<T>([NotNull] this Assembly assembly) where T : class
        {
            assembly = assembly.IsArgumentNotNull();

            var types = assembly.GetTypes()
                .Where(x => !x.IsInterface
                && !x.IsAbstract && !x.IsGenericType
                && typeof(T).IsAssignableFrom(x));

            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is T instance)
                {
                    yield return instance;
                }
            }
        }

        public static IEnumerable<Type> GetAllTypes([NotNull] this Assembly assembly)
        {
            assembly = assembly.IsArgumentNotNull();

            return assembly.GetTypes().Where(p => !p.IsAbstract).AsEnumerable();
        }

        public static IEnumerable<Type> GetTypes([NotNull] this Assembly assembly, [NotNull] Type type)
        {
            assembly = assembly.IsArgumentNotNull();
            type = type.IsArgumentNotNull();

            return assembly.GetTypes().Where(p => !p.IsAbstract && type.IsAssignableFrom(p)).AsEnumerable();
        }

        public static bool? IsJitOptimizationDisabled(this Assembly assembly) => GetDebuggableAttribute(assembly).IsJitOptimizerDisabled();

        public static bool? IsDebug(this Assembly assembly) => GetDebuggableAttribute(assembly).IsJitTrackingEnabled();

        public static bool IsTrue(this bool? valueOrNothing) => valueOrNothing.HasValue && valueOrNothing.Value;

        private static DebuggableAttribute GetDebuggableAttribute(Assembly assembly) 
            => assembly?.GetCustomAttributes().OfType<DebuggableAttribute>().SingleOrDefault();

        private static bool? IsJitOptimizerDisabled(this DebuggableAttribute attribute) => attribute?.IsJITOptimizerDisabled;

        private static bool? IsJitTrackingEnabled(this DebuggableAttribute attribute) => attribute?.IsJITTrackingEnabled;

    }
}
