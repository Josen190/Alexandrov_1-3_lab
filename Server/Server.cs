using RIAT.serializer;
using RIAT.unit;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RIAT.Server
{
    public class Server
    {

        Serializer serializer = new Serializer();
        NetworkStream stream;
        public bool runServer;
        public Input input;
        public Output answer;

        public Server()
        {
            
        }

        public void Run(bool runServer)
        {
            this.runServer = runServer;
            try
            {
                TcpListener serverSocet = new TcpListener(IPAddress.Any, 7000);
                Console.WriteLine("Start Server");
                serverSocet.Start();
                while (this.runServer)
                {
                    TcpClient clientSocet = serverSocet.AcceptTcpClient();

                    stream = clientSocet.GetStream();
                    var IpClient = ((IPEndPoint)clientSocet.Client.LocalEndPoint).Address;
                    Console.WriteLine(DateTime.Now + " -> подключен клиент -> " + IpClient.Address);

                    byte[] buffer = new byte[512];
                    int length = 0;
                    StringBuilder messeng = new StringBuilder();
                    do
                    {
                        length = stream.Read(buffer, 0, buffer.Length);
                        messeng.Append(Encoding.UTF8.GetString(buffer).Substring(0, length));
                    } while (length == 512);

                    string[] sub = messeng.ToString().Split('&');
                    switch (sub[0])
                    {
                        case "Ping":
                            Console.WriteLine(DateTime.Now + " -> подключен запрос {Ping}");
                            Ping();
                            break;
                        case "PostInputData":
                            Console.WriteLine(DateTime.Now + " -> подключен запрос {PostInputData}");
                            PostInputData();
                            break;
                        case "GetAnswer":
                            Console.WriteLine(DateTime.Now + " -> подключен запрос {GetAnswer}");
                            GetAnswer(sub[1]);
                            break;
                        case "Stop":
                            Console.WriteLine(DateTime.Now + " -> подключен запрос {Stop}");
                            Stop();
                            break;
                    }


                    Console.WriteLine(DateTime.Now + " -> отключен клиент -> " + IpClient.Address);
                    stream.Close();
                    clientSocet.Close();

                }
                serverSocet.Stop();
                Console.WriteLine("Stop Server");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        void Ping()
        {
            string code = "200";
            byte[] buffer = Encoding.UTF8.GetBytes(code);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        void PostInputData()
        {
            input = Input.CreteRandom();
            //input = Input.testInput();
            var json = serializer.serializationJosn(input);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            Console.WriteLine(DateTime.Now + " -> данные отправлены");
        }

        void GetAnswer(string json)
        {
            answer = serializer.deserializeJson<Output>(json);
            Output output = Output.createFrom(input);
            var isTru = output.Equals(answer);

            Console.WriteLine(DateTime.Now + " -> данные получены");

            byte[] buffer = Encoding.UTF8.GetBytes(isTru.ToString());
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            Console.WriteLine(DateTime.Now + $" -> Отыет отпрален -> {isTru}");
        }

        void Stop()
        {
            this.runServer = false;

            var isTru = !runServer;
            byte[] buffer = Encoding.UTF8.GetBytes(isTru.ToString());
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
    }
}
