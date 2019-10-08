using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShutdown
{
    enum TimerTypeEmum
    {
        Hour,
        Minute,
        Second
    }
    public enum ModifierKeys
    {
        None = 0,        //没有按下任何修饰符
        Alt = 1,             //Alt键
        Control = 2,     //Ctrl键
        Shift = 4,         //Shift键
        Windows = 8,   //Windows徽标键
        Apple = 8,       //按下Apple键徽标键

    }
}
