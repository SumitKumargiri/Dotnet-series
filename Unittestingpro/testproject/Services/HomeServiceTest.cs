using Xunit;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Unittestingpro.Model;
using Unittestingpro.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using Unittestingpro.utility;
using Dapper;

namespace testproject.Services
{
    public class HomeServiceTest
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<DBGateway> _dBGatewayMock;
        private readonly HomeService _homeService;
        private readonly string _cacheKey = "data";

        public HomeServiceTest()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _dBGatewayMock = new Mock<DBGateway>();
            _homeService = new HomeService("ConnectionString", _cacheMock.Object);
        }

        [Fact]
        public async Task InsertdataAsync_test()
        {
            var newHome = new home { name = "Sumit", email = "sumit@gmail.com", country = "India" };
            _dBGatewayMock.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>())).ReturnsAsync(1);

            var result = await _homeService.InsertdataAsync(newHome);

            Assert.True(result.Success);
            Assert.Equal("Insertdata Successfully", result.Message);
            _cacheMock.Verify(c => c.RemoveAsync(_cacheKey, default), Times.Once);
        }

        [Fact]
        public async Task UpdatedataAsync_test()
        {
            var updatedHome = new home { id = 1, name = "Sumit", email = "sumit123@gmail.com", country = "India" };
            _dBGatewayMock.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>())).ReturnsAsync(1);

            var result = await _homeService.UpdatedataAsync(updatedHome);

            Assert.True(result.Success);
            Assert.Equal("Data Update Successfully", result.Message);
            _cacheMock.Verify(c => c.RemoveAsync(_cacheKey, default), Times.Once);
        }

        [Fact]
        public async Task DeletedataAsync_test()
        {
            _dBGatewayMock.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>())).ReturnsAsync(1);

            var result = await _homeService.DeletedataAsync(1);

            Assert.True(result.Success);
            Assert.Equal("Data Deleted Successfully", result.Message);
            _cacheMock.Verify(c => c.RemoveAsync(_cacheKey, default), Times.Once);
        }
    }
}
