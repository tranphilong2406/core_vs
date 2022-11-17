using System;
using System.Collections.Generic;

namespace BaiTapVideo.Areas.Admin.Models
{
    public partial class Account
    {
        public Account()
        {
            Posts = new HashSet<Post>();
        }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Roles { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
