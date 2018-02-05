using System;
using System.Runtime.InteropServices;
using System.Security;

namespace FuturisX.Helpers.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool IsFatal(this System.Exception ex)
        {
            return
                ex is OutOfMemoryException ||
                ex is SecurityException ||
                ex is SEHException;
        }
    }
}