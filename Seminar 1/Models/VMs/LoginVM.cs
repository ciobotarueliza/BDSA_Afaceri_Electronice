using System.ComponentModel.DataAnnotations;

namespace Seminar_1.Models.VMs
{
  
        public class LoginVM
        {
            public LoginVM()
            {
                UserName = string.Empty;
                Password = string.Empty;
            }

            [Required(AllowEmptyStrings = false)]
            public string UserName { get; set; }
            [Required(AllowEmptyStrings = false)]
            public string Password { get; set; }
        }
    }
