using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cinemas.ModelViews
{
    public class UpdateProfileViewModel
    {

        public string? FullName { get; set; }


        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string? Email { get; set; }

        [Remote(action: "ValidatePhone", controller: "Accounts")]

        public DateOnly NgaySinh { get; set; }

        public bool? GioiTinh { get; set; }

        public string? DiaChi { get; set; }

    }
}
