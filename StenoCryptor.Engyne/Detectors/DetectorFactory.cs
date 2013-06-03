using StenoCryptor.Interfaces;

namespace StenoCryptor.Engyne.Detectors
{
    public class DetectorFactory: IDetectorFactory
    {
        public IDetector GetInstance()
        {
            return new GeneralDetector();
        }
    }
}
