using System;
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

            SqlCommand command = new SqlCommand("Select * from Tenant");
            using (SqlDataReader reader = DbConnector.Instance.ExecuteCommand(this.GetType(), command))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DbTenant tenant = new DbRecords.DbTenant();
                        tenant.RecordId = (long)reader[(int)Columns.ID];
                        tenant.Name = (string)reader[(int)Columns.Name];
                        tenant.DbConnectionString = (string)reader[(int)Columns.DbConnectionString];
                        tenant.DateCreated = (DateTime)reader[(int)Columns.DateCreated];
                        tenant.Status = (State)reader[(int)Columns.Status];
                        tenant.Alias = (string)reader[(int)Columns.Alias];

                        records.Add(tenant);
                    }
                }
            }

            return records;
        }
    }
}
