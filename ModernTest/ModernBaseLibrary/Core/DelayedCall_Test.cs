namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Windows;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DelayedCall_Test
    {
        private DelayedCallBase dc;

        [TestInitialize]
        public void OnTestInitialize()
        {
            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            }
        }

        [TestMethod]
        public void RunDelayedMethodes()
        {
            ThreadContext.InvokeOnUiThread(delegate () {
                dc = DelayedCall<string>.StartAsync(SetButtonText, "Ich wurde geklickt", 2000);
            });
        }

        private void SetButtonText(string newText)
        {
            Assert.IsTrue(newText == "Ich wurde geklickt");
        }
    }

    public static class ThreadContext
    {
        public static void InvokeOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }

        public static void BeginInvokeOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
        }
    }
}