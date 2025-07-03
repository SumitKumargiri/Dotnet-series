using Dllprocess.Controllers;
using Dllprocess.Interface;
using Dllprocess.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace testpro.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CrudControllerTest : ControllerBase
    {
        private readonly CrudController _crudController;
        private readonly Mock<ICrud> _mockCrudService;

        public CrudControllerTest()
        {
            _mockCrudService = new Mock<ICrud>();
            _crudController = new CrudController(_mockCrudService.Object);
        }

        [Fact]
        public async Task Getdata_Success()
        {
            var data = new ResultModel<object>
            {
                Success = true,
            };
            _mockCrudService.Setup(service => service.GetAsync()).ReturnsAsync(data);

            var result = await _crudController.Getdata();

            var finalresult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResultModel<object>>(finalresult.Value);
            Assert.True(returnValue.Success);
        }


        [Fact]
        public async Task Getdata_through_Exception()
        {
            _mockCrudService.Setup(service => service.GetAsync()).ThrowsAsync(new Exception("An error occurred"));

            var result = await _crudController.Getdata();

            var status = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, status.StatusCode);
            Assert.Equal("An error occurred", status.Value);
        }


        [Fact]
        public async Task Insertdata_Success()
        {
            var querydata = new Crud { name = "Amit", email = "amit78@gmail.com", country = "India" };
            var data = new ResultModel<object>
            {
                Success = true,
                Data = querydata,
                Message = "Data inserted successfully."
            };
            _mockCrudService.Setup(service => service.CheckInsertAsync(querydata)).ReturnsAsync(data);

            var result = await _crudController.Insertdata(querydata);

            var finalresult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResultModel<object>>(finalresult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal("Data inserted successfully.", returnValue.Message);
        }

        [Fact]
        public async Task UpdateData_Success()
        {
            var query = new Crud { id = 1, name = "sumit", email = "sumit895@gmail.com", country = "UK" };
            var data = new ResultModel<object>
            {
                Success = true,
                Data = query
            };
            _mockCrudService.Setup(service => service.UpdateAsync(query)).ReturnsAsync(data);

            var result = await _crudController.UpdateData(query);

            var finalresult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResultModel<object>>(finalresult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal(query, returnValue.Data);
        }

        [Fact]
        public async Task DeleteData_Success()
        {
            int id = 1;
            var data = new ResultModel<object>
            {
                Success = true,
                Message = "Data deleted successfully."
            };
            _mockCrudService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(data);

            var result = await _crudController.DeleteData(id);

            var finalresult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResultModel<object>>(finalresult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal("Data deleted successfully.", returnValue.Message);
        }

    }
}

