using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuanPhat.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Display(Name = "Tiêu đề", Description = "Tiêu đề dài tối đa 150 ký tự"),
         Required(ErrorMessage = "Hãy nhập tiêu đề"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"),
         UIHint("TextBox")]
        public string ServiceName { get; set; }
        [Display(Name = "Trích dẫn ngắn"), Required(ErrorMessage = "Hãy nhập trích dẫn ngắn"), UIHint("EditorBox")]
        public string Description { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Ảnh đại diện"), StringLength(500)]
        public string Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Danh mục dịch vụ"), Required(ErrorMessage = "Hãy chọn danh mục bài viết")]
        public int ServiceCategoryId { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool Home { get; set; }
        [StringLength(300)]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }

        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public virtual ICollection<ServiceLang> ServiceLangs { get; set; }

        public Service()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }

    public class ServiceLang
    {
        [Key, Column(Order = 1)]
        public int ServiceId { get; set; }
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }
        [StringLength(150, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string ServiceName { get; set; }
        [Display(Name = "Trích dẫn ngắn"), UIHint("EditorBox")]
        public string Description { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [StringLength(300)]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }

        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }

        public virtual Service Service { get; set; }
        public virtual Language Language { get; set; }
    }

    public class ServiceDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreateDate { get; set; }
        public int ServiceCategoryId { get; set; }
        public int? ParentId { get; set; }
        public bool Active { get; set; }
        public bool Home { get; set; }
        public string Url { get; set; }
        public string TitleMeta { get; set; }
        public string DescriptionMeta { get; set; }
        public string CategoryName { get; set; }
        public string CategoryUrl { get; set; }

    }
}