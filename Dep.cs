using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShutdown
{
    class Dep
    {
        public delegate void EventDelegate(object obj);
        public event EventDelegate Event;


        static Dep instance = null;

        private Dep()
        {

        }
        public static Dep GetInstance()
        {
            if (Dep.instance == null)
            {
                return Dep.instance = new Dep();
            }
            return Dep.instance;
        }

        public void Emit(object obj)
        {
            if (this.Event != null)
            {
                Event(obj);
            }
        }
    }
}
