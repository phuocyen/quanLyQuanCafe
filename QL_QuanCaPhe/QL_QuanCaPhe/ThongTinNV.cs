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
    public partial class ThongTinNV : Form
    {
        public ThongTinNV()
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
        private void btn_QuayVe_Click(object sender, EventArgs e)
        {
            this.Hide();
            Order frmOrder = new Order();
            frmOrder.TK = TK;
            frmOrder.ShowDialog();
            db.CloseDB();
            this.Close();
        }
        public string XuatMaNV(string TenTK)
        {
            string sql = "select MANV from TaiKhoan where TenTK ='" + TenTK + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            return kq;
        }
        public string XuatHoTenNV(string MaNV)
        {
            string sql = "select HoTenNV from NhanVien where MaNV = '" + MaNV + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            return kq;
        }
        public string XuatGT(string MaNV)
        {
            string sql = "select GioiTinh from NhanVien where MaNV = '" + MaNV + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            return kq;
        }
        public string XuatSDT(string MaNV)
        {
            string sql = "select SoDienThoai from NhanVien where MaNV = '" + MaNV + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            return kq;
        }
        public string XuatQQ(string MaNV)
        {
            string sql = "select QueQuan from NhanVien where MaNV = '" + MaNV + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            return kq;
        }
        public string XuatCV(string MaNV)
        {
            string sql = "select ChucVu from NhanVien where MaNV = '" + MaNV + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            return kq;
        }
        private void ThongTinNV_Load(object sender, EventArgs e)
        {
            db.OpenDB();
            string MaNV = XuatMaNV(TK);
            lbl_MaNV.Text = MaNV;
            lbl_HoTen.Text = XuatHoTenNV(MaNV);
            lbl_GT.Text = XuatGT(MaNV);
            lbl_CV.Text = XuatCV(MaNV);
            lbl_QQ.Text = XuatQQ(MaNV);
            lbl_SDT.Text = XuatSDT(MaNV);
            string Anh = SelectAnh(MaNV);
            if (Anh != "None")
                pictureBox_anh.ImageLocation = Anh;
            pictureBox_anh.Image = QL_QuanCaPhe.Properties.Resources.Default;
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
    }
}
