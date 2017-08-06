using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbRecords
{
    class DbTenant : IDbRecord
    {
        private Int64 Id { get; set; }
        public string Name { get; set; }
        public string DbConnectionString { get; set; }
        public string Status { get; set; }
        private IDbRecord orginalRecord;
        private IDbRecord currentRecord;

        public long RecordId
        {
            get { return Id; }
            set { Id = value; }
        }

        public IDbRecord OrginalRecord
        {
            get { return orginalRecord; }
            set { orginalRecord = value; }
        }

        public IDbRecord CurrentRecord
        {
            get { return currentRecord; }
            set { currentRecord = value; }
        }

        public void DeleteOne(int tenantId)
        {

        }

        public void DeleteAll()
        {

        }

        public int UpdateOne()
        {
            throw new NotImplementedException();
        }

        public int UpdateAll()
        {
            throw new NotImplementedException();
        }

        public bool HasChanged()
        {
            throw new NotImplementedException();
        }

        public void AuditRecord()
        {
            throw new NotImplementedException();
        }

        public object Read()
        {
            DbTenant record = new DbTenant();

            return record;
        }

        public List<object> ReadAll()
        {

            List<object> records = new List<object>();


            return records;
        }
    }
}
