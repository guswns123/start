using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Real
{
    static class ChangeEdid
    {
        public static int[,] resolution = new int[10, 8];
        public static long[] pi = new long[10];
        public static int[] index = new int[10];
        public static int countm = 0;
        public static string[] read = new string[5];
        public static int des = 0;
        public static int[] de = new int[5];
        public static string dess = "";
        public static string[] proprety = new string[20];
        public static int dtd = 0;
    }
    static class Global
    {
        public static string EDID; // wirte EDID value
        public static int length = 0; // EDID length
        public static string WirteFlag;
    }
    class CheckSums : converter
    {
        public string SortsStr;
        public string strs;
        long checksum = 0;
        long checksum1 = 0;
        long che = 0;
        string chec = "";
        public CheckSums(string strss) : base(strss)
        {
            strss = (strss.Replace(Environment.NewLine, "")).ToUpper();
            strss = (strss.Replace("\r", "")).ToUpper();
            strss = strss.Replace(" ", "");
            while (strss.Substring(strss.Length - 1, 1) == " ")
                strss = strss.Remove(strss.Length - 1, 1);
            int e1 = strss.Length;
            strs = strss;
            for (int j = 0; j < e1; j = j + 256)
            {
                checksum1 = 0;
                for (int i = 0; 254 > i; i = i + 2)
                {
                    checksum1 = checksum1 + imformation10[i + j] * 16 + imformation10[i + 1 + j];
                }

                che = checksum1 % 256;
                checksum = 256 - che;
                if (checksum == 256) checksum = 0;
                chec = checksum.ToString("X");
                if (checksum < 16)
                    chec = "0" + chec;

                strs = strs.Remove((254 + j), 2);
                strs = strs.Insert((254 + j), chec);
            }
            sort check = new sort(strs);
            SortsStr = check.str;
        }
    }
    class resoution
    {
        public char ms_ms = ' ';
        public char ms = ' ';
        public char ls_m = ' ';
        public char ls_l = ' ';
        public string sf = "";
        public int flag;
        public resoution(int value)
        {
            string str = Convert.ToString(value, 2);
            if (str.Length <= 12)
            {
                for (int i = str.Length; i < 12; i++)
                    str = "0" + str;
                string m = Convert.ToString(Convert.ToInt32(str.Substring(0, 4), 2), 16);
                string l_m = Convert.ToString(Convert.ToInt32(str.Substring(4, 4), 2), 16);
                string l_l = Convert.ToString(Convert.ToInt32(str.Substring(8, 4), 2), 16);
                ms = Convert.ToChar(m);
                ls_m = Convert.ToChar(l_m);
                ls_l = Convert.ToChar(l_l);
            }
            else
            {
                flag = 1;
                System.Windows.MessageBox.Show("값의 범위를 벗어났습니다.");
            }
        }
        public resoution(int value, string b)
        {
            string str = Convert.ToString(value, 2);
            if (b == "H")
            {
                if (str.Length <= 10)
                {
                    for (int i = str.Length; i < 10; i++)
                        str = "0" + str;
                    string l_m = Convert.ToString(Convert.ToInt32(str.Substring(2, 4), 2), 16);
                    string l_l = Convert.ToString(Convert.ToInt32(str.Substring(6, 4), 2), 16);
                    ls_m = Convert.ToChar(l_m);
                    ls_l = Convert.ToChar(l_l);
                    sf = str.Substring(0, 2);
                }
                else
                {
                    flag = 1;
                    System.Windows.MessageBox.Show("값의 범위를 벗어났습니다.");
                }
            }
            else if (b == "V")
            {
                if (str.Length <= 6)
                {
                    for (int i = str.Length; i < 6; i++)
                        str = "0" + str;
                    string l_l = Convert.ToString(Convert.ToInt32(str.Substring(2, 4), 2), 16);
                    ls_l = Convert.ToChar(l_l);
                    sf = str.Substring(0, 2);
                }
                else
                {
                    flag = 1;
                    System.Windows.MessageBox.Show("값의 범위를 벗어났습니다.");
                }
            }
            else if (b == "P")
            {
                if (str.Length <= 16)
                {
                    for (int i = str.Length; i < 16; i++)
                        str = "0" + str;
                    string m_m = Convert.ToString(Convert.ToInt32(str.Substring(0, 4), 2), 16);
                    string m = Convert.ToString(Convert.ToInt32(str.Substring(4, 4), 2), 16);
                    string l_m = Convert.ToString(Convert.ToInt32(str.Substring(8, 4), 2), 16);
                    string l_l = Convert.ToString(Convert.ToInt32(str.Substring(12, 4), 2), 16);
                    ms_ms = Convert.ToChar(m_m);
                    ms = Convert.ToChar(m);
                    ls_m = Convert.ToChar(l_m);
                    ls_l = Convert.ToChar(l_l);
                }
                else
                {
                    flag = 1;
                    System.Windows.MessageBox.Show("값의 범위를 벗어났습니다.");
                }
            }
        }
    }
    class fileroad
    {
        public string arr = "";
        public string n = "";
        string Extension = "";
        public fileroad()
        {
            string l = "";
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Multiselect = false;
            opendlg.Filter = "Text, Binary Files(*.txt;*.bin) |*.txt;*.bin";
            if (opendlg.ShowDialog() != 0)
            {
                l = opendlg.FileName;
                int index = l.LastIndexOf("\\");
                int indexs = l.LastIndexOf(".");
                n = l.Substring(index + 1);
                Extension = l.Substring(indexs + 1);
            }
            try
            {
                if (Extension == "txt")
                {
                    using (StreamReader sr = new StreamReader(new FileStream(l, FileMode.Open)))
                    {
                        arr = (sr.ReadToEnd()).ToUpper();
                    }
                }
                else if(Extension == "bin")
                {
                    byte[] buff = File.ReadAllBytes(l);
                    foreach (byte b in buff)
                    {
                        if(b > 15)
                            arr += Convert.ToString(b, 16);
                        else
                            arr += "0" + Convert.ToString(b, 16);
                    }
                }
                sort sort = new sort(arr);
                Global.length = sort.strr.Length;
            }
            catch
            {
            }
        }
        public fileroad(bool? Txt, bool? Bin, string path, string edid)
        {
            try
            {
                if (Txt == true)
                {
                    path += ".txt";
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Exists == true)
                    {
                        System.Windows.MessageBox.Show("A file with the same name exists in the specified path.");
                        path = "";
                    }
                    System.IO.File.WriteAllText(path, edid, Encoding.Default);
                }
                else if (Bin == true)
                {
                    path += ".bin";
                    if (File.Exists(path))
                    {
                        System.Windows.MessageBox.Show("A file with the same name exists in the specified path.");
                        path = "";
                    }
                    else
                    {
                        sort sort = new sort(edid);
                        byte[] arr_byteStr = new byte[sort.strr.Length / 2];
                        for (int i = 0; i < sort.strr.Length; i = i + 2)
                        {
                            arr_byteStr[i / 2] = Convert.ToByte(sort.strr.Substring(i, 2), 16);
                        }
                        FileStream fs = File.Open(path, FileMode.Create);
                        using (BinaryWriter wr = new BinaryWriter(fs, Encoding.UTF7))
                        {
                            foreach (byte b in arr_byteStr)
                                wr.Write(b);
                        }
                        System.Windows.MessageBox.Show("save file success.");
                    }
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("save file failing.");
            }
        }
    }
    class Initialization
    {
        public Initialization()
        {
            for (int cho = 0; cho < ChangeEdid.countm; cho++)
                ChangeEdid.read[cho] = string.Empty;
            ChangeEdid.countm = 0;
            ChangeEdid.des = 0;
            ChangeEdid.dess = "";
            ChangeEdid.dtd = 0;
            Global.WirteFlag = "";
        }

    }
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
    class Namechanger
    {
        public string edidcn = "";
        int count = 0;
        public Namechanger(char[] arr16, byte[] arr_byteStr)
        {
            string[] result = new string[arr_byteStr.Length];
            char[] arr = new char[arr_byteStr.Length * 2];
            for (int i = 0; i < arr_byteStr.Length; i++)
            {
                result[i] = string.Format("{0:X2}", arr_byteStr[i]);
                arr[i * 2] = Convert.ToChar(result[i].Substring(0, 1));
                arr[(i * 2) + 1] = Convert.ToChar(result[i].Substring(1, 1));
            }
            for (int j = 0; j < ChangeEdid.des; j++)
            {
                for (int i = 10; i < 36; i++)
                {
                    try
                    {
                        arr16[ChangeEdid.de[j] + i] = arr[i - 10 + (j * 38)];
                    }
                    catch
                    {
                        count++;
                        if (count == 1)
                            arr16[ChangeEdid.de[j] + i] = 'A';
                        else if (count % 2 == 1)
                            arr16[ChangeEdid.de[j] + i] = '2';
                        else if (count % 2 == 0)
                            arr16[ChangeEdid.de[j] + i] = '0';

                    }
                }
            }
            foreach (char c in arr16)
                edidcn += c;
        }
    }
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChenaged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public DelegateCommand(Action exectue) : this(exectue, null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            this.execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
