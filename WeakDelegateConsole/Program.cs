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
        public event Action<int> Complited;
        public event Action<int, double> Complited1;
        public event Action<int, double, int> Complited2;
        public event Action<int, int, int, int> Complited3;

    }

    public class Listner
    {
        private double Sum;
        public void Handler (int param1)
        {
            Sum = param1;
            Console.WriteLine("{0} = ", Sum.ToString());
        }

        public void Handler(int param1, double param2)
        {
            Sum = param1 + param2;
            Console.WriteLine("{0} + {1} = ", Sum.ToString());
        }

        public void Handler(int param1, double param2, int param3)
        {
            Sum = param1 + param2 + param3;
            Console.WriteLine("(0} + {1} + {2} = ", Sum.ToString());
        }

        public void Handler(int param1, int param2, int param3, int param4)
        {
            Sum = param1 + param2 + param3 + param4;
            Console.WriteLine("{0} + {1} + {2} + {3} = ", Sum.ToString());
        }

    }

    public class WeakDelegate
    {
        private WeakReference Target;
        private MethodInfo Method;
        private Type TypeOfDelegate;

        public WeakDelegate(Delegate d)
        {
            Target = new WeakReference(d.Target);
            Method = d.Method;
            TypeOfDelegate = d.GetType();
            d = null;

        }

        public Delegate Weak
        {
            get
            {
                if (Target != null && Target.IsAlive)
                {
                    var newDelegate = Delegate.CreateDelegate(TypeOfDelegate, Target.Target, Method);
                    return newDelegate;
                }
                else
                    return null;
            }
        }
            
    }
}
