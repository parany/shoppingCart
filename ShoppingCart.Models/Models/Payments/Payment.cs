﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.Models.Models.Payments
{
    [XmlInclude(typeof(Mvola))]
    [XmlInclude(typeof(Check))]
    [XmlInclude(typeof(PaymentCash))]
    public abstract class Payment : BaseObject
    {
        public string Name { get; set; }
    }
}
