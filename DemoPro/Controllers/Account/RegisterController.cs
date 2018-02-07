//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using FuturisX;
//using FuturisX.AspNetCore.Mvc.Controllers;
//using FuturisX.Data.Uow;
//using FuturisX.Notifications;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using myCore.Authorization;
//using DAL.Model.ViewModels;
//using FuturisX.AspNetCore.Mvc.Authorization;
//using FuturisX.Security;

//namespace DemoPro.Controllers.Account
//{
//    [Produces("application/json")]
//    [Route("api/register")]
//    public class RegisterController : AppController
//    {
//        private readonly UserManager<FuturisX.Security.User> _userManager;
//        private readonly SignInManager<FuturisX.Security.User> _signInManager;
//        private readonly IUnitOfWorkManager _unitOfWorkManager;
//        private readonly INotificationService _notificationService;
//        public RegisterController(UserManager<FuturisX.Security.User> userManager, SignInManager<FuturisX.Security.User> signInManager, IUnitOfWorkManager unitOfWorkManager, INotificationService notificationService)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _unitOfWorkManager = unitOfWorkManager;
//            _notificationService = notificationService;
//        }
//        [HttpPost]
//        [AppAuthorize(DemoPermission.Administrator, DemoPermission.Page)]
//        public async Task<IActionResult> Post([FromBody]RegisterViewModel model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var user = await _userManager.FindByEmailAsync(model.email);
//                    if (user == null)
//                    {
//                        return BadRequest("This email had been registed, please choose the another email address!");
//                    }
//                    await _unitOfWorkManager.PerformAsyncUow(async () =>
//                    {
//                        var identityResult = await _userManager.CreateAsync(new User
//                        {
//                            UserName = model.username,
//                            IsActive = true,
//                            Name = model.firstName,
//                            Surname = model.lastName,
//                            Email = model.email,
//                            //PasswordHash = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
//                        },model.password);

//                        await _unitOfWorkManager.Current.SaveChangesAsync();

//                        if (!identityResult.Succeeded)
//                            throw new UserFriendlyException(identityResult.Errors.Select(r => r.Description).FirstOrDefault());

//                        user = await _userManager.FindByEmailAsync(model.email);

//                        await _userManager.AddToRoleAsync(user, DemoPermission.Administrator);
//                    });
//                    return Ok(user);
//                }
//                else
//                {
//                    return BadRequest(ModelState);
//                }
//            }catch(Exception err)
//            {
//                return BadRequest(err);
//            }
//        }
//    }
//}