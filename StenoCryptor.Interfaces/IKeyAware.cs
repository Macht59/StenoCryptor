using StenoCryptor.Commons;
using System.IO;

namespace StenoCryptor.Interfaces
{
    public interface IKeyAware
    {
        bool ValidateKey(string key);

        bool ValidateKey(Key key, Container container);

        byte[] ParseKey(string key);
    }
}
