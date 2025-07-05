using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalControl
{
    //    public class Rule
    //    {
    //        public int Id { get; set; }
    //        public string Type { get; set; }
    //        public string Value { get; set; }
    //        public string Action { get; set; }
    //    }

    //    public static class RuleDatabase
    //    {
    //        private static string _dbPath = "Data Source=rules.db";

    //        public static void Init()
    //        {
    //            using var con = new SQLiteConnection(_dbPath);
    //            con.Open();
    //            using var cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Rules (Id INTEGER PRIMARY KEY AUTOINCREMENT, Type TEXT, Value TEXT, Action TEXT)", con);
    //            cmd.ExecuteNonQuery();
    //        }

    //        public static List<Rule> GetAllRules()
    //        {
    //            var list = new List<Rule>();
    //            using var con = new SQLiteConnection(_dbPath);
    //            con.Open();
    //            using var cmd = new SQLiteCommand("SELECT * FROM Rules", con);
    //            using var reader = cmd.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                list.Add(new Rule
    //                {
    //                    Id = Convert.ToInt32(reader["Id"]),
    //                    Type = reader["Type"].ToString(),
    //                    Value = reader["Value"].ToString(),
    //                    Action = reader["Action"].ToString()
    //                });
    //            }
    //            return list;
    //        }

    //        public static void AddRule(Rule rule)
    //        {
    //            using var con = new SQLiteConnection(_dbPath);
    //            con.Open();
    //            using var cmd = new SQLiteCommand("INSERT INTO Rules (Type, Value, Action) VALUES (@type, @value, @action)", con);
    //            cmd.Parameters.AddWithValue("@type", rule.Type);
    //            cmd.Parameters.AddWithValue("@value", rule.Value);
    //            cmd.Parameters.AddWithValue("@action", rule.Action);
    //            cmd.ExecuteNonQuery();
    //        }

    //        public static void Export(string path)
    //        {
    //            File.Copy("rules.db", path, overwrite: true);
    //        }

    //        public static void Import(string path)
    //        {
    //            File.Copy(path, "rules.db", overwrite: true);
    //        }
    //    }

}
