using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //var tipoThread = new Thread(RunClient);
            //tipoThread.Start();

            AsyncClient.StartClient();



            //var saida = client.GetStream();

            //var escreve = new BinaryWriter(saida);

            //var ler = new BinaryReader(saida);

            // {"id":"Lukeira", "evento":"login" }
            // {"id":"Lukeira", "evento":"logout" }
            // {"id":"006B02CD9E29205A", "evento":"update", "dados":{ "updateAttach":"zika" } }
            // {"id":"006B02CD9E29205E", "evento":"hangup", "dados":{ "updateAttach":"zika" } }
            // {"id":"006B02CD9E29205A", "evento":"transfer-ura", "dados":{ "destino":"777" } }

        }

        public static void RunClient()
        {
            TcpClient client;
            byte[] bytes = new byte[1024];

            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = null;
                foreach (var ip in ipHostInfo.AddressList)
                {
                    //log.Info(ip);
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ip;
                    }
                }
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5656);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    while (true)
                    {
                        string content = Console.ReadLine();
                        // Encode the data string into a byte array.  
                        byte[] msg = Encoding.ASCII.GetBytes(content);

                        // Send the data through the socket.  
                        int bytesSent = sender.Send(msg);

                        // Receive the response from the remote device.  
                        int bytesRec = sender.Receive(bytes);
                        Console.WriteLine("Echoed test = {0}",
                            Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    }

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //Console.ReadKey();
        }

        static void Send(TcpClient client, string resposta)
        {
            NetworkStream netStream = client.GetStream();
            Byte[] enviado = Encoding.ASCII.GetBytes(resposta);
            netStream.Write(enviado, 0, enviado.Length);
            netStream.Flush();
            netStream.Close();
        }

        static string ReadSocket(TcpClient cliente, int BUFFER_SIZE)
        {
            NetworkStream netStream = cliente.GetStream();
            byte[] recebido = new byte[BUFFER_SIZE];
            netStream.Read(recebido, 0, (int)cliente.ReceiveBufferSize);
            string msgClient = Encoding.ASCII.GetString(recebido);
            //msgClient = Util.ReadSocketStringFormat(msgClient);

            //log.DebugFormat("[InteraxaExtension][ClientConnection] Recebido no Socket : {0}", msgClient);
            return (msgClient.Equals("") || msgClient.Equals(' ')) ? null : msgClient;
        }
    }
}
