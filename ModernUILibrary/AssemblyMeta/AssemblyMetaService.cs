//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaService.cs" company="www.pta.de">
//     Class: AssemblyMetaService
//     Copyright © www.pta.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>04.12.2024 15:59:19</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUILibrary.AssemblyMeta
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using ModernIU.Base;

    using ModernUILibrary.Core;

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

        protected override void DisposeManagedResources()
        {
        }

        protected override void DisposeUnmanagedResources()
        {
        }
    }
}
