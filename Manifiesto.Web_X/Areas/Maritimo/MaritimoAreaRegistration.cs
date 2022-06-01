using System.Web.Mvc;

namespace Manifiesto.Web.Areas.Maritimo
{
    public class MaritimoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Maritimo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Maritimo_default",
                "Maritimo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
