using System.ComponentModel;

namespace BaiTapVideo.Areas.Admin.Models
{
    public class CategoryCustom
    {
        public CategoryCustom(int id, string name, string slug, int status, int numberPost)
        {
            Id = id;
            Name = name;
            Slug = slug;
            Status = status;
            NumberPost = numberPost;
        }

        public int Id { get; set; }
        [DisplayName("Tên chuyên mục")]
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        [DisplayName("Trạng thái")]
        public int Status { get; set; }
        [DisplayName("Số lượng bài viết")]
        public int NumberPost { get; set; }
    }
}
