using System;

namespace GoodReading.Web.Api.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
