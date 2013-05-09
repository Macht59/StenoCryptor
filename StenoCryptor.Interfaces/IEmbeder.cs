using StenoCryptor.Commons;

namespace StenoCryptor.Interfaces
{
    public interface IEmbeder
    {
        void Embed(Container container, byte[] message);

        byte[] Extract(Container container);
    }
}
