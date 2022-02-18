using System.Collections.Generic;

namespace GoodReading.Web.Api.Models
{
    public class AddCustomerOrderModel
    {
        public List<AddCustomerOrderProductModel> Products { get; set; }
    }

    public class AddCustomerOrderProductModel
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
