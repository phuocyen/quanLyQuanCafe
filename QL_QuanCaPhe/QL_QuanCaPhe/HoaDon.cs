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
    public partial class HoaDon : Form
    {
        public HoaDon()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        DataColumn[] prikey = new DataColumn[1];
        SqlDataAdapter da_HD;
        string TaiKhoan;
        public string TK
        {
            get { return TaiKhoan; }
            set { TaiKhoan = value; }
        }
        public void create_HD()
        {
            string sql = "select * from HoaDon";
            da_HD = new SqlDataAdapter(sql, db.Conn);
            da_HD.Fill(db.DS, "HD");
            prikey[0] = db.DS.Tables["HD"].Columns["MaHD"];
            db.DS.Tables["HD"].PrimaryKey = prikey;
        }
        private void HoaDon_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel_TK.Text = "Tài khoản: " + TK;
            LoadDataGridView_Xuat_HD();
            create_HD();
        }
        public void LoadDataGridView_Xuat_HD()
        {
            string sql = "select * from HoaDon";
            SqlDataAdapter da_Xuat_HD = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_HD = new DataTable();
            da_Xuat_HD.Fill(dt_Xuat_HD);
            dataGridView_HD.DataSource = dt_Xuat_HD;
        }
        private void quayLạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Order frmOrder = new Order();
            frmOrder.TK = TK;
            frmOrder.ShowDialog();
            db.CloseDB();
            this.Close();
        }

        private void dataGridView_HD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_HD.Rows[e.RowIndex];
                txt_MaHD.Text = row.Cells[0].Value.ToString();
            }
        }

        private void btn_XemCT_Click(object sender, EventArgs e)
        {
            ChiTietHD frmCTHD = new ChiTietHD();
            frmCTHD.MAHD = txt_MaHD.Text;
            frmCTHD.ShowDialog();
        }
        public void xoaCTHD(string MaCTHD)
        {
            db.OpenDB();
            string sql = "delete from ChiTietHD where MaCTHD ='" + MaCTHD + "'";
            if (db.get_ExcuteNoneQuery(sql) == 1)
                db.CloseDB();
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string MaHD = txt_MaHD.Text;
            DialogResult del;
            del = MessageBox.Show("Bạn có chắc muốn xóa ?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (del == DialogResult.Yes)
            {
                try
                {
                    xoaCTHD(MaHD);
                    DataRow dr = db.DS.Tables["HD"].Rows.Find(MaHD);
                    if (dr != null)
                        dr.Delete();
                    SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_HD);
                    da_HD.Update(db.DS, "HD");
                    MessageBox.Show("Xóa thành công !");
                    LoadDataGridView_Xuat_HD();
                }
                catch
                {
                    MessageBox.Show("Xóa không thành công !");
                }
            }
        }

    }
}
