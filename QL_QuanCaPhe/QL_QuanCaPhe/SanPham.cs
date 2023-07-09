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
using System.IO;

namespace QL_QuanCaPhe
{
    public partial class SanPham : Form
    {
        public SanPham()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        string TaiKhoan;
        public string TK
        {
            get { return TaiKhoan; }
            set { TaiKhoan = value; }
        }
        public void reset()
        {
            txt_TenSP.Enabled = false;
            txt_DV.Enabled = false;
            txt_Gia.Enabled = false;
            txt_MaSP.Enabled = false;
            btn_Luu.Enabled = false;
            btn_Sua.Enabled = false;
            btn_TimKiem.Enabled = false;
            btn_TaiAnh.Enabled = false;
            btn_Xoa.Enabled = false;
            txt_TimTenSP.Enabled = false;
            cbo_LoaiSP.Enabled = false;
            cbo_TimGia.Enabled = false;
            cbo_TimLoaiSP.Enabled = false;
            rdo_LoaiSP.Checked = false;
            rdo_TimGia.Checked = false;
            rdo_TimTenSP.Checked = false;
            txt_TimTenSP.Clear();
            LoadDataGridView_Xuat_SP();

        }
        public string SelectAnh(string linkanh)
        {
            db.OpenDB();
            string sql = "select LinkHinh from SanPham where MaSP = '" + linkanh + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string AnhSP = (string)cmd.ExecuteScalar();
            db.CloseDB();
            return AnhSP;
        }
        private void SanPham_Load(object sender, EventArgs e)
        {
            db.OpenDB();
            toolStripStatusLabel_TK.Text = "Tài khoản: " + TK;
            checkradio_TimKiem();
            LoadCbo_LoaiSP();
            LoadCbo_TimLoaiSP();
            LoadDataGridView_Xuat_SP();
            cbo_TimGia.SelectedIndex = 0;
            reset();
            create_SP();
        }
        public void LoadCbo_LoaiSP()
        {
            string sql = "select * from LoaiSP";
            cbo_LoaiSP.DataSource = db.getDataTable(sql, "LoaiSP");
            cbo_LoaiSP.DisplayMember = "TenLoai";
            cbo_LoaiSP.ValueMember = "MaLoaiSP";
            cbo_LoaiSP.SelectedIndex = 0;
        }
        public void LoadCbo_TimLoaiSP()
        {
            string sql = "select * from LoaiSP";
            cbo_TimLoaiSP.DataSource = db.getDataTable(sql, "TimLoaiSP");
            cbo_TimLoaiSP.DisplayMember = "TenLoai";
            cbo_TimLoaiSP.ValueMember = "MaLoaiSP";
            cbo_TimLoaiSP.SelectedIndex = 0;
        }
        public void LoadDataGridView_Xuat_SP()
        {
            string sql = "exec XuatSP";
            SqlDataAdapter da_Xuat_SP = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_SP = new DataTable();
            da_Xuat_SP.Fill(dt_Xuat_SP);
            dataGridView_SP.DataSource = dt_Xuat_SP;
        }
        public void LoadDataGridView_Xuat_SP_LoaiSP()
        {
            string sql = "exec XuatSP_LoaiSP N'" + cbo_TimLoaiSP.SelectedValue.ToString() + "'";
            SqlDataAdapter da_Xuat_LoaiSP = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_LoaiSP = new DataTable();
            da_Xuat_LoaiSP.Fill(dt_Xuat_LoaiSP);
            dataGridView_SP.DataSource = dt_Xuat_LoaiSP;
        }
        public void LoadDataGridView_Xuat_SP_TenSP()
        {
            string sql = "exec XuatSP_TenSP N'" + txt_TimTenSP.Text + "'";
            SqlDataAdapter da_Xuat_TenSP = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_TenSP = new DataTable();
            da_Xuat_TenSP.Fill(dt_Xuat_TenSP);
            dataGridView_SP.DataSource = dt_Xuat_TenSP;
        }
        public void LoadDataGridView_Xuat_SP_GiaTang()
        {
            string sql = "exec XuatSP_GiaTang";
            SqlDataAdapter da_Xuat_GiaTang = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_GiaTang = new DataTable();
            da_Xuat_GiaTang.Fill(dt_Xuat_GiaTang);
            dataGridView_SP.DataSource = dt_Xuat_GiaTang;
        }
        public void LoadDataGridView_Xuat_SP_GiaGiam()
        {
            string sql = "exec XuatSP_GiaGiam";
            SqlDataAdapter da_Xuat_GiaGiam = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_GiaGiam = new DataTable();
            da_Xuat_GiaGiam.Fill(dt_Xuat_GiaGiam);
            dataGridView_SP.DataSource = dt_Xuat_GiaGiam;
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

        private void btn_Them_Click(object sender, EventArgs e)
        {
            txt_TenSP.Enabled = true;
            txt_DV.Enabled = true;
            txt_Gia.Enabled = true;
            txt_MaSP.Enabled = true;
            btn_Luu.Enabled = true;
            btn_TaiAnh.Enabled = true;
            cbo_LoaiSP.Enabled = true;
            txt_MaSP.Clear();
            txt_TenSP.Clear();
            txt_Gia.Clear();
            txt_DV.Clear();
            txt_MaSP.Focus();
        }
        public void checkradio_TimKiem()
        {
            if (rdo_TimTenSP.Checked || rdo_LoaiSP.Checked || rdo_TimGia.Checked)
            {
                btn_TimKiem.Enabled = true;
                if (rdo_TimTenSP.Checked)
                {
                    txt_TimTenSP.Enabled = true;
                    txt_TimTenSP.Focus();
                }
                else
                {
                    txt_TimTenSP.Enabled = false;
                    txt_TimTenSP.Clear();
                }
                if (rdo_LoaiSP.Checked)
                    cbo_TimLoaiSP.Enabled = true;
                else
                    cbo_TimLoaiSP.Enabled = false;
                if (rdo_TimGia.Checked)
                    cbo_TimGia.Enabled = true;
                else
                    cbo_TimGia.Enabled = false;
            }
            else
                btn_TimKiem.Enabled = false;
        }
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void rdo_TimTenSP_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimGia_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_LoaiSP_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            if (rdo_TimTenSP.Checked)
                LoadDataGridView_Xuat_SP_TenSP();
            else
                if (rdo_TimGia.Checked)
                {
                    if (cbo_TimGia.SelectedItem == "Tăng dần")
                        LoadDataGridView_Xuat_SP_GiaTang();
                    else
                        LoadDataGridView_Xuat_SP_GiaGiam();
                }
                else
                    LoadDataGridView_Xuat_SP_LoaiSP();
        }

        private void txt_Gia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void dataGridView_SP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_SP.Rows[e.RowIndex];
                txt_MaSP.Text = row.Cells[0].Value.ToString();
                txt_TenSP.Text = row.Cells[1].Value.ToString();
                txt_Gia.Text = row.Cells[3].Value.ToString();
                txt_DV.Text = row.Cells[4].Value.ToString();
                string Anh = SelectAnh(row.Cells[0].Value.ToString());
                if(Anh !="None")
                    pictureBox1.ImageLocation = Anh;
                pictureBox1.Image = QL_QuanCaPhe.Properties.Resources.Default;
                int index = cbo_LoaiSP.FindString(row.Cells[2].Value.ToString());
                cbo_LoaiSP.SelectedIndex = index;
                btn_Sua.Enabled = true;
                btn_Xoa.Enabled = true;
                btn_TaiAnh.Enabled = true;
            }
        }

        private void btn_TaiAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string fileName = open.FileName;

                pictureBox1.Image = new Bitmap(fileName);

                Directory.CreateDirectory(@"~/Image/Download");
                string path = @"~/Image/Download/" + Path.GetFileNameWithoutExtension(open.FileName) + ".jpeg";
                pictureBox1.Image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                pictureBox1.ImageLocation = path;

            } 
        }
        DataColumn[] prikey = new DataColumn[1];
        SqlDataAdapter da_SP;
        public void create_SP()
        {
            string sql = "select * from SanPham";
            da_SP = new SqlDataAdapter(sql, db.Conn);
            da_SP.Fill(db.DS, "SP");
            prikey[0] = db.DS.Tables["SP"].Columns["MaSP"];
            db.DS.Tables["SP"].PrimaryKey = prikey;
        }
        private void btn_Sua_Click(object sender, EventArgs e)
        {
            string MaSP = txt_MaSP.Text.Trim();
            string TenSP = txt_TenSP.Text.Trim();
            string LoaiSP = cbo_LoaiSP.SelectedValue.ToString();
            string Gia = txt_Gia.Text.Trim();
            string dv = txt_DV.Text.Trim();
            string Hinh = pictureBox1.ImageLocation;
            if (MaSP == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã sản phẩm !");
                txt_MaSP.Focus();
                return;
            }
            if (TenSP == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên sản phẩm !");
                txt_TenSP.Focus();
                return;
            }
            if (Gia == string.Empty)
            {
                MessageBox.Show("Chưa nhập giá sản phẩm !");
                txt_Gia.Focus();
                return;
            }
            if (dv == string.Empty)
            {
                MessageBox.Show("Chưa nhập đơn vị sản phẩm !");
                txt_DV.Focus();
                return;
            }
            DialogResult edit;
            edit = MessageBox.Show("Bạn có chắc muốn sửa ?", "Sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (edit == DialogResult.Yes)
            {
                try
                {
                    DataRow dr = db.DS.Tables["SP"].Rows.Find(MaSP);
                    if (dr != null)
                    {
                        dr["MaSP"] = MaSP;
                        dr["TenSP"] = TenSP;
                        dr["MaLoaiSP"] = LoaiSP;
                        dr["Gia"] = Gia;
                        dr["DonViTinh"] = dv;
                        dr["LinkHinh"] = Hinh;
                    }

                    SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_SP);
                    da_SP.Update(db.DS, "SP");
                    MessageBox.Show("Sửa thành công !");
                    LoadDataGridView_Xuat_SP();
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công !");
                }
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string MaSP = txt_MaSP.Text.Trim();
            if (MaSP == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã sản phẩm !");
                txt_MaSP.Focus();
                return;
            }
            DialogResult del;
            del = MessageBox.Show("Bạn có chắc muốn xóa ?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (del == DialogResult.Yes)
            {
                try
                {
                    DataRow dr = db.DS.Tables["SP"].Rows.Find(MaSP);
                    if (dr != null)
                        dr.Delete();
                    SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_SP);
                    da_SP.Update(db.DS, "SP");
                    MessageBox.Show("Xóa thành công !");
                    LoadDataGridView_Xuat_SP();
                }
                catch
                {
                    MessageBox.Show("Xóa không thành công !");
                }
            }
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string MaSP = txt_MaSP.Text.Trim();
            string TenSP = txt_TenSP.Text.Trim();
            string LoaiSP = cbo_LoaiSP.SelectedValue.ToString();
            string Gia = txt_Gia.Text.Trim();
            string dv = txt_DV.Text.Trim();
            string Hinh = pictureBox1.ImageLocation;
            if (pictureBox1.ImageLocation == null)
                Hinh = "None";
            if(MaSP==string.Empty)
            {
                MessageBox.Show("Chưa nhập mã sản phẩm !");
                txt_MaSP.Focus();
                return;
            }
            if (TenSP == string.Empty)
            {
                MessageBox.Show("Chưa nhập tên sản phẩm !");
                txt_TenSP.Focus();
                return;
            }
            if (Gia == string.Empty)
            {
                MessageBox.Show("Chưa nhập giá sản phẩm !");
                txt_Gia.Focus();
                return;
            }
            if (dv == string.Empty)
            {
                MessageBox.Show("Chưa nhập đơn vị sản phẩm !");
                txt_DV.Focus();
                return;
            }
            try
            {
                DataRow dr = db.DS.Tables["SP"].Rows.Find(MaSP);
                if (dr != null)
                {
                    MessageBox.Show("Đã tồn tại sản phẩm này !");
                    return;
                }
                DataRow newrow = db.DS.Tables["SP"].NewRow();
                newrow["MaSP"] = MaSP;
                newrow["TenSP"] = TenSP;
                newrow["MaLoaiSP"] = LoaiSP;
                newrow["Gia"] = Gia;
                newrow["DonViTinh"] = dv;
                newrow["LinkHinh"] = Hinh;
                db.DS.Tables["SP"].Rows.Add(newrow);

                SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_SP);
                da_SP.Update(db.DS, "SP");
                MessageBox.Show("Lưu thành công !");
                LoadDataGridView_Xuat_SP();
            }
            catch
            {
                MessageBox.Show("Lưu không thành công !");
            }
        }
    }
}
