[assembly: WebActivator.PreApplicationStartMethod(typeof(StenoCryptor.Web.App_Start.Combres), "PreStart")]
namespace StenoCryptor.Web.App_Start {
    using global::Combres;
    using System.Web.Routing;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}