using ShoppingCart.Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Log
{
    public enum ChangeType
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
    }

    public class ChangeTracking : BaseObject
    {
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public Guid PrimaryKey { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Description { get; set; }
        public ChangeType Type { get; set; }
        public DateTime DateChanged { get; set; }
    }
}

