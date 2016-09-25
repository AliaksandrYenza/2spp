using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeakDelegateConsole;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestWeakReferenceDisappearing()
        {
            Source source = new Source();
            Listener listener = new Listener();
            WeakDelegate weakDelegate = new WeakDelegate((Action<int>)listener.Handler);
            source.Completed += (Action<int>)weakDelegate.Weak;
            listener = null;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete(100);
            GC.WaitForPendingFinalizers();

        }
    }
}
