using System;
using System.Collections.Generic;

namespace BaiTapVideo.Areas.Admin.Models
{
    public partial class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int Status { get; set; }
        public int? Numberpost { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
