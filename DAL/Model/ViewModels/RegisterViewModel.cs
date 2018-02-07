using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace DAL.Model.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string gender { get; set; }
    }
}
