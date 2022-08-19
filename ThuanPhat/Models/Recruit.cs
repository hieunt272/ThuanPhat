using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ThuanPhat.Properties;

namespace ThuanPhat.Models
{
    public class Recruit
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "FullName"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), UIHint("TextBox"), StringLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Maxlength")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "BirthYear"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), UIHint("TextBox"), StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Maxlength")]
        public string BirthYear { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Gender"), UIHint("Gender")]
        public int Gender { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InValid"), UIHint("TextBox")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NumberPhone"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), Phone(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InValid"), UIHint("TextBox")]
        public string Mobile { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "ContactMethod"), UIHint("ContactMethod")]
        public int ContactMethod { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "ContactHour")]
        public string ContactHour { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Place"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required"), UIHint("TextArea")]
        public string Address { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Resume"), StringLength(500)]
        public string Resume { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Message"), DataType(DataType.MultilineText), StringLength(4000)]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Career"), Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        public int CareerId { get; set; }
        public virtual Career Career { get; set; }
        public Recruit()
        {
            CreateDate = DateTime.Now;
        }
    }
}