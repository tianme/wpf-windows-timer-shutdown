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
using System.Windows.Shapes;

namespace ComputerShutdown
{
    /// <summary>
    /// Instructions.xaml 的交互逻辑
    /// </summary>
    public partial class Instructions : Window
    {
        public Instructions(double left, double top)
        {
            InitializeComponent();
            this.Top = top;
            this.Left = left;
            this.ResizeMode = ResizeMode.CanMinimize;
            this.Remarks.Text = @"程序在指定时间强制关机，
如有未保存的应用，会导致丢失数据。";
            this.Info.Text = @"输入框：支持输入数字或小数


单选框：可以选择时、分、秒


备注：如果单选框选择了秒，并且输入框填写了小数，会向下取整


";
        }

        private void linkHelp_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            // 激活的是当前默认的浏览器
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }
    }
}
