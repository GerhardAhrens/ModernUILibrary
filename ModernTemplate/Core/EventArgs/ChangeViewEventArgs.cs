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
        /// Steuerung, ob im nachfolgenden Dialog ein neuer Ertrag erstellt werden soll
        /// </summary>
        public bool IsNew { get; set; } = false;

        /// <summary>
        /// Steuerung, ob beim Wechsel auf eine Übersicht ein Refresh der angezeigten Daten erfolgen soll
        /// </summary>
        public bool IsRefresh { get; set; } = false;

        /// <summary>
        /// Steuerung, ob eine Kopie des aktuellen Datensatz erfolgen soll
        /// </summary>
        public bool IsCopy { get; set; } = false;

        /// <summary>
        /// Steuerung, Ursprungsposition des aktuellen datensatz
        /// </summary>
        public int RowPosition { get; set; }

        /// <summary>
        /// Ursprungsdialog
        /// </summary>
        public CommandButtons FromPage { get; set; }
    }
}
