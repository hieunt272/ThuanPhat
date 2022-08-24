using System;
using System.ComponentModel.DataAnnotations;
using ThuanPhat.Properties;

namespace ThuanPhat.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "FullName"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), UIHint("TextBox"), StringLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Maxlength")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NumberPhone"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), Phone(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InValid"), UIHint("TextBox")]
        public string Mobile { get; set; }
        [Display(Name = "Email"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), StringLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Maxlength"), EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InValid"), UIHint("TextBox")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Body"), DataType(DataType.MultilineText), StringLength(4000)]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public Contact()
        {
            CreateDate = DateTime.Now;
        }
    }
}