using System;
using System.Windows.Input;

namespace ComputerShutdown
{
    static class Utils
    {
        public static int TransfromSecond(Double number, TimerTypeEmum timerType)
        {


            int second = 0;
            switch (timerType)
            {
                case TimerTypeEmum.Hour:
                    second = (int)(number * 60 * 60);
                    break;
                case TimerTypeEmum.Minute:
                    second = (int)(number * 60);
                    break;
                case TimerTypeEmum.Second:
                    second = (int)number;
                    break;
            }

            return second;

        }

        public static bool isInputNumber(KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
               e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.OemPeriod)
            {
                //按下了Alt、ctrl、shift等修饰键

                e.Handled = true;

            }
            else//按下了字符等其它功能键
            {
                e.Handled = true;
            }
            return false;

        }
    }
}
