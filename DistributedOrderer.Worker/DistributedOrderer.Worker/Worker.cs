using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DistributedOrderer.Common;

namespace DistributedOrderer.Worker
{
    public class Worker
    {
        public Socket Sender { get; set; }
        public IPEndPoint RemoteEp { get; set; }

        public Worker()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            RemoteEp = new IPEndPoint(ipAddress, 8080);

            Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            AskForJobs();
        }


        private void AskForJobs()
        {
            JobAction receivedAction = null;
            Sender.Connect(RemoteEp);

            do
            {
                var bytes = new byte[100000];
                try
                {
                    Sender.Receive(bytes);
                    receivedAction = (JobAction)SocketHelper.Deserialize(bytes);

                    if (!receivedAction.NoJobsLeft)
                    {
                        Console.WriteLine("I got a job");
                        receivedAction.Run();

                        Sender.Send(SocketHelper.Serialize(receivedAction));

                    }

                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                    Sender.Shutdown(SocketShutdown.Both);
                    Sender.Close();
                }

            } while (receivedAction != null && !receivedAction.NoJobsLeft);
        }
    }
}
