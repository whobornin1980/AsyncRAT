using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Server;

public class Messages
{
    public static Form1 F;
    delegate void _Read(Client C, byte[] b);

    public static void Read(Client C, byte[] b)
    {
        try
        {
            string msg = Helper.BS(b).Replace(Settings.EOF, null);
            string[] A = msg.Split(new string[] { Settings.SPL }, StringSplitOptions.None);
            // Console.WriteLine(BS(b))
            switch (A[0])
            {
                case "INFO":
                    {
                        if (F.InvokeRequired)
                        {
                            F.Invoke(new _Read(Read), new object[] { C, b }); return;
                        }
                        else
                        {
                            C.L = F.LV1.Items.Insert(0, C.IP);
                            C.L.Tag = C;
                            for (int i = 1; i <= A.Length - 1; i++)
                                C.L.SubItems.Add(A[i]);
                            F.LV1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        }
                        return;
                    }
            }
        }
        catch (Exception ex)
        {
        }
    }
}
