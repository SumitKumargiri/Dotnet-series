using FactoryDesignPattern.Interface;
using FactoryDesignPattern.Model;
using Microsoft.AspNetCore.Mvc;

namespace FactoryDesignPattern.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly ICrud _crudService;

        public CrudController(ICrudFactory crudServiceFactory)
        {
            _crudService = crudServiceFactory.CreateCrudService("crud");
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> Getdata()
        {
            var result = await _crudService.GetAsync();
            return Ok(result);
        }

        [HttpPost("insertdata")]
        public async Task<IActionResult> Insertdata(Crud crud)
        {
            var result = await _crudService.CheckInsertAsync(crud);
            return Ok(result);
        }
    }
}
