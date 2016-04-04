using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Models.Payments;

namespace ShoppingCart.Models.Models.Entities
{
    public enum DeliveryMethod
    {
        Delivered = 0,
        NotDelivered = 1
    }
    class Provider : BaseObject
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Payment> PaymentMethods {get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}
