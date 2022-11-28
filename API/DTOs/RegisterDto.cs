using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}