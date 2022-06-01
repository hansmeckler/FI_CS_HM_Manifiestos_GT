using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Manifiesto.Data.Aereo;
using Manifiesto.Data.Admin;
using Manifiesto.Data.Maritimo;
using Manifiesto.Data.Trafico;
using Manifiesto.Web.Repositories.CuscarServices;


namespace Manifiesto.Web
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();   
      container.RegisterType<IAdminService, AdminService>();
      container.RegisterType<IAereoService, AereoService>();
      container.RegisterType<IMaritimoServices, MaritimoServices>();
      container.RegisterType<ICuscarMaritimoServices, CuscarMaritimoServices>();
      container.RegisterType<ICuscarAereoServices, CuscarAereoServices>();
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}