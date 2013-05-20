using StenoCryptor.Commons;
using StenoCryptor.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Helpers
{
    public static class KeyMakerHelper
    {
        public static Key GenerateKey(Container container, byte[] message, CryptType cryptType, byte[] keyValue)
        {
            Key key = new Key();
            key.CryptType = cryptType;
            key.MessageLength = message.Length;
            key.Value = keyValue;

            return key;
        }
    }
}