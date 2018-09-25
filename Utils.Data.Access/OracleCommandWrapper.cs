using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Data.Access
{
    public class OracleCommandWrapper
    {
        public string CommandText { get; set; }

        public CommandType CommandType { get; set; }

        public List<OracleParameter> Parameters { get; set; }

        public bool Equals(OracleCommand cmd)
        {
            if (cmd != null
                && cmd.CommandText == CommandText
                && cmd.CommandType == CommandType
                && cmd.Parameters.Count == Parameters.Count)
            {
                foreach (OracleParameter item in cmd.Parameters)
                {
                    OracleParameter par;
                    if (Parameters.Any(p => p.ParameterName == item.ParameterName))
                    {
                        par = Parameters.First(p => p.ParameterName == item.ParameterName);

                        if (item.DbType != par.DbType || item.Value != par.Value || item.Direction != par.Direction)
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
