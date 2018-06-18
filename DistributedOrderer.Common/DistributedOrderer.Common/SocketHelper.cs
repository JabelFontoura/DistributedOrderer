using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DistributedOrderer.Common
{
    public class SocketHelper
    {
        public static byte[] Serialize(JobAction action)
        {
            if (action == null)
                return null;

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, action);
            ms.Close();

            return ms.ToArray();
        }

        public static object Deserialize(byte[] arrBytes)
        {
            var ms = new MemoryStream(8192);
            var bf = new BinaryFormatter();

            ms.Write(arrBytes, 0, arrBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);

            return (object) bf.Deserialize(ms);
        }
    }
}