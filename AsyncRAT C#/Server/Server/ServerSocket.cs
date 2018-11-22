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
using System.Net;

public class ServerSocket
{
    public Socket S;

    public void Start(int Port)
    {
        S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, Port);
        S.ReceiveBufferSize = 1024 * 100;
        S.SendBufferSize = 1024 * 100;
        S.ReceiveTimeout = -1;
        S.SendTimeout = -1;
        S.Bind(IpEndPoint);
        S.Listen(999);

        System.Threading.Thread T = new System.Threading.Thread(BegingAccpet); T.Start();
    }

    public void BegingAccpet()
    {
        while (true)
        {
            S.BeginAccept(new AsyncCallback(EndAccept), S);
            System.Threading.Thread.Sleep(1);
        }
    }

    public void EndAccept(IAsyncResult ar)
    {
        try
        {
            Client C = new Client(S.EndAccept(ar));
        }
        catch (Exception)
        {
        }
    }
}
