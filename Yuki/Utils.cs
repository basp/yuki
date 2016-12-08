namespace Yuki
{
    using KlwReken;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Utils
    {
        public static Tuple<int,string> CreatePdf(string path)
        {
            var handle = Process.GetCurrentProcess().Handle.ToInt32();
            var data = new KlwData();
            var xmlOut = "";
            var xmlIn = File.ReadAllText(path);
            var res = data.GetUitvoerKlw(handle, path, xmlIn, ref xmlOut);
            return Tuple.Create(res, xmlOut);
        }

        static readonly Regex GoCmdPattern = new Regex(@"^\s*[G|g][O|o]\s*$");

        static readonly string Server = @"(localdb)\mssqllocaldb";
        static readonly string ConnStr = $"Data Source={Server};Integrated Security=SSPI";

        public static int Run(string cmdText)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(cmdText, conn))
                {
                    try
                    {
                        var rowsModified = cmd.ExecuteNonQuery();
                        return rowsModified;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        // Define other methods and classes here
        public static IEnumerable<string[]> SplitSql(IEnumerable<string> lines)
        {
            lines = lines.Where(x => !string.IsNullOrWhiteSpace(x));
            var batch = new List<string>();
            foreach (var line in lines)
            {
                if (GoCmdPattern.IsMatch(line))
                {
                    yield return batch.ToArray();
                    batch = new List<string>();
                    continue;
                }
                batch.Add(line);
            }
            yield return batch.ToArray();
        }
    }
}
