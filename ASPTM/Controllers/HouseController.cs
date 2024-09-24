using ASPTM.Helpers;
using ASPTM.MIddleware;
using ASPTM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASPTM.Controllers
{
    public class HouseController : Controller
    {
        private HouseMiddleware h_middleware;


        public HouseController(HouseMiddleware h_middleware)
        {
            this.h_middleware = h_middleware;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 252)]
        public async Task<IActionResult> Table()
        {

            var houses = await h_middleware.GetHouses();
            if (houses != null)
            {
                return View(houses);
            }
            return View();
        }
    }
}
