using BookShop.Shared.PresentationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Server.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route( "[controller]" )]
    public class IdentityManagerController : ControllerBase
    {
        private RoleManager<IdentityRole> _roleManager;

        public IdentityManagerController(RoleManager<IdentityRole> roleManager )
        {
            _roleManager = roleManager;
        }

        [HttpGet("List")]
        public List<RoleModel> ListRoles()
        {
            return _roleManager.Roles.Select( r => new RoleModel { Name = r.Name } ).ToList();
        }
    }
}
