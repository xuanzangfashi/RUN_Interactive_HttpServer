using System;
using MySql.Data.MySqlClient;
using MyHttpServer.Statics;

namespace MyHttpServer.Sql
{
    public static class SqlWorker
    {
        public static bool MySqlInit()
        {
            MySqlConnection conn;
            conn = new MySqlConnection(StaticObjects.SqlUrl);

            var sqlre = true;
            try
            {
                conn.Open();
                Debuger.PrintStr("Connected!", EPRINT_TYPE.NORMAL, false);
            }
            catch (MySqlException ex)
            {
                Debuger.PrintStr(ex.Message, EPRINT_TYPE.ERROR, false);
                sqlre = false;
            }
            finally
            {
                conn.Close();

            }
            return sqlre;
        }

        public static int MySqlInsert(string dataBase, string tableName, string[] colNames, string[] values)
        {
            string command = "INSERT INTO " + dataBase + "." + tableName + " (";
            for (int i = 0; i < colNames.Length; i++)
            {
                command = command + colNames[i];
                if (i != colNames.Length - 1)
                {
                    command = command + ",";
                }
            }
            command = command + ") value (";
            for (int i = 0; i < values.Length; i++)
            {
                command = command + "\"" + values[i] + "\"";
                if (i != values.Length - 1)
                {
                    command = command + ",";
                }
            }
            command = command + ");";
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                var comm = new MySqlCommand(command, conn);
                int re = comm.ExecuteNonQuery();
                return re;
            }
        }

        public static MySqlDataReader MySqlQuery(string dataBase, string tableName, string[] colNames, string where, string whereValue, out MySqlConnection conn, out string reStr)
        {
            MySqlDataReader reader = null;
            conn = null;
            try
            {
                string command = "select ";
                for (int i = 0; i < colNames.Length; i++)
                {
                    command = command + colNames[i];
                    if (i != colNames.Length - 1)
                    {
                        command = command + ",";
                    }
                    else
                    {
                        command = command + " ";
                    }
                }
                command = command + " from " + dataBase + "." + tableName;
                if (where != null)
                    command = command + " where " + where + "=\"" + whereValue + "\"";
                conn = new MySqlConnection(StaticObjects.SqlUrl);
                conn.Open();
                MySqlCommand CMD = new MySqlCommand(command, conn);

                reader = CMD.ExecuteReader();
                reStr = "OK";
                return reader;
            }
            catch (Exception ex)
            {
                if (conn != null)
                {
                    conn.Close();
                }
                reStr = ex.Message;
                reader = null;
                return reader;
            }
        }

        public static bool MySqlIsExist(string dataBase, string tableName, string colName, string specificValue)
        {
            string command = "select " + colName;

            command = command + " from " + dataBase + "." + tableName + " where " + colName + "=\"" + specificValue + "\"";
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand CMD = new MySqlCommand(command, conn);
                MySqlDataReader reader = null;
                reader = CMD.ExecuteReader();
                bool isExist = false;
                while (reader.Read())
                {
                    isExist = true;
                }
                return isExist;
            }
        }

        public static bool MySqlIsExist(string dataBase, string tableName, string[] colNames, string[] specificValues)
        {
            string command = "select * from " + dataBase + "." + tableName + " where ";
            for (int i = 0; i < colNames.Length; i++)
            {
                command = command + colNames[i] + "=" + "'" + specificValues[i] + "'";
                if (i != colNames.Length - 1)
                {
                    command = command + " and ";
                }
            }
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand CMD = new MySqlCommand(command, conn);
                MySqlDataReader reader = null;
                reader = CMD.ExecuteReader();
                bool isExist = false;
                while (reader.Read())
                {
                    isExist = true;
                }
                return isExist;
            }
        }

        public static bool MySqlIsExist(string dataBase, string tableName)
        {
            string command = "select * from " + dataBase + "." + tableName;
            bool isExist = false;
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                try
                {
                    conn.Open();
                    MySqlCommand CMD = new MySqlCommand(command, conn);
                    MySqlDataReader reader = null;
                    reader = CMD.ExecuteReader();


                    isExist = true;
                }
                catch
                {
                    isExist = false;
                }
            }
            return isExist;
        }

        public static bool MySqlIsExist(string dataBase, string tableName, string colName)
        {
            string command = "select " + colName + " from " + dataBase + "." + tableName;
            bool isExist = false;
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                try
                {
                    conn.Open();
                    MySqlCommand CMD = new MySqlCommand(command, conn);
                    MySqlDataReader reader = null;
                    reader = CMD.ExecuteReader();


                    isExist = true;
                }
                catch
                {
                    isExist = false;
                }
            }
            return isExist;
        }


        public static bool MySqlEdit(string dataBase, string tableName, string id, string[] keys, string[] values)
        {
            if (keys.Length != values.Length)
            {
                return false;
            }
            string command = "UPDATE " + dataBase + "." + tableName + " SET ";
            int index = 0;
            string tmp = "";
            foreach (var i in keys)
            {
                tmp = tmp + i + " = " + "\"" + values[index] + "\"";
                if (index != values.Length - 1)
                {
                    tmp = tmp + ",";
                }
                index++;
            }
            command = command + tmp + " WHERE id = " + id;
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand CMD = new MySqlCommand(command, conn);
                CMD.ExecuteNonQuery();
            }
            return true;
        }

        public static bool MySqlEdit(string dataBase, string tableName, string[] keys, string[] values, int index, string a_d_order)
        {
            string command = "update " + dataBase + "." + tableName + " set ";
            int index_ = 0;
            string tmp = "";
            foreach (var i in keys)
            {
                tmp = tmp + i + " = " + "\"" + values[index_] + "\"";
                if (index_ != values.Length - 1)
                {
                    tmp = tmp + ",";
                }
                index_++;
            }
            command = command + tmp + " where " + index.ToString() + " order by id " + a_d_order + " limit 1";
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand CMD = new MySqlCommand(command, conn);
                CMD.ExecuteNonQuery();
            }
            return true;
        }


        public static bool MySqlDelete(string dataBase, string tableName, string[] id)
        {
            string command = "DELETE FROM " + dataBase + "." + tableName + " WHERE id in(";
            int index = 0;
            foreach (var i in id)
            {
                command = command + i;
                if (index != id.Length - 1)
                {
                    command = command + ",";
                }
                index++;
            }
            command = command + ")";
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand CMD = new MySqlCommand(command, conn);
                CMD.ExecuteNonQuery();
            }
            return true;
        }

        public static void MySqlCreateTable(string dataBase, string tableName, string[] colNames)
        {
            string command = "create table " + dataBase + "." + tableName + " (id int not null AUTO_INCREMENT unique, ";
            for (int i = 0; i < colNames.Length; i++)
            {
                command = command + colNames[i] + " VARCHAR(45)";
                command = command + ", ";
            }
            command = command + "primary key (id)";
            command = command + ")";
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void MySqlCreateColumn(string dataBase, string tableName, string colName)
        {
            string command = "alter table " + dataBase + "." + tableName + " add column " + colName + " VARCHAR(45)";
            using (var conn = new MySqlConnection(StaticObjects.SqlUrl))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
