using StenoCryptor.Commons.Constants;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace StenoCryptor.Web.Helpers
{
    /// <summary>
    /// Helps to work with streams and files.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Converts string into stream in default encoding.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Stream with string in it.</returns>
        public static Stream StringToStream(string str)
        {
            return StringToStream(str, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// Converts string into stream.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="encoding">String encoding.</param>
        /// <returns>Stream with string in it.</returns>
        public static Stream StringToStream(string str, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(str);

            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Appends string to stream in default encoding.
        /// </summary>
        /// <param name="stream">Main stream.</param>
        /// <param name="appendingString">String to be appended.</param>
        /// <remarks>Used default encoding.</remarks>
        public static void AppendToStream(MemoryStream stream, string appendingString)
        {
            AppendToStream(stream, appendingString, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// Appends string to stream in default encoding.
        /// </summary>
        /// <param name="stream">Main stream.</param>
        /// <param name="appendingString">String to be appended.</param>
        /// <param name="encoding">String encoding.</param>
        public static void AppendToStream(MemoryStream stream, string appendingString, Encoding encoding)
        {
            byte[] streamBytes = StreamToBytesArray(stream);
            byte[] appendixBytes = encoding.GetBytes(appendingString);
            IEnumerable<byte> result = streamBytes.Concat(appendixBytes);
            stream = new MemoryStream(result.ToArray());
        }

        public static Stream AppendToStream(MemoryStream firstStream, Stream secondStream)
        {
            byte[] firstBytesArray = StreamToBytesArray(firstStream);
            byte[] secondBytesArray = StreamToBytesArray(secondStream);
            IEnumerable<byte> result = firstBytesArray.Concat(secondBytesArray);

            return new MemoryStream(result.ToArray());
        }

        /// <summary>
        /// Saves stream to file with pointed name.
        /// </summary>
        /// <param name="stream">Stream to save from.</param>
        /// <param name="fileName">Full file name.</param>
        /// <returns>Addres of file on server.</returns>
        public static string SaveFile(Stream stream, string fileName)
        {
            using (FileStream fs = File.Create(fileName))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                fs.Write(bytesInStream, 0, bytesInStream.Length);

                return fs.Name;
            }
        }

        public static byte[] StreamToBytesArray(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static Stream BytesToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static string StreamToString(Stream stream)
        {
            return StreamToString(stream, Constants.DEFAULT_ENCODING);
        }

        public static string StreamToString(Stream stream, Encoding encoding)
        {
            byte[] bytes = StreamToBytesArray(stream);

            return encoding.GetString(bytes);
        }
    }
}
