using StenoCryptor.Commons;
using System.IO;

namespace StenoCryptor.Interfaces
{
    public interface ICryptor: IKeyAware
    {
        Stream Encrypt(Stream message, Key key);

        Stream Decrypt(Stream message, Key key);
    }
}
