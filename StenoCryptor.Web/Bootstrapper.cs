using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using StenoCryptor.Interfaces;
using StenoCryptor.Engyne.CryptAlgorithms;
using StenoCryptor.Engyne.Embeders;
using StenoCryptor.Engyne.Detectors;

namespace StenoCryptor.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
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