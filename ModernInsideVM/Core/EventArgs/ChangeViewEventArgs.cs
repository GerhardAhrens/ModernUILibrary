namespace ModernInsideVM.Core
{
    using System;

    using ModernBaseLibrary.Core;

    public class ChangeViewEventArgs : EventArgs, IEventAggregatorArgs, IFactoryArgs
    {
        public string Sender { get; set; }

        public string Description { get; set; }

        public CommandButtons MenuButton { get; set; }
        
        /// <summary>
        /// Id des Entity Objektes
        /// </summary>
        public Guid EntityId { get; set; }

        public bool IsNew { get; set; } = false;

        public bool IsCopy { get; set; } = false;

        public int RowPosition { get; set; }

        /// <summary>
        /// Wechsel von Dialog
        /// </summary>
        public CommandButtons FromPage { get; set; }

        /// <summary>
        /// Wechsel zu Dialog
        /// </summary>
        public CommandButtons TargetPage { get; set; }
    }
}
