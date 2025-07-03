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
    public class AuthControllerTest
    {
        private readonly AuthController _authController;
        private readonly Mock<Ilogin> _authServicemock;

        public AuthControllerTest()
        {
            _authServicemock = new Mock<Ilogin>();
            _authController = new AuthController( _authServicemock.Object );
        }

        [Fact]
        public async Task LoginAuth_data()
        {
            var logindata = new Login { username = "yogender", password = "123" };
            var expecteddata = new ResultModel<object>
            {
                Success = true,
                Data = new { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" },
                Message = "Login Successful"
            };
            _authServicemock.Setup(service => service.LoginAsync(It.IsAny<Login>())).ReturnsAsync(expecteddata);

            var result = await _authController.LoginAuthentication(logindata);
            

            var resultdata = Assert.IsType<OkObjectResult>(result);
            var resultdatatype = Assert.IsType<ResultModel<object>>(resultdata.Value);
            Assert.Equal("Login Successful", resultdatatype.Message);
        }

        [Fact]
        public async Task LoginAuth_Fail()
        {
            var logindata = new Login { username = "wronguser", password = "wrongpass" };
            var expecteddata = new ResultModel<object>
            {
                Success = false,
                Data = null,
                Message = "Invalid username and password"
            };
            _authServicemock.Setup(service => service.LoginAsync(It.IsAny<Login>())).ReturnsAsync(expecteddata);


            var result = await _authController.LoginAuthentication(logindata);


            var resultdata = Assert.IsType<OkObjectResult>(result);
            var resultdatatype = Assert.IsType<ResultModel<object>>(resultdata.Value);
            Assert.Equal("Invalid username and password", resultdatatype.Message);
        }

    }
}
