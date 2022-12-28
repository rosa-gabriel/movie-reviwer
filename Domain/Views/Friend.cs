using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class Friend
    {
        public Guid Id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public bool Accepted { get; set; }

        public FriendView ToFriendView(User user)
        {
            return new FriendView
            {
                Id = this.Id,
                Friend = user == this.Receiver ? Sender.ToUserView() : Receiver.ToUserView(),
                Sent = user == this.Sender,
            };
        }

        public bool HasFriendRequest(User u1, User u2)
        {
            if (u1 == this.Sender && u2 == this.Receiver) return true;
            if (u1 == this.Receiver && u2 == this.Sender) return true;
            return false;
        }
    }

}