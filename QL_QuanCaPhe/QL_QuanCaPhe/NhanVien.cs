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
    public partial class NhanVien : Form
    {
        public NhanVien()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        SqlDataAdapter da_NV;
        DataColumn[] prikey = new DataColumn[1];
        string TaiKhoan;
        public string TK
        {
            get { return TaiKhoan; }
            set { TaiKhoan = value; }
        }
        public void reset()
        {
            txt_MaNV.Enabled = false;
            txt_DC.Enabled = false;
            txt_HT.Enabled = false;
            txt_QQ.Enabled = false;
            txt_SDT.Enabled = false;
            txt_SoCCCD.Enabled = false;
            rdo_Nam.Enabled = false;
            rdo_Nu.Enabled = false;
            btn_Luu.Enabled = false;
            btn_Sua.Enabled = false;
            btn_Xoa.Enabled = false;
            btn_TimKiem.Enabled = false;
            txt_TimHoTen.Enabled = false;
            txt_TimSoCCCD.Enabled = false;
            btn_TaiAnh.Enabled = false;
            cbo_LoaiNV.Enabled = false;
            cbo_TimLoaiNV.Enabled = false;
            maskedTextBox_NS.Enabled = false;
            maskedTextBox_NVL.Enabled = false;
            rdo_Nam.Checked = false;
            rdo_Nu.Checked = false;
            rdo_TimCCCD.Checked = false;
            rdo_TimHT.Checked = false;
            rdo_TimLoaiNV.Checked = false;
            rdo_TimNam.Checked = false;
            rdo_TimNu.Checked = false;
            rdo_TimSDT.Checked = false;
            txt_ChucVu.Enabled = false;
            LoadDataGridView_Xuat_NV();
            txt_TimSDT.Enabled = false;
            maskedTextBox_NS.Clear();
            maskedTextBox_NVL.Clear();
            txt_TimSoCCCD.Clear();
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            txt_MaNV.Enabled = true;
            txt_DC.Enabled = true;
            txt_HT.Enabled = true;
            txt_QQ.Enabled = true;
            txt_SDT.Enabled = true;
            txt_SoCCCD.Enabled = true;
            rdo_Nam.Enabled = true;
            rdo_Nu.Enabled = true;
            btn_Luu.Enabled = true;
            cbo_LoaiNV.Enabled = true;
            maskedTextBox_NS.Enabled = true;
            maskedTextBox_NVL.Enabled = true;
            btn_TaiAnh.Enabled = true;
            txt_SoCCCD.Clear();
            txt_MaNV.Clear();
            txt_HT.Clear();
            txt_DC.Clear();
            txt_QQ.Clear();
            txt_SDT.Clear();
            txt_ChucVu.Clear();
            txt_ChucVu.Enabled = true;
            txt_MaNV.Focus();
            maskedTextBox_NS.Clear();
            maskedTextBox_NVL.Clear();
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            reset();
        }
        public void create_NV()
        {
            string sql = "select * from NhanVien";
            da_NV = new SqlDataAdapter(sql, db.Conn);
            da_NV.Fill(db.DS, "NV");
            prikey[0] = db.DS.Tables["NV"].Columns["MaNV"];
            db.DS.Tables["NV"].PrimaryKey = prikey;
        }
        private void NhanVien_Load(object sender, EventArgs e)
        {
            db.OpenDB();
            toolStripStatusLabel_TK.Text = "Tài khoản: "+TK;
            reset();
            LoadDataGridView_Xuat_NV();
            Loadcbo_LoaiNV();
            Loadcbo_TimLoaiNV();
            create_NV();
        }
        public void LoadDataGridView_Xuat_NV()
        {
            string sql = "exec XuatNV";
            SqlDataAdapter da_Xuat_NV = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV = new DataTable();
            da_Xuat_NV.Fill(dt_Xuat_NV);
            dataGridView_NV.DataSource = dt_Xuat_NV;
        }
        public void Loadcbo_LoaiNV()
        {
            string sql = "select * from LoaiNV";
            cbo_LoaiNV.DataSource = db.getDataTable(sql, "LoaiNV");
            cbo_LoaiNV.DisplayMember = "TenLoaiNV";
            cbo_LoaiNV.ValueMember = "MaLoaiNV";
            cbo_LoaiNV.SelectedIndex = 0;
        }
        public void Loadcbo_TimLoaiNV()
        {
            string sql = "select * from LoaiNV";
            cbo_TimLoaiNV.DataSource = db.getDataTable(sql, "TimLoaiNV");
            cbo_TimLoaiNV.DisplayMember = "TenLoaiNV";
            cbo_TimLoaiNV.ValueMember = "MaLoaiNV";
            cbo_TimLoaiNV.SelectedIndex = 0;
        }
        public void LoadDataGridView_Xuat_NV_GT()
        {
            string gt = string.Empty;
            if (rdo_TimNam.Checked)
                gt = "Nam";
            if (rdo_TimNu.Checked)
                gt = "Nữ";
            string sql = "exec XuatNV_GT N'" + gt + "'";
            SqlDataAdapter da_Xuat_NV_GT = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV_GT = new DataTable();
            da_Xuat_NV_GT.Fill(dt_Xuat_NV_GT);
            dataGridView_NV.DataSource = dt_Xuat_NV_GT;
        }
        public void LoadDataGridView_Xuat_NV_SDT()
        {
            string sql = "exec XuatNV_SDT '"+txt_TimSDT.Text+"'";
            SqlDataAdapter da_Xuat_NV_SDT = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV_SDT = new DataTable();
            da_Xuat_NV_SDT.Fill(dt_Xuat_NV_SDT);
            dataGridView_NV.DataSource = dt_Xuat_NV_SDT;
        }
        public void LoadDataGridView_Xuat_NV_SoCCCD()
        {
            string sql = "exec XuatNV_SoCCCD '"+txt_TimSoCCCD.Text+"'";
            SqlDataAdapter da_Xuat_NV_SoCCCD = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV_SoCCCD = new DataTable();
            da_Xuat_NV_SoCCCD.Fill(dt_Xuat_NV_SoCCCD);
            dataGridView_NV.DataSource = dt_Xuat_NV_SoCCCD;
        }
        public void LoadDataGridView_Xuat_NV_HoTen()
        {
            string ht = txt_TimHoTen.Text.Trim();
            string sql = "exec XuatNV_HoTen N'"+ht+"'";
            SqlDataAdapter da_Xuat_NV_HoTen = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV_HoTen = new DataTable();
            da_Xuat_NV_HoTen.Fill(dt_Xuat_NV_HoTen);
            dataGridView_NV.DataSource = dt_Xuat_NV_HoTen;
        }
        public void LoadDataGridView_Xuat_NV_LoaiNV()
        {
            string sql = "exec XuatNV_LoaiNV '"+cbo_TimLoaiNV.SelectedValue.ToString()+"'";
            SqlDataAdapter da_Xuat_NV_LoaiNV = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_NV_LoaiNV = new DataTable();
            da_Xuat_NV_LoaiNV.Fill(dt_Xuat_NV_LoaiNV);
            dataGridView_NV.DataSource = dt_Xuat_NV_LoaiNV;
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
        public void checkradio_TimKiem()
        {
            if (rdo_TimCCCD.Checked || rdo_TimHT.Checked || rdo_TimLoaiNV.Checked || rdo_TimNam.Checked || rdo_TimNu.Checked || rdo_TimSDT.Checked)
            {
                btn_TimKiem.Enabled = true;
                if (rdo_TimCCCD.Checked)
                {
                    txt_TimSoCCCD.Enabled = true;
                    txt_TimSoCCCD.Focus();
                }
                else
                {
                    txt_TimSoCCCD.Enabled = false;
                    txt_TimSoCCCD.Clear();
                }
                if (rdo_TimHT.Checked)
                {
                    txt_TimHoTen.Enabled = true;
                    txt_TimHoTen.Focus();
                }
                else
                {
                    txt_TimHoTen.Enabled = false;
                    txt_TimHoTen.Clear();
                }
                if (rdo_TimLoaiNV.Checked)
                    cbo_TimLoaiNV.Enabled = true;
                else
                    cbo_TimLoaiNV.Enabled = false;
                if (rdo_TimSDT.Checked)
                {
                    txt_TimSDT.Enabled = true;
                    txt_TimSDT.Focus();
                }
                else
                {
                    txt_TimSDT.Enabled = false;
                    txt_TimSDT.Clear();
                }
            }
            else
                btn_TimKiem.Enabled = false;
        }

        private void rdo_TimCCCD_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimHT_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimLoaiNV_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimNam_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void rdo_TimNu_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            if (rdo_TimCCCD.Checked)
                LoadDataGridView_Xuat_NV_SoCCCD();
            else
                if (rdo_TimHT.Checked)
                    LoadDataGridView_Xuat_NV_HoTen();
                else
                    if (rdo_TimSDT.Checked)
                        LoadDataGridView_Xuat_NV_SDT();
                    else
                        if (rdo_TimLoaiNV.Checked)
                            LoadDataGridView_Xuat_NV_LoaiNV();
                        else
                            LoadDataGridView_Xuat_NV_GT();
        }

        private void txt_SoCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txt_TimSoCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txt_SDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void dataGridView_NV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_NV.Rows[e.RowIndex];
                txt_MaNV.Text = row.Cells[0].Value.ToString();
                txt_HT.Text = row.Cells[1].Value.ToString();
                txt_ChucVu.Text = row.Cells[2].Value.ToString();
                if (row.Cells[3].Value.ToString() == "Nam")
                    rdo_Nam.Checked = true;
                else
                    rdo_Nam.Checked = false;
                if (row.Cells[3].Value.ToString() == "Nữ")
                    rdo_Nu.Checked = true;
                else
                    rdo_Nu.Checked = false;
                txt_DC.Text = row.Cells[4].Value.ToString();
                txt_SDT.Text = row.Cells[5].Value.ToString();
                txt_QQ.Text = row.Cells[6].Value.ToString();
                txt_SoCCCD.Text = row.Cells[7].Value.ToString();
                maskedTextBox_NS.Text = row.Cells[8].Value.ToString();
                maskedTextBox_NVL.Text = row.Cells[10].Value.ToString();
                int index = cbo_LoaiNV.FindString(row.Cells[9].Value.ToString());
                cbo_LoaiNV.SelectedIndex = index;
                string Anh = SelectAnh(row.Cells[0].Value.ToString());
                if (Anh != "None")
                    pictureBox1.ImageLocation = Anh;
                pictureBox1.Image = QL_QuanCaPhe.Properties.Resources.Default;
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
        public string SelectAnh(string MaNV)
        {
            db.OpenDB();
            string sql = "select LinkHinh from NhanVien where MaNV = '" + MaNV + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string AnhSP = (string)cmd.ExecuteScalar();
            db.CloseDB();
            return AnhSP;
        }
        private void btn_Sua_Click(object sender, EventArgs e)
        {
            string MaNV = txt_MaNV.Text.Trim();
            string HoTen = txt_HT.Text.Trim();
            string ChucVu = txt_ChucVu.Text.Trim();
            string GT = string.Empty;
            if (rdo_Nam.Checked)
                GT = "Nam";
            if (rdo_Nu.Checked)
                GT = "Nữ";
            string DiaChi = txt_DC.Text.Trim();
            string SDT = txt_SDT.Text.Trim();
            string QQ = txt_QQ.Text.Trim();
            string SoCCCD = txt_SoCCCD.Text.Trim();
            string MaLoaiNV = cbo_LoaiNV.SelectedValue.ToString();
            string NgaySinh = maskedTextBox_NS.Text;
            string NgayVaoLam = maskedTextBox_NVL.Text;
            string Hinh = pictureBox1.ImageLocation;
            if (MaNV == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã nhân viên !");
                txt_MaNV.Focus();
                return;
            }
            if (HoTen == string.Empty)
            {
                MessageBox.Show("Chưa nhập họ tên !");
                txt_HT.Focus();
                return;
            }
            if (DiaChi == string.Empty)
            {
                MessageBox.Show("Chưa nhập địa chỉ !");
                txt_DC.Focus();
                return;
            }
            if (SDT == string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại !");
                txt_SDT.Focus();
                return;
            }
            if (QQ == string.Empty)
            {
                MessageBox.Show("Chưa nhập quê quán !");
                txt_QQ.Focus();
                return;
            }
            if (SoCCCD == string.Empty)
            {
                MessageBox.Show("Chưa nhập số CCCD !");
                txt_SoCCCD.Focus();
                return;
            }
            if (NgaySinh == string.Empty)
            {
                MessageBox.Show("Chưa nhập ngày sinh !");
                maskedTextBox_NS.Focus();
                return;
            }
            if (NgayVaoLam == string.Empty)
            {
                MessageBox.Show("Chưa nhập ngày vào làm !");
                maskedTextBox_NVL.Focus();
                return;
            }
            DialogResult edit;
            edit = MessageBox.Show("Bạn có chắc muốn sửa ?", "Sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (edit == DialogResult.Yes)
            {
                try
                {
                    DataRow dr = db.DS.Tables["NV"].Rows.Find(MaNV);
                    if (dr != null)
                    {
                        dr["MaNV"] =MaNV;
                        dr["HoTenNV"] = HoTen;
                        dr["ChucVu"] = ChucVu;
                        dr["GioiTinh"] = GT;
                        dr["DiaChi"] = DiaChi;
                        dr["SoDienThoai"] = SDT;
                        dr["QueQuan"] = QQ;
                        dr["SoCCCD"] = SoCCCD;
                        dr["NgaySinh"] = NgaySinh;
                        dr["MaLoaiNV"] = MaLoaiNV;
                        dr["NgayVaoLam"] = NgayVaoLam;
                        dr["LinkHinh"] = Hinh;
                    }

                    SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_NV);
                    da_NV.Update(db.DS, "NV");
                    MessageBox.Show("Sửa thành công !");
                    LoadDataGridView_Xuat_NV();
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công !");
                }
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string MaNV = txt_MaNV.Text.Trim();
            if (MaNV == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã nhân viên !");
                txt_MaNV.Focus();
                return;
            }
            DialogResult del;
            del = MessageBox.Show("Bạn có chắc muốn xóa ?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (del == DialogResult.Yes)
            {
                if (XuatQuyen_TK(TK) == "Admin")
                {
                    try
                    {
                        DataRow dr = db.DS.Tables["NV"].Rows.Find(MaNV);
                        if (dr != null)
                            dr.Delete();
                        SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_NV);
                        da_NV.Update(db.DS, "NV");
                        MessageBox.Show("Xóa thành công !");
                        LoadDataGridView_Xuat_NV();
                    }
                    catch
                    {
                        MessageBox.Show("Xóa không thành công !");
                    }
                }
                else
                    MessageBox.Show("Không phải Admin không thể xóa !");
            }
        }
        public string XuatQuyen_TK(string TenTK)
        {
            db.OpenDB();
            string sql = "select Quyen from TaiKhoan where TenTK = '" + TenTK + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            db.CloseDB();
            return kq;
        }
        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string MaNV = txt_MaNV.Text.Trim();
            string HoTen = txt_HT.Text.Trim();
            string ChucVu = txt_ChucVu.Text.Trim();
            string GT = string.Empty;
            if (rdo_Nam.Checked)
                GT = "Nam";
            if (rdo_Nu.Checked)
                GT = "Nữ";
            string DiaChi = txt_DC.Text.Trim();
            string SDT = txt_SDT.Text.Trim();
            string QQ = txt_QQ.Text.Trim();
            string SoCCCD = txt_SoCCCD.Text.Trim();
            string MaLoaiNV = cbo_LoaiNV.SelectedValue.ToString();
            string NgaySinh = maskedTextBox_NS.Text;
            string NgayVaoLam = maskedTextBox_NVL.Text;
            string Hinh = pictureBox1.ImageLocation;
            if (pictureBox1.ImageLocation == null)
                Hinh = "None";
            if (MaNV == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã nhân viên !");
                txt_MaNV.Focus();
                return;
            }
            if (HoTen == string.Empty)
            {
                MessageBox.Show("Chưa nhập họ tên !");
                txt_HT.Focus();
                return;
            }
            if (DiaChi == string.Empty)
            {
                MessageBox.Show("Chưa nhập địa chỉ !");
                txt_DC.Focus();
                return;
            }
            if (SDT == string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại !");
                txt_SDT.Focus();
                return;
            }
            if (QQ == string.Empty)
            {
                MessageBox.Show("Chưa nhập quê quán !");
                txt_QQ.Focus();
                return;
            }
            if (SoCCCD == string.Empty)
            {
                MessageBox.Show("Chưa nhập số CCCD !");
                txt_SoCCCD.Focus();
                return;
            }
            if (NgaySinh == string.Empty)
            {
                MessageBox.Show("Chưa nhập ngày sinh !");
                maskedTextBox_NS.Focus();
                return;
            }
            try
            {
                DataRow dr = db.DS.Tables["NV"].Rows.Find(MaNV);
                if (dr != null)
                {
                    MessageBox.Show("Đã tồn tại nhân viên này !");
                    return;
                }
                DataRow newrow = db.DS.Tables["NV"].NewRow();
                newrow["MaNV"] = MaNV;
                newrow["HoTenNV"] = HoTen;
                newrow["ChucVu"] = ChucVu;
                newrow["GioiTinh"] = GT;
                newrow["DiaChi"] = DiaChi;
                newrow["SoDienThoai"] = SDT;
                newrow["QueQuan"] = QQ;
                newrow["SoCCCD"] = SoCCCD;
                newrow["NgaySinh"] = NgaySinh;
                newrow["MaLoaiNV"] = MaLoaiNV;
                newrow["NgayVaoLam"] = NgayVaoLam;
                newrow["LinkHinh"] = Hinh;
                db.DS.Tables["NV"].Rows.Add(newrow);

                SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_NV);
                da_NV.Update(db.DS, "NV");
                MessageBox.Show("Lưu thành công !");
                LoadDataGridView_Xuat_NV();
            }
            catch
            {
                MessageBox.Show("Lưu không thành công !");
            }
        }

        private void rdo_TimSDT_CheckedChanged(object sender, EventArgs e)
        {
            checkradio_TimKiem();
        }

        private void txt_TimSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void maskedTextBox_NS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void maskedTextBox_NVL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

    }
}
