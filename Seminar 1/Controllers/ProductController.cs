using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Hosting.Internal;
using Seminar_1.Models.Entities;
using Seminar_1.Models.VMs;

namespace Seminar_1.Controllers
{
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        private const string imgFolderName = "img";
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly Seminar1Context context;

        public ProductController(IWebHostEnvironment hostEnvironment, Seminar1Context context)
        {      
            this.hostEnvironment= hostEnvironment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList();
            return View(list);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            var product = new ProductVM();
            return View(product);
        }

        [HttpPost]
        [Route("New")]
        public IActionResult Create(ProductVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");           
                return View("New", dto);
            }

            SaveImage(dto);
            context.Add(ProductVM.VMProdToProd(dto));
            context.SaveChanges();

            return View("Index", context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList());
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var prod = context.Products.FirstOrDefault(p => p.Id == id);

            if (prod == null)
                return View("Index", context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList());
            else
                return View(new ProductVM().ProdToProdVM(prod));
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, ProductVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");               
                return View($"Edit/{id}", dto);
            }

            var prod = context.Products.FirstOrDefault(p => p.Id == id);
            if (prod == null)
                return View("Index", context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList());

            var oldFileRelativePath = prod.ImagePath;
            if (dto.ProducImage == null)
                dto.ImagePath = oldFileRelativePath;
            else
            {
                if (!string.IsNullOrWhiteSpace(oldFileRelativePath))
                {
                    var olfFileFullPath = Path.Combine(hostEnvironment.WebRootPath, oldFileRelativePath);
                    if (System.IO.File.Exists(olfFileFullPath))
                        System.IO.File.Delete(olfFileFullPath);
                }

                SaveImage(dto);
            }

            prod.Name = dto.Name;
            prod.Description = dto.Description;
            prod.Price = dto.Price;
            prod.IsAvailable = dto.IsAvailable;
            prod.ImagePath = dto.ImagePath;

            context.Products.Update(prod);
            context.SaveChanges();


            return View("Index", context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList());
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public JsonResult Delete(int id)
        {
            var prod = context.Products.FirstOrDefault(p => p.Id == id);
            if (prod == null)
                return Json(new { success = true, message = "Already Deleted" });

            if (!string.IsNullOrWhiteSpace(prod.ImagePath))
            {
                var filePath = Path.Combine(hostEnvironment.WebRootPath, prod.ImagePath);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            context.Products.Remove(prod);
            context.SaveChanges();

            return Json(new { success = true, message = "Delete success" });
        }

        private void SaveImage(ProductVM dto)
        {
            if (dto.ProducImage == null)
                return;

            var imgFolderPath = Path.Combine(hostEnvironment.WebRootPath, imgFolderName);

            if (!Directory.Exists(imgFolderPath))
                Directory.CreateDirectory(imgFolderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.ProducImage.FileName);
            var imgFullPath = Path.Combine(imgFolderPath, fileName);

            using (var fileStream = new FileStream(imgFullPath, FileMode.Create))
                dto.ProducImage.CopyTo(fileStream);

            dto.ImagePath = Path.Combine(imgFolderName, fileName);
        }         
    }
}
