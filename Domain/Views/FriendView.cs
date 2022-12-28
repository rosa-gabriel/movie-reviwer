using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class FriendView
    {
        public Guid Id { get; set; }
        public UserView Friend { get; set; }
        public bool Sent { get; set; }
    }
}