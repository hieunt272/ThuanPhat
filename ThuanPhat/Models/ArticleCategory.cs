using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuanPhat.Models
{
    public class ArticleCategory
    {
        public int Id { get; set; }
        [Display(Name = "Tên danh mục"), Required(ErrorMessage = "Hãy nhập tên danh mục"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string CategoryName { get; set; }
        [Display(Name = "Giới thiệu ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int CategorySort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool CategoryActive { get; set; }
        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }
        [Display(Name = "Hiển thị menu")]
        public bool ShowMenu { get; set; }
        [Display(Name = "Hiển thị trang chủ")]
        public bool ShowHome { get; set; }

        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        [Display(Name = "Ảnh đại diện"), StringLength(500)]
        public string Image { get; set; }
        [Display(Name = "Ảnh giới thiệu"), StringLength(500)]
        public string AboutImage { get; set; }
        [Display(Name = "Ảnh hình thành và phát triển"), StringLength(500)]
        public string FormationImage { get; set; }
        [Display(Name = "Nội dung giới thiệu"), UIHint("EditorBox")]
        public string AboutText { get; set; }
        [Display(Name = "Nội dung phương châm"), UIHint("EditorBox")]
        public string MottoText { get; set; }
        [Display(Name = "Nội dung thành thành và phát triển"), UIHint("EditorBox")]
        public string FormationText { get; set; }
        [Display(Name = "Loại danh mục")]
        public TypePost TypePost { get; set; }
        [ForeignKey("ParentId")]
        public virtual ArticleCategory ParentCategory { get; set; }

        public virtual ICollection<ArticleCategoryLang> ArticleCategoryLangs { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

        public ArticleCategory()
        {
            CategoryActive = true;
        }
    }

    public class ArticleCategoryLang
    {
        [Key, Column(Order = 1)]
        public int ArticleCategoryId { get; set; }
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }
        [Display(Name = "Tên danh mục"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string CategoryName { get; set; }
        [Display(Name = "Giới thiệu ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        [Display(Name = "Ảnh giới thiệu"), StringLength(500)]
        public string AboutImage { get; set; }
        [Display(Name = "Nội dung giới thiệu"), UIHint("EditorBox")]
        public string AboutText { get; set; }
        [Display(Name = "Nội dung phương châm"), UIHint("EditorBox")]
        public string MottoText { get; set; }
        [Display(Name = "Nội dung thành thành và phát triển"), UIHint("EditorBox")]
        public string FormationText { get; set; }
        public virtual ArticleCategory ArticleCategory { get; set; }
        public virtual Language Language { get; set; }
    }

    public class ArticleCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Url { get; set; }
        public int CategorySort { get; set; }
        public int? ParentId { get; set; }
        public bool ShowHome { get; set; }
        public bool ShowMenu { get; set; }
        public bool CategoryActive { get; set; }
        public string TitleMeta { get; set; }
        public string DescriptionMeta { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string AboutImage { get; set; }
        public string FormationImage { get; set; }
        public string AboutText { get; set; }
        public string MottoText { get; set; }
        public string FormationText { get; set; }
        public virtual ICollection<ArticleDto> Articles { get; set; }
        public string RootUrl { get; set; }
        public string RootName { get; set; }
        public TypePost TypePost { get; set; }
    }

    public enum TypePost
    {
        [Display(Name = "Bài viết")]
        Article,
        [Display(Name = "Giới thiệu")]
        Introduce,
        [Display(Name = "Đào tạo")]
        Training,
        [Display(Name = "Tuyển dụng")]
        Recruit,
    }
}