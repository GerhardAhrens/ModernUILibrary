//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaService.cs" company="www.lifeprojects.de">
//     Class: AssemblyMetaService
//     Copyright © www.lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>04.12.2024 15:59:19</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyMetaService : DisposableCoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMetaService"/> class.
        /// </summary>
        public AssemblyMetaService()
        {
        }

        public IEnumerable<IAssemblyInfo> GetMetaInfo()
        {
            List<IAssemblyInfo> assemblyInfos = new List<IAssemblyInfo>();
            Type ti = typeof(IAssemblyInfo);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (ti.IsAssignableFrom(type))
                    {
                        if (type != null && type.IsInterface == false)
                        {
                            IAssemblyInfo assemblyInfoObject = (IAssemblyInfo)Activator.CreateInstance(type);
                            assemblyInfos.Add(assemblyInfoObject);
                        }
                    }
                }
            }

            return assemblyInfos;
        }
    }
}
