using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ComputerShutdown
{
    class ShutdownTimer
    {
        public Timer Timer { get; set; }

        public Dep Dep
        {
            get;
            set;
        }
        public DateTime endTime
        {
            get;
            set;
        }
        public ShutdownTimer() {
            this.Timer = new Timer();
            this.Dep = Dep.GetInstance();
        }
        public bool StartTimer(int secend)
        {
            
            this.Timer.Interval = 1000;
            this.Timer.Enabled = true;
            this.Timer.Elapsed += (object sender, ElapsedEventArgs e) => this.Timer_Elapsed(sender, e, secend);
            this.endTime = DateTime.Now.AddSeconds(secend);
            this.Timer.Start();
            return true;
        }

        public bool StopTimer()
        {
            this.Timer.Stop();
            return true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e, int secend)
        {
            long cTime = DateTime.Now.Ticks;
            long eTime = this.endTime.Ticks;
            if (cTime >= eTime)
            {
                this.Timer.Stop();

                // 执行关机
                var isSuccess =  WindowAPI.DoExitWin(WindowAPI.EWX_FORCE | WindowAPI.EWX_SHUTDOWN);

                if (!Convert.ToBoolean(isSuccess))
                {
                   WindowAPI.GetLastError();
                }
               

            }
            this.Dep.Emit(eTime - cTime);
        }

    }
}
