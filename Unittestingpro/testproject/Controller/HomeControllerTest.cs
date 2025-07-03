using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unittestingpro.Controllers;
using Unittestingpro.Interface;
using Unittestingpro.Model;

namespace testproject.Controller
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<Ihome> _homeServicemock;
        private object _homeServiceMock;

        public HomeControllerTest()
        {
            _homeServicemock = new Mock<Ihome>();
            _controller = new HomeController(_homeServicemock.Object);
        }

        /// <summary>
        /// Test case for Getdata method
        /// </summary>


        // [Fact]
        [Theory]
        [InlineData(true,"Data received Successfully")]
        [InlineData(false,"Not received data")]
        public async Task Getdata_test(bool success,string message)
        {
            var result = new ResultModel<object>
            {
                Data = success? new List<home>():null,
                Success = success,
                Message = message
            };
        _homeServicemock.Setup(service=>service.GetAsync()).ReturnsAsync(result);

            var actresult = await _controller.Getdata();

            Assert.NotNull(actresult);
        }

        /// <summary>
        /// Test case for Getdata method
        /// </summary>

        [Fact]
        public async Task Getdatanot_found()
        {
            var result = new ResultModel<object>
            {
                Data = new List<home>(),
                Success = true,
                Message = "Data not found"
            };
            _homeServicemock.Setup(service=>service.GetAsync()).ReturnsAsync(result);

            var actresult = await _controller.Getdata();

            Assert.NotNull(actresult);
        }


        /// <summary>
        /// Test case for Insertdata method
        /// </summary>

        // [Fact]
        [Theory]
        [InlineData("sumit","sumitgiri154@gmail.com","India",true,"Data Inserted Successfully")]
        [InlineData("","ankit189@gmail.com","USA",false,"Data insert Failed")]
        [InlineData("Aman","aman275@.com","India",false, "Data insert Failed")]
        public async Task Insertdata_test(string name,string email,string country,bool success,string message)
        {
            var newinsertdata = new home { name = name, email = email, country = country };
            var mockresult = new ResultModel<object> { Success = success, Message = message };
            _homeServicemock.Setup(service => service.InsertdataAsync(newinsertdata)).ReturnsAsync(mockresult);


            var result = await _controller.InsertData(newinsertdata);

            var getresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<ResultModel<object>>(getresult.Value);
            Assert.Equal(message, actualresult.Message);
        }

        /// <summary>
        /// Test case for Updatedata method
        /// </summary>

        [Fact]
        public async Task Updatedata_test()
        {
            var updatedata = new home { name = "Amit", email = "amit789@gmail.com", country = "USA" };
            var mockresult = new ResultModel<object> { Success = true, Message = "Update Data Successfully" }; 
            _homeServicemock.Setup(service => service.UpdatedataAsync(updatedata)).ReturnsAsync(mockresult);

            var result = await _controller.UpdateData(updatedata);

            var getresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<ResultModel<object>>(getresult.Value);
            Assert.Equal("Update Data Successfully", actualresult.Message); 
        }


        /// <summary>
        /// Test case for DeleteData method
        /// </summary>

        [Fact]
        public async Task Deletedata_test()
        {
            var mockresult = new ResultModel<object> { Success = true, Message = "Data Deleted Successfully" };
            _homeServicemock.Setup(Service => Service.DeletedataAsync(It.IsAny<int>())).ReturnsAsync(mockresult);


            var result = await _controller.DeleteData(1);

            var getresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<ResultModel<object>>(getresult.Value);
            Assert.Equal("Data Deleted Successfully", actualresult.Message);
        }


        // [Fact]
        [Theory]
        [InlineData("Nitish",false,"Data Not Found")]
        [InlineData("",false,"Not Received Data")]
        public async Task Getdataname_Notfound(string name,bool success,string message)
        {
            //var name = "Nilesh";
            var resultModel = new ResultModel<object>
            {
                Data = null,
                Success = success,
                Message = message
            };
            _homeServicemock.Setup(service => service.GetDatabyname(name)).ReturnsAsync(resultModel);


            var result = await _controller.Getdataname(name);

            var getresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<ResultModel<object>>(getresult.Value);
            Assert.False(actualresult.Success);
            Assert.Equal(message, actualresult.Message);
        }
    }
}
