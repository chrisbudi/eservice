using E.ServiceCore.Identity.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace E.ServiceCore.Identity.Api.Models
{
    public class RegisterModel : LoginModel
    {
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int JabatanId { get; set; }
        public int DepartmentId { get; set; }
        public int LocationId { get; set; }

      
    }
}
