using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DistributedOrderer.Common;

namespace DistributedOrderer.Server
{
    public class Server
    {
        public Tracker Tracker { get; set; }

        public Server()
        {
            Tracker = new Tracker();
        }

        public void StartListening()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var localEndPoint = new IPEndPoint(ipAddress, 8080);

            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    if (Tracker.StillHasJobs())
                    {
                        var handler = listener.Accept();

                        new Thread(() =>
                        {
                            var worker = handler;
                            while (Tracker.StillHasJobs())
                            {
                                try
                                {
                                    var bytes = new Byte[100000];

                                    if (Tracker.StillHasJobs())
                                        SendJob(worker, bytes);
                                    else
                                        SendNoJobsLeft(worker, bytes);
                                }
                                catch (SocketException e)
                                {
                                    worker.Shutdown(SocketShutdown.Both);
                                    worker.Close();
                                    break;
                                }
                            }
                            Tracker.ShowResult();
                        }).Start();
                    }
                    else
                    {
                        Tracker.ShowResult();
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SendNoJobsLeft(Socket worker, byte[] bytes)
        {
            var jobAction = new OrderAction() { NoJobsLeft = true };
            worker.Send(SocketHelper.Serialize(jobAction));
        }

        private void SendJob(Socket worker, byte[] bytes)
        {
            worker.Send(SocketHelper.Serialize(Tracker.GetJob()));
            Console.WriteLine($"Job sent to woker: {Tracker.Jobs.Count} jobs left.");

            worker.Receive(bytes);

            var jobAction = (JobAction)SocketHelper.Deserialize(bytes);
            Tracker.AddResult(jobAction);
        }
    }
}
