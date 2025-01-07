//-----------------------------------------------------------------------
// <copyright file="WMIQuery.cs" company="Lifeprojects.de">
//     Class: WMIQuery
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>21.09.2021</date>
//
// <summary>
// Die Klasse erstellt eine WMI Object
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.WMI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public class WMIQuery : DisposableCoreBase
    {
        private ManagementObjectSearcher wmiSearcher = null;

        public WMIQuery(string query = "")
        {
            if (string.IsNullOrEmpty(query) == false)
            {
                this.CreateWMISearcher(query);
            }
        }

        public WMIQuery(WMIQueryTyp query)
        {
            if (query != WMIQueryTyp.None)
            {
                this.Query = query.ToString();
                this.CreateWMISearcher(this.Query);
            }
        }

        public string Query { get; private set; }

        public OperationResult<Dictionary<string, WMIContentResult>>  Get()
        {            
            Dictionary<string, WMIContentResult> result = null;

            try
            {
                if (result == null)
                {
                    result = new Dictionary<string, WMIContentResult>();
                }

                IEnumerable<ManagementObject> wmiValues = this.wmiSearcher.Get().OfType<ManagementObject>();
                foreach (ManagementObject queryObj in wmiValues)
                {
                    foreach (PropertyData prop in queryObj.Properties)
                    {
                        if (result.ContainsKey(prop.Name) == false)
                        {
                            if (prop.Value != null)
                            {
                                object value = UTFToObject(prop.Value);
                                WMIContentResult content = new WMIContentResult(prop.Name, prop.Type, value, prop.Origin);
                                result.Add(prop.Name, content);
                            }
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                return OperationResult<Dictionary<string, WMIContentResult>>.Failure(ex, false);
            }
            catch (Exception)
            {
                throw;
            }

            return OperationResult<Dictionary<string, WMIContentResult>>.SuccessResult(result, DateTime.Now);
        }

        public OperationResult<TClass> Get<TClass>() where TClass : new()
        {
            TClass result = new TClass();
            var entity = typeof(TClass);
            Dictionary<string, PropertyInfo> propertyDict = new Dictionary<string, PropertyInfo>();

            try
            {
                PropertyInfo[] classProperties = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propertyDict = classProperties.ToDictionary(p => p.Name.ToUpper(), p => p);

                IEnumerable<ManagementObject> wmiValues = this.wmiSearcher.Get().OfType<ManagementObject>();
                foreach (ManagementObject queryObj in wmiValues)
                {
                    foreach (PropertyData prop in queryObj.Properties)
                    {
                        string propName = prop.Name.ToUpper();
                        if (propertyDict.ContainsKey(propName))
                        {
                            PropertyInfo info = propertyDict[propName];
                            if ((info != null) && info.CanWrite == true)
                            {
                                object value = UTFToObject(prop.Value);
                                info.SetValue(result, prop.Value == null ? default : prop.Value, null);
                            }
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                return OperationResult<TClass>.Failure(ex, false);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return OperationResult<TClass>.SuccessResult(result, DateTime.Now);
        }

        public List<string> GetClassNamesWithinWmiNamespace(string wmiNamespaceName = "root\\CIMV2")
        {
            List<String> classes = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(wmiNamespaceName), new WqlObjectQuery("SELECT * FROM meta_class"));
            List<string> classNames = new List<string>();

            try
            {
                ManagementObjectCollection objectCollection = searcher.Get();

                foreach (ManagementClass wmiClass in objectCollection)
                {
                    string stringified = wmiClass.ToString();
                    string[] parts = stringified.Split(new char[] { ':' });
                    if (wmiNamespaceName.ToLower() == "root\\cimv2")
                    {
                        if (parts[1].Contains("win32_", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            classes.Add(parts[1]);
                        }
                    }
                    else if (wmiNamespaceName.ToLower() == "root\\wmi")
                    {
                        if (parts[1].StartsWith("wmi", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            classes.Add(parts[1]);
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                string errorText = ex.Message;
            }
            catch (Exception)
            {

                throw;
            }

            return classes.OrderBy(s => s).ToList();
        }

        public override void DisposeManagedResources()
        {
            this.wmiSearcher = null;
        }

        private void CreateWMISearcher(string query)
        {
            ManagementPath managementPath = new ManagementPath();

            this.Query = query;
            if (this.Query.StartsWith("wmi", StringComparison.OrdinalIgnoreCase) == true)
            {
                managementPath.Path = "root\\wmi";
            }
            else
            {
                managementPath.Path = "root\\CIMV2";
            }

            ManagementScope managementScope = new ManagementScope(managementPath);
            ObjectQuery objectQuery = new ObjectQuery($"SELECT * FROM {query}");
            this.wmiSearcher = new ManagementObjectSearcher(managementScope, objectQuery);
        }

        private object UTFToObject(object obj)
        {
            switch (obj)
            {
                case null:
                case IReadOnlyList<ushort> decArray when decArray.Count == 0 || decArray[0] == 0:
                    return obj;
                case IReadOnlyList<ushort> decArray:
                    {
                        var sb = new StringBuilder();
                        foreach (ushort val in decArray)
                        {
                            if (val == 0)
                            {
                                break;
                            }

                            /* ASCII codes only (escape) */
                            if (val >= 32 && val <= 127)
                            {
                                sb.Append((char)val);
                            }
                        }

                        return sb.ToString().Trim();
                    }
                default:
                    return obj;
            }
        }
    }
}
