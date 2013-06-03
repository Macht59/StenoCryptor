using StenoCryptor.Interfaces;
using System;

namespace StenoCryptor.Engyne.Detectors
{
    public class GeneralDetector: IDetector
    {
        public bool Detect(Commons.Container container)
        {
            return new Random().Next() % 2 == 1;
        }
    }
}