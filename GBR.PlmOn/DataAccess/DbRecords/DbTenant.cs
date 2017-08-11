using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbRecords
{
    enum Columns
    {
        ID,
        Name,
        DbConnectionString,
        DateCreated,
        Status,
        Alias
    }

    public enum State
    {
        None,
        Active,
        Inactive,
        Unknown
    }

    public class DbTenant : IDbRecord
    {
        public string Name { get; set; }
        public string DbConnectionString { get; set; }
        public State Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Alias { get; set; }

        private Int64 Id { get; set; }
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
            // TODO : Add logic to delete specified tenant record
        }

        public void DeleteAll()
        {
            // TODO : Add logic to delete all tenants
        }

        public int UpdateOne(int tenantId)
        {
            // TODO : Add logic to update on record
            throw new NotImplementedException();
        }

        public int UpdateAll()
        {
            // TODO : Add logic to update all records
            throw new NotImplementedException();
        }

        public bool HasChanged()
        {
            // TODO : Add logic to return true if original record has updated
            throw new NotImplementedException();
        }

        public void AuditRecord()
        {
            // TODO : Add logic to write audit logs to audit trial log specific table
            throw new NotImplementedException();
        }

        public object Read(int tenantId)
        {
            // TODO : Add logic to read one record using the specific record id
            DbTenant record = new DbTenant();

            return record;
        }

        /// <summary>
        /// Read All Tenants
        /// </summary>
        /// <returns></returns>
        public List<object> ReadAll()
        {
            List<object> records = new List<object>();

            ArrayList dbrecords = DbConnector.Instance.ExecuteCommand(this.GetType(), "read_all_tenants");

            foreach (object[] raw in dbrecords)
            {
                DbTenant tenant = new DbRecords.DbTenant();
                tenant.RecordId = (long)(int)raw[(int)Columns.ID];
                tenant.Name = (string)raw[(int)Columns.Name];
                tenant.DbConnectionString = (string)raw[(int)Columns.DbConnectionString];
                tenant.DateCreated = DateTime.Now;//(DateTime)raw[(int)Columns.DateCreated]; // TODO : Get the actual created value
                tenant.Status = (State)(int)raw[(int)Columns.Status];
                tenant.Alias = (string)raw[(int)Columns.Alias];

                records.Add(tenant);
            }

            return records;
        }
    }
}
