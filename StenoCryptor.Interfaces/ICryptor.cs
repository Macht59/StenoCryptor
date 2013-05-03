using StenoCryptor.Commons;

namespace StenoCryptor.Interfaces
{
    public interface ICryptor
    {
        Container Encrypt(Container container, Key key);

        Container Decrypt(Container container, Key key);
    }
}
