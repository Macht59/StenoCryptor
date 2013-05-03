using StenoCryptor.Commons;
using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.CryptAlgorithms
{
    public static class AlgorithmFactory
    {
        public static ICryptor GetInstance(CryptType cryptType)
        {
            switch (cryptType)
            {
                case CryptType.DES:
                    return new DESCryptor();
                case CryptType.ThreeDES:
                    return new ThreeDESCryptor();
                case CryptType.AES:
                    return null;
                default:
                    throw new NotSupportedException("Crypt type is not supperted");
            }
        }
    }
}
