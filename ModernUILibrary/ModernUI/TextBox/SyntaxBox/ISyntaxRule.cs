﻿namespace ModernIU.Controls
{
    using System.Collections.Generic;

    public interface ISyntaxRule
    {
        int RuleId { get; set; }
        DriverOperation Op { get; set; }
        IEnumerable<FormatInstruction> Match(string Text);
    }
}