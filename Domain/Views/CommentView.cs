using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class CommentView
    {
        public UserView Creator { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }
    }
}