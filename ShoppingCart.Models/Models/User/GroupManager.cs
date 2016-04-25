﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.Models.User
{
    public class GroupManager
    {
        ShoppingCartDbContext _db;
        RoleManager<ApplicationRole> _roleManager;
        UserManager<ApplicationUser> _userManager;

        public GroupManager(ShoppingCartDbContext context)
        {
            _db = context;
            _roleManager = new RoleManager<ApplicationRole>(
                new RoleStore<ApplicationRole>(new ShoppingCartDbContext()));
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ShoppingCartDbContext()));
        }


        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }


        public bool CreateRole(string name, string description = "")
        {
            // Swap ApplicationRole for IdentityRole:
            var idResult = _roleManager.Create(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var idResult = _userManager.Create(user, password);
            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var idResult = _userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var user = _userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                var currentrole = _roleManager.FindById(role.RoleId);
                _userManager.RemoveFromRole(userId, currentrole.Name);
            }
        }

        public void CreateGroup(string groupName)
        {
            if (this.GroupNameExists(groupName))
            {
                throw new System.Exception("A group by that name already exists in the database. Please choose another name.");
            }

            var newGroup = new Group(groupName);
            _db.Groups.Add(newGroup);
            _db.SaveChanges();
        }


        public bool GroupNameExists(string groupName)
        {
            var g = _db.Groups.Where(gr => gr.Name == groupName);
            if (g.Count() > 0)
            {
                return true;
            }
            return false;
        }


        public void ClearUserGroups(string userId)
        {
            this.ClearUserRoles(userId);
            var user = _db.Users.Find(userId);
            user.Groups.Clear();
            _db.SaveChanges();
        }


        public void AddUserToGroup(string userId, int GroupId)
        {
            var group = _db.Groups.Find(GroupId);
            var user = _db.Users.Find(userId);

            var userGroup = new ApplicationUserGroup()
            {
                Group = group,
                GroupId = group.Id,
                User = user,
                UserId = user.Id
            };

            foreach (var role in group.Roles)
            {
                _userManager.AddToRole(userId, role.Role.Name);
            }
            user.Groups.Add(userGroup);
            _db.SaveChanges();
        }


        public void ClearGroupRoles(int groupId)
        {
            var group = _db.Groups.Find(groupId);
            var groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));

            foreach (var role in group.Roles)
            {
                var currentRoleId = role.RoleId;
                foreach (var user in groupUsers)
                {
                    // Is the user a member of any other groups with this role?
                    var groupsWithRole = user.Groups
                        .Where(g => g.Group.Roles
                            .Any(r => r.RoleId == currentRoleId)).Count();
                    // This will be 1 if the current group is the only one:
                    if (groupsWithRole == 1)
                    {
                        _userManager.RemoveFromRole(user.Id, role.Role.Name);
                    }
                }
            }
            group.Roles.Clear();
            _db.SaveChanges();
        }


        public void AddRoleToGroup(int groupId, string roleName)
        {
            var group = _db.Groups.Find(groupId);
            var role = _db.Roles.First(r => r.Name == roleName);
            var newgroupRole = new ApplicationRoleGroup()
            {
                GroupId = group.Id,
                Group = group,
                RoleId = role.Id,
                Role = (ApplicationRole)role
            };

            group.Roles.Add(newgroupRole);
            _db.SaveChanges();

            // Add all of the users in this group to the new role:
            var groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
            foreach (var user in groupUsers)
            {
                if (!(_userManager.IsInRole(user.Id, roleName)))
                {
                    this.AddUserToRole(user.Id, role.Name);
                }
            }
        }


        public void DeleteGroup(int groupId)
        {
            var group = _db.Groups.Find(groupId);

            // Clear the roles from the group:
            this.ClearGroupRoles(groupId);
            _db.Groups.Remove(group);
            _db.SaveChanges();
        }
    }
}