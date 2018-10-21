using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DbQuery
{
    public class File
    {
        public static string ReadSql(string action)
        {
            string sql = "";
            string SqlFilePath = AppDomain.CurrentDomain.BaseDirectory + "QueryScripts\\" + action + ".txt";
            using (FileStream stream = new FileStream(SqlFilePath, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(stream, Encoding.Default);
                StringBuilder builder = new StringBuilder();
                String line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    builder.AppendLine(line);
                }
                sql = builder.ToString();
                stream.Dispose();
            }

            return sql;
        }
    }
}