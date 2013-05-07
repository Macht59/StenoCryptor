using Microsoft.Practices.Unity;
using StenoCryptor.Engyne.CryptAlgorithms;
using StenoCryptor.Engyne.Detectors;
using StenoCryptor.Engyne.Embeders;
using StenoCryptor.Interfaces;
using System.Web.Mvc;
using Unity.Mvc3;

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