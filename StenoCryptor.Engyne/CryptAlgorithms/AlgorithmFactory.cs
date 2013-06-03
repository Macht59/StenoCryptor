using StenoCryptor.Commons.Enums;
using StenoCryptor.Interfaces;
using System;

namespace StenoCryptor.Engyne.CryptAlgorithms
{
    public class AlgorithmFactory : IAlgorithmFactory
    {
        public ICryptor GetInstance(CryptType cryptType)
        {
            switch (cryptType)
            {
                case CryptType.None:
                    return new MockCriptor();
                case CryptType.DES:
                    return new DESCryptor();
                case CryptType.ThreeDES:
                    return new TreepleDESCryptor();
                case CryptType.AES:
                    return new AESCryptor();
                case CryptType.RSA:
                    return new RSACryptor();
                default:
                    throw new NotSupportedException("Crypt type is not supperted");
            }
        }
    }
}
