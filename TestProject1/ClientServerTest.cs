using System;
using System.Collections.Generic;
using NUnit.Framework;
using RIAT.Server;
using RIAT.Client;
using RIAT.unit;
using System.Threading;

namespace TestProject1
{
    public class ClientServerTest
    {
        private string url = "127.0.0.1";
        private int port = 7000;
  

        [Test]
        public void PingTest()
        {
            RunServer run = new RunServer();
            Client client = new Client(url, port);

            var ping = client.Ping();

            Assert.IsNotNull(ping);
            Assert.IsTrue(ping);
            run.Stop();
        }

        [Test]
        public void GetAndPostInputDataTest()
        {
            RunServer run = new RunServer();
            Client client = new Client(url, port);

            var inputClient = client.GetInputData();

            Assert.IsTrue(inputClient.Equals(run.server.input));
            run.Stop();
        }

        [Test]
        public void PostAndGetAnswerTest()
        {
            RunServer run = new RunServer();
            Client client = new Client(url, port);

            var outputClient = client.PostAnswer();

            Assert.IsTrue(outputClient);
            run.Stop();
        }

        [Test]
        public void StopTest()
        {
            RunServer run = new RunServer();
            Client client = new Client(url, port);

            var stop = client.Stop();
            
            Assert.AreEqual(stop, run.server.runServer);
            run.Stop();
        }
    }
}
