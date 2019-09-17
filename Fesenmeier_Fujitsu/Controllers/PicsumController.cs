using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fesenmeier_Fujitsu.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fesenmeier_Fujitsu.Controllers
{
    public class PicsumController : Controller
    {

        private readonly IPicsumService _picsumService;

        public PicsumController(IPicsumService picsumService)
        {
            _picsumService = picsumService;
        }

        
        public IActionResult RandomImg()
        {
            ViewData["ImgPath"] =  _picsumService.GetRandomImg();
            return View();
        }
    }
}