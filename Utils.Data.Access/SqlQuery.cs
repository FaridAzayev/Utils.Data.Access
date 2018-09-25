using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Data.Access
{
    public class SqlQuery
    {
        public string Text { get; }
        public SqlQuery(string QueryText)
        {
            Text = QueryText;
        }
    }
}
