using System.IO;

namespace StenoCryptor.Commons
{
    public class Container
    {
        public Container()
        {

        }

        public Container(Stream stream)
        {
            Data = stream;
        }

        public Stream Data { get; set; }
    }
}
