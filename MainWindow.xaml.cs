using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Configuration;
using System.IO;
using ComputerShutdown.Model;

namespace ComputerShutdown
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.Init();

            var programPath = AppDomain.CurrentDomain.BaseDirectory;

            var configFileName = "ComputerShutdown.exe.config";

            string path = Path.Combine(programPath, configFileName);

            this.ConfigHelper = new ConfigHelper(path);
        }
        private const string WINDOW_POSITION = "WindowPosition";

        private const string Timer_TYPE = "Timer_TYPE";

        private const string SET_TIME = "SetTime";

        private ConfigHelper ConfigHelper
        {
            get;
            set;
        }

        private Setting GetConfigInfo()
        {
           /* string windowPosition = ConfigurationManager.AppSettings[WINDOW_POSITION];

            string [] array = windowPosition.*/

            
            string chooseType = ConfigurationManager.AppSettings[Timer_TYPE];
            int type = Convert.ToInt32(chooseType);

            string timerTime = ConfigurationManager.AppSettings[SET_TIME];

            double time = Convert.ToDouble(timerTime);


            string WPStr = ConfigurationManager.AppSettings[WINDOW_POSITION];

            Setting setting = new Setting();



            if (WPStr == null)
            {
                setting.WindowPosition = null;
            } else
            {
                string [] array = WPStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length != 2)
                {
                    setting.WindowPosition = null;
                } else
                {

                    if (!double.TryParse(array[0], out double left) || !double.TryParse(array[1], out double top))
                    {
                        setting.WindowPosition = null;
                    } else
                    {
                        setting.WindowPosition = new WindowPosition()
                        {
                            Left = left,
                            Top = top,
                        };
                    }
                }
                
            }



            setting.TimerType = type;

            setting.SetTime = time;


            return setting;

        }

        private void Init()
        {



            Setting setting = this.GetConfigInfo();


            this.ResizeMode = ResizeMode.CanMinimize;

            this.Operate.Content = START;

            /*
             * 0: 时
             * 1: 分
             * 2: 秒
             */
            this.TimerType = setting.TimerType;

            if (setting.WindowPosition == null)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            } else
            {
                this.Top = setting.WindowPosition.Top;
                this.Left = setting.WindowPosition.Left;
            }

            TimerInput.Text = setting.SetTime.ToString();

            if (this.TimerType == 0)
            {
                this.Hour.IsChecked = true;
            } else if (this.TimerType == 1)
            {
                this.Minute.IsChecked = true;
            } else
            {
                this.Second.IsChecked = true;
            }

            this.Hour.Checked += (object sender, RoutedEventArgs e) => this.RadioButtonEventHandle(sender, e);
            this.Minute.Checked += (object sender, RoutedEventArgs e) => this.RadioButtonEventHandle(sender, e);
            this.Second.Checked += (object sender, RoutedEventArgs e) => this.RadioButtonEventHandle(sender, e);

            this.btnAbout.Click += BtnAbout_Click;

            Dict = new Dictionary<int, TimerTypeEmum>();
            this.Dict.Add(0, TimerTypeEmum.Hour);
            this.Dict.Add(1, TimerTypeEmum.Minute);
            this.Dict.Add(2, TimerTypeEmum.Second);

            this.ShutdownTimer = new ShutdownTimer();
            this.Dep = Dep.GetInstance();
            this.Dep.Event += Dep_Event;
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            Instructions instructions = new Instructions(this.Left, this.Top);
            instructions.ShowDialog();
        }

        private void Dep_Event(object obj)
        {
            if (!long.TryParse(obj.ToString(), out long microsecond))
            {
                return;
            }
            // 显示剩余时间

            // 微秒转秒
            var s = microsecond / 10000000.0;

            int second = (int)s;

            int hour = second / 3600;

            int minute = second % 3600 / 60;

            
            int sec = second % 60;

            var message = String.Format("{0}小时{1}分钟{2}秒\r\n后将自动电脑关机", hour, minute, sec);

            this.RemainingTime.Dispatcher.Invoke(new Action(() =>  this.RemainingTime.Text = message ));
        }

        private Dep Dep
        {
            get;
            set;
        }
        private int TimerType {
            get;
            set;
        }
        private ShutdownTimer ShutdownTimer
        {
            get;
            set;
        }
        private const string START = "启动";
        private const string STOP = "停止";
        private Dictionary<int, TimerTypeEmum> Dict {
            get;
            set;
        }
        private void Run(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var inputMessage = this.TimerInput.Text;
            double number;
            if (!double.TryParse(inputMessage, out number))
            {
                MessageBox.Show("输入的时间格式不对", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var type = this.TimerType;

            int second =  Utils.TransfromSecond(number, this.Dict[type]);
            if (second < 5)
            {
                MessageBox.Show("输入时间需要大于 5 秒", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (sender == null)
            {
                return;
            }
            if (Convert.ToBoolean(button.Tag) == false)
            {
                button.Content = STOP;
                // 启动定时
                button.Tag = true;

                bool b = this.ShutdownTimer.StartTimer(second);

                ConfigHelper.SetConfig(SET_TIME, number.ToString());

               
            }
            else
            {
                button.Content = START;
                this.ShutdownTimer.StopTimer();
                this.RemainingTime.Text = "";
                button.Tag = false;
            }
        }

        private void RadioButtonEventHandle(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton == null)
            {
                return;
            }
            this.TimerType = Convert.ToInt32(radioButton.Tag);

            this.ConfigHelper.SetConfig(Timer_TYPE, this.TimerType.ToString());

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ShutdownTimer.StopTimer();
            var value = string.Format("{0},{1}", this.Left, this.Top);
            this.ConfigHelper.SetConfig(WINDOW_POSITION, value);

        }
    }
}
