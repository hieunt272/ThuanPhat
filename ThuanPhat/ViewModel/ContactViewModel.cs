using System.Collections.Generic;
using ThuanPhat.Models;

namespace ThuanPhat.ViewModel
{
    public class ListContactViewModel
    {
        public PagedList.IPagedList<Contact> Contacts { get; set; }
        public string Name { get; set; }
    }
    public class ListSubcribeViewModel
    {
        public PagedList.IPagedList<Subcribe> Subcribes { get; set; }
        public string Name { get; set; }
    }
    public class ListRecruitViewModel
    {
        public PagedList.IPagedList<Recruit> Recruits { get; set; }
        public string Name { get; set; }
    }
}