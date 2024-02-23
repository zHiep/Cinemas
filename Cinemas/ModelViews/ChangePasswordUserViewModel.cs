using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cinemas.ModelViews
{
    public class ChangePasswordUserViewModel
    {
        [Key]
        public int IdKh {get; set;}

        [Display(Name = "OldPass")]
        [Remote(action: "ValidateOldPass", controller: "Accounts")]
        public string? OldPass { get; set;}

        [Display(Name = "NewPass")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(6,ErrorMessage = "Mật khẩu tối thiểu 6 kí tự")]
        public string? NewPass { get; set;}

        [Display(Name = "ReNewPass")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự")]
        [Compare("NewPass",ErrorMessage = "Mật khẩu không trùng khớp")]
        public string? ReNewPass { get; set; }

    }
}
