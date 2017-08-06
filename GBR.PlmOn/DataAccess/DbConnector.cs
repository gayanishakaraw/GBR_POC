using DataAccess.DbRecords;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess
{
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

        [XmlElement("DateGenarated")]
        public string DbType { get; set; }

        /// <summary>
        /// Read Db Setting
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public static DbCredential ReadDbSetting(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation))
            {
                var dbSettingFilePath = ConfigurationManager.AppSettings.GetValues("DbSetting");
                fileLocation = dbSettingFilePath.ToString();
            }

            DbCredential credential = null;

            XmlSerializer serializer = new XmlSerializer(typeof(DbCredential));

            using (TextReader reader = new StreamReader(fileLocation))
            {
                object obj = serializer.Deserialize(reader);
                credential = (DbCredential)obj;
            }

            if (credential != null && credential.ClearPassword.Length > 0)
            {
                using (TextWriter writer = new StreamWriter(fileLocation))
                {
                    var sha1 = new SHA1CryptoServiceProvider();
                    var data = Encoding.ASCII.GetBytes(credential.ClearPassword);
                    var sha1data = sha1.ComputeHash(data);

                    credential.DateGenarated = DateTime.Now;
                    serializer.Serialize(writer, credential);
                }
            }

            return credential;
        }
    }

    public class DbConnector
    {
        private DbConnector instance;

        public DbConnector Instance
        {
            get
            {
                if (instance == null)
                    instance = new DbConnector();

                return instance;
            }
        }

        public SqlConnection DbConnection { get; set; }

        private DbConnector()
        {
            string connetionString = null;
            string dbSettingFile = "";

            DbCredential credentials = DbCredential.ReadDbSetting(dbSettingFile);
            byte[] pw = Encoding.ASCII.GetBytes(credentials.Password);
            var hashedPassword = Encoding.ASCII.GetString(pw, 0, pw.Length);

            connetionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={4};",credentials.DataSource, credentials.Catalog, credentials.UserName, hashedPassword);
            
            try
            {
                DbConnection = new SqlConnection(connetionString);
            }
            catch (Exception ex)
            {
                // LOG Error
            }
        }

        /// <summary>
        /// Close Db Connection
        /// </summary>
        public void CloseDbConnection()
        {
            if (DbConnection != null)
                DbConnection.Close();

            instance = null;
        }

        //public SqlDataReader ReadData(Type dataType, SqlCommand command)
        //{
        //    SqlDataReader reader;
        //    command.Connection = DbConnection;
        //    reader = command.ExecuteReader();

        //    return reader;
        //}
    }
}
