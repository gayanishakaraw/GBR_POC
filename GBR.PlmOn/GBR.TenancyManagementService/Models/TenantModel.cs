﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using GBR.Entity;
using DataAccess;

namespace GBR.TenancyManagementService.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class TenantService
    {
        private static Object lockObject = new Object();

        public static Tenant ResolveTenant(string tenantKey)
        {
            lock (lockObject)
            {
                tenantKey = tenantKey.ToLowerInvariant();
                DbConnector.Instance

                //We shall read these from the tenant meta data DB instead od using hard coded values
                switch (tenantKey)
                {
                    case "mas":
                        return new Tenant() { Id = 1, Name = "Mas Holdings", Status = "Active", DbConnectionString = "DB Connection 1" };
                    case "brandix":
                        return new Tenant() { Id = 1, Name = "Brandix", Status = "Active", DbConnectionString = "DB Connection 2" };
                    case "hela":
                        return new Tenant() { Id = 1, Name = "Hela Clothing", Status = "Active", DbConnectionString = "DB Connection 3" };
                    default:
                        return null;
                }
            }
        }
    }
}

