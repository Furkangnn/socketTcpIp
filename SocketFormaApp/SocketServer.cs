using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketFormaApp
{
    public class SocketServer
    {
        IPAddress mIp;
        int mPort;
        TcpListener mTcpListener;
        List<TcpClient> mClients = new List<TcpClient>();

        public async void StartListeningForIncomingConnection(IPAddress ip = null, int port = 8085)
        {
            if (ip == null)
            {
                ip = IPAddress.Any;
            }

            if (port <= 0)
            {
                port = 8085;
            }
            mIp = ip;
            mPort = port;

            System.Diagnostics.Debug.WriteLine(string.Format("IP Address: {0} - Port: {1}", mIp.ToString(), mPort));
            mTcpListener = new TcpListener(mIp, mPort);
            mTcpListener.Start();

            while (true)
            {
                var returnedByAccept = await mTcpListener.AcceptTcpClientAsync();
                mClients.Add(returnedByAccept);
                System.Diagnostics.Debug.WriteLine("client connected successfully", returnedByAccept.ToString());

                TakeCareOfTcpClient(returnedByAccept);
                Console.WriteLine("helloı");
            }

        }


        public async void TakeCareOfTcpClient(TcpClient client)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);
                
                char[] buff = new char[1024];

                while (true)
                {
                    Debug.WriteLine("ready to read");
                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);
                    Debug.WriteLine("nRet is : ", nRet);


                    if (nRet == 0)
                    {
                        RemoveClients(client);
                        Debug.WriteLine("nRert is zero and socket is disconnectted");
                        break;
                    }

                    string data = new string(buff);

                    Debug.WriteLine("data is ", data);

                    Array.Clear(buff, 0, buff.Length);

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void RemoveClients(TcpClient client)
        {
            if (mClients.Contains(client))
            {
                mClients.Remove(client);
                Debug.WriteLine("removed client", client.Client.LocalEndPoint.ToString());
            }
        }

        public async void sendToAll(string message)
        {
            byte[] buff = Encoding.ASCII.GetBytes(message);

            foreach (TcpClient client in mClients)
            {
                client.GetStream().WriteAsync(buff, 0, buff.Length);
            }
        }

        public void stopServer()
        {
            mTcpListener.Stop();
            foreach(TcpClient client in mClients)
            {
                client.Close();
            }
        }
    }
}