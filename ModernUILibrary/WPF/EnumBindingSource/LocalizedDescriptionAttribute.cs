//-----------------------------------------------------------------------
// <copyright file="LocalizedDescriptionAttribute.cs" company="Lifeprojects.de">
//     Class: LocalizedDescriptionAttribute
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>07.11.2023 13:28:37</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System;
    using System.ComponentModel;
    using System.Resources;

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private ResourceManager _resourceManager;
        private string _resourceKey;

        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            this._resourceManager = new ResourceManager(resourceType);
            this._resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string description = _resourceManager.GetString(_resourceKey);
                return string.IsNullOrWhiteSpace(description) ? string.Format("[[{0}]]", _resourceKey) : description;
            }
        }
    }
}
