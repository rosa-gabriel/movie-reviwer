using System.ComponentModel.DataAnnotations;
using Domain.Views;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class ConfirmationEmailToken
    {
        [Key]
        [StringLength(300)]
        public string Token { get; set; }
        public User User { get; set; }
    }
}