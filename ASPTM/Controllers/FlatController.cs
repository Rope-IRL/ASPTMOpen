
using ASPTM.Models;
using ASPTM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASPTM.Controllers
{
    public class FlatController : Controller
    {
        private FlatService _middleware;
        public FlatController(FlatService _middleware)
        {
            this._middleware = _middleware;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 252)]
        public async Task<IActionResult> Table()
        {
            var flats = await _middleware.GetFlats();
            if (flats != null)
            {
                return View(flats);
            }
            return View();
        }
    }
}
