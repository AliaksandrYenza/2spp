using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            source.LetsDoIt();
            Console.Read();
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
            return r.Next(0, 10);
        }

        public void LetsDoIt()
        {
            if (Completed != null)
                Completed.Invoke(RandomNumber());
            if (Completed1 != null)
                Completed1.Invoke(RandomNumber(), RandomNumber());
            if (Completed2 != null)
                Completed2.Invoke(RandomNumber(), RandomNumber(), RandomNumber());
            if (Completed3 != null)
                Completed3.Invoke(RandomNumber(), RandomNumber(), RandomNumber(), RandomNumber());
        }
    }

    public class Listener
    {
        private double Sum = 0;
        private short Destroyed;

        public void Handler(int param1)
        {
            Sum = param1;
            Console.WriteLine("{0} = {1}", param1, Sum);
        }

        public void Handler(int param1, double param2)
        {
            Sum = param1 + param2;
            Console.WriteLine("{0} + {1} = {2}", param1, param2, Sum);
        }

        public void Handler(int param1, double param2, int param3)
        {
            Sum = param1 + param2 + param3;
            Console.WriteLine("{0} + {1} + {2} = {3}", param1, param2, param3, Sum);
        }

        public void Handler(int param1, int param2, int param3, int param4)
        {
            Sum = param1 + param2 + param3 + param4;
            Console.WriteLine("{0} + {1} + {2} + {3} = {4}", param1, param2, param3, param4, Sum);
        }

        ~Listener()
        {
               
        }

    }

    public class WeakDelegate
    {
        private WeakReference Target;
        private MethodInfo Method;
        private Type TypeOfDelegate;

        public bool TargetIsNull
        {
            get { return !Target.IsAlive; }
        }

        public WeakDelegate(Delegate d)
        {
            Target = new WeakReference(d.Target);
            Method = d.Method;
            TypeOfDelegate = d.GetType();
        }

        private Delegate InitDelegate()
        {
            ParameterExpression[] paramArr = GetParametersExpression(Method);
            Expression targetObjectExpression = GetPropertyExpression(Expression.Constant(Target), "Target", Target.Target.GetType());
            Expression targetMethodInvoke = Expression.Call(targetObjectExpression, Method, paramArr);
            Expression conditionExpression = Expression.NotEqual(targetObjectExpression, Expression.Constant(null));
            Expression ifExpression = Expression.IfThen(conditionExpression, targetMethodInvoke);
            LambdaExpression labmda = Expression.Lambda(ifExpression, paramArr);
            return labmda.Compile();
        }

        private ParameterExpression[] GetParametersExpression(MethodInfo method)
        {
            ParameterInfo[] eventHandlerArgsInfo = method.GetParameters();
            ParameterExpression[] eventHandlerArgsExpressionMassive = new ParameterExpression[eventHandlerArgsInfo.Length];
            for (int i = 0; i < eventHandlerArgsInfo.Length; i++)
            {
                eventHandlerArgsExpressionMassive[i] = ParameterExpression.Parameter(eventHandlerArgsInfo[i].ParameterType);
            }
            return eventHandlerArgsExpressionMassive;
        }

        private Expression GetPropertyExpression(Expression declaryingObjectExpression, String propertyName, Type typeToCastProperty = null)
        {
            Type declaryingClassType = declaryingObjectExpression.Type;
            PropertyInfo targetPropertyInfo = declaryingClassType.GetProperty(propertyName);
            Expression targetObjectExpression = Expression.Property(declaryingObjectExpression, targetPropertyInfo);
            if (typeToCastProperty != null)
            {
                targetObjectExpression = Expression.Convert(targetObjectExpression, typeToCastProperty);
            }
            return targetObjectExpression;
        }




        public Delegate Weak
        {
            get
            {
                return InitDelegate();

            }
        }

    }
}
