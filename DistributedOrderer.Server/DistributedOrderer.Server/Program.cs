using System;

namespace DistributedOrderer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server().StartListening();
        }
    }
}
