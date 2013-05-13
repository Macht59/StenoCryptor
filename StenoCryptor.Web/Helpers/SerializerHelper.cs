using StenoCryptor.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace StenoCryptor.Web.Helpers
{
    public static class SerializeHelper
    {
        public static Stream SerializeBinary(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            stream.Position = 0;

            return stream;
        }

        public static object DeserializeBinary(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
         
            return formatter.Deserialize(stream);
        }
    }
}