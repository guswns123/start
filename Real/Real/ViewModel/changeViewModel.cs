using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Real.ViewModel
{
    class changeViewModel : Notifier
    {
        List<string> DTB = new List<string>(new string[ChangeEdid.dtd]);
        public changeViewModel()
        {
            for (int i = 0; i < ChangeEdid.dtd; i++)
                DTB.Add("DTB" + (i + 1));
        }
        public List<string> SelectDTB
        {
            get { return DTB; }
            set {  }
        }
        string hsyns;
        string hfront;
        string hactive;
        string hback;
        string vsyns;
        string vfront;
        string vactive;
        string vback;
        public string h_syns
        {
            get { return hsyns; }
            set
            {
                hsyns = value;
                OnPropertyChenaged("h_syns");
            }
        }
        public string h_front
        {
            get { return hfront; }
            set
            {
                hfront = value;
                OnPropertyChenaged("h_front");
            }
        }
        public string h_active
        {
            get { return hactive; }
            set
            {
                hactive = value;
                OnPropertyChenaged("h_active");
            }
        }
        public string h_back
        {
            get { return hback; }
            set
            {
                hback = value;
                OnPropertyChenaged("h_back");
            }
        }
        public string v_syns
        {
            get { return vsyns; }
            set
            {
                vsyns = value;
                OnPropertyChenaged("v_syns");
            }
        }
        public string v_front
        {
            get { return vfront; }
            set
            {
                vfront = value;
                OnPropertyChenaged("v_front");
            }
        }
        public string v_active
        {
            get { return vactive; }
            set
            {
               vactive = value;
                OnPropertyChenaged("v_active");
            }
        }
        public string v_back
        {
            get { return vback; }
            set
            {
                vback = value;
                OnPropertyChenaged("v_back");
            }
        }
        string pi;
        public string PI
        {
            get { return pi; }
            set
            {
                pi = value;
                OnPropertyChenaged("PI");
            }
        }
        int index;
        public int SelcetIndex
        {
            set
            {
                index = value;
                OnPropertyChenaged("SelcetIndex");
                OnIndexChanged();
            }
        }
        void OnIndexChanged()
        {
            h_syns = ChangeEdid.resolution[index - 1, 0].ToString();
            h_back = ChangeEdid.resolution[index - 1, 1].ToString();
            h_active = ChangeEdid.resolution[index - 1, 2].ToString();
            h_front = ChangeEdid.resolution[index - 1, 3].ToString();
            v_syns = ChangeEdid.resolution[index - 1, 4].ToString();
            v_back = ChangeEdid.resolution[index - 1, 5].ToString();
            v_active = ChangeEdid.resolution[index - 1, 6].ToString();
            v_front = ChangeEdid.resolution[index - 1, 7].ToString();
            int p = 0;
            int i = 0;

            for (int j = 0; j < 4; j++)
            {
                p += ChangeEdid.resolution[index - 1, j];
                i += ChangeEdid.resolution[index - 1, j + 4];
            }
            double piclock = Math.Round(((float)((ChangeEdid.pi[index - 1] * 10000) / (float)(p * i))), 3);

            PI = piclock.ToString();
        }
        sort sort = new sort(Global.EDID);
        ICommand changeedid;
        public ICommand ChangeEDID
        {
            get { return (this.changeedid) ?? (this.changeedid = new DelegateCommand(changed)); }

        }
        void changed()
        {
            char[] arr16 = sort.strc;
            try
            {
                double clock = Convert.ToDouble(pi);
                int p = Convert.ToInt32(hsyns) + Convert.ToInt32(hback) + Convert.ToInt32(hactive) + Convert.ToInt32(hfront);
                int i = Convert.ToInt32(vsyns) + Convert.ToInt32(vback) + Convert.ToInt32(vactive) + Convert.ToInt32(vfront);

                if ((p * i * clock) == 0)
                    vactive = null;

                resoution piclock = new resoution((int)(p * i * clock) / 10000, "P");
                resoution h_syns = new resoution(Convert.ToInt32(hsyns), "H");
                resoution h_back = new resoution(Convert.ToInt32(hback) + Convert.ToInt32(hsyns) + Convert.ToInt32(hfront));
                resoution h_active = new resoution(Convert.ToInt32(hactive));
                resoution h_front = new resoution(Convert.ToInt32(hfront), "H");
                resoution v_syns = new resoution(Convert.ToInt32(vsyns), "V");
                resoution v_back = new resoution(Convert.ToInt32(vback) + Convert.ToInt32(vsyns) + Convert.ToInt32(vfront));
                resoution v_active = new resoution(Convert.ToInt32(vactive));
                resoution v_front = new resoution(Convert.ToInt32(vfront), "V");

                char h = Convert.ToChar(Convert.ToString(Convert.ToInt32(h_front.sf + h_syns.sf, 2), 16));
                char v = Convert.ToChar(Convert.ToString(Convert.ToInt32(v_front.sf + v_syns.sf, 2), 16));

                if (piclock.flag + h_active.flag + h_back.flag + h_front.flag + h_syns.flag + v_active.flag + v_back.flag + v_front.flag + v_syns.flag >= 1)
                    goto jump;

                arr16[ChangeEdid.index[index - 1]] = piclock.ls_m;
                arr16[ChangeEdid.index[index - 1] + 1] = piclock.ls_l;
                arr16[ChangeEdid.index[index - 1] + 2] = piclock.ms_ms;
                arr16[ChangeEdid.index[index - 1] + 3] = piclock.ms;
                arr16[ChangeEdid.index[index - 1] + 4] = h_active.ls_m;
                arr16[ChangeEdid.index[index - 1] + 5] = h_active.ls_l;
                arr16[ChangeEdid.index[index - 1] + 6] = h_back.ls_m;
                arr16[ChangeEdid.index[index - 1] + 7] = h_back.ls_l;
                arr16[ChangeEdid.index[index - 1] + 8] = h_active.ms;
                arr16[ChangeEdid.index[index - 1] + 9] = h_back.ms;
                arr16[ChangeEdid.index[index - 1] + 10] = v_active.ls_m;
                arr16[ChangeEdid.index[index - 1] + 11] = v_active.ls_l;
                arr16[ChangeEdid.index[index - 1] + 12] = v_back.ls_m;
                arr16[ChangeEdid.index[index - 1] + 13] = v_back.ls_l;
                arr16[ChangeEdid.index[index - 1] + 14] = v_active.ms;
                arr16[ChangeEdid.index[index - 1] + 15] = v_back.ms;
                arr16[ChangeEdid.index[index - 1] + 16] = h_front.ls_m;
                arr16[ChangeEdid.index[index - 1] + 17] = h_front.ls_l;
                arr16[ChangeEdid.index[index - 1] + 18] = h_syns.ls_m;
                arr16[ChangeEdid.index[index - 1] + 19] = h_syns.ls_l;
                arr16[ChangeEdid.index[index - 1] + 20] = v_front.ls_l;
                arr16[ChangeEdid.index[index - 1] + 21] = v_syns.ls_l;
                arr16[ChangeEdid.index[index - 1] + 22] = h;
                arr16[ChangeEdid.index[index - 1] + 23] = v;


                string edidc = "";
                foreach (char c in arr16)
                    edidc += c;
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
