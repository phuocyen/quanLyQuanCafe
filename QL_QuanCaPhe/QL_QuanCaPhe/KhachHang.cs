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
    public partial class KhachHang : Form
    {
        public KhachHang()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        DataColumn[] prikey = new DataColumn[1];
        string TaiKhoan;
        public string TK
        {
            get { return TaiKhoan; }
            set { TaiKhoan = value; }
        }
        SqlDataAdapter da_KH;
        public void LoadDataGridView_Xuat_KhachHang()
        {
            string sql = "select * from KhachHang";
            SqlDataAdapter da_Xuat_KhachHang = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_KhachHang = new DataTable();
            da_Xuat_KhachHang.Fill(dt_Xuat_KhachHang);
            dataGridView_KH.DataSource = dt_Xuat_KhachHang;
        }
        public void create_KH()
        {
            string sql = "select * from KhachHang";
            da_KH = new SqlDataAdapter(sql, db.Conn);
            da_KH.Fill(db.DS, "KH");
            prikey[0] = db.DS.Tables["KH"].Columns["SoDienThoai"];
            db.DS.Tables["KH"].PrimaryKey = prikey;
        }
        public void LoadDataGridView_XuatKH_SDT()
        {
            string sql = "select * from KhachHang where SoDienThoai like '%'+'"+txt_TimKiemSDT.Text+"'+'%'";
            SqlDataAdapter da_XuatKH_SDT = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_XuatKH_SDT = new DataTable();
            da_XuatKH_SDT.Fill(dt_XuatKH_SDT);
            dataGridView_KH.DataSource = dt_XuatKH_SDT;
        }
        public void LoadDataGridView_XuatKH_HoTen()
        {
            string ht = txt_TimKiemHT.Text.Trim();
            string sql = "select * from KhachHang where HoTen like N'%'+'"+ht+"'+'%'";
            SqlDataAdapter da_XuatKH_HoTen = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_XuatKH_HoTen = new DataTable();
            da_XuatKH_HoTen.Fill(dt_XuatKH_HoTen);
            dataGridView_KH.DataSource = dt_XuatKH_HoTen;
        }
        public void LoadDataGridView_XuatKH_GT()
        {
            string GT=string.Empty;
            if(rdo_TimKiemNam.Checked)
                GT = "Nam";
            if(rdo_TimKiemNu.Checked)
                GT = "Nữ";
            string sql = "select * from KhachHang where GioiTinh = N'"+GT+"'";
            SqlDataAdapter da_XuatKH_GT = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_XuatKH_GT = new DataTable();
            da_XuatKH_GT.Fill(dt_XuatKH_GT);
            dataGridView_KH.DataSource = dt_XuatKH_GT;
        }
        private void KhachHang_Load(object sender, EventArgs e)
        {
            db.OpenDB();
            toolStripStatusLabel_TK.Text = "Tài khoản: " + TK;
            LoadDataGridView_Xuat_KhachHang();
            reset();
            create_KH();
        }
        public void reset()
        {
            txt_DC.Enabled = false;
            txt_HT.Enabled = false;
            txt_SDT.Enabled = false;
            btn_Luu.Enabled = false;
            btn_Xoa.Enabled = false;
            btn_Sua.Enabled = false;
            btn_TimKiem.Enabled = false;
            rdo_Nam.Enabled = false;
            rdo_Nu.Enabled = false;
            txt_TimKiemHT.Enabled = false;
            txt_TimKiemSDT.Enabled = false;
            rdo_TimKiemNam.Checked = false;
            rdo_TimKiemNu.Checked = false;
            rdo_TimKiemSDT.Checked = false;
            rdo_TimKiemHT.Checked = false;
            txt_TimKiemHT.Clear();
            txt_TimKiemSDT.Clear();
            txt_DC.Clear();
            txt_HT.Clear();
            txt_SDT.Clear();
            LoadDataGridView_Xuat_KhachHang();

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

        private void dataGridView_KH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                DataGridViewRow row = this.dataGridView_KH.Rows[e.RowIndex];
                txt_SDT.Text = row.Cells[0].Value.ToString();
                txt_HT.Text = row.Cells[1].Value.ToString();
                txt_DC.Text = row.Cells[3].Value.ToString();
                if (row.Cells[2].Value.ToString() == "Nam")
                    rdo_Nam.Checked = true;
                else
                    rdo_Nam.Checked = false;
                if (row.Cells[2].Value.ToString() == "Nữ")
                    rdo_Nu.Checked = true;
                else
                    rdo_Nu.Checked = false;
                btn_Sua.Enabled = true;
                btn_Xoa.Enabled = true;
            }
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            txt_DC.Enabled = true;
            txt_HT.Enabled = true;
            txt_SDT.Enabled = true;
            rdo_Nam.Enabled = true;
            rdo_Nu.Enabled = true;
            btn_Luu.Enabled = true;
            txt_DC.Clear();
            txt_HT.Clear();
            txt_SDT.Clear();
            txt_SDT.Focus();
            rdo_Nam.Checked = false;
            rdo_Nu.Checked = false;
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
        }
        public void checkradio_TimKiem()
        {
            if (rdo_TimKiemHT.Checked || rdo_TimKiemNam.Checked || rdo_TimKiemNu.Checked || rdo_TimKiemSDT.Checked)
            {
                btn_TimKiem.Enabled = true;
                if (rdo_TimKiemHT.Checked)
                {
                    txt_TimKiemHT.Enabled = true;
                    txt_TimKiemHT.Focus();
                }
                else
                {
                    txt_TimKiemHT.Enabled = false;
                    txt_TimKiemHT.Clear();
                }
                if (rdo_TimKiemSDT.Checked)
                {
                    txt_TimKiemSDT.Enabled = true;
                    txt_TimKiemSDT.Focus();
                }
                else
                {
                    txt_TimKiemSDT.Enabled = false;
                    txt_TimKiemSDT.Clear();
                }

            }
            else
                btn_TimKiem.Enabled = false;
        }
        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            if (rdo_TimKiemHT.Checked)
                LoadDataGridView_XuatKH_HoTen();
            else
                if (rdo_TimKiemSDT.Checked)
                    LoadDataGridView_XuatKH_SDT();
                else
                    LoadDataGridView_XuatKH_GT();
        }

        private void rdo_TimKiemHT_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimKiemSDT_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimKiemNam_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimKiemNu_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }
        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string SDT = txt_SDT.Text.Trim();
            string HT = txt_HT.Text.Trim();
            string DC = txt_DC.Text.Trim();
            string GT = string.Empty;
            if(rdo_Nam.Checked)
                GT = "Nam";
            if(rdo_Nu.Checked)
                GT = "Nữ";
            if(SDT==string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại khách !");
                txt_SDT.Focus();
                return;
            }
            if (HT == string.Empty)
            {
                MessageBox.Show("Chưa nhập họ tên khách !");
                txt_HT.Focus();
                return;
            }
            try
            {
                DataRow dr = db.DS.Tables["KH"].Rows.Find(SDT);
                if (dr != null)
                {
                    MessageBox.Show("Đã tồn tại khách hàng này !");
                    return;
                }
                DataRow newrow = db.DS.Tables["KH"].NewRow();
                newrow["SoDienThoai"] = SDT;
                newrow["HoTen"] = HT;
                newrow["GioiTinh"] = GT;
                newrow["DiaChi"] = DC;
                db.DS.Tables["KH"].Rows.Add(newrow);

                SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_KH);
                da_KH.Update(db.DS, "KH");
                MessageBox.Show("Lưu thành công !");
                LoadDataGridView_Xuat_KhachHang();
            }
            catch
            {
                MessageBox.Show("Lưu không thành công !");
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string SDT = txt_SDT.Text.Trim();
            if (SDT == string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại khách !");
                txt_SDT.Focus();
                return;
            }
            DialogResult del;
            del = MessageBox.Show("Bạn có chắc muốn xóa ?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (del == DialogResult.Yes)
            {
                try
                {
                    DataRow dr = db.DS.Tables["KH"].Rows.Find(SDT);
                    if (dr != null)
                        dr.Delete();
                    SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_KH);
                    da_KH.Update(db.DS, "KH");
                    MessageBox.Show("Xóa thành công !");
                    LoadDataGridView_Xuat_KhachHang();
                }
                catch
                {
                    MessageBox.Show("Xóa không thành công !");
                }
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            string SDT = txt_SDT.Text.Trim();
            string HT = txt_HT.Text.Trim();
            string DC = txt_DC.Text.Trim();
            string GT = string.Empty;
            if (rdo_Nam.Checked)
                GT = "Nam";
            if (rdo_Nu.Checked)
                GT = "Nữ";
            if (SDT == string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại khách !");
                txt_SDT.Focus();
                return;
            }
            if (HT == string.Empty)
            {
                MessageBox.Show("Chưa nhập họ tên khách !");
                txt_HT.Focus();
                return;
            }
            DialogResult edit;
            edit = MessageBox.Show("Bạn có chắc muốn sửa ?", "Sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (edit == DialogResult.Yes)
            {
                try
                {
                    DataRow dr = db.DS.Tables["KH"].Rows.Find(SDT);
                    if (dr != null)
                    {
                        dr["SoDienThoai"] = SDT;
                        dr["HoTen"] = HT;
                        dr["GioiTinh"] = GT;
                        dr["DiaChi"] = DC;
                    }

                    SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_KH);
                    da_KH.Update(db.DS, "KH");
                    MessageBox.Show("Sửa thành công !");
                    LoadDataGridView_Xuat_KhachHang();
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công !");
                }
            }
        }

        private void txt_SDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txt_HT_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txt_TimKiemSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }


    }
}
