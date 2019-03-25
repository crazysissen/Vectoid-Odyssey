using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    sealed class ParameterAction<T>
    {
        public event Action<T> OnCall;

        private T myValue;

        public ParameterAction(T aValue)
        {
            SetValue(aValue);
        }

        public ParameterAction(T aValue, Action<T> aDelegate)
        {
            SetValue(aValue);

            OnCall += aDelegate;
        }

        public void Activate()
        {
            if (OnCall != null)
            {
                OnCall.Invoke(myValue);

                return;
            }

            Console.WriteLine("ParameterAction activated without assigned delegates");
        }

        public void SetValue(T aValue)
        {
            myValue = aValue;
        }
    }
}
