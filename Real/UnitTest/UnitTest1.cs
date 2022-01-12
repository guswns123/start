using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDIDParser;
using Real;
using System.Text;

namespace UnitTest
{
    [TestClass]
    public class Test
    {

        #region
        private string _testEdid = @"00 FF FF FF FF FF FF 00 42 4C 00 50 8A 13 00 00\r
06 17 01 03 0E 20 14 78 6F EE 91 A3 54 4C 99 26\r
0F 50 54 21 08 00 81 80 81 40 81 00 90 40 95 00\r
A9 40 B3 00 D1 00 63 7B 40 00 B0 08 1C 70 00 00\r
00 00 00 00 00 00 00 1E 00 00 00 FD 00 18 FA 05\r
FA FA 00 0A 20 20 20 20 20 20 00 00 00 FC 00 50\r
61 72 61 6C 6C 65 6C 73 20 56 75 0A 00 00 00 10\r
00 50 61 72 61 6C 6C 65 6C 73 0A 0A 0A 0A 00 14";

        private string _testName = "Parallels Vu";

        private int _testDTBCount = 1;

        private int[,] _testresoultion = { { 48, 32, 160, 2560 }, { 3, 6, 46, 1600 } };

        private int _testClock = 1340000;

        sort sort = new sort();

        #endregion 

        [TestMethod]
        public void CheckSumAndSortAndConverter()
        {
            _testEdid = _testEdid.Replace("\\r\r\n", " ");

            CheckSums sums = new CheckSums(_testEdid);

            Assert.AreEqual(sums.SortsStr, _testEdid);
        }

        [TestMethod]
        public void Parsaring()
        {

            EdidParser parser = new EdidParser(_testEdid);

            byte[] _testNameByte = Encoding.Default.GetBytes(_testName);

            sort.Sort(_testEdid);

            char[] _testEdidChar = sort.strc;

            Namechanger namechanger = new Namechanger(_testEdidChar, _testNameByte);

            CheckSums check = new CheckSums(namechanger.edidcn);

            Assert.AreEqual(check.SortsStr, _testEdid);

            Assert.AreEqual(ChangeEdid.dtd, _testDTBCount);

            Assert.AreEqual(ChangeEdid.dess, _testName);
        }

        [TestMethod]
        public void Changing()
        {
            char[] arr16 = new char[25];

            int[] arr10 = new int[25]; 

            int[,] _testresoltions = new int[2, 4];

            int _testClocks = new int();


            #region
            resoution piclock = new resoution(_testClock, "P");
            resoution h_syns = new resoution(_testresoultion[0, 0], "H");
            resoution h_back = new resoution(_testresoultion[0, 2]);
            resoution h_active = new resoution(_testresoultion[0, 3]);
            resoution h_front = new resoution(_testresoultion[0, 1], "H");
            resoution v_syns = new resoution(_testresoultion[1, 0], "V");
            resoution v_back = new resoution(_testresoultion[1, 2]);
            resoution v_active = new resoution(_testresoultion[1, 3]);
            resoution v_front = new resoution(_testresoultion[1, 1], "V");

            arr16[0] = piclock.ls_m;
            arr16[1] = piclock.ls_l;
            arr16[2] = piclock.ms_ms;
            arr16[3] = piclock.ms;
            arr16[4] = h_active.ls_m;
            arr16[5] = h_active.ls_l;
            arr16[6] = h_back.ls_m;
            arr16[7] = h_back.ls_l;
            arr16[8] = h_active.ms;
            arr16[9] = h_back.ms;
            arr16[10] = v_active.ls_m;
            arr16[11] = v_active.ls_l;
            arr16[12] = v_back.ls_m;
            arr16[13] = v_back.ls_l;
            arr16[14] = v_active.ms;
            arr16[15] = v_back.ms;
            arr16[16] = h_front.ls_m;
            arr16[17] = h_front.ls_l;
            arr16[18] = h_syns.ls_m;
            arr16[19] = h_syns.ls_l;
            arr16[20] = v_front.ls_l;
            arr16[21] = v_syns.ls_l;

            for(int c = 0; c < arr16.Length; c++)
            {
                arr10[c] = (int)arr16[c];
                if (arr10[c] <= 57)
                    arr10[c] = arr10[c] - 48;
                else
                    arr10[c] = arr10[c] - 87;
            }

            _testClocks = (arr10[2] * 16 + arr10[3]) * 256 + arr10[0] * 16 + arr10[1];
            _testresoltions[0, 0] = Convert.ToInt32(h_syns.sf, 2) * 256 + arr10[18] * 16 + arr10[19];
            _testresoltions[0, 1] = Convert.ToInt32(h_front.sf, 2) * 256 + arr10[16] * 16 + arr10[17];
            _testresoltions[0, 2] = arr10[9] * 256 + arr10[6] * 16 + arr10[7];
            _testresoltions[0, 3] = arr10[8] * 256 + arr10[4] * 16 + arr10[5];
            _testresoltions[1, 0] = Convert.ToInt32(v_syns.sf, 2) * 16 + arr10[21];
            _testresoltions[1, 1] = Convert.ToInt32(v_front.sf, 2) * 16 + arr10[20];
            _testresoltions[1, 2] = arr10[15] * 256 + arr10[12] * 16 + arr10[13];
            _testresoltions[1, 3] = arr10[14] * 256 + arr10[10] * 16 + arr10[11];


            #endregion

            for (int a = 0; a < 2; a++)
            {
                for(int b = 0; b < 4; b++)
                {
                    Assert.AreEqual(_testresoultion[a, b], _testresoltions[a, b]);
                }
            }

        }

    }
}

    

