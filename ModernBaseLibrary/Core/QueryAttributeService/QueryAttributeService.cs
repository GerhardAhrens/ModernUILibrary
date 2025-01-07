//-----------------------------------------------------------------------
// <copyright file="IQueryAttributeService.cs" company="Lifeprojects.de">
//     Class: IQueryAttributeService
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.08.2017</date>
//
// <summary>
// Interface zum QueryAttributeService
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core

{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public sealed class QueryAttributeService : DisposableCoreBase, IQueryAttributeService
    {
        private readonly Assembly assemply = null;
        private IDictionary<string, string> getMetaAttributs = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAttributeService1"/> class.
        /// </summary>
        public QueryAttributeService()
        {
            this.assemply = Assembly.GetCallingAssembly();
        }

        public QueryAttributeService(Assembly assembly)
        {
            this.assemply = assembly;
        }

        /// <summary>
        /// Die Methode gibt für den übergebenen Typ eine Liste von Attributen zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Liste mit Attributen</returns>
        public IDictionary<string, object> GetExternAttributes<TAttr>(Type typ) where TAttr : Attribute
        {
            IDictionary<string, object> attrList = new ExpandoObject() as IDictionary<string, object>;

            try
            {
                IEnumerable<TAttr> getAttributs = typ.GetMembers().SelectMany(p => p.GetCustomAttributes(true)).OfType<TAttr>().AsParallel();
                foreach (TAttr item in getAttributs)
                {
                    string memberFullName = Guid.NewGuid().ToString();
                    PropertyInfo[] attrMembers = item.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in attrMembers)
                    {
                        if (propertyInfo.Name == "MemberName")
                        {
                            PropertyInfo pi = attrMembers.FirstOrDefault(f => f.Name == "MemberName");
                            if (pi != null && pi.GetValue(item) != null)
                            {
                                string memberName = pi.GetValue(item).ToString();
                                MemberInfo[] memberInfo = typ.GetMember(memberName);

                                PropertyInfo propMemberFullName = attrMembers.FirstOrDefault(f => f.Name == "MemberFullName");
                                if (propMemberFullName != null)
                                {
                                    MemberInfo[] memberInfo1 = typ.GetMember(memberName);
                                    MemberTypes memberType = memberInfo1.FirstOrDefault().MemberType;
                                    if (memberType == MemberTypes.Method)
                                    {
                                        MethodInfo objMethode = typ.GetMethod(memberName);
                                        memberFullName = $"{objMethode.ReflectedType.FullName}.{objMethode.Name}";
                                        string returnTyp = objMethode.ReturnType.Name;
                                        propMemberFullName.SetValue(item, memberFullName);
                                    }
                                    else if (memberType == MemberTypes.Property)
                                    {
                                        PropertyInfo objProperty = typ.GetProperty(memberName);
                                        memberFullName = $"{objProperty.ReflectedType.FullName}.{objProperty.Name}";
                                        propMemberFullName.SetValue(item, memberFullName);
                                    }
                                }

                                PropertyInfo propDataType = attrMembers.FirstOrDefault(f => f.Name == "DataType");
                                if (propDataType != null)
                                {
                                    MemberInfo[] memberInfo1 = typ.GetMember(memberName);
                                    MemberTypes memberType = memberInfo1.FirstOrDefault().MemberType;
                                    if (memberType == MemberTypes.Method)
                                    {
                                        MethodInfo objMethode = typ.GetMethod(memberName);
                                        string returnTyp = objMethode.ReturnType.Name;
                                        propDataType.SetValue(item, returnTyp);
                                    }
                                    else if (memberType == MemberTypes.Property)
                                    {
                                        PropertyInfo objProperty = typ.GetProperty(memberName);
                                        string returnTyp = objProperty.PropertyType.Name;
                                        propDataType.SetValue(item, returnTyp);
                                    }
                                }

                                PropertyInfo propMemberType = attrMembers.FirstOrDefault(f => f.Name == "MemberType");
                                if (propMemberType != null)
                                {
                                    MemberInfo[] memberInfo1 = typ.GetMember(memberName);
                                    MemberTypes memberType = memberInfo1.FirstOrDefault().MemberType;
                                    propMemberType.SetValue(item, memberType.ToString());
                                }
                            }
                        }
                    }

                    attrList.Add(memberFullName, item);
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return attrList;
        }

        public IDictionary<string, string> GetMetadataAttributes()
        {
            if (this.getMetaAttributs == null)
            {
                this.getMetaAttributs = new Dictionary<string, string>();
            }

            try
            {
                IList<string[]> attr = (IList<string[]>)this.assemply.GetCustomAttributes<AssemblyMetaAttribute>();
                foreach (string[] item in attr)
                {
                    string[] metaRow = item[2].Split('~');
                    if (this.getMetaAttributs.ContainsKey(metaRow[0]) == false)
                    {
                        this.getMetaAttributs.Add(metaRow[0], metaRow[1]);
                    }
                }

            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return this.getMetaAttributs;
        }

        public List<TAttribute> ReadClassAttribute<TAttribute>(Type attributeType)
        {
            List<TAttribute> attributeList = null;

            IEnumerable<TypeInfo> assemblyTypes = this.assemply.DefinedTypes;
            if (assemblyTypes?.Count() > 0)
            {
                attributeList = new List<TAttribute>();
                foreach (TypeInfo typeInfo in assemblyTypes)
                {
                    if (typeInfo.IsDefined(typeof(TAttribute), false))
                    {
                        object[] attributes = typeInfo.GetCustomAttributes(typeof(TAttribute), false);
                        if (attributes.Length > 0)
                        {
                            foreach (Attribute attribute in attributes)
                            {
                                TAttribute attributContent = (TAttribute)Convert.ChangeType(attribute, typeof(TAttribute));
                                dynamic attributValue = attributContent;

                                if (PropertyExist(attributValue, "FullName") == true)
                                {
                                    attributValue.FullName = typeInfo.FullName;
                                }

                                if (PropertyExist(attributValue, "ClassName") == true)
                                {
                                    attributValue.ClassName = typeInfo.FullName.Split(".").LastOrDefault();
                                }

                                attributeList.Add(attributContent);
                            }
                        }
                    }
                }
            }

            return attributeList;
        }

        public List<TAttribute> ReadMethodAttribute<TAttribute>(Type attributeType)
        {
            List<TAttribute> attributeList = null;

            Type[] assemblyTypes = this.assemply.GetTypes();
            if (assemblyTypes?.Count() > 0)
            {
                IEnumerable<MethodInfo> methodeInfos = assemblyTypes.SelectMany(t => t.GetMethods());
                foreach (MethodInfo methodeInfo in methodeInfos)
                {
                    if (methodeInfo.IsDefined(typeof(TAttribute), false))
                    {
                        object[] attributes = methodeInfo.GetCustomAttributes(typeof(TAttribute), false);
                        if (attributes.Length > 0)
                        {
                            attributeList = new List<TAttribute>();

                            foreach (Attribute attribute in attributes)
                            {
                                TAttribute attributContent = (TAttribute)Convert.ChangeType(attribute, typeof(TAttribute));
                                dynamic attributValue = attributContent;

                                if (PropertyExist(attributValue, "FullName") == true)
                                {
                                    attributValue.FullName = methodeInfo.GetBaseDefinition().DeclaringType.FullName;
                                }

                                if (PropertyExist(attributValue, "ClassName") == true)
                                {
                                    string minfo = methodeInfo.GetBaseDefinition().DeclaringType.FullName;
                                    attributValue.ClassName = minfo.Split(".").LastOrDefault();
                                }

                                if (PropertyExist(attributValue, "MethodeName") == true)
                                {
                                    attributValue.MethodeName = methodeInfo.Name;
                                }

                                attributeList.Add(attributContent);
                            }
                        }
                    }
                }
            }

            return attributeList;
        }

        /// <summary>
        /// Die Methode gibt eine Liste von gefundenen 'ChangeLogAttribut' als Liste zurück.
        /// </summary>
        public string GetDefaultAttribute<T>()
        {
            string getDefaultAttribute = string.Empty;
            try
            {
                object[] attribut = this.assemply.GetCustomAttributes(typeof(T), false);
                if (typeof(T).Name == "AssemblyVersionAttribute")
                {
                    getDefaultAttribute = this.assemply.GetName().Version.ToString();
                }
                else
                {
                    foreach (T item in attribut)
                    {
                        if (item is AssemblyTitleAttribute)
                        {
                            AssemblyTitleAttribute attr = item as AssemblyTitleAttribute;
                            getDefaultAttribute = attr.Title;
                        }
                        else if (item is AssemblyDescriptionAttribute)
                        {
                            AssemblyDescriptionAttribute attr = item as AssemblyDescriptionAttribute;
                            getDefaultAttribute = attr.Description;
                        }
                        else if (item is AssemblyConfigurationAttribute)
                        {
                            AssemblyConfigurationAttribute attr = item as AssemblyConfigurationAttribute;
                            getDefaultAttribute = attr.Configuration;
                        }
                        else if (item is AssemblyCompanyAttribute)
                        {
                            AssemblyCompanyAttribute attr = item as AssemblyCompanyAttribute;
                            getDefaultAttribute = attr.Company;
                        }
                        else if (item is AssemblyProductAttribute)
                        {
                            AssemblyProductAttribute attr = item as AssemblyProductAttribute;
                            getDefaultAttribute = attr.Product;
                        }
                        else if (item is AssemblyCopyrightAttribute)
                        {
                            AssemblyCopyrightAttribute attr = item as AssemblyCopyrightAttribute;
                            getDefaultAttribute = attr.Copyright;
                        }
                        else if (item is AssemblyTrademarkAttribute)
                        {
                            AssemblyTrademarkAttribute attr = item as AssemblyTrademarkAttribute;
                            getDefaultAttribute = attr.Trademark;
                        }
                        else if (item is AssemblyCultureAttribute)
                        {
                            AssemblyCultureAttribute attr = item as AssemblyCultureAttribute;
                            getDefaultAttribute = attr.Culture;
                        }
                        else if (item is AssemblyVersionAttribute)
                        {
                            AssemblyVersionAttribute attr = item as AssemblyVersionAttribute;
                            getDefaultAttribute = attr.Version;
                        }
                        else if (item is AssemblyFileVersionAttribute)
                        {
                            AssemblyFileVersionAttribute attr = item as AssemblyFileVersionAttribute;
                            getDefaultAttribute = attr.Version;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return getDefaultAttribute;
        }

        private bool PropertyExist(dynamic dynamicObject, string name)
        {
            if (dynamicObject is ExpandoObject)
            {
                return ((IDictionary<string, object>)dynamicObject).ContainsKey(name);
            }

            return dynamicObject.GetType().GetProperty(name) != null;
        }

    }
}
