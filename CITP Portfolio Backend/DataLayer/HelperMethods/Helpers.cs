using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.HelperMethods
{
    public static class Helpers
    {
        public static List<Dictionary<string, object>> ExecuteFunctionSQL(NpgsqlConnection dbConnection, string sql)
        {
            var results = new List<Dictionary<string, object>>();
             
            dbConnection.Open();

            var command = new NpgsqlCommand();
            command.Connection = dbConnection;
            command.CommandText = sql;

            var readResult = command.ExecuteReader();

            while (readResult.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < readResult.FieldCount; i++)
                {

                    string column = readResult.GetName(i);
                    object result = readResult.GetValue(i);
                    row[column] = result;
                }
                results.Add(row);
                
            }

            return results;
        }
    }
}
