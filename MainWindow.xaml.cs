using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace NotHibernate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer Timer = new Timer();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionstate(EXECUTION_STATE esFlag);
        public MainWindow()
        {
            InitializeComponent();
            IniciaTImer();
        }

        private void IniciaTImer()
        {
            this.Timer.Interval = 60000;
            this.Timer.Enabled = true;
            this.Timer.Elapsed += new ElapsedEventHandler(this.IniciaProcessamento);
        }

        private void IniciaProcessamento(object? sender, ElapsedEventArgs e)
        {
            try
            {
                this.Timer.Stop();
                ForceSystemAwake();
                this.Timer.Start();
            }
            catch (Exception ex)
            {
                this.LabelStatus.Content = "Inativo";
                this.LabelStatus.Foreground = new BrushConverter().ConvertFromString("#FF0000") as SolidColorBrush;
            }
        }

        private void ForceSystemAwake()
        {
            int num = (int)SetThreadExecutionstate(EXECUTION_STATE.ES_AWAYMODE_REQUIRED | EXECUTION_STATE.ES_CONTINUOS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 64,
            ES_CONTINUOS = 2147483648,
            ES_DISPLAY_REQUIRED = 2,
            ES_SYSTEM_REQUIRED = 1
        }
    }
}
