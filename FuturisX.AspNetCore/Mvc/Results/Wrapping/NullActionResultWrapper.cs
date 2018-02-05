using Microsoft.AspNetCore.Mvc.Filters;

namespace FuturisX.AspNetCore.Mvc.Results.Wrapping
{
    public class NullActionResultWrapper : IActionResultWrapper
    {
        public void Wrap(ResultExecutingContext actionResult)
        {
        }
    }
}