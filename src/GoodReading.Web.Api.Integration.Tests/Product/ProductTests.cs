using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Application.ResponseModels;
using GoodReading.Web.Api.Models;
using Newtonsoft.Json;
using Xunit;

namespace GoodReading.Web.Api.Integration.Tests.Product
{
    public class ProductTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public ProductTests(TestFixture fixture)
        {
            this._fixture = fixture;
            var token = GetToken();
            _fixture.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GetToken()
        {
            var rawResult = _fixture.HttpClient.GetAsync("api/Auth").Result;
            var stringResult = rawResult.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<DefaultResponse<string>>(stringResult)?.Response;
        }

        [Fact]
        public async Task Add_Product_Should_Return_Unauthorized()
        {
            // Arrange
            var product = new AddProductModel()
            {
                Name = "Test Product",
                Price = 1.2M,
                Quantity = 3
            };

            //Act
            var oldAuthorization = _fixture.HttpClient.DefaultRequestHeaders.Authorization;
            if (_fixture.HttpClient.DefaultRequestHeaders.Authorization != null)
                _fixture.HttpClient.DefaultRequestHeaders.Authorization = null;

            var rawResult = await _fixture.HttpClient.PostAsync("api/Product", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));

            _fixture.HttpClient.DefaultRequestHeaders.Authorization = oldAuthorization;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, rawResult.StatusCode);
        }

        [Fact]
        public async Task Add_Product_Should_Return_Bad_Request()
        {
            // Arrange
            var product = new AddProductModel()
            {
                Name = "Test Product"
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Product", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }

        [Fact]
        public async Task Add_Product_Successful_Should_Return_Product()
        {
            // Arrange
            var name = "Test Product for Test";
            var price = 15M;
            var quantity = 5;
            var product = new AddProductModel()
            {
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Product", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.Product>>(stringResult);

            var productResult = result?.Response;

            // Assert
            Assert.NotNull(productResult);
            Assert.NotNull(productResult.Id);
            Assert.Equal(name, productResult.Name);
            Assert.Equal(price, productResult.Price);
            Assert.Equal(quantity, productResult.Quantity);
            Assert.Equal(HttpStatusCode.Created, rawResult.StatusCode);
            Assert.NotNull(rawResult.Headers.Location);
        }

        [Fact]
        public async Task Get_Product_Should_Return_Product()
        {
            // Arrange
            var name = "Test Product for Test Get";
            var price = 15M;
            var quantity = 5;
            var product = new AddProductModel()
            {
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Product", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.Product>>(stringResult);

            var productId = result?.Response.Id;

            rawResult = await _fixture.HttpClient.GetAsync($"api/Product/{productId}");
            stringResult = await rawResult.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.Product>>(stringResult);

            var productResult = result?.Response;

            // Assert
            Assert.NotNull(productResult);
            Assert.Equal(productId, productResult.Id);
            Assert.Equal(HttpStatusCode.OK, rawResult.StatusCode);
        }

        [Fact]
        public async Task Get_Product_Should_Return_Bad_Request()
        {
            // Arrange
            var productId = "1ftest";

            //Act
            var rawResult = await _fixture.HttpClient.GetAsync($"api/Product/{productId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }

        [Fact]
        public async Task Update_Product_Should_Return_Bad_Request()
        {
            // Arrange
            var productId = "1ftest";
            var product = new AddProductModel()
            {
                Name = "Test Product"
            };

            //Act
            var rawResult = await _fixture.HttpClient.PutAsync($"api/Product/{productId}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }

        [Fact]
        public async Task Update_Product_Should_Return_Bad_Request_For_Id()
        {
            // Arrange
            var productId = "1ftest";
            var name = "Test Product for Test Get";
            var price = 15M;
            var quantity = 5;
            var product = new UpdateProductModel()
            {
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            //Act
            var rawResult = await _fixture.HttpClient.PutAsync($"api/Product/{productId}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }

        [Fact]
        public async Task Update_Product_Successful_Should_Return_Product()
        {
            // Arrange
            var name = "Test Product for Test Update";
            var price = 15M;
            var quantity = 5;
            var product = new AddProductModel()
            {
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Product", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.Product>>(stringResult);

            // Rearrange
            var productId = result?.Response.Id;
            var newName = "Test Product for Test Update - Updated";
            var newPrice = 5M;
            var newQuantity = 15;

            product.Name = newName;
            product.Price = newPrice;
            product.Quantity = newQuantity;
            // Rearrange

            rawResult = await _fixture.HttpClient.PutAsync($"api/Product/{productId}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            stringResult = await rawResult.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.Product>>(stringResult);

            var productResult = result?.Response;

            // Assert
            Assert.NotNull(productResult);
            Assert.Equal(productId, productResult.Id);
            Assert.Equal(newName, productResult.Name);
            Assert.Equal(newPrice, productResult.Price);
            Assert.Equal(newQuantity, productResult.Quantity);
            Assert.Equal(HttpStatusCode.OK, rawResult.StatusCode);
        }
    }
}
