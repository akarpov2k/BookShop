using BookShop.Server.Data;
using BookShop.Server.Models;
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
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public IdentityManagerController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("RoleList")]
        public List<RoleModel> ListRoles()
        {
            return _roleManager.Roles.Select( r => new RoleModel { RoleId = r.Id, Name = r.Name } ).ToList();
        }

        [HttpGet("UserList")]
        public List<UserModel> ListUsers()
        {
            var usersWithRoles = (from user in _context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from u in _context.Users
                                                   where u.Id == user.Id
                                                   join ur in _context.UserRoles on user.Id equals ur.UserId
                                                   join r in _context.Roles on ur.RoleId equals r.Id
                                                   select r.Name).ToList()
                                  }).ToList().Select( p => new UserModel()

                                  {
                                      UserId = p.UserId,
                                      Name = p.Username,
                                      Email = p.Email,
                                      Roles = string.Join( ",", p.RoleNames )
                                  } );
            return usersWithRoles.ToList();
        }

        [HttpPost("DeleteRole")]
        public async void DeleteRole(string roleId )
        {
            var role = await _roleManager.FindByIdAsync( roleId );
            if (role != null )
            {
                await _roleManager.DeleteAsync( role );
            }
        }
    }
}
