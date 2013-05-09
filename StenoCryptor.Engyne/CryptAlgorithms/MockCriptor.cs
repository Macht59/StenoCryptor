using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.CryptAlgorithms
{
    public class MockCriptor: ICryptor
    {
        public System.IO.Stream Encrypt(System.IO.Stream message, Commons.Key key)
        {
            return message;
        }

        public System.IO.Stream Decrypt(System.IO.Stream message, Commons.Key key)
        {
            return message;
        }

        public bool ValidateKey(string key)
        {
            return true;
        }

        public Commons.Key ParseKey(string key)
        {
            return new Commons.Key();
        }
    }
}
