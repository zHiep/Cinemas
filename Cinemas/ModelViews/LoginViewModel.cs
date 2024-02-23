using System.ComponentModel.DataAnnotations;

namespace Cinemas.ModelViews
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Địa chỉ Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Vui lòng nhập lại Email")]
        public string? Email { get; set; }


        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        public string? Password { get; set; }
    }
}
