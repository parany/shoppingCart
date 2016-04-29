using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Models.User
{
    public class ApplicationRoleGroup
    {
        public virtual string RoleId { get; set; }
        public virtual int GroupId { get; set; }
        public virtual ApplicationRole Role { get; set; }
        public virtual Group Group { get; set; }
    }
}
