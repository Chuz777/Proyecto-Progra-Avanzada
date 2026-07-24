using System.Web.Mvc;
using Proyecto.Filters;

namespace Proyecto
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuditActionFilter());
            filters.Add(new ApplicationInfoResultFilter());
        }
    }
}