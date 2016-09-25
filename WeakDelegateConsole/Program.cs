using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeakDelegateConsole
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class Source
    {
        public event Action<int> complited;
    }

    public class Listner
    {

    }

    public class WeakDelegate
    {
        private WeakReference Target;
        private MethodInfo Method;

        public WeakDelegate(Delegate d)
        {
            Target = new WeakReference(d.Target);
            Method = d.Method;
            d = null;
        }

        public Delegate Weak
        {
            get
            {
              
            }
        }
            
    }
}
