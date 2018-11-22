using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
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

    }
}
