using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Sepet.VMClasses
{
    public class UserVM
    {
        [Required(ErrorMessage ="Kullanıcı adı boş geçilemez")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string Password { get; set; }

    }
}