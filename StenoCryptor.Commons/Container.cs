using StenoCryptor.Commons.Enums;
using System.IO;

namespace StenoCryptor.Commons
{
    public class Container
    {
        public Container()
        {

        }

        public Container(Stream stream)
            : this(stream, null)
        {

        }

        public Container(Stream stream, string contentType)
        {
            InputStream = stream;
            ContentType = contentType;
        }

        public Stream InputStream { get; set; }

        public string ContentType { get; set; }


    }
}
