using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SessionFlats
{
    internal class WebIO
    {
        private static readonly object lockObj = new object();

        //const string statURL = "mcwaimea.tplinkdns.com";
        const string statURL = "192.168.1.122";
        const int statPort = 5950;

        TcpClient VoyWeb;
        NetworkStream VoyStream;

        public WebIO()
        {

            //Uses a remote endpoint to establish a socket connection.
            TcpClient VoyWeb = new();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, statPort);
            VoyWeb.Connect(ipEndPoint);
            VoyStream = VoyWeb.GetStream();

        }

        public dynamic ReadWebIO()
        {
            lock (lockObj)
            {
                Byte[] data = new Byte[2048];
                string responseData = String.Empty;
                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = VoyStream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                return responseData;
            }
        }

        public void WriteWebIO(string voyOut)
        {
            lock (lockObj)
            {
                voyOut += "\r\n";
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(voyOut);
                VoyStream.Write(data, 0, data.Length);
                return;
            }
        }

          public void CloseConnection()
        {
            VoyStream.Close();
            VoyWeb.Close();
        }

        public IPAddress GetLocalIP()
        {
            String strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            return addr[0];
        }
    }
}
