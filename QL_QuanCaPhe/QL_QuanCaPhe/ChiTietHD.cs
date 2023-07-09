using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using System.Data.SqlClient;

namespace QL_QuanCaPhe
{
    public partial class ChiTietHD : Form
    {
        public ChiTietHD()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        DataColumn[] prikey = new DataColumn[1];
        string MaHD;

        public string MAHD
        {
            get { return MaHD; }
            set { MaHD = value; }
        }
        private void ChiTietHD_Load(object sender, EventArgs e)
        {
            lbl_MaHD.Text = MAHD;
            LoadDataGridView_Xuat_CTHD();
        }
        public void LoadDataGridView_Xuat_CTHD()
        {
            string sql = "exec InHoaDon '"+MAHD+"'";
            SqlDataAdapter da_Xuat_CTHD = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_CTHD = new DataTable();
            da_Xuat_CTHD.Fill(dt_Xuat_CTHD);
            dataGridView_CTHD.DataSource = dt_Xuat_CTHD;
        }
    }
}
