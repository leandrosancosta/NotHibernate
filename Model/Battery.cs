using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NotHibernate.Model
{
    internal class Battery
    {
        public uint? Availability { get; set; }
        public uint? BatteryRechargeTime { get; set; }
        public uint? BatteryStatus { get; set; }
        public string? Caption { get; set; }
        public uint? Chemistry { get; set; }
        public uint? ConfigManagerErrorCode { get; set; }
        public bool? ConfigManagerUserConfig { get; set; }
        public string? CreationClassName { get; set; }
        public string? Description { get; set; }
        public uint? DesignCapacity { get; set; }
        public uint? DesignVoltage { get; set; }
        public string? DeviceID { get; set; }
        public bool? ErrorCleared { get; set; }
        public string? ErrorDescription { get; set; }
        public uint? EstimatedChargeRemaining { get; set; }
        public uint? EstimatedRunTime { get; set; }
        public uint? ExpectedBatteryLife { get; set; }
        public uint? ExpectedLife { get; set; }
        public uint? FullChargeCapacity { get; set; }
        public DateTime? InstallDate { get; set; }
        public uint? LastErrorCode { get; set; }
        public uint? MaxRechargeTime { get; set; }
        public string? Name { get; set; }
        public string? PNPDeviceID { get; set; }
        public uint[]? PowerManagementCapabilities { get; set; }
        public bool? PowerManagementSupported { get; set; }
        public string? SmartBatteryVersion { get; set; }
        public string? Status { get; set; }
        public uint? StatusInfo { get; set; }
        public string? SystemCreationClassName { get; set; }
        public string? SystemName { get; set; }
        public uint? TimeOnBattery { get; set; }
        public uint? TimeToFullCharge { get; set; }
    }
}
