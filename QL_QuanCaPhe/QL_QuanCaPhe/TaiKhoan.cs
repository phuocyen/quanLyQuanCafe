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
    public partial class TaiKhoan : Form
    {
        public TaiKhoan()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        SqlDataAdapter da_TK;
        DataColumn[] prikey = new DataColumn[1];

        string taikhoan;
        public string TK
        {
            get { return taikhoan; }
            set { taikhoan = value; }
        }
        private void TaiKhoan_Load(object sender, EventArgs e)
        {
            LoadDataGridView_Xuat_TK();
            LoadDataGridView_Xuat_NV();
            create_TK();
        }
        public void LoadDataGridView_Xuat_NV()
        {
            string sql = "exec XuatNV";
            SqlDataAdapter da_Xuat_NV = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV = new DataTable();
            da_Xuat_NV.Fill(dt_Xuat_NV);
            dataGridView_NV.DataSource = dt_Xuat_NV;
        }
        public void LoadDataGridView_Xuat_TK()
        {
            string sql = "select * from TaiKhoan";
            SqlDataAdapter da_Xuat_TK = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_TK = new DataTable();
            da_Xuat_TK.Fill(dt_Xuat_TK);
            dataGridView_TK.DataSource = dt_Xuat_TK;
        }
        public void create_TK()
        {
            string sql = "select * from TaiKhoan";
            da_TK = new SqlDataAdapter(sql, db.Conn);
            da_TK.Fill(db.DS, "TaiKhoan");
            prikey[0] = db.DS.Tables["TaiKhoan"].Columns["TenTK"];
            db.DS.Tables["TaiKhoan"].PrimaryKey = prikey;
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            txt_MaNV.Clear();
            txt_MK.Clear();
            txt_TenTK.Clear();
            rdo_Admin.Checked = false;
            rdo_Manage.Checked = false;
            rdo_User.Checked = false;
            txt_TenTK.Focus();
        }

        private void dataGridView_NV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_NV.Rows[e.RowIndex];
                txt_MaNV.Text = row.Cells[0].Value.ToString();
                txt_TenTK.Text = row.Cells[0].Value.ToString();
            }
        }

        private void dataGridView_TK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_TK.Rows[e.RowIndex];
                txt_TenTK.Text = row.Cells[0].Value.ToString();
                txt_MK.Text = row.Cells[1].Value.ToString();
                string Quyen = row.Cells[2].Value.ToString();
                if (Quyen == "Admin")
                    rdo_Admin.Checked = true;
                else
                    rdo_Admin.Checked = false;
                if (Quyen == "Manage")
                    rdo_Manage.Checked = true;
                else
                    rdo_Manage.Checked = false;
                if (Quyen == "User")
                    rdo_User.Checked = true;
                else
                    rdo_User.Checked = false;
                txt_MaNV.Text = row.Cells[3].Value.ToString();
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            string TenTK = txt_TenTK.Text.Trim();
            string MK = txt_MK.Text.Trim();
            string Quyen = string.Empty;
       
            if(TenTK == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên tài khoản !");
                txt_TenTK.Focus();
                return;
            }
            if(MK == string.Empty)
            {
                MessageBox.Show("Chưa nhập mật khẩu !");
                txt_MK.Focus();
                return;
            }
            if (rdo_Admin.Checked)
                Quyen = "Admin";
            if (rdo_Manage.Checked)
                Quyen = "Manage";
            if (rdo_User.Checked)
                Quyen = "User";
            if (rdo_Admin.Checked == false && rdo_Manage.Checked == false && rdo_User.Checked == false)
            {
                MessageBox.Show("Chưa chọn quyền");
                return;
            }
            DialogResult edit;
            edit = MessageBox.Show("Bạn có chắc muốn sửa ?", "Sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (edit == DialogResult.Yes)
            {
                if (TenTK != TK)
                {
                    try
                    {
                        DataRow dr = db.DS.Tables["TaiKhoan"].Rows.Find(TenTK);
                        if (dr != null)
                        {
                            dr["MaNV"] = txt_MaNV.Text;
                            dr["TenTK"] = TenTK;
                            dr["MatKhau"] = MK;
                            dr["Quyen"] = Quyen;
                        }

                        SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_TK);
                        da_TK.Update(db.DS, "TaiKhoan");
                        MessageBox.Show("Sửa thành công !");
                        LoadDataGridView_Xuat_TK();
                    }
                    catch
                    {
                        MessageBox.Show("Sửa không thành công !");
                    }
                }
                else
                    MessageBox.Show("Không thể sửa vì tên tài khoản đang dùng");
            }
        }
        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string TenTK = txt_TenTK.Text.Trim();
            if (TenTK == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên tài khoản !");
                txt_TenTK.Focus();
                return;
            }
            DialogResult del;
            del = MessageBox.Show("Bạn có chắc muốn xóa ?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (del == DialogResult.Yes)
            {
                if (TenTK!=TK)
                {
                    try
                    {
                        DataRow dr = db.DS.Tables["TaiKhoan"].Rows.Find(TenTK);
                        if (dr != null)
                            dr.Delete();
                        SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_TK);
                        da_TK.Update(db.DS, "TaiKhoan");
                        MessageBox.Show("Xóa thành công !");
                        LoadDataGridView_Xuat_TK();
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công !");
                    }
                }
                else
                    MessageBox.Show("Không thể xóa vì tên tài khoản đang dùng");
            }
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string TenTK = txt_TenTK.Text.Trim();
            string MK = txt_MK.Text.Trim();
            string Quyen = string.Empty;
            string MaNV = txt_MaNV.Text.Trim();
            if (MaNV == string.Empty)
                MaNV = null;
            if (TenTK == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên tài khoản !");
                txt_TenTK.Focus();
                return;
            }
            if (MK == string.Empty)
            {
                MessageBox.Show("Chưa nhập mật khẩu !");
                txt_MK.Focus();
                return;
            }
            if (rdo_Admin.Checked)
                Quyen = "Admin";
            if (rdo_Manage.Checked)
                Quyen = "Manage";
            if (rdo_User.Checked)
                Quyen = "User";
            if (rdo_Admin.Checked == false && rdo_Manage.Checked == false && rdo_User.Checked==false)
            {
                MessageBox.Show("Chưa chọn quyền");
                return;
            }
            try
            {
                DataRow dr = db.DS.Tables["TaiKhoan"].Rows.Find(TenTK);
                if (dr != null)
                {
                    MessageBox.Show("Đã tồn tại tài khoản này !");
                    return;
                }
                DataRow newrow = db.DS.Tables["TaiKhoan"].NewRow();
                newrow["TenTK"] = TenTK;
                newrow["MANV"] = MaNV;
                newrow["MatKhau"] = MK;
                newrow["Quyen"] = Quyen;
                db.DS.Tables["TaiKhoan"].Rows.Add(newrow);

                SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_TK);
                da_TK.Update(db.DS, "TaiKhoan");
                MessageBox.Show("Lưu thành công !");
                LoadDataGridView_Xuat_TK();
            }
            catch
            {
                MessageBox.Show("Lưu không thành công !");
            }
        }
    }
}
