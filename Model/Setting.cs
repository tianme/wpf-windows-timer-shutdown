using ComputerShutdown.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShutdown
{
    class Setting
    {

        public WindowPosition WindowPosition
        {
            get;
            set;
        }

        public int TimerType
        {
            get;
            set;
        }
        public double SetTime
        {
            get;
            set;
        }
    }
}
