using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ThuanPhat.Models
{
    public class Career
    {
        public int Id { get; set; }
        [Display(Name = "Nghề nghiệp"), Required(ErrorMessage = "Hãy nhập nghề nghiệp"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        public virtual ICollection<CareerLang> CareerLangs { get; set; }
    }

    public class CareerLang
    {
        [Key, Column(Order = 1)]
        public int CareerId { get; set; }
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }
        [Display(Name = "Nghề nghiệp"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        public virtual Career Career { get; set; }
        public virtual Language Language { get; set; }
    }

    public class CareerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }
    }
}