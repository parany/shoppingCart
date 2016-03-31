using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShoppingCart.Models.Models.Payments
{
    [XmlInclude(typeof(Mvola))]
    [XmlInclude(typeof(Check))]
    [XmlInclude(typeof(PaymentCash))]
    public abstract class Payment
    {
        public string Name { get; set; }
    }
}
