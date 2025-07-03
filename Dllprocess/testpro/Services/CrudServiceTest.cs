using Dapper;
using Dllprocess.Model;
using Dllprocess.Services;
using Dllprocess.utility;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dllprocess.Tests
{
    public class CrudServiceTests
    {
        private readonly Mock<DBGateway> _mockDBGateway;
        private readonly CrudService _crudService;

        public CrudServiceTests()
        {
            _mockDBGateway = new Mock<DBGateway>("ConnectionString1");
            _crudService = new CrudService("ConnectionString1");
        }

        [Fact]
        public async Task GetAsync_test()
        {
            var mockData = new List<Crud>
            {
                new Crud { name = "sumit", email = "sumit1@gmail.com", country = "India" }
            };
            _mockDBGateway.Setup(db => db.ExeQueryList<Crud>("select *from crud", null)).ReturnsAsync(mockData);

            var result = await _crudService.GetAsync();

            Assert.NotNull(result);
            Assert.Equal("Data received", result.Message);
        }

    }
}
