using DataAccess.DbRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SecurityProvidor;
using System.Web;

namespace DataAccess
{
    public sealed class DbConnector
    {
        private static DbConnector instance;

        // SQL Commands
        // Only DataAccess has access to the all the SQL commands. 
        // Developer need to call using the command name, so DbConnector will get the exact sql query and execute.
        // DbConnector will return the data raws back to the caller. (DbRecord)
        // DbRecord implementation should contains the logic to tranform the data to it's objects.
        private static Commands DbCommands; 

        public static DbConnector Instance // Database Connector object
        {
            get
            {
                if (instance == null)
                    instance = new DbConnector();

                return instance;
            }
        }

        // SQL Connection to Meta Data db
        private static SqlConnection DbConnectionMetaData { get; set; }

        // SQL Connection to Application db
        private static SqlConnection DbConnectionApp { get; set; }

        /// <summary>
        /// Read SQL Commands in Commands.xml file.
        /// If you need to add new commands simply add those in to the commands file
        /// </summary>
        private void ReadCommands()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Commands));
            using (Stream stream = typeof(Commands).Assembly.GetManifestResourceStream("DataAccess.Commands.xml"))
            {
                DbCommands = (Commands)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Read Db Setting
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        private CredentialsCollection ReadDbSetting()
        {
            string fileLocation = string.Empty;

            var dbSettingFilePath = ConfigurationManager.AppSettings.GetValues("DbSetting");
            if (dbSettingFilePath != null)
                fileLocation = dbSettingFilePath[0].ToString();

            if (string.IsNullOrEmpty(fileLocation))
                return null;

            CredentialsCollection credentials = null;

            XmlSerializer serializer = new XmlSerializer(typeof(CredentialsCollection));
            string path = Path.Combine(Environment.CurrentDirectory, fileLocation);

            using (StreamReader reader = new StreamReader(path))
            {
                credentials = (CredentialsCollection)serializer.Deserialize(reader);
            }

            // If the user credential changed please update the new password in ClearPassword element in the DbSetting file.
            // So below logic will encript the new password and allow user to login to DB as it does before. 
            if (credentials != null && credentials.DbCredential.Any(record => record.ClearPassword != null && record.ClearPassword.Length > 0))
            {
                using (TextWriter writer = new StreamWriter(path))
                {
                    foreach (DbCredential crdntl in credentials.DbCredential)
                    {
                        crdntl.DateGenarated = DateTime.Now;
                        crdntl.Password = Cryptor.Encrypt(crdntl.ClearPassword);
                        crdntl.ClearPassword = string.Empty;
                    }
                    serializer.Serialize(writer, credentials);
                }
            }

            return credentials;
        }

        private DbConnector()
        {
            string connetionString = null;

            CredentialsCollection credentials = ReadDbSetting();
            byte[] pw;
            var password = string.Empty;

            foreach (DbCredential cred in credentials.DbCredential)
            {
                if (cred.ConnectionName.Equals("TenantResolver"))
                {
                    password = Cryptor.Decrypt(cred.Password);

                    connetionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", cred.DataSource, cred.Catalog, cred.UserName, password);

                    try
                    {
                        DbConnectionMetaData = new SqlConnection(connetionString);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (cred.ConnectionName.Equals("AppDB"))
                {
                    pw = Encoding.ASCII.GetBytes(cred.Password);
                    password = Encoding.ASCII.GetString(pw, 0, pw.Length);

                    connetionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", cred.DataSource, cred.Catalog, cred.UserName, password);

                    try
                    {
                        DbConnectionApp = new SqlConnection(connetionString);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            ReadCommands(); // Read DB Commands from Commands.xml file
        }

        /// <summary>
        /// Execute Command
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public ArrayList ExecuteCommand(Type dataType, string commandId)
        {
            // Create an ArrayList to hold the results
            ArrayList rowList = new ArrayList();
            SqlDataReader reader = null;
            string sqlcommand = DbCommands.ReadCommands.FirstOrDefault(item => item.name.Equals(commandId)).SQLCommands.value; // TODO WORKING

            if (string.IsNullOrEmpty(sqlcommand))
                return rowList;

            SqlCommand command = new SqlCommand(sqlcommand);

            if (dataType == typeof(DbTenant))
            {
                DbConnectionMetaData.Open();
                command.Connection = DbConnectionMetaData;

                using (reader = command.ExecuteReader())
                {

                    // Process each result in the result set
                    while (reader.Read())
                    {
                        // Create an array big enough to hold the column values
                        object[] values = new object[reader.FieldCount];

                        // Get the column values into the array
                        reader.GetValues(values);

                        // Add the array to the ArrayList
                        rowList.Add(values);
                    }

                    // Close the reader and the connection
                    DbConnectionMetaData.Close();
                }
            }
            else
            {
                DbConnectionApp.Open();
                command.Connection = DbConnectionApp;
                using (reader = command.ExecuteReader())
                {
                    // Process each result in the result set
                    while (reader.Read())
                    {
                        // Create an array big enough to hold the column values
                        object[] values = new object[reader.FieldCount];

                        // Get the column values into the array
                        reader.GetValues(values);

                        // Add the array to the ArrayList
                        rowList.Add(values);
                    }

                    DbConnectionApp.Close();
                }
            }
            return rowList;
        }
    }
}
