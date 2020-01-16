using System;
using System.ComponentModel.DataAnnotations;

namespace E.ServiceCore.Identity.Api.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }


    public class LoginSSOModel
    {

        [Required]
        public string Username { get; set; }

    }
}
