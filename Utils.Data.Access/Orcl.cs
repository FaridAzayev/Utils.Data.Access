using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Data.Access;

namespace Utils.Data.Access
{
    public class Orcl
    {
        // TODO: 
        // Create Hashtable to store all executed datas

        private DataTable _DataTable;
        private SqlQuery _QueryText;
        private OracleCommand _cmd;
        private OracleDataReader _dataReader;
        
        //private int RowCount;

        /// <summary>
        /// Retrives data from data base
        /// </summary>
        /// <param name="QueryText">Valid sql query</param>
        /// <returns>DataTable or throws exception</returns>
        public DataTable getData(SqlQuery QueryText)
        {
            if (_DataTable == null && (_QueryText==null || !QueryText.Text.Equals(_QueryText.Text)))
            {
                _DataTable = new DataTable();

                _QueryText = new SqlQuery(QueryText.Text);

                try
                {
                    using (OracleDataAdapter dataAdapter = new OracleDataAdapter(_QueryText.Text,
                        new OracleConnection(ConnectionStrings.getConnectionString(DbTypes.oracle))))
                    {
                        int rowNumsImported = dataAdapter.Fill(_DataTable);
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Oracle connection error.\n{ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            foreach (DataColumn column in _DataTable.Columns)
                column.ColumnName = column.ColumnName.ToUpper();

            return _DataTable;
        }


        public OracleDataReader getDataReader(OracleCommandWrapper cmdWrapper , ConnectionStringType? connectionStringType = null)
        {
            if (_dataReader == null && !cmdWrapper.Equals(_cmd))
            {
                try
                {
                    var oldConnectionstring = ConnectionStrings.getConnectionString(DbTypes.oracle);
                    _cmd = new OracleCommand();
                    _cmd.Connection = new OracleConnection(
                        connectionStringType == null || connectionStringType != ConnectionStringType.changed 
                        ? oldConnectionstring : ConnectionStrings.newConnectionString
                        );
                    _cmd.CommandType = cmdWrapper.CommandType;
                    _cmd.CommandText = cmdWrapper.CommandText;

                    if (cmdWrapper.Parameters != null)
                        cmdWrapper.Parameters.ForEach(par => _cmd.Parameters.Add(par));

                    _cmd.Connection.Open();

                    _dataReader = _cmd.ExecuteReader();
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Oracle error.\n{ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unexpected eror: {ex.Message}", ex);
                }
            }

            return _dataReader;
        }
    }

    public enum ConnectionStringType
    {
        original,
        changed         
    }
}
