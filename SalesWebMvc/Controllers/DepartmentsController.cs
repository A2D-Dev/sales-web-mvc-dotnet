using SalesWebMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> list = new List<Department>();

            list.Add(new Department { Id = 1, Name = "Eletrônicos" });
            list.Add(new Department { Id = 2, Name = "Moda" });

            return View(list);
        }
    }
}
