//-----------------------------------------------------------------------
// <copyright file="WMIContentResult.cs" company="Lifeprojects.de">
//     Class: WMIContentResult
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>22.09.2021</date>
//
// <summary>
// Class for xyz Result Values from TupleOfT()
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.WMI
{
    using System;
    using System.Diagnostics;
    using System.Management;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [DebuggerDisplay("Name={Name}; Typ={Typ};Value={Value}")]
    public class WMIContentResult : Tuple<string, CimType, object, string>
    {
        public WMIContentResult(string name, CimType typ, object value, string wmiClass) : base(name,typ,value, wmiClass)
        {
        }

        public string Name
        {
            get
            {
                return this.Item1;
            }
        }

        public CimType Typ
        {
            get
            {
                return this.Item2;
            }
        }

        public object Value
        {
            get
            {
                return this.Item3;
            }
        }

        public string WMIClass
        {
            get
            {
                return this.Item4;
            }
        }

        public override string ToString()
        {
            return $"{this.Name};{this.Typ};{this.Value}";
        }
    }
}
