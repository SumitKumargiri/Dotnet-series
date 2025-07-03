using Microsoft.AspNetCore.Mvc;
using practicecrudmvc.Data;
using practicecrudmvc.Models;
using System.Diagnostics;

namespace practicecrudmvc.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

       public async Task<IActionResult> index()
        {
            return View();
        }

        
    }
}
