namespace RIAT.Server
{
    public class RunServer
    {
        public Server server;
        public RunServer()
        {
            Thread mythread = new Thread(Run);
            mythread.Start();
        }
        private void Run()
        {
            server = new Server();
            server.Run(true);
        }

        public void Stop()
        {
            server.runServer = false;
        }
    }
}
