using GBR.Entity;
using GBR.TenancyManagementService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GBR.TenancyManagementService.Controllers
{
    [AllowAnonymous]
    public class TenantController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/Tenant/mas
        [AllowAnonymous]
        public Tenant Get(string tenantKey)
        {
            return TenantService.ResolveTenant(tenantKey);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
