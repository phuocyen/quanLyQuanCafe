using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ConnectDB
    {
        SqlConnection conn;
        DataSet ds;

        public DataSet DS
        {
            get { return ds; }
            set { ds = value; }
        }
        public SqlConnection Conn
        {
            get { return conn; }
            set { conn = value; }
        }
        string ConnectSTR, SeverName, DatabaseName;

        public string DBName
        {
            get { return DatabaseName; }
            set { DatabaseName = value; }
        }

        public string SeverNameDB
        {
            get { return SeverName; }
            set { SeverName = value; }
        }

        public string ConnectStr
        {
            get { return ConnectSTR; }
            set { ConnectSTR = value; }
        }
        public ConnectDB()
        {
            SeverNameDB = "DESKTOP-BH4JOH6";
            DBName = "QL_QuanCaPhe";
            ConnectStr = "Data Source=" + SeverNameDB + ";Initial Catalog=" + DBName + ";Integrated Security=True";
            conn = new SqlConnection(ConnectStr);
            ds = new DataSet();
        }
        public bool OpenDB()
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
                return true;
            }
            return false;
        }
        public bool CloseDB()
        {
            if (conn.State.ToString() == "Open")
            {
                conn.Close();
                return true;
            }
            return false;
        }
        public int get_ExcuteNoneQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            int kq = cmd.ExecuteNonQuery();
            return kq;
        }
        public int get_ExcuteScalarQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            int count = (int)cmd.ExecuteScalar();
            return count;
        }
        public SqlDataReader getDataReader(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            return rd;
        }
        public DataTable getDataTable(string sql, string tablename)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(DS, tablename);
            return DS.Tables[tablename];
        }
    }
}
