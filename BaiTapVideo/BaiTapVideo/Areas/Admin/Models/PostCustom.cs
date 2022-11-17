using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaiTapVideo.Areas.Admin.Models
{
    public class PostCustom
    {
        public PostCustom(int id, string title, string fullContent, string img, int status, string name)
        {
            Id = id;
            Title = title;
            FullContent = fullContent;
            Img = img;
            Status = status;
            CatName = name;
        }

        public int Id { get; set; }
        [DisplayName("Tên bài viết")]
        [Required]
        [MaxLength(50, ErrorMessage = "Tiêu đề không được dài quá 50 ký tự")]
        [MinLength(10, ErrorMessage = "Tiêu đề không được ngắn hơn 10 ký tự")]
        public string Title { get; set; } = null!;
        [DisplayName("Nội dung")]
        public string FullContent { get; set; } = null!;
        [DisplayName("Hình ảnh")]
        public string? Img { get; set; }
        [DisplayName("Trạng thái")]
        public int Status { get; set; }
        [DisplayName("Tên chuyên mục")]
        public string CatName { get; set; }=null!;
    }
}
