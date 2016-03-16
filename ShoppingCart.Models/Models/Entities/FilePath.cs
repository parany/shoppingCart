using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Models.Entities
{
    public class FilePath : BaseObject
    {
        [StringLength(255)]
        public string FileName { get; set; }

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
