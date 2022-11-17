using System;
using System.Collections.Generic;

namespace BaiTapVideo.Areas.Admin.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string FullContent { get; set; } = null!;
        public string? Img { get; set; }
        public int Status { get; set; }
        public int? CatId { get; set; }
        public string? Author { get; set; }

        public virtual Account? AuthorNavigation { get; set; }
        public virtual Category? Cat { get; set; }
    }
}
