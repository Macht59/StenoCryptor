using Microsoft.Practices.Unity;
using StenoCryptor.Engyne.CryptAlgorithms;
using StenoCryptor.Engyne.Detectors;
using StenoCryptor.Engyne.Embeders;
using StenoCryptor.Interfaces;

namespace StenoCryptor.Desktop
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IAlgorithmFactory, AlgorithmFactory>();
            container.RegisterType<IEmbederFactory, EmbederFactory>();
            container.RegisterType<IDetectorFactory, DetectorFactory>();

            return container;
        }
    }
}