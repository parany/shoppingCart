﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Entities.Shopping;

namespace ShoppingCart.Models
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext() : base("name=ShoppingCartDb")
        {
        }

        public DbSet<Product> Products { get; set; }
        
        public DbSet<Category> Categories { get; set; } 
    }
}
