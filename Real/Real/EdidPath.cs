using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real
{
    static class Constants
    {
        public const int m = 36; /// <summary>
                                 /// one DTD lenth
                                 /// </summary>
        public const int l = 256;
        /// <summary>
        /// one Block length
        /// </summary>
    }
    class sort
    {
        int count = 0;
        public char[] strc;
        public string str;
        public string strr;
        public sort(string str1)
        {
            str1 = (str1.Replace(Environment.NewLine, "")).ToUpper();
            str1 = (str1.Replace("\r", "")).ToUpper();
            str1 = str1.Replace(" ", "");
            strr = str1;
            strc = str1.ToCharArray();
            try
            {
                foreach (char c in strc)
                {
                    count++;
                    str = str + c;
                    if (count % 2 == 0 && count != 0 && count % 32 != 0)
                        str = str + " ";
                    if (count % 32 == 0)
                        str = str + "\r";
                }
            }
            catch
            { }
        }
    }
    class converter
    {
        public static char[] imformation16;
        public string[] imformation2 = new string[Global.length];
        public int[] imformation10 = new int[Global.length];
        public converter(string strss)
        {
            sort con = new sort(strss);
            imformation16 = con.strc;
            strss = (strss.Replace(Environment.NewLine, "")).ToUpper();
            strss = (strss.Replace("\r", "")).ToUpper();
            strss = strss.Replace(" ", "");
            imformation16 = strss.ToCharArray();
            for (int i = 0; i < imformation16.Length; i++)
            {
                imformation10[i] = (int)imformation16[i];
                if (imformation10[i] <= 57) imformation10[i] -= 48;
                else imformation10[i] -= 55;
                imformation2[i] = Convert.ToString(imformation10[i], 2);
                for (int j = imformation2[i].Length; j < 4; j++)
                    imformation2[i] = "0" + imformation2[i];
            }
        }

    }
    class EdidPath : converter
    {
        public string Pather = "";
        public EdidPath(string strss) : base(strss)
        {
            sort sort = new sort(strss);
            if (sort.strr.Length % 128 == 0 && sort.strr.Length != 0)
            {
                Initialization inti = new Initialization();
                Pather = Header();
                Pather += BasicDisplayParameters();
                Pather += ChromaticityCoordinates();
                Pather += EstablishedTimingBitmap();
                Pather += StandardDisplayModes();
                Pather += Description();
                for (int l = 0; l <= (imformation10[252] * 16 + imformation10[253]); l++)
                {
                    if (imformation10[1 + (l * Constants.l)] == 2 && l != 0)
                        ExtensionDataFormat(l);
                }
            }
            else
                Pather = "wrong value";
        }

        public string Header() // 0~~~~19 byte data 처리
        {
            string Header = "";
            string version = "";                                                                         // EDID 버전 표기 해야함
            string date = "";                                                                            // 제조 날짜 표기 해야함
            string Serialnumber = "";                                                                    // Serialnumber 표기 해야함
            string productname = "";                                                                     // 제품명 이름 표기 해야함
            string realname = "";                                                                        // 제조사 이름 표기 해야함
            string[] temporaryname = new string[3];



            string name = (imformation2[16] + imformation2[17] + imformation2[18] + imformation2[19]).Remove(0, 1);

            for (int i = 0; name.Length > 0; i++)
            {
                temporaryname[i] = name.Substring(0, 5);
                name = name.Remove(0, 5);
                realname = realname + (char)(Convert.ToInt32(temporaryname[i], 2) + 64);
            }

            for (int j = 22; j >= 20; j -= 2)
                productname = productname + imformation16[j] + imformation16[j + 1];

            for (int k = 30; k >= 24; k -= 2)
                Serialnumber = Serialnumber + imformation16[k] + imformation16[k + 1];

            date = date + (1990 + (imformation10[34] * 16 + imformation10[35])) + "/" + imformation10[33];

            if (imformation10[39] == 4)
                version = "EDID Version 1.4";
            else if (imformation10[39] == 3)
                version = "EDID Version 1.3";
            else
                version = "unKnown";
            //////////표기

            Header = "Header Information\n";
            Header += "Manufacturer : " + realname + "\n";
            Header += "Product Name : " + productname + "\n";
            Header += "Serialnumber : " + Serialnumber + "\n";
            Header += "Date of Manufacture : " + date + "\n";
            Header += "version : " + version + "\n";
            return Header;
            //////////
        }

        public string BasicDisplayParameters()
        {
            string basic = "Basic_display_parameters\n";

            string[] Display_parameters = new string[3];/* 0 : Standard sRGB colour space 1 : Preferred timing mode specified in descriptor block 1 
                                                           2 : Continuous timings with GTF or CVT*/
            string Display_type = "RGB 4:4:4";          // Display type
            string DPMS = "";                           // DPMS type
            string input_type = "";                     // 입력 유형 Digital or Analog input
            string bit_depth = "";                      // bit per color(Digital)
            string Video_interface = "";                // Video_interface(Digital)
            string onoff = "";                          // Video white and sync levels, relative to blank(Analog)
            string[] analog = new string[5];            /* 0 : Blank-to-black setup 1 : Separate sync supported 2 : Composite sync(on HSync) supported
                                                           3 : Sync on green supported 4 : VSync pulse must be serrated when composite or sync-on - green is used.(Analog)*/
            string M_H_imagesize = "";                  // 표현 가능 image size (가로)
            string M_V_imagesize = "";                  // 표현 가능 image size (세로)
            string gamma = "";                          // gamma 전송 특성
            if (imformation2[40].Substring(0, 1) == "1")   ////디지털 일때
            {
                input_type = "Digital input";
                switch (imformation10[40] - 8)
                {
                    case 0:
                        bit_depth = "undefined";
                        break;
                    case 7:
                        bit_depth = "reserved";
                        break;
                    default:
                        bit_depth = (4 + (2 * (imformation10[40] - 8))).ToString() + " bits per color ";
                        break;
                }

                switch (imformation10[41])
                {
                    case 0:
                        Video_interface = "undefined";
                        break;
                    case 2:
                        Video_interface = "HDMIa";
                        break;
                    case 3:
                        Video_interface = "HDMIb";
                        break;
                    case 4:
                        Video_interface = "MDDI";
                        break;
                    case 5:
                        Video_interface = "DisplayPort";
                        break;
                }
                if (imformation2[49].Substring(0, 1) == "1")
                {
                    Display_type = Display_type + "+ YCrCb 4:4:4";
                    if (imformation2[48].Substring(3, 1) == "1")
                        Display_type = Display_type + "+ YCrCb 4:2:2";
                }
                else
                {
                    if (imformation2[48].Substring(3, 1) == "1")
                        Display_type = Display_type + "+ YCrCb 4:2:2";
                }
            }
            else if (imformation2[40].Substring(0, 1) == "0")  ///아날로그 일때 
            {
                input_type = "Analog input";
                switch (Convert.ToInt32(imformation2[40].Remove(3, 1), 2))
                {
                    case 0:
                        onoff = "+0.7V/-0.3V";
                        break;
                    case 1:
                        onoff = "+0.714V/-0.286V";
                        break;
                    case 2:
                        onoff = "+1.0V/-0.4V";
                        break;
                    case 3:
                        onoff = "+0.7V/-0V";
                        break;
                }
                if (imformation2[40].Substring(3, 1) == "1") analog[0] = "Blank-to-black setup (pedestal) expected";
                else analog[0] = "Blank-to-black none setup (pedestal) expected";
                if (imformation2[41].Substring(0, 1) == "1") analog[1] = "Separate sync supported";
                else analog[1] = "Separate sync none supported";
                if (imformation2[40].Substring(1, 1) == "1") analog[2] = "Composite sync (on HSync) supported";
                else analog[2] = "Composite sync (on HSync) none supported";
                if (imformation2[40].Substring(2, 1) == "1") analog[3] = "Sync on green supported";
                else analog[3] = "Sync on green none supported";
                if (imformation2[40].Substring(3, 1) == "1") analog[4] = "VSync pulse must be serrated when composite or sync-on-green is used.";
                else analog[4] = "VSync pulse must be not serrated when composite or sync-on-green is used.";

                switch (Convert.ToInt32(imformation2[48].Substring(3, 1), 2) + Convert.ToInt32(imformation2[49].Substring(0, 1), 2))
                {
                    case 0:
                        Display_type = "monochrome or grayscale";
                        break;
                    case 1:
                        Display_type = "RGB color";
                        if (imformation2[48].Substring(3, 1) == "1")
                            Display_type = "none" + Display_type;
                        break;
                    case 2:
                        Display_type = "undefined";
                        break;

                }

            }

            M_H_imagesize = (imformation10[42] * 16 + imformation10[43]).ToString();
            M_V_imagesize = (imformation10[44] * 16 + imformation10[45]).ToString();

            gamma = ((float)(imformation10[46] * 16 + imformation10[47] + 100) / 100).ToString();

            if (imformation2[48].Substring(3, 1) == "1")
                DPMS = "DPMS standby supported";
            else if (imformation2[48].Substring(2, 1) == "1")
                DPMS = "DPMS suspend supported";
            else if (imformation2[48].Substring(1, 1) == "1")
                DPMS = "DPMS active-off supported";
            else
                DPMS = "none DPMS";

            for (int i = 1; i <= 3; i++)
            {
                if (imformation2[49].Substring(i, 1) == "1")
                    Display_parameters[i - 1] = "yes";
                else
                    Display_parameters[i - 1] = "no";
            }

            ///////////////////////////////////
            basic = "\n" + input_type;
            if (input_type == "Digital input")
            {
                basic += "bit depth : " + bit_depth + "\n";
                basic += "Video_interface : " + Video_interface + "\n";
            }
            else if (input_type == "Analog input")
            {
                basic += "onoff  : " + onoff + "\n";
                for (int i = 0; i < analog.Length; i++)
                    basic += analog[i] + "\n";
            }
            basic += Display_type + "\n";
            basic += "Horizontal screen size : " + M_H_imagesize + "\n";
            basic += "Vertical screen size : " + M_V_imagesize + "\n";
            basic += "Gamma : " + gamma + "\n";
            basic += DPMS + "\n";
            basic += "Standard sRGB colour space : " + Display_parameters[0] + "\n";
            basic += "Preferred timing mode specified in descriptor block 1  : " + Display_parameters[1] + "\n";
            basic += "Continuous timings with GTF or CVT : " + Display_parameters[2] + "\n";
            return basic;
            ///////////////////////////////////
            ///
        }

        public string ChromaticityCoordinates()
        {
            string gamma = "\nChromaticity_coordinates\n";
            float[] Red_xy = new float[2];
            float[] Green_xy = new float[2];
            float[] Blue_xy = new float[2];
            float[] White_xy = new float[2];
            for (int i = 0; i < 2; i++)
            {
                Red_xy[i] =  ( ((float)Convert.ToInt32(imformation2[50].Substring(i * 2, 2), 2) / 1024) +
                    ((float)Convert.ToInt32(imformation2[54 + (i * 2)] + imformation2[55 + (i * 2)], 2) / 256));

                Green_xy[i] = ((float)Convert.ToInt32(imformation2[51].Substring(i * 2, 2), 2) / 1024) +
                    ((float)Convert.ToInt32(imformation2[58 + (i * 2)] + imformation2[59 + (i * 2)], 2) / 256);

                Blue_xy[i] = ((float)Convert.ToInt32(imformation2[52].Substring(i * 2, 2), 2) / 1024) +
                    ((float)Convert.ToInt32(imformation2[62 + (i * 2)] + imformation2[63 + (i * 2)], 2) / 256);

                White_xy[i] = ((float)Convert.ToInt32(imformation2[53].Substring(i * 2, 2), 2) / 1024) +
                    ((float)Convert.ToInt32(imformation2[66 + (i * 2)] + imformation2[67 + (i * 2)], 2) / 256);

            }

            //////////////////////

            gamma += "Red x :" + Red_xy[0] + "\n";
            gamma += "Red y :" + Red_xy[1] + "\n";
            gamma += "Green x :" + Green_xy[0] + "\n";
            gamma += "Green y :" + Green_xy[1] + "\n";
            gamma += "Blue x :" + Blue_xy[0] + "\n";
            gamma += "Blue y :" + Blue_xy[1] + "\n";
            gamma += "White x :" + White_xy[0] + "\n";
            gamma += "White y :" + White_xy[1] + "\n";
            return gamma;
            //////////////////////
        }

        public string EstablishedTimingBitmap()
        {
            string TimingBitamp = "\nEstablished_timing_bitmap\n";
            string sumstring = imformation2[70] + imformation2[71] + imformation2[72] + imformation2[73] + imformation2[74].Substring(0, 1);
            if (Convert.ToInt32(imformation2[74].Substring(1, 3), 2) + imformation10[75] > 0)
                TimingBitamp = "Other manufacturer-specific display modes";
            for (int i = 0; i < sumstring.Length; i++)
            {
                if (sumstring.Substring((sumstring.Length - 1) - i, 1) == "1")
                {
                    if (i == 0) TimingBitamp += "1152x870 ";
                    else if (i == 1) TimingBitamp += "1280x1024 ";
                    else if (i <= 5) TimingBitamp += "1024x768 ";
                    else if (i == 6) TimingBitamp += "832x624 ";
                    else if (i <= 10) TimingBitamp += "800x600 ";
                    else if (i <= 14) TimingBitamp += "640x480 ";
                    else if (i <= 16) TimingBitamp += "720x400 ";

                    if ((i == 4) || (i == 9) || (i == 14)) TimingBitamp = TimingBitamp + "@ 60Hz\n";
                    else if ((i == 0) || (i == 1) || (i == 2) || (i == 6) || (i == 7) || (i == 11)) TimingBitamp = TimingBitamp + "@ 75Hz\n";
                    else if ((i == 3) || (i == 16)) TimingBitamp = TimingBitamp + "@ 70Hz\n";
                    else if (i == 5) TimingBitamp = TimingBitamp + "@ 87Hz\n";
                    else if ((i == 8) || (i == 12)) TimingBitamp = TimingBitamp + "@ 72Hz\n";
                    else if (i == 13) TimingBitamp = TimingBitamp + "@ 67Hz\n";
                    else if (i == 15) TimingBitamp = TimingBitamp + "@ 88Hz\n";
                }

            }
            if (TimingBitamp == "")
                TimingBitamp = "none timing_bitmap";
            ////////////////////////////
            return TimingBitamp;
            ////////////////////////////
        }

        public string StandardDisplayModes()
        {
            string Stand = "\nStandard_Display_Modes\n";
            string[] Modes = new string[8];
            int[] X_resoloution = new int[8];
            int[] Y_resoloution = new int[8];
            int[] VerticalFrequency = new int[8];
            for (int i = 0; i < 8; i++)
            {
                if (imformation16[76 + (i * 4)].ToString() + imformation16[77 + (i * 4)].ToString() != "01")
                {
                    X_resoloution[i] = (imformation10[76 + (i * 2)] * 16 + imformation10[77 + (i * 2)] + 31) * 8;
                    switch (imformation2[78 + (i * 4)].Substring(0, 2))
                    {
                        case "00":
                            Y_resoloution[i] = (X_resoloution[i] / 16) * 10;
                            Modes[i] = "16:10";
                            break;
                        case "01":
                            Y_resoloution[i] = (X_resoloution[i] / 4) * 3;
                            Modes[i] = "4:3";
                            break;
                        case "10":
                            Y_resoloution[i] = (X_resoloution[i] / 5) * 4;
                            Modes[i] = "5:4";
                            break;
                        case "11":
                            Y_resoloution[i] = (X_resoloution[i] / 16) * 9;
                            Modes[i] = "16:9";
                            break;

                    }

                    VerticalFrequency[i] = Convert.ToInt32(imformation2[78 + (i * 4)].Substring(2, 2), 2) * 16 +
                        imformation10[79 + (i * 4)] + 60;


                    ///////////////////
                    Stand += "X_resoloution : " + X_resoloution[i] + "\n";
                    Stand += "Y_resoloution : " + Y_resoloution[i] + "\n";
                    Stand += "Vertical_frequency : " + VerticalFrequency[i] + "\n";
                    Stand += "modes : " + Modes[i] + "\n";
                    ////////////////////
                }
            }
            return Stand;
            
        }

        public string Description()
        {
            string FinallyDescription = "";
            string[,] Description = new string[20, 20];
            int p = 0;
            int flag = 0;
            int jumpflag = 0;
            int jumpn = 0;
            int jumpi = 0;
            for (int i = 0; i < ((imformation16.Length - 112) / 36); i++)
            {

                if (108 + (i * Constants.m) + p > imformation16.Length)
                    break;
                if (108 + (i * Constants.m) == 252)
                {
                    flag = 1;
                    p = p + 4;
                }
                if ((108 + (i * Constants.m) - 254 + p) % 256 == 0)
                    p = p + 2;
                if (flag == 1 && (108 + (i * Constants.m) + p) % 256 == 0 && imformation10[109 + (i * Constants.m) + p] == 2)
                {
                    p = p + (imformation10[112 + (i * Constants.m) + p] * 16 + imformation10[113 + (i * Constants.m) + p]) * 2;
                    jumpflag = 1;
                    jumpn = ((108 + (i * Constants.m) + p) / 256) + 1;
                }
                if (imformation16[114 + (i * Constants.m) + p] == 'F' && (imformation10[108 + (i * Constants.m) + p] + imformation10[109 + (i * Constants.m) + p]
                        + imformation10[110 + (i * Constants.m) + p] + imformation10[111 + (i * Constants.m) + p] + imformation10[112 + (i * Constants.m) + p] + imformation10[113 + (i * Constants.m) + p]) == 0) /// 150 144~149
                {
                    if (imformation10[115 + (i * Constants.m) + p] >= 12 && imformation10[115 + (i * Constants.m) + p] != 13) ////151
                    {

                        for (int j = 0; j <= 13; j++)
                        {
                            Description[i, 0] += Convert.ToChar(imformation10[118 + (j * 2) + (i * Constants.m) + p] * 16 + imformation10[119 + (j * 2) + (i * Constants.m) + p]);//152 153
                        }

                        if (imformation16[115 + (i * Constants.m) + p] == 'F')////151
                            Description[i, 0] = "Display serial number : " + Description[i, 0];
                        else if (imformation16[115 + (i * Constants.m) + p] == 'E')
                            Description[i, 0] = "Unspecified text : " + Description[i, 0];
                        else if (imformation16[115 + (i * Constants.m) + p] == 'C')
                        {
                            try
                            {
                                ChangeEdid.des++;
                                ChangeEdid.de[ChangeEdid.des - 1] = 108 + (i * Constants.m) + p;
                                ChangeEdid.dess += Description[i, 0] + " ";
                            }
                            catch { }
                            Description[i, 0] = "Display name : " + Description[i, 0];
                        }
                    }
                    else if (imformation16[115 + (i * Constants.m) + p] == 'D')                                             //////////////////////////////EDID_Display_Range_Limits start 151
                    {
                        Description[i, 0] = "EDID_Display_Range_Limits";
                        if (imformation2[117 + (i * Constants.m) + p].Substring(0, 2) == "00")
                            Description[i, 1] = "Horizontal rate offsets : none";
                        else if (imformation2[117 + (i * Constants.m) + p].Substring(0, 2) == "10")   ///////153
                            Description[i, 1] = "Horizontal rate offsets : +255 kHz for max. rate";
                        else if (imformation2[117 + (i * Constants.m) + p].Substring(0, 2) == "11") //////153
                            Description[i, 1] = "Horizontal rate offsets : +255 kHz for max and min. rate";

                        Description[i, 2] = "Minimum vertical line rate : " + (imformation10[118 + (i * Constants.m) + p] * 16 + imformation10[119 + (i * Constants.m) + p]).ToString() + "Hz";  ///154 155
                        Description[i, 3] = "Maximum vertical line rate : " + (imformation10[120 + (i * Constants.m) + p] * 16 + imformation10[121 + (i * Constants.m) + p]).ToString() + "Hz";  ////156 157
                        Description[i, 4] = "Minimum horizontal line rate : " + (imformation10[122 + (i * Constants.m) + p] * 16 + imformation10[123 + (i * Constants.m) + p]).ToString() + "Hz"; ////158 159
                        Description[i, 5] = "Maximum horizontall line rate : " + (imformation10[124 + (i * Constants.m) + p] * 16 + imformation10[125 + (i * Constants.m) + p]).ToString() + "Hz"; ///160 161
                        if (imformation16[129 + (i * Constants.m)] == '4') /////165
                            Description[i, 6] = "Maximum pixel clock rate : " + ((float)((imformation10[126 + (i * Constants.m) + p] * 16 + imformation10[127 + (i * Constants.m) + p]) * 10) +    ///////162 163 168 169 
                                                         (float)Convert.ToInt32(imformation2[132 + (i * Constants.m) + p] + imformation2[133 + (i * Constants.m) + p].Substring(0, 2), 2) * 0.25).ToString() + "MHz";
                        else
                            Description[i, 6] = "Maximum pixel clock rate : " + ((imformation10[126 + (i * Constants.m) + p] * 16 + imformation10[127 + (i * Constants.m) + p]) * 10).ToString() + "MHz";
                        if (imformation16[129 + (i * Constants.m) + p] == '0')
                            Description[i, 7] = "Extended timing information type : Default GTF";
                        else if (imformation16[129 + (i * Constants.m) + p] == '1')
                            Description[i, 7] = "Extended timing information type : No timing information.";
                        else if (imformation16[129 + (i * Constants.m) + p] == '2')                                 /////////////////////////With GTF secondary curve start
                        {
                            Description[i, 7] = "Extended timing information type : Secondary GTF supported, parameters as follows.";
                            Description[i, 8] = "start frequency : " + ((imformation10[132 + (i * Constants.m) + p] * 10 + imformation10[133 + (i * Constants.m) + p]) * 2).ToString() + "KHz";///////168   169
                            Description[i, 9] = "GTF C value : " + ((float)(imformation10[134 + (i * Constants.m) + p] * 10 + imformation10[135 + (i * Constants.m) + p]) / 2).ToString();     ///////171
                            Description[i, 10] = "GTF M value : " + (Convert.ToInt32(imformation2[138 + (i * Constants.m) + p] + imformation2[139 + (i * Constants.m) + p]                     ///////174~5 172 ~3
                                + imformation2[136 + (i * Constants.m) + p] + imformation2[137 + (i * Constants.m) + p], 2)).ToString();
                            Description[i, 11] = "GTF K value : " + (imformation10[140 + (i * Constants.m) + p] * 10 + imformation10[141 + (i * Constants.m) + p]).ToString();                   ////////176~7
                            Description[i, 12] = "GTF J value : " + ((float)(imformation10[142 + (i * Constants.m) + p] * 10 + imformation10[143 + (i * Constants.m) + p]) / 2).ToString();      ////////178~9
                        }                                                                             //////////////////////////////////With GTF secondary curve end
                        else if (imformation16[129 + (i * Constants.m)] == '4')                                ////////////////////With CVT support start
                        {
                            Description[i, 7] = "Extended timing information type : CVT.";
                            Description[i, 8] = "CVT major version" + imformation10[130 + (i * Constants.m) + p].ToString(); /////////////166
                            Description[i, 9] = "CVT minor version" + imformation10[131 + (i * Constants.m) + p].ToString();/////////167
                            Description[i, 10] = "Maximum active pixels per line : " + Convert.ToInt32(imformation2[133 + (i * Constants.m) + p].Substring(2, 2) //////169
                                + imformation2[170 + (i * Constants.m)] + imformation2[135 + (i * Constants.m) + p], 2).ToString();////171
                            Description[i, 11] = "Aspect ratio bitmap : ";
                            for (int k = 0; k < (imformation2[136 + (i * Constants.m) + p] + imformation2[137 + (i * Constants.m) + p].Substring(0, 1)).Length; k++)//////172 173
                            {
                                if (((imformation2[136 + (i * Constants.m) + p] + imformation2[137 + (i * Constants.m) + p].Substring(0, 1))).Substring(k, 1) == "1")///////172 173
                                {
                                    switch (k)
                                    {
                                        case 0:
                                            Description[i, 11] += "4:3\n";
                                            break;
                                        case 1:
                                            Description[i, 11] += "16:9\n";
                                            break;
                                        case 2:
                                            Description[i, 11] += "16:10\n";
                                            break;
                                        case 3:
                                            Description[i, 11] += "5:4\n";
                                            break;
                                        case 4:
                                            Description[i, 11] += "15:9\n";
                                            break;
                                    }
                                }
                            }
                            Description[i, 12] = "Aspect ratio preference";
                            switch ((Convert.ToInt32(imformation2[138 + (i * Constants.m) + p].Substring(0, 3), 2)))//////174
                            {
                                case 0:
                                    Description[i, 12] = "4:3";
                                    break;
                                case 1:
                                    Description[i, 12] = "16:9";
                                    break;
                                case 2:
                                    Description[i, 12] = "16:10";
                                    break;
                                case 3:
                                    Description[i, 12] = "5:4";
                                    break;
                                case 4:
                                    Description[i, 12] = "15:9";
                                    break;
                            }
                            if (imformation10[138 + (i * Constants.m) + p] % 2 == 1)     ////174
                                Description[i, 13] = "CVT-RB reduced blanking";
                            else
                                Description[i, 13] = "CVT-RB none reduced blanking";
                            if (imformation10[139 + (i * Constants.m) + p] >= 8)////175
                                Description[i, 14] = "CVT standard blanking";
                            else
                                Description[i, 14] = "CVT standard blanking = none";
                            Description[i, 15] = "Horizontal shrink";
                            Description[i, 16] = "Horizontal stretch";
                            Description[i, 17] = "Vertical shrink";
                            Description[i, 18] = "Vertical stretch";
                            for (int k = 0; k < 4; k++)
                            {
                                if (imformation2[140 + (i * Constants.m) + p].Substring(k, 1) == "1")///176
                                    Description[i, 15 + k] = "none" + Description[i, 15 + k];
                            }
                            Description[i, 20] = (imformation10[142 + (i * Constants.m) + p] * 16 + imformation10[143 + (i * Constants.m) + p]).ToString();////178 179

                        }                                                                            /////////////////////////////With CVT support end
                    }                                                                               ///////////////////////////////EDID_Display_Range_Limits end
                    else if (imformation16[114 + (i * Constants.m)] == 'B')           ///150                         ///////////////////////////////Additional white point descriptor start
                    {
                        Description[i, 0] = "Additional white point descriptor";
                        Description[i, 1] = "white point index number : " + (imformation10[118 + (i * Constants.m) + p] * 16 + imformation10[119 + (i * Constants.m) + p]).ToString();///154 155
                        Description[i, 2] = "White point x : " + (((float)Convert.ToInt32(imformation2[122 + (i * Constants.m) + p] +//////158
                            imformation2[123 + (i * Constants.m) + p] + imformation2[121 + (i * Constants.m) + p].Substring(0, 2), 2)) / 1024).ToString();///159 157
                        Description[i, 3] = "White point y : " + (((float)Convert.ToInt32(imformation2[124 + (i * Constants.m) + p] + ///160   
                            imformation2[125 + (i * Constants.m) + p] + imformation2[122 + (i * Constants.m) + p].Substring(2, 2), 2)) / 1024).ToString();//161 157
                        Description[i, 4] = "gamma : " + ((float)(imformation10[126 + (i * Constants.m) + p] * 16 + imformation10[127 + (i * Constants.m) + p] + 100) / 100).ToString();///162 163
                    }                                                                                 ///////////////////////////////Additional white point descriptor end
                    else if (imformation16[114 + (i * Constants.m) + p] == '9')//150
                    {
                        Description[i, 0] = "Color management data descriptor";
                        Description[i, 1] = "Version : 03";
                        Description[i, 2] = "Red a3";
                        Description[i, 3] = "Red a2";
                        Description[i, 4] = "Green a3";
                        Description[i, 5] = "Greeen a2";
                        Description[i, 6] = "Blue a3";
                        Description[i, 7] = "Blue a2";
                        for (int m = 0; m < 8; m++)
                        {
                            Description[i, 2 + m] += imformation16[120 + (m * 4) + (i * Constants.m) + p].ToString() + imformation16[121 + (m * 4) + (i * Constants.m) + p].ToString() //156~7
                                + imformation16[118 + (m * 4) + (i * Constants.m) + p].ToString() + imformation16[119 + (m * 4) + (i * Constants.m) + p].ToString();//154~5
                        }


                    }
                    else if (imformation10[114 + (i * Constants.m) + p] == '8')                                   /////////////////////////////////EDID CVT 3-byte timing codes descriptor start
                    {
                        Description[i, 0] = "Color management data descriptor";
                        Description[i, 1] = "Verson : 01";
                        for (int l = 0; l < 4; l++)
                        {
                            Description[i, 2 + (l * 4)] = "Addressable lines" + Convert.ToInt32(imformation2[122 + (i * Constants.m) + p] + imformation2[120 + (i * Constants.m) + p] + imformation2[121 + (i * Constants.m) + p]).ToString();///158 156 157
                            Description[i, 3 + (l * 4)] = "Aspect ratio : ";
                            if (imformation10[123 + (i * Constants.m) + p] == 0) Description[i, 3 + (l * 4)] += "4 : 3";/////////159
                            else if (imformation10[123 + (i * Constants.m) + p] == 4) Description[i, 3 + (l * 4)] += "16 : 9";
                            else if (imformation10[123 + (i * Constants.m) + p] == 8) Description[i, 3 + (l * 4)] += "16 : 10";
                            else if (imformation10[123 + (i * Constants.m) + p] == 12) Description[i, 3 + (l * 4)] += "15 : 9";////////159
                            Description[i, 4 + (l * 4)] = "Preferred vertical rate";
                            if (imformation2[124 + (i * Constants.m) + p].Substring(1, 2) == "00") Description[i, 4 + (l * 4)] += "50Hz"; //////160
                            else if (imformation2[124 + (i * Constants.m) + p].Substring(1, 2) == "01") Description[i, 4 + (l * 4)] += "60Hz";
                            else if (imformation2[124 + (i * Constants.m) + p].Substring(1, 2) == "10") Description[i, 4 + (l * 4)] += "75Hz";
                            else if (imformation2[124 + (i * Constants.m) + p].Substring(1, 2) == "00") Description[i, 4 + (l * 4)] += "85Hz";//////160
                            Description[i, 5 + (l * 4)] = "Vertical rate bitmap";
                            string ex = imformation2[124 + (i * Constants.m) + p].Substring(3, 1) + imformation2[125 + (i * Constants.m) + p];
                            for (int k = 0; k < ex.Length; k++)//160 161
                            {
                                if (ex.Substring(k, 1) == "1")//160 161
                                {
                                    switch (k)
                                    {
                                        case 0:
                                            Description[i, 5 + (l * 4)] += "50Hz\n";
                                            break;
                                        case 1:
                                            Description[i, 5 + (l * 4)] += "60Hz\n";
                                            break;
                                        case 2:
                                            Description[i, 5 + (l * 4)] += "75Hz\n";
                                            break;
                                        case 3:
                                            Description[i, 5 + (l * 4)] += "85Hz\n";
                                            break;
                                        case 4:
                                            Description[i, 5 + (l * 4)] += "60Hz reduced blanking\n";
                                            break;
                                    }
                                }
                            }
                        }


                    }                                                                                          //////////////////////////////EDID CVT 3-byte timing codes descriptor end
                    else if (imformation10[114 + (i * Constants.m)] == '7')///150
                    {
                        string sumstring2 = "";
                        Description[i, 0] = "Additional standard timings";
                        Description[i, 1] = "version : 10";
                        for (int k = 0; k < 12; k++)
                            sumstring2 += imformation2[120 + (i * Constants.m) + k + p];//// 156

                        for (int n = 0; n < sumstring2.Length; n++)
                        {
                            if (sumstring2.Substring(n, 1) == "1")
                            {
                                if (n == 0) Description[i, 2] = "1152 X 864";
                                else if (n == 1) Description[i, 2] = "1024 X 768";
                                else if (n == 2) Description[i, 2] = "800 X 600";
                                else if (n == 3) Description[i, 2] = "848 X 480";
                                else if (n == 4) Description[i, 2] = "640 X 480";
                                else if (n == 5) Description[i, 2] = "720 X 400";
                                else if (n == 6) Description[i, 2] = "640 X 400";
                                else if (n == 7) Description[i, 2] = "640 X 350";
                                else if (n <= 11) Description[i, 2] = "1280 X 768";
                                else if (n <= 13) Description[i, 2] = "1280 X 960";
                                else if (n <= 15) Description[i, 2] = "1280 X 1024";
                                else if (n == 16) Description[i, 2] = "1360 X 768";
                                else if (n == 17) Description[i, 2] = "1280 X 768";
                                else if (n <= 20) Description[i, 2] = "1440 X 900";
                                else if (n <= 24) Description[i, 2] = "1440 X 1050";
                                else if (n <= 28) Description[i, 2] = "1680 X 1050";
                                else if (n <= 33) Description[i, 2] = "1680 X 1200";
                                else if (n <= 35) Description[i, 2] = "1792 X 1344";
                                else if (n <= 37) Description[i, 2] = "1856 X 1392";
                                else if (n <= 41) Description[i, 2] = "1920 X 1200";
                                else if (n <= 43) Description[i, 2] = "1920 X 1440";

                                if (n <= 7 || n != 4 || n == 11 || n == 13 || n == 15 || n == 20 || n == 24 || n == 28 || n == 33 || n == 42) Description[i, 2] += "85Hz";
                                else if (n == 4 || n == (8) || n == 9 || n == 12 || n == 14 || n == (16) || n == 17 || n == (18) ||
                                            n == (21) || n == 22 || n == (24) || n == 25 || n == (26) || n == (29) || n == 34 || n == 36 || n == (38) || n == 39 || n == 42) Description[i, 2] += "60Hz";
                                else if (n == 30) Description[i, 2] += "65Hz";
                                else Description[i, 2] += "75Hz";
                                if (n == 8 || n == 16 || n == 18 || n == 21 || n == 24 || n == 26 || n == 29 || n == 38) Description[i, 2] += " (CVT - RB)";

                            }
                        }
                    }
                }
                else if (imformation16[114 + (i * Constants.m) + p] == '0' && (imformation10[108 + (i * Constants.m) + p] + imformation10[109 + (i * Constants.m) + p]  ////////150 144~9
                    + imformation10[110 + (i * Constants.m) + p] + imformation10[111 + (i * Constants.m) + p] + imformation10[112 + (i * Constants.m) + p] + imformation10[113 + (i * Constants.m) + p]) == 0)
                {
                    if (jumpflag == 1)
                    {

                        p = p + (jumpn * 256 - 108 + (i * Constants.m) + p);
                        jumpi = 108 + (i * Constants.m) + p;
                    }
                   FinallyDescription +=  "\nManufacturer reserved descriptors.\n" ;
                }
                else if ((imformation10[108 + (i * Constants.m) + p] + imformation10[109 + (i * Constants.m) + p]  ////////150 144~9
                    + imformation10[110 + (i * Constants.m) + p] + imformation10[111 + (i * Constants.m) + p] + imformation10[112 + (i * Constants.m) + p] + imformation10[113 + (i * Constants.m) + p]) != 0)
                {
                    string dtd = "\n";
                    string Pixel_clock = "";
                    int Horizontal_Active = 0;
                    int Horizontal_Blanking = 0;
                    int Vertical_Active = 0;
                    int Vertical_Blanking = 0;
                    int Horizontal_Sync_Offset = 0;
                    int Horizontal_Sync_Pulse = 0;
                    int Vertical_Sync_Offset = 0;
                    int Vertical_Sync_Pulse = 0;
                    int Horizontal_Display_Size = 0;
                    int Vertical_Display_Size = 0;
                    int Horizontal_Border = 0;
                    int Vertical_Border = 0;
                    string interlace = "";
                    string stereo = "";
                    string sync_flag = "";
                    string[] Analog_sync = new string[3];               //0 : Sync type 1 : Serration 2 : Sync on red and blue lines additionally to green
                    string[] Digital_sync = new string[2];              /*composite mode(0 : Serration 1 : Horizontal sync polarity) 
                                                                  separate mode (0 : Vertical sync polarity 1 : Horizontal sync polarity)*/

                    Pixel_clock = ((float)(Convert.ToInt32(imformation2[110 + (i * Constants.m) + p] + imformation2[111 + (i * Constants.m) + p] + imformation2[108 + (i * Constants.m) + p] + imformation2[109 + (i * Constants.m) + p], 2) * 10000)
                        / 1000000).ToString() + "MHz";
                    if (imformation10[110 + (i * Constants.m) + p] + imformation10[111 + (i * Constants.m) + p] + imformation10[108 + (i * Constants.m) + p] + imformation10[109 + (i * Constants.m) + p] == 0)
                        Pixel_clock = "10KHz";

                    Horizontal_Active = (imformation10[112 + (i * Constants.m) + p] * 16 + imformation10[113 + (i * Constants.m) + p]) + imformation10[116 + (i * Constants.m) + p] * 256;
                    Horizontal_Blanking = (imformation10[114 + (i * Constants.m) + p] * 16 + imformation10[115 + (i * Constants.m) + p]) + imformation10[117 + (i * Constants.m) + p] * 256;

                    Vertical_Active = (imformation10[118 + (i * Constants.m) + p] * 16 + imformation10[119 + (i * Constants.m) + p]) + imformation10[122 + (i * Constants.m) + p] * 256;
                    Vertical_Blanking = (imformation10[120 + (i * Constants.m) + p] * 16 + imformation10[121 + (i * Constants.m) + p]) + imformation10[123 + (i * Constants.m) + p] * 256;

                    Horizontal_Sync_Offset = (imformation10[124 + (i * Constants.m) + p] * 16 + imformation10[125 + (i * Constants.m) + p]) + Convert.ToInt32(imformation2[130 + (i * Constants.m) + p].Substring(0, 2), 2) * 256;
                    Horizontal_Sync_Pulse = (imformation10[126 + (i * Constants.m) + p] * 16 + imformation10[127 + (i * Constants.m) + p]) + Convert.ToInt32(imformation2[130 + (i * Constants.m) + p].Substring(2, 2), 2) * 256;

                    Vertical_Sync_Offset = imformation10[128 + (i * Constants.m) + p] + Convert.ToInt32(imformation2[131 + (i * Constants.m) + p].Substring(0, 2), 2) * 16;
                    Vertical_Sync_Pulse = imformation10[129 + (i * Constants.m) + p] + Convert.ToInt32(imformation2[131 + (i * Constants.m) + p].Substring(2, 2), 2) * 16;

                    Horizontal_Display_Size = (imformation10[132 + (i * Constants.m) + p] * 16 + imformation10[133 + (i * Constants.m) + p]) + imformation10[136 + (i * Constants.m) + p] * 256;
                    Vertical_Display_Size = (imformation10[134 + (i * Constants.m) + p] * 16 + imformation10[135 + (i * Constants.m) + p]) + imformation10[137 + (i * Constants.m) + p] * 256;

                    Horizontal_Border = (imformation10[138 + (i * Constants.m) + p] * 16) + imformation10[139 + (i * Constants.m) + p];
                    Vertical_Border = (imformation10[140 + (i * Constants.m) + p] * 16) + imformation10[141 + (i * Constants.m) + p];

                    ////need change DTB
                    try
                    {
                        ChangeEdid.dtd++;
                        ChangeEdid.index[ChangeEdid.dtd - 1] = 108 + (i * Constants.m) + p;
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 0] = Horizontal_Sync_Pulse;
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 1] = Horizontal_Blanking - (Horizontal_Sync_Pulse + Horizontal_Sync_Offset);
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 2] = Horizontal_Active;
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 3] = Horizontal_Sync_Offset;
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 4] = Vertical_Sync_Pulse;
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 5] = Vertical_Blanking - (Vertical_Sync_Pulse + Vertical_Sync_Offset);
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 6] = Vertical_Active;
                        ChangeEdid.resolution[ChangeEdid.dtd - 1, 7] = Vertical_Sync_Offset;
                        ChangeEdid.pi[ChangeEdid.dtd - 1] = Convert.ToInt64(imformation2[110 + (i * Constants.m) + p] + imformation2[111 + (i * Constants.m) + p] + imformation2[108 + (i * Constants.m) + p] + imformation2[109 + (i * Constants.m) + p], 2);
                    }
                    catch { }
                    ///

                    if (imformation10[142 + (i * Constants.m) + p] >= 8)
                        interlace = "interlaced";
                    else
                        interlace = "non-inerlaced";

                    if (Convert.ToInt32(imformation2[142 + (i * Constants.m) + p].Substring(1, 2), 2) == 0)
                        stereo = "X";
                    else if (Convert.ToInt32(imformation2[142 + (i * Constants.m) + p].Substring(1, 2), 2) == 1)
                    {
                        if (imformation10[143 + (i * Constants.m) + p] % 2 == 1)
                            stereo = "field sequential, right during stereo sync";
                        else
                            stereo = "2-way interleaved, right image on even lines";
                    }
                    else if (Convert.ToInt32(imformation2[142 + (i * Constants.m) + p].Substring(1, 2), 2) == 2)
                    {
                        if (imformation10[143 + (i * Constants.m) + p] % 2 == 1)
                            stereo = "field sequential, left during stereo sync";
                        else
                            stereo = "2-way interleaved, left image on even lines";
                    }
                    else if (Convert.ToInt32(imformation2[142 + (i * Constants.m) + p].Substring(1, 2), 2) == 3)
                    {
                        if (imformation10[143 + (i * Constants.m) + p] % 2 == 1)
                            stereo = "4-way interleaved";
                        else
                            stereo = "side-by-side interleaved";
                    }

                    if (imformation10[142 + (i * Constants.m) + p] % 2 == 0)
                    {
                        sync_flag = "analog";
                        if (imformation2[143 + (i * Constants.m) + p].Substring(1, 0) == "1")
                            Analog_sync[0] = "Sync type : bipolar analog composite.";
                        else
                            Analog_sync[0] = "Sync type : analog composite.";
                        if (imformation2[143 + (i * Constants.m) + p].Substring(2, 0) == "1")
                            Analog_sync[1] = "Serration : with serrations (H-sync during V-sync).";
                        else
                            Analog_sync[1] = "Serration : without serrations.";
                        if (imformation2[143 + (i * Constants.m) + p].Substring(2, 0) == "1")
                            Analog_sync[2] = "Sync on red and blue lines additionally to green : sync on all three (RGB) video signals.";
                        else
                            Analog_sync[2] = "Sync on red and blue lines additionally to green : sync on green signal only.";
                    }
                    else
                    {
                        sync_flag = "digital";
                        if (imformation10[143 + (i * Constants.m) + p] >= 8)
                        {
                            if (imformation2[143 + (i * Constants.m) + p].Substring(1, 1) == "1")
                                Digital_sync[0] = "Vertical sync polarity : positive";
                            else
                                Digital_sync[0] = "Vertical sync polarity : negative";
                        }
                        else
                        {
                            if (imformation2[143 + (i * Constants.m) + p].Substring(1, 1) == "1")
                                Digital_sync[0] = "Serration : without serration;";
                            else
                                Digital_sync[0] = "Serration : with serration (H-sync during V-sync)";
                        }

                        if (imformation2[143 + (i * Constants.m) + p].Substring(2, 1) == "1")
                            Digital_sync[1] = "Horizontal sync polarity : positive";
                        else
                            Digital_sync[1] = "Horizontal sync polarity : negative";

                    }
                    dtd += "Detailed Timing Descriptor\n";
                    dtd += "Pixel_clock : " + Pixel_clock + "\n";
                    dtd += "Horizontal_Active : " + Horizontal_Active + "\n";
                    dtd += "Horizontal_Blanking : " + Horizontal_Blanking + "\n";
                    dtd += "Horizontal_Sync_Offset : " + Horizontal_Sync_Offset + "\n";
                    dtd += "Horizontal_Sync_Pulse : " + Horizontal_Sync_Pulse + "\n";
                    dtd += "Horizontal image size : " + Horizontal_Display_Size + "\n";
                    dtd += "Horizontal_Border : " + Horizontal_Border + "\n";
                    dtd += "Vertical_Active : " + Vertical_Active + "\n";
                    dtd += "Vertical_Blanking : " + Vertical_Blanking + "\n";
                    dtd += "Vertical_Sync_Offset : " + Vertical_Sync_Offset + "\n";
                    dtd += "Vertical_Sync_Pulse : " + Vertical_Sync_Pulse + "\n";
                    dtd += "Vertical image size : " + Vertical_Display_Size + "\n";
                    dtd += "Vertical_Border : " + Vertical_Border + "\n";
                    dtd += "interface mode : " + interlace + "\n";
                    dtd += "stereo mod : " + stereo + "\n";
                    dtd += sync_flag + "syns \n";
                    if (sync_flag == "analog")
                        foreach (string j in Analog_sync)
                            dtd += j + "\n";
                    if (sync_flag == "digital")
                        foreach (string j in Digital_sync)
                            dtd += j + "\n";
                    FinallyDescription += dtd;
                }
            }
            for (int i = 10; i < 5; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (Description[i, j] != null)
                        FinallyDescription += Description[i, j] + "\n";
                }
                if(Description[i,0] != null && Description[i,0] != "" )
                    FinallyDescription += "\n";
            }
            return FinallyDescription;
        }

        public string ExtensionDataFormat(int l)
        {
            string ExtensionDataFormat = "";
            string[] Speaker_Allocation_Data_Block = new string[50];
            string[] Vendor_Specific = new string[50];
            string[] Audio_Descriptor = new string[50];
            string[] Video_Data_Blocks = new string[50];
            string DTD = "\n";
            int DBCcount = ((imformation10[4 + (l * Constants.l)] * 16 + imformation10[5 + (l * Constants.l)]) * 2) - 8;
            DTD = "Extension_data_format\n";
            if (imformation2[6 + (l * Constants.l)].Substring(0, 1) == "1") DTD += "display supports underscan\n";
            if (imformation2[6 + (l * Constants.l)].Substring(1, 1) == "1") DTD += "display supports basic audio\n";
            if (imformation2[6 + (l * Constants.l)].Substring(2, 1) == "1") DTD += "display supports YCbCr 4  4  4\n";
            if (imformation2[6 + (l * Constants.l)].Substring(3, 1) == "1") DTD += "display supports YCbCr 4  2  2\n";
            ExtensionDataFormat += DTD;
            for (int i = 0; i < DBCcount; i = i + (Convert.ToInt32(imformation2[8 + (l * Constants.l) + i].Substring(3, 1) + imformation2[9 + (l * Constants.l) + i], 2) + 1) * 2)
            {
                if (imformation2[8 + (l * Constants.l) + i].Substring(0, 3) == "001")
                {
                    for (int j = 0; j < (Convert.ToInt32(imformation2[8 + (l * Constants.l) + i].Substring(3, 1) + imformation2[9 + (l * Constants.l) + i], 2)) / 3; j = j + 1)
                    {
                        Audio_Descriptor[0 + j * 4] = "Audio format code : ";
                        Audio_Descriptor[1 + j * 4] = "Number of channels minus 1 : ";
                        Audio_Descriptor[2 + j * 4] = "Sampling frequencies (kHz) supported : ";
                        Audio_Descriptor[3 + j * 4] = "Bitrate / format dependent : ";
                        int foramtcode = imformation10[10 + (l * Constants.l) + i + (j * 6)] * 2 + Convert.ToInt32(imformation2[11 + (l * Constants.l) + i + (j * 6)].Substring(0, 1));
                        if (foramtcode == 1 || foramtcode == 15) Audio_Descriptor[0 + j * 4] += "reserved";
                        else if (foramtcode == 1) Audio_Descriptor[0 + j * 4] += "Linear pulse-code modulation (LPCM)";
                        else if (foramtcode == 2) Audio_Descriptor[0 + j * 4] += "AC-3";
                        else if (foramtcode == 3) Audio_Descriptor[0 + j * 4] += "MPEG-1";
                        else if (foramtcode == 4) Audio_Descriptor[0 + j * 4] += "MP3";
                        else if (foramtcode == 5) Audio_Descriptor[0 + j * 4] += "MPEG-2";
                        else if (foramtcode == 6) Audio_Descriptor[0 + j * 4] += "AAC";
                        else if (foramtcode == 7) Audio_Descriptor[0 + j * 4] += "DTS";
                        else if (foramtcode == 8) Audio_Descriptor[0 + j * 4] += "ATRAC";
                        else if (foramtcode == 9) Audio_Descriptor[0 + j * 4] += "1-bit audio, Super Audio CD";
                        else if (foramtcode == 10) Audio_Descriptor[0 + j * 4] += "DD+";
                        else if (foramtcode == 11) Audio_Descriptor[0 + j * 4] += "DTS-HD";
                        else if (foramtcode == 12) Audio_Descriptor[0 + j * 4] += "MLP/Dolby TrueHD";
                        else if (foramtcode == 13) Audio_Descriptor[0 + j * 4] += "DST Audio";
                        else if (foramtcode == 14) Audio_Descriptor[0 + j * 4] += "Microsoft WMA Pro";

                        Audio_Descriptor[1 + j * 4] += Convert.ToInt32(imformation2[11 + (l * Constants.l) + i + (j * 3)].Substring(1, 3)).ToString() + " channel";

                        int sample = imformation10[12 + (l * Constants.l) + i + (j * 6)] * 16 + imformation10[13 + (l * Constants.l) + i + (j * 6)];
                        if (sample == 0) Audio_Descriptor[2 + j * 4] += "32KHz";
                        else if (sample == 1) Audio_Descriptor[2 + j * 4] += "44.1KHz";
                        else if (sample == 2) Audio_Descriptor[2 + j * 4] += "48KHz";
                        else if (sample == 3) Audio_Descriptor[2 + j * 4] += "88KHz";
                        else if (sample == 4) Audio_Descriptor[2 + j * 4] += "96KHz";
                        else if (sample == 5) Audio_Descriptor[2 + j * 4] += "176KHz";
                        else if (sample == 6) Audio_Descriptor[2 + j * 4] += "192KHz";
                        else if (sample == 7) Audio_Descriptor[2 + j * 4] += "reserved";

                        if (imformation10[15 + (l * Constants.l) + i + (j * 6)] == 0) Audio_Descriptor[3 + j * 4] += "16-bit depth";
                        else if (imformation10[15 + (l * Constants.l) + i + (j * 6)] == 1) Audio_Descriptor[3 + j * 4] += "20-bit depth";
                        else if (imformation10[15 + (l * Constants.l) + i + (j * 6)] == 1) Audio_Descriptor[3 + j * 4] += "24-bit depth";
                        else Audio_Descriptor[3 + j * 4] += "reserved";


                    }
                    ExtensionDataFormat += "\n";
                    foreach (string s in Audio_Descriptor)
                        if (s != null)
                            ExtensionDataFormat += s + "\n";
                }
                else if (imformation2[8 + (l * Constants.l) + i].Substring(0, 3) == "010")
                {
                    int m = 0;
                    for (int j = 0; j < (Convert.ToInt32(imformation2[8 + (l * Constants.l) + i].Substring(3, 1) + imformation2[9 + (l * Constants.l) + i], 2)); j = j + 1)
                    {
                        m++;
                        if (imformation10[10 + (l * Constants.l) + i + j * 2] > 8)
                            Video_Data_Blocks[0 + j * 2] = "This resolution is native resolution : ";
                        Video_Data_Blocks[0 + j] += (Convert.ToInt32(imformation2[10 + (l * Constants.l) + i + j * 2].Substring(1, 3), 2) * 16 + imformation10[11 + (l * Constants.l) + i + j * 2]).ToString() + " VIC Number ";

                    }
                    foreach (string s in Video_Data_Blocks)
                    {
                        if (s != null)
                            ExtensionDataFormat += s;
                        if (m % 10 == 0)
                            ExtensionDataFormat += "\n";
                    }
                }
                else if (imformation2[8 + (l * Constants.l) + i].Substring(0, 3) == "011")
                {
                    for (int j = 0; j < (Convert.ToInt32(imformation2[8 + (l * Constants.l) + i + j * 1].Substring(3, 1) + imformation2[9 + (l * Constants.l) + i + j * 1], 2) / 13); j = j + 1)
                    {
                        Vendor_Specific[0 + j * 14] = "IEEE Registration Identifier : " + (imformation16[14 + (l * Constants.l) + i + j * 15] + imformation16[15 + (l * Constants.l) + i + j * 15] +
                            imformation16[12 + (l * Constants.l) + i + j * 1] + imformation16[13 + (l * Constants.l) + i + j * 1] + imformation16[10 + (l * Constants.l) + i + j * 1] + imformation16[11 + (l * Constants.l) + i + j * 1]);

                        Vendor_Specific[1 + j * 14] = "Components of Source Physical Address : " + imformation16[16 + (l * Constants.l) + i + j * 15] +
                            imformation16[17 + (l * Constants.l) + i + j * 15] + imformation16[18 + (l * Constants.l) + i + j * 15] + imformation16[19 + (l * Constants.l) + i + j * 15];

                        Vendor_Specific[2 + j * 14] = "A function that needs info from ACP or ISRC packets supported";
                        Vendor_Specific[3 + j * 14] = "16-bit-per-channel deep color (48-bit) supported";
                        Vendor_Specific[4 + j * 14] = "12-bit-per-channel deep color (48-bit) supported";
                        Vendor_Specific[5 + j * 14] = "10-bit-per-channel deep color (48-bit) supported";
                        Vendor_Specific[6 + j * 14] = "4∶4∶4 in deep color modes supported";
                        Vendor_Specific[7 + j * 14] = "DVI Dual Link Operation supported";

                        Vendor_Specific[9 + j * 14] = "Latency fields : present";
                        Vendor_Specific[10 + j * 14] = "Interlaced latency fields. Absent if latency fields are absent : present";

                        for (int k = 0; k < 8; k++)
                        {

                            if (imformation2[20 + (l * Constants.l) + i + j * 15] + imformation2[21 + (l * Constants.l) + i + j * 15].Substring(k, 1) == "0")
                            {
                                if (k == 7) Vendor_Specific[7 + j * 14].Insert(24, "un");
                                else if (k == 0) Vendor_Specific[(k + 2) + j * 14].Insert(52, "un");
                                else if (k <= 3 && k <= 1) Vendor_Specific[(k + 2) + j * 14].Insert(38, "un");
                                else Vendor_Specific[(k + 2) + j * 14].Insert(26, "un");
                            }
                            if (imformation2[24 + (l * Constants.l) + i + j * 15] + imformation2[25 + (l * Constants.l) + i + j * 15].Substring(k, 1) == "0")
                            {
                                if (k == 0)
                                {
                                    Vendor_Specific[9 + j * 14] = Vendor_Specific[9 + j * 14].Remove(17, 7);
                                    Vendor_Specific[9 + j * 14] += "absent";
                                }
                                else if (k == 1)
                                {
                                    Vendor_Specific[10 + j * 14] = Vendor_Specific[10 + j * 14].Remove(65, 7);
                                    Vendor_Specific[10 + j * 14] += "absent";

                                }
                            }
                        }

                        Vendor_Specific[8 + j * 14] = ((imformation10[22 + (l * Constants.l) + i + j * 15] * 16 + imformation10[23 + (l * Constants.l) + i + j * 15]) * 5).ToString() + "MHz";

                        Vendor_Specific[11 + j * 14] = "Video latency : ";
                        Vendor_Specific[12 + j * 14] = "Audio latency (video delay for progressive sources) : ";
                        Vendor_Specific[13 + j * 14] = "Interlaced video latency : ";
                        Vendor_Specific[14 + j * 14] = "Interlaced audio latency (video delay for interlaced sources) : ";

                        Vendor_Specific[11 + j * 14] += ((imformation10[26 + (l * Constants.l) + i + j * 15] * 16 + imformation10[27 + (l * Constants.l) + i + j * 15] - 1) / 2).ToString() + "ms";
                        Vendor_Specific[12 + j * 14] += ((imformation10[26 + (l * Constants.l) + i + j * 15] * 16 + imformation10[27 + (l * Constants.l) + i + j * 15] - 1) / 2).ToString() + "ms";
                        Vendor_Specific[13 + j * 14] += ((imformation10[26 + (l * Constants.l) + i + j * 15] * 16 + imformation10[27 + (l * Constants.l) + i + j * 15] - 1) / 2).ToString() + "ms";
                        Vendor_Specific[14 + j * 14] += ((imformation10[26 + (l * Constants.l) + i + j * 15] * 16 + imformation10[27 + (l * Constants.l) + i + j * 15] - 1) / 2).ToString() + "ms";


                    }
                    foreach (string s in Vendor_Specific)
                        if (s != null)
                            ExtensionDataFormat += s + "\n";

                }
                else if (imformation2[8 + (l * Constants.l) + i].Substring(0, 3) == "100")
                {
                    for (int j = 0; j < (Convert.ToInt32(imformation2[8 + (l * Constants.l) + i + j * 1].Substring(3, 1) + imformation2[9 + (l * Constants.l) + i + j * 1], 2)); j = j + 1)
                    {
                        Speaker_Allocation_Data_Block[0 + j * 2] = "Rear left and right center : present";
                        Speaker_Allocation_Data_Block[1 + j * 2] = "Front left and right center : present";
                        Speaker_Allocation_Data_Block[2 + j * 2] = "Rear center : present";
                        Speaker_Allocation_Data_Block[3 + j * 2] = "Rear left and right : present";
                        Speaker_Allocation_Data_Block[4 + j * 2] = "Front center : present";
                        Speaker_Allocation_Data_Block[5 + j * 2] = "Low-frequency effects (LFE) : present";
                        Speaker_Allocation_Data_Block[6 + j * 2] = "Front left and right : present";


                        for (int k = 1; k < 8; k++)
                        {
                            if ((imformation2[10 + (l * Constants.l) + i + j * 2] + imformation2[11 + (l * Constants.l) + i + j * 2]).Substring(k, 1) == "0")
                                Speaker_Allocation_Data_Block[(k - 1) + j * 2] = Speaker_Allocation_Data_Block[(k - 1) + j * 2].Replace("present", "absent");
                        }
                    }
                    foreach (string s in Speaker_Allocation_Data_Block)
                        if (s != null)
                            ExtensionDataFormat += s + "\n";
                }
                else
                    ExtensionDataFormat += "Data Block Collection reserved\n";
                ExtensionDataFormat += "\n";
            }
            return ExtensionDataFormat;
        }
    }
}
