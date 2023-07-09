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
    public partial class XuatHD : Form
    {
        public XuatHD()
        {
            InitializeComponent();
        }
        string SoDienThoaiKhach, TenTK, ThoiGian, MaHD;

        public string MAHD
        {
            get { return MaHD; }
            set { MaHD = value; }
        }

        public string TG
        {
            get { return ThoiGian; }
            set { ThoiGian = value; }
        }

        public string TK
        {
            get { return TenTK; }
            set { TenTK = value; }
        }

        public string SDTKH
        {
            get { return SoDienThoaiKhach; }
            set { SoDienThoaiKhach = value; }
        }
        ConnectDB db = new ConnectDB();
        public void LoadDataGridView_Xuat_SP()
        {
            string sql = "exec InHoaDon '"+MAHD+"'";
            SqlDataAdapter da_Xuat_SP = new SqlDataAdapter(sql, db.Conn);
            DataTable dt_Xuat_SP = new DataTable();
            da_Xuat_SP.Fill(dt_Xuat_SP);
            dataGridView_Mon.DataSource = dt_Xuat_SP;
        }
        public string XuatTenKH(string SDT)
        {
            db.OpenDB();
            string sql = "select HoTen FROM KhachHang where SoDienThoai = '" + SDT + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            db.CloseDB();
            return kq;
        }
        public int XuatSoLuongSP(string MaHD)
        {
            db.OpenDB();
            string sql = "select sum(SoLuong) from ChiTietHD where MaCTHD = '"+MaHD+"'";
            int kq = db.get_ExcuteScalarQuery(sql);
            db.CloseDB();
            return kq;
        }
        public int XuatTienThanhToan(string MaHD)
        {
            db.OpenDB();
            string sql = "select ThanhToan from HoaDon where MaHD ='"+MaHD+"'";
            int kq = db.get_ExcuteScalarQuery(sql);
            db.CloseDB();
            return kq;
        }
        private void XuatHD_Load(object sender, EventArgs e)
        {
            lbl_MaHD.Text = MAHD;
            lbl_TenTK.Text = TK;
            lbl_TienThanhToan.Text = XuatTienThanhToan(MAHD).ToString();
            lbl_TongSL.Text = XuatSoLuongSP(MAHD).ToString();
            lbl_TG.Text = TG;
            if (SDTKH == null)
                lbl_KH.Text = "Vãng lai";
            else
                lbl_KH.Text = XuatTenKH(SDTKH);
            LoadDataGridView_Xuat_SP();
        }
    }
}
