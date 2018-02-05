using FuturisX.Dependency;
using Microsoft.AspNetCore.Mvc;

namespace FuturisX.AspNetCore.Mvc.Controllers
{
    public class AppController : Controller, ITransientDependency
    {
    }
}