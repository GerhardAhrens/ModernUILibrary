namespace ModernBaseLibrary.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Loader;

    public class GlobalTypeMap : LoadContextTypeMap
    {
        public GlobalTypeMap() : base(null)
        {
            AddTypeCodes();
            AddBuiltinTypes();
            foreach (var lc in AssemblyLoadContext.All)
            {
                try
                {
                    AddLoadContext(lc);
                }
                catch(Exception ex)
                {
                    Trace.WriteLine($"Exception adding load context {lc.Name}: {ex.Message}");
                }
            }
        }

    }
}
