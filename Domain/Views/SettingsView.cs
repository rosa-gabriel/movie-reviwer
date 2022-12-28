using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class SettingsView
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}