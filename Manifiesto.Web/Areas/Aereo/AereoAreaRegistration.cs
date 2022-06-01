using System.Web.Mvc;

namespace Manifiesto.Web.Areas.Aereo
{
    public class AereoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Aereo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Aereo_default",
                "Aereo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
