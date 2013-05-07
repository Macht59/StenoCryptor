using System.IO;

namespace StenoCryptor.Commons.Helpers
{
    public static class FileHelper
    {
        public static string SaveFile(Stream stream, string fileName)
        {
            fileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName));
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
