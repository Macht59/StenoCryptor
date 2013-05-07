using StenoCryptor.Commons;
using System.IO;

namespace StenoCryptor.Interfaces
{
    public interface ICryptor
    {
        void Encrypt(Stream message, Key key);

        void Decrypt(Stream message, Key key);
    }
}
