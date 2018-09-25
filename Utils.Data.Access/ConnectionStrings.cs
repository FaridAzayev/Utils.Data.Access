using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Data.Access
{
    public class ConnectionStrings
    {
        private Hashtable ConnectionTypes = new Hashtable();
        private readonly static ConnectionStringSettingsCollection _ConnectionStrings;
        public static String newConnectionString { get; private set; }

        static ConnectionStrings()
        {
            Cipher.SecureConnectionStrings();
            if (_ConnectionStrings == null) _ConnectionStrings = ConfigurationManager.ConnectionStrings;
        }

        public static ConnectionStringSettingsCollection getConnectionStrings()
        {
            return _ConnectionStrings;
        }

        public static string getConnectionString(DbTypes paramType)
        {
            String connStr;

            switch (paramType)
            {
                case DbTypes.oracle:
                    connStr = _ConnectionStrings["OracleDbContext"].ConnectionString;
                    break;
                case DbTypes.mssql:
                    connStr = _ConnectionStrings["MsSqlDbContext"].ConnectionString;
                    break;
                default:
                    throw new ArgumentException($"Not such \"{paramType}\" case statement.", "paramType");
            }
            return connStr;
        }

        public static void ChangeConnectionStringUserPass(string user, string pass , string connectionString)
        {
            connectionString = connectionString.Trim();

            if (connectionString.ToUpper().IndexOf("USER ID") >= 0)
            {
                var userFistIndex = connectionString.IndexOf("=", connectionString.ToUpper().IndexOf("USER ID")) + 1;
                var userOldValue = connectionString.Substring(userFistIndex, connectionString.IndexOf(";", userFistIndex) - userFistIndex);
                connectionString = connectionString.Replace(userOldValue, " " + user);
            }
            else
            {
                connectionString = connectionString + $";USER ID = {user};";
            }

            if (connectionString.ToUpper().IndexOf("USER ID") >= 0)
            {
                var passFistIndex = connectionString.IndexOf("=", connectionString.ToUpper().IndexOf("PASSWORD")) + 1;
                var passOldValue = connectionString.Substring(passFistIndex, connectionString.IndexOf(";", passFistIndex) - passFistIndex);
                connectionString = connectionString.Replace(passOldValue, " " + pass);
            }
            else
            {
                connectionString = connectionString + $";PASSWORD = {pass};";
            }
            newConnectionString  = connectionString;
        }
    }

    public enum DbTypes
    {
        oracle,
        mssql
    }
}
