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
            Source source = new Source();
            Listener listener = new Listener();
            source.Completed += (Action<int>)new WeakDelegate((Action<int>)listener.Handler).Weak;
            source.Completed1 += (Action<int, double>)new WeakDelegate((Action<int, double>)listener.Handler).Weak;
            source.Completed2 += (Action<int, double, int>)new WeakDelegate((Action<int, double, int>)listener.Handler).Weak;
            source.Completed3 += (Action<int, int, int, int>)new WeakDelegate((Action<int, int, int, int>)listener.Handler).Weak;
        }
    }

    public class Source
    {
        public event Action<int> Completed;
        public event Action<int, double> Completed1;
        public event Action<int, double, int> Completed2;
        public event Action<int, int, int, int> Completed3;

        Random r = new Random();
        public int RandomNumber()
        {
            return r.Next(0, 100);
        }

        public void LetsDoIt()
        {
            Completed.Invoke(RandomNumber());
            Completed1.Invoke(RandomNumber(), RandomNumber());
            Completed2.Invoke(RandomNumber(), RandomNumber(), RandomNumber());
            Completed3.Invoke(RandomNumber(), RandomNumber(), RandomNumber(), RandomNumber());
        }
    }

    public class Listener
    {
        private double Sum;
        public void Handler(int param1)
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
