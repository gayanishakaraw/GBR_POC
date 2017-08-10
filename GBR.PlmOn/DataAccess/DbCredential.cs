using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess
{
    [Serializable()]
    [XmlRoot("DbCredentials")]
    public class DbCredentials
    {
        [XmlArray("DbCredentials")]
        [XmlArrayItem("DbCredential", typeof(DbCredential))]
        public DbCredential[] DbSettings { get; set; }
    }

    [Serializable()]
    public class DbCredential
    {
        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }

        [XmlIgnore]
        [XmlElement("ClearPassword")]
        public string ClearPassword { get; set; }

        [XmlElement("DataSource")]
        public string DataSource { get; set; }

        [XmlElement("Catalog")]
        public string Catalog { get; set; }

        [XmlElement("Timeout")]
        public int Timeout { get; set; }

        [XmlElement("Port")]
        public int Port { get; set; }

        [XmlElement("DateGenarated")]
        public DateTime DateGenarated { get; set; }

        [XmlElement("ConnectionName")]
        public string ConnectionName { get; set; }

        [XmlElement("DbType")]
        public string DbType { get; set; }      
    }
}
