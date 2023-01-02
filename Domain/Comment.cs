using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Views;

namespace Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public User Creator { get; set; }
        public Movie Movie { get; set; }
        public DateTime PostDate { get; set; }
        public string Message { get; set; }
        public bool wasEdited { get; set; }

        public CommentView ToCommentView()
        {
            CommentView cv = new CommentView();

            cv.Creator = this.Creator.ToUserView();
            cv.Id = Id;
            cv.Message = this.Message;
            cv.PostDate = this.PostDate;
            cv.wasEdited = this.wasEdited;
            return cv;
        }
    }
}