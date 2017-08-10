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
    //[Serializable]
    //public class DbCredentials
    //{
    //    [XmlArray("DbCredentials")]
    //    [XmlArrayItem("DbCredential", typeof(DbCredential))]
    //    public List<DbCredential> DbSettings { get; set; }
    //}
    //public class DbCredential
    //{
    //    [XmlElement("UserName")]
    //    public string UserName { get; set; }

    //    [XmlElement("Password")]
    //    public string Password { get; set; }

    //    [XmlIgnore]
    //    [XmlElement("ClearPassword")]
    //    public string ClearPassword { get; set; }

    //    [XmlElement("DataSource")]
    //    public string DataSource { get; set; }

    //    [XmlElement("Catalog")]
    //    public string Catalog { get; set; }

    //    [XmlElement("Timeout")]
    //    public int Timeout { get; set; }

    //    [XmlElement("Port")]
    //    public int Port { get; set; }

    //    [XmlElement("DateGenarated")]
    //    public DateTime DateGenarated { get; set; }

    //    [XmlElement("ConnectionName")]
    //    public string ConnectionName { get; set; }

    //    [XmlElement("DbType")]
    //    public string DbType { get; set; }

    //    /// <summary>
    //    /// Read Db Setting
    //    /// </summary>
    //    /// <param name="fileLocation"></param>
    //    /// <returns></returns>
    //    public static List<DbCredential> ReadDbSetting(string fileLocation = null)
    //    {
    //        if (string.IsNullOrEmpty(fileLocation))
    //        {
    //            var dbSettingFilePath = ConfigurationManager.AppSettings.GetValues("DbSetting");
    //            if (dbSettingFilePath != null)
    //                fileLocation = dbSettingFilePath[0].ToString();
    //        }

    //        if (string.IsNullOrEmpty(fileLocation))
    //            return null;

    //        List<DbCredential> credentials = null;

    //        XmlSerializer serializer = new XmlSerializer(typeof(DbCredentials));

    //        using (StreamReader reader = new StreamReader(fileLocation))
    //        {
    //            credentials = (List<DbCredential>)serializer.Deserialize(reader);
    //        }

    //        if (credentials != null && credentials.Any(record => record.ClearPassword.Length > 0))
    //        {
    //            using (TextWriter writer = new StreamWriter(fileLocation))
    //            {
    //                foreach (DbCredential crdntl in credentials)
    //                {
    //                    var sha1 = new SHA1CryptoServiceProvider();
    //                    var data = Encoding.ASCII.GetBytes(crdntl.ClearPassword);
    //                    var sha1data = sha1.ComputeHash(data);

    //                    crdntl.DateGenarated = DateTime.Now;
    //                }
    //                serializer.Serialize(writer, credentials);
    //            }
    //        }

    //        return credentials;
    //    }
    //}

    public sealed class DbConnector
    {
        private static DbConnector instance;

        public static DbConnector Instance
        {
            get
            {
                if (instance == null)
                    instance = new DbConnector();

                return instance;
            }
        }

        public SqlConnection DbConnectionMetaData { get; set; }

        public SqlConnection DbConnectionApp { get; set; }

        /// <summary>
        /// Read Db Setting
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public DbCredentials ReadDbSetting(string fileLocation = null)
        {
            if (string.IsNullOrEmpty(fileLocation))
            {
                var dbSettingFilePath = ConfigurationManager.AppSettings.GetValues("DbSetting");
                if (dbSettingFilePath != null)
                    fileLocation = dbSettingFilePath[0].ToString();
            }

            if (string.IsNullOrEmpty(fileLocation))
                return null;

            DbCredentials credentials = null;

            XmlSerializer serializer = new XmlSerializer(typeof(DbCredentials));

            using (StreamReader reader = new StreamReader(fileLocation))
            {
                credentials = (DbCredentials)serializer.Deserialize(reader);
            }

            if (credentials != null && credentials.DbSettings.Any(record => record.ClearPassword.Length > 0))
            {
                using (TextWriter writer = new StreamWriter(fileLocation))
                {
                    foreach (DbCredential crdntl in credentials.DbSettings)
                    {
                        var sha1 = new SHA1CryptoServiceProvider();
                        var data = Encoding.ASCII.GetBytes(crdntl.ClearPassword);
                        var sha1data = sha1.ComputeHash(data);

                        crdntl.DateGenarated = DateTime.Now;
                    }
                    serializer.Serialize(writer, credentials);
                }
            }

            return credentials;
        }

        private DbConnector()
        {
            string connetionString = null;
            string dbSettingFile = ""; // TODO : Get the db setting file location

            DbCredentials credentials = ReadDbSetting(dbSettingFile);
            byte[] pw;
            var hashedPassword = string.Empty;

            foreach (DbCredential cred in credentials.DbSettings)
            {
                pw = null;
                hashedPassword = string.Empty; 

                if (cred.ConnectionName.Equals("TenantResolver"))
                {
                    pw = Encoding.ASCII.GetBytes(cred.Password);
                    hashedPassword = Encoding.ASCII.GetString(pw, 0, pw.Length);

                    connetionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={4};", cred.DataSource, cred.Catalog, cred.UserName, hashedPassword);

                    try
                    {
                        DbConnectionMetaData = new SqlConnection(connetionString);
                    }
                    catch (Exception ex)
                    {
                        throw;
                        // LOG Error
                    }
                }

                if (cred.ConnectionName.Equals("AppDB"))
                {
                    pw = Encoding.ASCII.GetBytes(cred.Password);
                    hashedPassword = Encoding.ASCII.GetString(pw, 0, pw.Length);

                    connetionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={4};", cred.DataSource, cred.Catalog, cred.UserName, hashedPassword);

                    try
                    {
                        DbConnectionApp = new SqlConnection(connetionString);
                    }
                    catch (Exception ex)
                    {
                        throw;
                        // LOG Error
                    }
                }
            }
        }

        /// <summary>
        /// Close Db Connection
        /// </summary>
        public void CloseDbConnection()
        {
            if (DbConnectionMetaData != null)
                DbConnectionMetaData.Close();

            instance = null;
        }

        public SqlDataReader ExecuteCommand(Type dataType, SqlCommand command)
        {
            SqlDataReader reader;
            command.Connection = DbConnectionMetaData;
            reader = command.ExecuteReader();

            return reader;
        }
    }
}
