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
using Microsoft.VisualBasic;
using System.Net.Sockets;
using System.Windows.Forms;
using Server;

public class Client
{
    public bool IsConnected = true;
    public ListViewItem L = null;
    public Socket C = null;
    public string IP = null;
    public byte[] Buffer = new byte[102401];
    public MemoryStream MS = null;
    public static event ReadEventHandler Read;

    public  delegate void ReadEventHandler(Client C, byte[] b);

    public Client(Socket CL)
    {
        this.C = CL;
        this.Buffer = new byte[102401];
        this.MS = new System.IO.MemoryStream();
        this.IP = CL.RemoteEndPoint.ToString();
        C.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(BeginReceive), C);

    }

    public async void BeginReceive(IAsyncResult ar)
    {
        if (this.IsConnected == false)
            this.isDisconnected();

        try
        {
            int Received = this.C.EndReceive(ar);
            if (Received > 0)
            {
                await this.MS.WriteAsync(this.Buffer, 0, Received);

                if (Helper.BS(this.MS.ToArray()).Contains(Settings.EOF))
                {
                    Read?.Invoke(this, this.MS.ToArray());
                    await this.MS.FlushAsync();
                    this.MS.Dispose();
                    this.MS = new MemoryStream();
                }
            }
            this.C.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(BeginReceive), this);
        }
        catch (Exception ex)
        {
            this.isDisconnected();
        }
    }

    delegate void _isDisconnected();
    public void isDisconnected()
    {
        IsConnected = false;

        try
        {
            if (Messages.F.InvokeRequired)
            {
                Messages.F.Invoke(new _isDisconnected(isDisconnected));
                return;
            }
            else
            {
                this.L.Remove();
                L = null;
            }
        }
        catch (Exception ex)
        {
        }

        try
        {
            C.Close();
            C.Dispose();
            C = null;
        }
        catch (Exception ex)
        {
        }

        try
        {
            MS.Dispose();
            MS = null;
        }
        catch (Exception ex)
        {
        }

        try
        {
            Buffer = null;
        }
        catch (Exception ex)
        {
        }
    }

    public void EndSend(IAsyncResult ar)
    {
        try
        {
            C.EndSend(ar);
        }
        catch (Exception ex)
        {
        }
    }
}
