//-----------------------------------------------------------------------
// <copyright file="DialogPermissionAttribute.cs" company="PTA">
//     Class: DialogPermissionAttribute
//     Copyright © PTA GmbH 2020
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@contractors.roche.com</email>
// <date>04.06.2020</date>
//
// <summary>Class for DialogPermissionAttribute</summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;

    /// <summary>
    /// Die Klasse stellt ein Requirement Attribut füpr verschiedene Memeber zur Verfügung
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DialogPermissionAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the requirement attribute
        /// </summary>
        /// <param name="id">The requirement id</param>
        public DialogPermissionAttribute(CommandButtons id, string dialogName)
        {
            this.Id = id;
            this.Dialogname = dialogName;
            this.DialogPermissionUser = new DialogPermissionRole[] { DialogPermissionRole.User };
        }

        public DialogPermissionAttribute(string dialogName, DialogPermissionRole[] dialogPermissionUser)
        {
            this.Id = CommandButtons.None;
            this.Dialogname = dialogName;
            this.DialogPermissionUser =  dialogPermissionUser ;
        }

        public DialogPermissionAttribute(CommandButtons id, string dialogName, params DialogPermissionRole[] dialogPermissionUser)
        {
            this.Id = id;
            this.Dialogname = dialogName;
            this.DialogPermissionUser = dialogPermissionUser;
        }

        /// <summary>
        /// Gets the requirement id
        /// </summary>
        public CommandButtons Id
        {
            private set;
            get;
        }

        /// <summary>
        /// Gets or sets the Dialogname
        /// </summary>
        public string Dialogname
        {
            private set;
            get;
        }

        /// <summary>
        /// Gets or sets the dialog typ.
        /// </summary>
        /// <value>
        /// The dialog typ.
        /// </value>
        public DialogPermissionRole[] DialogPermissionUser
        {
            set;
            get;
        }

    }
}
