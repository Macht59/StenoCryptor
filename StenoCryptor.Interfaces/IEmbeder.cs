using StenoCryptor.Commons;
using System.IO;

namespace StenoCryptor.Interfaces
{
    public interface IEmbeder
    {
        void Embed(Container container, Stream message);
    }
}
