using Seminar_1.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Seminar_1.Models.VMs
{
    public class UserVM
    {
        public UserVM()
        {
            UserName = string.Empty;
            Password = string.Empty;
            SurName = string.Empty;
            Name = string.Empty;
        }

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]

        public string SurName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Name { get; set; }

        public DateTime? LastLogin { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string ConfirmPassword { get; set; }


        public static User VMUserToUser(UserVM vm)
        {
            var user = new User();

            user.UserName = vm.UserName;
            user.Password = vm.Password;
            user.SurName = vm.SurName;
            user.Name = vm.Name;
   

            return user;
        }

        public UserVM UserToUserVM(User? user)
        {
            if (user == null)
                return new UserVM();

            var vm = new UserVM();

            vm.Id = user.Id;
            vm.Name = user.Name;
            vm.Password = user.Password;
            vm.UserName = user.UserName;
            vm.SurName = user.SurName;
         

            return vm;
        }
    }
}
