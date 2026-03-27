using laba_3_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace laba_3_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CompanyModelContainer db = new CompanyModelContainerWithLazyLoad();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            SelectList departments = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            ViewBag.Departments = departments;


            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            Console.WriteLine(employee.Name + " " + employee.DepartmentId);
            db.Workers.Add(employee);
            db.SaveChanges();
            @ViewBag.Greeting = "Пользователь создан";
            SelectList departments = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            ViewBag.Departments = departments;
            return View();
        }
        [HttpGet]
        public IActionResult Entry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entry(string name)
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine(name);
                int count = Queries.SearchByName(name);
                if (count > 0)
                {
                    ViewBag.Greeting = "Успех";
                    return Redirect("Main");
                }
                else
                {
                    ViewBag.Greeting = "Пользователь не найден";
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Main()
        {

            //var workers = db.Workers.Include(d => d.Department).ToList();
            var workers = from x in db.Workers join y in db.Departments on x.DepartmentId equals y.DepartmentId select x;
            return View("Main", workers);
        }

        [HttpPost]
        public IActionResult Main(string query)
        {
            Console.WriteLine("A");
            Console.WriteLine(query);
            if(query == "Order_by_DepName")
            {
                var workers1 = from x in db.Workers join y in db.Departments on x.DepartmentId equals y.DepartmentId  orderby y.DepartmentName select x;
                //var workers1 = db.Workers.Include(d => d.Department).OrderBy(d => d.Department.DepartmentName).ToList();
                return View( workers1);
            }
            if (query == "ALL_A")
            {
                var workers2 = from x in db.Workers
                               join y in db.Departments on x.DepartmentId equals y.DepartmentId
                               where x.Name.StartsWith("A")
                               orderby y.DepartmentName 
                               select x;
                //var workers2 = db.Workers.Include(d => d.Department).Where(e => e.Name.StartsWith("A")).OrderBy(d => d.Department.DepartmentName).ToList();
                //var workers2 = db.Workers.Include(d => d.Department);
                //var workers3 = (from x in workers2
                //               group x by x.Department into temp
                //               where temp.All(t => t.Name.StartsWith("A")) select temp).ToList();

                return View(workers2);
            }
            var workers = db.Workers.Include(d => d.Department).ToList();
            return View(workers);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}