﻿using StenoCryptor.Commons.Enums;
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