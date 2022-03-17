using RIAT.serializer;
using RIAT.unit;
using System.Net.Sockets;
using System.Text;

namespace RIAT.Client
{
    public class Client
    {
        Serializer serializer = new Serializer();
        string url = "127.0.0.1";
        int port = 7000;
        NetworkStream stream;
        TcpClient client;
        Input input;
        Output output;

        public Client(string url, int port)
        {
            this.url = url;
            this.port = port;
        }

        public void Conect()
        {
            try
            {
                client = new TcpClient(url, port);
                Console.WriteLine(DateTime.Now + $" -> подключен к серверу -> {url}:{port}");
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                stream.Close();
                client.Close();
                Console.WriteLine(DateTime.Now + $" -> отключение от сервера");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //запросы
        public bool Ping()
        {
            Conect();

            string method = "Ping";
            byte[] buffer = Encoding.ASCII.GetBytes($"{method}&");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + " -> запрос к серверу {Ping}");

            buffer = new byte[256];
            int length = 1;
            StringBuilder josn = new StringBuilder();
            while (length > 0)
            {
                length = stream.Read(buffer, 0, buffer.Length);
                josn.Append(Encoding.UTF8.GetString(buffer).Substring(0, length));
            }

            Close();

            int code = Convert.ToInt32(josn.ToString());
            Console.WriteLine(DateTime.Now + $" -> Отыет: {code == 200}"); ;
            return code == 200;
        }

        public Input GetInputData()
        {
            Conect();

            string method = "PostInputData";
            byte[] buffer = Encoding.ASCII.GetBytes($"{method}&");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + " -> запрос к серверу {PostInputData}");

            buffer = new byte[256];
            int length;
            StringBuilder josn = new StringBuilder();
            do
            {
                length = stream.Read(buffer, 0, buffer.Length);
                josn.Append(Encoding.UTF8.GetString(buffer).Substring(0, length));
            } while (length == 256);

            Close();
            var st = josn.ToString();
            Console.WriteLine(st);
            input = serializer.deserializeJson<Input>(st);
            Console.WriteLine(DateTime.Now + $" -> Отыет: данные получены"); ;
            return input;
        }

        public bool PostAnswer()
        {
            if (input == null) GetInputData();
            output = Output.createFrom(input);

            Conect();

            var json = serializer.serializationJosn(output);
            string method = "GetAnswer";
            byte[] buffer = Encoding.ASCII.GetBytes($"{method}&{json}");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + " -> запрос к серверу {GetAnswer}");

            buffer = new byte[256];
            int length;
            string answer;

            length = stream.Read(buffer, 0, buffer.Length);
            answer = Encoding.UTF8.GetString(buffer).Substring(0, length);

            Close();

            bool isTru = Convert.ToBoolean(answer);
            Console.WriteLine(DateTime.Now + $" -> Отыет: {isTru}"); ;
            return isTru;

        }

        public bool Stop()
        {
            Conect();

            string method = "Stop";
            byte[] buffer = Encoding.ASCII.GetBytes($"{method}&");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + " -> запрос к серверу {Stop}");

            buffer = new byte[256];
            int length;
            string answer;

            length = stream.Read(buffer, 0, buffer.Length);
            answer = Encoding.UTF8.GetString(buffer).Substring(0, length);

            Close();

            bool isTru = Convert.ToBoolean(answer);
            Console.WriteLine(DateTime.Now + $" -> Отыет: {isTru}"); ;
            return isTru;
        }

    }




}
