using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class FriendsListView
    {
        public List<FriendView> Friends { get; set; } = new List<FriendView>();
        public List<FriendView> FriendRequests { get; set; } = new List<FriendView>();
    }
}