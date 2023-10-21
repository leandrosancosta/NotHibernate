using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Management;
using NotHibernate.Model;
using System.Windows.Controls;
using static NotHibernate.MainWindow;

namespace NotHibernate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void DelegateChamadaSegura(string text, string corFonte);
        private Timer Timer = new Timer();


        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        //public static extern EXECUTION_STATE 
        //static extern EXECUTION_STATE 
        //SetThreadExecutionstate(EXECUTION_STATE esFlag);
        private object _lock = new object();

        public MainWindow()
        {
            InitializeComponent();

            IniciaTImer();
        }

        private void IniciaTImer()
        {
            this.Timer.Interval = 60000;
            this.Timer.Enabled = true;
#if DEBUG
            this.Timer.Interval = 10000;
#endif
            this.Timer.Elapsed += new ElapsedEventHandler(this.IniciaProcessamento);
        }

        private void IniciaProcessamento(object? sender, ElapsedEventArgs e)
        {
            try
            {
                this.Timer.Stop();
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.LabelStatus.Content = "Ativo";
                    this.LabelStatus.Foreground = new BrushConverter().ConvertFromString("#0000FF") as SolidColorBrush;
                }));

                string LabelMessage = string.Empty;
                if (!ReturnBatteryStatus(ref LabelMessage))
                {
                    throw new Exception(LabelMessage);
                }
                else
                {
                    ForceSystemAwake();
                }

                this.Timer.Start();
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.LabelStatus.Content = $"Inativo: {ex.Message}";
                    this.LabelStatus.Foreground = new BrushConverter().ConvertFromString("#FF0000") as SolidColorBrush;
                }));
            }
        }

        private bool ReturnBatteryStatus(ref string error)
        {
            ObjectQuery query = new ObjectQuery("Select * FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection collection = searcher.Get();
            Battery? battery = null;
            try
            {
                foreach (ManagementObject mo in collection)
                {
                    battery = new Battery()
                    {
                        Availability = mo.Properties["Availability"].Value == null ? null : uint.Parse(mo.Properties["Availability"].Value.ToString()),
                        BatteryRechargeTime = mo.Properties["BatteryRechargeTime"].Value == null ? null : uint.Parse(mo.Properties["BatteryRechargeTime"].Value.ToString()),
                        BatteryStatus = mo.Properties["BatteryStatus"].Value == null ? null : uint.Parse(mo.Properties["BatteryStatus"].Value.ToString()),
                        Caption = mo.Properties["Caption"].Value == null ? null : mo.Properties["Caption"].Value.ToString(),
                        Chemistry = mo.Properties["Chemistry"].Value == null ? null : uint.Parse(mo.Properties["Chemistry"].Value.ToString()),
                        ConfigManagerErrorCode = mo.Properties["ConfigManagerErrorCode"].Value == null ? null : uint.Parse(mo.Properties["ConfigManagerErrorCode"].Value.ToString()),
                        ConfigManagerUserConfig = mo.Properties["ConfigManagerUserConfig"].Value == null ? null : bool.Parse(mo.Properties["ConfigManagerUserConfig"].Value.ToString()),
                        CreationClassName = mo.Properties["CreationClassName"].Value.ToString(),
                        Description = mo.Properties["Description"].Value == null ? null : mo.Properties["Description"].Value.ToString(),
                        DesignCapacity = mo.Properties["DesignCapacity"].Value == null ? null : uint.Parse(mo.Properties["DesignCapacity"].Value.ToString()),
                        DesignVoltage = mo.Properties["DesignVoltage"].Value == null ? null : uint.Parse(mo.Properties["DesignVoltage"].Value.ToString()),
                        DeviceID = mo.Properties["DeviceID"].Value.ToString(),
                        ErrorCleared = mo.Properties["ErrorCleared"].Value == null ? null : bool.Parse(mo.Properties["ErrorCleared"].Value.ToString()),
                        ErrorDescription = mo.Properties["ErrorDescription"].Value == null ? null : mo.Properties["ErrorDescription"].Value.ToString(),
                        EstimatedChargeRemaining = mo.Properties["EstimatedChargeRemaining"].Value == null ? null : uint.Parse(mo.Properties["EstimatedChargeRemaining"].Value.ToString()),
                        EstimatedRunTime = mo.Properties["EstimatedRunTime"].Value == null ? null : uint.Parse(mo.Properties["EstimatedRunTime"].Value.ToString()),
                        ExpectedBatteryLife = mo.Properties["ExpectedBatteryLife"].Value == null ? null : uint.Parse(mo.Properties["ExpectedBatteryLife"].Value.ToString()),
                        ExpectedLife = mo.Properties["ExpectedLife"].Value == null ? null : uint.Parse(mo.Properties["ExpectedLife"].Value.ToString()),
                        FullChargeCapacity = mo.Properties["FullChargeCapacity"].Value == null ? null : uint.Parse(mo.Properties["FullChargeCapacity"].Value.ToString()),
                        InstallDate = mo.Properties["InstallDate"].Value == null ? null : new DateTime(long.Parse(mo.Properties["InstallDate"].Value.ToString())),
                        LastErrorCode = mo.Properties["LastErrorCode"].Value == null ? null : uint.Parse(mo.Properties["LastErrorCode"].Value.ToString()),
                        MaxRechargeTime = mo.Properties["MaxRechargeTime"].Value == null ? null : uint.Parse(mo.Properties["MaxRechargeTime"].Value.ToString()),
                        Name = mo.Properties["Name"].Value == null ? null : mo.Properties["Name"].Value.ToString(),
                        PNPDeviceID = mo.Properties["PNPDeviceID"].Value == null ? null : mo.Properties["PNPDeviceID"].Value.ToString(),
                        PowerManagementSupported = mo.Properties["PowerManagementSupported"].Value == null ? null : bool.Parse(mo.Properties["PowerManagementSupported"].Value.ToString()),
                        SmartBatteryVersion = mo.Properties["SmartBatteryVersion"].Value == null ? null : mo.Properties["SmartBatteryVersion"].Value.ToString(),
                        Status = mo.Properties["Status"].Value == null ? null : (mo.Properties["Status"].Value.ToString()),
                        StatusInfo = mo.Properties["StatusInfo"].Value == null ? null : uint.Parse(mo.Properties["StatusInfo"].Value.ToString()),
                        SystemCreationClassName = mo.Properties["SystemCreationClassName"].Value == null ? null : (mo.Properties["SystemCreationClassName"].Value.ToString()),
                        SystemName = mo.Properties["SystemName"].Value == null ? null : (mo.Properties["SystemName"].Value.ToString()),
                        TimeOnBattery = mo.Properties["TimeOnBattery"].Value == null ? null : uint.Parse(mo.Properties["TimeOnBattery"].Value.ToString()),
                        TimeToFullCharge = mo.Properties["TimeToFullCharge"].Value == null ? null : uint.Parse(mo.Properties["TimeToFullCharge"].Value.ToString())
                    };
                }
            }
            catch
            {

            }

            if (battery == null)
            {
                error = "Não foi possível carregar as informações da bateria";
                return false;
            }

            if (battery.BatteryStatus != 1 && battery.BatteryStatus != 2 && battery.BatteryStatus != 3 && battery.BatteryStatus != 6 && battery.BatteryStatus != 7 && battery.BatteryStatus != 8 && battery.BatteryStatus != 9 && battery.BatteryStatus != 11)
            {
                error = "Bateria com nível baixo ou não foi possível identificar";
                return false;
            }

            if (battery.EstimatedRunTime == 71582788)
                return true;

            if (battery.EstimatedChargeRemaining < 20)
            {
                error = "Bateria com nível abaixo de 20%";
                return false;
            }

            return true;
        }

        private void ForceSystemAwake()
        {
            int num = (int)SetThreadExecutionState(EXECUTION_STATE.ES_AWAYMODE_REQUIRED | EXECUTION_STATE.ES_CONTINUOS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
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
