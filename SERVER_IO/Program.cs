using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SERVER_IO
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddr = IPAddress.Any; 
            IPEndPoint ipep = new IPEndPoint(ipaddr, 8085); 
               
            try
            {
                listenerSocket.Bind(ipep);
                 
                listenerSocket.Listen(5);
                Socket client = listenerSocket.Accept();
                Console.WriteLine("client connected " + "ıp address " + client.RemoteEndPoint.ToString());

                byte[] buff = new byte[128];

                while (true)
                {
                    int numOfRecivedByte = client.Receive(buff);

                    Console.WriteLine("numOfRecived byte " + numOfRecivedByte);
                    Console.WriteLine("data sent from client " + buff);

                    string recivedData = Encoding.ASCII.GetString(buff, 0, numOfRecivedByte);
                    Console.WriteLine("recived data is " + recivedData);

                    if (recivedData == "c") break;

                    client.Send(buff);
                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
           }
            }
        
           
    }
}
