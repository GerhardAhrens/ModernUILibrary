//-----------------------------------------------------------------------
// <copyright file="ChangeViewEventArgs.cs" company="Lifeprojects.de">
//     Class: ChangeViewEventArgs
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>14.04.2025</date>
//
// <summary>
// Die Klasse 'ChangeViewEventArgs' wird zum weitergeben von Informationen zwischen den Dialogen Verwendet,
// Über das Interface 'IFactoryArgs' werden die notwendigen Properties festgelegt.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;

    using ModernBaseLibrary.Core;
    using ModernUI.MVVM.Base;

    public partial class ChangeViewEventArgs : EventArgs, IEventAggregatorArgs, IChangeViewEventArgs
    {
        /// <summary>
        /// UserControl von dem der Wechsel aufgerufen wird
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Menüpunkt als Enum
        /// </summary>
        public CommandButtons MenuButton { get; set; }

        /// <summary>
        /// Id des Entity Objektes
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Id eines abhängigen Objectes
        /// </summary>
        public Guid ParentId { get; set; }

        public RowNextAction RowNextAction { get; set; }

        /// <summary>
        /// Steuerung, Ursprungsposition des aktuellen datensatz
        /// </summary>
        public int RowPosition { get; set; }

        public bool IsRefresh { get; set; }

        /// <summary>
        /// Ursprungsdialog
        /// </summary>
        public CommandButtons FromPage { get; set; }
    }
}
