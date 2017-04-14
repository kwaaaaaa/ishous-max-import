using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

// Find-Package MySql.Data
// Install-Package MySql.Data

class MysqlDB
{
    public MySqlConnection mysql_conn;
    public MySqlCommand cmd = new MySqlCommand();
    public MySqlDataReader reader;

    public void ConnectToDB(string conn_params) {
        // connect
        mysql_conn = new MySqlConnection(conn_params);
        mysql_conn.Open();
        Console.WriteLine("connected to mysql db");
        cmd = new MySqlCommand();
        cmd.Connection = mysql_conn;

        // set time zone
        string sql = "SET SESSION time_zone = 'Asia/Tokyo';";
        Operation(sql);

        // set encoding
        sql = "SET NAMES 'UTF8'";
        Operation(sql);
        sql = "SET CHARACTER SET 'UTF8'";
        Operation(sql);
    }

    public void CloseConnection() {
        cmd.Dispose();
        mysql_conn.Close();
    }

    public void Operation(string sql) {
        cmd.CommandText = sql;
        int affectedRows = cmd.ExecuteNonQuery();
        if (!sql.StartsWith("SET")) {
            Console.WriteLine("{0} rows uploaded", affectedRows);
        };
        cmd.Dispose();
    }

    public string PullSingleValue(string sql)
    {
        //Retry:
        //int retry = 0;
        string val = "error";
        //try {
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
        cmd.Dispose();
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            val = reader.GetString(0).ToString();
        };
        reader.Close();
        //} catch {
        //    System.Threading.Thread.Sleep(10000);
        //    retry++;
        //    if (retry >= 5) return "error";
        //    goto Retry;
        //};
        return val;
    }
}
