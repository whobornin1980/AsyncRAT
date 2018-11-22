using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Helper
    {
            public static byte[] SB(string s)
            {
                return System.Text.Encoding.Default.GetBytes(s);
            }

            public static string BS(byte[] b)
            {
                return System.Text.Encoding.Default.GetString(b);
            }

            public static string _Size(string Size)
            {
                if ((Size.ToString().Length < 4))
                    return (System.Convert.ToString(Size) + " Bytes");
                string str = string.Empty;
                double num = System.Convert.ToDouble(Size) / 1024;
                if ((num < 1024))
                    str = " KB";
                else
                {
                    num = (num / 1024);
                    if ((num < 1024))
                        str = " MB";
                    else
                    {
                        num = (num / 1024);
                        str = " GB";
                    }
                }
                return (num.ToString(".0") + str);
            }
        }
    }
