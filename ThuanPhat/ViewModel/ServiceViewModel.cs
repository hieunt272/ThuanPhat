using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuanPhat.Models;

namespace ThuanPhat.ViewModel
{
    public class ListServiceViewModel
    {
        public PagedList.IPagedList<Service> Services { get; set; }
        public SelectList SelectCategories { get; set; }
        public SelectList ChildCategoryList { get; set; }
        public int? CatId { get; set; }
        public int? ChildId { get; set; }
        public string Name { get; set; }

        public ListServiceViewModel()
        {
            ChildCategoryList = new SelectList(new List<ServiceCategory>(), "Id", "CategoryName");
        }
    }
    public class InsertServiceViewModel
    {
        public Service Service { get; set; }
        public IEnumerable<ServiceCategory> Categories { get; set; }
        public SelectList SelectCategories { get; set; }
        public int? CategoryId { get; set; }
    }

    public class ServiceCatViewModel
    {
        public ServiceCategory ServiceCategory { get; set; }
        public SelectList SelectGroup { get; set; }
        public ServiceCatViewModel()
        {
            SelectGroup = new SelectList(new List<ServiceCategory>(), "Key", "Value");
        }
    }
}