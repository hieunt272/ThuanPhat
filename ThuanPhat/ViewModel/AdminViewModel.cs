﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ThuanPhat.Models;
using PagedList;

namespace ThuanPhat.ViewModel  
{
    public class ChangePasswordModel
    {
        [Display(Name = "Mật khẩu hiện tại"), Required(ErrorMessage = "Hãy nhập mật khẩu hiện tại"), UIHint("Password")]
        public string OldPassword { get; set; }
        [Display(Name = "Mật khẩu mới"), Required(ErrorMessage = "Hãy nhập mật khẩu mới"),
         StringLength(16, MinimumLength = 4, ErrorMessage = "Mật khẩu từ 4, 16 ký tự"), UIHint("Password")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu"), System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác"),
         UIHint("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AdminLoginModel
    {
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }
    }
    public class InfoAdminViewModel
    {
        public int Articles { get; set; }
        public int Contacts { get; set; }
        public int Admins { get; set; }
        public int Banners { get; set; }
        public int Feedbacks { get; set; }
        public int Services { get; set; }
        public int Recruits { get; set; }
    }
    public class EditAdminViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Bạn chưa điền tên đăng nhập"), StringLength(20, MinimumLength = 4, ErrorMessage = "Tên đăng nhập từ 4 - 20 ký tự"), RegularExpression(@"[a-zA-Z0-9]+$", ErrorMessage = "Chấp nhận chữ cái không dấu, số, viết liền không khoảng trống"), UIHint("TextBox")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), UIHint("Password"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string Password { get; set; }
    }
}