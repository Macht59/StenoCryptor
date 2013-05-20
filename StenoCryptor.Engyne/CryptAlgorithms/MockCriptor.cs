using StenoCryptor.Commons;
using StenoCryptor.Interfaces;
using System.IO;

namespace StenoCryptor.Engyne.CryptAlgorithms
{
    public class MockCriptor: ICryptor
    {
        public System.IO.Stream Encrypt(Stream message, Key key)
        {
            return message;
        }

        public System.IO.Stream Decrypt(Stream message, Key key)
        {
            return message;
        }

        public bool ValidateKey(string key)
        {
            return true;
        }

        public byte[] ParseKey(string key)
        {
            return null;
        }


        public bool ValidateKey(Key key, Container container)
        {
            return true;
        }
    }
}
