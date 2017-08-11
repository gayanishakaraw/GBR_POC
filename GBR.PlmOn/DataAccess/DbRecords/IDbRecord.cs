using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbRecords
{
    public interface IDbRecord
    {
        Int64 RecordId { get; set; }
        IDbRecord OrginalRecord { get; set; }
        IDbRecord CurrentRecord { get; set; }

        object Read(int recordId);

        List<object> ReadAll();

        void DeleteOne(int recordId);

        void DeleteAll();

        int UpdateOne(int recordId);

        int UpdateAll();

        bool HasChanged();

        void AuditRecord();
    }
}
