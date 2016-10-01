using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeakDelegateConsole;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public void ClearGC()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForFullGCComplete(100);
            GC.WaitForPendingFinalizers();
        }

        static bool ListenerWasDestroyed;

        public class TestListener : Listener
        {
            ~TestListener()
            {
                ListenerWasDestroyed = true;
            }
        }

        [TestMethod]
        public void TestHardReferenceListenerDontDisappear()
        {
            ListenerWasDestroyed = false;
            Source source = new Source();
            Listener listener = new TestListener();
            source.Completed += listener.Handler;
            listener = null;
            ClearGC();
            Assert.IsFalse(ListenerWasDestroyed);
        }

        [TestMethod]
        public void TestWeakReferenceListenerDisappear()
        {
            ListenerWasDestroyed = false;
            Source source = new Source();
            Listener listener = new TestListener();
            WeakDelegate weakDelegate = new WeakDelegate((Action<int>)listener.Handler);
            source.Completed += (Action<int>)weakDelegate.Weak;
            listener = null;
            ClearGC();
        }

        [TestMethod]
        public void TestWeakReferenceDisappearing()
        {
            Source source = new Source();
            Listener listener = new Listener();
            WeakDelegate weakDelegate = new WeakDelegate((Action<int>)listener.Handler);
            source.Completed += (Action<int>)weakDelegate.Weak;
            listener = null;
            ClearGC();
            Assert.IsTrue(weakDelegate.TargetIsNull);

        }
    }
}
