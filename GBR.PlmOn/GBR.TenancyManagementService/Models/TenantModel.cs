using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using GBR.Entity;
using DataAccess;
using DataAccess.DbRecords;
using System.Collections.Generic;

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
                var currentTenant = new DbTenant().ReadAll().Find(item => (item as DbTenant).Alias.ToLowerInvariant() == tenantKey.ToLowerInvariant());

                if (currentTenant != null)
                {
                    DbTenant record = currentTenant as DbTenant;

                    return new Tenant() { Id = record.RecordId, Name = record.Name, Status = record.Status.ToString(), DbConnectionString = record.DbConnectionString };
                }

                return null;
            }
        }
    }
}

