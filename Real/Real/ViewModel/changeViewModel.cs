using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EDIDParser;
using Real.Model;
namespace Real.ViewModel
{
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
                System.Windows.MessageBox.Show("Value Range Exceeded.");
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
                    System.Windows.MessageBox.Show("Value Range Exceeded.");
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
                    System.Windows.MessageBox.Show("Value Range Exceeded.");
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
                    System.Windows.MessageBox.Show("Value Range Exceeded.");
                }
            }
        }
    }
    class changeViewModel : Notifier
    {
        EDIDModel Change = new EDIDModel();

        List<string> DTB = new List<string>(new string[ChangeEdid.dtd]);

        public changeViewModel()
        {
            for (int i = 0; i < ChangeEdid.dtd; i++)
                DTB[i] = ("DTB" + (i + 1));
        }

        public List<string> SelectDTB
        {
            get { return DTB; }
            set {  }
        }

        string AcsllRetrun(string value)
        {
            string OnlyNumber = "";
            byte[] acsll = Encoding.ASCII.GetBytes(value);
            if (value != "")
            {
                if ((48 <= acsll[acsll.Length - 1] && acsll[acsll.Length - 1] <= 57) || acsll[acsll.Length - 1] == 46)
                    OnlyNumber = value;
                else
                    OnlyNumber = value.Remove(value.Length - 1, 1);
            }
           return OnlyNumber;
        }

        public string h_syns
        {
            get{ return Change.hSyns; }
            set
            {
                Change.hSyns = AcsllRetrun(value);
                OnPropertyChenaged("h_syns");
            }
        }

        public string h_front
        {
            get { return Change.hFront; }
            set
            {
                Change.hFront = AcsllRetrun(value);
                OnPropertyChenaged("h_front");
            }
        }

        public string h_active
        {
            get { return Change.hActive; }
            set
            {
                Change.hActive = AcsllRetrun(value);
                OnPropertyChenaged("h_active");
            }
        }

        public string h_back
        {
            get { return Change.hBack; }
            set
            {
                Change.hBack = AcsllRetrun(value);
                OnPropertyChenaged("h_back");
            }
        }

        public string v_syns
        {
            get { return Change.vSyns; }
            set
            {
                Change.vSyns = AcsllRetrun(value);
                OnPropertyChenaged("v_syns");
            }
        }

        public string v_front
        {
            get { return Change.vFront; }
            set
            {
                Change.vFront = AcsllRetrun(value);
                OnPropertyChenaged("v_front");
            }
        }

        public string v_active
        {
            get { return Change.vActive; }
            set
            {
                Change.vActive = AcsllRetrun(value);
                OnPropertyChenaged("v_active");
            }
        }

        public string v_back
        {
            get { return Change.vBack; }
            set
            {
                Change.vBack = AcsllRetrun(value);
                OnPropertyChenaged("v_back");
            }
        }

        public string PI
        {
            get { return Change.pi; }
            set
            {
                Change.pi = AcsllRetrun(value);
                OnPropertyChenaged("PI");
            }
        }

        public int SelcetIndex
        {
            set
            {
                Change.index = value;
                OnPropertyChenaged("SelcetIndex");
                OnIndexChanged();
            }
        }

        void OnIndexChanged()
        {
            h_syns = ChangeEdid.resolution[Change.index , 0].ToString();
            h_back = ChangeEdid.resolution[Change.index , 1].ToString();
            h_active = ChangeEdid.resolution[Change.index , 2].ToString();
            h_front = ChangeEdid.resolution[Change.index, 3].ToString();
            v_syns = ChangeEdid.resolution[Change.index, 4].ToString();
            v_back = ChangeEdid.resolution[Change.index, 5].ToString();
            v_active = ChangeEdid.resolution[Change.index, 6].ToString();
            v_front = ChangeEdid.resolution[Change.index, 7].ToString();
            int p = 0;
            int i = 0;

            for (int j = 0; j < 4; j++)
            {
                p += ChangeEdid.resolution[Change.index, j];
                i += ChangeEdid.resolution[Change.index, j + 4];
            }
            double piclock = Math.Round(((float)((ChangeEdid.pi[Change.index] * 10000) / (float)(p * i))), 3);
            PI = piclock.ToString();
        }


        ICommand changeedid;

        public ICommand ChangeEDID
        {
            get { return (this.changeedid) ?? (this.changeedid = new DelegateCommand(changed)); }

        }

        void changed()
        {
            sort sort = new sort(Global.EDID);
            char[] arr16 = sort.strc;
            try
            {
                double clock = Convert.ToDouble(Change.pi);
                int p = Convert.ToInt32(Change.hSyns) + Convert.ToInt32(Change.hBack) + Convert.ToInt32(Change.hActive) + Convert.ToInt32(Change.hFront);
                int i = Convert.ToInt32(Change.vSyns) + Convert.ToInt32(Change.vBack) + Convert.ToInt32(Change.vActive) + Convert.ToInt32(Change.vFront);

                if ((p * i * clock) == 0)
                    Change.vActive = null;

                resoution piclock = new resoution((int)(p * i * clock) / 10000, "P");
                resoution h_syns = new resoution(Convert.ToInt32(Change.hSyns), "H");
                resoution h_back = new resoution(Convert.ToInt32(Change.hBack) + Convert.ToInt32(Change.hSyns) + Convert.ToInt32(Change.hFront));
                resoution h_active = new resoution(Convert.ToInt32(Change.hActive));
                resoution h_front = new resoution(Convert.ToInt32(Change.hFront), "H");
                resoution v_syns = new resoution(Convert.ToInt32(Change.vSyns), "V");
                resoution v_back = new resoution(Convert.ToInt32(Change.vBack) + Convert.ToInt32(Change.vSyns) + Convert.ToInt32(Change.vFront));
                resoution v_active = new resoution(Convert.ToInt32(Change.vActive));
                resoution v_front = new resoution(Convert.ToInt32(Change.vFront), "V");

                char h = Convert.ToChar(Convert.ToString(Convert.ToInt32(h_front.sf + h_syns.sf, 2), 16));
                char v = Convert.ToChar(Convert.ToString(Convert.ToInt32(v_front.sf + v_syns.sf, 2), 16));

                if (piclock.flag + h_active.flag + h_back.flag + h_front.flag + h_syns.flag + v_active.flag + v_back.flag + v_front.flag + v_syns.flag >= 1)
                    goto jump;

                arr16[ChangeEdid.index[Change.index]] = piclock.ls_m;
                arr16[ChangeEdid.index[Change.index] + 1] = piclock.ls_l;
                arr16[ChangeEdid.index[Change.index] + 2] = piclock.ms_ms;
                arr16[ChangeEdid.index[Change.index] + 3] = piclock.ms;
                arr16[ChangeEdid.index[Change.index] + 4] = h_active.ls_m;
                arr16[ChangeEdid.index[Change.index] + 5] = h_active.ls_l;
                arr16[ChangeEdid.index[Change.index] + 6] = h_back.ls_m;
                arr16[ChangeEdid.index[Change.index] + 7] = h_back.ls_l;
                arr16[ChangeEdid.index[Change.index] + 8] = h_active.ms;
                arr16[ChangeEdid.index[Change.index] + 9] = h_back.ms;
                arr16[ChangeEdid.index[Change.index] + 10] = v_active.ls_m;
                arr16[ChangeEdid.index[Change.index] + 11] = v_active.ls_l;
                arr16[ChangeEdid.index[Change.index] + 12] = v_back.ls_m;
                arr16[ChangeEdid.index[Change.index] + 13] = v_back.ls_l;
                arr16[ChangeEdid.index[Change.index] + 14] = v_active.ms;
                arr16[ChangeEdid.index[Change.index] + 15] = v_back.ms;
                arr16[ChangeEdid.index[Change.index] + 16] = h_front.ls_m;
                arr16[ChangeEdid.index[Change.index] + 17] = h_front.ls_l;
                arr16[ChangeEdid.index[Change.index] + 18] = h_syns.ls_m;
                arr16[ChangeEdid.index[Change.index] + 19] = h_syns.ls_l;
                arr16[ChangeEdid.index[Change.index] + 20] = v_front.ls_l;
                arr16[ChangeEdid.index[Change.index] + 21] = v_syns.ls_l;
                arr16[ChangeEdid.index[Change.index] + 22] = h;
                arr16[ChangeEdid.index[Change.index] + 23] = v;

                string edidc = new string(arr16);
                CheckSums checksums = new CheckSums(edidc);
                edidc = checksums.SortsStr;
                Global.EDID = edidc;
                System.Windows.MessageBox.Show("DTB Change Successful");
            jump:;
            }
            catch
            {
                System.Windows.MessageBox.Show("DTB Change Failing");
            }
        }


    }
}
