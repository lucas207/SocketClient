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
            //var tipoThread = new Thread(new ThreadStart(RunClient));
            //tipoThread.Start();

            AsyncClient.StartClient();



            //var saida = client.GetStream();

            //var escreve = new BinaryWriter(saida);

            //var ler = new BinaryReader(saida);

            //{"id":"Lukeira", "evento":"login" }
            //{"id":"Lukeira", "evento":"logout" }
            //{"id":"006B02CD9E29205A", "evento":"update", "dados":{ "updateAttach":"zika" } }
            //{"id":"006B02CD9E29205E", "evento":"hangup", "dados":{ "updateAttach":"zika" } }
            //{"id":"006B02CD9E29205A", "evento":"transfer-ura", "dados":{ "destino":"777" } }

        }

        public static void RunClient()
        {
            TcpClient client;
            while (true)
            {

                try
                {
                    client = new TcpClient();
                    client.Connect("localhost", 5656);

                    string msgClient = ReadSocket(client, 99999);
                    if (msgClient != null)
                    {
                        Console.WriteLine(msgClient);
                    }
                    else
                    {
                        Send(client, Console.ReadLine());
                    }



                    client.Close();
                }
                catch (Exception ex)
                {
                }
            }
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
