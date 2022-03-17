using RIAT.Client;
using RIAT.Server;

namespace serializer
{
    class Program
    {
        static string url = "127.0.0.1";
        static int port = 7000;
        static void Main(string[] args)
        {
            RunServer runServer = new RunServer();
            //Server server = new Server();
            //Console.ReadLine();
            Client client = new Client(url, port);

            client.Ping();

            client.GetInputData();

            client.PostAnswer();

            runServer.server.input.Write();
            Console.ReadLine();

        }
    }
}

