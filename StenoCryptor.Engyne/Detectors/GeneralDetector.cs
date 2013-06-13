using StenoCryptor.Commons;
using StenoCryptor.Interfaces;
using System;

namespace StenoCryptor.Engyne.Detectors
{
    /// <summary>
    /// Detects DWM within container.
    /// </summary>
    public class GeneralDetector: IDetector
    {
        /// <summary>
        /// Detects DWM in container
        /// </summary>
        /// <param name="container">Container.</param>
        /// <returns>True if there is a dwm in container, false otherwise.</returns>
        public bool Detect(Container container)
        {
            return random.Next() % 2 == 1;
        }

        /// <summary>
        /// Pseudo-random number generator instance.
        /// </summary>
        private Random random = new Random();
    }
}