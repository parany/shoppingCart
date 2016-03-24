﻿using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.BackOffice
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IDisposable
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store) : base(store) {
        }
        public static ApplicationRoleManager Create( IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new
            RoleStore<ApplicationRole>(context.Get<ShoppingCartDbContext>()));
        }
    }
}