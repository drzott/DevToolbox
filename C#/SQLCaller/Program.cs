using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SQLCaller
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection conn = new SqlConnection(SQLCaller.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SQLCaller.Properties.Settings.Default.SQLCommand, conn);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
