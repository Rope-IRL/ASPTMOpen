﻿using ASPTM.Helpers;
using ASPTM.MIddleware;
using ASPTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPTM.Controllers
{
    public class FlatsContractController : Controller
    {
        private FlatsContractMiddleware fc_middleware;


        public FlatsContractController( FlatsContractMiddleware fc_middleware)
        {
            this.fc_middleware = fc_middleware;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Location =ResponseCacheLocation.Any, Duration =252)]
        public async Task<IActionResult> Table()
        {

            var flatscs = await fc_middleware.GetFlatsContractsAsync();
            if (flatscs != null) { 
                return View(flatscs);
            }
            return View();
        }

    }
}
