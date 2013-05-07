using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.Detectors
{
    public class DetectorFactory: IDetectorFactory
    {
        public IDetector GetInstance()
        {
            return new MockDetector();
        }
    }
}
