using System.IO;
using System.Text;

namespace StenoCryptor.Web.Helpers
{
    /// <summary>
    /// Helps to work with streams and files.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Converts string into stream.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Stream with string in it.</returns>
        public static Stream StringToStream(string str)
        {
            return StringToStream(str, Encoding.UTF8);
        }

        public static Stream StringToStream(string str, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(str);

            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Saves stream to file with pointed name.
        /// </summary>
        /// <param name="stream">Stream to save from.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Addres of file on server.</returns>
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
