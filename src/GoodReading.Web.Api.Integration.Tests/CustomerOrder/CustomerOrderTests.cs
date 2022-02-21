using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Application.CustomerOrder.Queries.GetCustomerOrder;
using GoodReading.Application.CustomerOrder.Queries.GetCustomerOrders;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Exceptions;
using GoodReading.Web.Api.Models;
using Newtonsoft.Json;
using Xunit;

namespace GoodReading.Web.Api.Integration.Tests.CustomerOrder
{
    public class CustomerOrderTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public CustomerOrderTests(TestFixture fixture)
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
        public async Task Add_CustomerOrder_Should_Return_Unauthorized()
        {
            // Arrange
            var customerId = "620fbede45274bf9c0524abb";
            var orderModel = new AddCustomerOrderModel()
            {
                Products = new List<AddCustomerOrderProductModel>
                {
                    new AddCustomerOrderProductModel{Id = "620fe6dca8354573ae6b4e34", Quantity = 1}
                }
            };

            //Act
            var oldAuthorization = _fixture.HttpClient.DefaultRequestHeaders.Authorization;
            if (_fixture.HttpClient.DefaultRequestHeaders.Authorization != null)
                _fixture.HttpClient.DefaultRequestHeaders.Authorization = null;

            var rawResult = await _fixture.HttpClient.PostAsync($"api/CustomerOrder/{customerId}", new StringContent(JsonConvert.SerializeObject(orderModel), Encoding.UTF8, "application/json"));

            _fixture.HttpClient.DefaultRequestHeaders.Authorization = oldAuthorization;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, rawResult.StatusCode);
        }

        [Fact]
        public async Task Add_CustomerOrder_Should_Return_Bad_Request()
        {
            // Arrange
            var customerId = "620fbede45274bf9c0524abb";
            var orderModel = new AddCustomerOrderModel()
            {
                Products = new List<AddCustomerOrderProductModel>()
            };

            //Act
            var rawResult = await _fixture.HttpClient.PostAsync($"api/CustomerOrder/{customerId}", new StringContent(JsonConvert.SerializeObject(orderModel), Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, rawResult.StatusCode);
        }

        private async Task<CustomerDto> AddCustomerForOrder(AddCustomerModel customer)
        {
            var rawResult = await _fixture.HttpClient.PostAsync("api/Customer", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DefaultResponse<CustomerDto>>(stringResult)?.Response;
        }

        private async Task<Domain.Entities.Product> AddProductForOrder(AddProductModel product)
        {
            var rawResult = await _fixture.HttpClient.PostAsync("api/Product", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.Product>>(stringResult)?.Response;
        }

        [Fact]
        public async Task Add_CustomerOrder_Successful_Return_CustomerOrder()
        {
            // Arrange
            var customer = new AddCustomerModel
            {
                Name = "Test Customer for CustomerOrder Test",
                Email = "testcutomerCustomerOrder@test.com",
                Phone = "555 623 22 99",
                Address = "Test Address"
            };
            var product = new AddProductModel()
            {
                Name = "Test Product for CustomerOrder Test",
                Price = 3M,
                Quantity = 8,
            };
            var product2 = new AddProductModel()
            {
                Name = "Test Product for CustomerOrder Test2",
                Price = 5M,
                Quantity = 2,
            };
            var productQuantity = 1;
            var product2Quantity = 2;

            //Act
            var customerResult = await AddCustomerForOrder(customer);
            var productResult = await AddProductForOrder(product);
            var productResult2 = await AddProductForOrder(product2);

            var orderModel = new AddCustomerOrderModel()
            {
                Products = new List<AddCustomerOrderProductModel>
                {
                    new AddCustomerOrderProductModel{Id = productResult?.Id, Quantity = productQuantity},
                    new AddCustomerOrderProductModel{Id = productResult2?.Id, Quantity = product2Quantity}
                }
            };
            var totalAmount = (productResult?.Price * productQuantity) + (productResult2?.Price * product2Quantity);

            var rawResult = await _fixture.HttpClient.PostAsync($"api/CustomerOrder/{customerResult.Id}", new StringContent(JsonConvert.SerializeObject(orderModel), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.CustomerOrder>>(stringResult)?.Response;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalAmount, result.TotalPrice);
            Assert.Equal(customerResult.Id, result.CustomerId);
            Assert.Equal(HttpStatusCode.Created, rawResult.StatusCode);
            Assert.NotNull(rawResult.Headers.Location);
            _fixture.AddedCustomerIds.Add(customerResult.Id);
            _fixture.AddedProductIds.Add(productResult.Id);
            _fixture.AddedProductIds.Add(productResult2.Id);
            _fixture.AddedCustomerOrderIds.Add(result.Id);
        }

        [Fact]
        public async Task Get_Customer_Orders_Should_Return_Orders_List()
        {
            // Arrange
            var customer = new AddCustomerModel
            {
                Name = "Test Customer for CustomerOrder Test Get Orders",
                Email = "testcutomerCustomerOrderGetOrders@test.com",
                Phone = "555 623 22 35",
                Address = "Test Address"
            };
            var product = new AddProductModel()
            {
                Name = "Test Product for CustomerOrder Test Get Orders",
                Price = 3M,
                Quantity = 8,
            };
            var productQuantity = 3;

            //Act
            var customerResult = await AddCustomerForOrder(customer);
            var productResult = await AddProductForOrder(product);

            var orderModel = new AddCustomerOrderModel()
            {
                Products = new List<AddCustomerOrderProductModel>
                {
                    new AddCustomerOrderProductModel{Id = productResult?.Id, Quantity = productQuantity}
                }
            };
            var orderModel2 = new AddCustomerOrderModel()
            {
                Products = new List<AddCustomerOrderProductModel>
                {
                    new AddCustomerOrderProductModel{Id = productResult?.Id, Quantity = (productQuantity + 1)}
                }
            };
            await _fixture.HttpClient.PostAsync($"api/CustomerOrder/{customerResult.Id}", new StringContent(JsonConvert.SerializeObject(orderModel), Encoding.UTF8, "application/json"));
            await _fixture.HttpClient.PostAsync($"api/CustomerOrder/{customerResult.Id}", new StringContent(JsonConvert.SerializeObject(orderModel2), Encoding.UTF8, "application/json"));

            var rawResult = await _fixture.HttpClient.GetAsync($"api/CustomerOrder/{customerResult.Id}");
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<List<Domain.Entities.CustomerOrder>>>(stringResult)?.Response;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(HttpStatusCode.OK, rawResult.StatusCode);
            _fixture.AddedCustomerIds.Add(customerResult.Id);
            _fixture.AddedProductIds.Add(productResult.Id);
            _fixture.AddedCustomerOrderIds.AddRange(result.Select(r => r.Id));
        }

        [Fact]
        public async Task Get_Customer_Orders_Should_Return_Order()
        {
            // Arrange
            var customer = new AddCustomerModel
            {
                Name = "Test Customer for CustomerOrder Test Get Order",
                Email = "testcutomerCustomerOrderGetOrder@test.com",
                Phone = "555 623 22 32",
                Address = "Test Address"
            };
            var product = new AddProductModel()
            {
                Name = "Test Product for CustomerOrder Test Get Order",
                Price = 3M,
                Quantity = 8,
            };
            var productQuantity = 3;

            //Act
            var customerResult = await AddCustomerForOrder(customer);
            var productResult = await AddProductForOrder(product);

            var orderModel = new AddCustomerOrderModel()
            {
                Products = new List<AddCustomerOrderProductModel>
                {
                    new AddCustomerOrderProductModel{Id = productResult?.Id, Quantity = productQuantity}
                }
            };
            var totalAmount = productResult?.Price * productQuantity;
            var rawResult = await _fixture.HttpClient.PostAsync($"api/CustomerOrder/{customerResult.Id}", new StringContent(JsonConvert.SerializeObject(orderModel), Encoding.UTF8, "application/json"));
            var stringResult = await rawResult.Content.ReadAsStringAsync();
            var orderResult = JsonConvert.DeserializeObject<DefaultResponse<Domain.Entities.CustomerOrder>>(stringResult)?.Response;

            rawResult = await _fixture.HttpClient.GetAsync($"api/CustomerOrder/{customerResult.Id}/{orderResult?.Id}");
            stringResult = await rawResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DefaultResponse<GetCustomerOrderModel>>(stringResult)?.Response;


            Assert.NotNull(result);
            Assert.NotEmpty(result.Products);
            Assert.Collection(result.Products, model =>
            {
                Assert.Equal(productResult?.Price, model.Price);
                Assert.Equal(productResult?.Id, model.Id);
                Assert.Equal(productQuantity, model.Quantity);
            });
            Assert.Equal(totalAmount, result.TotalPrice);
            Assert.Equal(customerResult.Id, result.CustomerId);
            Assert.Equal(customerResult.Name, result.CustomerName);
            Assert.Equal(customerResult.Email, result.CustomerEmail);
            Assert.Equal(customerResult.Phone, result.CustomerPhone);
            Assert.Equal(orderResult?.Id, result.CustomerOrderId);
            _fixture.AddedCustomerIds.Add(customerResult.Id);
            _fixture.AddedProductIds.Add(productResult.Id);
            _fixture.AddedCustomerOrderIds.Add(result.CustomerOrderId);
        }

        [Fact]
        public async Task Get_Customer_Orders_Should_Return_Bad_Request()
        {
            var command = new GetCustomerOrdersQuery
            {
            };
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _fixture.Mediator.Send(command));
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task Get_Customer_Order_Should_Return_Bad_Request()
        {
            var command = new GetCustomerOrderQuery
            {
            };
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _fixture.Mediator.Send(command));
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task Get_Customer_Order_Should_Return_Bad_Request_For_Customer()
        {
            var command = new GetCustomerOrderQuery
            {
                OrderId = "551137c2f9e1fac808a5f572"
            };
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _fixture.Mediator.Send(command));
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task Get_Customer_Order_Should_Return_Bad_Request_For_Order()
        {
            var command = new GetCustomerOrderQuery
            {
                CustomerId = "551137c2f9e1fac808a5f572"
            };
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _fixture.Mediator.Send(command));
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }
    }
}
