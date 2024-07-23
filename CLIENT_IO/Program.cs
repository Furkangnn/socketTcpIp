using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CLIENT_IO
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            IPAddress ipaddr = null;
            try
            {
                
                Console.WriteLine("welcom to client socket starter and enter valid ip addres ");
                string strIpAddr = Console.ReadLine();

                Console.WriteLine("enter port addres ");
                string strPort = Console.ReadLine();
                int nPortInput = 0;

               

                if (strIpAddr.StartsWith("<HOST>"))
                {
                    strIpAddr = strIpAddr.Replace("<HOST>", string.Empty);
                    ipaddr = ResolveHostNameToIpAdress(strIpAddr);
                    if (ipaddr == null)
                    {
                        Console.WriteLine("ip address is wrong for provided hostname");
                    }
                }

                else if (!IPAddress.TryParse(strIpAddr, out ipaddr))
                {
                    Console.WriteLine("inavalid ip adress");
                    return;
                }

                if (! int.TryParse(strPort , out nPortInput))
                {
                    Console.WriteLine("inavalid port ");
                    return;
                }
               

                client.Connect(ipaddr, nPortInput);

                Console.WriteLine("connected  to the server enter a message");
                string inputCommand = string.Empty;

                while (true)
                {
                    inputCommand = Console.ReadLine();
                    if (inputCommand == "c") break;

                    byte[] buffSend = Encoding.ASCII.GetBytes(inputCommand);
                    client.Send(buffSend);

                   byte[] buffRecived = new byte[128];
                   int nRecsize =  client.Receive(buffRecived);
                    Console.WriteLine("Data recived is " + Encoding.ASCII.GetString(buffRecived, 0, nRecsize));

                }

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            finally
            {
                if(client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                    }
                    client.Close();
                    client.Dispose();
                }
            }

        }

        public static IPAddress ResolveHostNameToIpAdress(string strHostName)
        {
            IPAddress[] retAddr = null;
            try
            {
                retAddr = Dns.GetHostAddresses(strHostName);
                foreach(IPAddress addr in retAddr)
                {
                    if(addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return addr;
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);  ///in here am gettinm no suct hostname is known ??
            }
            return null;
        }
    }
}
