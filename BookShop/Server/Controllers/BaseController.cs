using BookShop.Server.SystemModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController( AppSettings appSettings )
        {
            AppSettings = appSettings;
        }

        protected AppSettings AppSettings { get; init; }
    }
}
