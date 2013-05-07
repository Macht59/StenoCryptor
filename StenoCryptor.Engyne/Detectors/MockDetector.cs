using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.Detectors
{
    public class MockDetector: IDetector
    {
        public bool Detect(Commons.Container container)
        {
            return new Random().Next() % 2 == 1;
        }
    }
}
