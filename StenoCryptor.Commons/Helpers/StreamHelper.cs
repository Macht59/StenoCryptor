using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Commons.Helpers
{
    public static class StreamHelper
    {
        public static Stream StringToStream(string str)
        {
            return StringToStream(str, Encoding.UTF8);
        }

        public static Stream StringToStream(string str, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(str);

            return new MemoryStream(bytes);
        }
    }
}
