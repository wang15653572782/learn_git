
using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.IO;

using System.Net;

using System.Net.Sockets;

using System.Threading;

//服务器端，需要先使用Open函数打开服务器，然后等待客户端连接，然后就可以使用send和get来发送消息
namespace BombPlane.Server
{
    class Server
    {
        static Socket serverSocket;

        static Socket clientSocket;


        public static void Open()

        {

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 3001);

            serverSocket = new Socket(ipep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(ipep);

            serverSocket.Listen(20);

           

        }


        public static void Accept()

        {

           
            clientSocket = serverSocket.Accept();
            Socket s = clientSocket;//客户端信息

        }

        public static string getMessage()

        {

            Socket s = clientSocket;//客户端信息
            
            try

            {


                Byte[] inBuffer = new Byte[1024];

                

                String inBufferStr;

              

                s.Receive(inBuffer, 1024, SocketFlags.None);//如果接收的消息为Null 则阻塞当前循环

                inBufferStr = Encoding.ASCII.GetString(inBuffer);
                return inBufferStr;

               




            }

            catch

            {

                Console.WriteLine("客户端关闭了！");
                return "0 0";
            }

        }
        public static void sendMessage(string text)

        {

            Socket s = clientSocket;//客户端信息
            
            try

            {


                

                Byte[] outBuffer = new Byte[1024];

               
                String outBufferStr;



                outBufferStr = text;

                outBuffer = Encoding.ASCII.GetBytes(outBufferStr);

                s.Send(outBuffer, outBuffer.Length, SocketFlags.None);



            }

            catch

            {

                Console.WriteLine("客户端关闭了！");

            }

        }
    }
}