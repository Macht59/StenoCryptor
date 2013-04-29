using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Commons
{
    public static class FileHelper
    {
        public static string SaveFile(Stream stream, string fileName)
        {
            using (FileStream fs = File.Create(fileName))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                fs.Write(bytesInStream, 0, bytesInStream.Length);

                return fs.Name;
            }
        }
    }
}
