using System.Web;
using System.Web.Mvc;

namespace ASC_Coil_Tracker_Production
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
