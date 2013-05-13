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

        public byte[] ParseKey(string key)
        {
            throw new NotImplementedException();
        }


        public bool ValidateKey(Commons.Key key, Commons.Container container)
        {
            throw new NotImplementedException();
        }
    }
}
