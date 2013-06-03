using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.CryptAlgorithms
{
    public class RSACryptor: ICryptor
    {
        public System.IO.Stream Encrypt(System.IO.Stream message, Commons.Key key)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream Decrypt(System.IO.Stream message, Commons.Key key)
        {
            throw new NotImplementedException();
        }

        public bool ValidateKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool ValidateKey(Commons.Key key, Commons.Container container)
        {
            throw new NotImplementedException();
        }

        public byte[] ParseKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}
