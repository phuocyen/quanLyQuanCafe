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
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        SqlDataAdapter da_CTHD;
        DataColumn[] prikey = new DataColumn[2];
        string TaiKhoan;
        public string TK
        {
            get { return TaiKhoan; }
            set { TaiKhoan = value; }
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongTinNV frmThongTinNV = new ThongTinNV();
            frmThongTinNV.TK = TK;
            frmThongTinNV.ShowDialog();
            db.CloseDB();
            this.Close();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            DangNhap frmDN = new DangNhap();
            frmDN.ShowDialog();
            db.CloseDB();
            this.Close();
        }

        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoiMatKhau frmDMK = new DoiMatKhau();
            frmDMK.TK = TK;
            frmDMK.ShowDialog();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            KhachHang frmKH = new KhachHang();
            frmKH.TK = TK;
            frmKH.ShowDialog();
            db.CloseDB();
            this.Close();
        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            NhanVien frmNV = new NhanVien();
            frmNV.TK = TK;
            frmNV.ShowDialog();
            db.CloseDB();
            this.Close();
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            SanPham frmSP = new SanPham();
            frmSP.TK = TK;
            frmSP.ShowDialog();
            db.CloseDB();
            this.Close();
        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaiKhoan frmTK = new TaiKhoan();
            frmTK.TK = TK;
            frmTK.ShowDialog();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel_TK.Text = "Tài khoản: " + TK;
            KiemTraThongTinCaNhan(TK);
            KiemTraQuyen(TK);
            LoadDataGridView_Xuat_SP();
            LoadDataGridView_Xuat_CTHD();
            create_CTHD();
        }
        public void KiemTraThongTinCaNhan(string TenTK)
        {
            db.OpenDB();
            string sql = "select count(*) from TaiKhoan where MANV is null and TenTK = '" + TenTK + "'";
            if (db.get_ExcuteScalarQuery(sql) == 1)
                thôngTinCáNhânToolStripMenuItem.Enabled = false;
            else
                thôngTinCáNhânToolStripMenuItem.Enabled = true;
            db.CloseDB();
        }
        public void KiemTraQuyen(string TenTK)
        {
            db.OpenDB();
            string sql = "select Quyen from TaiKhoan where TenTK = '" + TenTK + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            if (kq != "User")
            {
                if (kq == "Admin")
                {
                    kháchHàngToolStripMenuItem.Enabled = true;
                    nhânViênToolStripMenuItem.Enabled = true;
                    sảnPhẩmToolStripMenuItem.Enabled = true;
                    tàiKhoảnToolStripMenuItem.Enabled = true;
                }
                else
                {
                    kháchHàngToolStripMenuItem.Enabled = true;
                    nhânViênToolStripMenuItem.Enabled = true;
                    sảnPhẩmToolStripMenuItem.Enabled = true;
                    tàiKhoảnToolStripMenuItem.Enabled = false;
                }
            }
            else
            {
                kháchHàngToolStripMenuItem.Enabled = false;
                nhânViênToolStripMenuItem.Enabled = false;
                sảnPhẩmToolStripMenuItem.Enabled = false;
                tàiKhoảnToolStripMenuItem.Enabled = false;
            }
            db.CloseDB();
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
        public void LoadDataGridView_Xuat_SP()
        {
            string sql = "exec XuatSP";
            SqlDataAdapter da_Xuat_SP = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_SP = new DataTable();
            da_Xuat_SP.Fill(dt_Xuat_SP);
            dataGridView_SP.DataSource = dt_Xuat_SP;
        }
        public void LoadDataGridView_Xuat_SP_TenSP()
        {
            string sql = "exec XuatSP_TenSP N'" + txt_TimTenSP.Text + "'";
            SqlDataAdapter da_Xuat_TenSP = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_TenSP = new DataTable();
            da_Xuat_TenSP.Fill(dt_Xuat_TenSP);
            dataGridView_SP.DataSource = dt_Xuat_TenSP;
        }
        private void dataGridView_SP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_SP.Rows[e.RowIndex];
                txt_MaSP.Text = row.Cells[0].Value.ToString();
                string Anh = SelectAnh(row.Cells[0].Value.ToString());
                if (Anh != "None")
                    pictureBox1.ImageLocation = Anh;
                pictureBox1.Image = QL_QuanCaPhe.Properties.Resources.Default;
            }
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            if (txt_TimTenSP.Text == null)
                MessageBox.Show("Chưa nhập tên sản phẩm cần tìm !");
            else
                LoadDataGridView_Xuat_SP_TenSP();
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            txt_TimTenSP.Clear();
            txt_SL.Clear();
            txt_MaHD.Clear();
            txt_MaHD.Focus();
            txt_SDTKhach.Clear();
            LoadDataGridView_Xuat_SP();
            LoadDataGridView_Xuat_CTHD();
            rdo_TimMaHD.Checked = false;
        }
        public void TaoHD(string MaHD)
        {
            db.OpenDB();
            string sql = "insert into HoaDon(MaHD) values('" + MaHD + "')";
            if(db.get_ExcuteNoneQuery(sql)==1)
            db.CloseDB();
        }
        public void UpdateHD_CoSDTKH(string SDTKH, string TenTK, string TG, string MaHD)
        {
            db.OpenDB();
            string sql = "set dateformat dmy update HoaDon set SDTKH = '" + SDTKH + "', TenTK_HD = '" + TenTK + "', ThoiGian = '" + TG + "' where MaHD = '" + MaHD + "'";
            if(db.get_ExcuteNoneQuery(sql)==1)
            db.CloseDB();
        }
        public void UpdateHD_KoCoSDTKH(string TenTK, string TG, string MaHD)
        {
            db.OpenDB();
            string sql = "set dateformat dmy update HoaDon set TenTK_HD = '" + TenTK + "', ThoiGian = '" + TG + "' where MaHD = '" + MaHD + "'";
            if (db.get_ExcuteNoneQuery(sql) == 1)
                db.CloseDB();
        }
        public int KT_HD_ThanhToan(string MaHD)
        {
            db.OpenDB();
            string sql = "select count(*) from HoaDon where ThoiGian is not null and MaHD = '" + MaHD + "'";
            int kq = db.get_ExcuteScalarQuery(sql);
            db.CloseDB();
            return kq;
        }
        public int KT_TonTai_HD(string MaHD)
        {
            db.OpenDB();
            string sql = "select count(*) from HoaDon where MaHD = '"+MaHD+"'";
            int kq = db.get_ExcuteScalarQuery(sql);
            db.CloseDB();
            return kq;
        }
        public void TinhThanhToan_HD(string MaHD)
        {
            db.OpenDB();
            string sql = "exec CapNhatThanhToan '"+MaHD+"'";
            if(db.get_ExcuteNoneQuery(sql)==1)
            db.CloseDB();
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            string MaSP = txt_MaSP.Text;
            string MaHD = txt_MaHD.Text.Trim();
            string SL = txt_SL.Text;
            if (MaHD == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã hóa đơn !");
                return;
            }
            if (SL == string.Empty)
            {
                MessageBox.Show("Chưa nhập số lượng !");
                return;
            }
            if (KT_TonTai_HD(MaHD) == 0)
                TaoHD(MaHD);
            try
            {
                
                if (KT_HD_ThanhToan(MaHD) == 0)
                {
                    string[] Primarykey = { MaHD, MaSP };
                    DataRow dr = db.DS.Tables["CTHD"].Rows.Find(Primarykey);

                    if (dr != null)
                    {
                        MessageBox.Show("Đã tồn tại mã hóa đơn " + MaHD + " và mã sản phẩm " + MaSP);
                        return;
                    }
                    DataRow newRow = db.DS.Tables["CTHD"].NewRow();
                    newRow["MaCTHD"] = MaHD;
                    newRow["MaSP_CTHD"] = MaSP;
                    newRow["SoLuong"] = SL;
                    db.DS.Tables["CTHD"].Rows.Add(newRow);

                    SqlCommandBuilder cmdBuil = new SqlCommandBuilder(da_CTHD);
                    da_CTHD.Update(db.DS, "CTHD");
                    LoadDataGridView_Xuat_CTHD();
                    TinhThanhToan_HD(MaHD);
                }
                else
                    MessageBox.Show("Mã hóa đơn này đã thanh toán rồi !");
            }
            catch
            {
                MessageBox.Show("Chọn thất bại !");
            }
        }
        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            DialogResult del;
            del = MessageBox.Show("Bạn có chắc muốn xóa ?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (del == DialogResult.Yes)
            {
                string MaSP = txt_MaSP.Text;
                string MaHD = txt_MaHD.Text.Trim();
                if (MaHD == string.Empty)
                {
                    MessageBox.Show("Chưa nhập mã hóa đơn !");
                    return;
                }
                if (KT_HD_ThanhToan(MaHD) == 0)
                {
                    try
                    {
                    
                            string[] Primarykey = { MaHD, MaSP };
                            DataRow dr = db.DS.Tables["CTHD"].Rows.Find(Primarykey);
                            if (dr != null)
                                dr.Delete();
                            SqlCommandBuilder cmdBuil = new SqlCommandBuilder(da_CTHD);
                            da_CTHD.Update(db.DS, "CTHD");
                            LoadDataGridView_Xuat_CTHD();
                            TinhThanhToan_HD(MaHD);
                    
                    }
                    catch
                    {
                        MessageBox.Show("Xóa thất bại !");
                    }
                }
                else
                    MessageBox.Show("Mã hóa đơn này đã thanh toán rồi !");
            }
        }

        private void txt_SL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void dataGridView_DatMon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_DatMon.Rows[e.RowIndex];
                txt_MaSP.Text = row.Cells[1].Value.ToString();
                txt_MaHD.Text = row.Cells[0].Value.ToString();
                txt_SL.Text = row.Cells[2].Value.ToString();
            }
        }
        public void create_CTHD()
        {
            string sql = "select * from ChiTietHD";
            da_CTHD = new SqlDataAdapter(sql, db.Conn);
            da_CTHD.Fill(db.DS, "CTHD");
            prikey[0] = db.DS.Tables["CTHD"].Columns["MaCTHD"];
            prikey[1] = db.DS.Tables["CTHD"].Columns["MaSP_CTHD"];
            db.DS.Tables["CTHD"].PrimaryKey = prikey;
        }
        public void LoadDataGridView_Xuat_CTHD()
        {
            string sql = "select * from ChiTietHD";
            SqlDataAdapter da_Xuat_CTHD = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_CTHD = new DataTable();
            da_Xuat_CTHD.Fill(dt_Xuat_CTHD);
            dataGridView_DatMon.DataSource = dt_Xuat_CTHD;
        }
        public int TimKH(string SDT)
        {
            db.OpenDB();
            string sql = "select count(*) from KhachHang where SoDienThoai = '" + SDT + "'";
            int kq = db.get_ExcuteScalarQuery(sql);
            return kq;
            db.CloseDB();
        }
        public void XuatHD(string SDTKH, string TenTK, string TG, string MaHD)
        {
            XuatHD frmXuatHD = new XuatHD();
            frmXuatHD.SDTKH = SDTKH;
            frmXuatHD.TK = TenTK;
            frmXuatHD.TG = TG;
            frmXuatHD.MAHD = MaHD;
            frmXuatHD.ShowDialog();
        }
        public string ThoiGian()
        {
            string Giay = DateTime.Now.Second.ToString();
            string Phut = DateTime.Now.Minute.ToString();
            string Gio = DateTime.Now.Hour.ToString();
            string Ngay = DateTime.Now.Day.ToString();
            string Thang = DateTime.Now.Month.ToString();
            string Nam = DateTime.Now.Year.ToString();
            string TG = Ngay + '/' + Thang + '/' + Nam + ' ' + Gio + ':' + Phut + ':' + Giay;
            return TG;
        }
        private void btn_ThanhToan_Click(object sender, EventArgs e)
        {
            string SDT = txt_SDTKhach.Text.Trim();
            string MaHD = txt_MaHD.Text.Trim();
            string TG = ThoiGian();
            if(SDT==string.Empty)
            {
                SDT = null;
            }
            if(TimKH(SDT)==0 && SDT !=null)
            {
                MessageBox.Show("Khách hàng này chưa có (Nên thêm khách hàng) !");
                return;
            }
            if(MaHD==string.Empty)
            {
                MessageBox.Show("CHưa nhập mã hóa đơn !");
                return;
            }
            if (KT_HD_ThanhToan(MaHD) == 0)
            {
                if (SDT != null)
                {
                    UpdateHD_CoSDTKH(SDT, TK, TG, MaHD);
                    XuatHD(SDT, TK, TG, MaHD);
                }
                else
                {
                    UpdateHD_KoCoSDTKH(TK, TG, MaHD);
                    XuatHD(SDT, TK, TG, MaHD);
                }
            }
            else
                MessageBox.Show("Mã hóa đơn này đã thanh toán rồi !");

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ThemKH frmThemKH = new ThemKH();
            frmThemKH.SDTKHACH = txt_SDTKhach.Text;
            frmThemKH.ShowDialog();
        }
        public void LoadDataGridView_Xuat_CTHD_MaHD()
        {
            string MaHD = txt_MaHD.Text.Trim();
            string sql = "select * from ChiTietHD where MaCTHD = '" + MaHD + "'";
            SqlDataAdapter da_Xuat_CTHD_MaHD = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_CTHD_MaHD = new DataTable();
            da_Xuat_CTHD_MaHD.Fill(dt_Xuat_CTHD_MaHD);
            dataGridView_DatMon.DataSource = dt_Xuat_CTHD_MaHD;
        }
        private void rdo_TimMaHD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo_TimMaHD.Checked)
                LoadDataGridView_Xuat_CTHD_MaHD();
            else
                LoadDataGridView_Xuat_CTHD();
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            string MaSP = txt_MaSP.Text;
            string MaHD = txt_MaHD.Text.Trim();
            string SL = txt_SL.Text;
            if (MaHD == string.Empty)
            {
                MessageBox.Show("Chưa nhập mã hóa đơn !");
                return;
            }
            if (SL == string.Empty)
            {
                MessageBox.Show("Chưa nhập số lượng !");
                return;
            }
            if (KT_HD_ThanhToan(MaHD) == 0)
            {
                try
                {
                

                        string[] Primarykey = { MaHD, MaSP };
                        DataRow dr = db.DS.Tables["CTHD"].Rows.Find(Primarykey);
                        if (dr != null)
                        {
                            dr["MaCTHD"] = MaHD;
                            dr["MaSP_CTHD"] = MaSP;
                            dr["SoLuong"] = SL;
                        }

                        SqlCommandBuilder cmdbuil = new SqlCommandBuilder(da_CTHD);
                        da_CTHD.Update(db.DS, "CTHD");
                        LoadDataGridView_Xuat_CTHD();
                        TinhThanhToan_HD(MaHD);
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công !");
                }
            }
            else
                MessageBox.Show("Mã hóa đơn này đã thanh toán rồi !");
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon frmHD = new HoaDon();
            frmHD.TK = TK;
            frmHD.ShowDialog();
            db.CloseDB();
            this.Close();
        }
    }
}
