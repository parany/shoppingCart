﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ShoppingCart.Models.Models.Payments
{
    public class Payments
    {
        public Payments()
        {
            this.Modules = new List<Payment>();
        }

        [XmlArrayItem("Mvola", typeof(Mvola))]
        [XmlArrayItem("Check", typeof(Check))]
        [XmlArrayItem("PaymentCash", typeof(PaymentCash))]
        public List<Payment> Modules { get; set; }

       public void InitPaymentsList(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Payments));
            using (StreamReader rd = new StreamReader(path))
            {
                Payments p = xs.Deserialize(rd) as Payments;

                foreach (Payment pay in p.Modules)
                {
                    this.Modules.Add(pay);
                }
            }
        }

        public void InitPaymentsListFromResourceString(string payment)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Payments));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(payment));
            Payments p = xs.Deserialize(memStream) as Payments;

            foreach (Payment pay in p.Modules)
            {
                this.Modules.Add(pay);
            }            
        }
    }
}
