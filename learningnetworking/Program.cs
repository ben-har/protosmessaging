using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace learningnetworking
{
    public class StartServer
    {
        // we have to make it so it puts its clients in a list 
        // it needs to broadcast the message to all other clints behind the sender 

        public void ServerCode()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            //starts listening for a connectiong on pot 5000
            Console.WriteLine("the server has been started");

            List<TcpClient> clients = new List<TcpClient>();
            //creates a list of clients we can use later
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                //the code stops running until a user has connected to the server 
                Console.WriteLine($"{client} has joined the server ");

                WelcomeMessage(client);
                //sends welcome message to client to check connection
                clients.Add(client);
                //this adds the client to a list of clients
                var thread = new Thread(() => HandleClient(client,clients));
                //creates a thread for each client and then constantly checks for incoming messages
                thread.IsBackground = true;
                thread.Start();


                

            }

        }
       void WelcomeMessage(TcpClient client)
       {
            NetworkStream stream = client.GetStream();

            byte[] message = Encoding.UTF8.GetBytes("welcome to the server user :)");

            stream.Write(message, 0, message.Length);
       }
       void HandleClient(TcpClient client, List<TcpClient> listoclients)
       {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];

            try
            {
                while(true)
                {
                    int messagebytes = stream.Read(buffer, 0, buffer.Length);

                    string message = Encoding.UTF8.GetString(buffer,0, messagebytes);

                    Console.WriteLine($"{client} has written {message}");

                    Broadcast(message,client,listoclients);



                }



            }
            catch (Exception ex)
            {
                Console.Write("error with client");
            }
       }

        void Broadcast(string message, TcpClient sender, List<TcpClient> listoclient)
        {
            byte[] arrmessage = Encoding.UTF8.GetBytes(message);

            foreach(var client in  listoclient)
            {
                if(client != sender)
                {
                    NetworkStream stream = client.GetStream();

                    stream.Write(arrmessage,0,arrmessage.Length);
                }
            }
        
        
        
        
        
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
           StartServer myser = new StartServer();
           myser.ServerCode();
        }
    }
}
