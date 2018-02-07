//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using FuturisX;
//using FuturisX.AspNetCore.Mvc.Authorization;
//using FuturisX.AspNetCore.Mvc.Controllers;
//using FuturisX.Data.Uow;
//using FuturisX.Notifications;
//using FuturisX.Security;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using myCore.Authorization;

//namespace DemoPro.Controllers
//{
//    [Route("api/[controller]")]
//    public class ValuesController : AppController
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;
//        private readonly IUnitOfWorkManager _unitOfWorkManager;
//        private readonly INotificationService _notificationService;
//        public ValuesController(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWorkManager unitOfWorkManager, INotificationService notificationService)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _unitOfWorkManager = unitOfWorkManager;
//            _notificationService = notificationService;
//        }
//        // GET api/values
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/values/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/values
//        [HttpPost]
//        public void Post([FromBody]string value)
//        {
//        }

//        // PUT api/values/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody]string value)
//        {
//        }
//        // DELETE api/values/5
//        [HttpDelete("{id}")]
//        [AppAuthorize(DemoPermission.Administrator, DemoPermission.Page)]
//        public void Delete(int id)
//        {
//        }
//    }
//}
