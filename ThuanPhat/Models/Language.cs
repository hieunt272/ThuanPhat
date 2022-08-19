using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThuanPhat.Models
{
    public class Language
    {
        public int Id { get; set; }
        [StringLength(50), Required]
        public string Name { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [StringLength(10)]
        public string Culture { get; set; }
        public bool Active { get; set; }
    }
}