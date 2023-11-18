using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Seminar_1.Models.VMs;

namespace Seminar_1.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly Seminar1Context context;

        public UserController(Seminar1Context context)
        {
            this.context = context;
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            var list = context.Users.Select(p => new UserVM().UserToUserVM(p)).ToList();
            return View(list);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            var user = new UserVM();
            return View(user);
        }

        [HttpPost]
        [Route("New")]
        public IActionResult Create(UserVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");
                return View("New", dto);
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Password and ConfirmPassword doesn't match");
                return View("New", dto);
            }

            dto.Password=Base64.Base64Encode(dto.ConfirmPassword);
            
            context.Users.Add(UserVM.VMUserToUser(dto));
            context.SaveChanges();

            return View("Index", context.Users.Select(p => new UserVM().UserToUserVM(p)).ToList());
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public JsonResult Delete(int id)
        {
            var user = context.Users.FirstOrDefault(p => p.Id == id);
            if (user == null)
                return Json(new { success = true, message = "Already Deleted" });

         

            context.Users.Remove(user);
            context.SaveChanges();

            return Json(new { success = true, message = "Delete success" });
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = context.Users.FirstOrDefault(p => p.Id == id);

            if (user == null)
                return View("Index", context.Users.Select(p => new UserVM().UserToUserVM(p)).ToList());
            else
                return View(new UserVM().UserToUserVM(user));
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, UserVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");
                return View($"Edit/{id}", dto);
            }

            var user = context.Users.FirstOrDefault(p => p.Id == id);
            if (user == null)
                return View("Index", context.Users.Select(p => new UserVM().UserToUserVM(p)).ToList());

            
            user.UserName = dto.UserName;
            user.SurName = dto.SurName;
            user.Name = dto.Name;
            

            context.Users.Update(user);
            context.SaveChanges();


            return View("Index", context.Users.Select(p => new UserVM().UserToUserVM(p)).ToList());
        }

    }
}
