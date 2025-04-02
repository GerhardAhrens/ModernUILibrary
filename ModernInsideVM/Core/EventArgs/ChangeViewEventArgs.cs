namespace ModernInsideVM.Core
{
    using System;

    using ModernBaseLibrary.Core;

    using ModernInsideVM.DialogNavigation;

    public class ChangeViewEventArgs : EventArgs, IEventAggregatorArgs, IFactoryArgs
    {
        public string Sender { get; set; }
        public string Description { get; set; }
        public FunctionButtons MenuButton { get; set; }
        public string LoginHash { get; set; }

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
        public FunctionButtons FromPage { get; set; }

        /// <summary>
        /// Wechsel zu Dialog
        /// </summary>
        public FunctionButtons TargetPage { get; set; }
    }
}
