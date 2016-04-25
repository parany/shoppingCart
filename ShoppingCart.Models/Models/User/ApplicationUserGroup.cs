using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Models.User
{
    public class ApplicationUserGroup
    {
        [Required]
        public virtual string UserId { get; set; }
        [Required]
        public virtual int GroupId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }
}
