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

namespace GoodReading.Web.Api.Integration.Tests.Customer
{
    public class CustomerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public CustomerTests(TestFixture fixture)
        {
            _fixture = fixture;
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
        public async Task Add_Customer_Should_Return_Unauthorized()
        {
            // Arrange
            var customer = new AddCustomerModel
            {
                Name = "Test Customer",
                Email = "testcutomer@test.com",
                Phone = "555 623 22 44",
                Address = "Test Address"
            };

            //Act
            var oldAuthorization = _fixture.HttpClient.DefaultRequestHeaders.Authorization;
            if (_fixture.HttpClient.DefaultRequestHeaders.Authorization != null)
                _fixture.HttpClient.DefaultRequestHeaders.Authorization = null;

            var rawResult = await _fixture.HttpClient.PostAsync("api/Customer", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));

            _fixture.HttpClient.DefaultRequestHeaders.Authorization = oldAuthorization;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, rawResult.StatusCode);
        }

        [Fact]
        public async Task Add_Customer_Should_Return_Bad_Request()
        {
            // Arrange
            var customer = new AddCustomerModel
            {
                Email = "testcutomer@test.com",
                Phone = "555 623 22 44"
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Customer", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }

        [Fact]
        public async Task Add_Customer_Successful_Should_Return_Customer()
        {
            // Arrange
            var name = "Test Customer";
            var email = "testcutomer@test.com";
            var phone = "555 623 22 44";
            var address = "Test Address";
            var customer = new AddCustomerModel
            {
                Name = name,
                Email = email,
                Phone = phone,
                Address = address
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Customer", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<CustomerDto>>(stringResult);

            var customerResult = result?.Response;

            // Assert
            Assert.NotNull(customerResult);
            Assert.NotNull(customerResult.Id);
            Assert.Equal(name, customerResult.Name);
            Assert.Equal(email, customerResult.Email);
            Assert.Equal(phone, customerResult.Phone);
            Assert.Equal(HttpStatusCode.Created, rawResult.StatusCode);
            Assert.NotNull(rawResult.Headers.Location);
            _fixture.AddedCustomerIds.Add(customerResult.Id);
        }

        [Fact]
        public async Task Get_Customer_Should_Return_Customer()
        {
            // Arrange
            var name = "Test Customer Get";
            var email = "testcutomerget@testget.com";
            var phone = "555 623 33 44";
            var address = "Test Address Get";
            var customer = new AddCustomerModel
            {
                Name = name,
                Email = email,
                Phone = phone,
                Address = address
            };
            string customerId;

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync("api/Customer", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<CustomerDto>>(stringResult);

            customerId = result?.Response.Id;

            rawResult = await _fixture.HttpClient.GetAsync($"api/Customer/{customerId}");
            stringResult = await rawResult.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<DefaultResponse<CustomerDto>>(stringResult);

            var customerResult = result?.Response;

            // Assert
            Assert.NotNull(customerResult);
            Assert.Equal(customerId, customerResult.Id);
            Assert.Equal(HttpStatusCode.OK, rawResult.StatusCode);
            _fixture.AddedCustomerIds.Add(customerResult.Id);
        }

        [Fact]
        public async Task Get_Customer_Should_Return_Bad_Request()
        {
            // Arrange
            var customerId = "1ftest";

            //Act
            var rawResult = await _fixture.HttpClient.GetAsync($"api/Customer/{customerId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }
    }
}
