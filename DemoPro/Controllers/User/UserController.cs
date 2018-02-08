using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturisX.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Model;
using DAL.Model.ViewModels;
using Microsoft.Extensions.Logging;

namespace DemoPro.Controllers.User
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : AppController
    {
        private readonly DemoProContext _ctx;
        private readonly ILogger<UserController> _logger;

        public UserController(DemoProContext context, ILogger<UserController> iLogger)
        {
            this._ctx = context;
            this._logger = iLogger;
        }
        [HttpGet]
        public IActionResult GetAllUser()
        {
            try
            {
                var users = _ctx.Users.ToList();
                var viewUsers = new List<UserViewModel>();
                if (users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        viewUsers.Add(new UserViewModel()
                        {
                            id = user.Id,
                            username = user.UserName,
                            email = user.Email,
                            firstname = user.Name,
                            lastname = user.Surname,
                            accessFailedCount = user.AccessFailedCount,
                            isLockoutEnabled = user.IsLockoutEnabled,
                            lockoutEndDateUtc = user.LockoutEndDateUtc,
                            isActive = user.IsActive
                        });
                    }
                    return Ok(viewUsers);
                }
                else
                    return BadRequest("Cant get any users!");
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}