using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuanPhat.Models
{
    public class Banner
    {
        public int Id { get; set; }
        [Display(Name = "Tên banner"), Required(ErrorMessage = "Hãy nhập tên banner"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string BannerName { get; set; }
        [Display(Name = "Slogan"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Slogan { get; set; }
        [Display(Name = "Hình ảnh"), StringLength(500)]
        public string Image { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Vị trí quảng cáo"), Required(ErrorMessage = "Hãy chọn vị trí quảng cáo"), UIHint("GroupId")]
        public int GroupId { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Nội dung giới thiệu"), UIHint("EditorBox")]
        public string Content { get; set; }
        public virtual ICollection<BannerLang> BannerLangs { get; set; }
    }

    public class BannerLang
    {
        [Key, Column(Order = 1)]
        public int BannerId { get; set; }
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }

        [Display(Name = "Tên banner"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string BannerName { get; set; }
        [Display(Name = "Slogan"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Slogan { get; set; }
        [Display(Name = "Hình ảnh"), StringLength(500)]
        public string Image { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Nội dung giới thiệu"), UIHint("EditorBox")]
        public string Content { get; set; }
        public virtual Banner Banner { get; set; }
        public virtual Language Language { get; set; }
    }
    public class BannerDto
    {
        public int Id { get; set; }
        public string BannerName { get; set; }
        public string Slogan { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public int GroupId { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public string Content { get; set; }
    }
}