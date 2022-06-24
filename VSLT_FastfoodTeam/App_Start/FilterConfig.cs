using System.Web;
using System.Web.Mvc;

namespace VSLT_FastfoodTeam
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
