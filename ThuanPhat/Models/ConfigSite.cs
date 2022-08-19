using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ThuanPhat.Models
{
    public class ConfigSite
    {
        public int Id { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Facebook"),
         Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Facebook { get; set; }

        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Youtube"),
        Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Youtube { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Twitter"),
        Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Twitter { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Instagram"),
        Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Instagram { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã nhúng Live chat"),
        UIHint("TextArea")]
        public string LiveChat { get; set; }
        [Display(Name = "Logo"), UIHint("ImageConfig")]
        public string Image { get; set; }
        [Display(Name = "Favicon"), UIHint("ImageConfig")]
        public string Favicon { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã Google Map"), UIHint("TextArea")]
        public string GoogleMap { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã Google Analytics"), UIHint("TextArea")]
        public string GoogleAnalytics { get; set; }
        [Display(Name = "Địa chỉ"), UIHint("TextBox")]
        public string Place { get; set; }
        [Display(Name = "Thẻ title"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Title { get; set; }
        [Display(Name = "Thẻ description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Ảnh giới thiệu"), UIHint("ImageAbout")]
        public string AboutImage { get; set; }
        [Display(Name = "Thông tin bài giới thiệu"), UIHint("EditorBox")]
        public string AboutText { get; set; }
        [Display(Name = "Hotline"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Hotline { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Email"), UIHint("TextBox")]
        public string Email { get; set; }
        [Display(Name = "Thông tin liên hệ"), UIHint("EditorBox")]
        public string InfoContact { get; set; }
        [Display(Name = "Thông tin chân trang"), UIHint("EditorBox")]
        public string InfoFooter { get; set; }
        [Display(Name = "Mã số thuế"), UIHint("TextBox")]
        public string TaxCode { get; set; }

        public virtual ICollection<ConfigSiteLang> ConfigSiteLangs { get; set; }
    }

    public class ConfigSiteLang
    {
        [Key, Column(Order = 1)]
        public int ConfigSiteId { get; set; }
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }
        [Display(Name = "Địa chỉ"), UIHint("TextBox")]
        public string Place { get; set; }   
        [Display(Name = "Thẻ title"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Title { get; set; }
        [Display(Name = "Thẻ description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Thông tin bài giới thiệu"), UIHint("EditorBox")]
        public string AboutText { get; set; }
        [Display(Name = "Thông tin liên hệ"), UIHint("EditorBox")]
        public string InfoContact { get; set; }
        [Display(Name = "Thông tin chân trang"), UIHint("EditorBox")]
        public string InfoFooter { get; set; }

        public virtual ConfigSite ConfigSite { get; set; }
        public virtual Language Language { get; set; }
    }

    public class ConfigSiteDto
    {
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LiveChat { get; set; }
        public string Image { get; set; }
        public string Favicon { get; set; }
        public string GoogleMap { get; set; }
        public string GoogleAnalytics { get; set; }
        public string Place { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AboutImage { get; set; }
        public string AboutText { get; set; }
        public string Hotline { get; set; }
        public string Email { get; set; }
        public string InfoContact { get; set; }
        public string InfoFooter { get; set; }
        public string TaxCode { get; set; }
    }
}