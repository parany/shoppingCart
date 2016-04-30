using System.Xml.Serialization;

namespace ShoppingCart.Models.Models.Payments
{
    [XmlInclude(typeof(Mvola))]
    [XmlInclude(typeof(Check))]
    [XmlInclude(typeof(PaymentCash))]
    public abstract class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
