using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Models.User
{
    public class Group
    {
        public Group() { }

        public Group(string name) : this()
        {
            this.Roles = new List<ApplicationRoleGroup>();
            this.Name = name;
        }

        [Key]
        [Required]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<ApplicationRoleGroup> Roles { get; set; }

    }
}
