using CoreLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly ICrud _crudService;

        public CrudController(ICrud crudService)
        {
            _crudService = crudService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var result = await _crudService.GetDataAsync();
            return Ok(result.Data);
        }
    }

}
