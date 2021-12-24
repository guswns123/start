using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Real.Model;
namespace Real.ViewModel
{
    class monitor
    {
        public monitor() { }
        public monitor(string name) { this.Name = name; }
        public string Name { set; get; }
        public static List<monitor> Monitor()
        {
            var mc = new System.Management.ManagementClass(string.Format(@"\\{0}\root\wmi:WmiMonitorDescriptorMethods", Environment.MachineName));
            foreach (ManagementObject mo in mc.GetInstances()) //Do this for each connected monitor
            {
                string[] split = mo.GetPropertyValue("InstanceName").ToString().Split(
                    new string[] { "\\" }, StringSplitOptions.None);
                ChangeEdid.proprety[ChangeEdid.countm] = split[1];
                ChangeEdid.countm++;
                for (int i = 0; i < 256; i++)
                {

                    var inParams = mo.GetMethodParameters("WmiGetMonitorRawEEdidV1Block");
                    inParams["BlockId"] = i;

                    ManagementBaseObject outParams = null;
                    try
                    {
                        outParams = mo.InvokeMethod("WmiGetMonitorRawEEdidV1Block", inParams, null);
                        uint blktype = Convert.ToUInt16(outParams["BlockType"]);


                        if ((blktype == 1) || (blktype == 255))
                        {
                            byte[] readbtye = (Byte[])outParams["BlockContent"];
                            for (int k = 0; k < readbtye.Length; k++)
                            {
                                if (readbtye[k] > 15)
                                    ChangeEdid.read[ChangeEdid.countm - 1] += readbtye[k].ToString("X") + " ";
                                else
                                    ChangeEdid.read[ChangeEdid.countm - 1] += "0" + readbtye[k].ToString("X") + " ";
                                if ((k % 16) == 15)
                                    ChangeEdid.read[ChangeEdid.countm - 1] += "\r";
                            }
                        }
                    }
                    catch
                    {
                        break;
                    } //No more EDID blocks

                }
            }
            List<monitor> m = new List<monitor>();
            foreach (string mo in ChangeEdid.proprety)
            {
                m.Add(new monitor(mo));
            }
            return m;
        }
    }
    class readViewModel : Notifier
    {
        EDIDModel read = new EDIDModel();

        public List<monitor> ListMonitor
        { 
            get
            {
                return read.m;
            }
            set
            {
                OnPropertyChenaged("ListBoxItem");
            }
        }

        public int IndexMonitor
        {
            set
            {
                read.indexmonitor = value;
                OnPropertyChenaged("IndexMonitor");
                OnIndexChanged();
            }
        }

        void OnIndexChanged()
        {
            ReadEdid = ChangeEdid.read[read.indexmonitor];
        }
        public string ReadEdid
        {
            get { return read.readedid; }
            set
            {
                read.readedid = value;
                OnPropertyChenaged("ReadEdid");
            }
        }

        public ICommand SelectEdid
        {         
            get { return (this.read.selectedid) ?? (this.read.selectedid = new DelegateCommand(selected)); }

        }
        void selected()
        {
            Global.EDID = read.readedid;
        }


    }
}
