namespace BaiTapVideo.Areas.Admin.Models
{
    public class Status
    {
        public Status(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int id { get; set; }
        public String name { get; set; }
    }
}
