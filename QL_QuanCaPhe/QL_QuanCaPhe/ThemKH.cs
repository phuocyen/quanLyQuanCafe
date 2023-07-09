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
    public partial class ThemKH : Form
    {
        public ThemKH()
        {
            InitializeComponent();
        }
        ConnectDB db = new ConnectDB();
        string SDTKhach;

        public string SDTKHACH
        {
            get { return SDTKhach; }
            set { SDTKhach = value; }
        }
        public int TimKH(string SDT)
        {
            db.OpenDB();
            string sql = "select count(*) from KhachHang where SoDienThoai = '" + SDT + "'";
            int kq = db.get_ExcuteScalarQuery(sql);
            return kq;
            db.CloseDB();
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            string SDT = txt_SDT.Text, HoTen = txt_HT.Text, GT = string.Empty, DiaChi=txt_DC.Text;
            if(SDT==string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại");
                return;
            }
            if(HoTen==string.Empty)
            {
                MessageBox.Show("Chưa nhập họ tên");
                return;
            }
            if(rdo_Nam.Checked)
                GT = "Nam";
            if(rdo_Nu.Checked)
                GT = "Nữ";
            if (rdo_Nam.Checked == false && rdo_Nu.Checked == false)
                GT = null;
            if(DiaChi==string.Empty)
                DiaChi = null;
            DialogResult add;
            add = MessageBox.Show("Bạn có chắc muốn thêm ?", "Thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (add == DialogResult.Yes)
            {
                try
                {
                    if (TimKH(SDT) == 0)
                    {
                        db.OpenDB();
                        string sql = "insert into KhachHang values ('" + SDT + "',N'" + HoTen + "',N'" + GT + "',N'" + DiaChi + "')";
                        int kq = db.get_ExcuteNoneQuery(sql);
                        if (kq == 1)
                            MessageBox.Show("Thêm thành công !");
                        else
                            MessageBox.Show("Thêm không thành công !");
                        db.CloseDB();
                    }
                    else
                        MessageBox.Show("Khách hàng này đã có chỉ nên sửa thông tin !");
                }
                catch
                {
                    MessageBox.Show("Thêm không thành công");
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

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            string SDT = txt_SDT.Text, HoTen = txt_HT.Text, GT = string.Empty, DiaChi = txt_DC.Text;
            if (SDT == string.Empty)
            {
                MessageBox.Show("Chưa nhập số điện thoại");
                return;
            }
            if (HoTen == string.Empty)
            {
                MessageBox.Show("Chưa nhập họ tên");
                return;
            }
            if (rdo_Nam.Checked)
                GT = "Nam";
            if (rdo_Nu.Checked)
                GT = "Nữ";
            if (rdo_Nam.Checked == false && rdo_Nu.Checked == false)
                GT = null;
            if (DiaChi == string.Empty)
                DiaChi = null;
            DialogResult add;
            add = MessageBox.Show("Bạn có chắc muốn Sửa ?", "Thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (add == DialogResult.Yes)
            {
                try
                {
                    db.OpenDB();
                    string sql = "update KhachHang set SoDienThoai = '" + SDT + "', HoTen = N'" + HoTen + "', GioiTinh = N'" + GT + "', DiaChi = N'" + DiaChi + "' where SoDienThoai = '" + SDT + "'";
                    int kq = db.get_ExcuteNoneQuery(sql);
                    if (kq == 1)
                        MessageBox.Show("Sửa thành công !");
                    else
                        MessageBox.Show("Sửa không thành công !");
                    db.CloseDB();
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công");
                }
            }
        }
        public string XuatTen_KH(string s)
        {
            db.OpenDB();
            string sql = "select HoTen from KhachHang where SoDienThoai = '" + s + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            if (kq == string.Empty)
                kq = null;
            return kq;
            db.CloseDB();
        }
        public string XuatGT_KH(string s)
        {
            db.OpenDB();
            string sql = "select GioiTinh from KhachHang where SoDienThoai = '" + s + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            if (kq == string.Empty)
                kq = null;
            return kq;
            db.CloseDB();
        }
        public string XuatDC_KH(string s)
        {
            db.OpenDB();
            string sql = "select DiaChi from KhachHang where SoDienThoai = '" + s + "'";
            SqlCommand cmd = new SqlCommand(sql, db.Conn);
            string kq = (string)cmd.ExecuteScalar();
            if (kq == string.Empty)
                kq = null;
            return kq;
            db.CloseDB();
        }
        private void ThemKH_Load(object sender, EventArgs e)
        {
            txt_SDT.Text = SDTKHACH;
            txt_HT.Text = XuatTen_KH(SDTKHACH);
            txt_DC.Text = XuatDC_KH(SDTKHACH);
            string GT = XuatGT_KH(SDTKHACH);
            if (GT == "Nam")
                rdo_Nam.Checked = true;
            else
                rdo_Nu.Checked = true;
        }
    }
}
