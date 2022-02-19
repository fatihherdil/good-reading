using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GoodReading.Application.ResponseModels;
using Newtonsoft.Json;
using Xunit;

namespace GoodReading.Web.Api.Integration.Tests.Auth
{
    public class TokenTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public TokenTests(TestFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async Task Get_Token()
        {
            var rawResult = await _fixture.HttpClient.GetAsync("api/Auth");
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse>(stringResult).Response;

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.Created, rawResult.StatusCode);
        }
    }
}
